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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ResourcesModelTests.cs
 *
 *  Purpose:  Tests for ResourcesModel class
 *
 *  Date        Name                Reason
 *  29/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Resources;

using PluginManager.Tests.Mocks;

using Resources.Plugin.Controllers;
using Resources.Plugin.Models;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;


namespace AspNetCore.PluginManager.Tests.Plugins.ResourceTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class ResourceControllerTests : BaseControllerTests
	{
		private const string TestCategoryName = "Resources";

		[TestMethod]
		[TestCategory(TestCategoryName)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidInstanceParameterNull_Throws_ArgumentNullException()
		{
			new ResourcesController(null);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Index_ResourcesHome_ReturnsCorrectView_Success()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.Index();

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			ResourcesModel resourceModel = result.Model as ResourcesModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreEqual(5, resourceModel.ResourceCategories.Count);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewCategory_ResourceNameNull_RedirectsToIndex()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewCategory(1, null);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
			Assert.IsNull(result.ControllerName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewCategory_ResourceNameNotFound_RedirectsToIndex()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewCategory(101, "not a resource");

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
			Assert.IsNull(result.ControllerName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewCategory_ResourceFound_ReturnsViewCategory()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewCategory(1, "resource-1");

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			ResourceCategoryModel resourceModel = result.Model as ResourceCategoryModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);
			Assert.AreEqual("Resource 1", resourceModel.Name);
			Assert.AreEqual("Resource desc 1", resourceModel.Description);
			Assert.AreEqual("black", resourceModel.ForeColor);
			Assert.AreEqual("resource-1", resourceModel.RouteName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ItemResponse_ResourceNotFound_ReturnsJson400()
		{
			ResourcesController sut = CreateResourceController();

			JsonResult response = sut.ItemResponse(new ItemResponseModel() { id = -10, value = false});
			Assert.IsNotNull(response);

			Assert.AreEqual(400, response.StatusCode);
			Assert.AreEqual("application/json", response.ContentType);

			JsonResponseModel jsonResponseModel = response.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);

			Assert.AreEqual("item not found", jsonResponseModel.ResponseData);
			Assert.IsFalse(jsonResponseModel.Success);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ItemResponse_ResourceFound_ReturnsJsonResponseWithIncrementedDislike()
		{
			ResourcesController sut = CreateResourceController();

			_ = sut.Index();
			JsonResult response = sut.ItemResponse(new ItemResponseModel() { id = 101, value = false });
			Assert.IsNotNull(response);

			Assert.AreEqual(200, response.StatusCode);
			Assert.AreEqual("application/json", response.ContentType);

			JsonResponseModel jsonResponseModel = response.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);

			Assert.AreEqual("{\"itemId\":101,\"likes\":0,\"dislikes\":1}", jsonResponseModel.ResponseData);
			Assert.IsTrue(jsonResponseModel.Success);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ItemResponse_ResourceFound_ReturnsJsonResponseWithIncrementedLike()
		{
			ResourcesController sut = CreateResourceController();

			_ = sut.Index();
			JsonResult response = sut.ItemResponse(new ItemResponseModel() { id = 102, value = true });
			Assert.IsNotNull(response);

			Assert.AreEqual(200, response.StatusCode);
			Assert.AreEqual("application/json", response.ContentType);

			JsonResponseModel jsonResponseModel = response.Value as JsonResponseModel;
			Assert.IsNotNull(jsonResponseModel);

			Assert.AreEqual("{\"itemId\":102,\"likes\":1,\"dislikes\":0}", jsonResponseModel.ResponseData);
			Assert.IsTrue(jsonResponseModel.Success);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateCategory_ReturnsCorrectViewAndModel_Success()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.CreateCategory(parentId: 0);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateCategoryModel resourceModel = result.Model as CreateCategoryModel;

			Assert.IsNotNull(resourceModel);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateCategory_Post_NullModel_RedirectsToIndex()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.CreateCategory(model: null);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateCategory_Post_NameExists_ReturnsModelStateError()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.CreateCategory(new CreateCategoryModel() { Name = "Resource 1" });

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateCategoryModel resourceModel = result.Model as CreateCategoryModel;

			Assert.IsNotNull(resourceModel);

			ViewResultContainsModelStateError(result, "Name", "The category name already exists");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateCategory_UserHasNoClaimToManageResources_RedirectsToCategorySubmitted()
		{
			ResourcesController sut = CreateResourceController(null, new MockHttpContext());

			IActionResult response = sut.CreateCategory(new CreateCategoryModel() { Name = "My new Resource" });

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("CategorySubmitted", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateCategory_UserHasClaimToManageResources_RedirectsToManageCategories()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			List<Claim> webClaims = new List<Claim>();
			webClaims.Add(new Claim(ClaimNameManageResources, "true"));

			claimsIdentities.Add(new ClaimsIdentity(webClaims, ClaimIdentityWebsite));

			requestContext.User = new System.Security.Claims.ClaimsPrincipal(claimsIdentities);
			ResourcesController sut = CreateResourceController(null, requestContext);

			IActionResult response = sut.CreateCategory(new CreateCategoryModel() { Name = "My new Resource" });

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ManageCategories", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CategoryEdit_CategoryNotFound_RedirectsToManageCategories()
		{
			ResourcesController sut = CreateResourceController(null, new MockHttpContext());

			IActionResult response = sut.CategoryEdit(23698745);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ManageCategories", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CategoryEdit_CategoryFound_ReturnsValidViewAndModel()
		{
			ResourcesController sut = CreateResourceController(null, new MockHttpContext());

			IActionResult response = sut.CategoryEdit(2);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);
			
			ResourceEditCategoryModel resourceModel = result.Model as ResourceEditCategoryModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreEqual("Resource desc 2", resourceModel.Description);
			Assert.AreEqual("Resource 2", resourceModel.Name);
			Assert.AreEqual(2, resourceModel.Id);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CategoryEdit_ModelIsNull_RedirectsToManageCategories()
		{
			ResourcesController sut = CreateResourceController(null, new MockHttpContext());

			IActionResult response = sut.CategoryEdit(null);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ManageCategories", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CategoryEdit_ModelIsSameNameAsExisting_ReturnsModelStateError()
		{
			ResourcesController sut = CreateResourceController(null, new MockHttpContext());

			ResourceEditCategoryModel model = new ResourceEditCategoryModel() { Name = "Resource 1" };
			IActionResult response = sut.CategoryEdit(model);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			ResourceCategoryModel resourceModel = result.Model as ResourceCategoryModel;

			Assert.IsNotNull(resourceModel);

			ViewResultContainsModelStateError(result, "Name", "The category name already exists");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CategoryEdit_UpdateAllowd_ReturnsRedirectToActionWithGrowlMessage()
		{
			Dictionary<Type, object> services = new Dictionary<Type, object>();

			ITempDataProvider tempDataProvider = new MockTempDataProvider();
			services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

			MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
			services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

			MockServiceProvider mockServiceProvider = new MockServiceProvider(services);

			ResourcesController sut = CreateResourceController(null, new MockHttpContext(), mockServiceProvider);

			ResourceEditCategoryModel model = new ResourceEditCategoryModel() { Name = "Test New Resource" };
			IActionResult response = sut.CategoryEdit(model);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ManageCategories", result.ActionName);
			Assert.AreEqual(1, sut.TempData.Count);
			Assert.AreEqual("Category 'Test New Resource' has been updated", sut.TempData["growl"]);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewResource_ItemIdNotFound_RedirectsToIndex()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewResource(Int64.MaxValue);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
			Assert.IsNull(result.ControllerName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewResource_ResourceItemIsTickTock_RedirectsToCategory()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewResource(101);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ViewCategory", result.ActionName);
			Assert.IsNull(result.ControllerName);
			Assert.AreEqual(2, result.RouteValues.Count);
			Assert.IsTrue(result.RouteValues.Keys.Contains("id"));
			Assert.IsTrue(result.RouteValues.Keys.Contains("categoryName"));
			Assert.IsTrue(result.RouteValues.Values.Contains("resource-1"));
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewResource_ResourceItemIsYouTube_RedirectsToCategory()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewResource(202);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ViewCategory", result.ActionName);
			Assert.IsNull(result.ControllerName);
			Assert.AreEqual(2, result.RouteValues.Count);
			Assert.IsTrue(result.RouteValues.Keys.Contains("id"));
			Assert.IsTrue(result.RouteValues.Keys.Contains("categoryName"));
			Assert.IsTrue(result.RouteValues.Values.Contains("resource-2"));
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewResource_ResourceItemIsUri_RedirectsToCategory()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewResource(303);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ViewCategory", result.ActionName);
			Assert.IsNull(result.ControllerName);
			Assert.AreEqual(2, result.RouteValues.Count);
			Assert.IsTrue(result.RouteValues.Keys.Contains("id"));
			Assert.IsTrue(result.RouteValues.Keys.Contains("categoryName"));
			Assert.IsTrue(result.RouteValues.Values.Contains("resource-3"));
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewResource_ResourceItemIsText_IncrementsViewCountRedirectsToCategory()
		{
			MockResourceProvider mockResourceProvider = new MockResourceProvider();
			ResourceItem resourceItem = mockResourceProvider.GetResourceItem(405);
			Assert.IsNotNull(resourceItem);
			Assert.AreEqual(0, resourceItem.ViewCount);
			ResourcesController sut = CreateResourceController(mockResourceProvider);

			IActionResult response = sut.ViewResource(405);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			ResourceViewItemModel resourceModel = result.Model as ResourceViewItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreEqual(405, resourceModel.Id);

			resourceItem = mockResourceProvider.GetResourceItem(405);
			Assert.IsNotNull(resourceItem);
			Assert.AreEqual(1, resourceItem.ViewCount);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewExternalResource_InvalidResourceItemId_RedirectsToAction()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewExternalResource(2365);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
			Assert.IsNull(result.ControllerName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewExternalResource_YoutubeResource_RedirectsToYoutube()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewExternalResource(101);

			RedirectResult result = response as RedirectResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("https://www.youtube.com/watch?v=OP1tBC6dBW0", result.Url);
			Assert.IsFalse(result.Permanent);
			Assert.IsFalse(result.PreserveMethod);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewExternalResource_TikTokResource_RedirectsToTikTok()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewExternalResource(202);

			RedirectResult result = response as RedirectResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("https://www.tiktok.com/@visualstudio/video/7026423558041537839", result.Url);
			Assert.IsFalse(result.Permanent);
			Assert.IsFalse(result.PreserveMethod);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewExternalResource_UriResource_RedirectsToUri()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewExternalResource(303);

			RedirectResult result = response as RedirectResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("https://www.pluginmanager.website/", result.Url);
			Assert.IsFalse(result.Permanent);
			Assert.IsFalse(result.PreserveMethod);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewExternalResource_ImageResource_RedirectsToResourcesView()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.ViewExternalResource(404);

			RedirectResult result = response as RedirectResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("/Resources/View/404/", result.Url);
			Assert.IsFalse(result.Permanent);
			Assert.IsFalse(result.PreserveMethod);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void ViewExternalResource_TextResource_IncrementsViewCountRedirectsToResourcesView()
		{
			MockResourceProvider mockResourceProvider = new MockResourceProvider();
			ResourceItem resourceItem = mockResourceProvider.GetResourceItem(405);
			Assert.IsNotNull(resourceItem);
			Assert.AreEqual(0, resourceItem.ViewCount);
			ResourcesController sut = CreateResourceController(mockResourceProvider);
			IActionResult response = sut.ViewExternalResource(405);

			RedirectResult result = response as RedirectResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("/Resources/View/405/", result.Url);
			Assert.IsFalse(result.Permanent);
			Assert.IsFalse(result.PreserveMethod);

			resourceItem = mockResourceProvider.GetResourceItem(405);
			Assert.IsNotNull(resourceItem);
			Assert.AreEqual(1, resourceItem.ViewCount);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_CategoryDoesNotExist_RedirectsToAction()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.CreateResourceItem(9247);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_CategoryExists_ReturnsViewWithModel()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.CreateResourceItem(2);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel model = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(model);
			Assert.AreEqual(2, model.Breadcrumbs.Count);
			Assert.AreEqual(2, model.ParentId);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_ModelIsNull_RedirectsToIndex()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.CreateResourceItem(null);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_ParentIdDoesNotExist_RedirectsToIndex()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.CreateResourceItem(new CreateResourceItemModel() { ParentId = 39721 });

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_NameIsNull_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel() 
			{ 
				ParentId = 2, 
				Name = null,
				Description = null,
				Value = "test",
				ResourceType = 3,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("test", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "Name", "Please enter a valid name between 5 and 30 characters");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_NameIsTooShort_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "blah",
				Description = null,
				Value = "test",
				ResourceType = 3,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("test", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "Name", "Please enter a valid name between 5 and 30 characters");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_NameIsTooLong_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name of the resource item which is waaayyyyy too long",
				Description = null,
				Value = "test",
				ResourceType = 3,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("test", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "Name", "Please enter a valid name between 5 and 30 characters");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_DescriptionIsNull_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name",
				Description = null,
				Value = "test",
				ResourceType = 3,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("test", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "Description", "Please enter a valid description between 15 and 100 characters");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_DescriptionIsTooShort_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name",
				Description = "desc",
				Value = "test",
				ResourceType = 3,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("test", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "Description", "Please enter a valid description between 15 and 100 characters");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_DescriptionIsTooLong_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name",
				Description = "The description maximum length is one hundred characters, this description is waaayyyy too long to fit in and will cause error",
				Value = "test",
				ResourceType = 3,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("test", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "Description", "Please enter a valid description between 15 and 100 characters");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_YouTubeInvalidIdEmptyString_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name",
				Description = "The description maximum length is one hundred characters",
				Value = "",
				ResourceType = (int)ResourceType.YouTube,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "", "YouTube id vi9Vo2kIQrww7vi9Vo2kIQrww7vi9Vo2kIQrww7 does not appear to be valid.");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_YouTubeInvalidIdContainsInvalidCharacters_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name",
				Description = "The description maximum length is one hundred characters",
				Value = "ydR%30=",
				ResourceType = (int)ResourceType.YouTube,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("ydR%30=", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "", "YouTube id vi9Vo2kIQrww7vi9Vo2kIQrww7vi9Vo2kIQrww7 does not appear to be valid.");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_YouTubeInvalidId_ReturnsModelStateError()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			ResourcesController sut = CreateResourceController();

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name",
				Description = "The description maximum length is one hundred characters",
				Value = "vi9Vo2kIQrww7vi9Vo2kIQrww7",
				ResourceType = (int)ResourceType.YouTube,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			ViewResult result = response as ViewResult;

			Assert.IsNotNull(result.Model);
			Assert.IsNull(result.ViewName);

			CreateResourceItemModel resourceModel = result.Model as CreateResourceItemModel;

			Assert.IsNotNull(resourceModel);
			Assert.AreNotSame(itemModel, resourceModel);

			Assert.AreEqual("vi9Vo2kIQrww7vi9Vo2kIQrww7", resourceModel.Value);
			Assert.AreEqual(3, resourceModel.ResourceType);
			Assert.AreEqual(2, resourceModel.ParentId);
			Assert.AreEqual(2, resourceModel.Breadcrumbs.Count);

			ViewResultContainsModelStateError(result, "", "YouTube id vi9Vo2kIQrww7vi9Vo2kIQrww7 does not appear to be valid.");
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void CreateResourceItem_YouTubeValidId_CreatesResourceItem_RedirectsToThankYou()
		{
			MockHttpContext requestContext = new MockHttpContext();
			List<ClaimsIdentity> claimsIdentities = new List<ClaimsIdentity>();

			List<Claim> webClaims = new List<Claim>();
			webClaims.Add(new Claim(ClaimNameManageResources, "true"));

			claimsIdentities.Add(new ClaimsIdentity(webClaims, ClaimIdentityWebsite));

			requestContext.User = new System.Security.Claims.ClaimsPrincipal(claimsIdentities);
			ResourcesController sut = CreateResourceController(null, requestContext);
			MockHttpContext mockHttpContext = sut.HttpContext as MockHttpContext;
			mockHttpContext.LogUserIn = true;

			CreateResourceItemModel itemModel = new CreateResourceItemModel()
			{
				ParentId = 2,
				Name = "This is the name",
				Description = "The description maximum length is one hundred characters",
				Value = "l8jn32uoZZM",
				ResourceType = (int)ResourceType.YouTube,
			};

			IActionResult response = sut.CreateResourceItem(itemModel);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ResourceItemSubmitted", result.ActionName);
		}

		private ResourcesController CreateResourceController(MockResourceProvider mockResourceProvider = null, 
			MockHttpContext mockContext = null, MockServiceProvider mockServiceProvider = null)
		{

			ResourcesController Result = new ResourcesController(mockResourceProvider ?? new MockResourceProvider());

			Result.ControllerContext = CreateTestControllerContext(null, null, mockServiceProvider, null, mockContext);

			return Result;
		}
	}
}
