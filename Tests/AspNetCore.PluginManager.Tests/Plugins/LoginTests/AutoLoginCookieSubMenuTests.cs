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
 *  File: AutoLoginCookieSubMenuTests.cs
 *
 *  Purpose:  Tests for AutoLoginCookieSubMenu
 *
 *  Date        Name                Reason
 *  09/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using LoginPlugin.Classes.SystemAdmin;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AutoLoginCookieSubMenuTests
    {
        private const string TestCategoryName = "Login";
        private const string SystemAdminCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void CreateValidInstance_Success()
        {
            const string DataResult = "Setting|Value\rTotal Requests|0\rFastest ms|0\rSlowest ms|0\rAverage ms|0\rTrimmed Avg ms|0\rTotal ms|0";
            AutoLoginCookieSubMenu sut = new AutoLoginCookieSubMenu();

            Assert.IsNotNull(sut);

            Assert.AreEqual("", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("", sut.Controller());
            Assert.AreEqual(DataResult, sut.Data());
            Assert.AreEqual("stopwatch", sut.Image());
            Assert.AreEqual(SystemAdminMenuType.Grid, sut.MenuType());
            Assert.AreEqual("Cookie Login", sut.Name());
            Assert.AreEqual("Timings", sut.ParentMenuName());
            Assert.AreEqual(0, sut.SortOrder());
        }
    }
}
