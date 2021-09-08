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
 *  File: SitemapMenuTimingTests.cs
 *
 *  Purpose:  Tests for sitemap timing system admin menu
 *
 *  Date        Name                Reason
 *  31/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Blog.Plugin.Classes;

using Company.Plugin.Classes;

using HelpdeskPlugin.Classes;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

using Sitemap.Plugin;
using Sitemap.Plugin.Classes.SystemAdmin;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Tests.Plugins.SitemapTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MiddlewareTests
    {
        private const string TestCategoryName = "Sitemap Plugin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_INotificationService_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, new TestPluginClassesService(), new TestMemoryCache(), null, new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_IPluginClassesService_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, null, new TestMemoryCache(), new TestNotificationService(), new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_IMemoryCache_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, new TestPluginClassesService(), null, new TestNotificationService(), new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ILogger_Null_Throws_ArgumentNullException()
        {
            SitemapMiddleware sut = new SitemapMiddleware(null, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            SitemapMiddleware sut = new SitemapMiddleware(requestDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), new TestLogger());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Invoke_InvalidParam_Context_Null_Throws_ArgumentNullException()
        {
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            SitemapMiddleware sut = new SitemapMiddleware(requestDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), new TestLogger());

            await sut.Invoke(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public async Task Invoke_RouteIsNotASitemapRoute_NextDelegateCalled_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; ; await Task.Delay(0); };
            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/"
            };

            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            SitemapMiddleware sut = new SitemapMiddleware(nextDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), new TestLogger());

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public async Task Invoke_RouteIsSitemapRoute_SitemapNotFound_NextDelegateCalled_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; ; await Task.Delay(0); };
            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/sitemap998765.xml"
            };

            List<object> registeredServices = new List<object>();

            registeredServices.Add(new BlogSitemapProvider(new DemoWebsite.Classes.MockBlogProvider()));
            registeredServices.Add(new HelpdeskSitemapProvider(new DemoWebsite.Classes.MockHelpdeskProvider()));
            registeredServices.Add(new CompanySitemapProvider(new TestSettingsProvider("{}")));

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            SitemapMiddleware sut = new SitemapMiddleware(nextDelegate, testPluginClassesService, new TestMemoryCache(), new TestNotificationService(), new TestLogger());

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public async Task Invoke_RouteIsSitemapRoute_NoSitemapDataRegistered_Throws_NullReferenceException()
        {
            RequestDelegate nextDelegate = async (context) => { await Task.Delay(0); };
            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/SitemaP"
            };

            TestLogger testLogger = new TestLogger();
            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            SitemapMiddleware sut = new SitemapMiddleware(nextDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), testLogger);

            try
            {
                await sut.Invoke(httpContext);
            }
            catch (NullReferenceException)
            {

            }
            catch (Exception)
            {
                throw new InvalidOperationException("NullReferenceException should have been thrown!");
            }

            Assert.AreEqual(1, testLogger.Errors.Count);
            Assert.AreEqual("MoveNext", testLogger.Errors[0].Data);
            Assert.AreEqual("SitemapMiddleware", testLogger.Errors[0].Module);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public async Task Invoke_RouteIsSitemapRoute_NextDelegateNotCalled_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; ; await Task.Delay(0); };
            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/SiTEMap.XmL"
            };

            List<object> registeredServices = new List<object>();

            registeredServices.Add(new BlogSitemapProvider(new DemoWebsite.Classes.MockBlogProvider()));
            registeredServices.Add(new HelpdeskSitemapProvider(new DemoWebsite.Classes.MockHelpdeskProvider()));
            registeredServices.Add(new CompanySitemapProvider(new TestSettingsProvider("{}")));

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            SitemapMiddleware sut = new SitemapMiddleware(nextDelegate, testPluginClassesService, new TestMemoryCache(), new TestNotificationService(), new TestLogger());

            await sut.Invoke(httpContext);

            Assert.IsFalse(delegateCalled);
            Assert.AreEqual(200, httpResponse.StatusCode);

            byte[] responseBytes = new byte[httpResponse.Body.Length];
            httpResponse.Body.Position = 0;
            httpResponse.Body.Read(responseBytes, 0, (int)httpResponse.Body.Length);

            string content = Encoding.ASCII.GetString(responseBytes);
            Assert.IsTrue(content.StartsWith("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<sitemapindex xmlns=\"https://www.sitemaps.org/schemas/sitemap/0.9\">\r\n\t<sitemap>\r\n\t\t<loc>http://localhost/sitemap1.xml</loc>"));
            Assert.IsTrue(content.Contains("<loc>http://localhost/sitemap2.xml</loc>"));
            Assert.IsTrue(content.Contains("<loc>http://localhost/sitemap3.xml</loc>"));
            Assert.IsTrue(content.EndsWith("</sitemapindex>"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetEvents_ReturnsValidListOfEvents_Success()
        {
            List<string> events = new List<string>();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            SitemapMiddleware sut = new SitemapMiddleware(requestDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), new TestLogger());
            events = sut.GetEvents();

            Assert.AreEqual(1, events.Count);
            Assert.AreEqual("Sitemap Names", events[0]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_MethodNotImplemented_NoExceptionRaised_Success()
        {
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            SitemapMiddleware sut = new SitemapMiddleware(requestDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), new TestLogger());
            sut.EventRaised("event name", null, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameNotRecognised_InvalidParam1_Null_ReturnsFalse()
        {
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            SitemapMiddleware sut = new SitemapMiddleware(requestDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), new TestLogger());
            object refObject = new object();
            bool eventResponse = sut.EventRaised("Sitemap Names", null, null, ref refObject);

            Assert.IsFalse(eventResponse);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameNotRecognised_ReturnsFalse()
        {
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            SitemapMiddleware sut = new SitemapMiddleware(requestDelegate, new TestPluginClassesService(), new TestMemoryCache(), new TestNotificationService(), new TestLogger());
            object refObject = new object();
            bool eventResponse = sut.EventRaised("event name", null, null, ref refObject);

            Assert.IsFalse(eventResponse);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EventRaised_EventNameRecognised_ValidContext_ReturnsListOfSitemapKeys()
        {
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/SiTEMap.XmL"
            };

            List<object> registeredServices = new List<object>();

            registeredServices.Add(new TestSitemapProviderWithDomain());
            registeredServices.Add(new BlogSitemapProvider(new DemoWebsite.Classes.MockBlogProvider()));
            registeredServices.Add(new HelpdeskSitemapProvider(new DemoWebsite.Classes.MockHelpdeskProvider()));
            registeredServices.Add(new CompanySitemapProvider(new TestSettingsProvider("{}")));

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);

            SitemapMiddleware sut = new SitemapMiddleware(requestDelegate, testPluginClassesService, new TestMemoryCache(), new TestNotificationService(), new TestLogger());
            List<string> keys;
            object refObject = null;
            bool eventResponse = sut.EventRaised("Sitemap Names", httpContext, null, ref refObject);

            Assert.IsTrue(eventResponse);

            Assert.IsNotNull(refObject);

            keys = refObject as List<string>;

            Assert.IsNotNull(keys);
            Assert.AreEqual(6, keys.Count);
        }
    }
}
