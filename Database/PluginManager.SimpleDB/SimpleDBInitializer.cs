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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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
	public sealed class SimpleDBInitializer : ISimpleDBInitializer
    {
        public const uint DefaultMinimumVersion = 1;

        private uint _minimumVersion = DefaultMinimumVersion;
        private readonly Dictionary<string, ISimpleDBTable> _tables = new Dictionary<string, ISimpleDBTable>();
        private readonly object _lock = new object();

        public SimpleDBInitializer(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            SimpleDBSettings settings = settingsProvider.GetSettings<SimpleDBSettings>(nameof(SimpleDBSettings));

            if (settings == null)
                throw new InvalidOperationException();

            if (String.IsNullOrEmpty(settings.Path))
                throw new ArgumentNullException(nameof(settings.Path));

            if (!Directory.Exists(settings.Path))
                throw new ArgumentException($"Path does not exist: {settings.Path}", nameof(settings.Path));

            Path = settings.Path;
            EncryptionKey = settings.EnycryptionKey;
        }

        public SimpleDBInitializer(string path)
			: this(path, null)
        {

        }

		public SimpleDBInitializer(string path, string encryptionKey)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException(nameof(path));

			if (!Directory.Exists(path))
				throw new ArgumentException($"Path does not exist: {path}", nameof(path));

			Path = path;
			EncryptionKey = encryptionKey;
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

        public void RegisterTable(ISimpleDBTable textTable)
        {
            if (textTable == null)
                throw new ArgumentNullException(nameof(textTable));

            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                if (String.IsNullOrEmpty(textTable.TableName))
                    throw new ArgumentException("Null table name", nameof(textTable.TableName));

                if (_tables.ContainsKey(textTable.TableName))
                    throw new ArgumentException($"Table {textTable.TableName} already exists");

                _tables.Add(textTable.TableName, textTable);
            }
        }

        public void UnregisterTable(ISimpleDBTable textTable)
        {
            if (textTable == null)
                throw new ArgumentNullException(nameof(textTable));

            using (TimedLock timedLock = TimedLock.Lock(_lock))
            {
                if (string.IsNullOrEmpty(textTable.TableName))
                    return;

                if (!_tables.ContainsKey(textTable.TableName))
                    throw new ArgumentException($"Table {textTable.TableName} already exists");

                _tables.Remove(textTable.TableName);
            }
        }

        public IReadOnlyDictionary<string, ISimpleDBTable> Tables
        {
            get
            {
                return new Dictionary<string, ISimpleDBTable>(_tables);
            }
        }
    }
}

#pragma warning restore CA2208