using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Middleware.Search;
using SearchPlugin.Classes.Search;
using System.Linq;
using System.Reflection;


using PluginManager.Abstractions;

using SharedPluginFeatures;

using pm = PluginManager.Internal;
using Middleware;
using Constants = SharedPluginFeatures.Constants;

namespace AspNetCore.PluginManager.Tests.Search
{
    [TestClass]
    public class DefaultSearchProviderTests
    {
        private static TestSearchPluginManager _testPlugin = new TestSearchPluginManager();
        private static bool? _pluginLoaded = null;
        private static IPluginClassesService _pluginServices;
        private static IDocumentationService _documentationService;


        [TestInitialize]
        public void InitializeDocumentationLoadTest()
        {
            lock (_testPlugin)
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

                _testPlugin = new TestSearchPluginManager();
                _testPlugin.AddAssembly(Assembly.GetExecutingAssembly());
                _testPlugin.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(DocumentationPlugin.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(ProductPlugin.PluginInitialisation));
                _testPlugin.UsePlugin(typeof(SearchPlugin.PluginInitialisation));

                _testPlugin.ConfigureServices();

                _pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
                TimeSpan docLoadTime = new TimeSpan(0, 0, 30);
                DateTime startLoadDocs = DateTime.Now;

                while (Shared.Classes.ThreadManager.Exists(SharedPluginFeatures.Constants.DocumentationLoadThread))
                {
                    System.Threading.Thread.Sleep(100);

                    if (DateTime.Now - startLoadDocs > docLoadTime)
                        break;
                }

                Assert.IsFalse(Shared.Classes.ThreadManager.Exists(Constants.DocumentationLoadThread));

                _documentationService = (IDocumentationService)_testPlugin.GetServiceProvider()
                    .GetService(typeof(IDocumentationService));

                Assert.IsNotNull(_documentationService);

                Assert.IsTrue(_documentationService.GetDocuments().Count > 100);
                _pluginLoaded = true;
            }

            Assert.IsNotNull(_pluginServices);

        }

        [TestMethod]
        public void NormalSearchFindAllKeywordProdALoggedOut()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

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

            string cacheName = String.Format("Keyword Search {0} {1} {2} {3} {4}",
                keywordSearchOptions.IsLoggedIn, keywordSearchOptions.ExactMatch,
                keywordSearchOptions.MaximumSearchResults, keywordSearchOptions.Timeout,
                keywordSearchOptions.SearchTerm);


            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            Assert.IsNotNull(pluginServices);

            List<ISearchProvider> searchProviders = pluginServices.GetPluginClasses<ISearchProvider>();

            Assert.AreEqual(1, searchProviders.Count);
            IMemoryCache memoryCache = _testPlugin.GetRequiredService<IMemoryCache>();
            memoryCache.GetCache().Clear();
            Assert.IsNull(memoryCache.GetCache().Get(cacheName));

            List<SearchResponseItem> results = searchProviders[0].KeywordSearch(keywordSearchOptions);

            
            Assert.AreEqual(1, results.Count);

            Assert.IsNotNull(memoryCache.GetCache().Get(cacheName));
        }
    }
}
