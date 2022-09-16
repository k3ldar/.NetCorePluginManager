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

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Resources.Plugin.Controllers;
using Resources.Plugin.Models;

using SharedPluginFeatures;


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
			Assert.AreEqual(3, resourceModel.Breadcrumbs.Count);
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

			IActionResult response = sut.CreateCategory(parentId: null);

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
		public void View_ItemIdNotFound_RedirectsToIndex()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.View(Int64.MaxValue);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("Index", result.ActionName);
			Assert.IsNull(result.ControllerName);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void View_ResourceItemIsTickTock_RedirectsToCategory()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.View(101);

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
		public void View_ResourceItemIsYouTube_RedirectsToCategory()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.View(202);

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
		public void View_ResourceItemIsUri_RedirectsToCategory()
		{
			ResourcesController sut = CreateResourceController();

			IActionResult response = sut.View(303);

			RedirectToActionResult result = response as RedirectToActionResult;

			Assert.IsNotNull(result);
			Assert.AreEqual("ViewCategory", result.ActionName);
			Assert.IsNull(result.ControllerName);
			Assert.AreEqual(2, result.RouteValues.Count);
			Assert.IsTrue(result.RouteValues.Keys.Contains("id"));
			Assert.IsTrue(result.RouteValues.Keys.Contains("categoryName"));
			Assert.IsTrue(result.RouteValues.Values.Contains("resource-3"));
		}

		private ResourcesController CreateResourceController()
		{
			ResourcesController Result = new ResourcesController(new MockResourceProvider());

			Result.ControllerContext = CreateTestControllerContext();

			return Result;
		}
	}
}
