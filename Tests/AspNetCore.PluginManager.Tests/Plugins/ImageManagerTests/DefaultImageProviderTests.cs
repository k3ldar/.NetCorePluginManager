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

using Shared.Abstractions;
using Shared.Classes;

using sc = Shared.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultImageProviderTests : GenericBaseClass
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";
        private const string DemoWebsiteImagePath = "..\\..\\..\\..\\..\\..\\.NetCorePluginManager\\Demo\\NetCorePluginDemoWebsite\\wwwroot\\images";

        [TestInitialize]
        public void InitializeTest()
        {
            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.ClearAllCaches();
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidSettingsProvider_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = new DefaultImageProvider(new MockHostEnvironment(), null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidIHostEnvironment_Null_Throws_ArgumentNullException()
        {
            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(null, testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_Success()
        {
            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(new MockHostEnvironment(), testSettingsProvider);

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstanceWithEmptyPath_Success()
        {
            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"\"}}");
            DefaultImageProvider sut = new DefaultImageProvider(new MockHostEnvironment(), testSettingsProvider);

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
            string testPath = TestHelper.GetTestPath();
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
            string testPath = TestHelper.GetTestPath();
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
            string testPath = TestHelper.GetTestPath();
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
            string testPath = TestHelper.GetTestPath();
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");
            sut.CreateGroup("SecondGroup");

            Directory.CreateDirectory(Path.Combine(newGroupPath, "First Sub Group"));
            Directory.CreateDirectory(Path.Combine(newGroupPath, "Second Sub Group"));
            File.WriteAllText(Path.Combine(newGroupPath, "Second Sub Group", "test.txt"), "test file");

            Dictionary<string, List<string>> groups = sut.Groups();

            Directory.Delete(testPath, true);

            Assert.IsNotNull(groups);

            Assert.AreEqual(2, groups.Count);
            Assert.IsTrue(groups.ContainsKey("FirstGroup"));
            Assert.IsTrue(groups.ContainsKey("SecondGroup"));
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
            string testPath = TestHelper.GetTestPath();
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");
            File.WriteAllText(Path.Combine(newGroupPath, "file2.txt"), "text for file 2");

            List<ImageFile> images = sut.Images("FirstGroup");

            Directory.Delete(testPath, true);

            Assert.AreEqual(2, images.Count);
            Assert.IsTrue(images.Where(i => i.Name.Equals("file1.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("file2.txt")).Any());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidateUri_Success()
        {
            string testPath = TestHelper.GetTestPath();
            string newGroupPath = Path.Combine(testPath, "FirstGroup");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            sut.CreateGroup("FirstGroup");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");

            List<ImageFile> images = sut.Images("FirstGroup");

            Directory.Delete(testPath, true);

            Assert.AreEqual(1, images.Count);

            ImageFile imageFile = images.FirstOrDefault(i => i.Name.Equals("file1.txt"));

            Assert.IsNotNull(imageFile);
            Assert.AreEqual("/images/FirstGroup/file1.txt", imageFile.ImageUri.ToString());
            Assert.IsFalse(imageFile.ImageUri.IsAbsoluteUri);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Images_ValidateUri_WithoutGroupName_Success()
        {
            string testPath = TestHelper.GetTestPath();
            string newGroupPath = testPath;

            if (!Directory.Exists(newGroupPath))
                Directory.CreateDirectory(newGroupPath);

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");

            List<ImageFile> images = sut.Images("");

            Directory.Delete(testPath, true);

            Assert.AreEqual(1, images.Count);

            ImageFile imageFile = images.FirstOrDefault(i => i.Name.Equals("file1.txt"));

            Assert.IsNotNull(imageFile);
            Assert.AreEqual("/images/file1.txt", imageFile.ImageUri.ToString());
            Assert.IsFalse(imageFile.ImageUri.IsAbsoluteUri);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidGroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images(null, "valid string");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidGroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("", "valid string");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidSubgroupName_EmptyString_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("valid string", "");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubgroupImages_InvalidSubgroupName_Null_Throws_ArgumentNullException()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("valid string", null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void SubgroupImages_InvalidSubgroupName_NotFound_ReturnsEmptyList()
        {
            DefaultImageProvider sut = CreateDefaultImageProvider();

            List<ImageFile> images = sut.Images("valid string", "my subgroup");
            Assert.IsNotNull(images);
            Assert.AreEqual(0, images.Count);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Groups_ValidSubgroupName_ReturnsListOfImagesAndSubGroupNames_Success()
        {
            string testPath = TestHelper.GetTestPath();
            string newGroupPath = Path.Combine(testPath, "FirstGroup");
            string subGroup1 = Path.Combine(newGroupPath, "SubGroup 1");
            string subGroup2 = Path.Combine(newGroupPath, "SubGroup 2");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            Assert.IsFalse(sut.GroupExists("FirstGroup"));

            bool addGroup = sut.CreateGroup("FirstGroup");
            Assert.IsTrue(addGroup);
            Assert.IsTrue(sut.GroupExists("FirstGroup"));

            sut.AddSubgroup("FirstGroup", "SubGroup 1");
            Assert.IsTrue(sut.SubgroupExists("FirstGroup", "SubGroup 1"));

            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file1.txt"), "text for file 1 in sub group 1");
            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file2.txt"), "text for file 2 in sub group 1");

            bool addSubGroup = sut.AddSubgroup("FirstGroup", "SubGroup 2");
            Assert.IsTrue(addSubGroup);
            Assert.IsTrue(sut.SubgroupExists("FirstGroup", "SubGroup 2"));
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file1.txt"), "text for file 1 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file2.txt"), "text for file 2 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file3.txt"), "text for file 3 in sub group 2");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");
            File.WriteAllText(Path.Combine(newGroupPath, "file2.txt"), "text for file 2");

            addSubGroup = sut.AddSubgroup("FirstGroup", "SubGroup 2");
            Assert.IsFalse(addSubGroup);

            List<ImageFile> images = sut.Images("FirstGroup", "Subgroup 2");

            Assert.AreEqual(3, images.Count);
            Assert.IsTrue(images.Where(i => i.Name.Equals("subgroup 2 file1.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("subgroup 2 file2.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("subgroup 2 file3.txt")).Any());

            Directory.Delete(testPath, true);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Groups_ValidGroupName_WithSubGroups_ReturnsListOfImagesAndSubGroupNames_Success()
        {
            string testPath = TestHelper.GetTestPath();
            string newGroupPath = Path.Combine(testPath, "FirstGroup");
            string subGroup1 = Path.Combine(newGroupPath, "SubGroup 1");
            string subGroup2 = Path.Combine(newGroupPath, "SubGroup 2");

            DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
            Assert.IsFalse(sut.GroupExists("FirstGroup"));

            bool addGroup = sut.CreateGroup("FirstGroup");
            Assert.IsTrue(addGroup);
            Assert.IsTrue(sut.GroupExists("FirstGroup"));

            sut.AddSubgroup("FirstGroup", "SubGroup 1");
            Assert.IsTrue(sut.SubgroupExists("FirstGroup", "SubGroup 1"));

            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file1.txt"), "text for file 1 in sub group 1");
            File.WriteAllText(Path.Combine(subGroup1, "subgroup 1 file2.txt"), "text for file 2 in sub group 1");

            bool addSubGroup = sut.AddSubgroup("FirstGroup", "SubGroup 2");
            Assert.IsTrue(addSubGroup);
            Assert.IsTrue(sut.SubgroupExists("FirstGroup", "SubGroup 2"));
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file1.txt"), "text for file 1 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file2.txt"), "text for file 2 in sub group 2");
            File.WriteAllText(Path.Combine(subGroup2, "subgroup 2 file3.txt"), "text for file 3 in sub group 2");

            File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1");
            File.WriteAllText(Path.Combine(newGroupPath, "file2.txt"), "text for file 2");

            addSubGroup = sut.AddSubgroup("FirstGroup", "SubGroup 2");
            Assert.IsFalse(addSubGroup);

            List<ImageFile> images = sut.Images("FirstGroup");

            Assert.AreEqual(2, images.Count);
            Assert.IsTrue(images.Where(i => i.Name.Equals("file1.txt")).Any());
            Assert.IsTrue(images.Where(i => i.Name.Equals("file2.txt")).Any());

            Dictionary<string, List<string>> group = sut.Groups();

            Assert.AreEqual(1, group.Count);
            Assert.AreEqual(2, group["FirstGroup"].Count);
            Assert.AreEqual("SubGroup 1", group["FirstGroup"][0]);
            Assert.AreEqual("SubGroup 2", group["FirstGroup"][1]);

            bool subGroupDeleted = sut.DeleteSubgroup("FirstGroup", "SubGroup 1");
            Assert.IsTrue(subGroupDeleted);
            Assert.IsFalse(sut.SubgroupExists("FirstGroup", "SubGroup 1"));

            subGroupDeleted = sut.DeleteSubgroup("FirstGroup", "SubGroup 2");
            Assert.IsTrue(subGroupDeleted);
            Assert.IsFalse(sut.SubgroupExists("FirstGroup", "SubGroup 2"));

            subGroupDeleted = sut.DeleteSubgroup("FirstGroup", "SubGroup 2");
            Assert.IsFalse(subGroupDeleted);

            bool groupDeleted = sut.DeleteGroup("FirstGroup");
            Assert.IsTrue(groupDeleted);

            groupDeleted = sut.DeleteGroup("FirstGroup");
            Assert.IsFalse(groupDeleted);

            Directory.Delete(testPath, true);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupExists_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.GroupExists(null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GroupExists_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.GroupExists("");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubgroupExists(null, "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubgroupExists("", "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamSubroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubgroupExists("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SubGroupExists_InvalidParamSubroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.SubgroupExists("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubgroup(null, "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubgroup("", "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamSubroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubgroup("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void DeleteSubGroup_InvalidParamSubroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.DeleteSubgroup("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubgroup(null, "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubgroup("", "subgroup name");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamSubgroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubgroup("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddSubGroup_InvalidParamSubgroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.AddSubgroup("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_InvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists(null, "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_InvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_InvalidParamImageName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_InvalidParamImageName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ImageExists_FileNotFound_ReturnsFalse()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "FirstGroup");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                Assert.IsFalse(sut.GroupExists("FirstGroup"));

                bool addGroup = sut.CreateGroup("FirstGroup");
                Assert.IsTrue(addGroup);
                Assert.IsTrue(sut.GroupExists("FirstGroup"));

                File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1 in sub group 1");

                sut.ImageExists("group", "file2.txt");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_WithSubgroupInvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists(null, "subgroup", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_WithSubgroupInvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("", "subgroup", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_WithSubgroupInvalidParamSubgroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("group", null, "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_WithSubgroupInvalidParamSubgroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("group", "", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_WithSubgroupInvalidParamImageName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("group", "subgroup", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageExists_WithSubgroupInvalidParamImageName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageExists("group", "subgroup", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithoutSubgroupInvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete(null, "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithoutSubgroupInvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithoutSubgroupInvalidParamImageName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("group", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithoutSubgroupInvalidParamImageName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("group", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ImageDelete_FileNotFound_ReturnsFalse()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "FirstGroup");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                Assert.IsFalse(sut.GroupExists("FirstGroup"));

                bool addGroup = sut.CreateGroup("FirstGroup");
                Assert.IsTrue(addGroup);
                Assert.IsTrue(sut.GroupExists("FirstGroup"));

                File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1 in group 1");

                Assert.IsTrue(sut.ImageExists("FirstGroup", "file1.txt"));
                bool imageDeleted = sut.ImageDelete("FirstGroup", "file2.txt");
                Assert.IsFalse(imageDeleted);
                Assert.IsTrue(sut.ImageExists("FirstGroup", "file1.txt"));
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ImageDelete_FileFound_ReturnsTrue()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "FirstGroup");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                Assert.IsFalse(sut.GroupExists("FirstGroup"));

                bool addGroup = sut.CreateGroup("FirstGroup");
                Assert.IsTrue(addGroup);
                Assert.IsTrue(sut.GroupExists("FirstGroup"));

                File.WriteAllText(Path.Combine(newGroupPath, "file1.txt"), "text for file 1 in group 1");

                Assert.IsTrue(sut.ImageExists("FirstGroup", "file1.txt"));
                bool imageDeleted = sut.ImageDelete("FirstGroup", "file1.txt");
                Assert.IsTrue(imageDeleted);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithSubgroupInvalidParamGroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete(null, "subgroup", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithSubgroupInvalidParamGroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("", "subgroup", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithSubgroupInvalidParamSubgroupName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("group", null, "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithSubgroupInvalidParamSubgroupName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("group", "", "image.jpeg");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithSubgroupInvalidParamImageName_Null_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("group", "subgroup", null);
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImageDelete_WithSubgroupInvalidParamImageName_EmptyString_Throws_ArgumentNullException()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.ImageDelete("group", "subgroup", "");
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ImageDelete_WithSubgroup_FileNotFound_ReturnsFalse()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "FirstGroup");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                Assert.IsFalse(sut.GroupExists("FirstGroup"));

                bool addGroup = sut.CreateGroup("FirstGroup");
                Assert.IsTrue(addGroup);
                Assert.IsTrue(sut.GroupExists("FirstGroup"));

                Assert.IsFalse(sut.ImageExists("FirstGroup", "subgroup", "file2.txt"));
                bool imageDeleted = sut.ImageDelete("FirstGroup", "subgroup", "file2.txt");
                Assert.IsFalse(imageDeleted);
                Assert.IsFalse(sut.ImageExists("FirstGroup", "subgroup", "file2.txt"));
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ImageDelete_WithSubgroup_FileFound_ReturnsTrue()
        {
            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "FirstGroup");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                Assert.IsTrue(CacheManagerExists("DefaultImageProvider"));
                Assert.IsFalse(sut.GroupExists("FirstGroup"));

                bool addGroup = sut.CreateGroup("FirstGroup");
                Assert.IsTrue(addGroup);
                Assert.IsTrue(sut.GroupExists("FirstGroup"));

                bool addSubgroup = sut.AddSubgroup("FirstGroup", "subgroup");
                Assert.IsTrue(addSubgroup);
                Assert.IsTrue(sut.SubgroupExists("FirstGroup", "subgroup"));

                File.WriteAllText(Path.Combine(newGroupPath, "subgroup", "file1.txt"), "text for file 1 in sub group 1");

                Assert.IsTrue(sut.ImageExists("FirstGroup", "subgroup", "file1.txt"));
                bool imageDeleted = sut.ImageDelete("FirstGroup", "subgroup", "file1.txt");
                Assert.IsTrue(imageDeleted);
                Assert.IsFalse(sut.ImageExists("FirstGroup", "subgroup", "file1.txt"));
            }
            catch
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath);

                throw;
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TemporaryImageFile_InvalidParam_Null_Throws_ArgumentNullException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.TemporaryImageFile(null);
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TemporaryImageFile_InvalidParam_EmptyString_Throws_ArgumentNullException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.TemporaryImageFile("");
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TemporaryImageFile_InvalidParam_DoesNotStartWithFullStop_Throws_ArgumentException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);
                sut.TemporaryImageFile("gif");
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void TemporaryImageFile_RetrieveTemporaryImageFileName_Success()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                string tempFile = sut.TemporaryImageFile(".GiF");

                Assert.IsTrue(File.Exists(tempFile));
                FileInfo fileInfo = new FileInfo(tempFile);
                Assert.IsNotNull(fileInfo);
                Assert.AreEqual(0, fileInfo.Length);
                Assert.IsTrue(fileInfo.CreationTimeUtc >= fileCreated);
                Assert.AreEqual(".gif", Path.GetExtension(fileInfo.Name));
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFile_InvalidParam_GroupName_Null_Throws_ArgumentNullException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile(null, null, "filename.dat", new byte[] { 0, 1, 2 });
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFile_InvalidParam_GroupName_EmptyString_Throws_ArgumentNullException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile("", null, "filename.dat", new byte[] { 0, 1, 2 });
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFile_InvalidParam_FileName_Null_Throws_ArgumentNullException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile("Group", null, null, new byte[] { 0, 1, 2 });
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddFile_InvalidParam_FileName_EmptyString_Throws_ArgumentNullException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile("Group", null, "", new byte[] { 0, 1, 2 });
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddFile_InvalidParam_FileContents_ZeroLength_Throws_ArgumentException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string newGroupPath = Path.Combine(testPath, "TempImages");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile("Group", null, "filename.dat", new byte[] { });
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddFile_InvalidParam_FileName_AlreadyExists_Throws_ArgumentOutOfRangeException()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string directoryName = Path.Combine(testPath, "Group");

                if (!Directory.Exists(directoryName))
                    Directory.CreateDirectory(directoryName);

                string fileName = Path.Combine(directoryName, "filename.dat");

                File.WriteAllBytes(fileName, new byte[] { 2, 3, 4 });

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile("Group", null, "filename.dat", new byte[] { 10, 9, 8 });
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void AddFile_WithoutSubgroup_Success()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string directoryName = Path.Combine(testPath, "Group");

                string fileName = Path.Combine(directoryName, "filename.dat");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile("Group", null, "filename.dat", new byte[] { 10, 9, 8 });

                Assert.IsTrue(File.Exists(fileName));
                Assert.IsTrue(File.ReadAllBytes(fileName).SequenceEqual(new byte[] { 10, 9, 8 }));
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void AddFile_WithSubgroup_Success()
        {
            DateTime fileCreated = DateTime.UtcNow;

            string testPath = TestHelper.GetTestPath();
            try
            {
                string directoryName = Path.Combine(testPath, "Group", "Subgroup");

                string fileName = Path.Combine(directoryName, "filename.dat");

                DefaultImageProvider sut = CreateDefaultImageProvider(testPath);

                sut.AddFile("Group", "Subgroup", "filename.dat", new byte[] { 11, 10, 9 });

                Assert.IsTrue(File.Exists(fileName));
                Assert.IsTrue(File.ReadAllBytes(fileName).SequenceEqual(new byte[] { 11, 10, 9 }));
            }
            finally
            {
                if (Directory.Exists(testPath))
                    Directory.Delete(testPath, true);
            }
        }

        private DefaultImageProvider CreateDefaultImageProvider(string imagePath = "")
        {
            if (imagePath == null)
                imagePath = String.Empty;

            if (!String.IsNullOrEmpty(imagePath))
                imagePath = imagePath.Replace("\\", "\\\\");

            MockSettingsProvider testSettingsProvider = new MockSettingsProvider("{\"ImageManager\": {\"ImagePath\": \"" + imagePath + "\"}}");

            return new DefaultImageProvider(new MockHostEnvironment(), testSettingsProvider);
        }
    }
}
