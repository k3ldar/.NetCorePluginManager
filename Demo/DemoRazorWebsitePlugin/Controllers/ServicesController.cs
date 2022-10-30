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
 *  Product:  Demo Website Plugin
 *  
 *  File: ServicesController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

namespace DemoWebsitePlugin.Controllers
{
    [Subdomain(ServicesController.Name)]
    public class ServicesController : BaseController
    {
        public const string Name = "Services";


        [Breadcrumb(nameof(Languages.LanguageStrings.Middleware))]
        public IActionResult Middleware()
        {
            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Api))]
        public IActionResult Api()
        {
            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.DependencyInjection))]
        public IActionResult DependencyInjection()
        {
            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Website))]
        public IActionResult Website()
        {
            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Custom))]
        public IActionResult Custom()
        {
            return View(new BaseModel(GetModelData()));
        }
    }
}