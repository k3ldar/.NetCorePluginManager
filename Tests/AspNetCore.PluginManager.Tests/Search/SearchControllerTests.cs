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
using System.IO;

using AspNetCore.PluginManager.Tests.PluginFeatures;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Search;

using PluginManager.Abstractions;

using SearchPlugin.Controllers;
using SearchPlugin.Models;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Search
{
    [TestClass]
    public class SearchControllerTests : SearchTestBase
    {
        //[TestMethod]
        //public void CreateQuickSearchViewModelValidModelData()
        //{
        //    using (BaseControllerWrapper baseController = new BaseControllerWrapper())
        //    {
        //        BaseModelData modelData = new BaseModelData(null, baseController.TestGetShoppingCartSummary(),
        //            String.Empty, String.Empty, String.Empty, String.Empty, false);
        //        QuickSearchViewModel viewModel = new QuickSearchViewModel(modelData);

        //        Assert.IsNotNull(viewModel);
        //    }
        //}

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
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

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
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new TestHttpContext();

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
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

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
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new TestHttpContext();

                ViewResult contentResult = searchController.Index(new SearchViewModel()) as ViewResult;

                Assert.IsNotNull(contentResult);

                Assert.IsNull(contentResult.ViewName);

                Assert.IsInstanceOfType(contentResult.Model, typeof(SearchViewModel));
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostNullKeywords()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new TestHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch(null) as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostEmptyKeywords()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new TestHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch("") as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostWhiteSpaceKeywords()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new TestHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch(" \n \r \t  ") as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostKeywordsTooShortDefaultOfThree()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new TestHttpContext();

                StatusCodeResult statusResult = searchController.QuickKeywordSearch("as") as StatusCodeResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(400, statusResult.StatusCode);
            }
        }

        [TestMethod]
        public void RouteKeywordSearchPostValidKeywordPlugin()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;

            using (SearchController searchController = new SearchController(
                new pm.DefaultSettingProvider(Directory.GetCurrentDirectory()),
                new SearchPlugin.Classes.Search.DefaultSearchProvider(pluginServices)))
            {
                Assert.IsNotNull(searchController);

                searchController.ControllerContext.HttpContext = new TestHttpContext();

                JsonResult statusResult = searchController.QuickKeywordSearch("Plugin") as JsonResult;

                Assert.IsNotNull(statusResult);

                Assert.AreEqual(200, statusResult.StatusCode);

                Assert.AreEqual("application/json", statusResult.ContentType);

                List<SearchResponseItem> searchResults = (List<SearchResponseItem>)statusResult.Value;

                Assert.IsTrue(searchResults.Count > 10);
            }
        }
    }
}
