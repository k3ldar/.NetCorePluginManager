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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using ImageManager.Plugin.Models;

using Languages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Middleware.Images;
using Middleware.Interfaces;

using PluginManager.Abstractions;

using SharedPluginFeatures;

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
        private const string ErrorInvalidImageName = "Invalid ImageName";
        private const string ErrorInvalidGroupName = "Invalid GroupName";
        private const string ErrorInvalidSubgroupName = "Invalid SubgroupName";
        private const string ErrorUnableToDeleteImage = "Unable to delete image";

        #endregion Public / Private Consts

        #region Private Members

        private readonly ISettingsProvider _settingsProvider;
        private readonly IImageProvider _imageProvider;

        #endregion Private Members

        #region Constructors

        public ImageManagerController(ISettingsProvider settingsProvider, IImageProvider imageProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _imageProvider = imageProvider ?? throw new ArgumentNullException(nameof(imageProvider));
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

            if (!groups.ContainsKey(groupName))
                return RedirectToAction(nameof(Index));

            if (String.IsNullOrEmpty(subgroupName))
                return RedirectToAction(nameof(Index));

            if (!groups[groupName].Contains(subgroupName))
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
        public IActionResult DeleteImage([FromBody]DeleteImageModel model)
        {
            if (model == null)
                return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest);

            if (!model.ConfirmDelete)
                return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest);

            if (String.IsNullOrEmpty(model.ImageName))
                return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidImageName);

            if (String.IsNullOrEmpty(model.GroupName) || !_imageProvider.GroupExists(model.GroupName))
                return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidGroupName);

            if (!String.IsNullOrEmpty(model.SubgroupName) && !_imageProvider.SubgroupExists(model.GroupName, model.SubgroupName))
                return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidSubgroupName);

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
                return GenerateJsonErrorResponse(Constants.HtmlResponseBadRequest, ErrorInvalidImageName);

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

        #endregion Public Action Methods

        #region Private Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string ReplaceLastDash(string s)
        {
            StringBuilder Result = new StringBuilder(s);

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
            List<ImageFile> images = null;

            if (String.IsNullOrEmpty(subgroupName))
                images = _imageProvider.Images(groupName);
            else
                images = _imageProvider.Images(groupName, subgroupName);

            Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();

            foreach (KeyValuePair<string, List<string>> item in _imageProvider.Groups())
            {
                groups.Add(item.Key, new List<string>());

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
                    image = _imageProvider.Images(groupName).Where(i => i.Name.Equals(imageName)).FirstOrDefault();
                else
                    image = _imageProvider.Images(groupName, subgroupName).Where(i => i.Name.Equals(imageName)).FirstOrDefault();
            }

            bool canManageImages = ControllerContext.HttpContext.User.HasClaim(Constants.ClaimNameManageImages, "true");

            ImagesViewModel Result = new ImagesViewModel(GetModelData(), canManageImages, groupName, subgroupName, image, groups, images);

            Result.Breadcrumbs.Add(new BreadcrumbItem(LanguageStrings.ImageManager, $"/{Name}", true));

            if (!String.IsNullOrEmpty(groupName))
                Result.Breadcrumbs.Add(new BreadcrumbItem(groupName, $"/{Name}/ViewGroup/{groupName}/", true));

            if (!String.IsNullOrEmpty(subgroupName))
            {
                Result.Breadcrumbs.Add(new BreadcrumbItem(subgroupName, $"/{Name}/ViewSubgroup/{groupName}/{subgroupName}/", true));

                if (!String.IsNullOrEmpty(imageName))
                    Result.Breadcrumbs.Add(new BreadcrumbItem(imageName, $"ImageManager/ViewSubgroupImage/{groupName}/{subgroupName}/{SharedPluginFeatures.BaseModel.RouteFriendlyName(imageName)}/", true));

            }
            else if (!String.IsNullOrEmpty(imageName))
            {
                Result.Breadcrumbs.Add(new BreadcrumbItem(imageName, $"ImageManager/ViewImage/{groupName}/{SharedPluginFeatures.BaseModel.RouteFriendlyName(imageName)}/", true));
            }

            return Result;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591