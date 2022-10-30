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
 *  File: ImageProcessOptionsViewModel.cs
 *
 *  Purpose:  Unit tests for image manager system menu
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using ImageManager.Plugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class ImageProcessOptionsViewModelTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Descends_FromIImageProcessOptions_Success()
        {
            ImageProcessOptionsViewModel sut = new ImageProcessOptionsViewModel();
            Assert.IsInstanceOfType(sut, typeof(IImageProcessOptions));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_PropertiesContainDefaultValues_Success()
        {
            ImageProcessOptionsViewModel sut = new ImageProcessOptionsViewModel();
            Assert.IsNull(sut.GroupName);
            Assert.IsNull(sut.SubgroupName);
            Assert.IsTrue(sut.ShowSubgroup);
            Assert.IsNull(sut.AdditionalDataName);
            Assert.IsNull(sut.AdditionalData);
            Assert.IsFalse(sut.AdditionalDataMandatory);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_PropertyValuesStored_Success()
        {
            ImageProcessOptionsViewModel sut = new ImageProcessOptionsViewModel()
            {
                AdditionalData = "CR001",
                AdditionalDataName = "Product Id or SKU",
                AdditionalDataMandatory = true,
                GroupName = "Products",
                SubgroupName = "",
                ShowSubgroup = false
            };

            Assert.AreEqual("Products", sut.GroupName);
            Assert.AreEqual("", sut.SubgroupName);
            Assert.IsFalse(sut.ShowSubgroup);
            Assert.AreEqual("Product Id or SKU", sut.AdditionalDataName);
            Assert.AreEqual("CR001", sut.AdditionalData);
            Assert.IsTrue(sut.AdditionalDataMandatory);
        }
    }
}
