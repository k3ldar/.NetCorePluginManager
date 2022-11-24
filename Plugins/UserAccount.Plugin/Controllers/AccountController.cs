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

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Licences;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

#pragma warning disable IDE0017, CA1416, CS1591

namespace UserAccount.Plugin.Controllers
{
    [LoggedIn]
    [DenySpider]
    [Subdomain(AccountController.Name)]
    public partial class AccountController : BaseController
    {
        #region Private Members

        private const string Name = "Account";

		private readonly AccountSettings _accountSettings;
		private readonly IAccountProvider _accountProvider;
        private readonly IDownloadProvider _downloadProvider;
        private readonly ILicenceProvider _licenceProvider;
        private readonly IUserCultureChangeProvider _userCultureChanged;
        private readonly ICultureProvider _cultureProvider;
        private readonly bool _blogLoaded;

        private static readonly CacheManager _createAccountCache = new CacheManager("Create Account Cache", new TimeSpan(0, 30, 0));

        #endregion Private Members

        #region Public Static Members

        public static List<Country> Countries { get; private set; }

        public static List<LicenceType> LicenceTypes { get; private set; }

        #endregion Public Static Members

        #region Constructors

        public AccountController(ISettingsProvider settingsProvider, IAccountProvider accountProvider,
            IDownloadProvider downloadProvider, ICountryProvider countryProvider,
            ILicenceProvider licenceProvider, IUserCultureChangeProvider userCultureChanged,
            ICultureProvider cultureProvider, IPluginHelperService pluginHelperService)
        {
			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

            _accountProvider = accountProvider ?? throw new ArgumentNullException(nameof(accountProvider));
            _downloadProvider = downloadProvider ?? throw new ArgumentNullException(nameof(downloadProvider));
            _licenceProvider = licenceProvider ?? throw new ArgumentNullException(nameof(licenceProvider));
            _userCultureChanged = userCultureChanged ?? throw new ArgumentNullException(nameof(userCultureChanged));
            _cultureProvider = cultureProvider ?? throw new ArgumentNullException(nameof(cultureProvider));

            if (pluginHelperService == null)
                throw new ArgumentNullException(nameof(pluginHelperService));

            if (countryProvider == null)
                throw new ArgumentNullException(nameof(countryProvider));

            if (Countries == null)
                Countries = countryProvider.GetVisibleCountries();

            if (LicenceTypes == null)
                LicenceTypes = _licenceProvider.LicenceTypesGet();

			_accountSettings = settingsProvider.GetSettings<AccountSettings>("UserAccount");

			_blogLoaded = pluginHelperService.PluginLoaded("Blog.Plugin.dll", out _);
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.MyAccount))]
        public IActionResult Index()
        {
            string growl = GrowlGet();
            AccountViewModel model = new AccountViewModel(GetModelData(),
				_accountSettings,
                growl ?? String.Empty);
            model.Settings.ShowBlog = _blogLoaded && _accountSettings.ShowBlog;

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            return View(model);
        }

        [HttpGet]
        [LoggedInOut]
        public ActionResult GetCaptchaImage()
        {
            CreateAccountCacheItem createAccountCacheItem = GetCachedCreateAccountAttempt(false);

            if (createAccountCacheItem == null)
                return StatusCode(SharedPluginFeatures.Constants.HtmlResponseBadRequest);

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
            byte upperCharCount = 0;
            byte lowerCharCount = 0;
            byte numberCharCount = 0;
            byte specialCharCount = 0;

            foreach (char c in password)
            {
                if (c >= 65 && c <= 90)
                    upperCharCount++;
                else if (c >= 61 && c <= 122)
                    lowerCharCount++;
                else if (c >= 48 && c <= 57)
                    numberCharCount++;
                else if (_accountSettings.PasswordSpecialCharacters.Contains(c))
                    specialCharCount++;
            }

            return upperCharCount >= _accountSettings.PasswordUppercaseCharCount &&
                lowerCharCount >= _accountSettings.PasswordLowercaseCharCount &&
                numberCharCount >= _accountSettings.PasswordNumberCharCount &&
                specialCharCount >= _accountSettings.PasswordSpecialCharCount;
        }

        #endregion Private Methods
    }
}

#pragma warning restore IDE0017, CA1416, CS1591
