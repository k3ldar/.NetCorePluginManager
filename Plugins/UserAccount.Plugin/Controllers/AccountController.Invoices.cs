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
 *  File: AccountController.Invoices.cs
 *
 *  Purpose:  Manages Invoices
 *
 *  Date        Name                Reason
 *  04/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Middleware.Accounts.Invoices;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
#pragma warning disable CS1591

	public partial class AccountController
	{
		#region Public Action Methods

		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.MyInvoices), nameof(AccountController), nameof(Index))]
		public ActionResult Invoices()
		{
			InvoicesViewModel model = new(GetModelData(),
				_accountProvider.InvoicesGet(UserId()))
			{
				Breadcrumbs = GetBreadcrumbs(),
				CartSummary = GetCartSummary()
			};

			return View(model);
		}

		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.Invoice), nameof(AccountController), nameof(Invoices))]
		public ActionResult InvoiceView(int id)
		{
			Invoice invoice = _accountProvider.InvoicesGet(UserId()).Find(o => o.Id == id);

			if (invoice == null)
				return RedirectToAction(nameof(Index));

			InvoiceViewModel model = new(GetModelData(), invoice)
			{
				Breadcrumbs = GetBreadcrumbs(),
				CartSummary = GetCartSummary()
			};

			return View(model);
		}

		#endregion Public Action Methods
	}

#pragma warning restore CS1591
}
