﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: DynamicContentMenuTests.cs
 *
 *  Purpose:  Tests for dynamic content system admin menu
 *
 *  Date        Name                Reason
 *  29/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using DynamicContent.Plugin.Classes.SystemAdmin;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MemoryCacheMenuTests
    {
        private const string TestCategoryName = "Dynamic Content";
        private const string SystemAdminCategoryName = "System Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void CreateValidInstance_Success()
        {
            DynamicContentMenu sut = new DynamicContentMenu();

            Assert.IsInstanceOfType(sut, typeof(SystemAdminMainMenu));
            Assert.AreEqual("DynamicContent", sut.Name);
            Assert.AreEqual("DynamicContent", sut.Controller());
            Assert.AreEqual("GetCustomPages", sut.Action());
            Assert.AreEqual("/Images/DynamicContent/dcicon.png", sut.Image);
        }
    }
}
