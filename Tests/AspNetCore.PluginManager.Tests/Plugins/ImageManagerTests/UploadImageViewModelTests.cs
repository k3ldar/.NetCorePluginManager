/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: UploadImageViewModelTests.cs
 *
 *  Purpose:  Unit tests for upload images View Model
 *
 *  Date        Name                Reason
 *  21/04/2021  Simon Carter        Initially Created
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
    public class UploadImageViewModelTests : GenericBaseClass
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceUsingDefaultConstructor_Success()
        {
            UploadImageViewModel sut = new UploadImageViewModel();

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Files);
            Assert.AreEqual(0, sut.Files.Count);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidArgument_BaseModelData_Null_Throws_ArgumentNullException()
        {
            UploadImageViewModel sut = new UploadImageViewModel(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidArgument_GroupName_Null_Throws_ArgumentNullException()
        {
            UploadImageViewModel sut = new UploadImageViewModel(GenerateTestBaseModelData(), null, null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidArgument_GroupName_EmptyString_Throws_ArgumentNullException()
        {
            UploadImageViewModel sut = new UploadImageViewModel(GenerateTestBaseModelData(), "", null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceWithBaseModelData_Success()
        {
            UploadImageViewModel sut = new UploadImageViewModel(GenerateTestBaseModelData());

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Files);
            Assert.AreEqual(0, sut.Files.Count);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceWithBaseModelDataGroupAndSubgroup_Success()
        {
            UploadImageViewModel sut = new UploadImageViewModel(GenerateTestBaseModelData(), "My Group", "My Subgroup");

            Assert.IsNotNull(sut);
            Assert.IsNotNull(sut.Files);
            Assert.AreEqual(0, sut.Files.Count);
            Assert.AreEqual("My Group", sut.GroupName);
            Assert.AreEqual("My Subgroup", sut.SubgroupName);
        }
    }
}
