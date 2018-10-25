using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SystemAdmin.Plugin.Models;

namespace SystemAdmin.Plugin.Controllers
{
    public class HomeController : Controller
    {
        #region Constructors

        public HomeController()
        {

        }

        #endregion Constructors

        #region Controller Action Methods

        public IActionResult Index ()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion Controller Action Methods
    }
}
