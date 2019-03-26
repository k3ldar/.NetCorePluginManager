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
using Middleware.Accounts;
using Middleware.ShoppingCart;

namespace ShoppingCartPlugin.Controllers
{
    public class CartController : BaseController
    {
        #region Private Members

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IShoppingCartProvider _shoppingCartProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IPluginClassesService _pluginClassesService;

        #endregion Private Members

        #region Constructors

        public CartController(IHostingEnvironment hostingEnvironment, IShoppingCartProvider shoppingCartProvider,
            IAccountProvider accountProvider, IPluginClassesService pluginClassesService)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
            _accountProvider = accountProvider ?? throw new ArgumentNullException(nameof(accountProvider));
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.ShoppingCart))]
        public IActionResult Index()
        {
            List<BasketItemModel> basketItems = new List<BasketItemModel>();
            ShoppingCartSummary cartSummary = GetCartSummary();

            ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(cartSummary.Id);

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

            BasketModel model = new BasketModel(GetBreadcrumbs(), cartSummary, basketItems, 
                cartDetails.CouponCode, cartDetails.RequiresShipping, 
                !String.IsNullOrEmpty(GetUserSession().UserEmail));

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

        public IActionResult Shipping()
        {
            HttpContext.Abort();
            ShoppingCartSummary cartSummary = GetCartSummary();
            ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(cartSummary.Id);

            if (!cartDetails.RequiresShipping)
                return RedirectToAction(nameof(CheckOut));

            ShippingModel model = new ShippingModel(GetBreadcrumbs(), GetCartSummary());
            PrepareShippingAddressModel(in model, _accountProvider.GetDeliveryAddresses(GetUserSession().UserID));

            return View(model);
        }

        public IActionResult CheckOut()
        {
            CheckoutModel model = new CheckoutModel();
            ShoppingCartDetail cartDetail = _shoppingCartProvider.GetDetail(GetCartSummary().Id);

            foreach (IPaymentProvider provider in _pluginClassesService.GetPluginClasses<IPaymentProvider>())
            {
                if (provider.GetCurrencies().Contains(cartDetail.CurrencyCode))
                    model.Providers.Add(provider);
            }
           
            return View(model);
        }

        public IActionResult Failed()
        {
            return View();
        }

        public IActionResult Success(string provider)
        {

            return View();
        }

        #endregion Public Action Methods

        #region Private Methods

        private void PrepareShippingAddressModel(in ShippingModel model, in List<DeliveryAddress> deliveryAddresses)
        {
            foreach (DeliveryAddress address in deliveryAddresses)
            {
                model.ShippingAddresses.Add(new ShippingAddressModel(address.AddressId, address.BusinessName,
                    address.AddressLine1, address.AddressLine2, address.AddressLine3, address.City,
                    address.County, address.Postcode, address.Country, address.PostageCost));
            }
        }

        #endregion Private Methods
    }
}