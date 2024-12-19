/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: Product.cs
 *
 *  Purpose:  Product 
 *
 *  Date        Name                Reason
 *  01/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Products
{
	/// <summary>
	/// Display options for a product within a website.
	/// </summary>
	public sealed class Product
	{
		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique product id.</param>
		/// <param name="productGroupId">Primary ProductGroup the product belongs to.</param>
		/// <param name="name">Name of the product.</param>
		/// <param name="description">Description of the product.</param>
		/// <param name="features">Product feature list.  This will be converted to a bullet list when displayed on a website.</param>
		/// <param name="videoLink">The url for a video link to the product if one exists.</param>
		/// <param name="images">List of images which represent the product.</param>
		/// <param name="retailPrice">Retail price of product.</param>
		/// <param name="sku">Unique product SKU.</param>
		/// <param name="isDownload">Indicates the product is downloadable.</param>
		/// <param name="allowBackorder">Indicates the product is allowed to be back ordered, if there is no stock available at the time of purchase.</param>
		/// <param name="isVisible">Indicates whether the product is visible or not</param>
		public Product(in int id, in int productGroupId, in string name, in string description, in string features,
			in string videoLink, in string[] images, in decimal retailPrice, in string sku, in bool isDownload,
			in bool allowBackorder, in bool isVisible)
			: this(id, productGroupId, name, description, features, videoLink, images, retailPrice, sku, isDownload, allowBackorder, false, false, isVisible)
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="id">Unique product id.</param>
		/// <param name="productGroupId">Primary ProductGroup the product belongs to.</param>
		/// <param name="name">Name of the product.</param>
		/// <param name="description">Description of the product.</param>
		/// <param name="features">Product feature list.  This will be converted to a bullet list when displayed on a website.</param>
		/// <param name="videoLink">The url for a video link to the product if one exists.</param>
		/// <param name="images">List of images which represent the product.</param>
		/// <param name="retailPrice">Retail price of product.</param>
		/// <param name="sku">Unique product SKU.</param>
		/// <param name="isDownload">Indicates the product is downloadable.</param>
		/// <param name="allowBackorder">Indicates the product is allowed to be back ordered, if there is no stock available at the time of purchase.</param>
		/// <param name="isNew">Indicates the product is a new product.</param>
		/// <param name="isBestSeller">Indicates the product is a best selling product.</param>
		/// <param name="isVisible">Indicates whether the product is visible or not</param>
		public Product(in int id, in int productGroupId, in string name, in string description, in string features,
			in string videoLink, in string[] images, in decimal retailPrice, in string sku, in bool isDownload,
			in bool allowBackorder, in bool isNew, in bool isBestSeller, in bool isVisible)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (String.IsNullOrEmpty(description))
				throw new ArgumentNullException(nameof(description));

			Id = id;
			ProductGroupId = productGroupId;
			Name = name;
			Description = description;
			Features = features;
			VideoLink = videoLink;
			Images = images;
			RetailPrice = retailPrice;
			Sku = sku;
			IsDownload = isDownload;
			AllowBackorder = allowBackorder;
			NewProduct = isNew;
			BestSeller = isBestSeller;
			IsVisible = isVisible;
		}

		#endregion Constructors

		#region Public Methods

		/// <summary>
		/// Sets the current stock availability for the product.
		/// </summary>
		/// <param name="currentStock"></param>
		public void SetCurrentStockLevel(uint currentStock)
		{
			StockAvailability = currentStock;
		}

		#endregion Public Methods

		#region Properties

		/// <summary>
		/// Unique product id.
		/// </summary>
		/// <value>int</value>
		public int Id { get; }

		/// <summary>
		/// Primary ProductGroup the product belongs to.
		/// </summary>
		/// <value>int</value>
		public int ProductGroupId { get; }

		/// <summary>
		/// Name of the product.
		/// </summary>
		/// <value>string</value>
		public string Name { get; }

		/// <summary>
		/// Description of the product.
		/// </summary>
		/// <value>string</value>
		public string Description { get; }

		/// <summary>
		/// Product feature list.  This will be converted to a bullet list when displayed on a website.
		/// </summary>
		/// <value>string</value>
		public string Features { get; }

		/// <summary>
		/// The url for a video linkt to the product if one exists.
		/// </summary>
		/// <value>string</value>
		public string VideoLink { get; }

		/// <summary>
		/// List of images which represent the product.
		/// </summary>
		/// <value>string[]</value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "ok on this occasion")]
		public string[] Images { get; }

		/// <summary>
		/// Indicates the product is a new product.
		/// </summary>
		/// <value>bool.  if true the product may have different display options on the website.</value>
		public bool NewProduct { get; }

		/// <summary>
		/// Indicates the product is a best selling product.
		/// </summary>
		/// <value>bool.  If true the product may have different display options on the website.</value>
		public bool BestSeller { get; }

		/// <summary>
		/// Retail price of product.
		/// </summary>
		/// <value>decimal</value>
		public decimal RetailPrice { get; }

		/// <summary>
		/// Unique product SKU.
		/// </summary>
		/// <value>string</value>
		public string Sku { get; }

		/// <summary>
		/// Indicates the product is downloadable.
		/// </summary>
		/// <value>bool</value>
		public bool IsDownload { get; }

		/// <summary>
		/// Indicates the product is allowed to be back ordered, if there is no stock available at the time of purchase.
		/// </summary>
		/// <value>bool.  If true the item can be back ordered.</value>
		public bool AllowBackorder { get; }

		/// <summary>
		/// The quantity of stock available for the product.
		/// </summary>
		/// <value>uint.  Quantity of stock or zero.</value>
		public uint StockAvailability { get; private set; }

		/// <summary>
		/// Indicates whether the product is available and visible or not
		/// </summary>
		public bool IsVisible { get; set; }

		#endregion Properties
	}
}
