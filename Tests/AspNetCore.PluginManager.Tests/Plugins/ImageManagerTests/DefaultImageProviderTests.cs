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
 *  File: ImageManagerControllerTests.cs
 *
 *  Purpose:  Unit tests for Image Manager Controller
 *
 *  Date        Name                Reason
 *  16/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultImageProviderTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";
        private const string DemoWebsiteImagePath = "..\\..\\..\\..\\..\\..\\.NetCorePluginManager\\Demo\\NetCorePluginDemoWebsite\\wwwroot\\images";

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidSettingsProvider_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = new DefaultImageProvider(new TestHostEnvironment(), null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidIHostEnvironment_Null_Throws_ArgumentNullException()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(null, testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_Success()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(new TestHostEnvironment(), testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceWithEmptyPath_Success()
        {
            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(new TestHostEnvironment(), testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateGroup_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathCreated = sut.CreateGroup(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateGroup_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathCreated = sut.CreateGroup("");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void CreateGroup_GroupCreated_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "TestGroup");
            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

            bool pathCreated = sut.CreateGroup("TestGroup");

            Assert.IsTrue(pathCreated);
            Assert.IsTrue(Directory.Exists(newGroupPath));

            Directory.Delete(newGroupPath);
            Directory.Delete(testPath);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void CreateGroup_DuplicateGroupName_ReturnsFalse()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "TestGroup");
            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

            bool pathCreated = sut.CreateGroup("TestGroup");

            Assert.IsTrue(pathCreated);
            Assert.IsTrue(Directory.Exists(newGroupPath));

            pathCreated = sut.CreateGroup(newGroupPath);

            Assert.IsFalse(pathCreated);

            Directory.Delete(newGroupPath);
            Directory.Delete(testPath);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteGroup_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathDeleted = sut.DeleteGroup(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteGroup_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathDeleted = sut.DeleteGroup("");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteGroup_PathDoesNotExist_ReturnsFalse()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            bool pathDeleted = sut.DeleteGroup("Not Exists");

            Assert.IsFalse(pathDeleted);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteGroup_GroupContainsItems_ReturnsSuccess()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            Directory.CreateDirectory(Path.Combine(newGroupPath, "Second Group"));
            Directory.CreateDirectory(Path.Combine(newGroupPath, "Third Group"));
            File.WriteAllText(Path.Combine(newGroupPath, "Second Group", "test.txt"), "test file");

            bool pathDeleted = sut.DeleteGroup("FirstGroup");

            Assert.IsTrue(pathDeleted);

            Assert.IsFalse(Directory.Exists(newGroupPath));

            Directory.Delete(testPath, true);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Groups_Retrieve_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");
            sut.CreateGroup("SecondGroup");

            Directory.CreateDirectory(Path.Combine(newGroupPath, "First Sub Group"));
            Directory.CreateDirectory(Path.Combine(newGroupPath, "Second Sub Group"));
            File.WriteAllText(Path.Combine(newGroupPath, "Second Sub Group", "test.txt"), "test file");

            List<string> groups = sut.Groups();

            Directory.Delete(testPath, true);

            Assert.IsNotNull(groups);

            Assert.AreEqual(2, groups.Count);
            Assert.IsTrue(groups.Contains("FirstGroup"));
            Assert.IsTrue(groups.Contains("SecondGroup"));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Images_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            sut.Images(null);
        }

        //[TestMethod]
        //[TestCategory(ImageManagerTestsCategory)]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void Images_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        //{
        //    DefaultImageProvider sut = CreateDefaultImageProvider();

        //    sut.Images("");
        //}

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentException))]
        public void Images_InvalidGroupName_NotFound_Throws_ArgumentException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("Fantasy Image Group");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidGroupName_ReturnsListOfImages_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");
            File.WriteAllText(Path.Combine(newGroupPath, "file2.txt"), "text for file 2");

            List<ImageFile> images = sut.Images("FirstGroup");

            Directory.Delete(testPath, true);

            Assert.AreEqual(2, images.Count);
            Assert.IsNotNull(images.Where(i => i.Name.Equals("file1.txt")).Any());
            Assert.IsNotNull(images.Where(i => i.Name.Equals("file2.txt")).Any());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidateUri_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");

            List<ImageFile> images = sut.Images("FirstGroup");

            Directory.Delete(testPath, true);

            Assert.AreEqual(1, images.Count);

            ImageFile imageFile = images.Where(i => i.Name.Equals("file1.txt")).FirstOrDefault();

            Assert.IsNotNull(imageFile);
            Assert.AreEqual("/images/FirstGroup/file1.txt", imageFile.ImageUri.ToString());
            Assert.IsFalse(imageFile.ImageUri.IsAbsoluteUri);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidateUri_WithoutGroupName_Success()
        {
            string testPath = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            string newGroupPath = testPath;

            if (!Directory.Exists(newGroupPath))
                Directory.CreateDirectory(newGroupPath);

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");

            List<ImageFile> images = sut.Images("");

            Directory.Delete(testPath, true);

            Assert.AreEqual(1, images.Count);

            ImageFile imageFile = images.Where(i => i.Name.Equals("file1.txt")).FirstOrDefault();

            Assert.IsNotNull(imageFile);
            Assert.AreEqual("/images/file1.txt", imageFile.ImageUri.ToString());
            Assert.IsFalse(imageFile.ImageUri.IsAbsoluteUri);
        }

        private DefaultImageProvider CreateDefaultImageProvider(string imagePath = "")
        {
            if (imagePath == null)
                imagePath = String.Empty;

            if (!String.IsNullOrEmpty(imagePath))
                imagePath = imagePath.Replace("\\", "\\\\");

            TestSettingsProvider testSettingsProvider = new TestSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + imagePath + "\"}}");

            return new DefaultImageProvider(new TestHostEnvironment(), testSettingsProvider);
        }
    }
}
