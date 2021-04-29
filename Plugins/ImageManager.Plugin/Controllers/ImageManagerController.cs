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
    [Authorize(Policy = Constants.PolicyNameImageManager)]

    public class ImageManagerController : BaseController
    {
        #region Public Consts

        public const string Name = nameof(ImageManager);

        #endregion Public Consts

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

        [Breadcrumb(nameof(LanguageStrings.AppImageManagement))]
        public IActionResult Index()
        {
            string groupName = String.Empty;

            return View(CreateImagesViewModel(groupName, String.Empty));
        }

        [Breadcrumb(nameof(LanguageStrings.ViewGroup), nameof(Index), HasParams = true)]
        [Route("ImageManager/ViewGroup/{groupName}")]
        public IActionResult ViewGroup(string groupName)
        {
            if (String.IsNullOrEmpty(groupName))
                return RedirectToAction(nameof(Index));

            if (!_imageProvider.Groups().ContainsKey(groupName))
                return RedirectToAction(nameof(Index));

            return View("/Views/ImageManager/Index.cshtml", CreateImagesViewModel(groupName, String.Empty));
        }

        [Breadcrumb(nameof(LanguageStrings.ViewGroup), nameof(Index), HasParams = true)]
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

            return View("/Views/ImageManager/Index.cshtml", CreateImagesViewModel(groupName, subgroupName));
        }

        #endregion Public Action Methods

        #region Private Methods

        private ImagesViewModel CreateImagesViewModel(string groupName, string subgroupName/*, int page = 1*/)
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

            ImagesViewModel Result = new ImagesViewModel(GetModelData(), groupName, subgroupName, groups, images);

            //Result.Pagination = BuildPagination(images.Count, (int)_settings.ProductsPerPage, page,
            //    $"/Products/{Result.RouteText(groupName)}/{group.Id}/", "",
            //    LanguageStrings.Previous, LanguageStrings.Next);

            return Result;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591