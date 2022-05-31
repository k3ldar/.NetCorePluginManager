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
 *  File: TextReaderWriter.cs
 *
 *  Purpose:  IAccountProvider for text based storage
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Text.Json;
using SharedPluginFeatures;
using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Internal
{
    /// <summary>
    /// Internal structure for file is:
    /// 
    /// ushort      Internal version number
    /// byte[13]    Header
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
    internal sealed class TextReaderWriter<T> : ITextReaderWriter<T>, ITextTable
        where T : BaseRow
    {
        #region Constants

        private static readonly byte[] Header = new byte[] { 80, 108, 117, 103, 105, 110, 77, 97, 110, 97, 103, 101, 114 };
        private const string DefaultExtension = ".dat";
        private const ushort FileVersion = 1;
        private const byte CompressionNone = 0;
        private const byte CompressionBrotli = 1;
        private const int RowCount = 0;
        private const int DefaultLength = 0;
        private const int TotalHeaderLength = sizeof(ushort) + HeaderLength + sizeof(long) + (sizeof(int) * 4) + sizeof(byte) + sizeof(int) + sizeof(int) + sizeof(int);
        private const int SequenceStart = HeaderLength + sizeof(ushort);
        private const int StartOfRecordCount = TotalHeaderLength - ((sizeof(int) * 3) + sizeof(byte));
        private const int HeaderLength = 13;
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
        private readonly IReaderWriterInitializer _initializer;
        private List<T> _allRecords = null;

        #region Constructors / Destructors

        public TextReaderWriter(IReaderWriterInitializer readerWriterInitializer)
        {
            _initializer = readerWriterInitializer ?? throw new ArgumentNullException(nameof(readerWriterInitializer));

            _tableAttributes = GetTableAttributes();

            if (_tableAttributes == null)
                throw new InvalidOperationException();

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

                throw;
            }

            _initializer.RegisterTable(this);
        }

        ~TextReaderWriter()
        {
            Dispose(false);
        }

        #endregion Constructors / Destructors

        #region ITextTable

        public string TableName => _tableAttributes.TableName;

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
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                return InternalReadAllRecords().AsReadOnly();
            }
        }

        public T Select(long id)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            return InternalReadAllRecords().Where(r => r.Id.Equals(id)).FirstOrDefault();
        }

        public void Insert(List<T> records)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            if (records.Count == 0)
                throw new ArgumentException("Does not contain any records", nameof(records));

            InternalInsertRecords(records);
        }

        public void Insert(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            InternalInsertRecords(new List<T> { record });
        }

        public void Delete(List<T> records)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            InternalDeleteRecords(records);
        }

        public void Delete(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            InternalDeleteRecords(new List<T>() { record });
        }

        public void Update(List<T> records)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            InternalUpdateRecords(records);
        }

        public void Update(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            if (record == null)
                throw new ArgumentNullException(nameof(record));

            InternalUpdateRecords(new List<T>() { record });
        }

        #region Sequences

        public long NextSequence()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            return NextSequence(DefaultSequenceIncrement);
        }

        public long NextSequence(long increment)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            return InternalNextSequence(increment);
        }

        public void ResetSequence(long sequence)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

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
                List<T> existingRecords = InternalReadAllRecords();

                foreach (T record in records)
                {
                    for (int i = existingRecords.Count - 1; i >= 0; i--)
                    {
                        if (existingRecords[i].Id.Equals(record.Id))
                        {
                            existingRecords[i] = record;
                            break;
                        }

                    }
                }

                InternalSaveRecordsToDisk(existingRecords);
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

            _compactPercent = Convert.ToByte(Shared.Utilities.Percentage(_fileStream.Length, _fileStream.Position));

            _fileStream.Flush(true);
        }

        private void InternalDeleteRecords(List<T> records)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                List<T> existingRecords = InternalReadAllRecords();

                foreach (T record in records)
                {
                    for (int i = existingRecords.Count -1; i >= 0; i--)
                    {
                        if (existingRecords[i].Id.Equals(record.Id))
                        { 
                            existingRecords.RemoveAt(i);
                            break;
                        }
                            
                    }
                }

                InternalSaveRecordsToDisk(existingRecords);
            }
        }

        private void InternalInsertRecords(List<T> records)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                long nextSequence = Sequence + 1;
                _ = NextSequence(records.Count);

                records.ForEach(r => r.Id = nextSequence++);

                List<T> existingRecords = InternalReadAllRecords();
                existingRecords.AddRange(records);

                InternalSaveRecordsToDisk(existingRecords);
            }
        }

        private TableAttribute GetTableAttributes()
        {
            return (TableAttribute)typeof(T).GetCustomAttributes(true)
                .Where(a => a.GetType() == typeof(TableAttribute))
                .FirstOrDefault();
        }

        private void Dispose(bool disposing)
        { 
            if (_disposed)
                return;

            _initializer?.UnregisterTable(this);

            if (disposing)
                GC.SuppressFinalize(this);

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
                name = name + DefaultExtension;

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
