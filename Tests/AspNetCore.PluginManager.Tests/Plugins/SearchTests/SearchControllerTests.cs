/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: QuickSearchControllerTests.cs
 *
 *  Purpose:  Tests for quick searching via controller
 *
 *  Date        Name                Reason
 *  03/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Search;

using PluginManager.Abstractions;

using SearchPlugin.Controllers;
using SearchPlugin.Models;

using Shared.Classes;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.SearchTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class SearchControllerTests : MockBasePlugin
    {
        [TestInitialize]
        public void InitializeSearchTests()
        {
            ThreadManager.Initialise();
            InitializeDocumentationPluginManager();
        }

        [TestCleanup]
        public void FinaliseSearchTests()
        {
            ThreadManager.Finalise();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateSearchControllerInvalidConstructorArgs()
        {
            using (SearchController searchController = new SearchController(null, null))
            {

            }
        }

        [TestMethod]
        public void CreateSearchControllerValidConstructorArgs()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);
            }
        }

        [TestMethod]
        public void RouteIndexGetValidResult()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                ViewResult contentResult = searchController.Index() as ViewResult;

                Assert.IsNotNull(contentResult);

                Assert.IsNull(contentResult.ViewName);

                Assert.IsInstanceOfType(contentResult.Model, typeof(SearchViewModel));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RouteIndexPostInvalidArgs()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.Index(null);
            }
        }

        [TestMethod]
        public void RouteIndexPostValidResults()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                ViewResult contentResult = searchController.Index(new SearchViewModel()) as ViewResult;

                Assert.IsNotNull(contentResult);

                Assert.IsNull(contentResult.ViewName);

                Assert.IsInstanceOfType(contentResult.Model, typeof(SearchViewModel));

                Assert.IsFalse(searchController.ModelState.IsValid);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostNullKeywords()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch(null) as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostEmptyKeywords()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch(new QuickSearchModel() { keywords = "" }) as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostWhiteSpaceKeywords()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch(new QuickSearchModel() { keywords = " \n \r \t  " }) as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostKeywordsTooShortDefaultOfThree()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch(new QuickSearchModel() { keywords = "as" }) as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostValidKeywordPlugin()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                JsonResult statusResult = searchController.QuickKeywordSearch(new QuickSearchModel() { keywords = "Plugin" }) as JsonResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(200, statusResult.StatusCode);

                Assert.AreEqual("application/json", statusResult.ContentType);

                string json = JsonSerializer.Serialize(statusResult.Value);
                List<SearchResponseItem> searchResults = JsonSerializer.Deserialize<List<SearchResponseItem>>(json);

                Assert.AreEqual(5, searchResults.Count);

                Assert.IsFalse(String.IsNullOrEmpty(searchResults[0].DisplayName));
                Assert.IsFalse(String.IsNullOrEmpty(searchResults[0].ResponseType));
                Assert.IsNotNull(searchResults[0].Properties);
                Assert.IsTrue((searchResults[0].Properties["ShortDescription"]).ToString().Length > 10);
            }
        }

        [TestMethod]
        public void RouteIndexGetValidResultSearchPlugin()
        {
            IPluginClassesService pluginServices = _testPluginDocs as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new MockHttpContext();

                ViewResult contentResult = searchController.Index(new SearchViewModel() { SearchText = "Plugin" }) as ViewResult;
                Assert.IsTrue(searchController.ModelState.IsValid);
                Assert.IsNotNull(contentResult);

                Assert.IsNull(contentResult.ViewName);

                Assert.IsInstanceOfType(contentResult.Model, typeof(SearchViewModel));

                SearchViewModel model = (SearchViewModel)contentResult.Model;

                Assert.IsNotNull(model.SearchResults);
                Assert.AreEqual("Plugin", model.SearchText);
                Assert.IsTrue(model.SearchResults.Count > 0);
            }
        }
    }
}
