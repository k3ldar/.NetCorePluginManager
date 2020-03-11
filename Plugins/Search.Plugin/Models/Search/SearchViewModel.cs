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
        }

        public SearchViewModel(string searchText, int page)
        {
            SearchText = searchText ?? String.Empty;
            Page = page;
        }

        public SearchViewModel(in BaseModelData modelData, Dictionary<string, string> searchNames)
            : base(modelData)
        {
            Page = 1;
            SearchNames = searchNames ?? throw new ArgumentNullException(nameof(searchNames));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Text to be searched for
        /// </summary>
        [Display(Name = nameof(Languages.LanguageStrings.SearchDescription))]
        [StringLength(200, MinimumLength = 3)]
        [Required(ErrorMessage = nameof(Languages.LanguageStrings.SearchInvalid))]
        public string SearchText { get; set; }

        /// <summary>
        /// Id of the current search, this will be used when verifying that the call is legit
        /// </summary>
        /// <value>string</value>
        public string SearchId { get; set; }

        public Dictionary<string, string> SearchNames { get; set; }

        /// <summary>
        /// Available search results from users search
        /// </summary>
        /// <value>List&lt;SearchResponseItem&gt;</value>
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
        /// Name of the active tab for advanced searching
        /// </summary>
        public string ActiveTab { get; set; }

        #endregion Properties
    }
}

#pragma warning restore CS1591