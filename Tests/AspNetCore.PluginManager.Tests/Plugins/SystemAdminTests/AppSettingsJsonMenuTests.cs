/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: AppSettingsJsonMenuTests.cs
 *
 *  Purpose:  Tests for system admin menu - All timings
 *
 *  Date        Name                Reason
 *  02/10/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes.MenuItems;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AppSettingsJsonMenuTests
    {
        private const string TestCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProvider_Null_Throws_ArgumentNullException()
        {
            AppSettingsJsonMenu sut = new AppSettingsJsonMenu(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void CreateValidInstance_ViewingAppSettings_Json_Enabled_Success()
        {
            AppSettingsJsonMenu sut = new AppSettingsJsonMenu(new MockSettingsProvider("{\"SystemAdmin\":{\"ShowAppSettingsJson\":true}}"));

            Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
            Assert.AreEqual("", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("", sut.Controller());
            Assert.AreEqual("", sut.Image());
            Assert.AreEqual(Enums.SystemAdminMenuType.Text, sut.MenuType());
            Assert.AreEqual("appsettingsjson", sut.Name());
            Assert.AreEqual(0, sut.SortOrder());
            Assert.AreEqual("System", sut.ParentMenuName());
            string ExpectedData = sut.Data();

            Assert.IsTrue(sut.Data().StartsWith("{\n"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void CreateValidInstance_ViewingAppSettings_Json_Disabled_Success()
        {
            AppSettingsJsonMenu sut = new AppSettingsJsonMenu(new MockSettingsProvider());

            Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
            Assert.AreEqual("", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("", sut.Controller());
            Assert.AreEqual("", sut.Image());
            Assert.AreEqual(Enums.SystemAdminMenuType.Text, sut.MenuType());
            Assert.AreEqual("appsettingsjson", sut.Name());
            Assert.AreEqual(0, sut.SortOrder());
            Assert.AreEqual("System", sut.ParentMenuName());
            Assert.AreEqual("Viewing appsettings.json has been disabled", sut.Data());
        }
    }
}
