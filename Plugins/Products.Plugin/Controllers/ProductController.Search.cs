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
 *  File: ProductController.cs
 *
 *  Purpose:  Product Controller
 *
 *  Date        Name                Reason
 *  23/03/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

using Languages;

using Microsoft.AspNetCore.Mvc;

using Middleware.Products;
using Middleware.Search;

using ProductPlugin.Classes;
using ProductPlugin.Models;

using Shared.Classes;

using SharedPluginFeatures;

using static Shared.Utilities;

#pragma warning disable CS1591

namespace ProductPlugin.Controllers
{
	public partial class ProductController
	{
		#region Private Static Members

		private static readonly object _lockObject = new();

		#endregion Private Static Members

		#region Public Action Methods

		[HttpGet]
		[Route("Product/Search/{searchId}/")]
		[Route("Product/Search/")]
		public IActionResult Search(string searchId)
		{
			return PartialView("/Views/Product/_ProductSearch.cshtml", GetSearchModel(searchId));
		}

		[HttpGet]
		[Route("Product/SearchOptions/{searchId}")]
		[Route("Product/SearchOptions/")]
		public IActionResult SearchOptions(string searchId)
		{
			return PartialView("/Views/Product/_ProductSearchOption.cshtml", GetSearchModel(searchId));
		}

		[HttpGet]
		public IActionResult AdvancedSearch(ProductSearchViewModel model)
		{
			if (model == null || !ModelState.IsValid)
			{
				return Redirect($"/Search/Advanced/{LanguageStrings.Products}/");
			}

			GetSearchId(model);

			KeywordSearchProvider searchProvider = new(_productProvider);

			KeywordSearchOptions options = new(model.SearchText ?? String.Empty)
			{
				MaximumSearchResults = Constants.MaximumProducts
			};

			GetSearchId(model);
			options.SearchName = model.SearchName;

			// add additional search options
			foreach (CheckedViewItemModel item in model.ProductGroups)
			{
				if (item.Selected)
					options.Properties.Add(item.Name, KeywordSearchProvider.ProductGroup);
			}

			List<ProductPriceInfo> prices = [];
			List<ProductPriceInfo> priceGroups = GetPriceGroups();

			foreach (CheckedViewItemModel item in model.Prices)
			{
				if (item.Selected)
				{
					ProductPriceInfo priceGroup = priceGroups.Find(pg => pg.Text.Equals(item.Name));

					if (priceGroup != null)
						prices.Add(priceGroup);
				}
			}

			if (prices.Count == 0)
			{
				prices.AddRange(priceGroups);
			}

			options.Properties.Add(KeywordSearchProvider.Price, prices);

			if (model.ContainsVideo)
			{
				options.Properties.Add(KeywordSearchProvider.ContainsVideo, true);
			}

			// perform the search, the results will be held in memory and shown on search page
			DefaultSearchThread.KeywordSearch(searchProvider, options);

			string cacheName = $"Product Search View Model {model.SearchName}";
			_memoryCache.GetExtendingCache().Add(cacheName, new CacheItem(cacheName, model), true);

			return Redirect($"/Search/Advanced/{LanguageStrings.Products}/{model.SearchName}/");
		}

		#endregion Public Action Methods

		#region Private Methods

		private List<string> GetProductGroups()
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				string cacheName = "Product Search Product Group Product Counts";
				CacheItem cacheItem = _memoryCache.GetCache().Get(cacheName);

				if (cacheItem == null)
				{
					List<string> productGroups = [];

					foreach (ProductGroup item in _productProvider.ProductGroupsGet())
						productGroups.Add(item.Description);

					if (_settings.ShowProductCounts)
					{
						ThreadManager.ThreadStart(new ProductGroupProductCounts(_productProvider, productGroups),
							"Update product group product counts", System.Threading.ThreadPriority.Lowest);
					}

					cacheItem = new CacheItem(cacheName, productGroups);
					_memoryCache.GetCache().Add(cacheName, cacheItem, true);
				}

