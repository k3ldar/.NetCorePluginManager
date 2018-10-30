using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using SystemAdmin.Plugin.Models;

using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Controllers
{
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

            GridViewModel model = new GridViewModel(subMenu);

            return (View(model));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion Controller Action Methods
    }
}
