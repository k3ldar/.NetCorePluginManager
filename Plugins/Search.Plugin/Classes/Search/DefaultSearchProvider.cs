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

        private readonly IMemoryCache _memoryCache;
        private readonly IPluginClassesService _pluginClassesService;
        private readonly List<ISearchKeywordProvider> _searchProviders;
        private static readonly object _lockObject = new object();

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="memoryCache">IMemoryCache instance</param>
        /// <param name="pluginClassesService">IPluginClassesService instance</param>
        public DefaultSearchProvider(IMemoryCache memoryCache, IPluginClassesService pluginClassesService)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _pluginClassesService = pluginClassesService ?? throw new ArgumentNullException(nameof(pluginClassesService));

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
            if (keywordSearchOptions == null)
            {
                throw new ArgumentNullException(nameof(keywordSearchOptions));
            }

            string cacheName = String.Format("Keyword Search {0} {1} {2} {3} {4}",
                keywordSearchOptions.IsLoggedIn, keywordSearchOptions.ExactMatch,
                keywordSearchOptions.MaximumSearchResults, keywordSearchOptions.Timeout,
                keywordSearchOptions.SearchTerm);

            CacheItem cacheItem = _memoryCache.GetCache().Get(cacheName);

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                if (cacheItem == null)
                {
                    if (!ThreadManager.Exists(cacheName))
                    {
                        DefaultSearchThread searchThread = new DefaultSearchThread(_searchProviders, _memoryCache, keywordSearchOptions);
                        ThreadManager.ThreadStart(searchThread, cacheName, System.Threading.ThreadPriority.BelowNormal);
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

                cacheItem = _memoryCache.GetCache().Get(cacheName);

                if (cacheItem != null)
                {
                    break;
                }
            }

            return (List<SearchResponseItem>)cacheItem.Value;
        }

        #endregion ISearchProvider Methods
    }
}
