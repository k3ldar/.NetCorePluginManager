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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SystemAdminController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Middleware;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Models;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Controllers
{
    [LoggedIn]
    [RestrictedIpRoute("SystemAdminRoute")]
    [Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameStaff)]
    [DenySpider]
    public partial class SystemAdminController : BaseController
    {
        #region Private Members

        private readonly ISystemAdminHelperService _systemAdminHelperService;
        private readonly ISettingsProvider _settingsProvider;
        private readonly ISeoProvider _seoProvider;
        private readonly IUserSearch _userSearch;
        private readonly IClaimsProvider _claimsProvider;

        #endregion Private Members

        #region Constructors

        public SystemAdminController(ISettingsProvider settingsProvider, ISystemAdminHelperService systemAdminHelperService,
            ISeoProvider seoProvider, IUserSearch userSearch, IClaimsProvider claimsProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _systemAdminHelperService = systemAdminHelperService ?? throw new ArgumentNullException(nameof(systemAdminHelperService));
            _seoProvider = seoProvider ?? throw new ArgumentNullException(nameof(seoProvider));
            _userSearch = userSearch ?? throw new ArgumentNullException(nameof(userSearch));
            _claimsProvider = claimsProvider ?? throw new ArgumentNullException(nameof(claimsProvider));
        }

        #endregion Constructors

        #region Constants

        public const string Name = "SystemAdmin";

        #endregion Constants

        #region Controller Action Methods

        public IActionResult Index(int id)
        {
            SystemAdminMainMenu selectedMenu = _systemAdminHelperService.GetSystemAdminMainMenu(id);

            if (selectedMenu == null)
                return View(new AvailableIconViewModel(GetModelData(),
                    _systemAdminHelperService.GetSystemAdminMainMenu()));

            return View(new AvailableIconViewModel(GetModelData(), selectedMenu));
        }

        public IActionResult Grid(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return Redirect("/SystemAdmin/");

            return View(new GridViewModel(GetModelData(), subMenu));
        }

        public IActionResult Map(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return Redirect("/SystemAdmin/");

            return View(new MapViewModel(GetModelData(), _settingsProvider, subMenu));
        }

        public IActionResult View(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return Redirect("/SystemAdmin/");

            return View("PartialView", new PartialViewModel(GetModelData(), subMenu));
        }

        public IActionResult Text(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return Redirect("/SystemAdmin/");

            return View(new TextViewModel(GetModelData(), subMenu));
        }

        public IActionResult TextEx(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return Redirect("/SystemAdmin/");

            return View(new TextExViewModel(GetModelData(), _settingsProvider, subMenu));
        }

        public IActionResult Chart(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return Redirect("/SystemAdmin/");

            return View(new ChartViewModel(GetModelData(), subMenu));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel(GetModelData(),
                Activity.Current?.Id ?? HttpContext.TraceIdentifier));
        }

        #endregion Controller Action Methods
    }
}

#pragma warning restore CS1591