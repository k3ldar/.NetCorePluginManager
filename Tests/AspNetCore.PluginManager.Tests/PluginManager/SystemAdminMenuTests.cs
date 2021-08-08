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
 *  File: LoadedModulesMenuTests.cs
 *
 *  Purpose:  Tests loaded modules system admin menu
 *
 *  Date        Name                Reason
 *  29/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Classes.SystemAdmin;
using AspNetCore.PluginManager.Middleware;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

using pm = PluginManager;

namespace AspNetCore.PluginManager.Tests.AspNetCore.PluginManager
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SystemAdminMenuTests : GenericBaseClass
    {
        private const string TestCategoryName = "AspNetCore Plugin Manager Tests";
        private const string SystemAdminCategoryName = "System Admin Menu";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void LoadedModulesMenu_CreateValidInstance_Success()
        {
            string DefaultDataStart = "Module|FileVersion\r";
            string[] DefaultDataContains = { "Module|FileVersion", "\rAppSettings.dll|", "\rAspNetCore.PluginManager.dll|", "\rAspNetCore.PluginManager.Tests.dll|" };

            PluginManagerService.Initialise();
            try
            {
                LoadedModulesMenu sut = new LoadedModulesMenu();

                Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
                Assert.AreEqual("", sut.Action());
                Assert.AreEqual("", sut.Area());
                Assert.AreEqual("", sut.Controller());
                Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
                Assert.AreEqual("Loaded Modules", sut.Name());
                Assert.AreEqual("System", sut.ParentMenuName());
                Assert.AreEqual(0, sut.SortOrder());
                Assert.AreEqual("", sut.Image());
                string data = sut.Data();

                ValidateSystemAdminColumnCounts(data);
                Assert.IsTrue(data.StartsWith(DefaultDataStart));

                for (int i = 0; i < DefaultDataContains.Length; i++)
                    Assert.IsTrue(data.Contains(DefaultDataContains[i]), "Should contain " + DefaultDataContains[i]);
            }
            finally
            {
                PluginManagerService.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void LoadedPluginsMenu_CreateValidInstance_Success()
        {
            string DefaultDataStart = "Module|Plugin Version|File Version\r";
            string[] DefaultDataContains = { "Module|Plugin Version|File Version", "\rPluginManager.dll|1|", "\rAspNetCore.PluginManager.dll|1|", "\rDemoApiPlugin.dll|1|", "\rDemoWebsitePlugin.dll|1|" };

            PluginManagerService.Initialise();
            try
            {
                LoadedPluginsMenu sut = new LoadedPluginsMenu();

                Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
                Assert.AreEqual("", sut.Action());
                Assert.AreEqual("", sut.Area());
                Assert.AreEqual("", sut.Controller());
                Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
                Assert.AreEqual("Loaded Plugins", sut.Name());
                Assert.AreEqual("System", sut.ParentMenuName());
                Assert.AreEqual(0, sut.SortOrder());
                Assert.AreEqual("", sut.Image());
                string data = sut.Data();

                ValidateSystemAdminColumnCounts(data);
                Assert.IsTrue(data.StartsWith(DefaultDataStart));

                for (int i = 0; i < DefaultDataContains.Length; i++)
                    Assert.IsTrue(data.Contains(DefaultDataContains[i]), "Should contain " + DefaultDataContains[i]);
            }
            finally
            {
                PluginManagerService.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void ThreadMenu_CreateValidInstance_Success()
        {
            ThreadManager.Initialise();
            try
            {
                string DefaultDataStart = "Name|Process Usage|System Usage|Thread Id|Cancelled|Unresponsive|Marked For Removal";
                string[] DefaultDataContains = { "Name|Process Usage|System Usage|Thread Id|Cancelled|Unresponsive|Marked For Removal\r", "Just a test|" };

                TestThread testThread = new TestThread();

                PluginManagerService.Initialise();
                try
                {
                    ThreadManager.ThreadStart(testThread, "Just a test", System.Threading.ThreadPriority.Normal);

                    ThreadMenu sut = new ThreadMenu();

                    Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
                    Assert.AreEqual("", sut.Action());
                    Assert.AreEqual("", sut.Area());
                    Assert.AreEqual("", sut.Controller());
                    Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
                    Assert.AreEqual("Threads", sut.Name());
                    Assert.AreEqual("System", sut.ParentMenuName());
                    Assert.AreEqual(0, sut.SortOrder());
                    Assert.AreEqual("", sut.Image());
                    string data = sut.Data();

                    ValidateSystemAdminColumnCounts(data);
                    Assert.IsTrue(data.StartsWith(DefaultDataStart));

                    for (int i = 0; i < DefaultDataContains.Length; i++)
                        Assert.IsTrue(data.Contains(DefaultDataContains[i]), "Should contain " + DefaultDataContains[i]);
                }
                finally
                {
                    testThread.CancelThread();
                    PluginManagerService.Finalise();
                }
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void LoggerStatisticsMenu_CreateValidInstance_Success()
        {
            string DefaultDataStart = "DateTime|Log Type|Message\r";
            string[] DefaultDataContains = { "DateTime|Log Type|Message\r", "|PluginLoadSuccess|PluginManager.dll", "|PluginConfigureError|testhost.dll contains no IPlugin Interface", "|PluginLoadSuccess|AspNetCore.PluginManager.dll", "|PluginLoadSuccess|DemoApiPlugin.dll" };

            PluginManagerService.Initialise();
            try
            {
                LoggerStatisticsMenu sut = new LoggerStatisticsMenu();

                Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
                Assert.AreEqual("", sut.Action());
                Assert.AreEqual("", sut.Area());
                Assert.AreEqual("", sut.Controller());
                Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
                Assert.AreEqual("Logs", sut.Name());
                Assert.AreEqual("System", sut.ParentMenuName());
                Assert.AreEqual(0, sut.SortOrder());
                Assert.AreEqual("", sut.Image());
                string data = sut.Data();

                ValidateSystemAdminColumnCounts(data);
                Assert.IsTrue(data.StartsWith(DefaultDataStart));

                for (int i = 0; i < DefaultDataContains.Length; i++)
                    Assert.IsTrue(data.Contains(DefaultDataContains[i]), "Should contain " + DefaultDataContains[i]);
            }
            finally
            {
                PluginManagerService.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerStatisticsMenu_SetLogger_InvalidParam_Null_Throws_ArgumentNullException()
        {
            LoggerStatisticsMenu.SetLogger(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerStatisticsMenu_AddToLog_InvalidParam_Exception_Null_Throws_ArgumentNullException()
        {
            LoggerStatisticsMenu sut = new LoggerStatisticsMenu();
            sut.AddToLog(pm.LogLevel.Error, exception: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerStatisticsMenu_AddToLog_InvalidParam_Data_Null_Throws_ArgumentNullException()
        {
            LoggerStatisticsMenu sut = new LoggerStatisticsMenu();
            sut.AddToLog(pm.LogLevel.Error, data: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerStatisticsMenu_AddToLog_InvalidParam_Data_EmptyString_Throws_ArgumentNullException()
        {
            LoggerStatisticsMenu sut = new LoggerStatisticsMenu();
            sut.AddToLog(pm.LogLevel.Error, data: "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoggerStatisticsMenu_AddToLog_InvalidParam_Exception_Null_WithModuleName_Throws_ArgumentNullException()
        {
            LoggerStatisticsMenu sut = new LoggerStatisticsMenu();
            sut.AddToLog(pm.LogLevel.Error, module: "my module", exception: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void LoggerStatisticsMenu_EnsureLogQueueDoesNotExceedMaxQueueLength_Success()
        {
            string DefaultDataStart = "DateTime|Log Type|Message\r";
            string[] DefaultDataContains = { "DateTime|Log Type|Message\r", "|PluginLoadSuccess|PluginManager.dll", "|PluginConfigureError|testhost.dll contains no IPlugin Interface", "|PluginLoadSuccess|AspNetCore.PluginManager.dll", "|PluginLoadSuccess|DemoApiPlugin.dll" };

            PluginManagerService.Initialise();
            try
            {
                TestLogger testLogger = new TestLogger();
                LoggerStatisticsMenu.SetLogger(testLogger);
                LoggerStatisticsMenu sut = new LoggerStatisticsMenu();

                for (int i = 0; i < 100; i++)
                {
                    sut.AddToLog(pm.LogLevel.Information, new Exception());
                    sut.AddToLog(pm.LogLevel.Warning, "some data");
                    sut.AddToLog(pm.LogLevel.Localization, new Exception(), "exception data");
                    sut.AddToLog(pm.LogLevel.Critical, "module", new Exception());
                    sut.AddToLog(pm.LogLevel.IpRestricted, "module", "some data");
                    sut.AddToLog(pm.LogLevel.ThreadManager, "module", new Exception(), "module exception data");
                }

                LoggerStatisticsMenu.ClearLogger();

                sut.AddToLog(pm.LogLevel.Information, new Exception());
                sut.AddToLog(pm.LogLevel.Warning, "some data");
                sut.AddToLog(pm.LogLevel.Localization, new Exception(), "exception data");
                sut.AddToLog(pm.LogLevel.Critical, "module", new Exception());
                sut.AddToLog(pm.LogLevel.IpRestricted, "module", "some data");
                sut.AddToLog(pm.LogLevel.ThreadManager, "module", new Exception(), "module exception data");

                string data = sut.Data();

                Assert.AreEqual(200, testLogger.Logs.Count);
                Assert.AreEqual(400, testLogger.Errors.Count);

                ValidateSystemAdminColumnCounts(data);
                Assert.IsTrue(data.StartsWith(DefaultDataStart));

                string[] lines = data.Split('\r');
                Assert.AreEqual(100, lines.Length);
            }
            finally
            {
                PluginManagerService.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public async Task RouteLoadTimeMenu_CreateValidInstance_Success()
        {
            string DefaultDataStart = "Route ms|Total Requests|Fastest ms|Slowest ms|Average ms|Trimmed Avg ms|Total ms\r";
            string[] DefaultDataContains = { "\r/products/|1|" };

            PluginManagerService.Initialise();
            try
            {
                bool delegateCalled = false;
                RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
                RouteLoadTimeMiddleware routeLoadTimeMiddleware = new RouteLoadTimeMiddleware(nextDelegate);

                TestHttpRequest httpRequest = new TestHttpRequest()
                {
                    Path = "/products/"
                };

                httpRequest.SetHost(new HostString("www.pluginmanager.website"));
                TestHttpResponse httpResponse = new TestHttpResponse();
                TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
                httpRequest.SetContext(httpContext);

                await routeLoadTimeMiddleware.Invoke(httpContext);
                Assert.IsTrue(delegateCalled);

                RouteLoadTimeMenu sut = new RouteLoadTimeMenu();

                Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
                Assert.AreEqual("", sut.Action());
                Assert.AreEqual("", sut.Area());
                Assert.AreEqual("", sut.Controller());
                Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
                Assert.AreEqual("Route Load Times", sut.Name());
                Assert.AreEqual("Timings", sut.ParentMenuName());
                Assert.AreEqual(0, sut.SortOrder());
                Assert.AreEqual("stopwatch", sut.Image());
                string data = sut.Data();

                ValidateSystemAdminColumnCounts(data);
                Assert.IsTrue(data.StartsWith(DefaultDataStart));

                for (int i = 0; i < DefaultDataContains.Length; i++)
                    Assert.IsTrue(data.Contains(DefaultDataContains[i]), "Should contain " + DefaultDataContains[i]);
            }
            finally
            {
                PluginManagerService.Finalise();
            }
        }
    }

    [ExcludeFromCodeCoverage]
    public class TestThread : ThreadManager
    {
        public TestThread()
            : base(null, new TimeSpan(0, 0, 0, 0, 100))
        {

        }

        protected override bool Run(object parameters)
        {
            return !Cancelled;
        }
    }
}
