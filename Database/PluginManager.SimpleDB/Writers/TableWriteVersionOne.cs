﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: TableWriteVersionOne.cs
 *
 *  Purpose:  Version one data writer
 *
 *  Date        Name                Reason
 *  10/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text;
using System.Text.Json;

using SimpleDB.Interfaces;
using SimpleDB.Internal;

namespace SimpleDB.Writers
{
	internal sealed class TableWriteVersionOne : IDataWriter
	{
		public ushort Version => 1;

		public void WriteData<T>(FileStream fileStream, List<T> recordsToSave,
			CompressionType compressionType, PageSize pageSize,
			ref byte compactPercent, ref int pageCount)
		{
			byte[] data = recordsToSave.Count > 0 ? JsonSerializer.SerializeToUtf8Bytes(recordsToSave, recordsToSave.GetType(), Consts.JsonSerializerOptions) : [];

			int dataLength = data.Length;
			bool isCompressed = false;

			using BinaryWriter writer = new(fileStream, Encoding.UTF8, true);
			writer.Seek(Consts.StartOfRecordCount, SeekOrigin.Begin);

			if (compressionType == CompressionType.Brotli)
			{
				Span<byte> compressedData = data.Length < Consts.MaxStackAllocSize ? stackalloc byte[data.Length] : new byte[data.Length];
				isCompressed = System.IO.Compression.BrotliEncoder.TryCompress(data, compressedData, out dataLength);

				if (isCompressed)
				{
					compressionType = CompressionType.Brotli;
					writer.Write((byte)compressionType);
					writer.Write(recordsToSave.Count);
					writer.Write(data.Length);
					InternalSaveData(writer, compressedData[..dataLength].ToArray(), pageSize, ref pageCount);
					compactPercent = Convert.ToByte(Shared.Utilities.Percentage(fileStream.Length, fileStream.Position));
				}
				else
				{
					writer.Write((byte)compressionType);
					writer.Write(recordsToSave.Count);
					writer.Write(data.Length);
					InternalSaveData(writer, data, pageSize, ref pageCount);
				}
			}
			else
			{
				writer.Write((byte)compressionType);
				writer.Write(recordsToSave.Count);
				writer.Write(data.Length);
				InternalSaveData(writer, data, pageSize, ref pageCount);
			}

		}

		private static void InternalSaveData(BinaryWriter writer, byte[] data, PageSize pageSize, ref int pageCount)
		{
			pageCount = data.Length / (int)pageSize;

			if (data.Length % (int)pageSize > 0)
				pageCount++;

			writer.Write(data.Length);
			writer.Write(pageCount);
			int remainingData = data.Length;
			int pageSizeInt = (int)pageSize;

			for (int i = 0; i < pageCount; i++)
			{
				long nextPageStart = writer.BaseStream.Position + pageSizeInt + Consts.PageHeaderSize;
				int dataToWrite = remainingData > pageSizeInt ? pageSizeInt : remainingData;

				// page number 4
				writer.Write(i + 1);

				// page type 1
				writer.Write(Consts.PageTypeData);

				// page version 2
				writer.Write(Consts.PageVersion);

				// next page 8
				writer.Write(nextPageStart);

				// size of data on page 4
				writer.Write(dataToWrite);

				// write chunk of data 
				writer.Write(data, i * pageSizeInt, dataToWrite);

				remainingData -= dataToWrite;
			}
		}
	}
}
