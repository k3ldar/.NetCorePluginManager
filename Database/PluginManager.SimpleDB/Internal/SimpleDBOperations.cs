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
 *  File: SimpleDBOperations.cs
 *
 *  Purpose:  SimpleDBOperations for SimpleDB
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using SimpleDB.Abstractions;
using SimpleDB.Interfaces;

namespace SimpleDB.Internal
{
	/// <summary>
	/// Internal structure for file is:
	/// 
	/// ushort      Internal data version					0
	/// byte[2]     Header									2
	/// long        Primary Sequence						4
	/// long        Secondary Sequence						12
	/// ushort		File Version							20
	/// ushort		Reserved								22
	/// int         Reserved for future use					24
	/// int         Reserved for future use					28
	/// int         Page size								32
	/// byte        Compression								36
	/// int         Record Count							37
	/// int         Length of data before compression		41
	/// int         Length of data stored on disk			45
	/// int			PageCount								49
	/// Data		version specific						53
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal sealed class SimpleDBOperations<T> : ISimpleDBOperations<T>, ISimpleDBTable
		where T : TableRowDefinition
	{
		#region Private Classes

		private sealed class ForeignKeyRelation
		{
			public ForeignKeyRelation(string name, ForeignKeyAttributes foreignKeyAttributes)
			{
				if (String.IsNullOrEmpty(name))
					throw new ArgumentNullException(nameof(name));

				Name = name;
				Attributes = foreignKeyAttributes;
			}

			public string Name { get; }

			public ForeignKeyAttributes Attributes { get; }
		}

		#endregion Private Classes

		#region Private Members

		private const int DefaultSequenceId = -1;
		private const string TimingsSelectAll = "TimingsSelectAll";
		private const string TimingsSelectId = "TimingsSelectId";
		private const string TimingsSelectPredicate = "TimingsSelectPredicate";
		private const string TimingsInsertList = "TimingsInsertList";
		private const string TimingsInsert = "TimingsInsert";
		private const string TimingsDeleteList = "TimingsDeleteList";
		private const string TimingsDelete = "TimingsDelete";
		private const string TimingsTruncate = "TimingsTruncate";
		private const string TimingsUpdateList = "TimingsUpdateList";
		private const string TimingsUpdate = "TimingsUpdate";
		private const string TimingsInsertOrUpdate = "TimingsInsertOrUpdate";
		private const string TimingsForceWrite = "TimingsForceWrite";

		private readonly IVersionedReadWriteFactory _readWriteFactory = new VersionedReadWriteFactory();
		private readonly string _tableName;
		private readonly bool _tableCreated;
		private readonly FileStream _fileStream;
		private ushort _internalDataVersion;
		private ushort _internalWriteVersion;
		private bool _disposed;
		private int _recordCount = 0;
		private int _dataLength = 0;
		private byte _compactPercent = 0;
		private int _pageCount;
		private long _primarySequence = DefaultSequenceId;
		private long _SecondarySequence = DefaultSequenceId;
		private readonly object _lockObject = new object();
		private readonly TableAttribute _tableAttributes;
		private readonly Dictionary<string, ForeignKeyRelation> _foreignKeys;
		private readonly ISimpleDBManager _simleDBManager;
		private bool _hasInitialized;
		private readonly IForeignKeyManager _foreignKeyManager;
		private readonly BatchUpdateDictionary<string, IIndexManager> _indexes;
		private readonly Dictionary<TriggerType, List<ITableTriggers<T>>> _triggersMap;
		private PageSize _pageSize;
		private List<T> _allRecords = null;
		private readonly bool _isMemoryCaching;
		private readonly Dictionary<string, Timings> _ReadWriteTimes = new()
			{
				{ TimingsSelectAll, new() },
				{ TimingsSelectId, new() },
				{ TimingsSelectPredicate, new() },
				{ TimingsInsertList, new() },
				{ TimingsInsert, new() },
				{ TimingsDeleteList, new() },
				{ TimingsDelete, new() },
				{ TimingsTruncate, new() },
				{ TimingsUpdateList, new() },
				{ TimingsUpdate, new() },
				{ TimingsInsertOrUpdate, new() },
				{ TimingsForceWrite, new() }
			};

		#endregion Private Members

		#region Constructors / Destructors

		public SimpleDBOperations(ISimpleDBManager readerWriterInitializer, IForeignKeyManager foreignKeyManager)
		{
			_simleDBManager = readerWriterInitializer ?? throw new ArgumentNullException(nameof(readerWriterInitializer));
			_foreignKeyManager = foreignKeyManager ?? throw new ArgumentNullException(nameof(foreignKeyManager));

			_tableAttributes = GetTableAttributes();

			if (_tableAttributes == null)
				throw new InvalidOperationException($"TableAttribute is missing from class {typeof(T).FullName}");

			_isMemoryCaching = _tableAttributes.CachingStrategy == CachingStrategy.Memory ||
				_tableAttributes.CachingStrategy == CachingStrategy.SlidingMemory ||
				_tableAttributes.WriteStrategy == WriteStrategy.Lazy;

			_triggersMap = new Dictionary<TriggerType, List<ITableTriggers<T>>>();

			foreach (TriggerType triggerType in Enum.GetValues(typeof(TriggerType)))
				_triggersMap.Add(triggerType, new());

			_foreignKeys = GetForeignKeysForTable();
			_indexes = BuildIndexListForTable();

			(_tableCreated, _tableName) = ValidateTableName(_simleDBManager.Path, _tableAttributes.Domain, _tableAttributes.TableName, _tableAttributes.PageSize);
			_fileStream = File.Open(_tableName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

			try
			{
				ValidateTableContents();
			}
			catch (InvalidDataException)
			{
				_fileStream.Dispose();
				_fileStream = null;
				GC.SuppressFinalize(this);

				throw;
			}

			_simleDBManager.RegisterTable(this);
			_foreignKeyManager.RegisterTable(this);


			RebuildAllMissingIndexes();
		}

		~SimpleDBOperations()
		{
			Dispose(false);
		}

		#endregion Constructors / Destructors

		#region ISimpleDBTable

		#region Properties

		public string TableName
		{
			get
			{
				if (_tableAttributes != null)
					return _tableAttributes.TableName;

				return null;
			}
		}

		public TimeSpan SlidingMemoryTimeout => _tableAttributes.SlidingMemoryTimeout;

		public CachingStrategy CachingStrategy => _tableAttributes.CachingStrategy;

		public WriteStrategy WriteStrategy => _tableAttributes.WriteStrategy;

		public Dictionary<string, Timings> GetAllTimings
		{
			get
			{
				Dictionary<string, Timings> Result = new();

				foreach (KeyValuePair<string, Timings> item in _ReadWriteTimes)
				{
					Result.Add(item.Key, item.Value.Clone());
				}

				return Result;
			}
		}

		#endregion Properties

		#region Methods

		public void Initialize(IPluginClassesService pluginClassesService)
		{
			if (pluginClassesService == null)
				throw new ArgumentNullException(nameof(pluginClassesService));

			if (_hasInitialized)
				return;

			ITableDefaults<T> tableDefaults = pluginClassesService.GetPluginClasses<ITableDefaults<T>>()
				.FirstOrDefault();

			List<ITableTriggers<T>> triggers = pluginClassesService.GetPluginClasses<ITableTriggers<T>>();

			foreach (TriggerType triggerType in Enum.GetValues(typeof(TriggerType)))
				_triggersMap[triggerType].AddRange(triggers.Where(t => t.TriggerTypes.HasFlag(triggerType)).ToList());

			if (_tableCreated && tableDefaults != null)
			{
				if (_primarySequence == DefaultSequenceId)
					_primarySequence = tableDefaults.PrimarySequence;

				if (_SecondarySequence == DefaultSequenceId)
					_SecondarySequence = tableDefaults.SecondarySequence;

				if (tableDefaults.InitialData != null)
				{
					for (ushort i = ++_internalDataVersion; i < ushort.MaxValue; i++)
					{
						List<T> initialData = tableDefaults.InitialData(i);

						if (initialData == null || initialData.Count == 0)
							break;

						Insert(initialData);

						_internalDataVersion = InternalUpdateVersion(i);
					}
				}
			}

			_hasInitialized = true;
		}

		public bool IdExists(long id)
		{
			return _indexes[nameof(TableRowDefinition.Id)].Contains(id);
		}

		public bool IndexExists(string name, object value)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (!_indexes.ContainsKey(name))
				throw new ArgumentOutOfRangeException($"Index {name} does not exist");

			return _indexes[name].Contains(value);
		}

		public bool IdIsInUse(string propertyName, long value)
		{
			foreach (T record in Select())
			{
				long keyValue = Convert.ToInt64(record.GetType().GetProperty(propertyName).GetValue(record, null));

				if (value.Equals(keyValue))
					return true;
			}

			return false;
		}

		#endregion Methods

		#region Events

		public event SimpleDbEvent OnAction;

		#endregion Events

		#endregion ISimpleDBTable

		#region ISimpleDBOperation<T>

		#region Properties

		public ushort FileVersion => _internalDataVersion;

		public int DataLength => _dataLength;

		public int RecordCount => _recordCount;

		public long PrimarySequence => _primarySequence;

		public long SecondarySequence => _SecondarySequence;

		public byte CompactPercent => _compactPercent;

		public int PageCount => _pageCount;

		public PageSize PageSize => _pageSize;

		public object TableLock => _lockObject;

		#endregion Properties

		public void ClearAllMemory()
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject, TimeSpan.FromMilliseconds(30)))
			{
				_allRecords = null;
			}
		}

		public IReadOnlyList<T> Select()
		{
			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsSelectAll]))
			{
				if (_disposed)
					throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

				return InternalReadAllRecords().AsReadOnly();
			}
		}

		public T Select(long id)
		{
			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsSelectId]))
			{
				if (_disposed)
					throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

				return InternalReadAllRecords().FirstOrDefault(r => r.Id.Equals(id));
			}
		}

		public IReadOnlyList<T> Select(Func<T, bool> predicate)
		{
			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsSelectPredicate]))
			{
				if (_disposed)
					throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

				if (predicate == null)
					throw new ArgumentNullException(nameof(predicate));

				return InternalReadAllRecords().Where(predicate).ToList().AsReadOnly();
			}
		}

		public void Insert(List<T> records)
		{
			Insert(records, new InsertOptions());
		}

		public void Insert(List<T> records, InsertOptions insertOptions)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			if (records == null)
				throw new ArgumentNullException(nameof(records));

			if (records.Count == 0)
				throw new ArgumentException("Does not contain any records", nameof(records));

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsInsertList]))
			{
				InternalInsertRecords(records, insertOptions ?? new InsertOptions());
			}
		}

		public void Insert(T record)
		{
			Insert(record, new InsertOptions());
		}

		public void Insert(T record, InsertOptions insertOptions)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			if (record == null)
				throw new ArgumentNullException(nameof(record));

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsInsert]))
			{
				InternalInsertRecords(new List<T> { record }, insertOptions ?? new InsertOptions());
			}
		}

		public void Delete(List<T> records)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			if (records == null)
				throw new ArgumentNullException(nameof(records));

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsDeleteList]))
			{
				InternalDeleteRecords(records);
			}
		}

		public void Delete(T record)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			if (record == null)
				throw new ArgumentNullException(nameof(record));

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsDelete]))
			{
				InternalDeleteRecords(new List<T>() { record });
			}
		}

		public void Truncate()
		{
			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsTruncate]))
			{
				InternalDeleteRecords(InternalReadAllRecords());
			}
		}

		public void Update(List<T> records)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			if (records == null)
				throw new ArgumentNullException(nameof(records));

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsUpdateList]))
			{
				InternalUpdateRecords(records);
			}
		}

		public void Update(T record)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			if (record == null)
				throw new ArgumentNullException(nameof(record));

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsUpdate]))
			{
				InternalUpdateRecords(new List<T>() { record });
			}
		}

		public void InsertOrUpdate(T record)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			if (record == null)
				throw new ArgumentNullException(nameof(record));

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsInsertOrUpdate]))
			{
				if (IdExists(record.Id))
					InternalUpdateRecords(new List<T> { record });
				else
					InternalInsertRecords(new List<T>() { record }, new InsertOptions());
			}
		}

		public void ForceWrite()
		{
			using (StopWatchTimer timer = StopWatchTimer.Initialise(_ReadWriteTimes[TimingsForceWrite]))
			{
				if (_tableAttributes != null && _tableAttributes.WriteStrategy != WriteStrategy.Forced)
				{
					using (TimedLock timedLock = TimedLock.Lock(_lockObject, TimeSpan.FromMilliseconds(30)))
					{

						InternalSaveRecordsToDisk(InternalReadAllRecords(), true);
					}
				}
			}
		}

		#region Sequences

		public long NextSequence()
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			return NextSequence(Consts.DefaultSequenceIncrement);
		}

		public long NextSequence(long increment)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			return InternalNextSequence(increment);
		}

		public long NextSecondarySequence(long increment)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			return InternalNextSecondarySequence(increment);
		}

		public void ResetSequence(long primarySequence, long secondarySequence)
		{
			if (_disposed)
				throw new ObjectDisposedException(nameof(SimpleDBOperations<T>));

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
				writer.Seek(Consts.PrimarySequenceStart, SeekOrigin.Begin);
				writer.Write(primarySequence);
				writer.Write(secondarySequence);

				_fileStream.Flush(true);

				_primarySequence = primarySequence;
				_SecondarySequence = secondarySequence;
			}
		}

		#endregion Sequences

		#endregion ISimpleDBOperation<T>

		#region Disposable

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion Disposable

		#region Private Methods

		private long InternalNextSequence(long increment)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				_primarySequence += increment;

				using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
				writer.Seek(Consts.PrimarySequenceStart, SeekOrigin.Begin);
				writer.Write(_primarySequence);

				if (_tableAttributes.WriteStrategy == WriteStrategy.Forced)
					_fileStream.Flush(true);

				return _primarySequence;
			}
		}

		private long InternalNextSecondarySequence(long increment)
		{

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				_SecondarySequence += increment;

				using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
				writer.Seek(Consts.SecondarySequenceStart, SeekOrigin.Begin);
				writer.Write(_SecondarySequence);

				if (_tableAttributes.WriteStrategy == WriteStrategy.Forced)
					_fileStream.Flush(true);

				return _SecondarySequence;
			}
		}

		private List<T> InternalReadAllRecords()
		{
			OnAction?.Invoke(this);

			if (_allRecords != null)
			{
				return _allRecords;
			}

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{

				IDataReader dataReader = _readWriteFactory.GetReader(_internalWriteVersion);

				List<T> Result = dataReader.ReadRecords<T>(_fileStream, ref _pageCount, ref _recordCount, ref _dataLength);

				Result.ForEach(r => { r.Immutable = true; r.Loaded = true; });

				if (_isMemoryCaching)
				{
					_allRecords = Result;
				}

				return Result;
			}
		}

		private void InternalUpdateRecords(List<T> records)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				records = records.Where(r => r.HasChanged).ToList();

				if (_foreignKeys.Count > 0)
					ValidateForeignKeys(records);

				_indexes.BeginUpdate();
				try
				{
					List<T> existingRecords = InternalReadAllRecords();

					_triggersMap[TriggerType.BeforeUpdate].ForEach(t => t.BeforeUpdate(records));

					foreach (T record in records)
					{
						for (int i = existingRecords.Count - 1; i >= 0; i--)
						{
							T existingRecord = existingRecords[i];
							if (existingRecord.Id.Equals(record.Id))
							{
								_triggersMap[TriggerType.BeforeUpdateCompare].ForEach(t => t.BeforeUpdate(record, existingRecord));
								InternalRemoveIndex(existingRecords[i]);
								InternalAddIndex(record);
								existingRecords[i] = record;
								break;
							}
						}
					}

					InternalSaveRecordsToDisk(existingRecords, false);

					_triggersMap[TriggerType.AfterUpdate].ForEach(t => t.AfterUpdate(records));

					records.ForEach(r => r.HasChanged = false);
				}
				finally
				{
					_indexes.EndUpdate();
				}
			}
		}

		private void InternalSaveRecordsToDisk(List<T> recordsToSave, bool forceWrite)
		{
			_recordCount = recordsToSave.Count;

			if (forceWrite || _tableAttributes.WriteStrategy == WriteStrategy.Forced)
			{
				IDataWriter dataWriter = _readWriteFactory.GetWriter();
				dataWriter.WriteData<T>(_fileStream, recordsToSave, _tableAttributes.Compression,
					_pageSize, ref _compactPercent, ref _pageCount);

				if (_internalWriteVersion < dataWriter.Version)
				{
					using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
					_fileStream.Seek(Consts.WriteVersionStart, SeekOrigin.Begin);
					writer.Write(dataWriter.Version);
					_internalWriteVersion = dataWriter.Version;
				}

				_fileStream.Flush(true);
			}



			if (_isMemoryCaching)
			{
				_allRecords = recordsToSave;
				_allRecords.ForEach(ar => { ar.Immutable = true; ar.Loaded = true; });
			}
			else
			{
				_allRecords = null;
			}

			OnAction?.Invoke(this);
		}

		private void InternalDeleteRecords(List<T> records)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				ValidateForeignKeysPriorToDelete(records);

				_indexes.BeginUpdate();
				try
				{
					_triggersMap[TriggerType.BeforeDelete].ForEach(t => t.BeforeDelete(records));

					List<T> existingRecords = InternalReadAllRecords();

					for (int i = records.Count - 1; i >= 0; i--)
					{
						T record = records[i];

						for (int j = existingRecords.Count - 1; j >= 0; j--)
						{
							T existingRecord = existingRecords[j];

							if (existingRecord.Id.Equals(record.Id))
							{
								existingRecords.RemoveAt(j);
								InternalRemoveIndex(existingRecord);

								break;
							}

						}
					}

					InternalSaveRecordsToDisk(existingRecords, false);

					_triggersMap[TriggerType.AfterDelete].ForEach(t => t.AfterDelete(records));
				}
				finally
				{
					_indexes.EndUpdate();
				}
			}
		}

		private void InternalInsertRecords(List<T> records, InsertOptions textTableInsertOptions)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				ValidateInternalIndexes(records);

				long nextSequence;

				if (textTableInsertOptions.AssignPrimaryKey)
				{
					nextSequence = PrimarySequence + 1;
					_ = NextSequence(records.Count);
				}
				else
				{
					nextSequence = 0;
				}

				_indexes.BeginUpdate();
				try
				{
					_triggersMap[TriggerType.BeforeInsert].ForEach(t => t.BeforeInsert(records));

					records.ForEach(r =>
					{
						ValidateForeignKeys(r);
						if (textTableInsertOptions.AssignPrimaryKey)
						{
							r.Id = nextSequence++;
						}

						InternalAddIndex(r);
					});

					List<T> existingRecords = InternalReadAllRecords();
					existingRecords.AddRange(records);
					InternalSaveRecordsToDisk(existingRecords, false);

					_triggersMap[TriggerType.AfterInsert].ForEach(t => t.AfterInsert(records));
				}
				finally
				{
					_indexes.EndUpdate();
				}
			}
		}

		private void ValidateInternalIndexes(List<T> records)
		{
			foreach (KeyValuePair<string, IIndexManager> item in _indexes)
			{
				foreach (T record in records)
				{
					object keyValue = GetIndexValue(record, item.Value);

					if (_indexes[item.Key].Contains(keyValue))
					{
						throw new UniqueIndexException($"Index already exists; Table: {TableName}; Index Name: {item.Key}; Property: {String.Join(',', item.Value.PropertyNames)}; Value: {keyValue}");
					}
				}
			}
		}

		private void InternalAddIndex(T record)
		{
			foreach (KeyValuePair<string, IIndexManager> item in _indexes)
			{
				object keyValue = GetIndexValue(record, item.Value);

				_indexes[item.Key].Add(keyValue);
			}
		}

		private void InternalRemoveIndex(T record)
		{
			foreach (KeyValuePair<string, IIndexManager> item in _indexes)
			{
				object value = GetIndexValue(record, item.Value);

				_indexes[item.Key].Remove(value);
			}
		}

		private void RebuildAllMissingIndexes()
		{
			IReadOnlyList<T> allRecords = Select();

			foreach (KeyValuePair<string, IIndexManager> item in _indexes)
			{
				item.Value.BeginUpdate();
				try
				{
					foreach (T record in allRecords)
					{
						object keyValue = GetIndexValue(record, item.Value);

						if (!_indexes[item.Key].Contains(keyValue))
							_indexes[item.Key].Add(keyValue);
					}
				}
				finally
				{
					item.Value.EndUpdate();
				}
			}
		}

		private void ValidateForeignKeysPriorToDelete(List<T> records)
		{
			foreach (KeyValuePair<string, IIndexManager> index in _indexes)
			{
				if (index.Value.PropertyNames.Count > 1)
					continue;

				foreach (T record in records)
				{
					object keyValue = record.GetType().GetProperty(index.Key).GetValue(record, null);

					if (Int64.TryParse(keyValue.ToString(), out long value))
					{
						ForeignKeyUsage foreignKeyUsage = _foreignKeyManager.ValueInUse(TableName, index.Key, value, out string table, out string propertyName);

						if (foreignKeyUsage == ForeignKeyUsage.Referenced && 
							(foreignKeyUsage != ForeignKeyUsage.AllowDefault && foreignKeyUsage != ForeignKeyUsage.CascadeDelete))
						{
							throw new ForeignKeyException($"Foreign key value {keyValue} from table {TableName} is being used in Table: {table}; Property: {propertyName}");
						}
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ValidateForeignKeys(List<T> records)
		{
			foreach (KeyValuePair<string, ForeignKeyRelation> foreignKey in _foreignKeys)
			{
				foreach (T record in records)
				{
					long keyValue = Convert.ToInt64(record.GetType().GetProperty(foreignKey.Key).GetValue(record, null));
					ForeignKeyRelation foreignKeyRelation = foreignKey.Value;

					if (!_foreignKeyManager.ValueExists(foreignKeyRelation.Name, keyValue))
					{
						if (!(foreignKeyRelation.Attributes == ForeignKeyAttributes.DefaultValue && keyValue.Equals(0)))
							throw new ForeignKeyException($"Foreign key value {keyValue} does not exist in table {foreignKey.Value}; Table: {TableName}; Property: {foreignKey.Key}");
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ValidateForeignKeys(T record)
		{
			foreach (KeyValuePair<string, ForeignKeyRelation> foreignKey in _foreignKeys)
			{
				long keyValue = Convert.ToInt64(record.GetType().GetProperty(foreignKey.Key).GetValue(record, null));
				ForeignKeyRelation foreignKeyRelation = foreignKey.Value;

				if (!_foreignKeyManager.ValueExists(foreignKeyRelation.Name, keyValue))
				{
					if (!(foreignKeyRelation.Attributes == ForeignKeyAttributes.DefaultValue && keyValue.Equals(0)))
						throw new ForeignKeyException($"Foreign key value {keyValue} does not exist in table {foreignKey.Value}; Table: {TableName}; Property: {foreignKey.Key}");
				}
			}
		}

		private Dictionary<string, ForeignKeyRelation> GetForeignKeysForTable()
		{
			Dictionary<string, ForeignKeyRelation> Result = new Dictionary<string, ForeignKeyRelation>();

			foreach (PropertyInfo property in typeof(T).GetProperties())
			{
				ForeignKeyAttribute foreignKey = (ForeignKeyAttribute)property.GetCustomAttributes(true)
					.FirstOrDefault(ca => ca.GetType().Equals(typeof(ForeignKeyAttribute)));

				if (foreignKey != null && property.PropertyType.Equals(typeof(long)))
				{
					_foreignKeyManager.AddRelationShip(TableName, foreignKey.TableName, property.Name, foreignKey.PropertyName, foreignKey.Attributes);
					Result.Add(property.Name, new ForeignKeyRelation(foreignKey.TableName, foreignKey.Attributes));
				}
			}

			return Result;
		}

		private static TableAttribute GetTableAttributes()
		{
			return (TableAttribute)typeof(T).GetCustomAttributes(true)
				.FirstOrDefault(a => a.GetType() == typeof(TableAttribute));
		}

		private static BatchUpdateDictionary<string, IIndexManager> BuildIndexListForTable()
		{
			BatchUpdateDictionary<string, IIndexManager> Result = new BatchUpdateDictionary<string, IIndexManager>();
			foreach (PropertyInfo property in typeof(T).GetProperties())
			{
				UniqueIndexAttribute uniqueIndex = (UniqueIndexAttribute)property.GetCustomAttributes(true)
					.FirstOrDefault(ca => ca.GetType().Equals(typeof(UniqueIndexAttribute)));

				if (uniqueIndex != null)
				{
					string indexName = String.IsNullOrEmpty(uniqueIndex.Name) ? property.Name : uniqueIndex.Name;

					if (Result.ContainsKey(indexName))
					{
						List<string> propertyNames = Result[indexName].PropertyNames;
						propertyNames.Add(property.Name);
						Result.Remove(indexName);
						Result.Add(indexName, new IndexManager<string>(uniqueIndex.IndexType, propertyNames.ToArray()));
					}
					else
					{
						if (property.PropertyType == typeof(long))
							Result.Add(indexName, new IndexManager<long>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(string))
							Result.Add(indexName, new IndexManager<string>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(int))
							Result.Add(indexName, new IndexManager<int>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(float))
							Result.Add(indexName, new IndexManager<float>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(double))
							Result.Add(indexName, new IndexManager<double>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(decimal))
							Result.Add(indexName, new IndexManager<decimal>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(uint))
							Result.Add(indexName, new IndexManager<uint>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(ulong))
							Result.Add(indexName, new IndexManager<ulong>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(short))
							Result.Add(indexName, new IndexManager<short>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(ushort))
							Result.Add(indexName, new IndexManager<ushort>(uniqueIndex.IndexType, property.Name));
						else if (property.PropertyType == typeof(bool))
							Result.Add(indexName, new IndexManager<bool>(uniqueIndex.IndexType, property.Name));
						else
							throw new InvalidOperationException($"Type {property.PropertyType.Name} not supported");
					}
				}
			}

			return Result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static object GetIndexValue(T record, IIndexManager indexManager)
		{
			if (indexManager.PropertyNames.Count == 1)
			{
				return record.GetType().GetProperty(indexManager.PropertyNames[0]).GetValue(record, null);
			}
			else
			{
				StringBuilder sb = new StringBuilder();

				foreach (string propertyName in indexManager.PropertyNames)
				{
					sb.Append(record.GetType().GetProperty(propertyName).GetValue(record, null) ?? String.Empty);
				}

				return sb.ToString();
			}
		}

		private void Dispose(bool disposing)
		{
			if (_disposed)
				return;

			if (disposing)
			{
				_simleDBManager?.UnregisterTable(this);

				if (_foreignKeyManager != null)
					_foreignKeyManager.UnregisterTable(this);
			}

			ForceWrite();

			if (_fileStream != null)
			{
				_fileStream.Flush(true);
				_fileStream.Close();
				_fileStream.Dispose();
			}

			_disposed = true;
		}

		private void ValidateTableContents()
		{
			using BinaryReader reader = new BinaryReader(_fileStream, Encoding.UTF8, true);
			_fileStream.Seek(0, SeekOrigin.Begin);

			_internalDataVersion = reader.ReadUInt16();

			Span<byte> header = reader.ReadBytes(Consts.HeaderLength);

			for (int i = 0; i < header.Length; i++)
			{
				if (header[i] != Consts.Header[i])
					throw new InvalidDataException();
			}

			_primarySequence = reader.ReadInt64();
			_SecondarySequence = reader.ReadInt64();
			_internalWriteVersion = reader.ReadUInt16();
			_ = reader.ReadUInt16();
			_ = reader.ReadInt32();
			_ = reader.ReadInt32();
			_pageSize = (PageSize)reader.ReadInt32();
			_ = (CompressionType)reader.ReadByte();
			_recordCount = reader.ReadInt32();
			_ = reader.ReadInt32();
			_dataLength = reader.ReadInt32();
			_pageCount = reader.ReadInt32();
		}

		private (bool, string) ValidateTableName(string path, string domain, string name, PageSize pageSize)
		{
			string extension = Path.GetExtension(name);

			if (String.IsNullOrEmpty(extension))
				name += Consts.DefaultExtension;

			string tableName = path;

			if (!String.IsNullOrEmpty(domain))
				tableName = Path.Combine(tableName, domain);

			if (!Directory.Exists(tableName))
				Directory.CreateDirectory(tableName);

			tableName = Path.Combine(tableName, name);

			bool tableCreated = false;

			if (!File.Exists(tableName))
			{
				CreateTableHeaderRecords(tableName, pageSize);
				tableCreated = true;
			}

			return (tableCreated, tableName);
		}

		private ushort InternalUpdateVersion(ushort version)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
				writer.Seek(Consts.DataVersionStart, SeekOrigin.Begin);
				writer.Write(version);
				_fileStream.Flush(true);
			}

			return version;
		}

		private void CreateTableHeaderRecords(string fileName, PageSize pageSize)
		{
			using FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);
			using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false);
			writer.Seek(0, SeekOrigin.Begin);

			writer.Write(FileVersion);
			writer.Write(Consts.Header);
			writer.Write(_primarySequence);
			writer.Write(_SecondarySequence);
			writer.Write((ushort)0);
			writer.Write((ushort)0);
			writer.Write((int)0);
			writer.Write((int)0);
			writer.Write((int)pageSize);
			writer.Write((byte)_tableAttributes.Compression);
			writer.Write(Consts.RowCountZero);
			writer.Write(Consts.DefaultLength);
			writer.Write(Consts.DefaultLength);
			writer.Write(Consts.DefaultLength);

			writer.Flush();
		}

		#endregion Private Methods
	}
}
