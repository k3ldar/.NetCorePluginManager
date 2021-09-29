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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Search;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

using pm = PluginManager.Internal;
using sl = Shared.Classes;

#pragma warning disable IDE0059

namespace AspNetCore.PluginManager.Tests.Plugins.SearchTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class KeywordSearchTests : TestBasePlugin
    {
        private const int _searchClassCount = 4;

        [TestInitialize]
        public void InitializeDocumentationLoadTest()
        {
            ThreadManager.Initialise();
            InitializeDocumentationPluginManager();
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
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                IPluginClassesService pluginServices = pluginManager as IPluginClassesService;

                Assert.IsNotNull(pluginServices);

                List<Type> classTypes = pluginServices.GetPluginClassTypes<ISearchKeywordProvider>();

                Assert.AreEqual(2, classTypes.Count);

                Assert.AreEqual("AspNetCore.PluginManager.Tests.Search.Mocks.MockKeywordSearchProviderA", classTypes[0].FullName);
            }

        }

        [TestMethod]
        public void FindAllProvidersIncludingDocumentationPluginProvider()
        {
            using (TestPluginManager pluginManager = new TestPluginManager())
            {
                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                IPluginClassesService pluginServices = pluginManager as IPluginClassesService;

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
            Assert.IsFalse(sl.ThreadManager.Exists(DocumentationLoadThread));

            IDocumentationService documentationService = (IDocumentationService)_testPluginDocs.GetServiceProvider()
                .GetService(typeof(IDocumentationService));

            Assert.IsNotNull(documentationService);

            Assert.IsTrue(documentationService.GetDocuments().Count > 100);
        }

        [TestMethod]
        public void QuickSearchFindAllKeywordsModularLoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _testPluginDocs.GetPluginClasses<ISearchKeywordProvider>();

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
            List<ISearchKeywordProvider> classTypes = _testPluginDocs.GetPluginClasses<ISearchKeywordProvider>();

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
            List<ISearchKeywordProvider> classTypes = _testPluginDocs.GetPluginClasses<ISearchKeywordProvider>();

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
            List<ISearchKeywordProvider> classTypes = _testPluginDocs.GetPluginClasses<ISearchKeywordProvider>();

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
            List<ISearchKeywordProvider> classTypes = _testPluginDocs.GetPluginClasses<ISearchKeywordProvider>();

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

            Assert.IsTrue(Results.Count > 30);
        }

        [TestMethod]
        public void QuickSearchFindAllKeywordProductLoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _testPluginDocs.GetPluginClasses<ISearchKeywordProvider>();

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

            Assert.IsTrue(Results[6].Url.Equals("/Product/5/Product-E/"));
        }

        [TestMethod]
        public void NormalSearchFindAllKeywordProdALoggedOut()
        {
            List<ISearchKeywordProvider> classTypes = _testPluginDocs.GetPluginClasses<ISearchKeywordProvider>();

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

            Assert.IsTrue(Results[Results.Count - 1].Url.Equals("/Product/1/Product-A/"));
        }
    }
}

#pragma warning restore IDE0059