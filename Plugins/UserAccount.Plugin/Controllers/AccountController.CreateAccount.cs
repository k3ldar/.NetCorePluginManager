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
 *  File: AccountController.CreateAccount.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Mvc;

using Middleware;

using Shared.Classes;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

using static Middleware.Constants;
using static Shared.Utilities;

namespace UserAccount.Plugin.Controllers
{
#pragma warning disable CS1591

    public partial class AccountController
    {
        #region Public Action Methods

        [HttpGet]
        [LoggedOut]
        [Breadcrumb(nameof(Languages.LanguageStrings.CreateAccount), nameof(AccountController), nameof(Index))]
        public IActionResult CreateAccount(string returnUrl)
        {
            CreateAccountViewModel model = new CreateAccountViewModel(GetModelData(), returnUrl);
            PrepareCreateAccountModel(ref model);

            return View(model);
        }

        [HttpPost]
        [LoggedOut]
        public IActionResult CreateAccount(CreateAccountViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            ValidateCreateAccountModel(ref model);

            if (ModelState.IsValid)
            {
                if (_accountProvider.CreateAccount(model.Email, model.FirstName, model.Surname,
                    model.Password, model.Telephone, model.BusinessName, model.AddressLine1,
                    model.AddressLine2, model.AddressLine3, model.City, model.County,
                    model.Postcode, model.Country, out Int64 userId))
                {
                    UserSession session = GetUserSession();

                    if (session != null)
                        session.Login(userId, $"{ValidateUserInput(model.FirstName, ValidationType.Name)} {ValidateUserInput(model.Surname, ValidationType.Name)}", ValidateUserInput(model.Email, ValidationType.Email));

                    if (String.IsNullOrEmpty(model.ReturnUrl))
                        return Redirect("/Account/");
                    else
                        return Redirect(model.ReturnUrl);
                }
            }

            PrepareCreateAccountModel(ref model);

            return View(model);
        }

        #endregion Public Action Methods

        #region Private Members

        private CreateAccountCacheItem GetCachedCreateAccountAttempt(bool createIfNotExist)
        {
            CreateAccountCacheItem Result = null;

            string cacheId = GetIpAddress();

            CacheItem loginCache = _createAccountCache.Get(cacheId);

            if (loginCache != null)
            {
                Result = (CreateAccountCacheItem)loginCache.Value;
            }
            else if (createIfNotExist)
            {
                Result = new CreateAccountCacheItem();
                loginCache = new CacheItem(cacheId, Result);
                _createAccountCache.Add(cacheId, loginCache);
            }

            return (Result);
        }

        private void ValidateCreateAccountModel(ref CreateAccountViewModel model)
        {
            CreateAccountCacheItem createAccountCacheItem = GetCachedCreateAccountAttempt(true);

            if (!String.IsNullOrEmpty(createAccountCacheItem.CaptchaText) && 
				!createAccountCacheItem.CaptchaText.Equals(model.CaptchaText))
			{
				ModelState.AddModelError(String.Empty, Languages.LanguageStrings.CodeNotValid);
            }

            createAccountCacheItem.CreateAttempts++;

            if (createAccountCacheItem.CreateAttempts > 10)
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.TooManyAttempts);

            AddressOptions addressOptions = _accountProvider.GetAddressOptions(AddressOption.Billing);

            if (!ValidatePasswordComplexity(model.Password))
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.PasswordComplexityFailed);

            if (!model.Password.Equals(model.ConfirmPassword))
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.PasswordDoesNotMatch);

            if (addressOptions.HasFlag(AddressOptions.AddressLine1Mandatory) && String.IsNullOrEmpty(model.AddressLine1))
                ModelState.AddModelError(nameof(model.AddressLine1), Languages.LanguageStrings.AddressLine1Required);

            if (addressOptions.HasFlag(AddressOptions.AddressLine2Mandatory) && String.IsNullOrEmpty(model.AddressLine2))
                ModelState.AddModelError(nameof(model.AddressLine2), Languages.LanguageStrings.AddressLine2Required);

            if (addressOptions.HasFlag(AddressOptions.AddressLine3Mandatory) && String.IsNullOrEmpty(model.AddressLine3))
                ModelState.AddModelError(nameof(model.AddressLine3), Languages.LanguageStrings.AddressLine3Required);

            if (addressOptions.HasFlag(AddressOptions.CityMandatory) && String.IsNullOrEmpty(model.City))
                ModelState.AddModelError(nameof(model.City), Languages.LanguageStrings.CityRequired);

            if (addressOptions.HasFlag(AddressOptions.CountyMandatory) && String.IsNullOrEmpty(model.County))
                ModelState.AddModelError(nameof(model.County), Languages.LanguageStrings.CountyRequired);

            if (addressOptions.HasFlag(AddressOptions.PostCodeMandatory) && String.IsNullOrEmpty(model.Postcode))
                ModelState.AddModelError(nameof(model.Postcode), Languages.LanguageStrings.PostcodeRequired);

            if (addressOptions.HasFlag(AddressOptions.BusinessNameMandatory) && String.IsNullOrEmpty(model.BusinessName))
                ModelState.AddModelError(nameof(model.BusinessName), Languages.LanguageStrings.BusinessNameRequired);

            if (addressOptions.HasFlag(AddressOptions.TelephoneMandatory) && String.IsNullOrEmpty(model.Telephone))
                ModelState.AddModelError(nameof(model.Telephone), Languages.LanguageStrings.TelephoneRequired);

            createAccountCacheItem.CaptchaText = GetRandomWord(6, CaptchaCharacters);
            model.CaptchaText = String.Empty;
        }

        private void PrepareCreateAccountModel(ref CreateAccountViewModel model)
        {
            AddressOptions addressOptions = _accountProvider.GetAddressOptions(AddressOption.Billing);

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            model.ShowAddressLine1 = addressOptions.HasFlag(AddressOptions.AddressLine1Show);
            model.ShowAddressLine2 = addressOptions.HasFlag(AddressOptions.AddressLine2Show);
            model.ShowAddressLine3 = addressOptions.HasFlag(AddressOptions.AddressLine3Show);
            model.ShowBusinessName = addressOptions.HasFlag(AddressOptions.BusinessNameShow);
            model.ShowCity = addressOptions.HasFlag(AddressOptions.CityShow);
            model.ShowCounty = addressOptions.HasFlag(AddressOptions.CountyShow);
            model.ShowPostcode = addressOptions.HasFlag(AddressOptions.PostCodeShow);
            model.ShowTelephone = addressOptions.HasFlag(AddressOptions.TelephoneShow);

            model.ShowCaptchaImage = true;

            CreateAccountCacheItem createAccountCacheItem = GetCachedCreateAccountAttempt(true);
            createAccountCacheItem.CaptchaText = GetRandomWord(6, CaptchaCharacters);
        }

        #endregion Private Members
    }

#pragma warning restore CS1591
}
