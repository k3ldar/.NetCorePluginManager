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
 *  File: UserSearch.cs
 *
 *  Purpose:  IUserSearch for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;

using Middleware.Users;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class UserSearch : IUserSearch
    {
        public List<SearchUser> GetUsers(in int pageNumber, in int pageSize, string searchField, string searchOrder)
        {
            return new List<SearchUser>()
            {
                new SearchUser(1, "Joe Bloggs", "joe@blogs.com"),
                new SearchUser(2, "Jane Doe", "jane@doe.com"),
                new SearchUser(3, "John Doe", "john@doe.com"),
            };
        }
    }
}
