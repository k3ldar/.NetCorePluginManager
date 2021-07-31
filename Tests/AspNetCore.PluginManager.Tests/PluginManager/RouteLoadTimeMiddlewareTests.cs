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
 *  File: PageLoadSpeedMiddlewareTests.cs
 *
 *  Purpose:  Tests for page load speed middleware
 *
 *  Date        Name                Reason
 *  29/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Middleware;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.AspNetCore.PluginManager
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class RouteLoadTimeMiddlewareTests : GenericBaseClass
    {
        private const string TestCategoryName = "AspNetCore Plugin Manager Tests";
        private const string MiddlewareCategoryName = "Middleware";

        [TestInitialize]
        public void InitializeTests()
        {
            RouteLoadTimeMiddleware.ClearPageTimings();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(MiddlewareCategoryName)]
        public void Construct_NullContext_DoesNotThrowException_Success()
        {
            RouteLoadTimeMiddleware sut = new RouteLoadTimeMiddleware(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(MiddlewareCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Invoke_InvalidParam_Context_Null_Throws_ArgumentNullException()
        {
            RouteLoadTimeMiddleware sut = new RouteLoadTimeMiddleware(null);

            await sut.Invoke(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(MiddlewareCategoryName)]
        public async Task Invoke_CallIsPassedToNextDelegate_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
            RouteLoadTimeMiddleware sut = new RouteLoadTimeMiddleware(nextDelegate);

            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/products/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);

            Dictionary<string, Timings> pageTimings = RouteLoadTimeMiddleware.ClonePageTimings();

            Assert.AreEqual(1, pageTimings.Count);
            Assert.AreEqual(1u, pageTimings["/products/"].Requests);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(MiddlewareCategoryName)]
        public async Task Invoke_MultipleCallsToSameRouteAreRegistered_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
            RouteLoadTimeMiddleware sut = new RouteLoadTimeMiddleware(nextDelegate);

            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/products/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            for (int i = 0; i < 10; i++)
            {
                await sut.Invoke(httpContext);
            }

            Assert.IsTrue(delegateCalled);

            Dictionary<string, Timings> pageTimings = RouteLoadTimeMiddleware.ClonePageTimings();

            Assert.AreEqual(1, pageTimings.Count);
            Assert.AreEqual(10u, pageTimings["/products/"].Requests);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(MiddlewareCategoryName)]
        public void PageLoadSpeedMiddlewareExtender_UseSubdomainRouting_RegistersPageLoadSpeedMiddlewareExtender_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();

            RouteLoadTimeMiddlewareExtender.UseRouteLoadTimes(testApplicationBuilder);
            RouteLoadTimeMiddlewareExtender.UseRouteLoadTimes(testApplicationBuilder);
            RouteLoadTimeMiddlewareExtender.UseRouteLoadTimes(testApplicationBuilder);
            RouteLoadTimeMiddlewareExtender.UseRouteLoadTimes(testApplicationBuilder);
            RouteLoadTimeMiddlewareExtender.UseRouteLoadTimes(testApplicationBuilder);

            Assert.IsTrue(testApplicationBuilder.UseCalled);
            Assert.AreEqual(1, testApplicationBuilder.UseCalledCount);
        }
    }
}
