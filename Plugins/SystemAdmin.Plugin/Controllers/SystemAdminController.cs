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

using Microsoft.AspNetCore.Mvc;

using SystemAdmin.Plugin.Models;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Controllers
{
    [LoggedIn]
    [RestrictedIpRoute("SystemAdminRoute")]
    public class SystemAdminController : Controller
    {
        #region Private Members

        private readonly ISystemAdminHelperService _systemAdminHelperService;

        #endregion Private Members

        #region Constructors

        public SystemAdminController(ISystemAdminHelperService systemAdminHelperService)
        {
            _systemAdminHelperService = systemAdminHelperService ?? throw new ArgumentNullException(nameof(systemAdminHelperService));
        }

        #endregion Constructors

        #region Controller Action Methods

        public IActionResult Index(int id)
        {
            SystemAdminMainMenu selectedMenu = _systemAdminHelperService.GetSystemAdminMainMenu(id);

            if (selectedMenu == null)
                return View(new AvailableIconViewModel(_systemAdminHelperService.GetSystemAdminMainMenu()));

            return (View(new AvailableIconViewModel(selectedMenu)));
        }

        public IActionResult Grid(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return (Redirect("/SystemAdmin/"));

            return (View(new GridViewModel(subMenu)));
        }

        public IActionResult Map(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return (Redirect("/SystemAdmin/"));

            return (View(new MapViewModel(subMenu)));
        }

        public IActionResult Text(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return (Redirect("/SystemAdmin"));

            return (View(new TextViewModel(subMenu)));
        }

        public IActionResult TextEx(int id)
        {
            SystemAdminSubMenu subMenu = _systemAdminHelperService.GetSubMenuItem(id);

            if (subMenu == null)
                return (Redirect("/SystemAdmin"));

            return (View(new TextExViewModel(subMenu)));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion Controller Action Methods
    }
}
