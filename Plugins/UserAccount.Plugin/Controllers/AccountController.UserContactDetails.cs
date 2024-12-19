/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: AccountController.UserContactDetails.cs
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

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
#pragma warning disable CS1591, IDE0017

	public partial class AccountController
	{
		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.MyMemberDetails), nameof(AccountController), nameof(Index))]
		public IActionResult UserContactDetails()
		{
			if (_accountProvider.GetUserAccountDetails(UserId(), out string firstName, out string lastName, out string email,
				out bool emailConfirmed, out string telephone, out bool telephoneConfirmed))
			{
				UserContactDetailsViewModel model = new(GetModelData(),
					firstName, lastName, email, emailConfirmed, telephone, telephoneConfirmed,
					_accountProvider.GetAddressOptions(AddressOption.Billing).HasFlag(AddressOptions.TelephoneShow));

				model.Breadcrumbs = GetBreadcrumbs();
				model.CartSummary = GetCartSummary();

				return View(model);
			}

			throw new InvalidOperationException("Unable to retrieve account details");
		}

		[HttpPost]
		public IActionResult UserContactDetails(UserContactDetailsViewModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			AddressOptions addressOptions = _accountProvider.GetAddressOptions(AddressOption.Billing);

			if (addressOptions.HasFlag(AddressOptions.TelephoneMandatory) && String.IsNullOrEmpty(model.Telephone))
				ModelState.AddModelError($"{nameof(model.Telephone)}", Languages.LanguageStrings.InvalidTelephoneNumber);

			if (ModelState.IsValid)
			{
				if (_accountProvider.SetUserAccountDetails(UserId(), model.FirstName, model.LastName, model.Email, model.Telephone))
				{
					GrowlAdd(Languages.LanguageStrings.ContactDetailsUpdated);
					return RedirectToAction(nameof(Index), "Account");
				}

				ModelState.AddModelError(String.Empty, Languages.LanguageStrings.FailedToUpdateAccount);
			}

			model.Breadcrumbs = GetBreadcrumbs();
			model.CartSummary = GetCartSummary();

			return View(model);
		}
	}

#pragma warning restore CS1591, IDE0017
}
