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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: StockProvider.cs
 *
 *  Purpose:  IStockProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;
using Middleware.Products;
using Middleware.ShoppingCart;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal sealed class StockProvider : IStockProvider
    {
        #region Private Members

        private readonly ISimpleDBOperations<StockDataRow> _stock;

        #endregion Private Members

        #region Constructors

        public StockProvider(ISimpleDBOperations<StockDataRow> stock)
        {
            _stock = stock ?? throw new ArgumentNullException(nameof(stock));
        }

        #endregion Constructors

        #region IStockProvider Methods

        public void GetStockAvailability(in Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            GetStockAvailability(new List<Product>() { product });
        }

        public void GetStockAvailability(in List<Product> productList)
        {
            if (productList == null)
                throw new ArgumentNullException(nameof(productList));

            IReadOnlyList<StockDataRow> productStock = _stock.Select();

            productList.ForEach(p =>
            {
                StockDataRow stockDataRow = productStock.Where(ps => ps.ProductId.Equals(p.Id)).FirstOrDefault();

                if (stockDataRow == null)
                    p.SetCurrentStockLevel(0);
                else
                    p.SetCurrentStockLevel(stockDataRow.StockAvailability);
            });
        }

        public void GetStockAvailability(in ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException(nameof(shoppingCartItem));

            GetStockAvailability(new List<ShoppingCartItem>() { shoppingCartItem });
        }

        public void GetStockAvailability(in List<ShoppingCartItem> shoppingCartItemList)
        {
            if (shoppingCartItemList == null)
                throw new ArgumentNullException(nameof(shoppingCartItemList));

            IReadOnlyList<StockDataRow> productStock = _stock.Select();

            shoppingCartItemList.ForEach(sci =>
            {
                StockDataRow stockDataRow = productStock.Where(ps => ps.ProductId.Equals(sci.ItemId)).FirstOrDefault();

                if (stockDataRow == null)
                    sci.SetCurrentStockLevel(0);
                else
                    sci.SetCurrentStockLevel(stockDataRow.StockAvailability);
            });
        }

        #endregion IStockProvider Methods
    }
}
