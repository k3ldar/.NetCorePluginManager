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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SystemAdminController.Permissions.cs
 *
 *  Purpose:  Permission specific methods for system admin
 *
 *  Date        Name                Reason
 *  06/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

using SystemAdmin.Plugin.Models;

namespace SystemAdmin.Plugin.Controllers
{
    public partial class SystemAdminController
    {
        #region Controller Action Methods

        [HttpGet]
        public IActionResult Permissions()
        {
            return View(new PermissionsModel(GetModelData()));
        }

        [HttpGet]
        public IActionResult GetUserPermissions(long id)
        {
            UserPermissionsViewModel model = new UserPermissionsViewModel(id,
                _claimsProvider.GetClaimsForUser(id), _claimsProvider.GetAllClaims());

            return PartialView("_UserPermissions", model);
        }

        [HttpPost]
        public IActionResult SetUserPermissions(UserPermissionsViewModel model)
        {
            if (String.IsNullOrEmpty(model.SelectedClaims))
                model.SelectedClaims = String.Empty;

            string[] claims = model.SelectedClaims.Split(';', StringSplitOptions.RemoveEmptyEntries);
            _claimsProvider.SetClaimsForUser(model.UserId, claims.ToList());

            return new JsonResult(new { updated = true });
        }

        #endregion Controller Action Methods
    }
}
