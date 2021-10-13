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
 *  File: SystemAdminSettingsTests.cs
 *
 *  Purpose:  Tests for System Admin Settings
 *
 *  Date        Name                Reason
 *  30/09/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SystemAdmin.Plugin;

namespace AspNetCore.PluginManager.Tests.Plugins.SystemAdminTests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class SystemAdminSettingsTests
    {
        private const string TestCategoryDescription = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void Construct_ValidInstance_DefaultSettingsApplied_Success()
        {
            MockSettingsProvider testSettingsProvider = new MockSettingsProvider();
            SystemAdminSettings sut = testSettingsProvider.GetSettings<SystemAdminSettings>("SystemAdmin");
            Assert.IsNotNull(sut);

            Assert.IsTrue(String.IsNullOrEmpty(sut.GoogleMapApiKey));
            Assert.IsFalse(sut.ShowAppSettingsJson);
            Assert.IsFalse(sut.EnableFormattedText);
        }

        [TestMethod]
        [TestCategory(TestCategoryDescription)]
        public void Construct_ValidInstance_SettingsFromJson_Success()
        {
            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"SystemAdmin\":{\"EnableFormattedText\":true,\"ShowAppSettingsJson\":true,\"GoogleMapApiKey\":\"abcdefghijklmnopqrstuvw123xyz\"}}");
            SystemAdminSettings sut = testSettingsProvider.GetSettings<SystemAdminSettings>("SystemAdmin");
            Assert.IsNotNull(sut);

            Assert.AreEqual("abcdefghijklmnopqrstuvw123xyz", sut.GoogleMapApiKey);
            Assert.IsTrue(sut.ShowAppSettingsJson);
            Assert.IsTrue(sut.EnableFormattedText);
        }
    }
}
