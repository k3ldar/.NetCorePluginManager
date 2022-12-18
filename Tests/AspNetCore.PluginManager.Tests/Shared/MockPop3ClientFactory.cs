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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockPop3ClientFactory.cs
 *
 *  Purpose:  Mock pop 3 client factory
 *
 *  Date        Name                Reason
 *  16/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Shared.Communication;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
	[ExcludeFromCodeCoverage]
	public class MockPop3ClientFactory : IPop3ClientFactory
	{
		private readonly MockPop3Client _mockPop3Client;

		public MockPop3ClientFactory(MockPop3Client mockPop3Client = null)
		{
			_mockPop3Client = mockPop3Client ?? new();
		}

		public IPop3Client Create()
		{
			return _mockPop3Client;
		}
	}
}
