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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: UserSearchProvider.cs
 *
 *  Purpose:  IUserSearch for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;

using Middleware.Users;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class UserSearch : IUserSearch
    {
        private readonly ISimpleDBOperations<UserDataRow> _users;

        public UserSearch(ISimpleDBOperations<UserDataRow> users)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
        }

        public List<SearchUser> GetUsers(in int pageNumber, in int pageSize, string searchField, string searchOrder)
        {
            List<SearchUser> Result = new List<SearchUser>();

            List<UserDataRow> users = _users.Select().Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            users.ForEach(u => Result.Add(new SearchUser(u.Id, u.FullName, u.Email)));

            return Result;
        }
    }
}
