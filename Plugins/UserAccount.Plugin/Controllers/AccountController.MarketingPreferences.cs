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
 *  File: AccountController.MarketingPreferences.cs
 *
 *  Purpose:  Manages billing address
 *
 *  Date        Name                Reason
 *  30/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Mvc;

using UserAccount.Plugin.Models;

using Middleware;
using Middleware.Accounts;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Controllers
{
    public partial class AccountController
    {
        #region Public Action Methods

		[HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.MarketingPreferences), nameof(AccountController), nameof(Index))]
        public IActionResult MarketingPreferences()
        {
            MarketingPreferencesViewModel model = new MarketingPreferencesViewModel(
                GetBreadcrumbs(), GetCartSummary());
            PrepareMarketingModel(ref model, _accountProvider.GetMarketingPreferences(UserId()));

            return View(model);
        }

		[HttpPost]
		public IActionResult MarketingPreferences(MarketingPreferencesViewModel model)
        {
			if (ModelState.IsValid)
            {
                Marketing marketing = new Marketing(model.EmailOffers, model.TelephoneOffers,
                    model.SMSOffers, model.PostalOffers);

                if (_accountProvider.SetMarketingPreferences(UserId(), marketing))
                {
                    GrowlAdd(Languages.LanguageStrings.MarketingUpdated);
                    return RedirectToAction(nameof(Index));
                }
            }

            PrepareMarketingModel(ref model, null);

            return View();
        }

        #endregion Public Action Methods

        #region Private Methods

		private void PrepareMarketingModel(ref MarketingPreferencesViewModel model, in Marketing marketing)
        {
            MarketingOptions options = _accountProvider.GetMarketingOptions();

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            model.ShowEmail = options.HasFlag(MarketingOptions.ShowEmail);
            model.ShowPostal = options.HasFlag(MarketingOptions.ShowPostal);
            model.ShowSMS = options.HasFlag(MarketingOptions.ShowSMS);
            model.ShowTelephone = options.HasFlag(MarketingOptions.ShowTelephone);

			if (marketing != null)
            {
                model.EmailOffers = marketing.EmailOffers;
                model.PostalOffers = marketing.PostalOffers;
                model.SMSOffers = marketing.SMSOffers;
                model.TelephoneOffers = marketing.TelephoneOffers;
            }
        }

        #endregion Private Methods
    }
} 
