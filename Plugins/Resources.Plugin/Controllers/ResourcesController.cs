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

			ResourceCategory resourceCategory = _resourceProvider.GetResourceFromRouteName(categoryName);

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
					resourceItem.Dislikes, resourceItem.Approved));
			}

			ResourceCategoryModel Result = new ResourceCategoryModel(GetModelData(), resourceCategory.Id, resourceCategory.Name,
				resourceCategory.Description, resourceCategory.ForeColor, resourceCategory.BackColor,
				resourceCategory.Image, resourceCategory.RouteName, resources);

			Result.Breadcrumbs.Clear();
			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.Home, "/", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, "/Resources/", false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(resourceCategory.Name, $"/Resources/Category/{resourceCategory.Id}/{resourceCategory.RouteName}/", false));

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

		#endregion Private Methods
	}
}

#pragma warning restore CS1591