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
 *  Product:  Resources.Plugin
 *  
 *  File: ResourcesController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Languages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Resources;

using Resources.Plugin.Models;

using SharedPluginFeatures;


#pragma warning disable CS1591

namespace Resources.Plugin.Controllers
{
    [DenySpider]
    public class ResourcesController : BaseController
    {
		#region Private Members

		private readonly IResourceProvider _resourceProvider;

		#endregion Private Members

		#region Constructors

		public ResourcesController(IResourceProvider resourceProvider)
        {
			_resourceProvider = resourceProvider ?? throw new ArgumentNullException(nameof(resourceProvider));
        }

        #endregion Constructors

        #region Constants

        public const string Name = "Resources";

        #endregion Constants

        #region Controller Action Methods

        [HttpGet]
		[Breadcrumb(nameof(LanguageStrings.ResourcesMain))]
        public IActionResult Index()
        {
            return View(CreateResourcesModel(_resourceProvider.GetAllResources()));
        }

		[HttpGet]
		[Route("/Resources/Category/{id}/{categoryName}/")]
		public IActionResult ViewCategory(long id, string categoryName)
		{
			if (String.IsNullOrEmpty(categoryName))
				return RedirectToAction(nameof(Index));

			categoryName = ValidateUserInput(categoryName, ValidationType.RouteName);

			ResourceCategory resourceCategory = _resourceProvider.GetResourceCategory(id);

			if (resourceCategory == null)
				return RedirectToAction(nameof(Index));

			return View(CreateResourceCategoryViewModel(resourceCategory));
		}

		[LoggedIn]
		[AjaxOnly]
		[HttpPost]
		public JsonResult ItemResponse(ItemResponseModel model)
		{
			ResourceItem resouceItem = _resourceProvider.IncrementResourceItemResponse(model.id, UserId(), model.value);

			if (resouceItem != null)
			{
				int likes = resouceItem.Likes;
				int dislikes = resouceItem.Dislikes;
				long itemId = model.id;
				return GenerateJsonSuccessResponse(new { itemId, likes, dislikes });
			}

			return GenerateJsonErrorResponse(400, "item not found");
		}

