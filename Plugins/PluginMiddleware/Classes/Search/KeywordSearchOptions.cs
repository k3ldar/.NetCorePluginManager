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
 *  Product:  Plugin Middleware
 *  
 *  File: KeywordSearchOptions.cs
 *
 *  Purpose:  Keyword search options
 *
 *  Date        Name                Reason
 *  02/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Search
{
    /// <summary>
    /// Keyword search request
    /// </summary>
    public class KeywordSearchOptions : BaseSearchOptions
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isLoggedIn">Indicates that the search is being completed from a user who is logged in.</param>
        /// <param name="searchTerm">The search term being sought.</param>
        /// <param name="quickSearch">Indicates a quick search should be completed, i.e. searching titles instead of biography etc.</param>
        /// <param name="exactMatch">Determines whether the search should be an exact match or not.</param>
        public KeywordSearchOptions(in bool isLoggedIn, in string searchTerm, in bool quickSearch, in bool exactMatch)
            : base(isLoggedIn, searchTerm)
        {
            Timeout = 30000;
            Properties = new Dictionary<string, object>();
            QuickSearch = quickSearch;
            MaximumSearchResults = 100;
            ExactMatch = exactMatch;
            SearchName = String.Format("Keyword Search {0} {1} {2} {3} {4} {5}",
                IsLoggedIn,
                ExactMatch,
                MaximumSearchResults,
                Timeout,
                SearchTerm,
                QuickSearch);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isLoggedIn">Indicates that the search is being completed from a user who is logged in.</param>
        /// <param name="searchTerm">The search term being sought.</param>
        /// <param name="quickSearch">Indicates a quick search should be completed, i.e. searching titles instead of biography etc.</param>
        public KeywordSearchOptions(in bool isLoggedIn, in string searchTerm, in bool quickSearch)
            : this(isLoggedIn, searchTerm, quickSearch, false)
        {
            Timeout = 30000;
            Properties = new Dictionary<string, object>();
            QuickSearch = quickSearch;
            MaximumSearchResults = 100;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="searchTerm">The search term being sought.</param>
        /// <param name="quickSearch">Indicates a quick search should be completed, i.e. searching titles instead of biography etc.</param>
        public KeywordSearchOptions(in string searchTerm, in bool quickSearch)
            : this(false, searchTerm, quickSearch)
        {
            Timeout = 30000;
            Properties = new Dictionary<string, object>();
            QuickSearch = quickSearch;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isLoggedIn">Indicates that the search is being completed from a user who is logged in.</param>
        /// <param name="searchTerm">The search term being sought.</param>
        public KeywordSearchOptions(in bool isLoggedIn, in string searchTerm)
            : this(isLoggedIn, searchTerm, false)
        {
            Timeout = 30000;
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constructor for logged out user
        /// </summary>
        /// <param name="searchTerm">The search term being sought.</param>
        public KeywordSearchOptions(in string searchTerm)
            : this(false, searchTerm)
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Timeout in milliseconds, if implemented then after the timeout period an error should be raised.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Indicates that searching should be based on pattern matching.
        /// </summary>
        /// <value>bool</value>
        public bool PatternMatching { get; set; }

        /// <summary>
        /// Dictionary of properties, these are user defined on the premise that the writer of the propery will know.
        /// and check for it's type before using it.
        /// </summary>
        /// <value>Dictionary&lt;string, object&gt;</value>
        public Dictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// Indicates that a quick search should be completed, this should be based on titles etc
        /// </summary>
        /// <value>bool</value>
        public bool QuickSearch { get; set; }

        /// <summary>
        /// Maximum number of search results to be returned, default value is 100
        /// </summary>
        /// <value>int</value>
        public int MaximumSearchResults { get; set; }

        /// <summary>
        /// Determines whether the search should be an exact match or not.
        /// </summary>
        /// <value>bool</value>
        public bool ExactMatch { get; set; }

        /// <summary>
        /// Name of the search prefix, this is used by other search providers who can create a search and prefix it so as to return afterwards
        /// </summary>
        public string SearchName { get; set; }

        #endregion Properties
    }
}
