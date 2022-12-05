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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.MiddlewareTests;
using AspNetCore.PluginManager.Tests.Shared;

using BadEgg.Plugin;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.BadEggTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BadEggTests : BaseMiddlewareTests
    {
        [TestInitialize]
        public void InitialiseBadEggTests()
        {
			string appSettingsFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "appsettings.json");

			if (!File.Exists(appSettingsFile))
			{
				File.WriteAllText(appSettingsFile, Encoding.UTF8.GetString(Properties.Resources.appsettings));
			}
			
			base.InitializeBadEggPluginManager();
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidRequestDelegate()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(null, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidRouteProvider()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, null,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidRouteDataServices()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                null, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidPluginHelperServices()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

           // IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, null, pluginTypesService, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidPluginTypesService()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, null, iPValidation,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidIpValidation()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, null,
                settingsProvider, notificationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidSettingsProvider()
        {
            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                null, notificationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BadEggValidationInvalidNotificationService()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task BadEggValidationSuccess()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();

            BadEggMiddleware badEgg = new BadEggMiddleware(requestDelegate, testActionDescriptorCollectionProvider,
                routeDataServices, pluginHelperServices, pluginTypesService, iPValidation,
                settingsProvider, notificationService);

            await badEgg.Invoke(httpContext);

            Assert.IsTrue(nextDelegateCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task BadEggValidationIpBlackListed()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();
            IIpManagement ipManagement = _testPluginBadEgg.GetRequiredService<IIpManagement>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
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
        [TestCategory(TestCategoryMiddleware)]
        public async Task BadEggValidationIpWhiteListed()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();
            IIpManagement ipManagement = _testPluginBadEgg.GetRequiredService<IIpManagement>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
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
        [TestCategory(TestCategoryMiddleware)]
        public async Task BadEggValidationIgnoreValidation()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            BadEggSettings badEggSettings = settingsProvider.GetSettings<BadEggSettings>(BadEggSettingsName);

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginBadEgg as IPluginClassesService;
            IPluginHelperService pluginHelperServices = _testPluginBadEgg.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginBadEgg.GetRequiredService<IPluginTypesService>();
            INotificationService notificationService = _testPluginBadEgg.GetRequiredService<INotificationService>();
            IIpValidation iPValidation = new MockIpValidation();
            IIpManagement ipManagement = _testPluginBadEgg.GetRequiredService<IIpManagement>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            MockActionDescriptorCollectionProvider testActionDescriptorCollectionProvider = new MockActionDescriptorCollectionProvider(actionDescriptorCollection);
            RouteDataServices routeDataServices = new RouteDataServices();


            httpRequest.Headers.Add(BadEggValidationIgnoreHeaderName,
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
