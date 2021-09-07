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
 *  File: SitemapMenuTimingTests.cs
 *
 *  Purpose:  Tests for sitemap timing system admin menu
 *
 *  Date        Name                Reason
 *  31/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Sitemap.Plugin.Classes.SystemAdmin;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Tests.Plugins.SitemapTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class SitemapMenuTimingTests
    {
        private const string TestCategoryName = "Sitemap Plugin";
        private const string SystemAdminCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void Validate_MenuSettings_Success()
        {
            SitemapTimingsSubMenu sut = new SitemapTimingsSubMenu();

            Assert.IsNotNull(sut);

            Assert.AreEqual("", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("", sut.Controller());
            string data = sut.Data();
            Assert.IsTrue(data.StartsWith("Setting|Value\rTotal Requests"));
            Assert.IsTrue(data.Contains("\rFastest ms|"));
            Assert.IsTrue(data.Contains("\rSlowest ms|"));
            Assert.IsTrue(data.Contains("\rAverage ms|"));
            Assert.IsTrue(data.Contains("\rTrimmed Avg ms|"));
            Assert.IsTrue(data.Contains("\rTotal ms|"));
            Assert.IsTrue(data.Length < 110, "Has a new column been added to data as it is too long");
            Assert.AreEqual("stopwatch", sut.Image());
            Assert.AreEqual(SystemAdminMenuType.Grid, sut.MenuType());
            Assert.AreEqual("Sitemap", sut.Name());
            Assert.AreEqual("Timings", sut.ParentMenuName());
            Assert.AreEqual(0, sut.SortOrder());
        }
    }
}
