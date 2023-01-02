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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: SystemMenuAllTimings.cs
 *
 *  Purpose:  Tests for system admin menu - All timings
 *
 *  Date        Name                Reason
 *  02/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Classes.SystemAdmin;
using AspNetCore.PluginManager.Tests.Shared;

using CacheControl.Plugin.Classes.SystemAdmin;

using DynamicContent.Plugin.Classes.SystemAdmin;

using LoginPlugin.Classes.SystemAdmin;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

using SystemAdmin.Plugin;
using SystemAdmin.Plugin.Classes.MenuItems;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AllTimingsMenuTests
    {
        private const string TestCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SystemAdminHelperService_Null_Throws_ArgumentNullException()
        {
            List<object> classServices = new List<object>();

            MockPluginClassesService pluginClassesService = new MockPluginClassesService(classServices);
            AllTimings sut = new AllTimings(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void CreateValidInstance_Success()
        {
            List<object> classServices = new List<object>();
            classServices.Add(new GCAdminMenu());
            classServices.Add(new CacheControlTimingsSubMenu());
            classServices.Add(new AutoLoginBasicAuthSubMenu());
            classServices.Add(new AutoLoginCookieSubMenu());
            classServices.Add(new RouteLoadTimeMenu());

            MockPluginClassesService pluginClassesService = new MockPluginClassesService(classServices);
            SystemAdminHelper systemAdminHelper = new SystemAdminHelper(new MockMemoryCache(), pluginClassesService);

            AllTimings sut = new AllTimings(systemAdminHelper);

            Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
            Assert.AreEqual("", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("", sut.Controller());
            Assert.AreEqual("stopwatch", sut.Image());
            Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
            Assert.AreEqual("AllTimings", sut.Name());
            Assert.AreEqual(int.MinValue, sut.SortOrder());
            Assert.AreEqual("Timings", sut.ParentMenuName());
            string data = sut.Data();

            Assert.IsTrue(data.StartsWith("Name|Total Requests|Fastest|Slowest|Average|Trimmed Avg ms|Total ms"));
            Assert.IsTrue(data.Contains("\rBasic Auth Login"));
            Assert.IsTrue(data.Contains("\rCache Control"));
            Assert.IsTrue(data.Contains("\rCookie Login"));
        }
    }
}
