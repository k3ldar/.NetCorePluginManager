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
 *  File: SubdomainMiddlewareTests.cs
 *
 *  Purpose:  Test units for MVC Subdomain Middleware class
 *
 *  Date        Name                Reason
 *  13/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.MiddlewareTests;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

using Subdomain.Plugin;
using Subdomain.Plugin.Classes.SystemAdmin;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.SubdomainTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SubdomainMiddlewareTests : BaseMiddlewareTests
    {
        #region Constants

        private const string SubDomainDisbledJson = "{\"SubdomainSettings\":{\"Enabled\":false,\"DomainName\":\"pluginManager.website\"}}";
        private const string BlogDisbledSubDomainJson = "{\"SubdomainSettings\":{\"Enabled\":true,\"DomainName\":\"pluginManager.website\",\"Subdomains\":{\"" +
            "Blog\":{\"Disabled\":true,\"RedirectedRoute\":\"blog\",\"PermanentRedirect\":true}}}}";
        private const string SubdomainAllEnabled = "{\"SubdomainSettings\":{\"Enabled\":true,\"DomainName\":\"pluginManager.website\",\"Subdomains\":{\"Blog\":" +
            "{\"Disabled\":false,\"RedirectedRoute\":\"blog\",\"PermanentRedirect\":true},\"Helpdesk\":{\"Disabled\":false,\"RedirectedRoute\":\"blog\"," +
            "\"PermanentRedirect\":true},\"Account\":{\"Disabled\":false,\"RedirectedRoute\":\"account\",\"PermanentRedirect\":true}}}}";
        private const string SubdomainAllEnabledPreventStaticFiles = "{\"SubdomainSettings\":{\"Enabled\":true,\"StaticFileExtensions\":\".js;.css;\"," +
            "\"DomainName\":\"pluginManager.website\",\"Subdomains\":{\"Blog\":" +
            "{\"Disabled\":false,\"RedirectedRoute\":\"blog\",\"PermanentRedirect\":true},\"Helpdesk\":{\"Disabled\":false,\"RedirectedRoute\":\"blog\"," +
            "\"PermanentRedirect\":true},\"Account\":{\"Disabled\":false,\"RedirectedRoute\":\"account\",\"PermanentRedirect\":true}}}}";
        private const string SubdomainAllEnabledWwwDisabled = "{\"SubdomainSettings\":{\"Enabled\":true, \"DisableRedirectWww\":true,\"DomainName\":\"pluginManager.website\",\"Subdomains\":{\"Blog\":" +
            "{\"Disabled\":false,\"RedirectedRoute\":\"blog\",\"PermanentRedirect\":true},\"Helpdesk\":{\"Disabled\":false,\"RedirectedRoute\":\"blog\"," +
            "\"PermanentRedirect\":true},\"Account\":{\"Disabled\":false,\"RedirectedRoute\":\"account\",\"PermanentRedirect\":true}}}}";
        private const string InvalidControllerClass = "Warning Subdomain can only be used on classes descending from Controller, class AspNetCore.PluginManager." +
            "Tests.MVCMiddlewareTests.Mocks.MockClassWithAttributeNotOnControllerDescendent is invalid";

        #endregion Constants

        [TestInitialize]
        public void InitializeTest()
        {
            InitializeSubdomainManager();
        }

        #region Subdomain Middleware

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_MiddlewareResponseStarted_LogsWarningAndDoesNotRedirect()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabledWwwDisabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/Login/"
            };

            httpRequest.SetHost(new HostString("blog.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse()
            {
                TestHasStarted = true
            };
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(testLogger.ContainsMessage("Warning Subdomain Middleware is unable to redirect the request because the response has started"));
            Assert.AreEqual(0, httpResponse.RedirectCount);
            Assert.IsNull(httpResponse.RedirectPermanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_BlogSubdomainAttemptLogin_RedirectsToHostOnly_Returns_302()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabledWwwDisabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/Login/"
            };

            httpRequest.SetHost(new HostString("blog.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.AreEqual(1, httpResponse.RedirectCount);
            Assert.IsNotNull(httpResponse.RedirectPermanent);
            Assert.AreEqual(true, httpResponse.RedirectPermanent.Value);
            Assert.AreEqual("http://pluginmanager.website/Login/", httpResponse.RedirectLocation);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_BlogSubdomainAttemptLogin_RedirectsToWwwSubdomain_Returns_302()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/Login/"
            };

            httpRequest.SetHost(new HostString("blog.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.AreEqual(1, httpResponse.RedirectCount);
            Assert.IsNotNull(httpResponse.RedirectPermanent);
            Assert.AreEqual(true, httpResponse.RedirectPermanent.Value);
            Assert.AreEqual("http://www.pluginmanager.website/Login/", httpResponse.RedirectLocation);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_BlogSubdomainAttemptLoginWithPort_RedirectsToWwwSubdomain_Returns_302()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/Login/"
            };

            httpRequest.SetHost(new HostString("blog.pluginmanager.website", 1928));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.AreEqual(1, httpResponse.RedirectCount);
            Assert.IsNotNull(httpResponse.RedirectPermanent);
            Assert.AreEqual(true, httpResponse.RedirectPermanent.Value);
            Assert.AreEqual("http://www.pluginmanager.website:1928/Login/", httpResponse.RedirectLocation);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_BlogRoute_ResetsContextRequestPath()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; ; await Task.Delay(0); };
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true, nextDelegate);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/"
            };

            httpRequest.SetHost(new HostString("blog.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
            Assert.IsNull(httpResponse.RedirectPermanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_BlogRoute_RedirectsToBlogSubdomain_Returns_302()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/blog/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.AreEqual(1, httpResponse.RedirectCount);
            Assert.IsNotNull(httpResponse.RedirectPermanent);
            Assert.AreEqual(true, httpResponse.RedirectPermanent.Value);
            Assert.AreEqual("http://blog.pluginmanager.website/", httpResponse.RedirectLocation);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_EmptyHost_CallsNextDelegate_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; ; await Task.Delay(0); };
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true, nextDelegate);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/blog/"
            };

            httpRequest.SetHost(new HostString());
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
            Assert.IsNull(httpResponse.RedirectPermanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_BlogRouteWithPort_RedirectsToBlogSubdomain_Returns_302()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/blog/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website", 6500));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.AreEqual(1, httpResponse.RedirectCount);
            Assert.IsNotNull(httpResponse.RedirectPermanent);
            Assert.AreEqual(true, httpResponse.RedirectPermanent.Value);
            Assert.AreEqual("http://blog.pluginmanager.website:6500/", httpResponse.RedirectLocation);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainMiddleware_InitiallyLoadsValidSubdomains_Success()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            Assert.AreEqual(1, sut.RoutesWithoutSubdomain.Count);
            Assert.AreEqual(2, sut.RoutesWithSubdomain.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainMiddleware_AttributeAppliedToNonControllerClass_AddsWarningToLogger()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider);

            Assert.IsTrue(testLogger.ContainsMessage(InvalidControllerClass));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_StaticFile_CallsNextDelegate()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabledPreventStaticFiles);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true, nextDelegate);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/myfile.css"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_ValidContext_DisabledMiddleware_CallsNextDelegate()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubDomainDisbledJson);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true, nextDelegate);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/blog/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_ValidContext_NoMappingsFound_NoDescriptorsCreated_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(BlogDisbledSubDomainJson);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, false, nextDelegate);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/products/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task SubdomainMiddleware_Invoke_ValidContext_NoMappingsFound_WithDescriptorsCreated_Success()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true, nextDelegate);

            MockHttpRequest httpRequest = new MockHttpRequest()
            {
                Path = "/products/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            MockHttpResponse httpResponse = new MockHttpResponse();
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task SubdomainMiddleware_Invoke_NullContext_Throws_ArgumentNullException()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider, null, true);

            await sut.Invoke(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainMiddleware_BlogRouteFoundAndDisabled_AddsWarningToLogger()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(BlogDisbledSubDomainJson);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider);

            Assert.IsTrue(testLogger.ContainsMessage("Warning Configuration for subdomain Blog is disabled"));
            Assert.IsFalse(testLogger.ContainsMessage("Warning Configuration for subdomain Blog is missing"));
            Assert.IsTrue(testLogger.ContainsMessage("Warning Configuration for subdomain Helpdesk is missing"));
            Assert.IsTrue(testLogger.ContainsMessage("Warning Configuration for subdomain Account is missing"));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainMiddleware_DisabledViaConfig_AddsWarningToLogger()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubDomainDisbledJson);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider);

            Assert.IsTrue(testLogger.ContainsMessage("Information Subdomain Middleware is disabled"));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainMiddleware_BlogRouteDisabled_AddsWarningToLogger()
        {
            MockLogger testLogger = new MockLogger();
            ISettingsProvider settingsProvider = new MockSettingsProvider(BlogDisbledSubDomainJson);
            SubdomainMiddleware sut = CreateSubdomainMiddlewareInstance(testLogger, settingsProvider);

            Assert.IsTrue(testLogger.ContainsMessage("Warning Configuration for subdomain Blog is disabled"));
            Assert.IsTrue(testLogger.ContainsMessage("Warning Configuration for subdomain Helpdesk is missing"));
            Assert.IsTrue(testLogger.ContainsMessage("Warning Configuration for subdomain Account is missing"));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainMiddleware_Construct_InvalidRequestDelegate_DoesNotThrow_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            MockLogger testLogger = new MockLogger();

            SubdomainMiddleware sut = new SubdomainMiddleware(null, actionDescriptorCollectionProvider, new RouteDataServices(),
                pluginHelperService, pluginClassesService, pluginTypesServices, settingsProvider, testLogger);
			Assert.IsNotNull(sut);
		}

		[TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainMiddleware_Construct_InvalidRouteProvider_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ISettingsProvider settingsProvider = new MockSettingsProvider(SubdomainAllEnabled);
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            MockLogger testLogger = new MockLogger();

            new SubdomainMiddleware(requestDelegate, null, new RouteDataServices(),
                pluginHelperService, pluginClassesService, pluginTypesServices, settingsProvider, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainMiddleware_Construct_InvalidRouteDataServices_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            MockLogger testLogger = new MockLogger();

            new SubdomainMiddleware(requestDelegate, actionDescriptorCollectionProvider, null,
                pluginHelperService, pluginClassesService, pluginTypesServices, settingsProvider, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainMiddleware_Construct_InvalidPluginHelperServices_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            MockLogger testLogger = new MockLogger();

            new SubdomainMiddleware(requestDelegate, actionDescriptorCollectionProvider, new RouteDataServices(),
                null, pluginClassesService, pluginTypesServices, settingsProvider, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainMiddleware_Construct_InvalidPluginTypesService_Throws_ArgumentNullException()
        {
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            MockLogger testLogger = new MockLogger();

            new SubdomainMiddleware(requestDelegate, actionDescriptorCollectionProvider, new RouteDataServices(),
                pluginHelperService, pluginClassesService, null, settingsProvider, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainMiddleware_Construct_InvalidSettingsProvider_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            MockLogger testLogger = new MockLogger();

            new SubdomainMiddleware(requestDelegate, actionDescriptorCollectionProvider, new RouteDataServices(),
                pluginHelperService, pluginClassesService, pluginTypesServices, null, testLogger);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainMiddleware_Construct_InvalidLogger_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            MockLogger testLogger = new MockLogger();

            new SubdomainMiddleware(requestDelegate, actionDescriptorCollectionProvider, new RouteDataServices(),
                pluginHelperService, pluginClassesService, pluginTypesServices, settingsProvider, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainMiddleware_Construct_InvalidIPluginClassesService_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            MockLogger testLogger = new MockLogger();

            new SubdomainMiddleware(requestDelegate, actionDescriptorCollectionProvider, new RouteDataServices(),
                pluginHelperService, null, pluginTypesServices, settingsProvider, testLogger);
        }

        #endregion Subdomain Middleware

        #region Subdomain Attribute

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainAttribute_DescendsFromAttribute_Success()
        {
            SubdomainAttribute sut = new SubdomainAttribute("test");
            Assert.IsInstanceOfType(sut, typeof(Attribute));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainAttribute_HasAttributeUsageClass_Success()
        {
            SubdomainAttribute sut = new SubdomainAttribute("test");
            Assert.IsTrue(ClassHasAttributeUsageFlag(sut.GetType(), AttributeTargets.Class));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainAttribute_AttributeUsageAllowMultipleFalse_Success()
        {
            SubdomainAttribute sut = new SubdomainAttribute("test");
            bool allowMultiple = ClassAttributeUsageAllowsMultiple(sut.GetType());

            Assert.IsFalse(allowMultiple);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainAttribute_Construct_InvalidConfigurationName_Null_Throws_ArgumentNullException()
        {
            SubdomainAttribute sut = new SubdomainAttribute(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubdomainAttribute_Construct_InvalidConfigurationName_EmptyString_Throws_ArgumentNullException()
        {
            SubdomainAttribute sut = new SubdomainAttribute("");
        }

        #endregion Subdomain Attribute

        #region Subdomain Setting

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainSetting_Construct_DefaultDisabledFalse_Success()
        {
            SubdomainSetting sut = new SubdomainSetting();
            Assert.IsFalse(sut.Disabled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainSetting_Construct_DefaultPermanentRedirectIsFalse_Success()
        {
            SubdomainSetting sut = new SubdomainSetting();
            Assert.IsFalse(sut.PermanentRedirect);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainSetting_Construct_DefaultRedirectedRouteIsNull_Success()
        {
            SubdomainSetting sut = new SubdomainSetting();
            Assert.IsNull(sut.RedirectedRoute);
        }

        #endregion Subdomain Setting

        #region Subdomain Settings

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainSettings_Construct_DefaultDisabledFalse_Success()
        {
            SubdomainSettings sut = new SubdomainSettings();
            Assert.IsFalse(sut.Enabled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainSettings_Construct_DefaultSubdomainsNotNull_Success()
        {
            SubdomainSettings sut = new SubdomainSettings();
            Assert.IsNotNull(sut.Subdomains);
        }

        #endregion Subdomain Settings

        #region PluginInitialisation

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void PluginInitialisation_ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void PluginInitialisation_AfterConfigure_DoesNotConfigurePipeline_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void PluginInitialisation_Configure_DoesNotConfigurePipeline_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void PluginInitialisation_BeforeConfigure_RegistersSubdomainRouting()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsTrue(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void PluginInitialisation_Finalise_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
			Assert.IsNotNull(sut);

			sut.Finalise();
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void PluginInitialisation_ConfigureServices_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
			Assert.IsNotNull(sut);

			sut.ConfigureServices(new MockServiceCollection());
        }

        #endregion PluginInitialisation

        #region Subdomain Middleware Extender

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainMiddlewareExtender_UseSubdomainRouting_RegistersSubdomainMiddleware_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();

            SubdomainMiddlewareExtender.UseSubdomainRouting(testApplicationBuilder);

            Assert.IsTrue(testApplicationBuilder.UseCalled);
        }

        #endregion Subdomain Middleware Extender

        #region Subdomain Timings

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_Construct_DefaultInstance_Success()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_ValidateInstanceOfSystemAdminSubMenu()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_SortOrder_ReturnsZero()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual(0, sut.SortOrder());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_Action_ReturnsEmptyString()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual(String.Empty, sut.Action());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_Area_ReturnsEmptyString()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual(String.Empty, sut.Area());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_Controller_ReturnsEmptyString()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual(String.Empty, sut.Controller());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_Image_ReturnsStopwatch()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual("stopwatch", sut.Image());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_MenuType_ReturnsGrid()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_Name_ReturnsSubdomain()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual("Subdomain", sut.Name());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_ParentMenuName_ReturnsTimings()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            Assert.AreEqual("Timings", sut.ParentMenuName());
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void SubdomainTimingsSubMenu_Data_ReturnsSettingsValue()
        {
            SubdomainTimingsSubMenu sut = new SubdomainTimingsSubMenu();
            string data = sut.Data();

            string[] rows = data.Split("\r");
            Assert.AreEqual("Setting|Value", rows[0]);
            Assert.IsTrue(rows[1].StartsWith("Total Requests|"));
            Assert.IsTrue(rows[2].StartsWith("Fastest ms|"));
            Assert.IsTrue(rows[3].StartsWith("Slowest ms|"));
            Assert.IsTrue(rows[4].StartsWith("Average ms|"));
            Assert.IsTrue(rows[5].StartsWith("Trimmed Avg ms|"));
            Assert.IsTrue(rows[6].StartsWith("Total ms|"));
        }

        #endregion Subdomain Timings

        #region Private Methods

        private SubdomainMiddleware CreateSubdomainMiddlewareInstance(MockLogger testLogger,
            ISettingsProvider settingsProvider,
            MockNotificationService customNotification = null,
            bool createDescriptors = false,
            RequestDelegate nextDelegate = null)
        {
            IPluginTypesService pluginTypesServices = _testPluginSubdomain as IPluginTypesService;

            ActionDescriptorCollection actionDescriptorCollection = null;

            if (createDescriptors)
            {
                var descriptors = new List<ActionDescriptor>()
                {
                    new ControllerActionDescriptor()
                    {
                        DisplayName = "Blog.Plugin.Controllers.BlogController",
                        ControllerName = "Blog"/*,
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Blog",
                            Name = "Index"
                        }*/
                    },
                    new ControllerActionDescriptor()
                    {
                        DisplayName = "HelpdeskPlugin.Controllers.HelpdeskController",
                        ControllerName = "Helpdesk"/*,
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Helpdesk",
                            Name = "Index"
                        }*/
                    },
                    new ControllerActionDescriptor()
                    {
                        DisplayName = "UserAccount.Plugin.Controllers.AccountController",
                        ControllerName = ""/*,
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Account",
                            Name = "Index"
                        }*/
                    },
                    new ControllerActionDescriptor()
                    {
                        DisplayName = "AspNetCore.PluginManager.DemoWebsite.Controllers.HomeController",
                        ControllerName = "/"
                    },
                    new ControllerActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController",
                        ControllerName = "Login"
                    }
                };
                actionDescriptorCollection = new ActionDescriptorCollection(descriptors, 1);
            }
            else
            {
                actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            }

            if (nextDelegate == null)
                nextDelegate = async (context) => { await Task.Delay(0); };

            if (settingsProvider == null)
                settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            IPluginHelperService pluginHelperService = _testPluginSubdomain as IPluginHelperService;
            IPluginClassesService pluginClassesService = _testPluginSubdomain as IPluginClassesService;
            MockActionDescriptorCollectionProvider actionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);

            return new SubdomainMiddleware(nextDelegate, actionDescriptorCollectionProvider, new RouteDataServices(),
                pluginHelperService, pluginClassesService, pluginTypesServices, settingsProvider, testLogger);
        }

        #endregion Private Methods
    }
}
