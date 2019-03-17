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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping CartPlugin Plugin
 *  
 *  File: CartController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  06/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

using ShoppingCartPlugin.Models;

using Middleware;
using Middleware.ShoppingCart;

namespace ShoppingCartPlugin.Controllers
{
    public class CartController : BaseController
    {
        #region Private Members

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IShoppingCartProvider _shoppingCartProvider;

        #endregion Private Members

        #region Constructors

        public CartController(IHostingEnvironment hostingEnvironment, IShoppingCartProvider shoppingCartProvider)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.ShoppingCart))]
        public IActionResult Index()
        {
            List<BasketItemModel> basketItems = new List<BasketItemModel>();

            ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(GetCartSummary().Id);

            foreach (ShoppingCartItem item in cartDetails.Items)
            {
                basketItems.Add(new BasketItemModel(item.Id, item.Name, item.Description,
                    item.Size, item.SKU, item.ItemCost, (int)item.ItemCount, null,
                    item.ItemCount * item.ItemCost, false, item.Images[0]));
            }

            if (TempData.ContainsKey("VoucherError"))
            {
                ModelState.AddModelError(nameof(VoucherModel.Voucher), Languages.LanguageStrings.VoucherInvalid);
                TempData.Remove("VoucherError");
            }

            BasketModel model = new BasketModel(GetBreadcrumbs(), GetCartSummary(), basketItems);

            return View(model);
        }

        [Route("/Cart/Delete/{productId}")]
        public IActionResult Delete(int productId)
        {
            ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(GetCartSummary().Id);

            cartDetails.Delete(productId);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Update(UpdateQuantityModel model)
        {
            ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(GetCartSummary().Id);

            ShoppingCartItem item = cartDetails.Items.Where(i => i.Id == model.ProductId).FirstOrDefault();

            if (item != null)
            {
                cartDetails.Update(item.Id, model.Quantity);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AddVoucher(VoucherModel model)
        {
            if (!String.IsNullOrEmpty(model.Voucher))
            {
                if (_shoppingCartProvider.ValidateVoucher(GetCartSummary(), model.Voucher, GetUserSession().UserID))
                    return RedirectToAction(nameof(Index));
            }

            TempData["VoucherError"] = Languages.LanguageStrings.VoucherInvalid;

            return RedirectToAction(nameof(Index));
        }

        #endregion Public Action Methods

        #region Private Methods

        #endregion Private Methods
    }
}