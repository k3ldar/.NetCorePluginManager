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
 *  File: BadEggTests.cs
 *
 *  Purpose:  Test units for MVC BadEgg Middleware class
 *
 *  Date        Name                Reason
 *  23/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using AspNetCore.PluginManager.DemoWebsite.Classes;

using BadEgg.Plugin;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.MiddlewareTests
{
    [TestClass]
    public class BadEggTests : TestBasePlugin
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidRequestDelegate()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(null, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidRouteProvider()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, null,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidRouteDataServices()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                null, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidPluginHelperServices()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, null, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidPluginTypesService()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, null, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidIpValidation()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, null,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidSettingsProvider()
        {
            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                null, notificationService);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidNotificationService()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, null);
        }

        [TestMethod]
        public async Task BadEggValidationSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);

            await badEgg.Invoke(httpContext);

            Assert.IsTrue(nextDelegateCalled);
        }

        [TestMethod]
        public async Task BadEggValidationIpBlackListed()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();
            IIpManagement ipManagement = _testPlugin.GetRequiredService<IIpManagement>();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            ipManagement.AddBlackListedIp(httpContext.Connection.RemoteIpAddress.ToString());

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);

            await badEgg.Invoke(httpContext);

            ipManagement.ClearAllIpAddresses();

            Assert.IsFalse(nextDelegateCalled);
            Assert.AreEqual(400, httpContext.Response.StatusCode);
        }

        [TestMethod]
        public async Task BadEggValidationIpWhiteListed()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();
            IIpManagement ipManagement = _testPlugin.GetRequiredService<IIpManagement>();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            ipManagement.AddWhiteListedIp(httpContext.Connection.RemoteIpAddress.ToString());

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);

            await badEgg.Invoke(httpContext);

            ipManagement.ClearAllIpAddresses();

            Assert.IsTrue(nextDelegateCalled);
            Assert.AreEqual(200, httpContext.Response.StatusCode);
        }

        [TestMethod]
        public async Task BadEggValidationIgnoreValidation()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            BadEggSettings badEggSettings = settingsProvider.GetSettings<BadEggSettings>(SharedPluginFeatures.Constants.BadEggSettingsName);

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPlugin.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPlugin.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPlugin.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new TestIPValidation();
            IIpManagement ipManagement = _testPlugin.GetRequiredService<IIpManagement>();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            TestActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new TestActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();


            httpRequest.Headers.Add(SharedPluginFeatures.Constants.BadEggValidationIgnoreHeaderName,
                badEggSettings.IgnoreValidationHeaderCode);

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);

            await badEgg.Invoke(httpContext);

            Assert.IsTrue(httpContext.Response.Headers.ContainsKey(Constants.BadEggValidationIgnoreHeaderName));
            Assert.AreEqual(httpContext.Response.Headers[Constants.BadEggValidationIgnoreHeaderName], Boolean.TrueString);
            Assert.IsTrue(nextDelegateCalled);
            Assert.AreEqual(200, httpContext.Response.StatusCode);
        }
    }
}
