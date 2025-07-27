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
 *  Product:  Products.Plugin
 *  
 *  File: EditProductModel.cs
 *
 *  Purpose:  Edit product model
 *
 *  Date        Name                Reason
 *  09/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Middleware;

using SharedPluginFeatures;

namespace ProductPlugin.Models
{
	/// <summary>
	/// Model for editing/creating products
	/// </summary>
	public sealed class EditProductModel : BaseModel
	{
		#region Constructors

		/// <summary>
		/// Standard constructor
		/// </summary>
		public EditProductModel()
		{

		}

		/// <summary>
		/// Constructor used for creating a product
		/// </summary>
		/// <param name="modelData"></param>
		public EditProductModel(in IBaseModelData modelData)
			: base(modelData)
		{
			Id = -1;
			ProductGroups = [];
		}

		/// <summary>
		/// Constructor used for editing a product
		/// </summary>
		/// <param name="modelData"></param>
		/// <param name="productGroups"></param>
		/// <param name="id"></param>
		/// <param name="productGroupId"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		/// <param name="features"></param>
		/// <param name="videoLink"></param>
		/// <param name="newProduct"></param>
		/// <param name="bestSeller"></param>
		/// <param name="retailPrice"></param>
		/// <param name="sku"></param>
		/// <param name="isDownload"></param>
		/// <param name="allowBackOrder"></param>
		/// <param name="isVisible"></param>
		/// <param name="pageNumber"></param>
		public EditProductModel(in IBaseModelData modelData, List<LookupListItem> productGroups, int id, int productGroupId, string name, string description,
			string features, string videoLink, bool newProduct, bool bestSeller, decimal retailPrice, string sku,
			bool isDownload, bool allowBackOrder, bool isVisible, int pageNumber)
			: base(modelData)
		{
			ProductGroups = productGroups ?? throw new ArgumentNullException(nameof(productGroups));
			Id = id;
			ProductGroupId = productGroupId;
			Name = name;
			Description = description;
			Features = features;
			VideoLink = videoLink;
			NewProduct = newProduct;
			BestSeller = bestSeller;
			RetailPrice = retailPrice;
			Sku = sku;
			IsDownload = isDownload;
			AllowBackorder = allowBackOrder;
			IsVisible = isVisible;
			PageNumber = pageNumber;
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Unique product id.
		/// </summary>
		/// <value>int</value>
		public int Id { get; set; }

		/// <summary>
		/// Primary ProductGroup the product belongs to.
		/// </summary>
		/// <value>int</value>
		public int ProductGroupId { get; set; }

		/// <summary>
		/// Name of the product.
		/// </summary>
		/// <value>string</value>
		[Required(AllowEmptyStrings = false, ErrorMessage = nameof(Languages.LanguageStrings.AppErrorInvalidProductName))]
		[StringLength(80, MinimumLength = 5)]
		public string Name { get; set; }

		/// <summary>
		/// Description of the product.
		/// </summary>
		/// <value>string</value>
		[Required(AllowEmptyStrings = false, ErrorMessage = nameof(Languages.LanguageStrings.AppErrorInvalidProductDescription))]
		[StringLength(8000, MinimumLength = 30)]
		public string Description { get; set; }

		/// <summary>
		/// Product feature list.  This will be converted to a bullet list when displayed on a website.
		/// </summary>
		/// <value>string</value>
		[StringLength(500, MinimumLength = 0)]
		public string Features { get; set; }

		/// <summary>
		/// The url for a video linkt to the product if one exists.
		/// </summary>
		/// <value>string</value>
		[StringLength(250, MinimumLength = 0)]
		public string VideoLink { get; set; }

		/// <summary>
		/// Indicates the product is a new product.
		/// </summary>
		/// <value>bool.  if true the product may have different display options on the website.</value>
		public bool NewProduct { get; set; }

		/// <summary>
		/// Indicates the product is a best selling product.
		/// </summary>
		/// <value>bool.  If true the product may have different display options on the website.</value>
		public bool BestSeller { get; set; }

		/// <summary>
		/// Retail price of product.
		/// </summary>
		/// <value>decimal</value>
		[Range(0, 999999999.99, ErrorMessage = nameof(Languages.LanguageStrings.AppErrorInvalidProductPrice))]
		public decimal RetailPrice { get; set; }

		/// <summary>
		/// Unique product SKU.
		/// </summary>
		/// <value>string</value>
		[StringLength(25, MinimumLength = 4, ErrorMessage = nameof(Languages.LanguageStrings.AppErrorInvalidProductSku))]
		public string Sku { get; set; }

		/// <summary>
		/// Indicates the product is downloadable.
		/// </summary>
		/// <value>bool</value>
		public bool IsDownload { get; set; }

		/// <summary>
		/// Indicates the product is allowed to be back ordered, if there is no stock available at the time of purchase.
		/// </summary>
		/// <value>bool.  If true the item can be back ordered.</value>
		public bool AllowBackorder { get; set; }

		/// <summary>
		/// Indicates whether the product is visible or not
		/// </summary>
		public bool IsVisible { get; set; }

		/// <summary>
		/// List of existing product Groups
		/// </summary>
		public List<LookupListItem> ProductGroups { get; }

		/// <summary>
		/// Page in product list where the product belongs
		/// </summary>
		public int PageNumber { get; set; }

		#endregion Properties
	}
}
