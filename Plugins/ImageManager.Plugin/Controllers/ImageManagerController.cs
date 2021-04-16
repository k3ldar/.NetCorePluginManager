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
 *  Product:  Image Manager Plugin
 *  
 *  File: TempErrorManager.cs
 *
 *  Date        Name                Reason
 *  15/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using static Shared.Utilities;

#pragma warning disable CS1591

namespace ImageManager.Plugin.Controllers
{
    /// <summary>
    /// Error Controller
    /// </summary>
    [DenySpider]
    public class ImageManagerController : BaseController
    {
        #region Private Members

        private readonly ISettingsProvider _settingsProvider;

        #endregion Private Members

        #region Constructors

        public ImageManagerController(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        #endregion Constructors

        #region Public Action Methods

//        [Breadcrumb(nameof(Languages.LanguageStrings.Error))]
//        public IActionResult Index()
//        {
//            return View(new BaseModel(GetModelData()));
//        }

//        [Breadcrumb(nameof(Languages.LanguageStrings.MissingLink))]
//        public IActionResult NotFound404()
//        {
//            Response.StatusCode = 404;
//            Error404Model model;

//            ErrorManagerSettings settings = _settingsProvider.GetSettings<ErrorManagerSettings>("ErrorManager");

//            if (settings.RandomQuotes)
//            {
//                // grab a random quote
//                Random rnd = new Random(Convert.ToInt32(DateTime.Now.ToString("Hmsffff")));
//                int quote = rnd.Next(settings.Count());
//                model = new Error404Model(GetModelData(),
//                    Languages.LanguageStrings.PageNotFound, settings.GetQuote(quote), GetImageFile(quote));
//            }
//            else
//            {
//                int index = 0;

//                // sequential, save current state to cookie
//                if (CookieExists("Error404"))
//                {
//                    // get index from cookie
//                    string cookieValue = Decrypt(CookieValue("Error404"), settings.EncryptionKey);
//                    index = StrToInt(cookieValue, 0) + 1;
//                }

//                if (index < 0 || index > settings.Count())
//                    index = 0;

//                CookieAdd("Error404", Encrypt(Convert.ToString(index), settings.EncryptionKey), 30);

//                model = new Error404Model(GetModelData(),
//                    Languages.LanguageStrings.PageNotFound, settings.GetQuote(index), GetImageFile(index));
//            }

//            return View(model);
//        }

//        [Breadcrumb(nameof(Languages.LanguageStrings.HighVolume))]
//        public IActionResult HighVolume()
//        {
//            return View(new BaseModel(GetModelData()));
//        }

//        [Breadcrumb(nameof(Languages.LanguageStrings.NotAcceptable))]
//        public IActionResult NotAcceptable()
//        {
//            return View(new BaseModel(GetModelData()));
//        }

//        [Breadcrumb(nameof(Languages.LanguageStrings.AccessDenied))]
//        public IActionResult AccessDenied()
//        {
//            return View(new BaseModel(GetModelData()));
//        }

//#if DEBUG
//        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Debug Only")]
//        public IActionResult Raise(string s)
//        {
//            if (String.IsNullOrEmpty(s))
//                s = "Oopsies";

//            throw new Exception(s);
//        }
//#endif
        #endregion Public Action Methods

        #region Private Methods

        #endregion Private Methods
    }
}

#pragma warning restore CS1591