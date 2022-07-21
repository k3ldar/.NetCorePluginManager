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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: TextTableOperations.cs
 *
 *  Purpose:  TextTableOperations for text based storage
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Tables;

using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Internal
{
    /// <summary>
    /// Internal structure for file is:
    /// 
    /// ushort      Internal version number
    /// byte[2]     Header
    /// long        Primary Sequence
    /// long        Secondary Sequence
    /// int         Reserved for future use
    /// int         Reserved for future use
    /// int         Reserved for future use
    /// byte        Compression
    /// int         RowCount
    /// int         Length of data before compression
    /// int         Length of data stored on disk
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class TextTableOperations<T> : ITextTableOperations<T>, ITextTable
        where T : TableRowDefinition
    {
        #region Private Classes

        private sealed class ForeignKeyRelation
        {
            public ForeignKeyRelation(string name, bool allowDefaultValue)
            {
                if (String.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                Name = name;
                AllowDefaultValue = allowDefaultValue;
            }

            public string Name { get; }

            public bool AllowDefaultValue { get; }
        }

        #endregion Private Classes

        #region Constants

        private static readonly byte[] Header = new byte[] { 80, 77 };
        private const string DefaultExtension = ".dat";
        private const byte CompressionNone = 0;
        private const byte CompressionBrotli = 1;
        private const int RowCount = 0;
        private const int DefaultLength = 0;
        private const int TotalHeaderLength = sizeof(ushort) + HeaderLength + sizeof(long) + sizeof(long) + (sizeof(int) * 3) + sizeof(byte) + sizeof(int) + sizeof(int) + sizeof(int);
        private const int PrimarySequenceStart = HeaderLength + sizeof(ushort);
        private const int SecondarySequenceStart = PrimarySequenceStart + sizeof(long);
        private const int DefaultStackSize = 1000000;
        private const int MaxStackAllocSize = DefaultStackSize / 4;


        private const int StartOfRecordCount = TotalHeaderLength - ((sizeof(int) * 3) + sizeof(byte));
        private const int HeaderLength = 2;
        private const int DefaultSequenceIncrement = 1;
        private const int VersionStart = 0;

        #endregion Constants

        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string _tableName;
        private readonly FileStream _fileStream;
        private readonly ushort _version;
        private bool _disposed;
        private int _recordCount = 0;
        private int _dataLength = 0;
        private byte _compactPercent = 0;
        private CompressionType _compressionAlgorithm = CompressionNone;
        private long _primarySequence = -1;
        private long _SecondarySequence = -1;
        private readonly object _lockObject = new object();
        private readonly TableAttribute _tableAttributes;
        private readonly Dictionary<string, ForeignKeyRelation> _foreignKeys;
        private readonly ITextTableInitializer _initializer;
        private readonly IForeignKeyManager _foreignKeyManager;
        private readonly BatchUpdateDictionary<string, IIndexManager> _indexes;
        private readonly List<ITableTriggers<T>> _triggers;
        private List<T> _allRecords = null;

        #region Constructors / Destructors

        public TextTableOperations(ITextTableInitializer readerWriterInitializer, 
            IForeignKeyManager foreignKeyManager, IPluginClassesService pluginClassesService)
        {
            _initializer = readerWriterInitializer ?? throw new ArgumentNullException(nameof(readerWriterInitializer));
            _foreignKeyManager = foreignKeyManager ?? throw new ArgumentNullException(nameof(foreignKeyManager));

            if (pluginClassesService == null)
                throw new ArgumentNullException(nameof(pluginClassesService));

            _tableAttributes = GetTableAttributes();

            if (_tableAttributes == null)
                throw new InvalidOperationException($"TableAttribute is missing from class {typeof(T).FullName}");

            ITableDefaults<T> tableDefaults = pluginClassesService.GetPluginClasses<ITableDefaults<T>>()
                .FirstOrDefault();

            if (tableDefaults != null)
            {
                _primarySequence = tableDefaults.PrimarySequence;
                _SecondarySequence = tableDefaults.SecondarySequence;
            }

            _triggers = pluginClassesService.GetPluginClasses<ITableTriggers<T>>();

            _foreignKeys = GetForeignKeysForTable();
            _indexes = BuildIndexListForTable();
            _jsonSerializerOptions = new JsonSerializerOptions();

            bool tableCreated;
            (tableCreated, _tableName) = ValidateTableName(_initializer.Path, _tableAttributes.Domain, _tableAttributes.TableName);
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

            _initializer.RegisterTable(this);
            _foreignKeyManager.RegisterTable(this);

            if (tableCreated && tableDefaults != null && tableDefaults.InitialData != null)
            {
                for (ushort i = ++_version; i < ushort.MaxValue; i++)
                {
                    List<T> initialData = tableDefaults.InitialData(i);

                    if (initialData == null)
                        break;

                    Insert(initialData);

                    _version = InternalUpdateVersion(i);
                }
            }

            RebuildAllMissingIndexes();
        }

        ~TextTableOperations()
        {
            Dispose(false);
        }

        #endregion Constructors / Destructors

        #region ITextTable

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

        #endregion Properties

        #region Methods

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

        #endregion ITextTable

        #region ITextReaderWriter<T>

        #region Properties

        public ushort FileVersion => _version;

        public int DataLength => _dataLength;

        public int RecordCount => _recordCount;

        public long PrimarySequence => _primarySequence;

        public long SecondarySequence => _SecondarySequence;

        public byte CompactPercent => _compactPercent;

        #endregion Properties

        public IReadOnlyList<T> Select()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                return InternalReadAllRecords().AsReadOnly();
            }
        }

        public T Select(long id)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            return InternalReadAllRecords().Where(r => r.Id.Equals(id)).FirstOrDefault();
        }

        public void Insert(List<T> records)
        {
            Insert(records, new TextTableInsertOptions());
        }

        public void Insert(List<T> records, TextTableInsertOptions insertOptions)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            if (records.Count == 0)
                throw new ArgumentException("Does not contain any records", nameof(records));

            InternalInsertRecords(records, insertOptions ?? new TextTableInsertOptions());
        }

        public void Insert(T record)
        {
            Insert(record, new TextTableInsertOptions());
        }

        public void Insert(T record, TextTableInsertOptions insertOptions)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            InternalInsertRecords(new List<T> { record }, insertOptions ?? new TextTableInsertOptions());
        }

        public void Delete(List<T> records)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            InternalDeleteRecords(records);
        }

        public void Truncate()
        {
            InternalDeleteRecords(InternalReadAllRecords());
        }

        public void Delete(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            InternalDeleteRecords(new List<T>() { record });
        }

        public void Update(List<T> records)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            InternalUpdateRecords(records);
        }

        public void Update(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            InternalUpdateRecords(new List<T>() { record });
        }

        public void InsertOrUpdate(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            if (IdExists(record.Id))
                InternalUpdateRecords(new List<T> { record }); 
            else
                InternalInsertRecords(new List<T>() { record }, new TextTableInsertOptions());
        }

        #region Sequences

        public long NextSequence()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            return NextSequence(DefaultSequenceIncrement);
        }

        public long NextSequence(long increment)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            return InternalNextSequence(increment);
        }

        public long NextSecondarySequence(long increment)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            return InternalNextSecondarySequence(increment);
        }

        public void ResetSequence(long primarySequence, long secondarySequence)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(PrimarySequenceStart, SeekOrigin.Begin);
                writer.Write(primarySequence);
                writer.Write(secondarySequence);
                _fileStream.Flush(true);

                _primarySequence = primarySequence;
                _SecondarySequence = secondarySequence;
            }
        }

        #endregion Sequences

        #endregion ITextReaderWriter<T>

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
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(PrimarySequenceStart, SeekOrigin.Begin);
                _primarySequence += increment;
                writer.Write(_primarySequence);
                _fileStream.Flush(true);

                return _primarySequence;
            }
        }

        private long InternalNextSecondarySequence(long increment)
        {

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(SecondarySequenceStart, SeekOrigin.Begin);
                _SecondarySequence += increment;
                writer.Write(_SecondarySequence);
                _fileStream.Flush(true);

                return _SecondarySequence;
            }
        }

        private List<T> InternalReadAllRecords()
        {
            if (_allRecords != null)
                return _allRecords;

            using BinaryReader reader = new BinaryReader(_fileStream, Encoding.UTF8, true);
            _fileStream.Seek(StartOfRecordCount, SeekOrigin.Begin);
            CompressionType compressionType = (CompressionType)reader.ReadByte();
            int recordsCount = reader.ReadInt32();
            int uncompressedSize = reader.ReadInt32();
            int dataLength = reader.ReadInt32();
            Span<byte> data = dataLength < MaxStackAllocSize ? stackalloc byte[dataLength] : new Byte[dataLength];

            data = reader.ReadBytes(dataLength);

            if (dataLength == 0)
                return new List<T>();

            List<T> Result = null;

            if (compressionType == CompressionType.Brotli)
            {
                Span<byte> uncompressed = uncompressedSize < MaxStackAllocSize ? uncompressed = stackalloc byte[uncompressedSize] : uncompressed = new byte[uncompressedSize];

                System.IO.Compression.BrotliDecoder.TryDecompress(data, uncompressed, out int byteLength);

                if (byteLength != uncompressedSize)
                    throw new InvalidDataException();

                Result = JsonSerializer.Deserialize<List<T>>(uncompressed, _jsonSerializerOptions);
            }
            else
            {
                Result = JsonSerializer.Deserialize<List<T>>(data, _jsonSerializerOptions);
            }

            _compactPercent = Convert.ToByte(Shared.Utilities.Percentage(_fileStream.Length, _fileStream.Position));

            Result.ForEach(r => { r.Immutable = true; r.Loaded = true; });

            if (_tableAttributes.CachingStrategy == CachingStrategy.Memory)
            {
                _allRecords = Result;
            }

            return Result;
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
                    _triggers.ForEach(t => t.BeforeUpdate(records));

                    List<T> existingRecords = InternalReadAllRecords();

                    foreach (T record in records)
                    {
                        for (int i = existingRecords.Count - 1; i >= 0; i--)
                        {
                            if (existingRecords[i].Id.Equals(record.Id))
                            {
                                InternalRemoveIndex(record);
                                InternalAddIndex(existingRecords[i]);
                                existingRecords[i] = record;
                                break;
                            }
                        }
                    }

                    InternalSaveRecordsToDisk(existingRecords);

                    _triggers.ForEach(t => t.AfterUpdate(records));

                    records.ForEach(r => r.HasChanged = false);
                }
                finally
                {
                    _indexes.EndUpdate();
                }
            }
        }

        private void InternalSaveRecordsToDisk(List<T> recordsToSave)
        {
            byte[] data = JsonSerializer.SerializeToUtf8Bytes(recordsToSave, recordsToSave.GetType(), _jsonSerializerOptions);
            Span<byte> compressedData = data.Length < MaxStackAllocSize ? compressedData = stackalloc byte[data.Length] : compressedData = new byte[data.Length];

            int dataLength = data.Length;
            bool isCompressed = false;
            CompressionType compressionType = CompressionType.None;

            if (_tableAttributes.Compression == CompressionType.Brotli)
            {
                isCompressed = System.IO.Compression.BrotliEncoder.TryCompress(data, compressedData, out dataLength);

                if (isCompressed)
                    compressionType = CompressionType.Brotli;
            }

            using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
            writer.Seek(StartOfRecordCount, SeekOrigin.Begin);
            writer.Write((byte)compressionType);
            writer.Write(recordsToSave.Count);
            writer.Write(data.Length);

            if (isCompressed)
            {
                writer.Write(dataLength);
                writer.Write(compressedData.ToArray(), 0, dataLength);
            }
            else
            {
                writer.Write(data.Length);
                writer.Write(data);
            }

            if (_tableAttributes.CachingStrategy == CachingStrategy.Memory)
            {
                _allRecords = recordsToSave;
                _allRecords.ForEach(ar => { ar.Immutable = true; ar.Loaded = true; });
            }
            else
            {
                _allRecords = null;
            }
            
            _compactPercent = Convert.ToByte(Shared.Utilities.Percentage(_fileStream.Length, _fileStream.Position));

            _fileStream.Flush(true);
            _recordCount = recordsToSave.Count;
        }

        private void InternalDeleteRecords(List<T> records)
        {
            VerifyIndexNotInUseAsForeignKey(records);

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                _indexes.BeginUpdate();
                try
                {
                    _triggers.ForEach(t => t.BeforeDelete(records));

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

                    InternalSaveRecordsToDisk(existingRecords);

                    _triggers.ForEach(t => t.AfterDelete(records));
                }
                finally
                {
                    _indexes.EndUpdate();
                }
            }
        }

        private void InternalInsertRecords(List<T> records, TextTableInsertOptions textTableInsertOptions)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                if (_foreignKeys.Count > 0)
                    ValidateForeignKeys(records);

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
                    _triggers.ForEach(t => t.BeforeInsert(records));
                    records.ForEach(r =>
                    {
                        if (textTableInsertOptions.AssignPrimaryKey)
                        {
                            r.Id = nextSequence++;
                        }

                        InternalAddIndex(r);
                    });

                    List<T> existingRecords = InternalReadAllRecords();
                    existingRecords.AddRange(records);
                    InternalSaveRecordsToDisk(existingRecords);

                    _triggers.ForEach(t => t.AfterInsert(records));
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
                        if (_indexes[item.Key].Contains(keyValue))
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
                object value = record.GetType().GetProperty(item.Key).GetValue(record, null);

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

        private void VerifyIndexNotInUseAsForeignKey(List<T> records)
        {
            foreach (KeyValuePair<string, IIndexManager> index in _indexes)
            {
                foreach (T record in records)
                {
                    object keyValue = record.GetType().GetProperty(index.Key).GetValue(record, null);

                    if (Int64.TryParse(keyValue.ToString(), out long value))
                    {
                        if (_foreignKeyManager.ValueInUse(TableName, index.Key, value, out string table, out string propertyName))
                            throw new ForeignKeyException($"Foreign key value {keyValue} from table {TableName} is being used in Table: {table}; Property: {propertyName}");
                    }
                }
            }
        }

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
                        if (!(foreignKeyRelation.AllowDefaultValue && keyValue.Equals(0)))
                            throw new ForeignKeyException($"Foreign key value {keyValue} does not exist in table {foreignKey.Value}; Table: {TableName}; Property: {foreignKey.Key}");
                    }
                }
            }
        }

        private Dictionary<string, ForeignKeyRelation> GetForeignKeysForTable()
        {
            Dictionary<string, ForeignKeyRelation> Result = new Dictionary<string, ForeignKeyRelation>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                ForeignKeyAttribute foreignKey = (ForeignKeyAttribute)property.GetCustomAttributes(true)
                    .Where(ca => ca.GetType().Equals(typeof(ForeignKeyAttribute)))
                    .FirstOrDefault();

                if (foreignKey != null && property.PropertyType.Equals(typeof(long)))
                {
                    _foreignKeyManager.AddRelationShip(TableName, foreignKey.TableName, property.Name, foreignKey.PropertyName);
                    Result.Add(property.Name, new ForeignKeyRelation(foreignKey.TableName, foreignKey.AllowDefaultValue));
                }
            }

            return Result;
        }

        private static TableAttribute GetTableAttributes()
        {
            return (TableAttribute)typeof(T).GetCustomAttributes(true)
                .Where(a => a.GetType() == typeof(TableAttribute))
                .FirstOrDefault();
        }

        private static BatchUpdateDictionary<string, IIndexManager> BuildIndexListForTable()
        {
            BatchUpdateDictionary<string, IIndexManager> Result = new BatchUpdateDictionary<string, IIndexManager>();
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                UniqueIndexAttribute uniqueIndex = (UniqueIndexAttribute)property.GetCustomAttributes(true)
                    .Where(ca => ca.GetType().Equals(typeof(UniqueIndexAttribute)))
                    .FirstOrDefault();

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
                _initializer?.UnregisterTable(this);

                if (_foreignKeyManager != null)
                    _foreignKeyManager?.UnregisterTable(this);
            }

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
            ushort _version = reader.ReadUInt16();

            //if (version != FileVersion)
            //    throw new InvalidDataException();

            Span<byte> header = stackalloc byte[HeaderLength];
            header = reader.ReadBytes(HeaderLength);

            for (int i = 0; i < header.Length; i++)
            {
                if (header[i] != Header[i])
                    throw new InvalidDataException();
            }

            _primarySequence = reader.ReadInt64();
            _SecondarySequence = reader.ReadInt64();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            _compressionAlgorithm = (CompressionType)reader.ReadByte();
            _recordCount = reader.ReadInt32();
            _ = reader.ReadInt32();
            _dataLength = reader.ReadInt32();

            _compactPercent = Convert.ToByte(Shared.Utilities.Percentage(_fileStream.Length, _fileStream.Position + _dataLength));
        }

        private (bool, string) ValidateTableName(string path, string domain, string name)
        {
            string extension = Path.GetExtension(name);

            if (String.IsNullOrEmpty(extension))
                name += DefaultExtension;

            string tableName = path;

            if (!String.IsNullOrEmpty(domain))
                tableName = Path.Combine(tableName, domain);

            if (!Directory.Exists(tableName))
                Directory.CreateDirectory(tableName);

            tableName = Path.Combine(tableName, name);

            bool tableCreated = false;

            if (!File.Exists(tableName))
            {
                CreateTableHeaderRecords(tableName);
                tableCreated = true;
            }

            return (tableCreated, tableName);
        }

        private ushort InternalUpdateVersion(ushort version)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(VersionStart, SeekOrigin.Begin);
                writer.Write(version);
                _fileStream.Flush(true);
            }

            return version;
        }

        private void CreateTableHeaderRecords(string fileName)
        {
            using FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);
            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false);

            writer.Write(FileVersion);
            writer.Write(Header);
            writer.Write(_primarySequence);
            writer.Write(_SecondarySequence);
            writer.Write((int)0);
            writer.Write((int)0);
            writer.Write((int)0);
            writer.Write((byte)_tableAttributes.Compression);
            writer.Write(RowCount);
            writer.Write(DefaultLength);
            writer.Write(DefaultLength);

            writer.Flush();
        }

        #endregion Private Methods
    }
}
