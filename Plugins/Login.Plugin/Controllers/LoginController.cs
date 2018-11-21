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
 *  Product:  Login Plugin
 *  
 *  File: LoginController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  19/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using Shared.Classes;

using SharedPluginFeatures;

using LoginPlugin.Classes;

using LoginPlugin.Models;

namespace LoginPlugin.Controllers
{
    public class LoginController : BaseController
    {
        #region Private Members

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILoginProvider _loginProvider;

        private static readonly CacheManager _loginCache = new CacheManager("LoginCache", new TimeSpan(0, 30, 0));

        #endregion Private Members

        #region Constructors

        public LoginController(IHostingEnvironment hostingEnvironment, ILoginProvider loginProvider)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _loginProvider = loginProvider ?? throw new ArgumentNullException(nameof(loginProvider));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        public IActionResult Index(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel(String.IsNullOrEmpty(returnUrl) ? String.Empty : returnUrl);

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            string ipAddress = GetIpAddress();

            CacheItem loginCache = _loginCache.Get(ipAddress);

            if (loginCache == null)
            {
                loginCache = new CacheItem(ipAddress, new LoginCacheItem());
                _loginCache.Add(ipAddress, loginCache);
            }

            LoginCacheItem loginCacheItem = (LoginCacheItem)loginCache.Value;
            loginCacheItem.LoginAttempts++;

            switch (_loginProvider.Login(model.Username, model.Password, ipAddress, loginCacheItem.LoginAttempts))
            {
                case Enums.LoginResult.Success:
                    return (View());

                case Enums.LoginResult.AccountLocked:
                    return (RedirectToAction("AccountLocked", model.Username));

                case Enums.LoginResult.PasswordChangeRequired:
                    return (RedirectToAction("UpdatePassword", model.Username));

                case Enums.LoginResult.InvalidCredentials:
                    ModelState.AddModelError(String.Empty, "Invalid username or password");
                    break;
            }

            return (View(model));
        }

        [HttpGet]
        public IActionResult AccountLocked(string username)
        {
            AccountLockedViewModel model = new AccountLockedViewModel()
            {
                Username = String.IsNullOrEmpty(username) ? String.Empty : username
            };

            return (View(model));
        }

        [HttpPost]
        public IActionResult AccountLocked(AccountLockedViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return (View(model));
        }

        [HttpGet]
        public IActionResult UpdatePassword(string username)
        {
            AccountLockedViewModel model = new AccountLockedViewModel()
            {
                Username = String.IsNullOrEmpty(username) ? String.Empty : username
            };

            return (View(model));
        }

        [HttpPost]
        public IActionResult UpdatePassword(UpdatePasswordViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return (View(model));
        }

        #endregion Public Action Methods

        #region Private Methods

        #endregion Private Methods
    }
}