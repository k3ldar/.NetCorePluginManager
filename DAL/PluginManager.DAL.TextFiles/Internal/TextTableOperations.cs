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
using System.Text;
using System.Text.Json;

using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Internal
{
    /// <summary>
    /// Internal structure for file is:
    /// 
    /// ushort      Internal version number
    /// byte[2]     Header
    /// long        Sequence
    /// int         Reserved
    /// int         Reserved
    /// int         Reserved
    /// int         Reserved
    /// byte        Compression
    /// int         RowCount
    /// int         Length of data before compression
    /// int         Length of data stored on disk
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class TextTableOperations<T> : ITextTableOperations<T>, ITextTable
        where T : TableRowDefinition
    {
        #region Constants

        private static readonly byte[] Header = new byte[] { 80, 77 };
        private const string DefaultExtension = ".dat";
        private const ushort FileVersion = 1;
        private const byte CompressionNone = 0;
        private const byte CompressionBrotli = 1;
        private const int RowCount = 0;
        private const int DefaultLength = 0;
        private const int TotalHeaderLength = sizeof(ushort) + HeaderLength + sizeof(long) + (sizeof(int) * 4) + sizeof(byte) + sizeof(int) + sizeof(int) + sizeof(int);
        private const int SequenceStart = HeaderLength + sizeof(ushort);
        private const int StartOfRecordCount = TotalHeaderLength - ((sizeof(int) * 3) + sizeof(byte));
        private const int HeaderLength = 2;
        private const int DefaultSequenceIncrement = 1;

        #endregion Constants

        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string _tableName;
        private readonly FileStream _fileStream;
        private bool _disposed;
        private int _recordCount = 0;
        private int _dataLength = 0;
        private byte _compactPercent = 0;
        private CompressionType _compressionAlgorithm = CompressionNone;
        private long _sequence = -1;
        private readonly object _lockObject = new object();
        private readonly TableAttribute _tableAttributes;
        private readonly Dictionary<string, string> _foreignKeys;
        private readonly ITextTableInitializer _initializer;
        private readonly IForeignKeyManager _foreignKeyManager;
        private readonly BatchUpdateDictionary<string, IIndexManager> _indexes;
        private List<T> _allRecords = null;

        #region Constructors / Destructors

        public TextTableOperations(ITextTableInitializer readerWriterInitializer, IForeignKeyManager foreignKeyManager)
        {
            _initializer = readerWriterInitializer ?? throw new ArgumentNullException(nameof(readerWriterInitializer));
            _foreignKeyManager = foreignKeyManager ?? throw new ArgumentNullException(nameof(foreignKeyManager));
            _tableAttributes = GetTableAttributes();

            if (_tableAttributes == null)
                throw new InvalidOperationException();

            _foreignKeys = GetForeignKeysForTable();
            _indexes = BuildIndexListForTable();
            _jsonSerializerOptions = new JsonSerializerOptions();
            _tableName = ValidateTableName(_initializer.Path, _tableAttributes.TableName);
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

            RebuildAllIndexes();
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

        public int DataLength => _dataLength;

        public int RecordCount => _recordCount;

        public long Sequence => _sequence;

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
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            if (records.Count == 0)
                throw new ArgumentException("Does not contain any records", nameof(records));

            InternalInsertRecords(records);
        }

        public void Insert(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            InternalInsertRecords(new List<T> { record });
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
                InternalInsertRecords(new List<T>() { record });
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

        public void ResetSequence(long sequence)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextTableOperations<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(SequenceStart, SeekOrigin.Begin);
                writer.Write(sequence);
                _fileStream.Flush(true);
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
                writer.Seek(SequenceStart, SeekOrigin.Begin);
                _sequence += increment;
                writer.Write(_sequence);
                _fileStream.Flush(true);
                return _sequence;
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
            byte[] data = new byte[dataLength];
            data = reader.ReadBytes(dataLength);

            if (dataLength == 0)
                return new List<T>();

            List<T> Result = null;

            if (compressionType == CompressionType.Brotli)
            {
                byte[] uncompressed = new byte[uncompressedSize];
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

            Result.ForEach(r => r.ImmutableId = true);

            if (_tableAttributes.CachingStrategy == CachingStrategy.Memory)
                _allRecords = Result;

            return Result;
        }

        private void InternalUpdateRecords(List<T> records)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                if (_foreignKeys.Count > 0)
                    ValidateForeignKeys(records);

                _indexes.BeginUpdate();
                try
                {
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
            byte[] compressedData = new byte[data.Length];
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
                writer.Write(compressedData, 0, dataLength);
            }
            else
            {
                writer.Write(data.Length);
                writer.Write(data);
            }

            if (_tableAttributes.CachingStrategy == CachingStrategy.Memory)
            {
                _allRecords = recordsToSave;
                _allRecords.ForEach(ar => ar.ImmutableId = true);
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
                }
                finally
                {
                    _indexes.EndUpdate();
                }
            }
        }

        private void InternalInsertRecords(List<T> records)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                if (_foreignKeys.Count > 0)
                    ValidateForeignKeys(records);

                ValidateInternalIndexes(records);

                long nextSequence = Sequence + 1;
                _ = NextSequence(records.Count);

                _indexes.BeginUpdate();
                try
                {
                    records.ForEach(r =>
                    {
                        r.Id = nextSequence++;
                        InternalAddIndex(r);
                    });

                    List<T> existingRecords = InternalReadAllRecords();
                    existingRecords.AddRange(records);
                    InternalSaveRecordsToDisk(existingRecords);
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
                    object value = record.GetType().GetProperty(item.Key).GetValue(record, null);

                    if (_indexes[item.Key].Contains(value))
                    {
                        if (_indexes[item.Key].Contains(value))
                            throw new UniqueIndexException($"Index already exists; Table: {TableName}; Property: {item.Key}; Value: {value}");
                    }
                }
            }
        }

        private void InternalAddIndex(T record)
        {
            foreach (KeyValuePair<string, IIndexManager> item in _indexes)
            {
                object value = record.GetType().GetProperty(item.Key).GetValue(record, null);

                _indexes[item.Key].Add(value);
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

        private void RebuildAllIndexes()
        {
            IReadOnlyList<T> allRecords = Select();

            foreach (KeyValuePair<string, IIndexManager> item in _indexes)
            {
                item.Value.BeginUpdate();
                try
                {
                    foreach (T record in allRecords)
                    {
                        long value = Convert.ToInt64(record.GetType().GetProperty(item.Key).GetValue(record, null));

                        _indexes[item.Key].Add(value);
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
            foreach (KeyValuePair<string, string> foreignKey in _foreignKeys)
            {
                foreach (T record in records)
                {
                    long keyValue = Convert.ToInt64(record.GetType().GetProperty(foreignKey.Key).GetValue(record, null));

                    if (!_foreignKeyManager.ValueExists(foreignKey.Value, keyValue))
                        throw new ForeignKeyException($"Foreign key value {keyValue} does not exist in table {foreignKey.Value}; Table: {TableName}; Property: {foreignKey.Key}");
                }
            }
        }

        private Dictionary<string, string> GetForeignKeysForTable()
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                ForeignKeyAttribute foreignKey = (ForeignKeyAttribute)property.GetCustomAttributes(true)
                    .Where(ca => ca.GetType().Equals(typeof(ForeignKeyAttribute)))
                    .FirstOrDefault();

                if (foreignKey != null && property.PropertyType.Equals(typeof(long)))
                {
                    _foreignKeyManager.AddRelationShip(TableName, foreignKey.TableName, property.Name, foreignKey.PropertyName);
                    Result.Add(property.Name, foreignKey.TableName);
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
                    if (property.PropertyType == typeof(long))
                        Result.Add(property.Name, new IndexManager<long>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(string))
                        Result.Add(property.Name, new IndexManager<string>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(int))
                        Result.Add(property.Name, new IndexManager<int>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(float))
                        Result.Add(property.Name, new IndexManager<float>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(double))
                        Result.Add(property.Name, new IndexManager<double>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(decimal))
                        Result.Add(property.Name, new IndexManager<decimal>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(uint))
                        Result.Add(property.Name, new IndexManager<uint>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(ulong))
                        Result.Add(property.Name, new IndexManager<ulong>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(short))
                        Result.Add(property.Name, new IndexManager<short>(uniqueIndex.IndexType));
                    else if (property.PropertyType == typeof(ushort))
                        Result.Add(property.Name, new IndexManager<ushort>(uniqueIndex.IndexType));
                    else
                        throw new InvalidOperationException($"Type {property.PropertyType.Name} not supported");
                }
            }

            return Result;
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
            ushort version = reader.ReadUInt16();

            if (version != FileVersion)
                throw new InvalidDataException();

            byte[] header = new byte[HeaderLength];
            header = reader.ReadBytes(HeaderLength);

            for (int i = 0; i < header.Length; i++)
            {
                if (header[i] != Header[i])
                    throw new InvalidDataException();
            }

            _sequence = reader.ReadInt64();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            _ = reader.ReadInt32();
            _compressionAlgorithm = (CompressionType)reader.ReadByte();
            _recordCount = reader.ReadInt32();
            _ = reader.ReadInt32();
            _dataLength = reader.ReadInt32();

            _compactPercent = Convert.ToByte(Shared.Utilities.Percentage(_fileStream.Length, _fileStream.Position + _dataLength));
        }

        private string ValidateTableName(string path, string name)
        {
            string extension = Path.GetExtension(name);

            if (String.IsNullOrEmpty(extension))
                name += DefaultExtension;

            string Result = Path.Combine(path, name);

            if (!File.Exists(Result))
            {
                CreateTableHeaderRecords(Path.Combine(path, Result));
            }

            return Result;
        }

        private void CreateTableHeaderRecords(string fileName)
        {
            using FileStream stream = File.Open(fileName, FileMode.OpenOrCreate);
            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, false);

            writer.Write(FileVersion);
            writer.Write(Header);
            writer.Write(_sequence);
            writer.Write((int)0);
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
