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

using Shared.Classes;

using SharedPluginFeatures;

namespace Middleware.Search
{
    /// <summary>
    /// The default thread item to be used for performing searches
    /// </summary>
    public class DefaultSearchThread : ThreadManager
    {
        #region Private Members

        private readonly List<ISearchKeywordProvider> _searchKeywordProviders;
        private readonly KeywordSearchOptions _keywordSearchOptions;
        private static readonly CacheManager _searchCache = new CacheManager("Search Cache", new TimeSpan(0, 20, 0), true, true);
        private static readonly object _lockObject = new object();
        private static readonly Timings _searchTimings = new Timings();

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        private DefaultSearchThread(in List<ISearchKeywordProvider> searchProviders,
            in KeywordSearchOptions keywordSearchOptions)
            : base(null, new TimeSpan(), null, 0, 0, true, true)
        {
            _searchKeywordProviders = searchProviders ?? throw new ArgumentNullException(nameof(searchProviders));
            _keywordSearchOptions = keywordSearchOptions ?? throw new ArgumentNullException(nameof(keywordSearchOptions));
        }

        #endregion Constructors

        #region Static Properties

        /// <summary>
        /// Default search cache item
        /// </summary>
        public static CacheManager SearchCache
        {
            get
            {
                return _searchCache;
            }
        }

        /// <summary>
        /// Retrieves the timings for searches
        /// </summary>
        /// <value>Timings</value>
        public static Timings SearchTimings
        {
            get
            {
                return _searchTimings;
            }
        }

        #endregion Static Properties

        #region Static Methods

        /// <summary>
        /// Default keyword search using the specified options and parameters.
        /// </summary>
        /// <param name="searchProviders">List of search providers to be searched.</param>
        /// <param name="keywordSearchOptions">Keyword search options</param>
        /// <returns>List&lt;SearchResponseItem&gt;</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Exceptions are meant for developers not end users.")]
        public static List<SearchResponseItem> KeywordSearch(in List<ISearchKeywordProvider> searchProviders,
            in KeywordSearchOptions keywordSearchOptions)
        {
            using (StopWatchTimer timer = new StopWatchTimer(_searchTimings))
            {
                if (keywordSearchOptions == null)
                {
                    throw new ArgumentNullException(nameof(keywordSearchOptions));
                }

                CacheItem cacheItem = _searchCache.Get(keywordSearchOptions.SearchName);

                using (TimedLock timedLock = TimedLock.Lock(_lockObject))
                {
                    if (cacheItem == null)
                    {
                        if (!ThreadManager.Exists(keywordSearchOptions.SearchName))
                        {
                            DefaultSearchThread searchThread = new DefaultSearchThread(searchProviders, keywordSearchOptions);
                            ThreadStart(searchThread, keywordSearchOptions.SearchName, System.Threading.ThreadPriority.BelowNormal);
                        }
                    }
                }

                DateTime searchStart = DateTime.Now;

                while (true)
                {
                    TimeSpan span = DateTime.Now - searchStart;

                    if (span.TotalMilliseconds > keywordSearchOptions.Timeout)
                    {
                        throw new TimeoutException("Search timed out");
                    }

                    cacheItem = _searchCache.Get(keywordSearchOptions.SearchName);

                    if (cacheItem != null)
                    {
                        break;
                    }
                }

                return (List<SearchResponseItem>)cacheItem.Value;
            }
        }

        /// <summary>
        /// Default keyword search using the specified options and parameters.
        /// </summary>
        /// <param name="searchProvider">Search providers to be searched.</param>
        /// <param name="keywordSearchOptions">Keyword search options</param>
        /// <returns>List&lt;SearchResponseItem&gt;</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Exceptions are meant for developers not end users.")]
        public static List<SearchResponseItem> KeywordSearch(in ISearchKeywordProvider searchProvider,
            in KeywordSearchOptions keywordSearchOptions)
        {
            using (StopWatchTimer timer = new StopWatchTimer(_searchTimings))
            {
                if (keywordSearchOptions == null)
                {
                    throw new ArgumentNullException(nameof(keywordSearchOptions));
                }

                CacheItem cacheItem = _searchCache.Get(keywordSearchOptions.SearchName);

                using (TimedLock timedLock = TimedLock.Lock(_lockObject))
                {
                    if (cacheItem == null)
                    {
                        if (!ThreadManager.Exists(keywordSearchOptions.SearchName))
                        {
                            List<ISearchKeywordProvider> searchProviders = new List<ISearchKeywordProvider>()
                            {
                                { searchProvider }
                            };

                            DefaultSearchThread searchThread = new DefaultSearchThread(searchProviders, keywordSearchOptions);
                            ThreadStart(searchThread, keywordSearchOptions.SearchName, System.Threading.ThreadPriority.BelowNormal);
                        }
                    }
                }

                DateTime searchStart = DateTime.Now;

                while (true)
                {
                    TimeSpan span = DateTime.Now - searchStart;

                    if (span.TotalMilliseconds > keywordSearchOptions.Timeout)
                    {
                        throw new TimeoutException("Search timed out");
                    }

                    cacheItem = _searchCache.Get(keywordSearchOptions.SearchName);

                    if (cacheItem != null)
                    {
                        break;
                    }
                }

                return (List<SearchResponseItem>)cacheItem.Value;
            }
        }

        /// <summary>
        /// Retrieve search data based on the name of the search, if it exists.
        /// </summary>
        /// <param name="searchName"></param>
        /// <returns>List&lt;SearchResponseItem&gt;</returns>
        public static List<SearchResponseItem> RetrieveSearch(in string searchName)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                CacheItem cache = _searchCache.Get(searchName);

                if (cache == null)
                {
                    return null;
                }

                return (List<SearchResponseItem>)cache.Value;
            }
        }

        #endregion Static Methods

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
