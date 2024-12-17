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
 *  File: ProductModel.cs
 *
 *  Purpose:  Product Model
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

#pragma warning disable CA1721, CS1591

namespace ProductPlugin.Models
{
	public sealed class ProductModel : BaseProductModel
	{
		#region Constructors

		public ProductModel()
		{
		}

		public ProductModel(in BaseModelData modelData,
			in int id, in string name, in string[] images, in int productGroupId,
			in bool newProduct, in bool bestSeller, in decimal lowestPrice, bool allowAddToBasket, string sku)
			: base(modelData)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (String.IsNullOrEmpty(sku))
				throw new ArgumentNullException(nameof(sku));

			Id = id;
			ProductGroupId = productGroupId;
			Name = name;
			Description = String.Empty;
			Features = String.Empty;
			VideoLink = String.Empty;
			Images = images;
			NewProduct = newProduct;
			BestSeller = bestSeller;
			Sku = sku;

			if (lowestPrice == 0)
				RetailPrice = Languages.LanguageStrings.Free;
			else
				RetailPrice = lowestPrice.ToString("C", System.Threading.Thread.CurrentThread.CurrentUICulture);

			AllowAddToBasket = allowAddToBasket;
		}

		public ProductModel(in BaseModelData modelData,
			in List<ProductCategoryModel> productCategories)
			: base(modelData, productCategories)
		{

		}

		public ProductModel(in BaseModelData modelData,
			in List<ProductCategoryModel> productCategories,
			in int id, in int productGroupId, in string name, in string description, in string features,
			in string videoLink, in string[] images, in decimal retailPrice, in string sku,
			in bool newProduct, in bool bestSeller,
			in bool allowAddToBasket, in uint stockAvailability)
			: this(modelData, productCategories)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			if (String.IsNullOrEmpty(description))
				throw new ArgumentNullException(nameof(description));

			if (String.IsNullOrEmpty(sku))
				throw new ArgumentNullException(nameof(sku));

			Id = id;
			ProductGroupId = productGroupId;
			Name = name;
			Description = description;
			Features = features;
			VideoLink = videoLink;
			Images = images;
			StockAvailability = stockAvailability;
			Sku = sku;
			NewProduct = newProduct;
			BestSeller = bestSeller;

			if (retailPrice == 0)
				RetailPrice = Languages.LanguageStrings.Free;
			else
				RetailPrice = retailPrice.ToString("C", System.Threading.Thread.CurrentThread.CurrentUICulture);

			AllowAddToBasket = allowAddToBasket;

			if (retailPrice > 0)
				AddToCart = new AddToCartModel(id, retailPrice, 0, stockAvailability);
		}

		#endregion Constructors

		#region Public Methods

		public string GetRouteName()
		{
			return RouteFriendlyName(Name);
		}

		public string GetVideoLink()
		{
			if (String.IsNullOrEmpty(VideoLink))
				return String.Empty;

			string Result = VideoLink;

			if (Result.StartsWith("https://www.facebook.com/video", StringComparison.CurrentCultureIgnoreCase) ||
				Result.StartsWith("http://www.facebook.com/video", StringComparison.CurrentCultureIgnoreCase))
			{
				//its from facebook
				string fbReference = Result.Replace("video.php?v=", "v/");
				Result = String.Format("<object class=\"productVideo\" ><param name=\"allowfullscreen\" value=\"true\" /> " +
					"<param name=\"allowscriptaccess\" value=\"always\" /> <param name=\"movie\" value=\"{0}\" /> " +
					"<embed src=\"{0}\" type=\"application/x-shockwave-flash\" allowscriptaccess=\"always\" " +
					"allowfullscreen=\"true\" width=\"640\" height=\"390\"></embed></object>", fbReference);
			}
			else if (!Result.StartsWith("http", StringComparison.CurrentCultureIgnoreCase))
			{
				//assume a you tube link here
				Result = String.Format("<iframe class=\"productVideo\" src=\"https://www.youtube.com/embed/{0}\" frameborder=\"0\"></iframe>", Result);
			}

			return Result;
		}

		public string[] FeatureList()
		{
			if (String.IsNullOrEmpty(Features))
				return [];

			string features = Features.Replace("\n", String.Empty);
			return features.Split('\r', StringSplitOptions.RemoveEmptyEntries);
		}

		#endregion Public Methods

		#region Properties

		public int Id { get; }

		public int ProductGroupId { get; }

		public string Name { get; }

		public string Description { get; }

		public string Features { get; }

		public string VideoLink { get; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "ok on this occasion")]
		public string[] Images { get; }

		public bool NewProduct { get; }

		public bool BestSeller { get; }

		public string RetailPrice { get; }

		public bool AllowAddToBasket { get; }

		public AddToCartModel AddToCart { get; }

		public uint StockAvailability { get; }

		public string Sku { get; }

		#endregion Properties
	}
}

#pragma warning restore CA1721, CS1591