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
 *  File: Consts.cs
 *
 *  Purpose:  Shared internal constants
 *
 *  Date        Name                Reason
 *  10/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Text.Json;

namespace SimpleDB.Internal
{
	internal static class Consts
	{
		public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions()
		{
			AllowTrailingCommas = false,
			IncludeFields = true,
		};

		public static readonly byte[] Header = new byte[] { 80, 77 };
		public const string DefaultExtension = ".dat";
		public const byte CompressionNone = 0;
		public const byte CompressionBrotli = 1;
		public const int RowCountZero = 0;
		public const int DefaultLength = 0;
		public const int TotalHeaderLength = sizeof(ushort) + HeaderLength + sizeof(long) + sizeof(long) + (sizeof(int) * 4) + sizeof(byte) + sizeof(int) + sizeof(int) + sizeof(int) + sizeof(int);
		public const int PrimarySequenceStart = HeaderLength + sizeof(ushort);
		public const int SecondarySequenceStart = PrimarySequenceStart + sizeof(long);
		public const int WriteVersionStart = SecondarySequenceStart + sizeof(long);
		public const int PageHeaderSize = sizeof(int) + sizeof(byte) + sizeof(ushort) + sizeof(int) + sizeof(long);


		public const int StartOfRecordCount = TotalHeaderLength - ((sizeof(int) * 4) + sizeof(byte));
		public const int StartOfPageSize = StartOfRecordCount - (sizeof(byte) + sizeof(int));

		public const int HeaderLength = 2;
		public const int DefaultSequenceIncrement = 1;
		public const int DataVersionStart = 0;
		public const int DefaultStackSize = 1000000;
		public const int MaxStackAllocSize = DefaultStackSize / 4;
		public const byte PageTypeData = 1;
		public const ushort PageVersion = 1;
	}
}
