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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Languages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

		private readonly static string[] SupportedImageTypes = { ".apng", ".gif", ".ico", ".jpeg", ".jpg", ".png", ".svg" };
		private readonly static string[] ReplacableTextFrom = { "<", ">" };
		private readonly static string[] ReplacableTextTo = { "&lt;", "&gt;" };

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
		public const int MaximumNameLength = 30;
		public const int MinimumNameLength = 5;
		public const int MinimumDescriptionLength = 15;
		public const int MaximumDescriptionLength = 100;
		private const string UserAgentHeader = "User-Agent";
		private const string UserAgentValue = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36";
		private const string ResourcesBreadcrumb = "/Resources/";
		private const string TikTokBaseUri = "https://www.tiktok.com/";
		private const string YouTubeImgBaseUri = "https://img.youtube.com/";

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

			return GenerateJsonErrorResponse(SharedPluginFeatures.Constants.HtmlResponseBadRequest, LanguageStrings.NotFound404);
		}

		[LoggedIn]
		[HttpGet]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameAddResources)]
		[Route("Resources/CreateCategory")]
		[Route("Resources/CreateCategory/{parentId}")]
		public IActionResult CreateCategory(long parentId)
		{
			BaseModelData baseModelData = GetModelData();

			baseModelData.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, ResourcesBreadcrumb, false));
			baseModelData.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.CreateCategory, $"/Resources/CreateCategory/{parentId}/", false));

			return View(new CreateCategoryModel(baseModelData, parentId));
		}

		[HttpPost]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameAddResources)]
		public IActionResult CreateCategory(CreateCategoryModel model)
		{
			if (model == null)
				return RedirectToAction(nameof(Index));

			if (_resourceProvider.GetAllResources(model.ParentId).Where(r => r.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase)).Any())
			{
				ModelState.AddModelError(nameof(model.Name), LanguageStrings.CategoryNameExists);
			}

			if (!ModelState.IsValid)
				return View(new CreateCategoryModel(GetModelData(), model.ParentId, model.Name, model.Description));

			ResourceCategory newCategory =_resourceProvider.AddResourceCategory(UserId(), model.ParentId, model.Name, model.Description);

			if (UserCanManageResources())
				return RedirectToAction(nameof(ManageCategories));

			return RedirectToAction(nameof(CategorySubmitted));
		}

		[LoggedIn]
		[HttpGet]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameManageResources)]
		[Breadcrumb(nameof(LanguageStrings.ManageCategories), Name, nameof(Index))]
		public IActionResult ManageCategories()
		{
			return View(CreateManageCategoryModel());
		}

		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.CategorySubmitted), Name, nameof(Index))]
		public IActionResult CategorySubmitted()
		{
			return View(new BaseModel(GetModelData()));
		}

		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.CategorySubmitted), Name, nameof(Index))]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameManageResources)]
		public IActionResult CategoryEdit(long id)
		{
			ResourceCategory category = _resourceProvider.GetResourceCategory(id);

			if (category == null)
				return RedirectToAction(nameof(ManageCategories));

			ResourceEditCategoryModel model = CreateResourceEditCategoryViewModel(category);

			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameManageResources)]
		public IActionResult CategoryEdit(ResourceEditCategoryModel model)
		{
			if (model == null)
				return RedirectToAction(nameof(ManageCategories));

			if (_resourceProvider.GetAllResources(model.ParentId)
				.Where(r => r.Id != model.Id && r.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
				.Any())
			{
				ModelState.AddModelError(nameof(model.Name), LanguageStrings.CategoryNameExists);
			}

			if (!ModelState.IsValid)
			{
				ResourceCategoryModel resourceCategoryModel = new ResourceCategoryModel(model.Id, model.Name, model.Description, model.ForeColor, 
					model.BackColor, model.Image, model.RouteName, model.IsVisible, model.ParentId);
				return View(resourceCategoryModel);
			}

			_resourceProvider.UpdateResourceCategory(UserId(), 
				new ResourceCategory(model.Id, model.ParentId, model.Name, 
				model.Description, model.ForeColor, model.BackColor, 
				model.Image, model.RouteName, model.IsVisible));

			GrowlAdd(String.Format(LanguageStrings.CategoryUpdated, model.Name));

			return RedirectToAction(nameof(ManageCategories));
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

		public IActionResult ResourceItemSubmitted()
		{
			return View(GetModelData());
		}

		[LoggedIn]
		[HttpGet]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameAddResources)]
		[Route("/Resources/CreateResourceItem/{parentCategory}/")]
		public IActionResult CreateResourceItem(long parentCategory)
		{
			ResourceCategory resourceCategory = _resourceProvider.GetResourceCategory(parentCategory);

			if (resourceCategory == null)
				return RedirectToAction(nameof(Index));

			BaseModelData baseModelData = GetModelData();

			baseModelData.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, ResourcesBreadcrumb, false));
			baseModelData.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.CreateCategory, $"/Resources/CreateCategory/{parentCategory}/", false));

			return View(new CreateResourceItemModel(baseModelData, parentCategory));
		}

		[LoggedIn]
		[HttpPost]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameAddResources)]
		public IActionResult CreateResourceItem(CreateResourceItemModel model)
		{
			if (model == null)
				return RedirectToAction(nameof(Index));

			ResourceCategory resourceCategory = _resourceProvider.GetResourceCategory(model.ParentId);

			if (resourceCategory == null)
				return RedirectToAction(nameof(Index));

			if (String.IsNullOrEmpty(model.Name) || model.Name.Length < MinimumNameLength || model.Name.Length > MaximumNameLength)
				ModelState.AddModelError(nameof(model.Name), String.Format(LanguageStrings.InvalidName, MinimumNameLength, MaximumNameLength));

			if (String.IsNullOrEmpty(model.Description) || model.Description.Length < MinimumDescriptionLength || model.Description.Length > MaximumDescriptionLength)
				ModelState.AddModelError(nameof(model.Description), String.Format(LanguageStrings.InvalidDescription, MinimumDescriptionLength, MaximumDescriptionLength));

			ValidateResourceType(model);

			if (ModelState.IsValid)
			{
				_resourceProvider.AddResourceItem(model.ParentId, (ResourceType)model.ResourceType, UserId(), UserName(),
					model.Name, model.Description, model.Value, false, ValidateAndCleanTags(model.Tags));

				if (UserCanManageResources())
					return RedirectToAction(nameof(ManageResourceItems));

				return RedirectToAction(nameof(ResourceItemSubmitted));
			}

			BaseModelData baseModelData = GetModelData();

			baseModelData.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, ResourcesBreadcrumb, false));
			baseModelData.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.CreateCategory, $"/Resources/CreateCategory/{model.ParentId}/", false));

			CreateResourceItemModel resultModel = new CreateResourceItemModel(baseModelData, model.ParentId)
			{
				Name = model.Name,
				Description = model.Description,
				Value = model.Value,
				ResourceType = model.ResourceType,
			};

			return View(resultModel);
		}

		[LoggedIn]
		[HttpGet]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameManageResources)]
		[Breadcrumb(nameof(Languages.LanguageStrings.ManageCategories), Name, nameof(Index))]
		public IActionResult ManageResourceItems()
		{
			return View(CreateManageResourceItemModel());
		}

		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.ResourceItemSubmitted), Name, nameof(Index))]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameManageResources)]
		public IActionResult ResourceItemEdit(long id)
		{
			ResourceItem resourceItem = _resourceProvider.GetResourceItem(id);

			if (resourceItem == null)
				return RedirectToAction(nameof(ManageResourceItems));

			List<NameIdModel> allCategories = new List<NameIdModel>();

			_resourceProvider.RetrieveAllCategories().ForEach(c =>
			{
				allCategories.Add(new NameIdModel(c.Id, c.Name));
			});

			ResourceEditResourceItemModel model =  new ResourceEditResourceItemModel(GetModelData(), resourceItem.Id, 
				resourceItem.CategoryId, resourceItem.ResourceType, resourceItem.UserId, resourceItem.UserName, 
				resourceItem.Name, resourceItem.Description, resourceItem.Value, resourceItem.Approved,
				String.Join(SharedPluginFeatures.Constants.NewLineChar, resourceItem.Tags), allCategories);

			return View(model);
		}

		[HttpPost]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameManageResources)]
		public IActionResult ResourceItemEdit(ResourceEditResourceItemModel model)
		{
			if (model == null)
				return RedirectToAction(nameof(ManageResourceItems));

			ValidateResourceEditResourceItemModel(model);

			if (!ModelState.IsValid)
			{
				List<NameIdModel> allCategories = new List<NameIdModel>();

				_resourceProvider.RetrieveAllCategories().ForEach(c =>
				{
					allCategories.Add(new NameIdModel(c.Id, c.Name));
				});

				ResourceEditResourceItemModel resourceCategoryModel = new ResourceEditResourceItemModel(GetModelData(), model.Id, 
					model.CategoryId, model.ResourceType, model.UserId, model.UserName, model.Name, model.Description,
					model.Value, model.Approved, model.Tags, allCategories);

				return View(resourceCategoryModel);
			}

			ResourceItem existingResourceItem = _resourceProvider.GetResourceItem(model.Id);

			_resourceProvider.UpdateResourceItem(UserId(),
				new ResourceItem(model.Id, model.CategoryId, model.ResourceType, existingResourceItem.UserId, model.UserName, 
				model.Name, model.Description, model.Value, existingResourceItem.Likes, existingResourceItem.Dislikes,
				existingResourceItem.ViewCount, model.Approved, ValidateAndCleanTags(model.Tags)));

			GrowlAdd(String.Format(LanguageStrings.ResourceItemUpdated, model.Name));

			return RedirectToAction(nameof(ManageResourceItems));
		}

		[LoggedIn]
		[AjaxOnly]
		[HttpPost]
		public JsonResult ToggleBookmark(ItemResponseModel model)
		{
			ResourceItem resouceItem = _resourceProvider.GetResourceItem(model.id);

			if (resouceItem != null)
			{
				BookmarkActionResult bookmarkResult = _resourceProvider.ToggleResourceBookmark(UserId(), resouceItem);

				string growlMessage;

				switch (bookmarkResult)
				{
					case BookmarkActionResult.Removed:
						growlMessage = LanguageStrings.BookmarkRemoved;
						break;
					case BookmarkActionResult.Added:
						growlMessage = LanguageStrings.BookmarkAdded;
						break;
					case BookmarkActionResult.QuotaExceeded:
						growlMessage = LanguageStrings.BookmarkQuotaExceeded;
						break;
					default:
						growlMessage = LanguageStrings.NotFound404;
						break;
				}

				return GenerateJsonSuccessResponse(String.Format(growlMessage, resouceItem.Name));
			}

			return GenerateJsonErrorResponse(SharedPluginFeatures.Constants.HtmlResponseBadRequest, LanguageStrings.NotFound404);
		}

		[LoggedIn]
		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.ViewBookmarks), Name, nameof(Index))]
		public IActionResult ViewBookmarks(long? id)
		{
			if (id.HasValue)
			{
				ResourceItem resouceItem = _resourceProvider.GetResourceItem(id.Value);
				_ = _resourceProvider.ToggleResourceBookmark(UserId(), resouceItem);
			}

			List<ResourceItem> resourceBookmarks = _resourceProvider.RetrieveUserBookmarks(UserId());
			
			List<NameIdModel> nameIdModels = new List<NameIdModel>();

			resourceBookmarks.ForEach(rb => nameIdModels.Add(new NameIdModel(rb.Id, rb.Name)));

			return View(new ViewBookmarksModel(GetModelData(), nameIdModels));
		}

		#endregion Controller Action Methods

		#region Private Methods

		private static List<string> ValidateAndCleanTags(string tags)
		{
			List<string> result = new();

			if (String.IsNullOrEmpty(tags))
				return result;

			string[] tagList = tags.Split(SharedPluginFeatures.Constants.NewLineChar,
#if NET_5_ABOVE
				StringSplitOptions.TrimEntries | 
#endif
				StringSplitOptions.RemoveEmptyEntries);

			for (int i = 0; i < tagList.Length; i++)
			{
				result.Add(Middleware.Utils.RemoveInvalidTagChars(tagList[i]));
			}

			return result;
		}

		private void ValidateResourceEditResourceItemModel(ResourceEditResourceItemModel model)
		{
			if (_resourceProvider.GetResourceCategory(model.CategoryId) == null)
			{
				ModelState.AddModelError(String.Empty, LanguageStrings.CategoryNotFound);
			}

			if (model.Name.Length < MinimumNameLength || model.Name.Length > MaximumNameLength)
			{
				ModelState.AddModelError(nameof(model.Name), 
					String.Format(LanguageStrings.InvalidName, MinimumNameLength, MaximumNameLength));
			}

			if (model.Description.Length < MinimumDescriptionLength || model.Description.Length > MaximumDescriptionLength)
			{
				ModelState.AddModelError(nameof(model.Description), 
					String.Format(LanguageStrings.InvalidDescription, MinimumDescriptionLength, MaximumDescriptionLength));
			}

			if (String.IsNullOrWhiteSpace(model.Value))
			{
				ModelState.AddModelError(nameof(model.Value), LanguageStrings.TextInvalidEmpty);
			}
		}

		private bool UserCanManageResources()
		{
			return Request.HttpContext.User.HasClaim(SharedPluginFeatures.Constants.ClaimNameManageResources, "true");
		}

		private void ValidateResourceType(CreateResourceItemModel model)
		{
			ResourceType resourceType = (ResourceType)model.ResourceType;

			switch (resourceType) 
			{
				case ResourceType.Text:
					ValidateResourceText(model);
					break;

				case ResourceType.Image:
					ValidateResourceImage(model.Value);
					break;

				case ResourceType.TikTok:
					ValidateResourceTikTok(model.Value);
					break;

				case ResourceType.YouTube:
					ValidateResourceYoutube(model.Value);
					break;

				case ResourceType.Uri:
					ValiddateResourceUri(model.Value);
					break;

				default:
					throw new InvalidOperationException();
			}
		}

		private void ValiddateResourceUri(string value)
		{
			try
			{
				Uri uri = new Uri(value, UriKind.Absolute);
				if (!uri.IsAbsoluteUri)
				{
					ModelState.AddModelError(String.Empty, LanguageStrings.InvalidUriNotAbsolute);
					return;
				}

				string baseAddress = value[..(value.Length - uri.LocalPath.Length + 1)];

				using HttpClient httpClient = CreateHttpClient(baseAddress);
				using HttpResponseMessage response = httpClient.GetAsync(uri.LocalPath[1..]).GetAwaiter().GetResult();

				if (response == null || response.StatusCode == HttpStatusCode.NotFound)
					ModelState.AddModelError(String.Empty, String.Format(LanguageStrings.InvalidUriNotFound, value));
			}
			catch (UriFormatException)
			{
				ModelState.AddModelError(String.Empty, String.Format(LanguageStrings.InvalidUriNotAbsolute));
				return;
			}
		}

		private void ValidateResourceYoutube(string value)
		{
			Match match = Regex.Match(value, "[a-zA-Z0-9_-]");

			if (!match.Success || String.IsNullOrEmpty(value))
			{
				ModelState.AddModelError(String.Empty, String.Format(LanguageStrings.InvalidYouTubeId, value));
				return;
			}

			using HttpClient httpClient = CreateHttpClient(YouTubeImgBaseUri);
			using HttpResponseMessage response = httpClient.GetAsync($"vi/{value}/mqdefault.jpg").GetAwaiter().GetResult();

			if (response == null || response.StatusCode == HttpStatusCode.NotFound)
				ModelState.AddModelError(String.Empty, String.Format(LanguageStrings.InvalidYouTubeId, value));
		}

		private void ValidateResourceTikTok(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				ModelState.AddModelError(String.Empty, LanguageStrings.InvalidTickTokIdEmpty);
				return;
			}

			string[] parts = value.Split(new char[] { SharedPluginFeatures.Constants.ColonChar }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length != 2 || String.IsNullOrEmpty(parts[0]) || String.IsNullOrEmpty(parts[1]))
			{
				ModelState.AddModelError(String.Empty, LanguageStrings.InvalidTickTokId);
				return;
			}

			using HttpClient httpClient = CreateHttpClient(TikTokBaseUri);
			using HttpResponseMessage response = httpClient.GetAsync($"{parts[0]}/video/{parts[1]}?is_copy_url=1&is_from_webapp=v1").GetAwaiter().GetResult();

			if (response == null || response.StatusCode != HttpStatusCode.OK)
				ModelState.AddModelError(String.Empty, String.Format(LanguageStrings.InvalidTickTokId, value));
		}

		private void ValidateResourceImage(string value)
		{
			try
			{
				Uri uri = new Uri(value, UriKind.Absolute);
				if (!uri.IsAbsoluteUri)
				{
					ModelState.AddModelError(String.Empty, LanguageStrings.InvalidImageNotAbsolute);
					return;
				}

				string extension = Path.GetExtension(value).ToLower();

				if (!SupportedImageTypes.Contains(extension))
				{
					ModelState.AddModelError(String.Empty, 
						String.Format(LanguageStrings.InvalidImageNotSupported, 
						extension, 
						String.Join(SharedPluginFeatures.Constants.CommaChar, SupportedImageTypes)));
					return;
				}

				string baseAddress = value[..(value.Length - uri.LocalPath.Length + 1)];

				using HttpClient httpClient = CreateHttpClient(baseAddress);
				using HttpResponseMessage response = httpClient.GetAsync(uri.LocalPath[1..]).GetAwaiter().GetResult();

				if (response == null || response.StatusCode == HttpStatusCode.NotFound)
					ModelState.AddModelError(String.Empty, String.Format(LanguageStrings.InvalidImageNotFound, value));
			}
			catch (UriFormatException)
			{
				ModelState.AddModelError(String.Empty, String.Format(LanguageStrings.InvalidImageNotAbsolute));
				return;
			}
		}

		private void ValidateResourceText(CreateResourceItemModel model)
		{
			if (String.IsNullOrWhiteSpace(model.Value))
			{
				ModelState.AddModelError(String.Empty, LanguageStrings.TextInvalidEmpty);
				return;
			}

			for (int i = 0; i < ReplacableTextFrom.Length; i++)
			{
				model.Value = model.Value.Replace(ReplacableTextFrom[i], ReplacableTextTo[i]);
			}

			model.Value = model.Value.Trim();
		}

		private static HttpClient CreateHttpClient(string baseAddress)
		{
			HttpClient httpClient = new HttpClient
			{
				BaseAddress = new Uri(baseAddress)
			};

			httpClient.DefaultRequestHeaders.Add(UserAgentHeader, UserAgentValue);

			return httpClient;
		}

		private ManageCategoryModel CreateManageCategoryModel()
		{
			List<ResourceCategoryModel> resources = new List<ResourceCategoryModel>();

			foreach (ResourceCategory row in _resourceProvider.RetrieveAllCategories())
			{
				resources.Add(new ResourceCategoryModel(row.Id, row.Name, row.Description, row.ForeColor, row.BackColor, 
					row.Image, row.RouteName, row.IsVisible, row.ParentId));
			}

			return new ManageCategoryModel(GetModelData(), GrowlGet(), resources);
		}

		private ManageResourceItemModel CreateManageResourceItemModel()
		{
			List<ResourceItemModel> resources = new List<ResourceItemModel>();

			foreach (ResourceItem row in _resourceProvider.RetrieveAllResourceItems())
			{
				resources.Add(new ResourceItemModel(row.Id, row.CategoryId, row.ResourceType, row.UserId, row.UserName,
					row.Name, row.Description, row.Value, row.Likes, row.Dislikes, row.ViewCount, row.Approved));
			}

			return new ManageResourceItemModel(GetModelData(), GrowlGet(), resources);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ResourceEditCategoryModel CreateResourceEditCategoryViewModel(ResourceCategory resourceCategory)
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

			List<NameIdModel> allCategories = new List<NameIdModel>()
			{
				new NameIdModel(0, LanguageStrings.NoParent)
			};

			_resourceProvider.RetrieveAllCategories().ForEach(c =>
			{
				if (c.Id != resourceCategory.Id)
					allCategories.Add(new NameIdModel(c.Id, c.Name));
			});

			ResourceEditCategoryModel Result = new ResourceEditCategoryModel(GetModelData(), resourceCategory.Id, resourceCategory.Name,
				resourceCategory.Description, resourceCategory.ForeColor, resourceCategory.BackColor,
				resourceCategory.Image, resourceCategory.RouteName, resourceCategory.IsVisible, resourceCategory.ParentId,
				allCategories);

			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, ResourcesBreadcrumb, false));

			return Result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private ResourceCategoryModel CreateResourceCategoryViewModel(ResourceCategory resourceCategory)
		{
			if (resourceCategory == null)
				return null;

			List<ResourceItemModel> resources = new List<ResourceItemModel>();

			foreach (ResourceItem resourceItem in resourceCategory.ResourceItems.OrderBy(rc => rc.Name))
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
				resourceCategory.Image, resourceCategory.RouteName, resourceCategory.IsVisible, resourceCategory.ParentId, 
				modelSubCategories, resources);

			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, ResourcesBreadcrumb, false));

			Result.Breadcrumbs.Add(new BreadcrumbItem(resourceCategory.Name, $"/Resources/Category/{resourceCategory.Id}/{resourceCategory.RouteName}/", false));

			BuildCategoryBreadCrumbs(resourceCategory, Result.Breadcrumbs);

			return Result;
		}

		private ResourcesModel CreateResourcesModel(List<ResourceCategory> resourceList)
		{
			List<ResourceCategoryModel> resources = new List<ResourceCategoryModel>();

			foreach (ResourceCategory row in resourceList)
			{
				resources.Add(new ResourceCategoryModel(row.Id, row.Name, row.Description, row.ForeColor, row.BackColor, row.Image, 
					row.RouteName, row.IsVisible, row.ParentId));
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
				resourceItem.Dislikes, resourceItem.ViewCount, resourceItem.Approved, 
				resourceItem.Tags);

			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ResourcesMain, ResourcesBreadcrumb, false));
			Result.Breadcrumbs.Add(new BreadcrumbItem(resourceCategory.Name, $"/Resources/Category/{resourceCategory.Id}/{resourceCategory.RouteName}/", false));

			BuildCategoryBreadCrumbs(resourceCategory, Result.Breadcrumbs);

			Result.Breadcrumbs.Add(new BreadcrumbItem(resourceItem.Name, $"/Resources/View/{resourceItem.Id}/", false));

			return Result;
		}

		private void BuildCategoryBreadCrumbs(ResourceCategory resourceCategory, List<BreadcrumbItem> breadcrumbItems)
		{
			if (resourceCategory.ParentId > 0)
			{
				ResourceCategory parent = _resourceProvider.GetResourceCategory(resourceCategory.ParentId);
				do
				{
					breadcrumbItems.Insert(2, new BreadcrumbItem(parent.Name, $"/Resources/Category/{parent.Id}/{parent.RouteName}/", false));
					parent = parent.ParentId > 0 ? _resourceProvider.GetResourceCategory(parent.ParentId) : null;
				} while (parent != null);
			}
		}

#endregion Private Methods
	}
}

#pragma warning restore CS1591