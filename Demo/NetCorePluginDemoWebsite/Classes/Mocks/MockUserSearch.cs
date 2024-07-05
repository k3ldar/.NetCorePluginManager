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
 *  Product:  Demo Website
 *  
 *  File: StockProvider.cs
 *
 *  Purpose:  Mock IStockProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  19/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;

using Middleware.Users;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
	[ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
	public class MockUserSearch : IUserSearch
	{
		public List<SearchUser> GetUsers(in int pageNumber, in int pageSize, string searchField, string searchOrder)
		{
			return
			[
				new(1, "Joe Bloggs", "joe@blogs.com"),
				new(2, "Jane Doe", "jane@doe.com"),
				new(3, "John Doe", "john@doe.com"),
			];

		}
	}
}
