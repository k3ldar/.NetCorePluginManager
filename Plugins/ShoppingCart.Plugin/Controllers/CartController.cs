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
 *  Copyright (c) 2019 - 2021 Simon Carter.  All Rights Reserved.
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

using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Orders;
using Middleware.ShoppingCart;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using ShoppingCartPlugin.Models;

#pragma warning disable CS1591

namespace ShoppingCartPlugin.Controllers
{
    [DenySpider]
    [Subdomain(CartController.Name)]
    public partial class CartController : BaseController
    {
        #region Private Members

        private const string Name = "Cart";

        private readonly IShoppingCartProvider _shoppingCartProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IPluginClassesService _pluginClassesService;
        private readonly IApplicationProvider _applicationProvider;
        private readonly IStockProvider _stockProvider;

        #endregion Private Members

        #region Constructors

        public CartController(IShoppingCartProvider shoppingCartProvider, IAccountProvider accountProvider,
            IPluginClassesService pluginClassesService, IStockProvider stockProvider,
            IApplicationProvider applicationProvider)
        {
            _applicationProvider = applicationProvider ?? throw new ArgumentNullException(nameof(applicationProvider));
            _shoppingCartProvider = shoppingCartProvider ?? throw new ArgumentNullException(nameof(shoppingCartProvider));
            _accountProvider = accountProvider ?? throw new ArgumentNullException(nameof(accountProvider));
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));
            _stockProvider = stockProvider ?? throw new ArgumentNullException(nameof(stockProvider));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.ShoppingCart))]
        public IActionResult Index()
        {
            List<BasketItemModel> basketItems = new List<BasketItemModel>();
            ShoppingCartSummary cartSummary = GetCartSummary();
            BasketModel model;

            BaseModelData modelData = GetModelData();

            if (cartSummary.Id != 0)
            {
                ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(cartSummary.Id);
                _stockProvider.GetStockAvailability(cartDetails.Items);

                foreach (ShoppingCartItem item in cartDetails.Items)
                {
                    basketItems.Add(new BasketItemModel(GetModelData(),
                        item.Id, item.Name, item.Description,
                        item.Size, item.SKU, item.ItemCost, (int)item.ItemCount,
                        item.StockAvailability > 500 ? "> 500" : item.StockAvailability.ToString(),
                        item.ItemCount * item.ItemCost, false, item.Images[0]));
                }

                if (TempData.ContainsKey("VoucherError"))
                {
                    ModelState.AddModelError(nameof(VoucherModel.Voucher), Languages.LanguageStrings.VoucherInvalid);
                    TempData.Remove("VoucherError");
                }

                modelData.ReplaceCartSummary(cartSummary);
                model = new BasketModel(modelData, basketItems,
                    cartDetails.CouponCode, cartDetails.RequiresShipping,
                    !String.IsNullOrEmpty(GetUserSession().UserEmail));
            }
            else
            {
                modelData.ReplaceCartSummary(cartSummary);
                model = new BasketModel(modelData, new List<BasketItemModel>(),
                    String.Empty, false, GetUserSession().UserID != 0);
            }

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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

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
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!String.IsNullOrEmpty(model.Voucher))
            {
                if (_shoppingCartProvider.ValidateVoucher(GetCartSummary(), model.Voucher, GetUserSession().UserID))
                    return RedirectToAction(nameof(Index));
            }

            TempData["VoucherError"] = Languages.LanguageStrings.VoucherInvalid;

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Breadcrumb(nameof(Shipping), nameof(CartController), nameof(Index))]
        public IActionResult Shipping()
        {
            ShoppingCartSummary cartSummary = GetCartSummary();
            ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(cartSummary.Id);

            if (!cartDetails.RequiresShipping)
                return RedirectToAction(nameof(Checkout));

            ShippingModel model = new ShippingModel(GetModelData());
            PrepareShippingAddressModel(in model, _accountProvider.GetDeliveryAddresses(GetUserSession().UserID));

            return View(model);
        }

        [HttpPost]
        [Breadcrumb(nameof(Checkout), nameof(CartController), nameof(Shipping))]
        public IActionResult Checkout(int? shippingId)
        {
            CheckoutModel model = new CheckoutModel(GetModelData());
            ShoppingCartDetail cartDetail = _shoppingCartProvider.GetDetail(GetCartSummary().Id);

            if (cartDetail.RequiresShipping && (!shippingId.HasValue || (shippingId.HasValue && shippingId.Value < 1)))
                return RedirectToAction(nameof(Shipping));

            if (!cartDetail.RequiresShipping)
                model.Breadcrumbs.RemoveAt(2);

            Address shippingAddress = _accountProvider.GetDeliveryAddress(GetUserSession().UserID, shippingId.Value);


            if (shippingAddress != null)
                cartDetail.SetDeliveryAddress(shippingAddress);

            PrepareCheckoutModel(model, cartDetail);

            return View(model);
        }

        [HttpPost]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "I deem it to be valid in this context!")]
        public IActionResult ProcessCheckout(CheckoutModel model)
        {
            // get the selected provider
            IPaymentProvider provider = _pluginClassesService.GetPluginClasses<IPaymentProvider>()
                .Where(p => p.UniqueId().CompareTo(model.SelectedProviderId) == 0).FirstOrDefault();

            if (provider == null)
                throw new InvalidOperationException(Middleware.Constants.PaymentProviderNotFound);

            ShoppingCartDetail cartDetails = _shoppingCartProvider.GetDetail(GetCartSummary().Id);
            UserSession session = GetUserSession();

            if (_shoppingCartProvider.ConvertToOrder(cartDetails, session.UserID, out Order order))
            {
                if (provider.Execute(HttpContext.Request, order, PaymentStatus.Unpaid, session, out string providerUrl))
                {
                    session.Tag = order.Id;
                    return Redirect(providerUrl);
                }
            }

            return RedirectToAction(nameof(Failed));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.PaymentFailed), "Cart", nameof(Index))]
        public IActionResult Failed()
        {
            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.ThankyouOrder), "Cart", nameof(Index))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Forms part of route name")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
        public IActionResult Success(string provider)
        {
            UserSession session = GetUserSession();

            BaseModelData modelData = GetModelData();
            modelData.ReplaceCartSummary(new ShoppingCartSummary(0, 0, 0, 0, 0, GetDefaultTaxRate(),
                System.Threading.Thread.CurrentThread.CurrentUICulture,
                SharedPluginFeatures.Constants.CurrencyCodeDefault));

            PaymentSuccessModel model = new PaymentSuccessModel(modelData, (int)session.Tag);

            // clear basket data
            session.Tag = null;
            session.UserBasketId = 0;
            CookieDelete(SharedPluginFeatures.Constants.ShoppingCart);

            return View(model);
        }

        #endregion Public Action Methods

        #region Private Methods

        private void PrepareShippingAddressModel(in ShippingModel model, in List<DeliveryAddress> deliveryAddresses)
        {
            foreach (DeliveryAddress address in deliveryAddresses)
            {
                model.ShippingAddresses.Add(new ShippingAddressModel(GetModelData(),
                    address.AddressId, address.BusinessName, address.AddressLine1, address.AddressLine2,
                    address.AddressLine3, address.City, address.County, address.Postcode,
                    address.Country, address.PostageCost));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "OK in this context")]
        private void PrepareCheckoutModel(in CheckoutModel model, in ShoppingCartDetail cartDetail)
        {

            foreach (IPaymentProvider provider in _pluginClassesService.GetPluginClasses<IPaymentProvider>())
            {
                if (provider.Enabled() && provider.GetCurrencies().Contains(cartDetail.CurrencyCode))
                    model.Providers.Add(provider);
            }

            if (model.Providers.Count == 0)
                throw new InvalidOperationException(Middleware.Constants.PaymentProviderNone);
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591