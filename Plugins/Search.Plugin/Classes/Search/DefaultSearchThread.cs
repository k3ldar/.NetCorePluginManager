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
 *  File: DefaultSearchThread.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Search;

using Shared.Classes;

namespace SearchPlugin.Classes.Search
{
    /// <summary>
    /// The default thread item to be used for performing searches
    /// </summary>
    public class DefaultSearchThread : ThreadManager
    {
        #region Private Members

        private readonly List<ISearchKeywordProvider> _searchKeywordProviders;
        private readonly KeywordSearchOptions _keywordSearchOptions;
        private readonly CacheManager _searchCache;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultSearchThread(in List<ISearchKeywordProvider> searchProviders,
            in CacheManager searchCache,
            in KeywordSearchOptions keywordSearchOptions)
            : base(null, new TimeSpan(), null, 0, 0, true, true)
        {
            _searchKeywordProviders = searchProviders ?? throw new ArgumentNullException(nameof(searchProviders));
            _searchCache = searchCache ?? throw new ArgumentNullException(nameof(searchCache));
            _keywordSearchOptions = keywordSearchOptions ?? throw new ArgumentNullException(nameof(keywordSearchOptions));
        }

        #endregion Constructors

        #region Overridden Methods

        /// <summary>
        /// Method that executes a search in a dedicated thread
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>bool</returns>
        protected override Boolean Run(object parameters)
        {
            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in _searchKeywordProviders)
            {
                if (HasCancelled())
                    return false;

                List<SearchResponseItem> keywords = keywordProvider.Search(_keywordSearchOptions);

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                    {
                        if (HasCancelled())
                            return false;

                        Results.Add(searchResult);
                    }
                }
            }

            CacheItem cacheItem = new CacheItem(Name, Results);
            _searchCache.Add(Name, cacheItem, true);

            return false;
        }

        #endregion Overridden Methods
    }
}
