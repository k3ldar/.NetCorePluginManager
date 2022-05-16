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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockStockProvider.cs
 *
 *  Purpose:  Mock IStockProvider class
 *
 *  Date        Name                Reason
 *  02/12/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Middleware;
using Middleware.Products;
using Middleware.ShoppingCart;

namespace AspNetCore.PluginManager.Tests.Shared
{
    public class MockStockProvider : IStockProvider
    {
        private readonly uint _stockCount;

        public MockStockProvider(uint stockCount)
        {
            _stockCount = stockCount;
        }

        public MockStockProvider()
            : this(5)
        {

        }

        public void GetStockAvailability(in Product product)
        {
            product.SetCurrentStockLevel(_stockCount);
        }

        public void GetStockAvailability(in List<Product> productList)
        {
            productList.ForEach(p => p.SetCurrentStockLevel(_stockCount));
        }

        public void GetStockAvailability(in ShoppingCartItem shoppingCartItem)
        {
            shoppingCartItem.SetCurrentStockLevel(_stockCount);
        }

        public void GetStockAvailability(in List<ShoppingCartItem> shoppingCartItemList)
        {
            shoppingCartItemList.ForEach(sc => sc.SetCurrentStockLevel(_stockCount));
        }
    }
}
