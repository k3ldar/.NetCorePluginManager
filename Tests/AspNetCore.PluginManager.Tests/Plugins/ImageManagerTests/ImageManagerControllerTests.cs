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

using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests.Mocks;
using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Controllers;
using ImageManager.Plugin.Models;

using MemoryCache.Plugin;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageManagerControllerTests : BaseControllerTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestInitialize]
        public void InitialiseImageManagerPlugin()
        {
            InitializeImageManagerPluginManager();
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidSettingsProvider_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(null, new MockImageProvider());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidImageProvider_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new TestSettingsProvider("{}"), null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_Success()
        {
            ImageManagerController sut = new ImageManagerController(new TestSettingsProvider("{}"), new MockImageProvider());

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Validate_ControllerName_Success()
        {
            Assert.AreEqual("ImageManager", ImageManagerController.Name);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Validate_ControllerHasCorrectAttributes_Success()
        {
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(ImageManagerController)));
            Assert.IsTrue(ClassHasAttribute<LoggedInAttribute>(typeof(ImageManagerController)));
            Assert.IsTrue(ClassHasAttribute<AuthorizeAttribute>(typeof(ImageManagerController)));

            Assert.IsTrue(ClassAuthorizeAttributeHasCorrectPolicy(typeof(ImageManagerController), "ImageManager"));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Index_HasCorrectAttributes_Success()
        {
            string methodName = "Index";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "AppImageManagement"));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ImageManagerController), methodName));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Index_ReturnsCorrectModel_Success()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.Index();

            // Assert
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual(null, viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.AreEqual("", imagesViewModel.SelectedGroupName);
            Assert.AreEqual(2, imagesViewModel.Groups.Count);
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 1"));
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 2"));
            Assert.AreEqual(0, imagesViewModel.Groups["Group 1"].Count);
            Assert.AreEqual(0, imagesViewModel.Groups["Group 2"].Count);
            Assert.AreEqual(1, imagesViewModel.ImageFiles.Count);
            Assert.AreEqual("myfile.gif", imagesViewModel.ImageFiles[0].Name);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewGroup_HasCorrectAttributes_Success()
        {
            string methodName = "ViewGroup";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewGroup", "Index", true));
            Assert.IsTrue(MethodRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewGroup/{groupName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewGroup_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewGroup(null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewGroup_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewGroup("");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewGroup_InvalidGroupName_DoesNotExist_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewGroup("Image Group");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewGroup_ValidGroupName_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewGroup("Group 1");

            // Assert
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual("/Views/ImageManager/Index.cshtml", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.AreEqual("Group 1", imagesViewModel.SelectedGroupName);
            Assert.AreEqual(2, imagesViewModel.Groups.Count);
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 1"));
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 2"));
            Assert.AreEqual(2, imagesViewModel.Groups["Group 1"].Count);
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 1"));
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 2"));
            Assert.AreEqual(1, imagesViewModel.ImageFiles.Count);
            Assert.AreEqual("myfile.gif", imagesViewModel.ImageFiles[0].Name);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_HasCorrectAttributes_Success()
        {
            string methodName = "ViewSubgroup";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewSubgroup", "ViewGroup", true));
            Assert.IsTrue(MethodRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewSubgroup/{groupName}/{subgroupName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroup(null, "subgroup");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroup("", "subgroup");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_InvalidParam_SubgroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_InvalidParam_SubgroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", "");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_SubgroupName_DoesNotExist_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", "Subgroup 1");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_InvalidGroupName_DoesNotExist_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroup("Image Group", "subgroup");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewSubgroup_ValidGroupName_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", "Group 1 Subgroup 1");

            // Assert
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual("/Views/ImageManager/Index.cshtml", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.AreEqual("Group 1", imagesViewModel.SelectedGroupName);
            Assert.AreEqual(2, imagesViewModel.Groups.Count);
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 1"));
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 2"));
            Assert.AreEqual(2, imagesViewModel.Groups["Group 1"].Count);
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 1"));
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 2"));
            Assert.AreEqual(1, imagesViewModel.ImageFiles.Count);
            Assert.AreEqual("myfile.gif", imagesViewModel.ImageFiles[0].Name);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_HasCorrectAttributes_Success()
        {
            string methodName = "ViewImage";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewImage", "ViewGroup", true));
            Assert.IsTrue(MethodRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewImage/{groupName}/{imageName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewImage(null, "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewImage("", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_InvalidParam_ImageName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewImage("Image Group", null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_InvalidParam_ImageName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewImage("Image Group", "");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_ValidImage_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewImage("Group 1", "myfile.gif");

            // Assert
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.IsNull(viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.AreEqual("Group 1", imagesViewModel.SelectedGroupName);
            Assert.AreEqual(2, imagesViewModel.Groups.Count);
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 1"));
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 2"));
            Assert.AreEqual(2, imagesViewModel.Groups["Group 1"].Count);
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 1"));
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 2"));
            Assert.AreEqual(1, imagesViewModel.ImageFiles.Count);
            Assert.AreEqual("myfile.gif", imagesViewModel.ImageFiles[0].Name);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_HasCorrectAttributes_Success()
        {
            string methodName = "ViewSubgroupImage";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewImage", "ViewGroup", true));
            Assert.IsTrue(MethodRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewSubgroupImage/{groupName}/{subgroupName}/{imageName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroupImage(null, "subgroup", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroupImage("", "subgroup", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_InvalidParam_SubgroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", null, "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_InvalidParam_SubgroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", "", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_InvalidParam_ImageName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", "sub group", null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_InvalidParam_ImageName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateDynamicContentController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", "sub group", "");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ViewImage_WithSubgroup_ValidImage_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroupImage("Group 1", "Group 1 Subgroup 1", "myfile.gif");

            // Assert
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual("/Views/ImageManager/ViewImage.cshtml", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.AreEqual("Group 1", imagesViewModel.SelectedGroupName);
            Assert.AreEqual(2, imagesViewModel.Groups.Count);
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 1"));
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 2"));
            Assert.AreEqual(2, imagesViewModel.Groups["Group 1"].Count);
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 1"));
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 2"));
            Assert.AreEqual(1, imagesViewModel.ImageFiles.Count);
            Assert.AreEqual("myfile.gif", imagesViewModel.ImageFiles[0].Name);
        }

        #region Private Methods

        private MockImageProvider CreateDefaultMockImageProvider()
        {
            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>()
            {
                { "Group 1", new List<string>() },
                { "Group 2", new List<string>() }
            };

            groups["Group 1"].Add("Group 1 Subgroup 1");
            groups["Group 1"].Add("Group 1 Subgroup 2");

            List<ImageFile> images = new List<ImageFile>()
            {
                { new ImageFile(new Uri("/", UriKind.RelativeOrAbsolute), "myfile.gif", ".gif", 23, DateTime.Now, DateTime.Now) }
            };


            return new MockImageProvider(groups, images);
        }

        private ImageManagerController CreateDynamicContentController(DefaultMemoryCache memoryCache = null,
            List<BreadcrumbItem> breadcrumbs = null, MockImageProvider mockImageProvider = null)
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testDynamicContentPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperService = new pm.PluginServices(_testDynamicContentPlugin) as IPluginHelperService;
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            ImageManagerController Result = new ImageManagerController(
                new TestSettingsProvider("{}"),
                mockImageProvider ?? new MockImageProvider());

            Result.ControllerContext = CreateTestControllerContext(breadcrumbs);

            return Result;
        }

        #endregion Private Methods
    }
}
