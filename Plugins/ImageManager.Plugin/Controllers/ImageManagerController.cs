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

        [Breadcrumb(nameof(Languages.LanguageStrings.AppImageManagement))]
        public IActionResult Index()
        {
            string groupName = String.Empty;
            List<ImageFile> images = _imageProvider.Images(groupName);

            return View(CreateImagesViewModel(groupName, images));
        }

        #endregion Public Action Methods

        #region Private Methods

        private ImagesViewModel CreateImagesViewModel(string groupName, List<ImageFile> images, int page = 1)
        {
            List<string> groups = new List<string>();

            _imageProvider.Groups().ForEach(g => groups.Add(g));

            ImagesViewModel Result = new ImagesViewModel(GetModelData(), groupName, groups, images);

            //Result.Pagination = BuildPagination(images.Count, (int)_settings.ProductsPerPage, page,
            //    $"/Products/{Result.RouteText(groupName)}/{group.Id}/", "",
            //    LanguageStrings.Previous, LanguageStrings.Next);

            return Result;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591