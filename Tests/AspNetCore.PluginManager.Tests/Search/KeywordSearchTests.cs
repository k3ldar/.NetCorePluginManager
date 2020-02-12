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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: KeywordSearchTests.cs
 *
 *  Purpose:  Tests for Keyword Search
 *
 *  Date        Name                Reason
 *  03/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Search;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using pm = PluginManager.Internal;

#pragma warning disable IDE0059

namespace AspNetCore.PluginManager.Tests.Search
{
    [TestClass]
    public sealed class KeywordSearchTests
    {
        private static TestSearchPluginManager _documentationLoadPlugin = new TestSearchPluginManager();
        private static bool? _pluginLoaded = null;
        private static IPluginClassesService _pluginServices;
        private static IDocumentationService _documentationService;
        private const int _searchClassCount = 4;

        [TestInitialize]
        public void InitializeDocumentationLoadTest()
        {
            lock (_documentationLoadPlugin)
            {
                while (_pluginLoaded.HasValue && !_pluginLoaded.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoaded.HasValue && _pluginLoaded.Value)
                {
                    return;
                }

                if (_pluginLoaded == null)
                {
                    _pluginLoaded = false;
                }

                _documentationLoadPlugin = new TestSearchPluginManager();
                _documentationLoadPlugin.AddAssembly(Assembly.GetExecutingAssembly());
                _documentationLoadPlugin.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _documentationLoadPlugin.UsePlugin(typeof(DocumentationPlugin.PluginInitialisation));
                _documentationLoadPlugin.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _documentationLoadPlugin.UsePlugin(typeof(ProductPlugin.PluginInitialisation));

                _documentationLoadPlugin.ConfigureServices();

                _pluginServices = new pm.PluginServices(_documentationLoadPlugin) as IPluginClassesService;
                TimeSpan docLoadTime = new TimeSpan(0, 0, 30);
                DateTime startLoadDocs = DateTime.Now;

                while (Shared.Classes.ThreadManager.Exists(SharedPluginFeatures.Constants.DocumentationLoadThread))
                {
                    System.Threading.Thread.Sleep(100);

                    if (DateTime.Now - startLoadDocs > docLoadTime)
                        break;
                }

                Assert.IsFalse(Shared.Classes.ThreadManager.Exists(SharedPluginFeatures.Constants.DocumentationLoadThread));

                _documentationService = (IDocumentationService)_documentationLoadPlugin.GetServiceProvider()
                    .GetService(typeof(IDocumentationService));

                Assert.IsNotNull(_documentationService);

                Assert.IsTrue(_documentationService.GetDocuments().Count > 100);
                _pluginLoaded = true;
            }

            Assert.IsNotNull(_pluginServices);

        }

        [TestMethod]
        public void KeywordLoggedInValidSearchTerm()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(true, "test");
        }

        [TestMethod]
        public void KeywordLoggedOutValidSearchTerm()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(false, "test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeywordLoggedOutInvalidSearchTermNull()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(false, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void KeywordLoggedOutInvalidSearchTermEmptyString()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(false, "");
        }

        [TestMethod]
        public void EnsurePropertiesDictionaryCreated()
        {
            KeywordSearchOptions options = new KeywordSearchOptions(false, "test");

            Assert.IsNotNull(options.Properties);
        }

        [TestMethod]
        public void KeywordSearchFindAllProviders()
        {
            using (TestSearchPluginManager pluginManager = new TestSearchPluginManager())
            {
                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                IPluginClassesService pluginServices = new pm.PluginServices(pluginManager) as IPluginClassesService;

                Assert.IsNotNull(pluginServices);

                List<Type> classTypes = pluginServices.GetPluginClassTypes<ISearchKeywordProvider>();

                Assert.AreEqual(2, classTypes.Count);

                Assert.AreEqual("AspNetCore.PluginManager.Tests.Search.Mocks.MockKeywordSearchProviderA", classTypes[0].FullName);
            }

        }

        [TestMethod]
        public void FindAllProvidersIncludingDocumentationPluginProvider()
        {
            using (TestSearchPluginManager pluginManager = new TestSearchPluginManager())
            {
                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                IPluginClassesService pluginServices = new pm.PluginServices(pluginManager) as IPluginClassesService;

                Assert.IsNotNull(pluginServices);

                List<Type> classTypes = pluginServices.GetPluginClassTypes<ISearchKeywordProvider>();

                Assert.AreEqual(2, classTypes.Count);

                Assert.AreEqual("AspNetCore.PluginManager.Tests.Search.Mocks.MockKeywordSearchProviderA", classTypes[0].FullName);

                pluginManager.AddAssembly(typeof(DocumentationPlugin.PluginInitialisation).Assembly);

                classTypes = pluginServices.GetPluginClassTypes<ISearchKeywordProvider>();

                Assert.AreEqual(3, classTypes.Count);

                Assert.AreEqual("DocumentationPlugin.Classes.KeywordSearchProvider", classTypes[classTypes.Count - 1].FullName);
            }
        }

