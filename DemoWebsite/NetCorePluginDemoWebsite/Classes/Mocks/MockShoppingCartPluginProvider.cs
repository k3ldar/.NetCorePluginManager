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
 *  Product:  Login Plugin
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
using System.Linq;

using Middleware;
using Middleware.Products;
using Middleware.ShoppingCart;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class MockShoppingCartPluginProvider : IShoppingCartProvider, IShoppingCartService
    {
        #region Private Members

        private static readonly CacheManager _cartCacheManager = new CacheManager("Shopping Carts", new TimeSpan(0, 20, 0), true);
        private static long _basketId = 0;
        private readonly IProductProvider _productProvider;
        private const string _encryptionKey = "FPAsdjn;casiicdkumjf4d4fjp0w45eir kc";

        #endregion Private Members

        #region Constructors

        public MockShoppingCartPluginProvider(IProductProvider productProvider)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
            _basketId = DateTime.Now.ToFileTimeUtc();
        }

        #endregion Constructors

        #region IShoppingCartProvider Methds

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
                throw new ArgumentOutOfRangeException(nameof(cost));

            if (shoppingCart.Id == 0 && userSession.UserBasketId != shoppingCart.Id)
                shoppingCart.ResetShoppingCartId(userSession.UserBasketId);

            if (shoppingCart.Id == 0)
                shoppingCart.ResetShoppingCartId(++_basketId);

            if (userSession.UserBasketId != shoppingCart.Id)
                userSession.UserBasketId = shoppingCart.Id;

            string cacheName = $"Cart {shoppingCart.Id}";

            CacheItem basket = _cartCacheManager.Get(cacheName);

            if (basket == null)
            {
                basket = new CacheItem(cacheName, shoppingCart);
                _cartCacheManager.Add(cacheName, basket, true);
            }

            ShoppingCartDetail cart = basket.Value as ShoppingCartDetail;

            cart.Add(product, count);

            return userSession.UserBasketId;
        }

        public ShoppingCartDetail GetDetail(in long shoppingCartId)
        {
            if (shoppingCartId == 0)
                throw new ArgumentOutOfRangeException(nameof(shoppingCartId));

            string basketCache = $"Cart Summary {shoppingCartId}";

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
                    "Test Coupon", items, requiresShipping);
                cacheItem = new CacheItem(basketCache, cartDetail);
                _cartCacheManager.Add(basketCache, cacheItem, true);
            }

            return (ShoppingCartDetail)cacheItem.Value;
        }

        public bool ValidateVoucher(in ShoppingCartSummary cartSummary, in string voucher, in long userId)
        {
            return false;
        }

        #endregion IShoppingCartProvider Methods

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
    }
}
