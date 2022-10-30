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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: StockProvider.cs
 *
 *  Purpose:  Mock IStockProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  19/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;
using Middleware.Products;
using Middleware.ShoppingCart;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
    public sealed class MockStockProvider : IStockProvider
    {
        #region Private Members

        private readonly Random _random;

        #endregion Private Members

        #region Constructors

        public MockStockProvider()
        {
            _random = new Random(DateTime.Now.Millisecond);
        }

        #endregion Constructors

        #region IStockProvider Methods

        public void GetStockAvailability(in Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            product.SetCurrentStockLevel(Convert.ToUInt32(_random.Next(1000)));
        }

        public void GetStockAvailability(in List<Product> productList)
        {
            if (productList == null)
                throw new ArgumentNullException(nameof(productList));

            productList.ForEach(p => p.SetCurrentStockLevel(Convert.ToUInt32(_random.Next(1000))));
        }

        public void GetStockAvailability(in ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            shoppingCartItem.SetCurrentStockLevel(Convert.ToUInt32(_random.Next(1000)));
        }

        public void GetStockAvailability(in List<ShoppingCartItem> shoppingCartItemList)
        {
            if (shoppingCartItemList == null)
                throw new ArgumentNullException(nameof(shoppingCartItemList));

            shoppingCartItemList.ForEach(p => p.SetCurrentStockLevel(Convert.ToUInt32(_random.Next(1000))));
        }

        #endregion IStockProvider Methods
    }
}
