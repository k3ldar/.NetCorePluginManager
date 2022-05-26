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

using PluginManager.DAL.TextFiles.Interfaces;
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
    internal sealed class TextReaderWriter<T> : ITextReaderWriter<T>
        where T : BaseTable
    {
        #region Constants

        private static readonly byte[] Header = new byte[] { 80, 108, 117, 103, 105, 110, 77, 97, 110, 97, 103, 101, 114 };
        private const string DefaultExtension = ".dat";
        private const ushort FileVersion = 1;
        private const byte CompressionNone = 0;
        private const byte CompressionBrotli = 1;
        private const int RowCount = 0;
        private const int DefaultLength = 0;
        private const int TotalHeaderLength = HeaderLength + sizeof(ushort) + sizeof(int) + sizeof(int) + sizeof(long) + (sizeof(int) * 4) + sizeof(byte);
        private const int SequenceStart = HeaderLength + sizeof(ushort);
        private const int StartOfRecordCount = TotalHeaderLength - (sizeof(int) * 3);
        private const int HeaderLength = 13;

        #endregion Constants

        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly string _tableName;
        private readonly FileStream _fileStream;
        private bool _disposed;
        private int _recordCount = 0;
        private int _dataLength = 0;
        private CompressionType _compressionAlgorithm = CompressionNone;
        private long _sequence = -1;
        private readonly object _lockObject = new object();
        private readonly TableAttribute _tableAttributes;

        public int DataLength => _dataLength;

        public int RecordCount => _recordCount;

        #region Constructors / Destructors

        public TextReaderWriter(IReaderWriterInitializer readerWriterInitializer)
        {
            if (readerWriterInitializer == null)
                throw new ArgumentNullException(nameof(readerWriterInitializer));

            _tableAttributes = GetTableAttributes();

            if (_tableAttributes == null)
                throw new InvalidOperationException();

            _jsonSerializerOptions = new JsonSerializerOptions();
            _tableName = ValidateTableName(readerWriterInitializer.Path, _tableAttributes.TableName);
            _fileStream = File.Open(_tableName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

            try
            {
                ValidateTableContents();
            }
            catch (InvalidDataException)
            {
                _fileStream.Dispose();
                _fileStream = null;
            }
        }

        ~TextReaderWriter()
        {
            Dispose(false);
        }

        #endregion Constructors / Destructors

        #region ITextReaderWriter<T>

        public List<T> Read()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                return InternalReadAllRecords();
            }
        }

        #region Saving

        public void Save(List<T> records)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            if (records == null)
                throw new ArgumentNullException(nameof(records));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                InternalSaveAllRecords(records);
            }
        }

        #endregion Saving 

        #region CRUD

        public void Create(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(SequenceStart, SeekOrigin.Begin);
                _fileStream.Flush();
            }
            throw new NotImplementedException();
        }

        public void Delete(T record)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(SequenceStart, SeekOrigin.Begin);
                _fileStream.Flush();
            }
            throw new NotImplementedException();
        }

        #endregion CRUD

        #region Sequences

        public long NextSequence()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(TextReaderWriter<T>));

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
                writer.Seek(SequenceStart, SeekOrigin.Begin);
                writer.Write(++_sequence);
                _fileStream.Flush();
                return _sequence;
            }
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
                _fileStream.Flush();
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

        private List<T> InternalReadAllRecords()
        {
            using BinaryReader reader = new BinaryReader(_fileStream, Encoding.UTF8, true);
            _fileStream.Position = StartOfRecordCount;
            int recordsCount = reader.ReadInt32();
            int uncompressedSize = reader.ReadInt32();
            int dataLength = reader.ReadInt32();
            byte[] data = new byte[dataLength];
            data = reader.ReadBytes(dataLength);

            if (_tableAttributes.Compression == CompressionType.Brotli)
            {
                byte[] uncompressed = new byte[uncompressedSize];
                System.IO.Compression.BrotliDecoder.TryDecompress(data, uncompressed, out int byteLength);

                if (byteLength != uncompressedSize)
                    throw new InvalidDataException();

                return JsonSerializer.Deserialize<List<T>>(uncompressed, _jsonSerializerOptions);
            }

            return JsonSerializer.Deserialize<List<T>>(data, _jsonSerializerOptions);
        }

        private void InternalSaveAllRecords(List<T> records)
        {
            byte[] data = JsonSerializer.SerializeToUtf8Bytes(records, records.GetType(), _jsonSerializerOptions);
            byte[] compressedData = new byte[data.Length];
            int dataLength = data.Length;
            bool isCompressed = false;

            if (_tableAttributes.Compression == CompressionType.Brotli)
            {
                isCompressed = System.IO.Compression.BrotliEncoder.TryCompress(data, compressedData, out dataLength);
            }

            using BinaryWriter writer = new BinaryWriter(_fileStream, Encoding.UTF8, true);
            writer.Seek(StartOfRecordCount, SeekOrigin.Begin);
            writer.Write(records.Count);
            writer.Write(data.Length);
            writer.Write(dataLength);

            if (isCompressed)
            {
                writer.Write(compressedData, 0, dataLength);
            }
            else
            {
                writer.Write(data);
            }

            _fileStream.Flush();
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

            if (disposing)
                GC.SuppressFinalize(this);

            if (_fileStream != null)
            {
                _fileStream.Flush();
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

            if (_fileStream.Length - TotalHeaderLength != _dataLength)
                throw new InvalidDataException();
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
