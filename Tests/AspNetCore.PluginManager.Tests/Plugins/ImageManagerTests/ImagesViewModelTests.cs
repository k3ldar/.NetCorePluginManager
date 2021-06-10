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
 *  File: ImagesViewModelTests.cs
 *
 *  Purpose:  Unit tests for Images View Model
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

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Models;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImagesViewModelTests : GenericBaseClass
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceUsingDefaultConstructor_Success()
        {
            ImagesViewModel sut = new ImagesViewModel();

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamBaseModelData_Null_Throws_ArgumentNullException()
        {
            ImagesViewModel sut = new ImagesViewModel(null, false, String.Empty, String.Empty, null, new Dictionary<string, List<string>>(), new List<ImageFile>());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), false, null, String.Empty, null, new Dictionary<string, List<string>>(), new List<ImageFile>());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamSubgroupName_Null_Throws_ArgumentNullException()
        {
            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), false, "group", null, null, new Dictionary<string, List<string>>(), new List<ImageFile>());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamGroups_Null_Throws_ArgumentNullException()
        {
            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), false, String.Empty, String.Empty, null, null, new List<ImageFile>());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamImageFiles_Null_Throws_ArgumentNullException()
        {
            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), false, String.Empty, String.Empty, null, new Dictionary<string, List<string>>(), null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceEmptyGroupName_Success()
        {
            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), false, "", String.Empty, null, new Dictionary<string, List<string>>(), new List<ImageFile>());
            Assert.IsNotNull(sut);
            Assert.AreEqual("", sut.SelectedGroupName);
            Assert.IsNotNull(sut.ImageFiles);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_WithoutImage_Success()
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
            {
                { "My group", new List<string>() },
                { "Second Group", new List<string>() }
            };

            List<ImageFile> imageFiles = new List<ImageFile>()
            {
                { new ImageFile(new Uri("/PathName/validGifFile.gif", UriKind.RelativeOrAbsolute), "validGifFile.gif", ".gif", 23, DateTime.Now, DateTime.Now) }
            };

            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), false, "My group", "Subgroup 1", null, groups, imageFiles);

            Assert.IsNotNull(sut);
            Assert.AreEqual("My group", sut.SelectedGroupName);
            Assert.AreEqual("Subgroup 1", sut.SelectedSubgroupName);
            Assert.IsNotNull(sut.ImageFiles);
            Assert.AreEqual(1, sut.ImageFiles.Count);
            Assert.AreEqual("/PathName/validGifFile.gif", sut.ImageFiles[0].ImageUri.ToString());
            Assert.AreEqual("validGifFile.gif", sut.ImageFiles[0].Name);
            Assert.AreEqual(2, sut.Groups.Count);
            Assert.IsTrue(sut.Groups.ContainsKey("My group"));
            Assert.IsTrue(sut.Groups.ContainsKey("Second Group"));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_WithImageAndImageManagement_Success()
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
            {
                { "My group", new List<string>() },
                { "Second Group", new List<string>() }
            };

            List<ImageFile> imageFiles = new List<ImageFile>()
            {
                { new ImageFile(new Uri("/PathName/validGifFile.gif", UriKind.RelativeOrAbsolute), "validGifFile.gif", ".gif", 23, DateTime.Now, DateTime.Now) }
            };

            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), true, "My group", "Subgroup 1", imageFiles[0], groups, imageFiles);

            Assert.IsNotNull(sut);
            Assert.IsTrue(sut.CanManageImages);
            Assert.AreEqual("My group", sut.SelectedGroupName);
            Assert.AreEqual("Subgroup 1", sut.SelectedSubgroupName);
            Assert.IsNotNull(sut.ImageFiles);
            Assert.AreEqual(1, sut.ImageFiles.Count);
            Assert.AreEqual("/PathName/validGifFile.gif", sut.ImageFiles[0].ImageUri.ToString());
            Assert.AreEqual("validGifFile.gif", sut.ImageFiles[0].Name);
            Assert.AreEqual(2, sut.Groups.Count);
            Assert.IsTrue(sut.Groups.ContainsKey("My group"));
            Assert.IsTrue(sut.Groups.ContainsKey("Second Group"));
            Assert.IsNotNull(sut.SelectedImageFile);
            Assert.AreEqual("validGifFile.gif", sut.SelectedImageFile.Name);
            Assert.AreEqual(".gif", sut.SelectedImageFile.FileExtension);
            Assert.AreEqual(23, sut.SelectedImageFile.Size);
            Assert.AreEqual(new Uri("/PathName/validGifFile.gif", UriKind.RelativeOrAbsolute), sut.SelectedImageFile.ImageUri);
            Assert.AreNotEqual(DateTime.MinValue, sut.SelectedImageFile.CreateDate);
            Assert.AreNotEqual(DateTime.MinValue, sut.SelectedImageFile.ModifiedDate);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_WithImageAndWithoutImageManagement_Success()
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
            {
                { "My group", new List<string>() },
                { "Second Group", new List<string>() }
            };

            List<ImageFile> imageFiles = new List<ImageFile>()
            {
                { new ImageFile(new Uri("/PathName/validGifFile.gif", UriKind.RelativeOrAbsolute), "validGifFile.gif", ".gif", 23, DateTime.Now, DateTime.Now) }
            };

            ImagesViewModel sut = new ImagesViewModel(GenerateTestBaseModelData(), false, "My group", "Subgroup 1", imageFiles[0], groups, imageFiles);

            Assert.IsNotNull(sut);
            Assert.IsFalse(sut.CanManageImages);
            Assert.AreEqual("My group", sut.SelectedGroupName);
            Assert.AreEqual("Subgroup 1", sut.SelectedSubgroupName);
            Assert.IsNotNull(sut.ImageFiles);
            Assert.AreEqual(1, sut.ImageFiles.Count);
            Assert.AreEqual("/PathName/validGifFile.gif", sut.ImageFiles[0].ImageUri.ToString());
            Assert.AreEqual("validGifFile.gif", sut.ImageFiles[0].Name);
            Assert.AreEqual(2, sut.Groups.Count);
            Assert.IsTrue(sut.Groups.ContainsKey("My group"));
            Assert.IsTrue(sut.Groups.ContainsKey("Second Group"));
            Assert.IsNotNull(sut.SelectedImageFile);
            Assert.AreEqual("validGifFile.gif", sut.SelectedImageFile.Name);
            Assert.AreEqual(".gif", sut.SelectedImageFile.FileExtension);
            Assert.AreEqual(23, sut.SelectedImageFile.Size);
            Assert.AreEqual(new Uri("/PathName/validGifFile.gif", UriKind.RelativeOrAbsolute), sut.SelectedImageFile.ImageUri);
            Assert.AreNotEqual(DateTime.MinValue, sut.SelectedImageFile.CreateDate);
            Assert.AreNotEqual(DateTime.MinValue, sut.SelectedImageFile.ModifiedDate);
        }
    }
}
