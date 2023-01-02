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
 *  File: ImageManagerSystemMenuMenuTests.cs
 *
 *  Purpose:  Unit tests for image manager system menu
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ImageManager.Plugin.Classes.SystemAdmin;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageManagerSystemMenuMenuTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Validate_MenuSettings_Success()
        {
            ImageManagerSystemAdminMenu sut = new ImageManagerSystemAdminMenu();

            Assert.IsNotNull(sut);

            Assert.AreEqual("Index", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("ImageManager", sut.Controller());
            Assert.AreEqual("", sut.Data());
            Assert.AreEqual("/images/ImageManager/imMenu.png", sut.Image());
            Assert.AreEqual(SystemAdminMenuType.View, sut.MenuType());
            Assert.AreEqual("Image Management", sut.Name());
            Assert.AreEqual("All Images", sut.ParentMenuName());
            Assert.AreEqual(1000, sut.SortOrder());
        }
    }
}
