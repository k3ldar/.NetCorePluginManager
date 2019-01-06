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
using System.IO;
using Microsoft.AspNetCore.Mvc;

using Shared.Classes;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Licences;

namespace UserAccount.Plugin.Controllers
{
    [LoggedIn]
    public partial class AccountController : BaseController
    {
        #region Private Members

        private readonly ISettingsProvider _settingsProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IDownloadProvider _downloadProvider;
        private readonly ILicenceProvider _licenceProvider;

        private static readonly CacheManager _createAccountCache = new CacheManager("Create Account Cache", new TimeSpan(0, 30, 0));

        #endregion Private Members

        #region Public Static Members

        public static List<Country> Countries { get; private set; }

        public static List<LicenceType> LicenceTypes { get; private set; }

        #endregion Public Static Members

        #region Constructors

        public AccountController(ISettingsProvider settingsProvider, IAccountProvider accountProvider, 
            IDownloadProvider downloadProvider, ICountryProvider countryProvider, 
            ILicenceProvider licenceProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _accountProvider = accountProvider ?? throw new ArgumentNullException(nameof(accountProvider));
            _downloadProvider = downloadProvider ?? throw new ArgumentNullException(nameof(downloadProvider));
            _licenceProvider = licenceProvider ?? throw new ArgumentNullException(nameof(licenceProvider));

            if (countryProvider == null)
                throw new ArgumentNullException(nameof(countryProvider));

            if (Countries == null)
                Countries = countryProvider.GetVisibleCountries();

            if (LicenceTypes == null)
                LicenceTypes = _licenceProvider.LicenceTypesGet();
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        public IActionResult Index()
        {
            string growl = GrowlGet();
            AccountViewModel model = new AccountViewModel(
                _settingsProvider.GetSettings<AccountSettings>("UserAccount"),
                growl ?? String.Empty);

            return View(model);
        }

        [HttpGet]
        [LoggedInOut]
        public ActionResult GetCaptchaImage()
        {
            CreateAccountCacheItem createAccountCacheItem = GetCachedCreateAccountAttempt(false);

            if (createAccountCacheItem == null)
                return StatusCode(400);

            CaptchaImage ci = new CaptchaImage(createAccountCacheItem.CaptchaText, 240, 60, "Century Schoolbook");
            try
            {
                // Write the image to the response stream in JPEG format.
                using (MemoryStream ms = new MemoryStream())
                {
                    ci.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    return File(ms.ToArray(), "image/png");
                }
            }
            catch (Exception err)
            {
                if (!err.Message.Contains("Specified method is not supported."))
                    throw;
            }
            finally
            {
                ci.Dispose();
            }

            return (null);
        }

        #endregion Public Action Methods

        #region Private Methods

        private bool ValidatePasswordComplexity(in string password)
        {
            return true;
        }

        #endregion Private Methods
    }
}