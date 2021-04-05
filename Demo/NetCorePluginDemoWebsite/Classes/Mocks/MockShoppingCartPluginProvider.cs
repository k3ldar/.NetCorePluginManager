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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockShoppingCartPluginProvider.cs
 *
 *  Purpose:  Mock IProductProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Orders;
using Middleware.Products;
using Middleware.ShoppingCart;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable IDE1006

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
    public sealed class MockShoppingCartPluginProvider : IShoppingCartProvider, IShoppingCartService, IDisposable
    {
        #region Private Members

        private static readonly CacheManager _cartCacheManager = new CacheManager("Shopping Carts", new TimeSpan(0, 20, 0), true);
        private static bool _cartHookedUp;
        private static long _basketId = 0;
        private readonly IProductProvider _productProvider;
        private readonly IAccountProvider _accountProvider;
        private const string _encryptionKey = "FPAsdjn;casiicdkumjf4d4fjp0w45eir kc";

        #endregion Private Members

        #region Constructors

        public MockShoppingCartPluginProvider(IProductProvider productProvider, IAccountProvider accountProvider)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
            _accountProvider = accountProvider ?? throw new ArgumentNullException(nameof(accountProvider));

            _basketId = DateTime.Now.ToFileTimeUtc();

            lock (this)
            {
                if (!_cartHookedUp)
                {
                    _cartCacheManager.ItemNotFound += cartCacheManager_ItemNotFound;
                    _cartHookedUp = true;
                }
            }
        }

        #endregion Constructors

        #region IShoppingCartProvider Methds

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Intended for developers not end users")]
        public long AddToCart(in UserSession userSession, in ShoppingCartSummary shoppingCart,
            in Product product, in int count)
        {
            if (userSession == null)
                throw new ArgumentNullException(nameof(userSession));

            if (shoppingCart == null)
                throw new ArgumentNullException(nameof(shoppingCart));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            decimal cost = product.RetailPrice;

            if (cost < 0)
                throw new InvalidOperationException($"{nameof(cost)} value must be greater than zero");

            ShoppingCartDetail cartDetail = null;

            if (shoppingCart.Id == 0)
            {
                // create a new cart
                shoppingCart.ResetShoppingCartId(++_basketId);
                cartDetail = new ShoppingCartDetail(shoppingCart.Id,
                    0, 0, 20, 0, 0, shoppingCart.Culture, String.Empty,
                    new List<ShoppingCartItem>(), false, "GBP");
            }

            if (userSession.UserBasketId != shoppingCart.Id)
                userSession.UserBasketId = shoppingCart.Id;

            string cacheName = $"Cart {shoppingCart.Id}";

            CacheItem basket = _cartCacheManager.Get(cacheName);

            if (basket == null)
            {
                basket = new CacheItem(cacheName, cartDetail);
                _cartCacheManager.Add(cacheName, basket, true);
            }

            if (shoppingCart.Id == 0 && userSession.UserBasketId != shoppingCart.Id)
                shoppingCart.ResetShoppingCartId(userSession.UserBasketId);

            ShoppingCartDetail cart = basket.Value as ShoppingCartDetail;

            cart.Add(product, count);

            return userSession.UserBasketId;
        }

        public ShoppingCartDetail GetDetail(in long shoppingCartId)
        {
            if (shoppingCartId == 0)
                throw new ArgumentOutOfRangeException(nameof(shoppingCartId));

            string basketCache = $"Cart {shoppingCartId}";

            CacheItem cacheItem = _cartCacheManager.Get(basketCache);

            if (cacheItem == null)
            {
                List<ShoppingCartItem> items = new List<ShoppingCartItem>();

                Product product = _productProvider.GetProducts(1, 10000).Where(p => p.RetailPrice > 0).FirstOrDefault();
                bool requiresShipping = !product.IsDownload;

                items.Add(new ShoppingCartItem(product.Id, 1, product.RetailPrice, product.Name,
                    product.Description.Substring(0, Shared.Utilities.CheckMinMax(product.Description.Length, 0, 49)),
                    product.Sku, product.Images, product.IsDownload, product.AllowBackorder, String.Empty));
                ShoppingCartDetail cartDetail = new ShoppingCartDetail(shoppingCartId, 1,
                    product.RetailPrice, 20, 0, 10, System.Threading.Thread.CurrentThread.CurrentUICulture,
                    "Test Coupon", items, requiresShipping, "GBP");
                cacheItem = new CacheItem(basketCache, cartDetail);
                _cartCacheManager.Add(basketCache, cacheItem, true);
            }

            return (ShoppingCartDetail)cacheItem.Value;
        }

        public bool ValidateVoucher(in ShoppingCartSummary cartSummary, in string voucher, in long userId)
        {
            return false;
        }

        public bool ConvertToOrder(in ShoppingCartSummary cartSummary, in long userId, out Order order)
        {
            if (cartSummary == null)
                throw new ArgumentNullException(nameof(cartSummary));

            if (userId == 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            ShoppingCartDetail cartDetail = GetDetail(cartSummary.Id);

            Order latest = _accountProvider.OrdersGet(userId).OrderByDescending(o => o.Id).FirstOrDefault();
            DeliveryAddress shippingAddress = _accountProvider.GetDeliveryAddress(userId, cartDetail.DeliveryAddressId);
            List<OrderItem> items = new List<OrderItem>();

            foreach (ShoppingCartItem item in cartDetail.Items)
            {
                items.Add(new OrderItem(item.Id, item.Name, item.ItemCost, 20, item.ItemCount,
                    ItemStatus.Received, DiscountType.None, 0));
            }

            order = new Order(latest.Id + 1, DateTime.Now, cartDetail.Shipping, cartDetail.Culture,
                ProcessStatus.PaymentPending, shippingAddress, items);

            return true;
        }

        #endregion IShoppingCartProvider Methods

        #region IDisposable Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "uses flag")]
        public void Dispose()
        {
            if (_cartHookedUp)
            {
                _cartCacheManager.ItemNotFound -= cartCacheManager_ItemNotFound;
                _cartHookedUp = false;
            }

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Methods

        #region IShoppingCartService Methods

        public ShoppingCartSummary GetSummary(in long shoppingCartId)
        {
            return GetDetail(shoppingCartId);
        }

        public string GetEncryptionKey()
        {
            return _encryptionKey;
        }

        #endregion IShoppingCartService Methods

        #region Private Methods

        private void cartCacheManager_ItemNotFound(object sender, Shared.CacheItemNotFoundArgs e)
        {
            if (!Int64.TryParse(e.Name.Substring(5), out long cartId))
                cartId = ++_basketId;

            ShoppingCartDetail cartDetail = new ShoppingCartDetail(cartId,
                0, 0, 20, 0, 0, System.Threading.Thread.CurrentThread.CurrentCulture,
                String.Empty, new List<ShoppingCartItem>(), false, "GBP");

            Product product = _productProvider.GetProducts(1, 10000).Where(p => p.RetailPrice > 0 && !p.IsDownload).FirstOrDefault();

            if (product != null)
                cartDetail.Add(product, 1);

            e.CachedItem = new CacheItem(e.Name, cartDetail);
        }

        #endregion Private Methods
    }
}
