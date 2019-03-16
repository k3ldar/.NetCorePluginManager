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

            decimal cost = product.LowestPrice;

            if (cost < 0)
                throw new ArgumentOutOfRangeException(nameof(cost));

            if (shoppingCart.Id == 0 && userSession.UserBasketId != shoppingCart.Id)
                shoppingCart.ResetBasketId(userSession.UserBasketId);

            if (shoppingCart.Id == 0)
                shoppingCart.ResetBasketId(++_basketId);

            userSession.UserBasketId = shoppingCart.Id;

            string cacheName = $"Cart {shoppingCart.Id}";

            CacheItem basket = _cartCacheManager.Get(cacheName);

            if (basket == null)
            {
                basket = new CacheItem(cacheName, shoppingCart);
                _cartCacheManager.Add(cacheName, basket, true);
            }

            ShoppingCartSummary cart = (ShoppingCartSummary)basket.Value;

            cart.ResetTotalItems(cart.TotalItems + count);
            cart.ResetTotalCost(cart.TotalCost + cost, cart.Currency);

            return userSession.UserBasketId;
        }

        public ShoppingCartDetail GetDetail()
        {
            throw new NotImplementedException();
        }

        #endregion IShoppingCartProvider Methods

        #region IShoppingCartService Methods

        public ShoppingCartSummary GetSummary(in long basketId)
        {
            if (basketId == 0)
                throw new ArgumentOutOfRangeException(nameof(basketId));

            string basketCache = $"Cart Summary {basketId}";

            CacheItem cacheItem = _cartCacheManager.Get(basketCache);

            if (cacheItem == null)
            {
                Product product = _productProvider.GetProducts(1, 10000).Where(p => p.LowestPrice > 0).FirstOrDefault();
                cacheItem = new CacheItem(basketCache, new ShoppingCartSummary(basketId, 1,
                    product.LowestPrice, System.Threading.Thread.CurrentThread.CurrentUICulture));
                _cartCacheManager.Add(basketCache, cacheItem, true);
            }

            return (ShoppingCartSummary)cacheItem.Value;
        }

        public string GetEncryptionKey()
        {
            return _encryptionKey;
        }

        #endregion IShoppingCartService Methods
    }
}
