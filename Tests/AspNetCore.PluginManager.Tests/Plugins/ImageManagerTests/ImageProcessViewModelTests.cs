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
 *  File: ImageProcessViewModelTests.cs
 *
 *  Purpose:  Unit tests for ImageProcessViewModel
 *
 *  Date        Name                Reason
 *  14/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageProcessViewModelTests : GenericBaseClass
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamFileUploadId_Null_Throws_ArgumentNullException()
        {
            ImageProcessViewModel sut = new ImageProcessViewModel(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamFileUploadId_EmptyString_Throws_ArgumentNullException()
        {
            ImageProcessViewModel sut = new ImageProcessViewModel("");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidParamFileUploadId_Success()
        {
            ImageProcessViewModel sut = new ImageProcessViewModel("uploadId");
            Assert.AreEqual("uploadId", sut.FileUploadId);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_PropertiesHaveDefaultValues_Success()
        {
            ImageProcessViewModel sut = new ImageProcessViewModel();

            Assert.IsNull(sut.FileUploadId);
            Assert.IsNull(sut.AdditionalData);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_AssignedPropertiesRemembered_Success()
        {
            ImageProcessViewModel sut = new ImageProcessViewModel()
            {
                FileUploadId = "test upload",
                AdditionalData = "extra data"
            };

            Assert.AreEqual("test upload", sut.FileUploadId);
            Assert.AreEqual("extra data", sut.AdditionalData);
        }
    }
}
