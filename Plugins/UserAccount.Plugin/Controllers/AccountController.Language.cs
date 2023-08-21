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
 *  Product:  UserAccount.Plugin
 *  
 *  File: AccountController.Language.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Globalization;

using Microsoft.AspNetCore.Mvc;

using Shared.Classes;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Controllers
{
#pragma warning disable CS1591

    public partial class AccountController
    {
        [HttpPost]
        [LoggedInOut]
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            UserSession userSession = GetUserSession();

            userSession.Culture = culture;
            CultureInfo newCulture = new CultureInfo(culture);

            if (_cultureProvider.IsCultureValid(newCulture))
                _userCultureChanged.CultureChanged(HttpContext, userSession, newCulture);

			string redirectPath = returnUrl ?? "/";

			if (Url.IsLocalUrl(redirectPath))
				return Redirect(redirectPath);

			return RedirectToAction("Index", "Home");
        }
    }

#pragma warning restore CS1591
}
