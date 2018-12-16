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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: AccountController.ChangePassword.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

using Shared.Classes;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
    public partial class AccountController : BaseController
    {
		[HttpGet]
		public IActionResult ChangePassword()
        {
            UserSession userSession = GetUserSession();

            return View(new ChangePasswordViewModel(userSession.UserEmail));
        }

		[HttpPost]
		public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!model.NewPassword.Equals(model.ConfirmNewPassword))
                ModelState.AddModelError(String.Empty, "New password and confirm new password do not match");

            if (!ValidatePasswordComplexity(model.NewPassword))
                ModelState.AddModelError(String.Empty, "Password does not match complexity rules.");

            if (ModelState.IsValid)
            {
                if (_accountProvider.ChangePassword(GetUserSession().UserID, model.NewPassword))
                {
                    TempData["growl"] = "Password successfully updated";
                    return RedirectToAction("Index", "Account");
                }

                ModelState.AddModelError(String.Empty, "Failed to change password");
            }

            return View(model);
        }
    }
}
