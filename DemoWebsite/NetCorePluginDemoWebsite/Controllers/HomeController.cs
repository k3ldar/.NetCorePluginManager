using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AspNetCore.PluginManager.DemoWebsite.Models;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Controllers
{
    public class HomeController : Controller
    {
        #region Private Members

        private readonly IMemoryCache _memoryCache;

        #endregion Private Members

        #region Constructors

        public HomeController(IMemoryCache memoryCache)
        {
            // Memory Cache is initialised during the Plugin Manager and set to be injected in using DI
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        #endregion Constructors

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