        [TestMethod]
        public void FindAllProvidersAndRetrieveInstanciatedClasses()
        {
            Assert.IsFalse(Shared.Classes.ThreadManager.Exists(SharedPluginFeatures.Constants.DocumentationLoadThread));

            IDocumentationService documentationService = (IDocumentationService)_documentationLoadPlugin.GetServiceProvider()
                .GetService(typeof(IDocumentationService));

            Assert.IsNotNull(documentationService);

            Assert.IsTrue(documentationService.GetDocuments().Count > 100);
        }

        [TestMethod]
        public void QuickSearchFindAllKeywordsModularLoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _pluginServices.GetPluginClasses<ISearchKeywordProvider>();

            Assert.AreEqual(_searchClassCount, classTypes.Count);

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in classTypes)
            {
                List<SearchResponseItem> keywords = keywordProvider.Search(new KeywordSearchOptions("Modular", true));

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                        Results.Add(searchResult);
                }
            }

            Assert.AreEqual(3, Results.Count);

            int countWithUri = Results.Where(r => r.Url != null).Count();

            Assert.AreEqual(1, countWithUri);
        }

        [TestMethod]
        public void QuickSearchValidOffset()
        {
            List<ISearchKeywordProvider> classTypes = _pluginServices.GetPluginClasses<ISearchKeywordProvider>();

            Assert.AreEqual(_searchClassCount, classTypes.Count);

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in classTypes)
            {
                List<SearchResponseItem> keywords = keywordProvider.Search(new KeywordSearchOptions(true, "modular", true));

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                        Results.Add(searchResult);
                }
            }

            Assert.AreEqual(4, Results.Count);

            int countWithUri = Results.Where(r => r.Url != null).Count();

            Assert.AreEqual(1, countWithUri);

            Assert.AreEqual(-1, Results[0].Offset);
            Assert.AreEqual(-1, Results[1].Offset);
            Assert.AreEqual(-1, Results[2].Offset);
            Assert.AreEqual(2, Results[3].Offset);
        }

        [TestMethod]
        public void QuickSearchFindAllKeywordModularLoggedIn()
        {
            List<ISearchKeywordProvider> classTypes = _pluginServices.GetPluginClasses<ISearchKeywordProvider>();

            Assert.AreEqual(_searchClassCount, classTypes.Count);

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in classTypes)
            {
                List<SearchResponseItem> keywords = keywordProvider.Search(new KeywordSearchOptions(true, "modular", true));

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                        Results.Add(searchResult);
                }
            }

            Assert.AreEqual(4, Results.Count);

            int countWithUri = Results.Where(r => r.Url != null).Count();

            Assert.AreEqual(1, countWithUri);
        }

        [TestMethod]
        public void QuickSearchFindAllKeywordPluginLoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _pluginServices.GetPluginClasses<ISearchKeywordProvider>();

            Assert.AreEqual(_searchClassCount, classTypes.Count);

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in classTypes)
            {
                List<SearchResponseItem> keywords = keywordProvider.Search(new KeywordSearchOptions("Plugin", true));

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                        Results.Add(searchResult);
                }
            }

            Assert.IsTrue(Results.Count > 25);
        }

        [TestMethod]
        public void NormalSearchFindAllKeywordPluginLoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _pluginServices.GetPluginClasses<ISearchKeywordProvider>();

            Assert.AreEqual(_searchClassCount, classTypes.Count);

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in classTypes)
            {
                List<SearchResponseItem> keywords = keywordProvider.Search(new KeywordSearchOptions("Plugin", false));

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                        Results.Add(searchResult);
                }
            }

            Assert.IsTrue(Results.Count > 60);
        }

        [TestMethod]
        public void QuickSearchFindAllKeywordProductLoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _pluginServices.GetPluginClasses<ISearchKeywordProvider>();

            Assert.AreEqual(_searchClassCount, classTypes.Count);

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in classTypes)
            {
                List<SearchResponseItem> keywords = keywordProvider.Search(new KeywordSearchOptions("product ", true));

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                        Results.Add(searchResult);
                }
            }

            Assert.IsTrue(Results.Count > 6);

            Assert.IsTrue(Results[6].Url.Equals("Product/5/Product-E/"));
        }

        [TestMethod]
        public void NormalSearchFindAllKeywordProdALoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _pluginServices.GetPluginClasses<ISearchKeywordProvider>();

            Assert.AreEqual(_searchClassCount, classTypes.Count);

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            foreach (ISearchKeywordProvider keywordProvider in classTypes)
            {
                List<SearchResponseItem> keywords = keywordProvider.Search(new KeywordSearchOptions(false, "PrODA", false, true));

                if (keywords != null)
                {
                    foreach (SearchResponseItem searchResult in keywords)
                        Results.Add(searchResult);
                }
            }

            Assert.AreEqual(1, Results.Count);

            Assert.IsTrue(Results[Results.Count - 1].Url.Equals("Product/1/Product-A/"));
        }
    }
}

#pragma warning restore IDE0059