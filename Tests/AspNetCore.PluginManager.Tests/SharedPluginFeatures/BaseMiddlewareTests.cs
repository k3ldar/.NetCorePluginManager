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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: BaseMiddlewareTests.cs
 *
 *  Purpose:  Tests for BaseMiddleware class
 *
 *  Date        Name                Reason
 *  19/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class BaseMiddlewareTests
    {
        private const string TestCategory = "BaseMiddlewareTests";

        [TestMethod]
        [TestCategory(TestCategory)]
        public void CreateBaseMiddleware()
        {
            BaseMiddleware sut = new BaseMiddleware();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetCurrentUri_NullContext_Throws_ArgumentNullException()
        {
            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            sut.TestGetCurrentUri(null);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetCurrentUri_NullPort_ValidContext_ReturnsValidUri()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            Uri uri = sut.TestGetCurrentUri(testContext);

            Assert.IsNotNull(uri);
            Assert.AreEqual("http://localhost/", uri.AbsoluteUri);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetCurrentUri_ValidPort_ValidContext_ReturnsValidUri()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            testRequest.SetHost(new Microsoft.AspNetCore.Http.HostString("15.23.25.43", 800));
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            Uri uri = sut.TestGetCurrentUri(testContext);

            Assert.IsNotNull(uri);
            Assert.AreEqual("http://15.23.25.43:800/", uri.AbsoluteUri);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetHost_InvalidContext_Throws_ArgumentNullException()
        {
            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            sut.TestGetHost(null);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetHost_NullPort_ValidContext_ReturnsValidUri()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            string Result = sut.TestGetHost(testContext);

            Assert.IsNotNull(Result);
            Assert.AreEqual("http://localhost/", Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetHost_ValidPort_ValidContext_ReturnsValidUri()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            testRequest.SetHost(new Microsoft.AspNetCore.Http.HostString("15.23.25.43", 800));
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            string Result = sut.TestGetHost(testContext);

            Assert.IsNotNull(Result);
            Assert.AreEqual("http://15.23.25.43:800/", Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetTempData_NullContext_Throws_ArgumentNullException()
        {
            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            sut.TestGetTempData(null);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetTempData_ValidContext_ReturnsEmptyTempDataDictionary()
        {
            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            serviceCollection.AddSingleton<ITempDataDictionaryFactory>(new TempDataDictionaryFactory(tempDataProvider));
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse, serviceCollection.BuildServiceProvider());


            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            ITempDataDictionary Result = sut.TestGetTempData(testContext);

            Assert.IsNotNull(Result);
            Assert.AreEqual(0, Result.Count);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetTempData_ValidContext_ReturnsNonEmptyTempDataDictionary()
        {
            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            serviceCollection.AddSingleton<ITempDataDictionaryFactory>(new TempDataDictionaryFactory(tempDataProvider));
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse, serviceCollection.BuildServiceProvider());
            Dictionary<string, object> tempData = new Dictionary<string, object>();
            tempData.Add("test", new object());
            tempDataProvider.SaveTempData(testContext, tempData);


            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            ITempDataDictionary Result = sut.TestGetTempData(testContext);

            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Result.Count);
            Assert.IsTrue(Result.ContainsKey("test"));
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetUserSession_NullContext_Throws_ArgumentNullException()
        {
            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            sut.TestGetUserSession(null);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetUserSession_ValidContext_SessionNotFound_ReturnsNull()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);
            testContext.CreateSession = false;

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            UserSession Result = sut.TestGetUserSession(testContext);

            Assert.IsNull(Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetUserSession_ValidContext_SessionFound_ReturnsUserSession()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            UserSession Result = sut.TestGetUserSession(testContext);

            Assert.IsNotNull(Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void IsUserLoggedIn_NullContext_Throws_ArgumentNullException()
        {
            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            sut.TestIsUserLoggedIn(null);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void IsUserLoggedIn_ValidContext_SessionNotFound_ReturnsFalse()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);
            testContext.CreateSession = false;

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            bool Result = sut.TestIsUserLoggedIn(testContext);

            Assert.IsFalse(Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void IsUserLoggedIn_ValidContext_SessionFound_ReturnsUserSession()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);
            testContext.LogUserIn = true;

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            bool Result = sut.TestIsUserLoggedIn(testContext);

            Assert.IsTrue(Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Route_NullContext_Throws_ArgumentNullException()
        {
            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            sut.TestRoute(null);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void Route_ValidContext_ReturnsValidRoute()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            testRequest.Path = "/MyRoute/Test";

            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);
            testContext.LogUserIn = true;

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            Assert.IsFalse(testContext.Items.ContainsKey("Route"));
            string Result = sut.TestRoute(testContext);

            Assert.IsTrue(testContext.Items.ContainsKey("Route"));
            Assert.AreEqual("/MyRoute/Test", Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void Route_ValidContext_ReturnsValidRouteFromItems()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            testRequest.Path = "/MyRoute/Test";

            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);
            testContext.LogUserIn = true;

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            Assert.IsFalse(testContext.Items.ContainsKey("Route"));
            string Result = sut.TestRoute(testContext);

            Assert.IsTrue(testContext.Items.ContainsKey("Route"));
            Assert.AreEqual("/MyRoute/Test", Result);

            string ItemsResult = sut.TestRoute(testContext);

            Assert.AreEqual("/MyRoute/Test", ItemsResult);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RouteLowered_NullContext_Throws_ArgumentNullException()
        {
            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();
            sut.TestRouteLowered(null);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void RouteLowered_ValidContext_ReturnsValidRoute()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            testRequest.Path = "/MyRoute/Test";

            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);
            testContext.LogUserIn = true;

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            Assert.IsFalse(testContext.Items.ContainsKey("RouteLowered"));
            string Result = sut.TestRouteLowered(testContext);

            Assert.IsTrue(testContext.Items.ContainsKey("RouteLowered"));
            Assert.AreEqual("/myroute/test", Result);
        }

        [TestMethod]
        [TestCategory(TestCategory)]
        public void RouteLowered_ValidContext_ReturnsValidRouteFromItems()
        {
            MockHttpRequest testRequest = new MockHttpRequest();
            testRequest.Path = "/MyRoute/Test";

            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse);
            testContext.LogUserIn = true;

            BaseMiddlewareWrapper sut = new BaseMiddlewareWrapper();

            Assert.IsFalse(testContext.Items.ContainsKey("RouteLowered"));
            string Result = sut.TestRouteLowered(testContext);

            Assert.IsTrue(testContext.Items.ContainsKey("RouteLowered"));
            Assert.AreEqual("/myroute/test", Result);

            string ItemsResult = sut.TestRouteLowered(testContext);

            Assert.AreEqual("/myroute/test", ItemsResult);
        }
    }
}
