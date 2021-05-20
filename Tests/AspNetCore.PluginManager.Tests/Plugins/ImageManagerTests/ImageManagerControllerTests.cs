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
using AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests.Mocks;
using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin.Controllers;
using ImageManager.Plugin.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Images;

using Newtonsoft.Json;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ImageManagerControllerTests : BaseControllerTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";
        private const string ExpectedResponseWithNoListener = "{\"GroupName\":\"Group\",\"SubgroupName\":\"test subgrouP\",\"ShowSubgroup\":true,\"AdditionalDataName\":null,\"AdditionalData\":null,\"AdditionalDataMandatory\":false}";
        private const string ExpectedResponseWithListener = "{\"GroupName\":\"Images\",\"SubgroupName\":null,\"ShowSubgroup\":false,\"AdditionalDataName\":\"Enter product code/sku\",\"AdditionalData\":null,\"AdditionalDataMandatory\":true}";

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
            ImageManagerController sut = new ImageManagerController(null, new MockImageProvider(), new TestNotificationService(), new TestMemoryCache());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidImageProvider_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new TestSettingsProvider("{}"), null, new TestNotificationService(), new TestMemoryCache());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidNotificationService_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new TestSettingsProvider("{}"), new MockImageProvider(), null, new TestMemoryCache());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidMemoryCache_Null_Throws_ArgumentNullException()
        {
            ImageManagerController sut = new ImageManagerController(new TestSettingsProvider("{}"), new MockImageProvider(), new TestNotificationService(), null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Construct_ValidInstance_Success()
        {
            ImageManagerController sut = new ImageManagerController(new TestSettingsProvider("{}"), new MockImageProvider(), new TestNotificationService(), new TestMemoryCache());

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
        public void Validate_ControllerViewsAreIncludedAsResources_Success()
        {
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "_LeftMenu"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "ImageUpload"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "Index"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "ViewImage"));
            Assert.IsTrue(AssemblyHasViewResource(typeof(ImageManagerController), ImageManagerController.Name, "_ImageUpload"));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
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
        [TestCategory(ImageManagerTestsCategory)]
        public void Index_ReturnsCorrectModel_Success()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());

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
        [TestCategory(ImageManagerTestsCategory)]
        public void Index_ReturnsCorrectModel_WithUserThatCanManageImages_Success()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
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

            IActionResult response = sut.ViewImage("Group 1", "myfile-gif");

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
            Assert.AreEqual(3, imagesViewModel.Breadcrumbs.Count);
            Assert.AreEqual("Image Manager", imagesViewModel.Breadcrumbs[0].Name);
            Assert.AreEqual("Group 1", imagesViewModel.Breadcrumbs[1].Name);
            //Assert.AreEqual("Group 1 Subgroup 1", imagesViewModel.Breadcrumbs[2].Name);
            Assert.AreEqual("myfile.gif", imagesViewModel.Breadcrumbs[2].Name);
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

            IActionResult response = sut.ViewSubgroupImage("Group 1", "Group 1 Subgroup 1", "myfile-gif");

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
            Assert.AreEqual(4, imagesViewModel.Breadcrumbs.Count);
            Assert.AreEqual("Image Manager", imagesViewModel.Breadcrumbs[0].Name);
            Assert.AreEqual("Group 1", imagesViewModel.Breadcrumbs[1].Name);
            Assert.AreEqual("Group 1 Subgroup 1", imagesViewModel.Breadcrumbs[2].Name);
            Assert.AreEqual("myfile.gif", imagesViewModel.Breadcrumbs[3].Name);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidParamModel_Null_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.DeleteImage(null);

            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_ConfirmDeleteFalse_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.DeleteImage(new DeleteImageModel());

            ValidateJsonResult(response, "Confirmation required");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_ImageName_Null_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid ImageName");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_ImageName_EmptyString_NoGroupOrSubgroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            DeleteImageModel deleteImageModel = new DeleteImageModel()
            {
                ConfirmDelete = true,
                ImageName = ""
            };
            IActionResult response = sut.DeleteImage(deleteImageModel);

            ValidateJsonResult(response, "Invalid ImageName");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_GroupName_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_GroupName_EmptyString_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_GroupName_DoesNotExist_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_ImageNotFound_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_InvalidModel_SubgroupNoGroup_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_ValidModel_WithoutSubgroup_ReturnsCorrectValidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(null, null, mockImageProvider);
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_ValidModel_WithNonExistantSubgroup_ReturnsCorrectInvalidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(null, null, mockImageProvider);
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_ValidModel_WithSubgroup_ReturnsCorrectValidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(null, null, mockImageProvider);
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
        [TestCategory(ImageManagerTestsCategory)]
        public void DeleteImage_ValidModel_WithoutSubgroup_ImageNotDeleted_ReturnsCorrectInvalidResponse()
        {
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            mockImageProvider.CanDeleteImages = false;

            ImageManagerController sut = CreateDynamicContentController(null, null, mockImageProvider);
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
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_HasCorrectAttributes_Success()
        {
            string methodName = "UploadImage";
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
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_InvalidFileList_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.UploadImage(null);

            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_InvalidFileList_Empty_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.UploadImage(new UploadImageModel(GenerateTestBaseModelData(), "My Group", null));

            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_InvalidGroupName_Null_ReturnsCorrectInvalidResponse()
        {
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData());

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.dat");
            model.Files.Add(formFile);
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            IActionResult response = sut.UploadImage(model);

            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_SingleFileUploaded_ReturnsCorrectValidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, mockImageProvider);
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", null);

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.jpeg");
            model.Files.Add(formFile);


            IActionResult response = sut.UploadImage(model);
            try
            {
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response);
                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.IsNull(responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_FileExtensionNotSupported_ReturnsCorrectInvalidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, mockImageProvider);
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", null);

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.dat");
            model.Files.Add(formFile);


            IActionResult response = sut.UploadImage(model);
            ValidateJsonResult(response, "The selected file type is not supported");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_SingleFileUploaded_WithSubgroup_ReturnsCorrectValidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, mockImageProvider);
            const string FileData = "test file data";
            byte[] fileContents = Encoding.UTF8.GetBytes(FileData);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "Subgroup 1");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents), 0, fileContents.Length, "test", "testdata.svg");
            model.Files.Add(formFile);


            IActionResult response = sut.UploadImage(model);
            try
            {
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response);
                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void UploadImage_MultipleFileUploaded_WithSubgroup_ReturnsCorrectValidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, mockImageProvider);
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
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response);
                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
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
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImage_Construct_InvalidParamModel_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImage(null);
            ValidateJsonResult(response, "Invalid Model");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImage_Construct_InvalidParamModel_NonExistentCacheItem_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImage(new ImageProcessViewModel(GenerateTestBaseModelData(), "not found"));
            ValidateJsonResult(response, "Image cache not found");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImage_Construct_InvalidParamModel_CachedItemNotValidClass_ReturnsCorrectInvalidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            memoryCache.GetCache().Add("found item", new CacheItem("found item", 123));
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, CreateDefaultMockImageProvider());

            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImage(new ImageProcessViewModel(GenerateTestBaseModelData(), "found item"));
            ValidateJsonResult(response, "Image cache not found");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImage_SingleFileUploaded_WithSubgroup_NotificationServiceCalled_ReturnsCorrectValidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            TestNotificationService notificationService = new TestNotificationService(null)
            {
                EventParam1Name = "ImageUploadedEvent"
            };

            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, mockImageProvider, notificationService);
            const string File1Data = "test file data for file 1";
            byte[] fileContents1 = Encoding.UTF8.GetBytes(File1Data);

            UploadImageModel model = new UploadImageModel(GenerateTestBaseModelData(), "My Pictures", "Subgroup 1");

            IFormFile formFile = new FormFile(new MemoryStream(fileContents1), 0, fileContents1.Length, "test", "testdata1.gif");
            model.Files.Add(formFile);

            IActionResult response = sut.UploadImage(model);
            try
            {
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response);
                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));

                CachedImageUpload cachedImageUpload = memoryCache.GetCache().Get(responseModel.MemoryCacheName).Value as CachedImageUpload;
                Assert.IsNotNull(cachedImageUpload);

                ImageProcessViewModel imageProcessViewModel = new ImageProcessViewModel(GenerateTestBaseModelData(), responseModel.MemoryCacheName);
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
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImage_MultipleFileUploaded_WithSubgroup_NotificationServiceCalled_ReturnsCorrectValidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            TestNotificationService notificationService = new TestNotificationService(null)
            {
                EventParam1Name = "ImageUploadedEvent"
            };

            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, mockImageProvider, notificationService);
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
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response);
                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));

                CachedImageUpload cachedImageUpload = memoryCache.GetCache().Get(responseModel.MemoryCacheName).Value as CachedImageUpload;
                Assert.IsNotNull(cachedImageUpload);
                ImageProcessViewModel imageProcessViewModel = new ImageProcessViewModel(GenerateTestBaseModelData(), responseModel.MemoryCacheName);
                IActionResult processResponse = sut.ProcessImage(imageProcessViewModel);

                Assert.AreEqual(2, mockImageProvider.FilesAdded.Count);
                Assert.IsTrue(notificationService.NotificationRaised("ImageUploadedEvent", cachedImageUpload));
                Assert.AreEqual(cachedImageUpload, notificationService.EventParam1);
            }
            finally
            {
                System.IO.File.Delete(mockImageProvider.TemporaryFiles[0]);
            }
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImage_MultipleFileUploaded_ProcessedBy_NotificationService_ReturnsCorrectValidResponse()
        {
            TestMemoryCache memoryCache = new TestMemoryCache();
            TestNotificationService notificationService = new TestNotificationService(true)
            {
                EventParam1Name = "ImageUploadedEvent"
            };

            MockImageProvider mockImageProvider = CreateDefaultMockImageProvider();
            ImageManagerController sut = CreateDynamicContentController(memoryCache, null, mockImageProvider, notificationService);
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
                ImagesUploadedModel responseModel = ValidateUploadImageResponse(response);
                Assert.AreEqual("My Pictures", responseModel.GroupName);
                Assert.AreEqual("Subgroup 1", responseModel.SubgroupName);
                Assert.IsNotNull(memoryCache.GetCache().Get(responseModel.MemoryCacheName));

                CachedImageUpload cachedImageUpload = memoryCache.GetCache().Get(responseModel.MemoryCacheName).Value as CachedImageUpload;
                Assert.IsNotNull(cachedImageUpload);
                ImageProcessViewModel imageProcessViewModel = new ImageProcessViewModel(GenerateTestBaseModelData(), responseModel.MemoryCacheName);
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


        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImageOptions_HasCorrectAttributes_Success()
        {
            string methodName = "ProcessImageOptions";
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
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImageOptions_Construct_InvalidParamModel_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImageOptions(null);
            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImageOptions_Construct_InvalidParamGroupName_Null_ReturnsCorrectInvalidResponse()
        {
            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider());
            Assert.IsNotNull(sut);

            IActionResult response = sut.ProcessImageOptions(new ImageProcessOptionsViewModel());
            ValidateJsonResult(response, "Invalid GroupName");
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImageOptions_NoListener_ReturnsCorrectValidResponse_WithDefaultOptions()
        {
            TestNotificationService notificationService = new TestNotificationService(null)
            {
                EventParam1Name = "ImageUploadOptions"
            };

            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider(), notificationService);
            Assert.IsNotNull(sut);

            ImageProcessOptionsViewModel options = new ImageProcessOptionsViewModel()
            {
                GroupName = "Group",
                SubgroupName = "test subgrouP",
            };

            IActionResult response = sut.ProcessImageOptions(options);
            ValidateJsonResult(response, ExpectedResponseWithNoListener, 200, "application/json", true);
            Assert.AreEqual(1, notificationService.NotificationRaised("ImageUploadOptions"));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ProcessImageOptions_WithValidListener_ReturnsCorrectValidResponse_WithDefaultOptions()
        {
            ImageProcessOptionsViewModel expectedResponseModel = new ImageProcessOptionsViewModel()
            {
                GroupName = "Images",
                SubgroupName = null,
                ShowSubgroup = false,
                AdditionalDataName = "Enter product code/sku",
                AdditionalDataMandatory = true
            };

            TestNotificationService notificationService = new TestNotificationService(expectedResponseModel)
            {
                EventParam1Name = "ImageUploadOptions"
            };

            ImageManagerController sut = CreateDynamicContentController(null, null, CreateDefaultMockImageProvider(), notificationService);
            Assert.IsNotNull(sut);

            ImageProcessOptionsViewModel options = new ImageProcessOptionsViewModel()
            {
                GroupName = "Group",
                SubgroupName = "test subgrouP",
            };

            IActionResult response = sut.ProcessImageOptions(options);
            ValidateJsonResult(response, ExpectedResponseWithListener, 200, "application/json", true);
            Assert.AreEqual(1, notificationService.NotificationRaised("ImageUploadOptions"));
            Assert.AreEqual(options, notificationService.EventParam1);
        }

        #region Private Methods

        private (ImageProcessViewModel, CachedImageUpload) ValidateValidUploadImageResponse(IActionResult response, TestMemoryCache memoryCache, bool modelStateValid,
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

        private ImagesUploadedModel ValidateUploadImageResponse(IActionResult response)
        {
            Assert.IsInstanceOfType(response, typeof(JsonResult));

            JsonResult jsonResult = response as JsonResult;

            Assert.IsNotNull(jsonResult);
            Assert.AreEqual(200, jsonResult.StatusCode);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNotNull(jsonResult.Value);
            Assert.IsInstanceOfType(jsonResult.Value, typeof(JsonResponseModel));

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.IsTrue(jsonResponseModel.Success);
            return JsonConvert.DeserializeObject<ImagesUploadedModel>(jsonResponseModel.Data);
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
            Assert.AreEqual(expectedData, jsonResponseModel.Data);
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

        private ImageManagerController CreateDynamicContentController(TestMemoryCache memoryCache = null,
            List<BreadcrumbItem> breadcrumbs = null, MockImageProvider mockImageProvider = null,
            TestNotificationService testNotificationService = null)
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testDynamicContentPlugin) as IPluginClassesService;
            IPluginHelperService pluginHelperService = new pm.PluginServices(_testDynamicContentPlugin) as IPluginHelperService;
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            ImageManagerController Result = new ImageManagerController(
                new TestSettingsProvider("{}"),
                mockImageProvider ?? new MockImageProvider(),
                testNotificationService ?? new TestNotificationService(),
                memoryCache ?? new TestMemoryCache());

            Result.ControllerContext = CreateTestControllerContext(breadcrumbs);

            return Result;
        }

        #endregion Private Methods
    }
}
