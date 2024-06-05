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
 *  File: AccountController.BillingAddress.cs
 *
 *  Purpose:  Manages billing address
 *
 *  Date        Name                Reason
 *  16/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Accounts;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
#pragma warning disable CS1591

    public partial class AccountController
    {
        #region Public Controller Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.MyDeliveryAddresses), nameof(AccountController), nameof(Index))]
        public IActionResult DeliveryAddress()
        {
            string growl = GrowlGet();
            return View(new DeliveryAddressViewModel(GetModelData(),
                _accountProvider.GetDeliveryAddresses(UserId()), growl));
        }

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.DeliveryAddressAdd), nameof(AccountController), nameof(DeliveryAddress))]
        public IActionResult DeliveryAddressAdd(string returnUrl)
        {
            EditDeliveryAddressViewModel model = new(returnUrl);
            PrepareDeliveryAddressModel(ref model, null);

            return View(model);
        }

        [HttpPost]
        public IActionResult DeliveryAddressAdd(EditDeliveryAddressViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            ValidateDeliveryAddressModel(model);

            if (ModelState.IsValid)
            {
                if (_accountProvider.AddDeliveryAddress(UserId(), new DeliveryAddress(model.AddressId,
                    model.Name, model.AddressLine1, model.AddressLine2, model.AddressLine3, model.City,
                    model.County, model.Postcode, model.Country, model.PostageCost)))
                {
                    GrowlAdd(Languages.LanguageStrings.DeliveryAddressCreated);

                    if (!String.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return new RedirectResult(model.ReturnUrl, false);

                    return new RedirectResult("/Account/DeliveryAddress", false);
                }

                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.FailedToUpdateDeliveryAddress);
            }

            PrepareDeliveryAddressModel(ref model, null);

            return View(model);
        }

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.DeliveryAddressEdit), nameof(AccountController), nameof(DeliveryAddress), HasParams = true)]
        public IActionResult DeliveryAddressEdit(int id)
        {
            DeliveryAddress address = _accountProvider.GetDeliveryAddress(UserId(), id);

            if (address == null)
                return new RedirectResult("/Account/DeliveryAddress", false);

            EditDeliveryAddressViewModel model = new(GetModelData());
            PrepareDeliveryAddressModel(ref model, address);

            return View(model);
        }

        [HttpPost]
        public IActionResult DeliveryAddressEdit(EditDeliveryAddressViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (_accountProvider.GetDeliveryAddress(UserId(), model.AddressId) == null)
                return new RedirectResult("/Account/DeliveryAddress");

            ValidateDeliveryAddressModel(model);

            if (ModelState.IsValid)
            {
                if (_accountProvider.SetDeliveryAddress(UserId(), new DeliveryAddress(model.AddressId,
                    model.Name, model.AddressLine1, model.AddressLine2, model.AddressLine3, model.City,
                    model.County, model.Postcode, model.Country, model.PostageCost)))
                {
                    GrowlAdd(Languages.LanguageStrings.DeliveryAddressUpdated);
                    return new RedirectResult("/Account/DeliveryAddress", false);
                }

                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.FailedToUpdateDeliveryAddress);
            }

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            return View(model);
        }

        [HttpPost]
        public IActionResult DeliveryAddressDelete(string id)
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (int.TryParse(id, out int addressId))
            {
                DeliveryAddress address = _accountProvider.GetDeliveryAddress(UserId(), addressId);

                if (address == null)
                    return new RedirectResult("/Account/DeliveryAddress", false);

                _accountProvider.DeleteDeliveryAddress(UserId(), address);

                GrowlAdd(Languages.LanguageStrings.DeliveryAddressDeleted);

                return StatusCode(SharedPluginFeatures.Constants.HtmlResponseSuccess);
            }

            throw new ArgumentOutOfRangeException(nameof(id));
        }

        #endregion Public Controller Methods

        #region Private Methods


        private void ValidateDeliveryAddressModel(in EditDeliveryAddressViewModel model)
        {
            AddressOptions addressOptions = _accountProvider.GetAddressOptions(AddressOption.Delivery);

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

            if (addressOptions.HasFlag(AddressOptions.BusinessNameMandatory) && String.IsNullOrEmpty(model.Name))
                ModelState.AddModelError(nameof(model.Name), Languages.LanguageStrings.BusinessNameRequired);
        }

        private void PrepareDeliveryAddressModel(ref EditDeliveryAddressViewModel model, in DeliveryAddress deliveryAddress)
        {
            AddressOptions addressOptions = _accountProvider.GetAddressOptions(AddressOption.Delivery);

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            model.ShowAddressLine1 = addressOptions.HasFlag(AddressOptions.AddressLine1Show);
            model.ShowAddressLine2 = addressOptions.HasFlag(AddressOptions.AddressLine2Show);
            model.ShowAddressLine3 = addressOptions.HasFlag(AddressOptions.AddressLine3Show);
            model.ShowName = addressOptions.HasFlag(AddressOptions.BusinessNameShow);
            model.ShowCity = addressOptions.HasFlag(AddressOptions.CityShow);
            model.ShowCounty = addressOptions.HasFlag(AddressOptions.CountyShow);
            model.ShowPostcode = addressOptions.HasFlag(AddressOptions.PostCodeShow);

            if (deliveryAddress != null)
            {
                model.AddressId = deliveryAddress.Id;
                model.AddressLine1 = deliveryAddress.AddressLine1;
                model.AddressLine2 = deliveryAddress.AddressLine2;
                model.AddressLine3 = deliveryAddress.AddressLine3;
                model.City = deliveryAddress.City;
                model.County = deliveryAddress.County;
                model.Postcode = deliveryAddress.Postcode;
                model.Country = deliveryAddress.Country;
            }
        }

        #endregion Private Methods
    }

#pragma warning restore CS1591
}