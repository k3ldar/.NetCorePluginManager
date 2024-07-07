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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Image Manager Plugin
 *  
 *  File: TempErrorManager.cs
 *
 *  Date        Name                Reason
 *  15/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

using ImageManager.Plugin.Models;

using Languages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

using Middleware.Images;
using Middleware.Interfaces;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace ImageManager.Plugin.Controllers
{
	[DenySpider]
	[LoggedIn]
	[Authorize(Policy = Constants.PolicyNameViewImageManager)]

	public class ImageManagerController : BaseController
	{
		#region Public / Private Consts

		public const string Name = nameof(ImageManager);

		private const string ErrorInvalidModel = "Invalid Model";
		private const string ErrorInvalidImageName = "Invalid ImageName";
		private const string ErrorInvalidGroupName = "Invalid GroupName";
		private const string ErrorInvalidSubgroupName = "Invalid SubgroupName";
		private const string ErrorUnableToDeleteImage = "Unable to delete image";
		private const string ErrorNoConfirmation = "Confirmation required";
		private const string ErrorInvalidImageCache = "Image cache not found";
		private const string EmptyUploadCacheName = "Not Cached";

		#endregion Public / Private Consts

		#region Private Members

		private static int _uploadId = 0;
		private readonly IImageProvider _imageProvider;
		private readonly INotificationService _notificationService;
		private readonly IMemoryCache _memoryCache;
		private readonly IVirusScanner _virusScanner;

		#endregion Private Members

		#region Constructors

		public ImageManagerController(IImageProvider imageProvider,
			INotificationService notificationService,
			IMemoryCache memoryCache,
			IVirusScanner virusScanner)
		{
			_imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));
			_notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
			_memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
			_virusScanner = virusScanner ?? throw new ArgumentNullException(nameof(virusScanner));
		}

		#endregion Constructors

		#region Public Action Methods

		[HttpGet]
		public IActionResult Index()
		{
			return View(CreateImagesViewModel(String.Empty, String.Empty, String.Empty));
		}

		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.ViewGroup), nameof(Index), HasParams = true)]
		[Route("ImageManager/ViewGroup/{groupName}")]
		public IActionResult ViewGroup(string groupName)
		{
			if (String.IsNullOrEmpty(groupName))
				return RedirectToAction(nameof(Index));

			if (!_imageProvider.Groups().ContainsKey(groupName))
				return RedirectToAction(nameof(Index));

			return View("/Views/ImageManager/Index.cshtml", CreateImagesViewModel(groupName, String.Empty, String.Empty));
		}

		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.ViewSubgroup), nameof(ViewGroup), HasParams = true)]
		[Route("ImageManager/ViewSubgroup/{groupName}/{subgroupName}")]
		public IActionResult ViewSubgroup(string groupName, string subgroupName)
		{
			if (String.IsNullOrEmpty(groupName))
				return RedirectToAction(nameof(Index));

			Dictionary<string, List<string>> groups = _imageProvider.Groups();

			if (!groups.TryGetValue(groupName, out List<string> subGroups))
				return RedirectToAction(nameof(Index));

			if (String.IsNullOrEmpty(subgroupName))
				return RedirectToAction(nameof(Index));

			if (!subGroups.Contains(subgroupName))
				return RedirectToAction(nameof(Index));

			return View("/Views/ImageManager/Index.cshtml", CreateImagesViewModel(groupName, subgroupName, String.Empty));
		}

		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.ViewImage), nameof(ViewGroup), HasParams = true)]
		[Route("ImageManager/ViewImage/{groupName}/{imageName}")]
		public IActionResult ViewImage(string groupName, string imageName)
		{
			if (String.IsNullOrEmpty(groupName))
				return RedirectToAction(nameof(Index));

			if (String.IsNullOrEmpty(imageName))
				return RedirectToAction(nameof(Index));

			return View(CreateImagesViewModel(groupName, String.Empty, ReplaceLastDash(imageName)));
		}

		[HttpGet]
		[Breadcrumb(nameof(LanguageStrings.ViewImage), nameof(ViewGroup), HasParams = true)]
		[Route("ImageManager/ViewSubgroupImage/{groupName}/{subgroupName}/{imageName}")]
		public IActionResult ViewSubgroupImage(string groupName, string subgroupName, string imageName)
		{
			if (String.IsNullOrEmpty(groupName))
				return RedirectToAction(nameof(Index));

			if (String.IsNullOrEmpty(subgroupName))
				return RedirectToAction(nameof(Index));

			if (String.IsNullOrEmpty(imageName))
				return RedirectToAction(nameof(Index));

			return View("/Views/ImageManager/ViewImage.cshtml", CreateImagesViewModel(groupName, subgroupName, ReplaceLastDash(imageName)));
		}

		[HttpPost]
		[AjaxOnly]
		[Authorize(Policy = Constants.PolicyNameImageManagerManage)]
		public IActionResult DeleteImage([FromBody] DeleteImageModel model)
		{
			if (!ValidModel(model, out JsonResult response))
				return response;

			if (!ValidateImageExists(model, out response))
				return response;


			bool imageDeleted;

			if (String.IsNullOrEmpty(model.SubgroupName))
				imageDeleted = _imageProvider.ImageDelete(model.GroupName, model.ImageName);
			else
				imageDeleted = _imageProvider.ImageDelete(model.GroupName, model.SubgroupName, model.ImageName);

			if (imageDeleted)
				return GenerateJsonSuccessResponse();
			else
				return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorUnableToDeleteImage);
		}

		[HttpPost]
		[Authorize(Policy = Constants.PolicyNameImageManagerManage)]
		public IActionResult UploadImage(UploadImageModel model)
		{
			if (model == null || model.Files == null || model.Files.Count == 0)
				return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidModel);

			if (String.IsNullOrEmpty(model.GroupName))
				return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidGroupName);

			CachedImageUpload cachedImageUpload = new(model.GroupName, model.SubgroupName);

			foreach (IFormFile formFile in model.Files)
			{
				if (formFile.Length > 0)
				{
					if (!FileExtensionAccepted(formFile))
					{
						ModelState.AddModelError(String.Empty, $"{LanguageStrings.InvalidFileType} {formFile.FileName}");
					}
					else
					{
						string filePath = _imageProvider.TemporaryImageFile(Path.GetExtension(formFile.FileName));

						using (FileStream stream = System.IO.File.Create(filePath))
						{
							formFile.CopyTo(stream);
							cachedImageUpload.Files.Add(filePath);
						}
					}
				}
			}

			_virusScanner.ScanFile(cachedImageUpload.Files.ToArray());
			string cacheName = GetCacheId();

			_memoryCache.GetCache().Add(cacheName, new CacheItem(cacheName, cachedImageUpload));

			return View("/Views/ImageManager/ImageUpload.cshtml", CreateImageUploadViewModel(model.GroupName, model.SubgroupName, null, cacheName));
		}

		[HttpPost]
		[Authorize(Policy = Constants.PolicyNameImageManagerManage)]
		[AjaxOnly]
		public IActionResult ProcessImage([FromBody] ImageProcessViewModel model)
		{
			if (model == null)
				return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidModel);

			if (model.FileUploadId == null)
				return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidImageCache);

			CacheItem uploadCache = _memoryCache.GetCache().Get(model.FileUploadId);

			if (uploadCache == null)
				return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidImageCache);

			if (uploadCache.Value is not CachedImageUpload cachedImageUpload)
				return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidImageCache);

			object notificationResponse = null;

			_notificationService.RaiseEvent(Constants.NotificationEventImageUploaded, cachedImageUpload, model.AdditionalData, ref notificationResponse);

			if (notificationResponse == null)
			{
				// custom processing has not taken place, move files to correct location and report success
				foreach (string file in cachedImageUpload.Files)
				{
					if (System.IO.File.Exists(file))
					{
						byte[] fileContents = System.IO.File.ReadAllBytes(file);
						_imageProvider.AddFile(cachedImageUpload.GroupName, cachedImageUpload.SubgroupName, Path.GetFileName(file), fileContents);
					}
				}
			}

			_memoryCache.GetCache().Remove(uploadCache);

			foreach (string file in cachedImageUpload.Files)
			{
				if (System.IO.File.Exists(file))
					System.IO.File.Delete(file);
			}

			string successUri = "";

			if (String.IsNullOrWhiteSpace(cachedImageUpload.SubgroupName))
				successUri = $"/{Name}/ViewGroup/{cachedImageUpload.GroupName}";
			else
				successUri += $"/{Name}/ViewSubgroup/{cachedImageUpload.GroupName}/{cachedImageUpload.SubgroupName}";

			return GenerateJsonSuccessResponse(new { uri = successUri });
		}

		[HttpGet]
		[AjaxOnly]
		public IActionResult ImageTemplateEditor(string data)
		{
			return PartialView("/Views/ImageManager/_ImageTemplateEditor.cshtml", new ImageTemplateEditorModel(_imageProvider, HttpUtility.HtmlDecode(data)));
		}

		[HttpPost]
		[AjaxOnly]
		public IActionResult ImageTemplateEditorSubGroups(string groupName, string subgroupName)
		{
			if (string.IsNullOrEmpty(groupName))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, ErrorInvalidGroupName);

			if (!_imageProvider.GroupExists(groupName))
				return GenerateJsonErrorResponse(HtmlResponseBadRequest, ErrorInvalidGroupName);

			List<string> subgroupNames = _imageProvider.Groups()[groupName];
			List<ImageFile> images = null;

			if (String.IsNullOrEmpty(subgroupName))
				images = _imageProvider.Images(groupName);
			else
				images = _imageProvider.Images(groupName, subgroupName);

			List<string> imageNames = [];

			images.ForEach(i => imageNames.Add(i.Name));

			return GenerateJsonSuccessResponse(new RetrieveImagesModel(subgroupNames, imageNames));
		}

		#endregion Public Action Methods

		#region Private Methods

		private static bool FileExtensionAccepted(IFormFile formFile)
		{
			string extension = Path.GetExtension(formFile.FileName).ToLower();

			switch (extension)
			{
				case Constants.FileExtensionApng:
				case Constants.FileExtensionAvif:
				case Constants.FileExtensionGif:
				case Constants.FileExtensionJpg:
				case Constants.FileExtensionJpeg:
				case Constants.FileExtensionPng:
				case Constants.FileExtensionSvg:
				case Constants.FileExtensionWebP:
					return true;
			}

			return false;
		}

		private string GetCacheId()
		{
			string Result;

			do
			{
				Result = $"ImageManager - {DateTime.UtcNow.Ticks:X}-{_uploadId++}";
			}
			while (_memoryCache.GetCache().Get(Result) != null);

			return Result;
		}

		private bool ValidateImageExists(DeleteImageModel model, out JsonResult invalidResponse)
		{
			bool imageExists;
			model.ImageName = ReplaceLastDash(model.ImageName);

			if (String.IsNullOrEmpty(model.SubgroupName))
			{
				imageExists = _imageProvider.ImageExists(model.GroupName, model.ImageName);
			}
			else
			{
				imageExists = _imageProvider.ImageExists(model.GroupName, model.SubgroupName, model.ImageName);
			}

			if (!imageExists)
				invalidResponse = GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidImageName);
			else
				invalidResponse = null;

			return imageExists;
		}

		private bool ValidModel(DeleteImageModel model, out JsonResult invalidResponse)
		{
			invalidResponse = null;

			if (model == null)
			{
				invalidResponse = GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidModel);
			}
			else
			{
				if (!model.ConfirmDelete)
					invalidResponse = GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorNoConfirmation);

				if (invalidResponse == null && String.IsNullOrEmpty(model.ImageName))
					invalidResponse = GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidImageName);

				if (invalidResponse == null && (String.IsNullOrEmpty(model.GroupName) || !_imageProvider.GroupExists(model.GroupName)))
					invalidResponse = GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidGroupName);

				if (invalidResponse == null && !String.IsNullOrEmpty(model.SubgroupName) && !_imageProvider.SubgroupExists(model.GroupName, model.SubgroupName))
					invalidResponse = GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidSubgroupName);
			}

			return invalidResponse == null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string ReplaceLastDash(string s)
		{
			StringBuilder Result = new(s);

			for (int i = s.Length - 1; i >= 0; i--)
			{
				if (Result[i] == '-')
				{
					Result[i] = '.';
					break;
				}
			}

			return Result.ToString();
		}


		private ImagesViewModel CreateImagesViewModel(string groupName, string subgroupName, string imageName)
		{
			return CreateImageUploadViewModel(groupName, subgroupName, imageName, EmptyUploadCacheName) as ImagesViewModel;
		}

		private ProcessImagesViewModel CreateImageUploadViewModel(string groupName, string subgroupName, string imageName, string memoryCacheName)
		{
			List<ImageFile> images = null;

			if (String.IsNullOrEmpty(subgroupName))
				images = _imageProvider.Images(groupName);
			else
				images = _imageProvider.Images(groupName, subgroupName);

			Dictionary<string, List<string>> groups = [];

			foreach (KeyValuePair<string, List<string>> item in _imageProvider.Groups())
			{
				groups.Add(item.Key, []);

				if (!String.IsNullOrEmpty(groupName) && item.Key.Equals(groupName))
				{
					foreach (string subGroup in item.Value)
					{
						groups[item.Key].Add(subGroup);
					}
				}
			}

			ImageFile image = null;

			if (!String.IsNullOrEmpty(imageName))
			{
				if (String.IsNullOrEmpty(subgroupName))
					image = _imageProvider.Images(groupName).Find(i => i.Name.Equals(imageName));
				else
					image = _imageProvider.Images(groupName, subgroupName).Find(i => i.Name.Equals(imageName));
			}

			bool canManageImages = ControllerContext.HttpContext.User.HasClaim(Constants.ClaimNameManageImages, "true");

			ProcessImagesViewModel Result = new(GetModelData(),
				canManageImages,
				groupName,
				subgroupName ?? String.Empty,
				image,
				groups,
				images,
				memoryCacheName);

			Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ImageManager, $"/{Name}", true));

			if (!String.IsNullOrEmpty(groupName))
				Result.Breadcrumbs.Add(new BreadcrumbItem(groupName, $"/{Name}/ViewGroup/{groupName}/", true));

			if (!String.IsNullOrEmpty(subgroupName))
			{
				Result.Breadcrumbs.Add(new BreadcrumbItem(subgroupName, $"/{Name}/ViewSubgroup/{groupName}/{subgroupName}/", true));

				if (!String.IsNullOrEmpty(imageName))
					Result.Breadcrumbs.Add(new BreadcrumbItem(imageName, $"ImageManager/ViewSubgroupImage/{groupName}/{subgroupName}/{BaseModel.RouteFriendlyName(imageName)}/", true));

			}
			else if (!String.IsNullOrEmpty(imageName))
			{
				Result.Breadcrumbs.Add(new BreadcrumbItem(imageName, $"ImageManager/ViewImage/{groupName}/{BaseModel.RouteFriendlyName(imageName)}/", true));
			}

			if (memoryCacheName != EmptyUploadCacheName)
			{
				Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ImageUploadFiles, $"ImageManager/UploadImage/", false));
				ProcessImageOptions(Result);
			}

			return Result;
		}

		private void ProcessImageOptions(ProcessImagesViewModel processImagesViewModel)
		{
			ImageProcessOptionsViewModel processOptionsViewModel = new()
			{
				GroupName = processImagesViewModel.SelectedGroupName,
				SubgroupName = processImagesViewModel.SelectedSubgroupName
			};

			object notificationResponse = null;

			_notificationService.RaiseEvent(Constants.NotificationEventImageUploadOptions, processOptionsViewModel, null, ref notificationResponse);

			processOptionsViewModel = notificationResponse as ImageProcessOptionsViewModel ?? processOptionsViewModel;

			processImagesViewModel.SubgroupName = processOptionsViewModel.SubgroupName;
			processImagesViewModel.ShowSubgroup = processOptionsViewModel.ShowSubgroup;
			processImagesViewModel.AdditionalData = processOptionsViewModel.AdditionalData;
			processImagesViewModel.AdditionalDataMandatory = processOptionsViewModel.AdditionalDataMandatory;
			processImagesViewModel.AdditionalDataName = processOptionsViewModel.AdditionalDataName;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591