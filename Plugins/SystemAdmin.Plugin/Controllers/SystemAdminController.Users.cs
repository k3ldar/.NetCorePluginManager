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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SystemAdminController.Users.cs
 *
 *  Purpose:  User specific methods for system admin
 *
 *  Date        Name                Reason
 *  08/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Middleware.Users;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Controllers
{
    public partial class SystemAdminController
    {
        [HttpGet]
        public JsonResult UserSearch(BootgridRequestData model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            BootgridResponseData<SearchUser> Result = new()
			{
                rows = _userSearch.GetUsers(model.current, model.rowCount, model.searchPhrase, ""),
                current = model.current,
                rowCount = model.rowCount
            };

            Result.total = Result.rows.Count();

            return new JsonResult(Result);
        }
    }
}

#pragma warning restore CS1591