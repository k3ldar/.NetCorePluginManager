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
 *  File: SpiderMiddlewareTests.cs
 *
 *  Purpose:  Test units for MVC Spider Middleware class
 *
 *  Date        Name                Reason
 *  12/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using Spider.Plugin;
using Spider.Plugin.Classes;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.MiddlewareTests
{
    [TestClass]
    public class SpiderMiddlewareTests : TestBasePlugin
    {
        [TestInitialize]
        public void InitializeTest()
        {
            InitializeSpiderPluginManager();
        }

        [TestMethod]
        public void SpiderMiddleware_Construct_Success()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Invalid_RouteDataServices_ThrowsArgumentNullException()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginServices = new pm.PluginServices(_testPluginSpider) as IPluginHelperService;
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            new SpiderMiddleware(requestDelegate,
                null,
                settingsProvider,
                new TestLogger(),
                new TestNotificationService(),
                new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection),
                    new RouteDataServices(), pluginTypesServices, new MockLoadData()));

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Invalid_IRobots_ThrowsArgumentNullException()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginServices = new pm.PluginServices(_testPluginSpider) as IPluginHelperService;
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            new SpiderMiddleware(requestDelegate,
                pluginServices,
                settingsProvider,
                new TestLogger(),
                new TestNotificationService(),
                null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Invalid_ISettingsProvider_ThrowsArgumentNullException()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;
            IPluginHelperService pluginServices = new pm.PluginServices(_testPluginSpider) as IPluginHelperService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            new SpiderMiddleware(requestDelegate,
                pluginServices,
                null,
                new TestLogger(),
                new TestNotificationService(),
                new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), new RouteDataServices(), pluginTypesServices, new MockLoadData()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Invalid_ILogger_ThrowsArgumentNullException()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginServices = new pm.PluginServices(_testPluginSpider) as IPluginHelperService;
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            new SpiderMiddleware(requestDelegate,
                pluginServices,
                settingsProvider,
                null,
                new TestNotificationService(),
                new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), new RouteDataServices(), pluginTypesServices, new MockLoadData()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Invalid_INotificationService_ThrowsArgumentNullException()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginServices = new pm.PluginServices(_testPluginSpider) as IPluginHelperService;
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };
            new SpiderMiddleware(requestDelegate,
                pluginServices,
                settingsProvider,
                new TestLogger(),
                null,
                new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), new RouteDataServices(), pluginTypesServices, new MockLoadData()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task Call_Invoke_InvalidContext_Success()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance();
            Assert.IsNotNull(sut);

            await sut.Invoke(null);
        }

        [TestMethod]
        public async Task Call_Invoke_ValidContext_Success()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance();
            Assert.IsNotNull(sut);

            TestRequestCookieCollection cookies = new TestRequestCookieCollection();
            TestHttpRequest httpRequest = new TestHttpRequest(cookies);
            TestHttpResponse httpResponse = new TestHttpResponse();

            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);

            await sut.Invoke(httpContext);
        }

        [TestMethod]
        public async Task Call_Invoke_ValidContext_RobotsRoute_Success()
        {
            TestNotificationService testNotificationService = new TestNotificationService(new List<object>() { "/sitemap.xml" });
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(testNotificationService);

            TestHttpContext httpContext = CreateContext("/robots.txt", false,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            Assert.AreEqual(200, httpResponse.StatusCode);
            Assert.IsTrue(httpResponse.ContentLength > 0);
            Assert.IsTrue(testNotificationService.NotificationRaised(Constants.NotificationSitemapNames) > 0);

            httpResponse.Body.Position = 0;
            byte[] responseDataBytes = new byte[httpResponse.Body.Length];
            await httpResponse.Body.ReadAsync(responseDataBytes, 0, responseDataBytes.Length);
            string result = Encoding.UTF8.GetString(responseDataBytes);
            Assert.IsTrue(result.IndexOf("\r\nSitemap: http://localhost/sitemap.xml\n") > -1);
        }

        [TestMethod]
        public async Task Call_Invoke_ValidContext_RobotsRoute_NoSitemap_Success()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(null, true);

            TestHttpContext httpContext = CreateContext("/robots.txt", false,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            Assert.AreEqual(200, httpResponse.StatusCode);
            Assert.IsTrue(httpResponse.ContentLength > 0);
            httpResponse.Body.Position = 0;
            byte[] responseDataBytes = new byte[httpResponse.Body.Length];
            await httpResponse.Body.ReadAsync(responseDataBytes, 0, responseDataBytes.Length);
            string result = Encoding.UTF8.GetString(responseDataBytes);
            Assert.IsTrue(result.Length > 374);
        }

        [TestMethod]
        public async Task Call_Invoke_ValidContext_DenyLogin_Success()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(null, true);

            TestHttpContext httpContext = CreateContext("/robots.txt", false,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            Assert.AreEqual(200, httpResponse.StatusCode);
            Assert.IsTrue(httpResponse.ContentLength > 0);

            httpResponse.Body.Position = 0;
            byte[] responseDataBytes = new byte[httpResponse.Body.Length];
            await httpResponse.Body.ReadAsync(responseDataBytes, 0, responseDataBytes.Length);
            string result = Encoding.UTF8.GetString(responseDataBytes);
            Assert.AreEqual(-1, result.IndexOf("\r\nSitemap: http://localhost/sitemap.xml\n"));
            Assert.IsTrue(result.IndexOf("Disallow: /Test/Login/") > 5);
        }

        [TestMethod]
        public async Task BotTrap_AddToAllAgents_Success()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(null, true);

            TestHttpContext httpContext = CreateContext("/robots.txt", false,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            Assert.AreEqual(200, httpResponse.StatusCode);
            Assert.IsTrue(httpResponse.ContentLength > 0);

            httpResponse.Body.Position = 0;
            byte[] responseDataBytes = new byte[httpResponse.Body.Length];
            await httpResponse.Body.ReadAsync(responseDataBytes, 0, responseDataBytes.Length);
            string result = Encoding.UTF8.GetString(responseDataBytes);
            Assert.IsTrue(result.IndexOf("Disallow: /identities/reveal") > 5);
        }

        [TestMethod]
        public async Task BotTrap_CallRoute_IBotTrap_NotRegistered_Returns_403()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(null, true);

            TestHttpContext httpContext = CreateContext("/identities/reveal", true,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            Assert.AreEqual(403, httpResponse.StatusCode);
        }

        [TestMethod]
        public async Task BotTrap_CallRoute_IBotTrap_Registered_Returns405()
        {
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(null, true);
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            string ipAddress = "10.10.10.111";
            string userAgent = "TestBotTrap/v1.0";

            TestBotTrap testBotTrap = new TestBotTrap();
            serviceCollection.AddSingleton<IBotTrap>(testBotTrap);

            TestHttpContext httpContext = CreateContext("/identities/reveal", true, serviceCollection,
                out TestHttpResponse httpResponse, out TestHttpRequest testHttpRequest);

            testHttpRequest.IpAddress = ipAddress;
            testHttpRequest.UserAgent = userAgent;
            await sut.Invoke(httpContext);

            Assert.AreEqual(405, httpResponse.StatusCode);
            Assert.IsTrue(testBotTrap.IpAddressTrapped(ipAddress));
            Assert.IsTrue(testBotTrap.UserAgentTrapped(userAgent));
        }

        [TestMethod]
        public async Task BotTrap_CallRoute_IBotTrap_Registered_ThrowsException_Returns405()
        {
            TestLogger testLogger = new TestLogger();
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(testLogger, null, true);
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;

            TestBotTrap testBotTrap = new TestBotTrap(true);
            serviceCollection.AddSingleton<IBotTrap>(testBotTrap);

            TestHttpContext httpContext = CreateContext("/identities/reveal", true, serviceCollection,
                out TestHttpResponse httpResponse, out TestHttpRequest testHttpRequest);

            await sut.Invoke(httpContext);

            Assert.AreEqual(405, httpResponse.StatusCode);
            Assert.IsTrue(testLogger.ExceptionLogged(typeof(IOException)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_Robot_InvalidActionDescriptor_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;

            new Robots(null, new RouteDataServices(), pluginTypesServices, new MockLoadData());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_Robot_InvalidRouteDataServices_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;

            var descriptors = new List<ActionDescriptor>()
                {
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Test",
                            Name = "Login"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "AspNetCore.PluginManager.DemoWebsite.Controllers.HomeController.Error",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Home",
                            Name = "Error"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController.GetCaptchaImage",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Login",
                            Name = "Captcha"
                        }
                    }
                };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(descriptors, 1);

            new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), null, pluginTypesServices, new MockLoadData());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_Robot_InvalidSaveData_Throws_ArgumentNullException()
        {
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;

            var descriptors = new List<ActionDescriptor>()
                {
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Test",
                            Name = "Login"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "AspNetCore.PluginManager.DemoWebsite.Controllers.HomeController.Error",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Home",
                            Name = "Error"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController.GetCaptchaImage",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Login",
                            Name = "Captcha"
                        }
                    }
                };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(descriptors, 1);

            new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), new RouteDataServices(), pluginTypesServices, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_Robot_InvalidPluginTypesService_Throws_ArgumentNullException()
        {
            var descriptors = new List<ActionDescriptor>()
                {
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Test",
                            Name = "Login"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "AspNetCore.PluginManager.DemoWebsite.Controllers.HomeController.Error",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Home",
                            Name = "Error"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController.GetCaptchaImage",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Login",
                            Name = "Captcha"
                        }
                    }
                };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(descriptors, 1);

            new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), new RouteDataServices(), null, new MockLoadData());
        }

        [TestMethod]
        public void Construct_Robot_Success()
        {
            var sut = CreateRobotsInstance();
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void Robots_Retrieve_Agents()
        {
            var sut = CreateRobotsInstance();

            List<string> agents = new List<string>();
            foreach (string item in sut.Agents)
            {
                agents.Add(item);
            }

            Assert.AreEqual(5, agents.Count);
            Assert.IsTrue(agents.Contains("*"));
        }

        [TestMethod]
        public void Robots_Retrieve_GetRoutes_AllAgents()
        {
            var sut = CreateRobotsInstance();
            List<string> items = sut.GetRoutes("*");

            Assert.AreEqual(2, items.Count);
            Assert.IsTrue(items.Contains("Disallow: /Test/Login/"));
        }

        [TestMethod]
        public void Robots_Retrieve_DeniedRoutes()
        {
            var sut = CreateRobotsInstance();
            var items = sut.DeniedRoutes;

            Assert.AreEqual(8, sut.DeniedRoutes.Count);
            Assert.AreEqual("/home/error/", items[0].Route);
            Assert.AreEqual("*", items[0].UserAgent);
        }

        [TestMethod]
        public void Robots_EnsureRouteStartsWithSingleSlash_Success()
        {
            var sut = CreateRobotsInstance();
            var items = sut.DeniedRoutes;

            foreach (var item in items)
            {
                Assert.IsFalse(item.Route.StartsWith("//"));
            }
        }

        [TestMethod]
        public void Robots_AddRemoveCustomAgent_Success()
        {
            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            bool agentRemoved = sut.AgentRemove("testAgent");
            Assert.IsTrue(agentRemoved);
            Assert.AreEqual(currentAgentCount, sut.Agents.Count);
        }

        [TestMethod]
        public void Robots_SaveAndReloadCustomData()
        {
            string rootPath = Path.GetTempPath();
            ISaveData saveData = new FileStorageSaveData(new TestLogger(), rootPath);
            ILoadData loadData = new FileStorageLoadData(new TestLogger(), rootPath);

            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            Assert.IsFalse(sut.Agents.Contains("testAgent"));

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            sut.AddAllowedRoute("testAgent", "route/");

            bool saved = sut.SaveData(saveData);
            Assert.IsTrue(saved);

            Assert.IsTrue(File.Exists(Path.Combine(rootPath, "Settings", "CustomRoutes", "Routes.dat")));

            sut = CreateRobotsInstance(loadData);

            Directory.Delete(Path.Combine(rootPath, "Settings"), true);

            Assert.IsTrue(sut.Agents.Contains("testAgent"));
            Assert.IsTrue(currentAgentCount < sut.Agents.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddAllowedRoute_NullAgent_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddAllowedRoute(null, "adf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddAllowedRoute_EmptyAgent_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddAllowedRoute("", "adf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Robots_AddAllowedRoute_AgnetNotRegistered_Throws_ArgumentException()
        {
            var sut = CreateRobotsInstance();
            sut.AddAllowedRoute("my agent", "adf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddAllowedRoute_NullRoute_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddAllowedRoute("*", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddAllowedRoute_EmptyRoute_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddAllowedRoute("*", "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddDeniedRoute_NullAgent_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddDeniedRoute(null, "adf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddDeniedRoute_EmptyAgent_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddDeniedRoute("", "adf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Robots_AddDeniedRoute_AgnetNotRegistered_Throws_ArgumentException()
        {
            var sut = CreateRobotsInstance();
            sut.AddDeniedRoute("my agent", "adf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddDeniedRoute_NullRoute_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddDeniedRoute("*", null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Robots_AddDeniedRoute_EmptyRoute_Throws_ArgumentNullException()
        {
            var sut = CreateRobotsInstance();
            sut.AddDeniedRoute("*", "");
        }

        [TestMethod]
        public void Robots_AddAllowedRoute_Returns_True()
        {
            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            bool added = sut.AddAllowedRoute("testAgent", "/*");
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void Robots_AddDuplicateAllowedRoute_Returns_False()
        {
            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            bool added = sut.AddAllowedRoute("testAgent", "/*");
            Assert.IsTrue(added);

            added = sut.AddAllowedRoute("testAgent", "/*");
            Assert.IsFalse(added);
        }

        [TestMethod]
        public void Robots_RemoveExistingCustomRoute_Returns_True()
        {
            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            bool added = sut.AddAllowedRoute("testAgent", "/*");
            Assert.IsTrue(added);

            bool removed = sut.RemoveRoute("testAgent", "/*");
            Assert.IsTrue(removed);
        }

        [TestMethod]
        public void Robots_RemoveNonExistingCustomRoute_Returns_False()
        {
            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            bool removed = sut.RemoveRoute("testAgent", "/*");
            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void Robots_AddDeniedRoute_Returns_True()
        {
            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            bool added = sut.AddDeniedRoute("testAgent", "/*");
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void Robots_AddDuplicateDeniedRoute_Returns_False()
        {
            var sut = CreateRobotsInstance();
            int currentAgentCount = sut.Agents.Count;

            bool agentAdded = sut.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            Assert.AreEqual(currentAgentCount + 1, sut.Agents.Count);

            bool added = sut.AddDeniedRoute("testAgent", "/*");
            Assert.IsTrue(added);

            added = sut.AddDeniedRoute("testAgent", "/*");
            Assert.IsFalse(added);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_RobotRouteData_InvalidAgent_Null_Throws_ArgumentNullException()
        {
            new RobotRouteData(null, null, "adf", true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_RobotRouteData_InvalidRoute_Null_Throws_ArgumentNullException()
        {
            new RobotRouteData(null, "", "adf", true, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Construct_RobotRouteData_InvalidRoute_ValidUri_Throws_ArgumentException()
        {
            new RobotRouteData("*", null, "http://test.it", true, true);
        }

        [TestMethod]
        public void Construct_RobotRouteData_Valid_StoresValues()
        {
            RobotRouteData sut = new RobotRouteData("testAgent", "comment", "/route", true, true);

            Assert.AreEqual("testAgent", sut.Agent);
        }

        [TestMethod]
        public void Construct_RobotRouteData_Valid_StoresValues_CaseSensitive()
        {
            RobotRouteData sut = new RobotRouteData("testAgent", null, "/route", true, true);

            Assert.AreNotEqual("TestAgent", sut.Agent);
        }

        [TestMethod]
        public async Task Call_Invoke_ValidContext_FindCustomDeniedRoute_Success()
        {
            var robot = CreateRobotsInstance();

            bool agentAdded = robot.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            bool added = robot.AddDeniedRoute("testAgent", "/*");
            Assert.IsTrue(added);

            TestNotificationService testNotificationService = new TestNotificationService(new List<object>() { "/sitemap.xml" });
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(new TestLogger(), robot, testNotificationService);

            TestHttpContext httpContext = CreateContext("/robots.txt", false,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            Assert.AreEqual(200, httpResponse.StatusCode);
            Assert.IsTrue(httpResponse.ContentLength > 0);
            Assert.IsTrue(testNotificationService.NotificationRaised(Constants.NotificationSitemapNames) > 0);

            httpResponse.Body.Position = 0;
            byte[] responseDataBytes = new byte[httpResponse.Body.Length];
            await httpResponse.Body.ReadAsync(responseDataBytes, 0, responseDataBytes.Length);
            string result = Encoding.UTF8.GetString(responseDataBytes);
            Assert.IsTrue(result.IndexOf("testAgent\r\nDisallow: /*") > -1);
        }

        [TestMethod]
        public async Task AddCustomAgent_NoRoutes_NotFoundInRobots_txt_Success()
        {
            var robot = CreateRobotsInstance();

            bool agentAdded = robot.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);

            TestNotificationService testNotificationService = new TestNotificationService(new List<object>() { "/sitemap.xml" });
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(new TestLogger(), robot, testNotificationService);

            TestHttpContext httpContext = CreateContext("/robots.txt", false,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            httpResponse.Body.Position = 0;
            byte[] responseDataBytes = new byte[httpResponse.Body.Length];
            await httpResponse.Body.ReadAsync(responseDataBytes, 0, responseDataBytes.Length);
            string result = Encoding.UTF8.GetString(responseDataBytes);
            Assert.IsTrue(result.IndexOf("testAgent") == -1);
        }

        [TestMethod]
        public async Task Call_Invoke_ValidContext_FindCustomAllowedRoute_Success()
        {
            var robot = CreateRobotsInstance();

            bool agentAdded = robot.AgentAdd("testAgent");
            Assert.IsTrue(agentAdded);
            bool added = robot.AddAllowedRoute("testAgent", "/*");
            Assert.IsTrue(added);

            TestNotificationService testNotificationService = new TestNotificationService(new List<object>() { "/sitemap.xml" });
            SpiderMiddleware sut = CreateSpiderMiddlewareInstance(new TestLogger(), robot, testNotificationService);

            TestHttpContext httpContext = CreateContext("/robots.txt", false,
                out TestHttpResponse httpResponse, out _);

            await sut.Invoke(httpContext);

            Assert.AreEqual(200, httpResponse.StatusCode);
            Assert.IsTrue(httpResponse.ContentLength > 0);
            Assert.IsTrue(testNotificationService.NotificationRaised(Constants.NotificationSitemapNames) > 0);

            httpResponse.Body.Position = 0;
            byte[] responseDataBytes = new byte[httpResponse.Body.Length];
            await httpResponse.Body.ReadAsync(responseDataBytes, 0, responseDataBytes.Length);
            string result = Encoding.UTF8.GetString(responseDataBytes);
            Assert.IsTrue(result.IndexOf("\r\nUser-agent: testAgent\r\nAllow: /*\r\n") > -1);
        }


        #region Private Methods

        private TestHttpContext CreateContext(string path, bool includeServiceCollection,
            out TestHttpResponse httpResponse, out TestHttpRequest httpRequest)
        {
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            return CreateContext(path, includeServiceCollection, serviceCollection, out httpResponse, out httpRequest);
        }

        private TestHttpContext CreateContext(string path, bool includeServiceCollection,
            IServiceCollection serviceCollection,
            out TestHttpResponse httpResponse, out TestHttpRequest httpRequest)
        {
            TestRequestCookieCollection cookies = new TestRequestCookieCollection();
            httpRequest = new TestHttpRequest(cookies);
            httpResponse = new TestHttpResponse();
            httpRequest.Path = path;
            TestHttpContext Result = null;

            if (includeServiceCollection)
            {
                Assert.IsNotNull(serviceCollection);

                Result = new TestHttpContext(httpRequest, httpResponse, serviceCollection.BuildServiceProvider());
            }
            else
            {
                Result = new TestHttpContext(httpRequest, httpResponse);
            }

            httpRequest.SetContext(Result);

            return Result;
        }

        private SpiderMiddleware CreateSpiderMiddlewareInstance(TestNotificationService customNotification = null,
            bool createDescriptors = false)
        {
            return CreateSpiderMiddlewareInstance(new TestLogger(), customNotification, createDescriptors);
        }

        private SpiderMiddleware CreateSpiderMiddlewareInstance(TestLogger testLogger,
            TestNotificationService customNotification = null,
            bool createDescriptors = false)
        {
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;

            ActionDescriptorCollection actionDescriptorCollection = null;
            if (createDescriptors)
            {
                var descriptors = new List<ActionDescriptor>()
                {
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Test",
                            Name = "Login"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "AspNetCore.PluginManager.DemoWebsite.Controllers.HomeController.Error",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Home",
                            Name = "Error"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController.GetCaptchaImage",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Login",
                            Name = "Captcha"
                        }
                    }
                };
                actionDescriptorCollection = new ActionDescriptorCollection(descriptors, 1);
            }
            else
            {
                actionDescriptorCollection = new ActionDescriptorCollection(new List<ActionDescriptor>(), 1);
            }

            return CreateSpiderMiddlewareInstance(testLogger,
                new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), new RouteDataServices(), pluginTypesServices, new MockLoadData()),
                customNotification);
        }

        private SpiderMiddleware CreateSpiderMiddlewareInstance(TestLogger testLogger,
            IRobots robots,
            TestNotificationService customNotification = null)
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginServices = new pm.PluginServices(_testPluginSpider) as IPluginHelperService;

            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            return new SpiderMiddleware(requestDelegate,
                pluginServices,
                settingsProvider,
                testLogger,
                customNotification ?? new TestNotificationService(),
                robots);
        }

        private Robots CreateRobotsInstance(ILoadData loadData = null)
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            IPluginHelperService pluginServices = new pm.PluginServices(_testPluginSpider) as IPluginHelperService;
            IPluginTypesService pluginTypesServices = new pm.PluginServices(_testPluginSpider) as IPluginTypesService;

            var descriptors = new List<ActionDescriptor>()
                {
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Test",
                            Name = "Login"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "AspNetCore.PluginManager.DemoWebsite.Controllers.HomeController.Error",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Home",
                            Name = "Error"
                        }
                    },
                    new ActionDescriptor()
                    {
                        DisplayName = "LoginPlugin.Controllers.LoginController.GetCaptchaImage",
                        AttributeRouteInfo = new Microsoft.AspNetCore.Mvc.Routing.AttributeRouteInfo()
                        {
                            Template = "Login",
                            Name = "Captcha"
                        }
                    }
                };
            ActionDescriptorCollection actionDescriptorCollection = new ActionDescriptorCollection(descriptors, 1);

            if (loadData == null)
                loadData = new MockLoadData();

            return new Robots(new TestActionDescriptorCollectionProvider(actionDescriptorCollection), new RouteDataServices(), pluginTypesServices, loadData);
        }

        #endregion Private Methods
    }
}
