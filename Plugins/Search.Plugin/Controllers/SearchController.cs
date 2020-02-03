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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Search Plugin
 *  
 *  File: SearchController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  01/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using System.Security.Claims;

using SearchPlugin.Classes;
using SearchPlugin.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using Middleware;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static Middleware.Constants;
using static Shared.Utilities;

#pragma warning disable CS1591

namespace SearchPlugin.Controllers
{
    /// <summary>
    /// Search controller, allows users to search using a standard interface implemented by ISearchProvider interface.
    /// </summary>
    public class SearchController : BaseController
    {
        #region Private Members

        private readonly SearchControllerSettings _settings;

        private static readonly CacheManager _searchCache = new CacheManager("Search Cache", new TimeSpan(0, 30, 0));

        #endregion Private Members

        #region Constructors

        public SearchController(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settings = settingsProvider.GetSettings<SearchControllerSettings>(nameof(SearchPlugin));
        }

        #endregion Constructors

        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.Search))]
        [LoggedInOut]
        public IActionResult Index(string returnUrl)
        {

            SearchViewModel model = new SearchViewModel(GetModelData());

            return View(model);
        }

        [HttpPost]
        [BadEgg]
        [LoggedInOut]
        public IActionResult Index(SearchViewModel model)
        {

            return View(model);
        }

        [HttpGet]
        public ActionResult GetCaptchaImage()
        {
            SearchCacheItem searchCacheItem = GetCachedSearchAttempt();

            CaptchaImage ci = new CaptchaImage(searchCacheItem.CaptchaText, 240, 60, "Century Schoolbook");
            try
            {
                // Write the image to the response stream in JPEG format.
                using (MemoryStream ms = new MemoryStream())
                {
                    ci.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    return File(ms.ToArray(), "image/png");
                }
            }
            catch (Exception err)
            {
                if (!err.Message.Contains("Specified method is not supported."))
                    throw;
            }
            finally
            {
                ci.Dispose();
            }

            return null;
        }

        private SearchCacheItem GetCachedSearchAttempt()
        {
            throw new NotImplementedException();
        }

        #endregion Public Action Methods

        #region Private Methods

        #endregion Private Methods
    }
}

#pragma warning restore CS1591