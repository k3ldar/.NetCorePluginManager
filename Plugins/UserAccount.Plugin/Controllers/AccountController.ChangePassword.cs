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

using Shared.Classes;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
#pragma warning disable CS1591

    public partial class AccountController : BaseController
    {
        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.MyPassword), nameof(AccountController), nameof(Index))]
        public IActionResult ChangePassword()
        {
            UserSession userSession = GetUserSession();

            return View(new ChangePasswordViewModel(GetModelData(), userSession.UserEmail));
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!model.NewPassword.Equals(model.ConfirmNewPassword))
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.PasswordDoesNotMatch);

            if (!ValidatePasswordComplexity(model.NewPassword))
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.PasswordComplexityFailed);

            if (ModelState.IsValid)
            {
                if (_accountProvider.ChangePassword(GetUserSession().UserID, model.NewPassword))
                {
                    GrowlAdd(Languages.LanguageStrings.PasswordUpdated);
                    return RedirectToAction(nameof(Index), "Account");
                }

                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.PasswordUpdateFailed);
            }

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            return View(model);
        }
    }

#pragma warning restore CS1591
}
