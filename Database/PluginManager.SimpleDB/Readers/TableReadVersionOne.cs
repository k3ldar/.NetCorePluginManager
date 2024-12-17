/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SimpleDB
 *  
 *  File: TableReadVersionOne.cs
 *
 *  Purpose:  Version one data reader
 *
 *  Date        Name                Reason
 *  10/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text;
using System.Text.Json;

using SimpleDB.Abstractions;
using SimpleDB.Internal;

namespace SimpleDB.Readers
{
	internal sealed class TableReadVersionOne : IDataReader
	{
		///		this part repeats for all pages					53
		/// int			page number
		/// byte		page type
		/// ushort		page version
		/// int			Page n Datastart
		/// long		Next page start

		public ushort Version => 1;

		public List<T> ReadRecords<T>(FileStream fileStream, ref int pageCount, ref int recordCount, ref int dataLength)
		{
			using BinaryReader reader = new(fileStream, Encoding.UTF8, true);
			fileStream.Seek(Consts.StartOfRecordCount, SeekOrigin.Begin);
			CompressionType compressionType = (CompressionType)reader.ReadByte();
			recordCount = reader.ReadInt32();
			int uncompressedSize = reader.ReadInt32();
			dataLength = reader.ReadInt32();

			if (dataLength == 0)
				return [];

			Span<byte> data = dataLength < Consts.MaxStackAllocSize ? stackalloc byte[dataLength] : new Byte[dataLength];

			int totalPageCount = reader.ReadInt32();

			if (totalPageCount != pageCount)
				throw new InvalidOperationException("Invalid page count");

			int bytePosition = 0;

			for (int i = 0; i < pageCount; i++)
			{
				int pageNumber = reader.ReadInt32();
				byte pageType = reader.ReadByte();
				_ = reader.ReadUInt16();

				if (pageNumber != 1 + i)
					throw new InvalidOperationException("Invalid page number");

				if (pageType == Consts.PageTypeData)
				{
					_ = reader.ReadInt64();
					int sizeinPage = reader.ReadInt32();

					Span<byte> pageData = reader.ReadBytes(sizeinPage);

					for (int j = 0; j < sizeinPage; j++)
					{
						data[bytePosition++] = pageData[j];
					}
				}
			}

			List<T> Result;

			if (compressionType == CompressionType.Brotli)
			{
				Span<byte> uncompressed = uncompressedSize < Consts.MaxStackAllocSize ? stackalloc byte[uncompressedSize] : new byte[uncompressedSize];

				System.IO.Compression.BrotliDecoder.TryDecompress(data, uncompressed, out int byteLength);

				if (byteLength != uncompressedSize)
					throw new InvalidDataException();

				Result = JsonSerializer.Deserialize<List<T>>(uncompressed, Consts.JsonSerializerOptions);
			}
			else
			{
				Result = JsonSerializer.Deserialize<List<T>>(data, Consts.JsonSerializerOptions);
			}

			return Result;
		}
	}
}
