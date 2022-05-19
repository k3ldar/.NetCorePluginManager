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
 *  Product:  Resources Plugin
 *  
 *  File: ResourcesController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  19/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Languages;

using Microsoft.AspNetCore.Mvc;

using Middleware;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using Constants = SharedPluginFeatures.Constants;

#pragma warning disable IDE0079
#pragma warning disable CS1591

namespace ResourcesPlugin.Controllers
{
    /// <summary>
    /// Resources plugin controller, allows users to view resources.
    /// </summary>
    [DenySpider]
    [Subdomain(ResourcesController.Name)]
    public class ResourcesController : BaseController
    {
        #region Private Members

        private readonly ResourcesControllerSettings _settings;

        #endregion Private Members

        #region Constructors

        public ResourcesController(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settings = settingsProvider.GetSettings<ResourcesControllerSettings>(nameof(ResourcesPlugin));
        }

        #endregion Constructors

        #region Constants

        public const string Name = "Resources";

        #endregion Constants

        #region Public Action Methods

        [HttpGet]
        [LoggedInOut]
        public IActionResult Index()
        {
            return View();
        }

        #endregion Public Action Methods

        #region Private Methods

        #endregion Private Methods
    }
}

#pragma warning restore CS1591
#pragma warning restore IDE0079
