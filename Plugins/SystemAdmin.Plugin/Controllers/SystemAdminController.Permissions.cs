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
 *  File: SystemAdminController.Permissions.cs
 *
 *  Purpose:  Permission specific methods for system admin
 *
 *  Date        Name                Reason
 *  06/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Models;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Controllers
{
    public partial class SystemAdminController
    {
        #region Controller Action Methods

        [HttpGet]
        [Authorize(Policy = Constants.PolicyNameManagePermissions)]
        public IActionResult Permissions()
        {
            return View(new PermissionsModel(GetModelData()));
        }

        [HttpGet]
        [Authorize(Policy = Constants.PolicyNameManagePermissions)]
        public IActionResult GetUserPermissions(long id)
        {
            UserPermissionsViewModel model = new(id,
                _claimsProvider.GetClaimsForUser(id), _claimsProvider.GetAllClaims());

            return PartialView("_UserPermissions", model);
        }

        [HttpPost]
        [Authorize(Policy = Constants.PolicyNameManagePermissions)]
        public IActionResult SetUserPermissions(UserPermissionsViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (String.IsNullOrEmpty(model.SelectedClaims))
                model.SelectedClaims = String.Empty;

            string[] claims = model.SelectedClaims.Split(';', StringSplitOptions.RemoveEmptyEntries);
            _claimsProvider.SetClaimsForUser(model.UserId, claims.ToList());

            return new JsonResult(new { updated = true });
        }

        #endregion Controller Action Methods
    }
}

#pragma warning restore CS1591