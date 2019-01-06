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

using SharedPluginFeatures;

using Shared.Classes;
using static Shared.Utilities;

using UserAccount.Plugin.Models;

using Middleware;
using static Middleware.Constants;

namespace UserAccount.Plugin.Controllers
{
    public partial class AccountController
    {
        #region Public Action Methods

        [HttpGet]
        [LoggedOut]
        public IActionResult CreateAccount()
        {
            CreateAccountViewModel model = new CreateAccountViewModel();
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
                        session.Login(userId, $"{model.FirstName} {model.Surname}", model.Email);

                    return Redirect("/Account/");
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
            else if (createIfNotExist && loginCache == null)
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

            if (!String.IsNullOrEmpty(createAccountCacheItem.CaptchaText))
            {
                if (!createAccountCacheItem.CaptchaText.Equals(model.CaptchaText))
                    ModelState.AddModelError(String.Empty, "Invalid Validation Code");
            }

            createAccountCacheItem.CreateAttempts++;

            if (createAccountCacheItem.CreateAttempts > 10)
                ModelState.AddModelError(String.Empty, "Too many attempts, please wait 30 minutes and try again");

            AddressOptions addressOptions = _accountProvider.GetAddressOptions();

            if (!ValidatePasswordComplexity(model.Password))
                ModelState.AddModelError(String.Empty, "Password does not match complexity rules.");

            if (!model.Password.Equals(model.ConfirmPassword))
                ModelState.AddModelError("", "Confirm password must match Password");

            if (addressOptions.HasFlag(AddressOptions.AddressLine1Mandatory) && String.IsNullOrEmpty(model.AddressLine1))
                ModelState.AddModelError(nameof(model.AddressLine1), "Address line 1 is required");

            if (addressOptions.HasFlag(AddressOptions.AddressLine2Mandatory) && String.IsNullOrEmpty(model.AddressLine2))
                ModelState.AddModelError(nameof(model.AddressLine2), "Address line 2 is required");

            if (addressOptions.HasFlag(AddressOptions.AddressLine3Mandatory) && String.IsNullOrEmpty(model.AddressLine3))
                ModelState.AddModelError(nameof(model.AddressLine3), "Address line 3 is required");

            if (addressOptions.HasFlag(AddressOptions.CityMandatory) && String.IsNullOrEmpty(model.City))
                ModelState.AddModelError(nameof(model.City), "City is required");

            if (addressOptions.HasFlag(AddressOptions.CountyMandatory) && String.IsNullOrEmpty(model.County))
                ModelState.AddModelError(nameof(model.County), "County is required");

            if (addressOptions.HasFlag(AddressOptions.PostCodeMandatory) && String.IsNullOrEmpty(model.Postcode))
                ModelState.AddModelError(nameof(model.Postcode), "Postcode is required");

            if (addressOptions.HasFlag(AddressOptions.BusinessNameMandatory) && String.IsNullOrEmpty(model.BusinessName))
                ModelState.AddModelError(nameof(model.BusinessName), "Business Name is required");

            if (addressOptions.HasFlag(AddressOptions.TelephoneMandatory) && String.IsNullOrEmpty(model.Telephone))
                ModelState.AddModelError(nameof(model.Telephone), "Telephone is required");

            createAccountCacheItem.CaptchaText = GetRandomWord(6, CaptchaCharacters);
            model.CaptchaText = String.Empty;
        }

        private void PrepareCreateAccountModel(ref CreateAccountViewModel model)
        {
            AddressOptions addressOptions = _accountProvider.GetAddressOptions();

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
}
