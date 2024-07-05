/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SimpleDB
 *  
 *  File: SimpleDBInitializer.cs
 *
 *  Purpose:  SimpleDB Initializer for SimpleDB
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using PluginManager.Abstractions;

using Shared.Classes;

#pragma warning disable CA2208

namespace SimpleDB
{
	internal sealed class SimpleDBManager : ThreadManager, ISimpleDBManager
	{
		public const uint DefaultMinimumVersion = 1;
		private const int ThreadRuntime = 500;


		private uint _minimumVersion = DefaultMinimumVersion;
		private readonly Dictionary<string, ISimpleDBTable> _tables = [];
		private readonly Dictionary<ISimpleDBTable, DateTime> _tableLastAction = [];
		private readonly object _lock = new();

		#region Constructors

		private SimpleDBManager()
			: base(null, TimeSpan.FromMilliseconds(ThreadRuntime))
		{
			ContinueIfGlobalException = true;
		}

		public SimpleDBManager(ISettingsProvider settingsProvider)
			: this()
		{
			ContinueIfGlobalException = true;

			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			SimpleDBSettings settings = settingsProvider.GetSettings<SimpleDBSettings>(nameof(SimpleDBSettings));

			if (settings == null || String.IsNullOrEmpty(settings.Path))
				throw new InvalidOperationException();

			if (!Directory.Exists(settings.Path))
				throw new DirectoryNotFoundException($"Path does not exist: {settings.Path}");

			Path = settings.Path;
			EncryptionKey = settings.EnycryptionKey;
		}

		public SimpleDBManager(string path)
			: this(path, null)
		{

		}

		public SimpleDBManager(string path, string encryptionKey)
			: this()
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (!Directory.Exists(path))
				throw new ArgumentException($"Path does not exist: {path}", nameof(path));

			Path = path;
			EncryptionKey = encryptionKey;
		}

		#endregion Constructors

		public void Initialize(IPluginClassesService pluginClassesService)
		{
			foreach (KeyValuePair<string, ISimpleDBTable> table in _tables)
				table.Value.Initialize(pluginClassesService);
		}

		public void ClearMemory()
		{
			foreach (string table in _tables.Keys)
				_tables[table].ClearAllMemory();
		}

		internal string EncryptionKey { get; private set; }


		public string Path { get; private set; }

		public uint MinimumVersion
		{
			get
			{
				return _minimumVersion;
			}

			set
			{
				if (value < DefaultMinimumVersion)
					value = DefaultMinimumVersion;

				_minimumVersion = value;
			}
		}

		public void RegisterTable(ISimpleDBTable simpleDBTable)
		{
			if (simpleDBTable == null)
				throw new ArgumentNullException(nameof(simpleDBTable));

			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				if (String.IsNullOrEmpty(simpleDBTable.TableName))
					throw new InvalidOperationException("Null table name");

				if (_tables.ContainsKey(simpleDBTable.TableName))
					throw new InvalidOperationException($"Table {simpleDBTable.TableName} already exists");

				_tables.Add(simpleDBTable.TableName, simpleDBTable);

				if (simpleDBTable.CachingStrategy == CachingStrategy.SlidingMemory)
				{
					_tableLastAction.Add(simpleDBTable, DateTime.UtcNow);
					simpleDBTable.OnAction += SimpleDBTable_OnAction;

					if (!ThreadManager.Exists(nameof(SimpleDBManager)))
						ThreadManager.ThreadStart(this, nameof(SimpleDBManager), ThreadPriority.Lowest);
				}
			}
		}

		public void UnregisterTable(ISimpleDBTable simpleDBTable)
		{
			if (simpleDBTable == null)
				throw new ArgumentNullException(nameof(simpleDBTable));

			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				if (string.IsNullOrEmpty(simpleDBTable.TableName))
					return;

				if (!_tables.ContainsKey(simpleDBTable.TableName))
					throw new ArgumentException($"Table {simpleDBTable.TableName} already exists");

				_tables.Remove(simpleDBTable.TableName);

				if (_tableLastAction.ContainsKey(simpleDBTable))
				{
					simpleDBTable.OnAction -= SimpleDBTable_OnAction;
					_tableLastAction.Remove(simpleDBTable);
				}
			}
		}

		public IReadOnlyDictionary<string, ISimpleDBTable> Tables
		{
			get
			{
				return new Dictionary<string, ISimpleDBTable>(_tables);
			}
		}

		protected override bool Run(object parameters)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				foreach (ISimpleDBTable simpleDBTable in _tableLastAction.Keys)
				{
					DateTime lastRun = _tableLastAction[simpleDBTable];

					TimeSpan timeFromLastRun = DateTime.UtcNow - lastRun;

					if (timeFromLastRun > simpleDBTable.SlidingMemoryTimeout)
					{
						try
						{
							simpleDBTable.ClearAllMemory();
							_tableLastAction[simpleDBTable] = DateTime.UtcNow;
						}
						catch (LockTimeoutException)
						{
							//ignore specific exception
						}

						OnMemoryCleared?.Invoke(simpleDBTable);
					}
				}
			}

			return !HasCancelled();
		}

		public event SimpleDbEvent OnMemoryCleared;

		#region Private Methods

		private void SimpleDBTable_OnAction(ISimpleDBTable sender)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lock))
			{
				if (!_tableLastAction.ContainsKey(sender))
					throw new InvalidOperationException($"Table {sender.TableName} failed to register sliding memory");

				_tableLastAction[sender] = DateTime.UtcNow;
			}
		}

		#endregion Private Methods
	}
}

#pragma warning restore CA2208