				return (List<string>)cacheItem.Value;
			}
		}

		private int GetProductWithVideoCounts()
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				string cacheName = "Product Search Product With Video Product Counts";
				CacheItem cacheItem = _memoryCache.GetCache().Get(cacheName);

				if (cacheItem == null)
				{
					int videoCount = _productProvider.GetProducts(1, Constants.MaximumProducts)
						.Count(p => !String.IsNullOrEmpty(p.VideoLink));
					List<string> productGroups = [];

					foreach (ProductGroup item in _productProvider.ProductGroupsGet())
						productGroups.Add(item.Description);

					if (_settings.ShowProductCounts)
					{
						ThreadManager.ThreadStart(new ProductGroupProductCounts(_productProvider, productGroups),
							"Update product with video product counts", System.Threading.ThreadPriority.Lowest);
					}

					cacheItem = new CacheItem(cacheName, videoCount);
					_memoryCache.GetCache().Add(cacheName, cacheItem, true);
				}

				return (int)cacheItem.Value;
			}
		}

		private List<ProductPriceInfo> GetPriceGroups()
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
				string cacheName = $"Product Search Product Price Groups {culture.Name}";
				CacheItem cacheItem = _memoryCache.GetCache().Get(cacheName);

				if (cacheItem == null)
				{
					List<ProductPriceInfo> priceGroups = [];

					string[] prices = _settings.PriceGroups.Split(';', StringSplitOptions.RemoveEmptyEntries);
					decimal lastValue = -1;

					for (int i = 0; i < prices.Length; i++)
					{
						string price = prices[i];

						if (decimal.TryParse(price, out decimal value))
						{
							if (value < 0 || value < lastValue)
								throw new InvalidOperationException();

							if (i == 0)
							{
								if (value == 0)
									priceGroups.Add(new ProductPriceInfo(LanguageStrings.Free, 0, 0));
								else
									priceGroups.Add(new ProductPriceInfo(
										$"{FormatMoney(0, culture)} - {FormatMoney(value, culture)}", 0, value));

								lastValue = value;
								continue;
							}

							if (lastValue == 0)
							{
								priceGroups.Add(new ProductPriceInfo(
									$"{LanguageStrings.Under} {FormatMoney(value, culture)}", 0, value));
							}
							else
							{
								priceGroups.Add(new ProductPriceInfo(
									$"{FormatMoney(lastValue, culture)} - {FormatMoney(value, culture)}", lastValue, value));
							}

							if (i == prices.Length - 1)
							{
								priceGroups.Add(new ProductPriceInfo(
									$"{LanguageStrings.Over} {FormatMoney(value, culture)}", value, Decimal.MaxValue));
							}

							lastValue = value;
						}
					}

					if (_settings.ShowProductCounts)
					{
						ThreadManager.ThreadStart(new PriceGroupProductCounts(_productProvider, priceGroups),
							$"Update price group product counts {culture.Name}", System.Threading.ThreadPriority.Lowest);
					}

					cacheItem = new CacheItem(cacheName, priceGroups);
					_memoryCache.GetCache().Add(cacheName, cacheItem, true);
				}

				return (List<ProductPriceInfo>)cacheItem.Value;
			}
		}

		private ProductSearchViewModel GetSearchModel(string searchId)
		{
			if (!String.IsNullOrEmpty(searchId))
			{
				CacheItem cacheItem = _memoryCache.GetExtendingCache().Get($"Product Search View Model {searchId}");

				if (cacheItem != null)
				{
					return (ProductSearchViewModel)cacheItem.Value;
				}
			}

			ProductSearchViewModel Result = new();

			foreach (string group in GetProductGroups())
			{
				Result.ProductGroups.Add(new CheckedViewItemModel(group, true));
			}

			foreach (ProductPriceInfo priceGroup in GetPriceGroups())
			{
				Result.Prices.Add(new CheckedViewItemModel(priceGroup.Text, true));
			}

			Result.VideoProductCount = GetProductWithVideoCounts();

			GetSearchId(Result);

			return Result;
		}

		private static void GetSearchId(in ProductSearchViewModel model)
		{
			// Use input string to calculate MD5 hash
			byte[] inputBytes = Encoding.ASCII.GetBytes(JsonSerializer.Serialize(model));
			byte[] hashBytes = MD5.HashData(inputBytes);

			// Convert the byte array to hexadecimal string
			StringBuilder sb = new();
			for (int i = 0; i < hashBytes.Length; i++)
			{
				sb.Append(hashBytes[i].ToString("X2"));
			}

			model.SearchName = $"P{sb}";
		}

		#endregion Private Methods
	}
}
