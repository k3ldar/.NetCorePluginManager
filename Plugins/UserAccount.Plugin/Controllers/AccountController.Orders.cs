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
 *  File: AccountController.Orders.cs
 *
 *  Purpose:  Manages Orders
 *
 *  Date        Name                Reason
 *  31/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Middleware.Accounts.Orders;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

#pragma warning disable IDE0017, CS1591

namespace UserAccount.Plugin.Controllers
{
	public partial class AccountController
	{
		#region Public Action Methods

		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.MyOrders), nameof(AccountController), nameof(Index))]
		public ActionResult Orders()
		{
			OrdersViewModel model = new(GetModelData(),
				_accountProvider.OrdersGet(UserId()));

			model.Breadcrumbs = GetBreadcrumbs();
			model.CartSummary = GetCartSummary();

			return View(model);
		}

		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.ViewOrder), nameof(AccountController), nameof(Orders))]
		public ActionResult OrderView(int id)
		{
			Order order = _accountProvider.OrdersGet(UserId()).Find(o => o.Id == id);

			if (order == null)
				return RedirectToAction(nameof(Index));

			OrderViewModel model = new(GetModelData(), order);

			model.Breadcrumbs = GetBreadcrumbs();
			model.CartSummary = GetCartSummary();

			return View(model);
		}

		#endregion Public Action Methods
	}
}

#pragma warning restore IDE0017, CS1591
