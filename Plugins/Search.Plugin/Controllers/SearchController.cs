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
 *  Product:  Search Plugin
 *  
 *  File: SearchController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  01/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Languages;

using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Search;

using PluginManager.Abstractions;

using SearchPlugin.Models;

using SharedPluginFeatures;

using Constants = SharedPluginFeatures.Constants;

#pragma warning disable IDE0079
#pragma warning disable CS1591

namespace SearchPlugin.Controllers
{
	/// <summary>
	/// Search controller, allows users to search using a standard interface implemented by ISearchProvider interface.
	/// </summary>
	[DenySpider]
	[Subdomain(SearchController.Name)]
	public class SearchController : BaseController
	{
		#region Private Members

		private readonly SearchControllerSettings _settings;
		private readonly ISearchProvider _searchProvider;

		#endregion Private Members

		#region Constructors

		public SearchController(ISettingsProvider settingsProvider, ISearchProvider searchProvider)
		{
			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			_settings = settingsProvider.GetSettings<SearchControllerSettings>(nameof(SearchPlugin));
			_searchProvider = searchProvider ?? throw new ArgumentNullException(nameof(searchProvider));
		}

		#endregion Constructors

		#region Constants

		public const string Name = "Search";

		#endregion Constants

		#region Public Action Methods

		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.Search))]
		[LoggedInOut]
		public IActionResult Index()
		{
			return View(CreateSearchModel(new SearchViewModel(), false, false, null));
		}

		[BadEgg]
		[LoggedInOut]
		public IActionResult Index(SearchViewModel model)
		{
			return View(CreateSearchModel(model, true, false, null));
		}

		[BadEgg]
		[LoggedInOut]
		public IActionResult Search(SearchViewModel model)
		{
			return View(nameof(Index), CreateSearchModel(model, true, false, null));
		}

		[BadEgg]
		[LoggedInOut]
		[Route("Search/Result/{searchText}/Page/{page}/")]
		public IActionResult PageSearch(string searchText, int page)
		{
			return View(nameof(Index), CreateSearchModel(new SearchViewModel(searchText, page), true, false, null));
		}

		[HttpGet]
		[BadEgg]
		[LoggedInOut]
		[Route("Search/Advanced/{providerName}/")]
		public IActionResult AdvancedSearch(string providerName)
		{
			SearchViewModel model = new();

			if (!String.IsNullOrWhiteSpace(providerName) &&
				_searchProvider.AdvancedSearch().ContainsKey(providerName))
			{
				model.ActiveTab = providerName;
			}

			return View(nameof(Index), CreateSearchModel(model, false, false, $"{providerName}/{model.SearchId}"));
		}

		[HttpGet]
		[BadEgg]
		[LoggedInOut]
		[Route("Search/Advanced/{providerName}/{searchId}/")]
		[Route("Search/Advanced/{providerName}/{searchId}/Page/{page}/")]
		public IActionResult AdvancedSearch(string providerName, string searchId, int page)
		{
			SearchViewModel model = new();

			if (!String.IsNullOrWhiteSpace(providerName) &&
				_searchProvider.AdvancedSearch().ContainsKey(providerName))
			{
				model.ActiveTab = providerName;
			}

			model.Page = page < 1 ? 1 : page;

			model.SearchId = searchId ?? String.Empty;

			if (String.IsNullOrEmpty(model.SearchId))
			{
				return View(nameof(Index), CreateSearchModel(model, false, false, $"{providerName}/{model.SearchId}"));
			}
			else
			{
				return View(nameof(Index), CreateSearchModel(model, true, true, $"{providerName}/{model.SearchId}"));
			}
		}

		[HttpGet]
		public IActionResult QuickSearch()
		{
			return PartialView("_QuickSearch", new SearchViewModel());
		}

		[HttpGet]
		[BadEgg]
		[LoggedInOut]
		public IActionResult QuickSearchDefault(SearchViewModel model)
		{
			return View(nameof(Index), CreateSearchModel(model, true, false, null));
		}

		[HttpPost]
		[BadEgg]
		[LoggedInOut]
		public IActionResult QuickKeywordSearch([FromBody] QuickSearchModel searchModel)
		{
			if (searchModel == null ||
				String.IsNullOrWhiteSpace(searchModel.keywords) ||
				searchModel.keywords.Length < _settings.MinimumKeywordSearchLength)
			{
				return new StatusCodeResult(Constants.HtmlResponseBadRequest);
			}

			KeywordSearchOptions searchOptions = new(IsUserLoggedIn(), searchModel.keywords, true);

			List<SearchResponseItem> searchResults = _searchProvider.KeywordSearch(searchOptions);

			IEnumerable<SearchResponseItem> topResults = searchResults.OrderByDescending(r => r.Relevance).Take(5);

			if (_settings.HighlightQuickSearchTerms)
			{
				foreach (SearchResponseItem item in topResults)
				{
					item.HighlightKeywords(searchModel.keywords.Length);
				}
			}

			return new JsonResult(topResults)
			{
				StatusCode = Constants.HtmlResponseSuccess,
				ContentType = Constants.ContentTypeApplicationJson
			};
		}

		#endregion Public Action Methods

		#region Private Methods

		private SearchViewModel CreateSearchModel(in SearchViewModel model, in bool validate,
			in bool retrieveResults, in string paginationText)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			if (validate && String.IsNullOrEmpty(model.SearchText))
			{
				ModelState.AddModelError(nameof(model.SearchText), Languages.LanguageStrings.SearchInvalid);
			}

			SearchViewModel Result = new(GetModelData(), _searchProvider.AdvancedSearch())
			{
				SearchResults = new List<SearchResponseItem>(),
				SearchText = model.SearchText ?? String.Empty,
				SearchId = model.SearchId ?? String.Empty,
				Page = model.Page,
				ActiveTab = model.ActiveTab
			};

			List<SearchResponseItem> results = null;

			if (retrieveResults)
			{
				results = _searchProvider.GetSearchResults(model.SearchId);
			}
			else if (validate && ModelState.IsValid)
			{
				KeywordSearchOptions searchOptions = new(IsUserLoggedIn(), model.SearchText, false);

				results = _searchProvider.KeywordSearch(searchOptions);
			}

			if (results != null)
			{
				CalculatePageOffsets<SearchResponseItem>(results, Result.Page, _settings.ItemsPerPage,
					out int startItem, out int endItem, out int totalPages);

				Result.TotalPages = totalPages;

				for (int i = startItem; i <= endItem; i++)
				{
					Result.SearchResults.Add(results[i]);
				}

				if (String.IsNullOrEmpty(paginationText))
				{
					Result.Pagination = BuildPagination(results.Count, _settings.ItemsPerPage, Result.Page,
						$"/Search/Result/{Result.RouteText(Result.SearchText)}/", "",
						LanguageStrings.Previous, LanguageStrings.Next);
				}
				else
				{
					Result.Pagination = BuildPagination(results.Count, _settings.ItemsPerPage, Result.Page,
						$"/Search/Advanced/{paginationText}/", "",
						LanguageStrings.Previous, LanguageStrings.Next);
				}
			}

			Result.AdvancedSearch ??= _searchProvider.AdvancedSearch();

			return Result;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591
#pragma warning restore IDE0079
