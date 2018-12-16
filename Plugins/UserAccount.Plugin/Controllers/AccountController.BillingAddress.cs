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

using UserAccount.Plugin.Models;

using Middleware;
using static Middleware.Enums;

namespace UserAccount.Plugin.Controllers
{
    public partial class AccountController
    {
        #region Public Controller Methods

		[HttpGet]
		public IActionResult BillingAddress()
        {
            Address billingAddress = _accountProvider.GetBillingAddress(UserId());

            if (billingAddress == null)
                throw new InvalidOperationException(nameof(billingAddress));

            BillingAddressViewModel model = new BillingAddressViewModel();
            PrepareBillingAddressModel(ref model, billingAddress);

            return View(model);
        }

        [HttpPost]
        public IActionResult BillingAddress(BillingAddressViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            ValidateBillingAddressModel(ref model);

			if (ModelState.IsValid)
            {
                Address billingAddress = new Address(model.BusinessName, model.AddressLine1,
                    model.AddressLine2, model.AddressLine3, model.City, model.County, model.Postcode, model.Country);

                if (_accountProvider.SetBillingAddress(UserId(), billingAddress))
                {
                    TempData["growl"] = "Billing address successfully updated";
                    return RedirectToAction("Index", "Account");
                }

                ModelState.AddModelError(String.Empty, "Failed to update billing address");
            }

            return View(model);
        }

        #endregion Public Controller Methods

        #region Private Methods


        private void ValidateBillingAddressModel(ref BillingAddressViewModel model)
        {
            AddressOptions addressOptions = _accountProvider.GetAddressOptions();

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
        }

        private void PrepareBillingAddressModel(ref BillingAddressViewModel model, in Address billingAddress)
        {
            AddressOptions addressOptions = _accountProvider.GetAddressOptions();

            model.ShowAddressLine1 = addressOptions.HasFlag(AddressOptions.AddressLine1Show);
            model.AddressLine1 = billingAddress.AddressLine1;
            model.ShowAddressLine2 = addressOptions.HasFlag(AddressOptions.AddressLine2Show);
            model.AddressLine3 = billingAddress.AddressLine3;
            model.ShowAddressLine3 = addressOptions.HasFlag(AddressOptions.AddressLine3Show);
            model.AddressLine3 = billingAddress.AddressLine3;
            model.ShowBusinessName = addressOptions.HasFlag(AddressOptions.BusinessNameShow);
            model.ShowCity = addressOptions.HasFlag(AddressOptions.CityShow);
            model.City = billingAddress.City;
            model.ShowCounty = addressOptions.HasFlag(AddressOptions.CountyShow);
            model.County = billingAddress.County;
            model.ShowPostcode = addressOptions.HasFlag(AddressOptions.PostCodeShow);
            model.Postcode = billingAddress.Postcode;
            model.Country = billingAddress.Country;
        }

        #endregion Private Methods
    }
}
