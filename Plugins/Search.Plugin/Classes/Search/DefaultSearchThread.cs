using System;
using System.Collections.Generic;
using Middleware;
using Middleware.Search;
using Shared.Classes;
using SharedPluginFeatures;

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
        private readonly IMemoryCache _memoryCache;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultSearchThread(in List<ISearchKeywordProvider> searchProviders, 
            in IMemoryCache memoryCache,
            in KeywordSearchOptions keywordSearchOptions)
            : base(null, new TimeSpan(), null, 0, 0, true, true)
        {
            _searchKeywordProviders = searchProviders ?? throw new ArgumentNullException(nameof(searchProviders));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
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
            _memoryCache.GetCache().Add(Name, cacheItem, true);

            return false;
        }

        #endregion Overridden Methods
    }
}
