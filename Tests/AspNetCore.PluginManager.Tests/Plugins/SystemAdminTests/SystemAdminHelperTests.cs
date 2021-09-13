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

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Abstractions;
using Shared.Classes;

using SharedPluginFeatures;

using SystemAdmin.Plugin;
using SystemAdmin.Plugin.Classes.MenuItems;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SystemAdminHelperTests : GenericBaseClass
    {
        private const string TestsCategory = "SystemAdmin Tests";

        [TestInitialize]
        public void InitializeTests()
        {
            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.ClearAllCaches();
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamMemoryCache_Null_Throws_ArgumentNullException()
        {
            new SystemAdminHelper(null, new TestPluginClassesService());
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamPluginClassServices_Null_Throws_ArgumentNullException()
        {
            new SystemAdminHelper(new TestMemoryCache(), null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]

        public void Construct_ValidInstance_Success()
        {
            SystemAdminHelper sut = new SystemAdminHelper(new TestMemoryCache(), new TestPluginClassesService());
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(ISystemAdminHelperService));
        }

        [TestMethod]
        [TestCategory(TestsCategory)]

        public void GetSystemAdminMainMenu_ReturnsValidAdminMenuList_WithZeroItems_Success()
        {
            SystemAdminHelper sut = new SystemAdminHelper(new TestMemoryCache(), new TestPluginClassesService(null));
            Assert.IsNotNull(sut);

            List<SystemAdminMainMenu> Result = sut.GetSystemAdminMainMenu();
            Assert.IsNotNull(Result);
            Assert.AreEqual(0, Result.Count);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]

        public void GetSystemAdminMainMenu_ReturnsValidAdminMenuList_Success()
        {
            List<object> registeredServices = new List<object>();

            registeredServices.Add(new AppSettingsJsonMenu(new TestSettingsProvider("{}")));
            registeredServices.Add(new GCAdminMenu());

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            SystemAdminHelper sut = new SystemAdminHelper(new TestMemoryCache(), testPluginClassesService);
            Assert.IsNotNull(sut);

            List<SystemAdminMainMenu> Result = sut.GetSystemAdminMainMenu();
            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Result.Count);
            Assert.AreEqual("System", Result[0].Name);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]

        public void GetSystemAdminDefaultMainMenu_ReturnsValidAdminMenuItem_Success()
        {
            List<object> registeredServices = new List<object>();

            registeredServices.Add(new AppSettingsJsonMenu(new TestSettingsProvider("{}")));
            registeredServices.Add(new GCAdminMenu());

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            SystemAdminHelper sut = new SystemAdminHelper(new TestMemoryCache(), testPluginClassesService);
            Assert.IsNotNull(sut);

            SystemAdminMainMenu Result = sut.GetSystemAdminDefaultMainMenu();
            Assert.IsNotNull(Result);
            Assert.AreEqual("System", Result.Name);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSystemAdminMainMenu_IndexNotFound_ReturnsNull_Success()
        {
            List<object> registeredServices = new List<object>();

            registeredServices.Add(new AppSettingsJsonMenu(new TestSettingsProvider("{}")));
            registeredServices.Add(new GCAdminMenu());

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            SystemAdminHelper sut = new SystemAdminHelper(new TestMemoryCache(), testPluginClassesService);
            Assert.IsNotNull(sut);

            SystemAdminMainMenu Result = sut.GetSystemAdminMainMenu(150);
            Assert.IsNull(Result);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSystemAdminMainMenu_ReturnsValidAdminMenu_Success()
        {
            List<object> registeredServices = new List<object>();

            registeredServices.Add(new AppSettingsJsonMenu(new TestSettingsProvider("{}")));
            registeredServices.Add(new GCAdminMenu());

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestMemoryCache testMemoryCache = new TestMemoryCache();
            SystemAdminHelper sut = new SystemAdminHelper(testMemoryCache, testPluginClassesService);
            Assert.IsNotNull(sut);

            SystemAdminMainMenu Result = sut.GetSystemAdminMainMenu(1);
            Assert.IsNotNull(Result);
            Assert.AreEqual("System", Result.Name);
            Assert.IsNotNull(testMemoryCache.GetCache().Get("System Admin Main Menu Cache"));

            // retrieve again from cache
            Result = sut.GetSystemAdminMainMenu(1);
            Assert.IsNotNull(Result);
            Assert.AreEqual("System", Result.Name);
            Assert.AreEqual(2, Result.ChildMenuItems.Count);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSubMenuItems_NoSubMenuItemsLoaded_ReturnsEmptyList_Success()
        {
            List<object> registeredServices = new List<object>();

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestMemoryCache testMemoryCache = new TestMemoryCache();
            SystemAdminHelper sut = new SystemAdminHelper(testMemoryCache, testPluginClassesService);
            Assert.IsNotNull(sut);

            List<SystemAdminSubMenu> Result = sut.GetSubMenuItems();
            Assert.IsNotNull(Result);
            Assert.AreEqual(0, Result.Count);
            Assert.IsNotNull(testMemoryCache.GetCache().Get("System Admin Main Menu Cache"));
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSubMenuItems_ReturnsAllValidSubMenuItems_Success()
        {
            List<object> registeredServices = new List<object>();

            registeredServices.Add(new AppSettingsJsonMenu(new TestSettingsProvider("{}")));
            registeredServices.Add(new GCAdminMenu());

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestMemoryCache testMemoryCache = new TestMemoryCache();
            SystemAdminHelper sut = new SystemAdminHelper(testMemoryCache, testPluginClassesService);
            Assert.IsNotNull(sut);

            List<SystemAdminSubMenu> Result = sut.GetSubMenuItems();
            Assert.IsNotNull(Result);
            Assert.AreEqual(2, Result.Count);
            Assert.IsNotNull(testMemoryCache.GetCache().Get("System Admin Main Menu Cache"));
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSubMenuItems_ByName_NoSubMenuItemsLoaded_ReturnsEmptyList_Success()
        {
            List<object> registeredServices = new List<object>();

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestMemoryCache testMemoryCache = new TestMemoryCache();
            SystemAdminHelper sut = new SystemAdminHelper(testMemoryCache, testPluginClassesService);
            Assert.IsNotNull(sut);

            List<SystemAdminSubMenu> Result = sut.GetSubMenuItems("Invalid Parent Name");
            Assert.IsNotNull(Result);
            Assert.AreEqual(0, Result.Count);
            Assert.IsNotNull(testMemoryCache.GetCache().Get("System Admin Main Menu Cache"));
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSubMenuItems_ByName_ReturnsAllValidSubMenuItems_Success()
        {
            List<object> registeredServices = new List<object>();

            registeredServices.Add(new TestSystemAdminMainMenu("Unused", 9876543));
            registeredServices.Add(new AppSettingsJsonMenu(new TestSettingsProvider("{}")));
            registeredServices.Add(new GCAdminMenu());

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestMemoryCache testMemoryCache = new TestMemoryCache();
            SystemAdminHelper sut = new SystemAdminHelper(testMemoryCache, testPluginClassesService);
            Assert.IsNotNull(sut);

            List<SystemAdminSubMenu> Result = sut.GetSubMenuItems("Unused");
            Assert.IsNotNull(Result);
            Assert.AreEqual(0, Result.Count);

            Result = sut.GetSubMenuItems("System");
            Assert.IsNotNull(Result);
            Assert.AreEqual(2, Result.Count);

            Assert.IsNotNull(testMemoryCache.GetCache().Get("System Admin Main Menu Cache"));
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSubMenuItem_ById_NoSubMenuItemsLoaded_ReturnsEmptyList_Success()
        {
            List<object> registeredServices = new List<object>();

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestMemoryCache testMemoryCache = new TestMemoryCache();
            SystemAdminHelper sut = new SystemAdminHelper(testMemoryCache, testPluginClassesService);
            Assert.IsNotNull(sut);

            SystemAdminSubMenu Result = sut.GetSubMenuItem(2310);
            Assert.IsNull(Result);
            //Assert.AreEqual(0, Result.Count);
            Assert.IsNull(testMemoryCache.GetCache().Get("System Admin Main Menu Cache"));
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetSubMenuItem_ById_ReturnsAllValidSubMenuItems_Success()
        {
            List<object> registeredServices = new List<object>();

            registeredServices.Add(new AppSettingsJsonMenu(new TestSettingsProvider("{}")));
            registeredServices.Add(new GCAdminMenu());

            TestPluginClassesService testPluginClassesService = new TestPluginClassesService(registeredServices);
            TestMemoryCache testMemoryCache = new TestMemoryCache();
            SystemAdminHelper sut = new SystemAdminHelper(testMemoryCache, testPluginClassesService);
            Assert.IsNotNull(sut);

            sut.GetSystemAdminMainMenu();

            SystemAdminSubMenu Result = sut.GetSubMenuItem(2);
            Assert.IsNotNull(Result);
            Assert.IsNotNull(testMemoryCache.GetCache().Get("System Admin Main Menu Cache"));
        }

    }
}
