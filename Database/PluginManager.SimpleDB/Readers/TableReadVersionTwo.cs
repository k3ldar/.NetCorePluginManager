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
 *  File: TableReadVersionTwo.cs
 *
 *  Purpose:  Version two data reader
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
	internal class TableReadVersionTwo : IDataReader
	{
		public ushort Version => 2;

		public List<T> ReadRecords<T>(FileStream fileStream, ref int pageCount, ref int recordCount, ref int dataLength)
		{
			using BinaryReader reader = new(fileStream, Encoding.UTF8, true);
			fileStream.Seek(Consts.StartOfRecordCount, SeekOrigin.Begin);
			CompressionType compressionType = (CompressionType)reader.ReadByte();
			pageCount = -1;
			recordCount = reader.ReadInt32();
			dataLength = reader.ReadInt32();

			if (dataLength == 0)
				return [];

			Span<byte> data = reader.ReadBytes(dataLength);

			List<T> Result;

			if (compressionType == CompressionType.Brotli)
			{
				Span<byte> uncompressed = dataLength < Consts.MaxStackAllocSize ? stackalloc byte[dataLength] : new byte[dataLength];

				System.IO.Compression.BrotliDecoder.TryDecompress(data, uncompressed, out int byteLength);

				if (byteLength != dataLength)
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
