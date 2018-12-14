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
 *  File: AccountController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
    [LoggedIn]
    public partial class AccountController : BaseController
    {
        #region Private Members

        private readonly ISettingsProvider _settingsProvider;
        private readonly IAccountProvider _accountProvider;

        #endregion Private Members

        #region Constructors

        public AccountController(ISettingsProvider settingsProvider, IAccountProvider accountProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _accountProvider = accountProvider ?? throw new ArgumentNullException(nameof(accountProvider));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        public IActionResult Index()
        {
            string growl = TempData.ContainsKey("growl") ? TempData["growl"].ToString() : String.Empty;
            AccountViewModel model = new AccountViewModel(
                _settingsProvider.GetSettings<AccountSettings>("UserAccount"),
                growl ?? String.Empty);

            return View(model);
        }

        #endregion Public Action Methods
    }
}