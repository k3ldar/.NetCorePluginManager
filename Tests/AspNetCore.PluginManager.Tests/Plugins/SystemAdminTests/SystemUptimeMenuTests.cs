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
 *  File: SystemUptimeMenuTests.cs
 *
 *  Purpose:  Tests for system admin menu - SystemUptimeMenu
 *
 *  Date        Name                Reason
 *  02/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes.MenuItems;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SystemUptimeMenuTests
    {
        private const string TestCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void CreateValidInstance_Success()
        {
            SystemUptimeMenu sut = new SystemUptimeMenu();

            Assert.IsInstanceOfType(sut, typeof(SystemAdminSubMenu));
            Assert.AreEqual("", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("", sut.Controller());
            Assert.AreEqual("uptime", sut.Image());
            Assert.AreEqual(Enums.SystemAdminMenuType.Grid, sut.MenuType());
            Assert.AreEqual("Uptime", sut.Name());
            Assert.AreEqual(int.MinValue, sut.SortOrder());
            Assert.AreEqual("System", sut.ParentMenuName());
            string data = sut.Data();

            Assert.IsTrue(data.StartsWith("Status|Value\rLoad Time|"));
            Assert.IsTrue(data.Contains("seconds"));
        }
    }
}
