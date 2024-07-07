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
 *  File: KeywordSearchProvider.cs
 *
 *  Purpose:  Product key word search provider
 *
 *  Date        Name                Reason
 *  12/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

using Middleware;
using Middleware.Products;
using Middleware.Search;

using ProductPlugin.Controllers;

using SharedPluginFeatures;

namespace ProductPlugin.Classes
{
	/// <summary>
	/// Product keyword provider
	/// </summary>
	public class KeywordSearchProvider : ISearchKeywordProvider
	{
		#region Public Constants

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

		public const string ContainsVideo = nameof(ContainsVideo);
		public const string ProductGroup = nameof(ProductGroup);
		public const string Price = nameof(Price);

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

		#endregion Public Constants

		#region Private Members

		private const string ProductSearchResultViewName = "~/Views/Product/_ProductSearchResult.cshtml";
		private readonly IProductProvider _productProvider;

		#endregion Private Members

		#region Constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="productProvider">IProductProvider instance</param>
		public KeywordSearchProvider(IProductProvider productProvider)
		{
			_productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
		}

		#endregion Constructors

		#region ISearchKeywordProvider Methods

		/// <summary>
		/// Search keywords
		/// </summary>
		/// <param name="searchOptions"></param>
		/// <returns>List&lt;SearchResponseItem&gt;</returns>
		public List<SearchResponseItem> Search(in KeywordSearchOptions searchOptions)
		{
			if (searchOptions == null)
				throw new ArgumentNullException(nameof(searchOptions));

			List<SearchResponseItem> Results = [];

			List<Product> products = _productProvider.GetProducts(1, SharedPluginFeatures.Constants.MaximumProducts);

			if (searchOptions.ExactMatch || searchOptions.QuickSearch)
			{
				ExactMatch(Results, searchOptions, products);
			}
			else
			{
				NonExactMatch(Results, searchOptions, products);
			}

			return Results;
		}

		/// <summary>
		/// Retrieves the advanced search options for the provider
		/// </summary>
		/// <returns>Dictionary&lt;string, AdvancedSearchOptions&gt;</returns>
		public Dictionary<string, AdvancedSearchOptions> AdvancedSearch()
		{
			AdvancedSearchOptions searchOptions = new(
				nameof(ProductController.AdvancedSearch),
				ProductController.Name,
				"/Product/Search/",
				"/Product/SearchOptions/",
				"/css/products.css");

			return new Dictionary<string, AdvancedSearchOptions>
			{
				{ Languages.LanguageStrings.Products, searchOptions }
			};
		}

		/// <summary>
		/// Returns a list of all available response types for the Documentation Plugin
		/// </summary>
		/// <param name="quickSearch">Indicates whether the response types are for quick or normal searching</param>
		/// <returns>List&lt;string&gt;</returns>
		public List<string> SearchResponseTypes(in Boolean quickSearch)
		{
			List<string> Result =
			[
				"ProductName"
			];

			if (!quickSearch)
			{
				Result.Add("ProductDescription");
				Result.Add("ProductSku");
			}

			return Result;
		}

		#endregion ISearchKeywordProvider Methods

		#region Private Methods

		private static void ExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
			in List<Product> products)
		{
			foreach (Product product in products)
			{
				if (results.Count >= searchOptions.MaximumSearchResults)
				{
					return;
				}

				int offset = product.Name.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

				if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
				{
					AddSearchResult(results, product, "ProductName", offset);
					continue;
				}

				if (!searchOptions.QuickSearch)
				{
					offset = product.Description.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

					if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
					{
						AddSearchResult(results, product, "ProductDescription", offset);
						continue;
					}

					offset = product.Sku.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

					if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
					{
						AddSearchResult(results, product, "ProductSku", offset);
					}
				}
			}
		}

