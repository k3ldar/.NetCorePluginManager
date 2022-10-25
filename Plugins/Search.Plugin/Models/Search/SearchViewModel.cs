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
 *  Product:  Search Plugin
 *  
 *  File: SearchModel.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  01/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Middleware.Search;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SearchPlugin.Models
{
    /// <summary>
    /// View model for searching all ISearchProvider 
    /// </summary>
    public sealed class SearchViewModel : BaseModel
    {
        #region Constructors

        public SearchViewModel()
        {
            Page = 1;
            ActiveTab = String.Empty;
        }

        public SearchViewModel(string searchText, int page)
        {
            SearchText = searchText ?? String.Empty;
            Page = page;
            ActiveTab = String.Empty;
        }

        public SearchViewModel(in BaseModelData modelData, Dictionary<string, AdvancedSearchOptions> searchNames)
            : base(modelData)
        {
            Page = 1;
            AdvancedSearch = searchNames ?? throw new ArgumentNullException(nameof(searchNames));
            ActiveTab = String.Empty;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Text to be searched for
        /// </summary>
        /// <value>string</value>
        [Display(Name = nameof(Languages.LanguageStrings.SearchDescription))]
        [StringLength(200, MinimumLength = 3)]
        [Required(ErrorMessage = nameof(Languages.LanguageStrings.SearchInvalid))]
        public string SearchText { get; set; }

        /// <summary>
        /// Id of the current search, this will be used when verifying that the call is legit
        /// </summary>
        /// <value>string</value>
        public string SearchId { get; set; }

        /// <summary>
        /// Available search names for display on search page
        /// </summary>
        /// <value>Dictionary&lt;string, string&gt;</value>
        public Dictionary<string, AdvancedSearchOptions> AdvancedSearch { get; set; }

        /// <summary>
        /// Retrieves the url for the active search name if it exists.
        /// </summary>
        /// <value>string</value>
        public string SearchName
        {
            get
            {
                if (AdvancedSearch != null && AdvancedSearch.ContainsKey(ActiveTab))
                {
                    return AdvancedSearch[ActiveTab].SearchName;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Retrieves the url for the active search option, if it exists.
        /// </summary>
        /// <value>string</value>
        public string SearchOption
        {
            get
            {
                if (AdvancedSearch != null && AdvancedSearch.ContainsKey(ActiveTab))
                {
                    return AdvancedSearch[ActiveTab].SearchOption;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Retrieves the name of the controller for the active search option, if it exists.
        /// </summary>
        /// <value>string</value>
        public string ControllerName
        {
            get
            {
                if (AdvancedSearch != null && AdvancedSearch.ContainsKey(ActiveTab))
                {
                    return AdvancedSearch[ActiveTab].ControllerName;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Retrieves the name of the action for the active search option, if it exists.
        /// </summary>
        /// <value>string</value>
        public string ActionName
        {
            get
            {
                if (AdvancedSearch != null && AdvancedSearch.ContainsKey(ActiveTab))
                {
                    return AdvancedSearch[ActiveTab].ActionName;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Retrieves an optional style sheet that can be used by the search provider.
        /// </summary>
        /// <value>string</value>
        public string StyleSheet
        {
            get
            {
                if (AdvancedSearch != null && AdvancedSearch.ContainsKey(ActiveTab))
                {
                    return AdvancedSearch[ActiveTab].StyleSheet;
                }

                return String.Empty;
            }
        }

        /// <summary>
        /// Available search results from users search
        /// </summary>
        /// <value>List&lt;<see cref="SearchResponseItem"/> SearchResponseItem&gt;</value>
        public List<SearchResponseItem> SearchResults { get; internal set; }

        /// <summary>
        /// Current page of search items
        /// </summary>
        /// <value>int</value>
        public int Page { get; set; }

        /// <summary>
        /// Total number of pages for this search
        /// </summary>
        /// <value>int</value>
        public int TotalPages { get; set; }

        /// <summary>
        /// Contains the html used for paging search results.
        /// </summary>
        /// <value>string</value>
        public string Pagination { get; set; }

        /// <summary>
        /// Name of the active tab for advanced searching.
        /// </summary>
        /// <value>string</value>
        public string ActiveTab { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591