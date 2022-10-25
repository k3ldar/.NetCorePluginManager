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
 *  File: MockUserSearch.cs
 *
 *  Purpose:  Mocks IUserSearch for unit testing
 *
 *  Date        Name                Reason
 *  07/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;
using Middleware.Users;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockUserSearch : IUserSearch
    {
        private readonly List<SearchUser> _users;

        public MockUserSearch(List<SearchUser> users)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public MockUserSearch()
            : this(new List<SearchUser>())
        {

        }

        public List<SearchUser> GetUsers(in int pageNumber, in int pageSize, string searchField, string searchOrder)
        {
            return _users;
        }
    }
}
