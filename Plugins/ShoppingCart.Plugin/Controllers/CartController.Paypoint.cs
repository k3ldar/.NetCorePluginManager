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
 *  Product:  Shopping CartPlugin Plugin
 *  
 *  File: CartController.Paypoint.cs
 *
 *  Purpose:  Paypoint specific methods
 *
 *  Date        Name                Reason
 *  31/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Middleware.Accounts.Orders;

using PluginManager.Abstractions;

using Shared;

using ShoppingCartPlugin.Classes;

#pragma warning disable CS1591

namespace ShoppingCartPlugin.Controllers
{
	public partial class CartController
	{
		public IActionResult Paypoint()
		{
			// valid=false&trans_id=320&code=N&resp_code=10&message=DATA+NOT+CHECKED+%3a+9999&ip=94.195.236.73&cv2avs=DATA+NOT+CHECKED&test_status=true&hash=bf555b3770642db0f80b0
			IQueryCollection keys = HttpContext.Request.Query;

			if (keys["valid"] == "true" || keys["code"] == "A")
			{
				// all worked, create an invoice etc
				int orderID = Utilities.StrToInt(keys["trans_id"], -1);

				Order order = _accountProvider.OrdersGet(GetUserSession().UserID).Find(o => o.Id == orderID);

				_accountProvider.OrderPaid(order, Middleware.PaymentStatus.PaidCard,
					String.Format("{0} : {1} : {2}", keys["ip"], keys["cv2avs"], keys["message"]));

				if (!ValidateHashCode())
				{
					string message = String.Format("Order #{0} had an invalid Hash, this is a potential fraudulant " +
						"transaction.\n\nPlease check Paypoint to confirm payment.", orderID);
					_applicationProvider.Email("Transaction Query - Hash", message);
				}

				return RedirectToAction(nameof(Success));
			}

			return RedirectToAction(nameof(Failed));
		}

#pragma warning disable S6932
		private bool ValidateHashCode()
		{
			bool Result = false;

			string s = Request.Path;
			s += "?";
			s += Request.QueryString.ToString();
			s = s[..s.IndexOf("hash")];

			ISettingsProvider settingsProvider = (ISettingsProvider)HttpContext.RequestServices.GetService(typeof(ISettingsProvider));
			PaypointSettings paypointSettings = settingsProvider.GetSettings<PaypointSettings>(nameof(Paypoint));

			s += paypointSettings.RemotePassword;

			s = Utilities.HashStringMD5(s);

			Result = s == HttpContext.Request.Query["hash"];

			if (!Result && (HttpContext.Request.Query["resp_code"] != "5" || HttpContext.Request.Query["resp_code"] != "10"))
			{
				string message = String.Format("Transaction Failed\n\nValid: {0}\n\nTransID: {1}\n\nCode: {2}\n\n" +
					"Resp Code: {7}\n\nMessage: {3}\n\nIP Addres: {4}\n\nCV2: {5}\n\nHash: {6}\n\nHash2: {8}",
					HttpContext.Request.Query["valid"], HttpContext.Request.Query["trans_id"],
					HttpContext.Request.Query["code"], HttpContext.Request.Query["message"],
					HttpContext.Request.Query["ip"], HttpContext.Request.Query["cv2avs"],
					HttpContext.Request.Query["hash"], HttpContext.Request.Query["resp_code"], s);
				_applicationProvider.Email("Transaction Failed - Hash", message);
			}

			return (Result);
		}
#pragma warning restore S6932
	}
}

#pragma warning restore CS1591