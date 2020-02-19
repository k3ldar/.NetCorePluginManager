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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
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

using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Search;

using PluginManager.Abstractions;

using SearchPlugin.Models;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SearchPlugin.Controllers
{
    /// <summary>
    /// Search controller, allows users to search using a standard interface implemented by ISearchProvider interface.
    /// </summary>
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

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.Search))]
        [LoggedInOut]
        public IActionResult Index()
        {
            SearchViewModel model = new SearchViewModel(GetModelData());

            return View(model);
        }

        [HttpPost]
        [BadEgg]
        [LoggedInOut]
        public IActionResult Index(SearchViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            return View(model);
        }

        [HttpPost]
        [BadEgg]
        [LoggedInOut]
        public IActionResult QuickKeywordSearch(string keywords)
        {
            if (String.IsNullOrWhiteSpace(keywords) || keywords.Length < _settings.MinimumKeywordSearchLength)
            {
                return new StatusCodeResult(400);
            }

            List<SearchResponseItem> searchResults = _searchProvider.KeywordSearch(new KeywordSearchOptions(IsUserLoggedIn(), keywords, true));

            return new JsonResult(searchResults) 
            { 
                StatusCode = 200,
                ContentType = "application/json"
            };
        }

        #endregion Public Action Methods

        #region Private Methods

        #endregion Private Methods
    }
}

#pragma warning restore CS1591