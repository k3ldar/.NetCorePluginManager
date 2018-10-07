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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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

        [DenySpider]
        public IActionResult Privacy()
        {
            return View();
        }

        [DenySpider("*")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
