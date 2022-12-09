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
 *  File: UserPermissionMenuTests.cs
 *
 *  Purpose:  Tests for user permission admin menu
 *
 *  Date        Name                Reason
 *  03/10/2021  Simon Carter        Initially Created
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
    public class UserPermissionMenuTests
    {
        private const string TestCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void CreateValidInstance__Success()
        {
            UserPermissionsMenu sut = new UserPermissionsMenu();

            Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
            Assert.AreEqual("Permissions", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("SystemAdmin", sut.Controller());
            Assert.AreEqual("", sut.Image());
            Assert.AreEqual(Enums.SystemAdminMenuType.View, sut.MenuType());
            Assert.AreEqual("UserPermissions", sut.Name());
            Assert.AreEqual(0, sut.SortOrder());
            Assert.AreEqual("Permissions", sut.ParentMenuName());
            Assert.AreEqual("", sut.Data());
        }
    }
}
