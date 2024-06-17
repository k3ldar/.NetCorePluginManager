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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IStockProvider.cs
 *
 *  Purpose:  Returns stock count for individual products
 *
 *  Date        Name                Reason
 *  19/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Middleware.Products;
using Middleware.ShoppingCart;

namespace Middleware
{
	/// <summary>
	/// Stock provider interface, disigned to provide stock quantities for individual prodoucts
	/// </summary>
	public interface IStockProvider
	{
		/// <summary>
		/// Gets the current available stock level for an individual product.
		/// </summary>
		/// <param name="product">Product who's stock will be checked, the StockAvailability property for the product will be updated with the stock level.</param>
		void GetStockAvailability(in Product product);

		/// <summary>
		/// Gets the current available stock level for a list of product items.
		/// </summary>
		/// <param name="productList">List of products who's stock will be checked, the StockAvailability property for the product will be updated with the stock level.</param>
		void GetStockAvailability(in List<Product> productList);

		/// <summary>
		/// Gets the current available stock level for an individual product within a shopping cart.
		/// </summary>
		/// <param name="shoppingCartItem">ShoppingCartItem who's stock will be updated.</param>
		void GetStockAvailability(in ShoppingCartItem shoppingCartItem);

		/// <summary>
		/// Gets the current available stock level for a list of products within a shopping cart.
		/// </summary>
		/// <param name="shoppingCartItemList">ShoppingCartItem who's stock will be updated.</param>
		void GetStockAvailability(in List<ShoppingCartItem> shoppingCartItemList);

		/// <summary>
		/// Adds new stock availability for a product
		/// </summary>
		/// <param name="product">Product for which stock will be added</param>
		/// <param name="stockCount">Amount of stock to add</param>
		/// <param name="error">Error if the stock provider raises it's own issue</param>
		bool AddStockToProduct(Product product, uint stockCount, out string error);
	}
}
