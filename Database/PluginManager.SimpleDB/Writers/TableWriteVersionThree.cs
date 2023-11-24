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
 *  File: TableWriteVersionThree.cs
 *
 *  Purpose:  Version three for data writer
 *
 *  Date        Name                Reason
 *  06/01/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text;
using System.Text.Json;

using SimpleDB.Interfaces;
using SimpleDB.Internal;

namespace SimpleDB.Writers
{
	internal sealed class TableWriteVersionThree : IDataWriter
	{
		public ushort Version => 3;

		public void WriteData<T>(FileStream fileStream, List<T> recordsToSave, CompressionType compressionType, PageSize pageSize, ref byte compactPercent, ref int pageCount)
		{
			byte[] data = recordsToSave.Count > 0 ? JsonSerializer.SerializeToUtf8Bytes(recordsToSave, recordsToSave.GetType(), Consts.JsonSerializerOptions) : Array.Empty<byte>();

			bool isCompressed = false;

			using BinaryWriter writer = new BinaryWriter(fileStream, Encoding.UTF8, true);
			writer.Seek(Consts.StartOfRecordCount, SeekOrigin.Begin);

			if (compressionType == CompressionType.Brotli)
			{
				Span<byte> compressedData = data.Length < Consts.MaxStackAllocSize ? stackalloc byte[data.Length] : new byte[data.Length];
				isCompressed = System.IO.Compression.BrotliEncoder.TryCompress(data, compressedData, out int compressedDataLength);

				if (isCompressed)
				{
					compressionType = CompressionType.Brotli;
					writer.Write((byte)compressionType);
					writer.Write(recordsToSave.Count);
					writer.Write(compressedDataLength);
					writer.Write(data.Length);
					writer.Write(compressedData.ToArray(), 0, compressedDataLength);
					compactPercent = Convert.ToByte(Shared.Utilities.Percentage(fileStream.Length, fileStream.Position));
				}
				else
				{
					writer.Write((byte)compressionType);
					writer.Write(recordsToSave.Count);
					writer.Write(data.Length);
					writer.Write(data.Length);
					writer.Write(data, 0, data.Length);
				}
			}
			else
			{
				writer.Write((byte)compressionType);
				writer.Write(recordsToSave.Count);
				writer.Write(data.Length);
				writer.Write(data.Length);
				writer.Write(data, 0, data.Length);
			}

			writer.BaseStream.SetLength(writer.BaseStream.Position);
		}
	}
}
