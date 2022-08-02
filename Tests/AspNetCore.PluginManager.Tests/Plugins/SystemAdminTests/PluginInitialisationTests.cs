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
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  Tests for System Admin Manager Plugin Initialisation
 *
 *  Date        Name                Reason
 *  30/09/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

using Spider.Plugin.Classes.SystemAdmin;

using SystemAdmin.Plugin;
using SystemAdmin.Plugin.Classes.MenuItems;

using UserSessionMiddleware.Plugin.Classes.SystemAdmin;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string TestsCategoryName = "System Admin";

        [TestInitialize]
        public void InitializeTest()
        {
            ThreadManager.Initialise();
        }

        [TestCleanup]
        public void FinalizeTest()
        {
            ThreadManager.Finalise();
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void GetVersion_ReturnsCurrentVersion_Success()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.AreEqual((ushort)1, sut.GetVersion());
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void Initialize_DoesNotAddItemsToLogger()
        {
            PluginInitialisation sut = new PluginInitialisation();
            MockLogger testLogger = new MockLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void AfterConfigure_FailsToRegisterBreadcrumbs()
        {
            Dictionary<Type, object> services = new Dictionary<Type, object>();
            MockServiceProvider testServiceProvider = new MockServiceProvider(services);
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            testApplicationBuilder.ApplicationServices = testServiceProvider;

            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void AfterConfigure_RegistersBreadCrumbs_Success()
        {
            MockBreadcrumbService mockBreadcrumbService = new MockBreadcrumbService();
            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"UserSessionConfiguration\":{\"EnableDefaultSessionService\": true}}");
            List<object> classServices = new List<object>();
            classServices.Add(new GCAdminMenu());
            classServices.Add(new UserPermissionsMenu(testSettingsProvider));
            classServices.Add(new AppSettingsJsonMenu(testSettingsProvider));
            classServices.Add(new SpiderSettingsSubMenu());
            classServices.Add(new CurrentUserLocationMenu());
            classServices.Add(new UserDetailsMenu());
            classServices.Add(new BotVisitsWeeklySubMenu(testSettingsProvider, new MockSessionStatisticsProvider()));

            MockPluginClassesService pluginClassesService = new MockPluginClassesService(classServices);

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IBreadcrumbService), mockBreadcrumbService);
            services.Add(typeof(ISystemAdminHelperService), new SystemAdminHelper(new MockMemoryCache(), pluginClassesService));

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            testApplicationBuilder.ApplicationServices = testServiceProvider;
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);

            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("SystemAdmin"));
            Assert.AreEqual("/SystemAdmin", mockBreadcrumbService.RegisteredRoutes["SystemAdmin"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("Permissions"));
            Assert.AreEqual("/SystemAdmin/Index/3", mockBreadcrumbService.RegisteredRoutes["Permissions"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("UserPermissions"));
            Assert.AreEqual("/SystemAdmin/Permissions/", mockBreadcrumbService.RegisteredRoutes["UserPermissions"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("System"));
            Assert.AreEqual("/SystemAdmin/Index/1", mockBreadcrumbService.RegisteredRoutes["System"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("appsettingsjson"));
            Assert.AreEqual("/SystemAdmin/Text/5", mockBreadcrumbService.RegisteredRoutes["appsettingsjson"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("GC Timings"));
            Assert.AreEqual("/SystemAdmin/Grid/2", mockBreadcrumbService.RegisteredRoutes["GC Timings"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("Robots.txt"));
            Assert.AreEqual("/SystemAdmin/View/6", mockBreadcrumbService.RegisteredRoutes["Robots.txt"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("User Sessions"));
            Assert.AreEqual("/SystemAdmin/Index/7", mockBreadcrumbService.RegisteredRoutes["User Sessions"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("Map of Visitors"));
            Assert.AreEqual("/SystemAdmin/Map/8", mockBreadcrumbService.RegisteredRoutes["Map of Visitors"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("User Session Details"));
            Assert.AreEqual("/SystemAdmin/TextEx/9", mockBreadcrumbService.RegisteredRoutes["User Session Details"]);
            Assert.IsTrue(mockBreadcrumbService.RegisteredRoutes.ContainsKey("Bot Visits - Weekly"));
            Assert.AreEqual("/SystemAdmin/Chart/10", mockBreadcrumbService.RegisteredRoutes["Bot Visits - Weekly"]);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void GetClaims_ReturnsValidClaims()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            List<string> claims = sut.GetClaims();

            Assert.AreEqual(7, claims.Count);
            Assert.AreEqual("StaffMember", claims[0]);
            Assert.AreEqual("Name", claims[1]);
            Assert.AreEqual("UserId", claims[2]);
            Assert.AreEqual("Email", claims[3]);
            Assert.AreEqual("Administrator", claims[4]);
            Assert.AreEqual("ManageSeo", claims[5]);
            Assert.AreEqual("UserPermissions", claims[6]);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void Configure_DoesNotConfigurePipeline_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void BeforeConfigure_DoesNotRegisterApplicationServices()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void Configure_DoesNotRegisterApplicationServices()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void Finalise_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Finalise();
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void BeforeConfigureServices_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.BeforeConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.ServicesRegistered);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void ConfigureServices_Registers_ISystemAdminHelperService_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);

            Assert.AreEqual(1, mockServiceCollection.ServicesRegistered);
            mockServiceCollection.HasServiceRegistered<ISystemAdminHelperService>(ServiceLifetime.Singleton);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AfterConfigureServices_InvalidParam_Services_Null_Throws_ArgumentNullException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();
            mockServiceCollection.AddSingleton<ISettingsProvider>(new MockSettingsProvider());

            sut.AfterConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(TestsCategoryName)]
        public void AfterConfigureServices_CreatesCorrectPolicies_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();
            mockServiceCollection.AddSingleton<ISettingsProvider>(new MockSettingsProvider());

            sut.AfterConfigureServices(mockServiceCollection);

            string[] claims = { "ManageSeo", "Name", "StaffMember", "Email", "UserId" };
            Assert.IsTrue(mockServiceCollection.HasPolicyConfigured("AlterSeo", claims));

            claims = new string[] { "Administrator", "UserPermissions", "StaffMember", };
            Assert.IsTrue(mockServiceCollection.HasPolicyConfigured("ManagePermissions", claims));

            claims = new string[] { "StaffMember", "UserId" };
            Assert.IsTrue(mockServiceCollection.HasPolicyConfigured("MemberOfStaff", claims));
        }
    }
}
