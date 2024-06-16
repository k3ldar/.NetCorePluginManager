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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Products.Plugin
 *  
 *  File: ProductAddStockModel.cs
 *
 *  Purpose:  Product add stock model
 *
 *  Date        Name                Reason
 *  12/02/2023  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace ProductPlugin.Models
{
	/// <summary>
	/// Add stock model for adding stock for products
	/// </summary>
	public class ProductAddStockModel
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ProductAddStockModel()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Id of product</param>
		/// <param name="productName">Name of product</param>
		/// <exception cref="ArgumentNullException"></exception>
		public ProductAddStockModel(int id, string productName)
		{
			Id = id;
			ProductName = productName ?? throw new ArgumentNullException(nameof(productName));
		}

		/// <summary>
		/// Id of product
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Name of product
		/// </summary>
		public string ProductName { get; set; }

		/// <summary>
		/// Quantity of stock to add
		/// </summary>
		public uint Quantity { get; set; }
	}
}
