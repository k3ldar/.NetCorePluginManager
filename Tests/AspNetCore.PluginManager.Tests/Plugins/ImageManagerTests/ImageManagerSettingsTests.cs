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
 *  File: ImageManagerSettingsTests.cs
 *
 *  Purpose:  Unit tests for Image Manager settings
 *
 *  Date        Name                Reason
 *  18/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageManagerSettingsTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";
        private const string DemoWebsiteImagePath = "..\\..\\..\\..\\..\\..\\.NetCorePluginManager\\Demo\\NetCorePluginDemoWebsite\\wwwroot\\images";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_Valid_Success()
        {
            ImageManagerSettings sut = new ImageManagerSettings();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ImagePath_Loads_Successfully()
        {
            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}");
            ImageManagerSettings sut = testSettingsProvider.GetSettings<ImageManagerSettings>("ImageManager");

            Assert.IsNotNull(sut);
            Assert.AreEqual(DemoWebsiteImagePath, sut.ImagePath);
            Assert.IsTrue(Directory.Exists(sut.ImagePath));
        }
    }
}
