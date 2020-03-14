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
 *  File: DefaultSearchProvider.cs
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

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace SearchPlugin.Classes.Search
{
    /// <summary>
    /// Default search provider to be used if no other search provider is registered
    /// </summary>
    public class DefaultSearchProvider : ISearchProvider
    {
        #region Private Members

        private readonly CacheManager _searchCache;
        private readonly IPluginClassesService _pluginClassesService;
        private readonly List<ISearchKeywordProvider> _searchProviders;
        private static readonly object _lockObject = new object();
        private static readonly Timings _searchTimings = new Timings();

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pluginClassesService">IPluginClassesService instance</param>
        public DefaultSearchProvider(IPluginClassesService pluginClassesService)
        {
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));

            _searchCache = new CacheManager("Search Cache", new TimeSpan(0, 20, 0), true, true);
            _searchProviders = _pluginClassesService.GetPluginClasses<ISearchKeywordProvider>();
        }

        #endregion Constructors

        #region ISearchProvider Methods

        /// <summary>
        /// Performs a keyword search
        /// </summary>
        /// <param name="keywordSearchOptions"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Exceptions are meant for developers not end users.")]
        public List<SearchResponseItem> KeywordSearch(in KeywordSearchOptions keywordSearchOptions)
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
                            DefaultSearchThread searchThread = new DefaultSearchThread(_searchProviders, _searchCache, keywordSearchOptions);
                            ThreadManager.ThreadStart(searchThread, keywordSearchOptions.SearchName, System.Threading.ThreadPriority.BelowNormal);
                        }
                    }
                }

                DateTime searchStart = DateTime.Now;

                while (true)
                {
                    TimeSpan span = DateTime.Now - searchStart;

                    if (span.TotalMilliseconds > keywordSearchOptions.Timeout)
                    {
                        throw new TimeoutException("Timed out waiting for search to complete");
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
        /// Retrieves all available search response types from all registered search providers
        /// </summary>
        /// <param name="quickSearch">Indicates whether its the providers from a quick search or normal search.</param>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> SearchResponseTypes(in Boolean quickSearch)
        {
            List<string> Result = new List<string>();

            foreach (ISearchKeywordProvider provider in _searchProviders)
            {
                List<string> providerResults = provider.SearchResponseTypes(quickSearch);

                if (providerResults == null)
                    continue;

                providerResults.ForEach(st => Result.Add(st));
            }

            return Result;
        }

        /// <summary>
        /// Retrieves a list of strings from all search providers that can optionally be used by the UI 
        /// to provide a paged or tabbed advance search option.
        /// </summary>
        /// <returns>List&lt;string&gt;</returns>
        public Dictionary<string, string> SearchNames()
        {
            Dictionary<string, string> Result = new Dictionary<string, string>();

            foreach (ISearchKeywordProvider provider in _searchProviders)
            {
                Dictionary<string, string> name = provider.SearchName();

                if (name == null)
                    continue;

                foreach (KeyValuePair<String, String> item in name)
                    Result.Add(item.Key, item.Value);
            }

            return Result;
        }

        #endregion ISearchProvider Methods

        #region Properties

        internal static Timings SearchTimings
        {
            get
            {
                return _searchTimings;
            }
        }

        internal CacheManager GetCacheManager
        {
            get
            {
                return _searchCache;
            }
        }

        #endregion Properties
    }
}
