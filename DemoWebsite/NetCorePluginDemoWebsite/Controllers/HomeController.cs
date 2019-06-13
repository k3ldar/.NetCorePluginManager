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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: HomeController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

using AspNetCore.PluginManager.DemoWebsite.Models;

using SharedPluginFeatures;

using Shared.Classes;

namespace AspNetCore.PluginManager.DemoWebsite.Controllers
{
    public class HomeController : BaseController
    {
        #region Private Members

        private readonly IMemoryCache _memoryCache;
        private readonly IStringLocalizer<Languages.LanguageStrings> _localizer;

        #endregion Private Members

        #region Constructors

        public HomeController(IMemoryCache memoryCache, IStringLocalizer<Languages.LanguageStrings> localizer)
        {
            // Memory Cache is initialised during the Plugin Manager and set to be injected in using DI
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        #endregion Constructors

        [Breadcrumb(nameof(Languages.LanguageStrings.PluginManager))]
        public IActionResult Index()
        {
            UserSession session = (UserSession)HttpContext.Items[Constants.UserSession];
            ViewBag.Username = String.IsNullOrEmpty(session.UserName) ? "Guest" : session.UserName;

            return View(new BaseModel(GetBreadcrumbs(), GetCartSummary()));
        }

        [DenySpider("*")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