		[LoggedIn]
		[HttpGet]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameAddResources)]
		public IActionResult CreateCategory(long? parentId)
		{
			return View(new CreateCategoryModel(GetModelData(), parentId));
		}

		[HttpPost]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameAddResources)]
		public IActionResult CreateCategory(CreateCategoryModel model)
		{
			if (model == null)
				return RedirectToAction(nameof(Index));

			if (model.ParentId.HasValue)
			{
				if (_resourceProvider.GetAllResources(model.ParentId.Value).Where(r => r.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)).Any())
				{
					ModelState.AddModelError(nameof(model.Name), LanguageStrings.CategoryNameExists);
				}
			}
			else
			{
				if (_resourceProvider.GetAllResources().Where(r => r.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)).Any())
				{
					ModelState.AddModelError(nameof(model.Name), LanguageStrings.CategoryNameExists);
				}
			}

			if (!ModelState.IsValid)
				return View(new CreateCategoryModel(GetModelData(), model.ParentId, model.Name, model.Description));

			_resourceProvider.AddResourceCategory(UserId(), model.ParentId, model.Name, model.Description);
			throw new NotImplementedException();
			//return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		[Route("/Resources/View/{resourceItemId}/")]
		public IActionResult ViewResource(long resourceItemId)
		{
			ResourceItem resource = _resourceProvider.GetResourceItem(resourceItemId);

			if (resource == null)
				return RedirectToAction(nameof(Index));

			switch (resource.ResourceType)
			{
				case ResourceType.Uri:
				case ResourceType.TikTok:
				case ResourceType.YouTube:
					ResourceCategory resourceCategory = _resourceProvider.GetResourceCategory(resource.CategoryId);
					return RedirectToAction(nameof(ViewCategory), new { id = resource.CategoryId, categoryName = resourceCategory.RouteName });
			}

			_resourceProvider.IncrementViewCount(resource.Id);

			return View(CreateResourceViewItemModel(resource));
		}

		[HttpGet]
		[Route("/Resources/ViewExternal/{resourceItemId}/")]
		public IActionResult ViewExternalResource(long resourceItemId)
		{
			ResourceItem resource = _resourceProvider.GetResourceItem(resourceItemId);

			if (resource == null)
				return RedirectToAction(nameof(Index));

			string link = "";

			switch (resource.ResourceType)
			{
				case Middleware.ResourceType.YouTube:
					link = String.Format("https://www.youtube.com/watch?v={0}", resource.Value);
					break;

				case Middleware.ResourceType.TikTok:
					string[] parts = resource.Value.Split(';');
					link = String.Format("https://www.tiktok.com/@{0}/video/{1}", parts[0], parts[1]);
					break;

				case Middleware.ResourceType.Uri:
					link = resource.Value;
					break;

				case Middleware.ResourceType.Text:
				case Middleware.ResourceType.Image:
					link = String.Format("/Resources/View/{0}/", resource.Id);
					break;
			}

			_resourceProvider.IncrementViewCount(resource.Id);

			return Redirect(link);
		}

		#endregion Controller Action Methods

		#region Private Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ResourceCategoryModel CreateResourceCategoryViewModel(ResourceCategory resourceCategory)
		{
			if (resourceCategory == null)
				return null;

			List<ResourceItemModel> resources = new List<ResourceItemModel>();

			foreach (ResourceItem resourceItem in resourceCategory.ResourceItems)
			{
				resources.Add(new ResourceItemModel(resourceItem.Id, resourceCategory.Id,
					(ResourceType)resourceItem.ResourceType, resourceItem.UserId, 
					resourceItem.UserName, resourceCategory.Name,
					resourceItem.Description, resourceItem.Value, resourceItem.Likes,
					resourceItem.Dislikes, resourceItem.ViewCount, resourceItem.Approved));
			}

			List<ResourceCategoryModel> modelSubCategories = new List<ResourceCategoryModel>();

			List<ResourceCategory> subCategories = _resourceProvider.GetAllResources(resourceCategory.Id);

			foreach (ResourceCategory subCategory in subCategories)
				modelSubCategories.Add(CreateResourceCategoryViewModel(subCategory));

			ResourceCategoryModel Result = new ResourceCategoryModel(GetModelData(), resourceCategory.Id, resourceCategory.Name,
				resourceCategory.Description, resourceCategory.ForeColor, resourceCategory.BackColor,
				resourceCategory.Image, resourceCategory.RouteName, modelSubCategories, resources);

			Result.Breadcrumbs.Clear();
			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.Home, "/", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, "/Resources/", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(resourceCategory.Name, $"/Resources/Category/{resourceCategory.Id}/{resourceCategory.RouteName}/", false));

			if (resourceCategory.ParentId.HasValue)
			{
				ResourceCategory parent = _resourceProvider.GetResourceCategory(resourceCategory.ParentId.Value);
				do
				{
					Result.Breadcrumbs.Insert(2, new BreadcrumbItem(parent.Name, $"/Resources/Category/{parent.Id}/{parent.RouteName}/", false));
					parent = parent.ParentId.HasValue ? _resourceProvider.GetResourceCategory(parent.ParentId.Value) : null;
				} while (parent != null);
			}


			return Result;
		}

		private ResourcesModel CreateResourcesModel(List<ResourceCategory> resourceList)
		{
			List<ResourceCategoryModel> resources = new List<ResourceCategoryModel>();

			foreach (ResourceCategory row in resourceList)
			{
				resources.Add(new ResourceCategoryModel(row.Id, row.Name, row.Description, row.ForeColor, row.BackColor, row.Image, row.RouteName));
			}

			return new ResourcesModel(GetModelData(), resources);
		}

		private ResourceViewItemModel CreateResourceViewItemModel(ResourceItem resourceItem)
		{
			ResourceCategory resourceCategory = _resourceProvider.GetResourceCategory(resourceItem.CategoryId);

			ResourceViewItemModel Result = new ResourceViewItemModel(GetModelData(), resourceItem.Id, resourceItem.CategoryId,
				(ResourceType)resourceItem.ResourceType, resourceItem.UserId,
				resourceItem.UserName, resourceCategory.Name,
				resourceItem.Description, resourceItem.Value, resourceItem.Likes,
				resourceItem.Dislikes, resourceItem.ViewCount, resourceItem.Approved);

			Result.Breadcrumbs.Clear();
			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.Home, "/", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, "/Resources/", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(resourceCategory.Name, $"/Resources/Category/{resourceCategory.Id}/{resourceCategory.RouteName}/", false));

			if (resourceCategory.ParentId.HasValue)
			{
				ResourceCategory parent = _resourceProvider.GetResourceCategory(resourceCategory.ParentId.Value);
				do
				{
					Result.Breadcrumbs.Insert(2, new BreadcrumbItem(parent.Name, $"/Resources/Category/{parent.Id}/{parent.RouteName}/", false));
					parent = parent.ParentId.HasValue ? _resourceProvider.GetResourceCategory(parent.ParentId.Value) : null;
				} while (parent != null);
			}

			Result.Breadcrumbs.Add(new BreadcrumbItem(resourceItem.Name, $"/Resources/View/{resourceItem.Id}/", false));

			return Result;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591