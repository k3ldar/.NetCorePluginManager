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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: DefaultSearchProviderTests.cs
 *
 *  Purpose:  Test for default search provider
 *
 *  Date        Name                Reason
 *  10/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Search;

using PluginManager.Abstractions;

using SearchPlugin.Classes.Search;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.SearchTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultSearchProviderTests : MockBasePlugin
    {
        [TestInitialize]
        public void InitializeSearchTests()
        {
            for (int i = CacheManager.GetCount() -1; i >= 0 ; i--)
            {
                string cacheName = CacheManager.GetCacheName(i);
                Debug.Print($"Removing Cache: {cacheName}");
                CacheManager.RemoveCacheManager(cacheName);
            }
         
            //InitializeSearchPluginManager();
            ThreadManager.Initialise();
            InitializeDocumentationPluginManager();
        }

        [TestCleanup]
        public void FinalizeSearchTests()
        {
            ThreadManager.Finalise();
        }

        [TestMethod]
		public void NormalSearchFindAllKeywordProdALoggedOut()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            Assert.IsNotNull(pluginServices);

            List<ISearchProvider> searchProviders = pluginServices.GetPluginClasses<ISearchProvider>();

            Assert.AreEqual(1, searchProviders.Count);

            List<SearchResponseItem> results = searchProviders[0].KeywordSearch(new KeywordSearchOptions(false, "PrODA", false, true));

            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        public void NormalSearchFindAllKeywordProdALoggedOutSavedToMemory()
        {
            KeywordSearchOptions keywordSearchOptions = new KeywordSearchOptions(false, "PrODA", false, true);

            string cacheName = String.Format("Keyword Search {0} {1} {2} {3} {4} {5}",
                keywordSearchOptions.IsLoggedIn, keywordSearchOptions.ExactMatch,
                keywordSearchOptions.MaximumSearchResults, keywordSearchOptions.Timeout,
                keywordSearchOptions.SearchTerm, keywordSearchOptions.QuickSearch);


            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            Assert.IsNotNull(pluginServices);

            List<ISearchProvider> searchProviders = pluginServices.GetPluginClasses<ISearchProvider>();

            Assert.AreEqual(1, searchProviders.Count);
            IMemoryCache memoryCache = _testPluginDocs.GetRequiredService<IMemoryCache>();
            memoryCache.GetCache().Clear();
            Assert.IsNull(memoryCache.GetCache().Get(cacheName));

            List<SearchResponseItem> results = searchProviders[0].KeywordSearch(keywordSearchOptions);


            Assert.AreEqual(1, results.Count);

            DefaultSearchProvider defaultSearchProvider = (DefaultSearchProvider)searchProviders[0];

            Assert.IsNotNull(defaultSearchProvider.GetCacheManager.Get(cacheName));
        }

        [TestMethod]
        public void RetrieveAvailableSearchResponseTypesQuickSearch()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            Assert.IsNotNull(pluginServices);

            List<ISearchProvider> searchProviders = pluginServices.GetPluginClasses<ISearchProvider>();

            Assert.AreEqual(1, searchProviders.Count);

            List<string> results = searchProviders[0].SearchResponseTypes(true);

            Assert.IsTrue(results.Count > 3);

            Assert.IsTrue(results.Contains("TestProviderA"));
            Assert.IsTrue(results.Contains("TestProviderB"));
            Assert.IsTrue(results.Contains("DocumentTitle"));
            Assert.IsTrue(results.Contains("ProductName"));
        }

        [TestMethod]
        public void RetrieveAvailableSearchResponseTypesNormalSearch()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            Assert.IsNotNull(pluginServices);

            List<ISearchProvider> searchProviders = pluginServices.GetPluginClasses<ISearchProvider>();

            Assert.AreEqual(1, searchProviders.Count);

            List<string> results = searchProviders[0].SearchResponseTypes(false);

            Assert.IsTrue(results.Count > 6);

            Assert.IsFalse(results.Contains("TestProviderA"));
            Assert.IsFalse(results.Contains("TestProviderB"));
            Assert.IsTrue(results.Contains("DocumentTitle"));
            Assert.IsTrue(results.Contains("ProductName"));
            Assert.IsTrue(results.Contains("ProductDescription"));
            Assert.IsTrue(results.Contains("ProductSku"));
            Assert.IsTrue(results.Contains("DocumentSummary"));
            Assert.IsTrue(results.Contains("DocumentLongShortDescription"));
            Assert.IsTrue(results.Contains("DocumentLongDescription"));
        }

        [TestMethod]
        public void RetrieveAvailableSearchNamesForAdvancedSearchingOptions()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            Assert.IsNotNull(pluginServices);

            List<ISearchProvider> searchProviders = pluginServices.GetPluginClasses<ISearchProvider>();

            Assert.AreEqual(1, searchProviders.Count);

            List<string> results = searchProviders[0].SearchResponseTypes(false);

            Assert.IsTrue(results.Count < 10);

            Assert.IsFalse(results.Contains("TestProviderA"));
            Assert.IsFalse(results.Contains("TestProviderB"));
            Assert.IsTrue(results.Contains("ProductName"));
        }
    }
}
