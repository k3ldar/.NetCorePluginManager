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
using System.Security.Claims;
using System.Text;

using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Controllers;
using ImageManager.Plugin.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

using PluginManager.Abstractions;

using Shared.Abstractions;
using Shared.Classes;

using SharedPluginFeatures;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageManagerControllerTests : BaseControllerTests
    {
        private const string TestCategoryName = "Image Manager Tests";
        private const string ExpectedResponseWithNoListener = "{\"GroupName\":\"Group\",\"SubgroupName\":\"test subgrouP\",\"ShowSubgroup\":true,\"AdditionalDataName\":null,\"AdditionalData\":null,\"AdditionalDataMandatory\":false}";
        private const string ExpectedResponseWithListener = "{\"GroupName\":\"Images\",\"SubgroupName\":null,\"ShowSubgroup\":false,\"AdditionalDataName\":\"Enter product code/sku\",\"AdditionalData\":null,\"AdditionalDataMandatory\":true}";

        [TestInitialize]
        public void InitialiseImageManagerPlugin()
        {
            InitializeImageManagerPluginManager();
        }

        [TestCleanup]
        public void FinalizeTest()
        {
            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.RemoveCache(Constants.CacheManagerImageManager);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidSettingsProvider_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(null, new MockImageProvider(), new MockNotificationService(), new MockMemoryCache(), new MockVirusScanner());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidImageProvider_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new MockSettingsProvider(), null, new MockNotificationService(), new MockMemoryCache(), new MockVirusScanner());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidNotificationService_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new MockSettingsProvider(), new MockImageProvider(), null, new MockMemoryCache(), new MockVirusScanner());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidMemoryCache_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new MockSettingsProvider(), new MockImageProvider(), new MockNotificationService(), null, new MockVirusScanner());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidVirusScanner_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new MockSettingsProvider(), new MockImageProvider(), new MockNotificationService(), new MockMemoryCache(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ImageManagerController sut = new ImageManagerController(new MockSettingsProvider(), new MockImageProvider(), new MockNotificationService(), new MockMemoryCache(), new MockVirusScanner());

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_ControllerName_Success()
        {
            Assert.AreEqual("ImageManager", ImageManagerController.Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_ControllerHasCorrectAttributes_Success()
        {
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(ImageManagerController)));
            Assert.IsTrue(ClassHasAttribute<LoggedInAttribute>(typeof(ImageManagerController)));
            Assert.IsTrue(ClassHasAttribute<AuthorizeAttribute>(typeof(ImageManagerController)));

            Assert.IsTrue(ClassAuthorizeAttributeHasCorrectPolicy(typeof(ImageManagerController), "ImageManager"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_ControllerViewsAreIncludedAsResources_Success()
        {
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "_LeftMenu"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "ImageUpload"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "Index"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "ViewImage"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "_ImageUpload"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_HasCorrectAttributes_Success()
        {
            string methodName = "Index";
            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ImageManagerController), methodName));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ReturnsCorrectModel_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.Index();

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
            Assert.AreEqual(1, imagesViewModel.Breadcrumbs.Count);
            Assert.AreEqual("Image Manager", imagesViewModel.Breadcrumbs[0].Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ReturnsCorrectModel_WithUserThatCanManageImages_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

            List<Claim> webClaims = new List<Claim>();
            webClaims.Add(new Claim(Constants.ClaimNameManageImages, "true"));

            claimsIdentities.Add(new ClaimsIdentity(webClaims, Constants.ClaimIdentityWebsite));

            sut.ControllerContext.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(claimsIdentities);

            IActionResult response = sut.Index();

            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual(null, viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.IsTrue(imagesViewModel.CanManageImages);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewGroup_HasCorrectAttributes_Success()
        {
            string methodName = "ViewGroup";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewGroup", "Index", true));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewGroup/{groupName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewGroup_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewGroup(null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewGroup_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewGroup("");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewGroup_InvalidGroupName_DoesNotExist_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewGroup("Image Group");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewGroup_ValidGroupName_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewGroup("Group 1");

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
            Assert.AreEqual(2, imagesViewModel.Breadcrumbs.Count);
            Assert.AreEqual("Image Manager", imagesViewModel.Breadcrumbs[0].Name);
            Assert.AreEqual("Group 1", imagesViewModel.Breadcrumbs[1].Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_HasCorrectAttributes_Success()
        {
            string methodName = "ViewSubgroup";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewSubgroup", "ViewGroup", true));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewSubgroup/{groupName}/{subgroupName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroup(null, "subgroup");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroup("", "subgroup");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_InvalidParam_SubgroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_InvalidParam_SubgroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", "");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_SubgroupName_DoesNotExist_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", "Subgroup 1");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_InvalidGroupName_DoesNotExist_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroup("Image Group", "subgroup");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewSubgroup_ValidGroupName_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroup("Group 1", "Group 1 Subgroup 1");

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
            Assert.AreEqual(3, imagesViewModel.Breadcrumbs.Count);
            Assert.AreEqual("Image Manager", imagesViewModel.Breadcrumbs[0].Name);
            Assert.AreEqual("Group 1", imagesViewModel.Breadcrumbs[1].Name);
            Assert.AreEqual("Group 1 Subgroup 1", imagesViewModel.Breadcrumbs[2].Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_HasCorrectAttributes_Success()
        {
            string methodName = "ViewImage";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewImage", "ViewGroup", true));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewImage/{groupName}/{imageName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewImage(null, "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewImage("", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_InvalidParam_ImageName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewImage("Image Group", null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_InvalidParam_ImageName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewImage("Image Group", "");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_ValidImage_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewImage("Group 1", "myfile-gif");

            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.IsNull(viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.IsNotNull(imagesViewModel.SelectedImageFile);
            Assert.AreEqual("Group 1", imagesViewModel.SelectedGroupName);
            Assert.AreEqual(2, imagesViewModel.Groups.Count);
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 1"));
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 2"));
            Assert.AreEqual(2, imagesViewModel.Groups["Group 1"].Count);
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 1"));
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 2"));
            Assert.AreEqual(1, imagesViewModel.ImageFiles.Count);
            Assert.AreEqual("myfile.gif", imagesViewModel.ImageFiles[0].Name);
            Assert.AreEqual(3, imagesViewModel.Breadcrumbs.Count);
            Assert.AreEqual("Image Manager", imagesViewModel.Breadcrumbs[0].Name);
            Assert.AreEqual("Group 1", imagesViewModel.Breadcrumbs[1].Name);
            Assert.AreEqual("myfile.gif", imagesViewModel.Breadcrumbs[2].Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_HasCorrectAttributes_Success()
        {
            string methodName = "ViewSubgroupImage";
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(ImageManagerController), methodName, "ViewImage", "ViewGroup", true));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ImageManagerController), methodName, "ImageManager/ViewSubgroupImage/{groupName}/{subgroupName}/{imageName}"));

            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_InvalidParam_GroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroupImage(null, "subgroup", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_InvalidParam_GroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroupImage("", "subgroup", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_InvalidParam_SubgroupName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", null, "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_InvalidParam_SubgroupName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", "", "image name");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_InvalidParam_ImageName_Null_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", "sub group", null);

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_InvalidParam_ImageName_EmptyString_RedirectsToIndex()
        {
            ImageManagerController sut = CreateImageManagerController();

            IActionResult response = sut.ViewSubgroupImage("Image Group", "sub group", "");

            Assert.IsInstanceOfType(response, typeof(RedirectToActionResult));

            RedirectToActionResult viewResult = response as RedirectToActionResult;

            Assert.IsFalse(viewResult.Permanent);
            Assert.AreEqual("Index", viewResult.ActionName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewImage_WithSubgroup_ValidImage_ReturnsCorrectModelAndView_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());

            IActionResult response = sut.ViewSubgroupImage("Group 1", "Group 1 Subgroup 1", "myfile-gif");

            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;

            Assert.IsNotNull(viewResult.Model);

            ValidateBaseModel(viewResult);

            Assert.AreEqual("/Views/ImageManager/ViewImage.cshtml", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(ImagesViewModel));

            ImagesViewModel imagesViewModel = (ImagesViewModel)viewResult.Model;

            Assert.IsNotNull(imagesViewModel.SelectedImageFile);
            Assert.AreEqual("Group 1", imagesViewModel.SelectedGroupName);
            Assert.AreEqual(2, imagesViewModel.Groups.Count);
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 1"));
            Assert.IsTrue(imagesViewModel.Groups.ContainsKey("Group 2"));
            Assert.AreEqual(2, imagesViewModel.Groups["Group 1"].Count);
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 1"));
            Assert.IsTrue(imagesViewModel.Groups["Group 1"].Contains("Group 1 Subgroup 2"));
            Assert.AreEqual(1, imagesViewModel.ImageFiles.Count);
            Assert.AreEqual("myfile.gif", imagesViewModel.ImageFiles[0].Name);
            Assert.AreEqual(4, imagesViewModel.Breadcrumbs.Count);
            Assert.AreEqual("Image Manager", imagesViewModel.Breadcrumbs[0].Name);
            Assert.AreEqual("Group 1", imagesViewModel.Breadcrumbs[1].Name);
            Assert.AreEqual("Group 1 Subgroup 1", imagesViewModel.Breadcrumbs[2].Name);
            Assert.AreEqual("myfile.gif", imagesViewModel.Breadcrumbs[3].Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_HasCorrectAttributes_Success()
        {
            string methodName = "DeleteImage";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(ImageManagerController), methodName, "ImageManagerManage"));

            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidParamModel_Null_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.DeleteImage(null);

            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_ConfirmDeleteFalse_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.DeleteImage(new DeleteImageModel());

            ValidateJsonResult(response, "Confirmation required");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_ImageName_Null_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid ImageName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_ImageName_EmptyString_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                ImageName = ""
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid ImageName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_GroupName_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = null,
                ImageName = "invalidimage.jpeg"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_GroupName_EmptyString_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = "",
                ImageName = "invalidimage.jpeg"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_GroupName_DoesNotExist_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = "FictionalGroup",
                ImageName = "invalidimage.jpeg"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_ImageNotFound_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = "Group 1",
                ImageName = "invalidimage.jpeg"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid ImageName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_InvalidModel_SubgroupNoGroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                SubgroupName = "Group 1",
                ImageName = "invalidimage.jpeg"
            };

            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_ValidModel_WithoutSubgroup_ReturnsCorrectValidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(null, null, mockImageProvider);
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = "Group 1",
                ImageName = "myfile.gif"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);
            Assert.IsTrue(mockImageProvider.DeletedImageList.Contains("myfile.gif"));
            ValidateJsonResult(response, "", 200, "application/json", true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_ValidModel_WithNonExistantSubgroup_ReturnsCorrectInvalidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(null, null, mockImageProvider);
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = "Group 1",
                SubgroupName = "Sub group",
                ImageName = "myfile.gif"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);
            Assert.IsFalse(mockImageProvider.DeletedImageList.Contains("myfile.gif"));
            ValidateJsonResult(response, "Invalid SubgroupName", 400);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_ValidModel_WithSubgroup_ReturnsCorrectValidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(null, null, mockImageProvider);
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = "Group 1",
                SubgroupName = "Group 1 Subgroup 1",
                ImageName = "myfile.gif"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);
            Assert.IsTrue(mockImageProvider.DeletedImageList.Contains("myfile.gif"));
            ValidateJsonResult(response, "", 200, "application/json", true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteImage_ValidModel_WithoutSubgroup_ImageNotDeleted_ReturnsCorrectInvalidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            mockImageProvider.CanDeleteImages = false;

            ImageManagerController sut = CreateImageManagerController(null, null, mockImageProvider);
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                GroupName = "Group 1",
                ImageName = "myfile-gif"
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);
            Assert.IsFalse(mockImageProvider.DeletedImageList.Contains("myfile.gif"));
            ValidateJsonResult(response, "Unable to delete image", 400);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_HasCorrectAttributes_Success()
        {
            string methodName = "UploadImage";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(ImageManagerController), methodName, "ImageManagerManage"));

            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<ValidateAntiForgeryTokenAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_InvalidFileList_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.UploadImage(null);

            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_InvalidFileList_Empty_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.UploadImage(new UploadImageModel(GenerateTestBaseModelData(), "My Group", null));

            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_InvalidGroupName_Null_ReturnsCorrectInvalidResponse()
        {
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData());

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.dat");
            model.Files.Add(formFile);
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.UploadImage(model);

            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_SingleFileUploaded_ReturnsCorrectValidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider);
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", null);

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.jpeg");
            model.Files.Add(formFile);


            IActionResult response = sut.UploadImage(model);
            try
            {
                ValidateUploadImageResponse(response, memoryCache, "My Pictures", null, 1);

            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_FileExtensionNotSupported_ReturnsCorrectResponseWithModelStateError()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider);
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", null);

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.dat");
            model.Files.Add(formFile);


            IActionResult response = sut.UploadImage(model);
            ValidateUploadImageResponse(response, memoryCache, "My Pictures", null, 0, 1);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_SingleFileUploaded_WithSubgroup_ReturnsCorrectValidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider);
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "Subgroup 1");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.svg");
            model.Files.Add(formFile);


            IActionResult response = sut.UploadImage(model);
            try
            {
                ValidateUploadImageResponse(response, memoryCache, "My Pictures", "Subgroup 1", 1);
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void UploadImage_MultipleFileUploaded_WithSubgroup_ReturnsCorrectValidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider);
            const string File1Data = "test file data for file 1";
            const string File2Data = "test file data for file 2";
            byte[] fileContents1 = Encoding.UTF8.GetBytes(File1Data);
            byte[] fileContents2 = Encoding.UTF8.GetBytes(File2Data);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "Subgroup 1");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents1), 0, fileContents1.Length, "test", "testdata1.gif");
            model.Files.Add(formFile);

            formFile = new FormFile(new MemoryStream(fileContents2), 0, fileContents2.Length, "test", "testdata2.gif");
            model.Files.Add(formFile);

            IActionResult response = sut.UploadImage(model);
            try
            {
                ValidateUploadImageResponse(response, memoryCache, "My Pictures", "Subgroup 1", 2);
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_HasCorrectAttributes_Success()
        {
            string methodName = "ProcessImage";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsTrue(MethodHasAuthorizeAttribute(typeof(ImageManagerController), methodName, "ImageManagerManage"));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ImageManagerController), methodName));

            Assert.IsFalse(MethodHasAttribute<ValidateAntiForgeryTokenAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_Construct_InvalidParamModel_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImage(null);
            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_Construct_InvalidParamModel_NonExistentCacheItem_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImage(new ImageProcessViewModel("not found"));
            ValidateJsonResult(response, "Image cache not found");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_Construct_InvalidParamModelFileUploadId_NonExistentCacheItem_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, CreateDefaultMockImageProvider());
            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImage(new ImageProcessViewModel());
            ValidateJsonResult(response, "Image cache not found");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_Construct_InvalidParamModel_CachedItemNotValidClass_ReturnsCorrectInvalidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            memoryCache.GetCache().Add("found item", new CacheItem("found item", 123));
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, CreateDefaultMockImageProvider());

            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImage(new ImageProcessViewModel("found item"));
            ValidateJsonResult(response, "Image cache not found");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_SingleFileUploaded_WithSubgroup_NotificationServiceCalled_ReturnsCorrectValidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockNotificationService notificationService = new MockNotificationService(null)
            {
                EventParam1Name = "ImageUploadedEvent"
            };

            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider, notificationService);
            const string File1Data = "test file data for file 1";
            byte[] fileContents1 = Encoding.UTF8.GetBytes(File1Data);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "Subgroup 1");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents1), 0, fileContents1.Length, "test", "testdata1.gif");
            model.Files.Add(formFile);

            IActionResult response = sut.UploadImage(model);
            try
            {
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response, memoryCache, "My Pictures", "Subgroup 1", 1);
                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));

                CachedImageUpload cachedImageUpload = memoryCache.GetCache().Get(responseModel.MemoryCacheName).Value as CachedImageUpload;
                Assert.IsNotNull(cachedImageUpload);

                ImageProcessViewModel imageProcessViewModel = new ImageProcessViewModel(responseModel.MemoryCacheName);
                IActionResult processResponse = sut.ProcessImage(imageProcessViewModel);

                Assert.AreEqual(1, mockImageProvider.FilesAdded.Count);
                Assert.IsTrue(notificationService.NotificationRaised("ImageUploadedEvent", cachedImageUpload));
                Assert.AreEqual(cachedImageUpload, notificationService.EventParam1);
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_MultipleFileUploaded_WithSubgroup_NotificationServiceCalled_ReturnsCorrectValidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockNotificationService notificationService = new MockNotificationService(null)
            {
                EventParam1Name = "ImageUploadedEvent"
            };

            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider, notificationService);
            const string File1Data = "test file data for file 1";
            const string File2Data = "test file data for file 2";
            byte[] fileContents1 = Encoding.UTF8.GetBytes(File1Data);
            byte[] fileContents2 = Encoding.UTF8.GetBytes(File2Data);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "Subgroup 1");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents1), 0, fileContents1.Length, "test", "testdata1.gif");
            model.Files.Add(formFile);
            formFile = new FormFile(new MemoryStream(fileContents2), 0, fileContents2.Length, "test", "testdata2.svg");
            model.Files.Add(formFile);

            IActionResult response = sut.UploadImage(model);

            ImagesUploadedModel responseModel = ValidateUploadImageResponse(response, memoryCache, "My Pictures", "Subgroup 1", 2);

            Assert.AreEqual("My Pictures", responseModel.GroupName);
            Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
            Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));

            CachedImageUpload cachedImageUpload = memoryCache.GetCache().Get(responseModel.MemoryCacheName).Value as CachedImageUpload;
            Assert.IsNotNull(cachedImageUpload);
            ImageProcessViewModel imageProcessViewModel = new ImageProcessViewModel(responseModel.MemoryCacheName);



            IActionResult processResponse = sut.ProcessImage(imageProcessViewModel);

            Assert.AreEqual(2, mockImageProvider.FilesAdded.Count);
            Assert.IsTrue(notificationService.NotificationRaised("ImageUploadedEvent", cachedImageUpload));
            Assert.AreEqual(cachedImageUpload, notificationService.EventParam1);

            Assert.IsNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));
            Assert.IsFalse(System.IO.File.Exists(mockImageProvider.TemporaryFiles[0]));
            Assert.IsFalse(System.IO.File.Exists(mockImageProvider.TemporaryFiles[1]));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_MultipleFileUploaded_WithoutSubgroup_NotificationServiceCalled_ReturnsCorrectValidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockNotificationService notificationService = new MockNotificationService(null)
            {
                EventParam1Name = "ImageUploadedEvent"
            };

            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider, notificationService);
            const string File1Data = "test file data for file 1";
            const string File2Data = "test file data for file 2";
            byte[] fileContents1 = Encoding.UTF8.GetBytes(File1Data);
            byte[] fileContents2 = Encoding.UTF8.GetBytes(File2Data);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents1), 0, fileContents1.Length, "test", "testdata1.gif");
            model.Files.Add(formFile);
            formFile = new FormFile(new MemoryStream(fileContents2), 0, fileContents2.Length, "test", "testdata2.svg");
            model.Files.Add(formFile);

            IActionResult response = sut.UploadImage(model);

            ImagesUploadedModel responseModel = ValidateUploadImageResponse(response, memoryCache, "My Pictures", "", 2);

            Assert.AreEqual("My Pictures", responseModel.GroupName);
            Assert.AreEqual("", responseModel.SubgroupName);
            Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));

            CachedImageUpload cachedImageUpload = memoryCache.GetCache().Get(responseModel.MemoryCacheName).Value as CachedImageUpload;
            Assert.IsNotNull(cachedImageUpload);
            ImageProcessViewModel imageProcessViewModel = new ImageProcessViewModel(responseModel.MemoryCacheName);



            IActionResult processResponse = sut.ProcessImage(imageProcessViewModel);

            Assert.AreEqual(2, mockImageProvider.FilesAdded.Count);
            Assert.IsTrue(notificationService.NotificationRaised("ImageUploadedEvent", cachedImageUpload));
            Assert.AreEqual(cachedImageUpload, notificationService.EventParam1);

            Assert.IsNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));
            Assert.IsFalse(System.IO.File.Exists(mockImageProvider.TemporaryFiles[0]));
            Assert.IsFalse(System.IO.File.Exists(mockImageProvider.TemporaryFiles[1]));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ProcessImage_MultipleFileUploaded_ProcessedBy_NotificationService_ReturnsCorrectValidResponse()
        {
            MockMemoryCache memoryCache = new MockMemoryCache();
            MockNotificationService notificationService = new MockNotificationService(true)
            {
                EventParam1Name = "ImageUploadedEvent"
            };
            MockVirusScanner testVirusScanner = new MockVirusScanner();

            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateImageManagerController(memoryCache, null, mockImageProvider, notificationService, testVirusScanner);
            const string File1Data = "test file data for file 1";
            const string File2Data = "test file data for file 2";
            byte[] fileContents1 = Encoding.UTF8.GetBytes(File1Data);
            byte[] fileContents2 = Encoding.UTF8.GetBytes(File2Data);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "Subgroup 1");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents1), 0, fileContents1.Length, "test", "testdata1.gif");
            model.Files.Add(formFile);
            formFile = new FormFile(new MemoryStream(fileContents2), 0, fileContents2.Length, "test", "testdata2.svg");
            model.Files.Add(formFile);

            IActionResult response = sut.UploadImage(model);
            try
            {
                Assert.AreEqual(2, testVirusScanner.ScannedItems.Count);
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response, memoryCache, "My Pictures", "Subgroup 1", 2);

                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));

                CachedImageUpload cachedImageUpload = memoryCache.GetCache().Get(responseModel.MemoryCacheName).Value as CachedImageUpload;
                Assert.IsNotNull(cachedImageUpload);
                ImageProcessViewModel imageProcessViewModel = new ImageProcessViewModel(responseModel.MemoryCacheName);
                IActionResult processResponse = sut.ProcessImage(imageProcessViewModel);

                Assert.AreEqual(0, mockImageProvider.FilesAdded.Count);
                Assert.IsTrue(notificationService.NotificationRaised("ImageUploadedEvent", cachedImageUpload));
                Assert.AreEqual(cachedImageUpload, notificationService.EventParam1);
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        #region Image Template Editor

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditor_Validate_Attributes()
        {
            string methodName = "ImageTemplateEditor";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ImageManagerController), methodName));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditor_Validate_IActionResult_ReturnsPartialView()
        {
            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.ClearAllCaches();
            InitializeImageManagerPluginManager();
            ImageManagerController sut = CreateImageManagerController(null, null, MockImageProvider.CreateDefaultMockImageProvider());

            IActionResult response = sut.ImageTemplateEditor("hello world");

            Assert.IsInstanceOfType(response, typeof(PartialViewResult));

            PartialViewResult viewResult = response as PartialViewResult;

            Assert.IsNotNull(viewResult.Model);

            Assert.AreEqual("/Views/ImageManager/_ImageTemplateEditor.cshtml", viewResult.ViewName);

            Assert.IsInstanceOfType(viewResult.Model, typeof(ImageTemplateEditorModel));
            ImageTemplateEditorModel model = viewResult.Model as ImageTemplateEditorModel;
            model.Data.Equals("hello world");
            Assert.AreEqual(2, model.Groups.Length);
            Assert.AreEqual(2, model.Subgroups.Length);
            Assert.IsNotNull(model.Images);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditorSubGroups_Validate_Attributes()
        {
            string methodName = "ImageTemplateEditorSubGroups";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ImageManagerController), methodName));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ImageManagerController), methodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ImageManagerController), methodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditorSubGroups_InvalidGroupName_Null_ReturnsValidErrorResponse_Success()
        {
            ImageManagerController sut = CreateImageManagerController();
            IActionResult response = sut.ImageTemplateEditorSubGroups(null, null);

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel contentModel = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(contentModel);
            Assert.IsFalse(contentModel.Success);
            Assert.AreEqual("Invalid GroupName", contentModel.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditorSubGroups_InvalidGroupName_EmptyString_ReturnsValidErrorResponse_Success()
        {
            ImageManagerController sut = CreateImageManagerController();
            IActionResult response = sut.ImageTemplateEditorSubGroups("", null);

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel contentModel = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(contentModel);
            Assert.IsFalse(contentModel.Success);
            Assert.AreEqual("Invalid GroupName", contentModel.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditorSubGroups_InvalidGroupName_NotFound_ReturnsValidErrorResponse_Success()
        {
            ImageManagerController sut = CreateImageManagerController();
            IActionResult response = sut.ImageTemplateEditorSubGroups("A group", null);

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel contentModel = jsonResult.Value as JsonResponseModel;

            Assert.IsNotNull(contentModel);
            Assert.IsFalse(contentModel.Success);
            Assert.AreEqual("Invalid GroupName", contentModel.ResponseData);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditorSubGroups_ValidGroupName_TwoItemsFound_ReturnsValidList_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, MockImageProvider.CreateDefaultMockImageProvider());
            IActionResult response = sut.ImageTemplateEditorSubGroups("Group 1", null);

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel responseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(responseModel);

            RetrieveImagesModel imagesModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RetrieveImagesModel>(responseModel.ResponseData);
            Assert.IsNotNull(imagesModel);

            Assert.AreEqual(2, imagesModel.Subgroups.Count);
            Assert.AreEqual("Group 1 Subgroup 1", imagesModel.Subgroups[0]);
            Assert.AreEqual("Group 1 Subgroup 2", imagesModel.Subgroups[1]);

            Assert.AreEqual(1, imagesModel.Images.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditorSubGroups_ValidGroupNameAndSubgroupName_TwoItemsFound_ReturnsValidList_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, MockImageProvider.CreateDefaultMockImageProvider());
            IActionResult response = sut.ImageTemplateEditorSubGroups("Group 1", "Group 1 Subgroup 2");

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel responseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(responseModel);

            RetrieveImagesModel imagesModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RetrieveImagesModel>(responseModel.ResponseData);
            Assert.IsNotNull(imagesModel);

            Assert.AreEqual(2, imagesModel.Subgroups.Count);
            Assert.AreEqual("Group 1 Subgroup 1", imagesModel.Subgroups[0]);
            Assert.AreEqual("Group 1 Subgroup 2", imagesModel.Subgroups[1]);

            Assert.AreEqual(1, imagesModel.Images.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ImageTemplateEditorSubGroups_ValidGroupName_NoSubgroupsFound_ReturnsValidList_Success()
        {
            ImageManagerController sut = CreateImageManagerController(null, null, MockImageProvider.CreateDefaultMockImageProvider());
            IActionResult response = sut.ImageTemplateEditorSubGroups("Group 2", null);

            JsonResult jsonResult = response as JsonResult;

            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel responseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(responseModel);

            RetrieveImagesModel imagesModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RetrieveImagesModel>(responseModel.ResponseData);
            Assert.IsNotNull(imagesModel);

            Assert.AreEqual(0, imagesModel.Subgroups.Count);
            Assert.AreEqual(1, imagesModel.Images.Count);
        }

        #endregion Image Template Editor

        #region Private Methods

        private (ImageProcessViewModel, CachedImageUpload) ValidateValidUploadImageResponse(IActionResult response, MockMemoryCache memoryCache, bool modelStateValid,
            int fileCount, string groupName, string subgroupName = null)
        {
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;
            Assert.AreEqual("/Views/ImageManager/ProcessImage.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.IsNotNull(viewResult.ViewData);
            Assert.AreEqual(modelStateValid, viewResult.ViewData.ModelState.IsValid);
            Assert.AreEqual(0, viewResult.ViewData.ModelState.ErrorCount);

            ValidateBaseModel(viewResult);

            Assert.IsInstanceOfType(viewResult.Model, typeof(ImageProcessViewModel));

            ImageProcessViewModel imageProcessViewModel = (ImageProcessViewModel)viewResult.Model;
            Assert.IsNotNull(imageProcessViewModel);
            Assert.IsNotNull(imageProcessViewModel.FileUploadId);
            Assert.AreNotEqual("", imageProcessViewModel.FileUploadId);

            CacheItem cachedItem = memoryCache.GetCache().Get(imageProcessViewModel.FileUploadId);
            Assert.IsNotNull(cachedItem);

            CachedImageUpload cachedImageUpload = (CachedImageUpload)cachedItem.Value;
            Assert.IsNotNull(cachedImageUpload);
            Assert.AreEqual(fileCount, cachedImageUpload.Files.Count);
            Assert.AreEqual(groupName, cachedImageUpload.GroupName);
            Assert.AreEqual(subgroupName, cachedImageUpload.SubgroupName);

            return (imageProcessViewModel, cachedImageUpload);
        }

        private ImagesUploadedModel ValidateUploadImageResponse(IActionResult response, MockMemoryCache memoryCache,
            string groupName, string subgroupName, int fileCount, int modelStateErrorCount = 0)
        {
            Assert.IsInstanceOfType(response, typeof(ViewResult));

            ViewResult viewResult = response as ViewResult;
            Assert.AreEqual("/Views/ImageManager/ImageUpload.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.IsNotNull(viewResult.ViewData);

            if (modelStateErrorCount == 0)
                Assert.IsTrue(viewResult.ViewData.ModelState.IsValid);
            else
                Assert.IsFalse(viewResult.ViewData.ModelState.IsValid);

            Assert.AreEqual(modelStateErrorCount, viewResult.ViewData.ModelState.ErrorCount);
            ValidateBaseModel(viewResult);

            Assert.IsInstanceOfType(viewResult.Model, typeof(ProcessImagesViewModel));

            ProcessImagesViewModel imageProcessViewModel = (ProcessImagesViewModel)viewResult.Model;
            Assert.IsNotNull(imageProcessViewModel);
            Assert.IsNotNull(imageProcessViewModel.FileUploadId);
            Assert.AreNotEqual("", imageProcessViewModel.FileUploadId);

            CacheItem cachedItem = memoryCache.GetCache().Get(imageProcessViewModel.FileUploadId);
            Assert.IsNotNull(cachedItem);

            CachedImageUpload cachedImageUpload = (CachedImageUpload)cachedItem.Value;
            Assert.IsNotNull(cachedImageUpload);
            Assert.AreEqual(groupName, cachedImageUpload.GroupName);
            Assert.AreEqual(subgroupName, cachedImageUpload.SubgroupName);
            Assert.AreEqual(fileCount, cachedImageUpload.Files.Count);
            Assert.IsNotNull(memoryCache.GetCache().Get(imageProcessViewModel.FileUploadId));

            return new ImagesUploadedModel(cachedImageUpload.GroupName, cachedImageUpload.SubgroupName, imageProcessViewModel.FileUploadId);
        }

        private void ValidateJsonResult(IActionResult response,
            string expectedData,
            int expectedStatusCode = 400,
            string expectedContentType = "application/json",
            bool successResponse = false)
        {
            Assert.IsInstanceOfType(response, typeof(JsonResult));

            JsonResult jsonResult = response as JsonResult;

            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(expectedStatusCode, jsonResult.StatusCode);
            Assert.AreEqual(expectedContentType, jsonResult.ContentType);
            Assert.IsNotNull(jsonResult.Value);
            Assert.IsInstanceOfType(jsonResult.Value, typeof(JsonResponseModel));

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual(successResponse, jsonResponseModel.Success);
            Assert.AreEqual(expectedData, jsonResponseModel.ResponseData);
        }

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

        private ImageManagerController CreateImageManagerController(MockMemoryCache memoryCache = null,
            List<BreadcrumbItem> breadcrumbs = null, MockImageProvider mockImageProvider = null,
            MockNotificationService testNotificationService = null,
            MockVirusScanner testVirusScanner = null)
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            IPluginHelperService pluginHelperService = _testDynamicContentPlugin as IPluginHelperService;
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            ImageManagerController Result = new ImageManagerController(
                new MockSettingsProvider(),
                mockImageProvider ?? new MockImageProvider(),
                testNotificationService ?? new MockNotificationService(),
                memoryCache ?? new MockMemoryCache(),
                testVirusScanner ?? new MockVirusScanner());

            Result.ControllerContext = CreateTestControllerContext(breadcrumbs);

            return Result;
        }

        #endregion Private Methods
    }
}