		private static void AddSearchResult(in List<SearchResponseItem> results, in Product product, in string searchType, in int offset)
		{
			SearchResponseItem searchItem = new(searchType, product.Name, offset,
				$"/Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/",
				product.Name, ProductSearchResultViewName);

			searchItem.Properties.Add(nameof(product.Id), product.Id);
			searchItem.Properties.Add(nameof(product.Images), product.Images[0]);
			searchItem.Properties.Add(nameof(product.BestSeller), product.BestSeller);
			searchItem.Properties.Add(nameof(product.NewProduct), product.NewProduct);
			searchItem.Properties.Add(nameof(product.RetailPrice),
				Shared.Utilities.FormatMoney(product.RetailPrice, Thread.CurrentThread.CurrentUICulture));
			searchItem.Properties.Add(nameof(Price), product.RetailPrice);
			searchItem.Properties.Add(nameof(product.StockAvailability), StockMessage(product.StockAvailability));

			results.Add(searchItem);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string StockMessage(in uint stockCount)
		{
			if (stockCount == 0)
			{
				return Languages.LanguageStrings.OutOfStock;
			}

			return $"{stockCount} {Languages.LanguageStrings.InStock}";
		}

		private void NonExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
			in List<Product> products)
		{
			string[] words = searchOptions.SearchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries);

			if (words.Length == 0)
			{
				SearchProducts(results, searchOptions, products, String.Empty);
			}
			else
			{
				foreach (string word in words)
				{
					if (String.IsNullOrWhiteSpace(word))
						continue;

					SearchProducts(results, searchOptions, products, word);
				}
			}
		}

		private void SearchProducts(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
			in List<Product> products, in string word)
		{
			List<int> productGroups = GetProductGroupsFromOptions(searchOptions);

			foreach (Product product in products)
			{
				if (results.Count >= searchOptions.MaximumSearchResults)
				{
					return;
				}

				if (AddToSearchResults(productGroups, product, searchOptions, word, out int offset))
				{
					AddSearchResult(results, product, "ProductName", offset);
				}
			}
		}

		private List<int> GetProductGroupsFromOptions(in KeywordSearchOptions searchOptions)
		{
			List<int> Result = [];

			if (!searchOptions.QuickSearch)
			{
				foreach (KeyValuePair<string, object> item in searchOptions.Properties)
				{
					if (item.Value.ToString().Equals(ProductGroup))
					{
						ProductGroup productGroup = _productProvider.ProductGroupsGet().Find(p => p.Description == item.Key);

						if (productGroup != null)
							Result.Add(productGroup.Id);
					}
				}
			}

			if (Result.Count == 0)
			{
				foreach (ProductGroup item in _productProvider.ProductGroupsGet())
				{
					Result.Add(item.Id);
				}
			}

			return Result;
		}

		private static bool AddToSearchResults(in List<int> productGroups, in Product product,
			in KeywordSearchOptions searchOptions, in string word, out int offset)
		{
			bool Result = false;
			bool anyWord = String.IsNullOrWhiteSpace(word);

			offset = product.Name.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

			if (anyWord || offset > -1)
			{
				Result = true;
			}

			if (!searchOptions.QuickSearch)
			{
				if (!anyWord)
				{
					offset = product.Description.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

					if (offset > -1)
					{
						Result = true;
					}

					if (!Result)
					{
						offset = product.Sku.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

						if (offset > -1)
						{
							Result = true;
						}
					}
				}

				if (Result && searchOptions.Properties.ContainsKey(ContainsVideo) && String.IsNullOrEmpty(product.VideoLink))
				{
					Result = false;
				}

				if (Result)
				{
					Result = productGroups.Contains(product.ProductGroupId);
				}

				if (Result)
				{
					bool priceFound = false;

					if (searchOptions.Properties.TryGetValue(Price, out object value) &&
						value is List<ProductPriceInfo> priceInfo)
					{
						foreach (ProductPriceInfo productPrice in priceInfo)
						{
							if (productPrice.PriceMatch(product.RetailPrice))
							{
								priceFound = true;
								break;
							}
						}

						Result = priceFound;
					}
				}
			}

			return Result;
		}

		#endregion Private Methods
	}
}
