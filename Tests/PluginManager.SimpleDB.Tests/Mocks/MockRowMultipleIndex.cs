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
 *  Product:  SimpleDB.Tests
 *  
 *  File: MockRowMultipleIndex.cs
 *
 *  Purpose:  MockRow for SimpleDB storage with named index
 *
 *  Date        Name                Reason
 *  20/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

namespace SimpleDB.Tests.Mocks
{
	[ExcludeFromCodeCoverage]
	[Table("MockTableIndex", cachingStrategy: CachingStrategy.Memory)]
	public class MockRowMultipleIndex : TableRowDefinition
	{
		private string _name;
		private int _index;

		[UniqueIndex("TestIndex")]
		public string Name
		{
			get => _name;

			set
			{
				_name = value;
				Update();
			}
		}

		[UniqueIndex("TestIndex")]
		public int Index
		{
			get => _index;

			set
			{
				_index = value;
				Update();
			}
		}
	}
}
