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
 *  Product:  Error Manager Plugin
 *  
 *  File: TempErrorManager.cs
 *
 *  Purpose:  Provides a container for IErrorManager when not used as a plugin
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using ErrorManager.Plugin.Models.Error;

using static Shared.Utilities;

using SharedPluginFeatures;

namespace ErrorManager.Plugin.Controllers
{
    public class ErrorController : BaseController
    {
        #region Private Members

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISettingsProvider _settingsProvider;

        #endregion Private Members

        #region Constructors

        public ErrorController(IHostingEnvironment hostingEnvironment, ISettingsProvider settingsProvider)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        #endregion Constructors

        #region Public Action Methods

        [Breadcrumb(nameof(Languages.LanguageStrings.Error))]
        public IActionResult Index()
        {
            return View(new BaseModel(GetBreadcrumbs()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.MissingLink))]
        public IActionResult NotFound404()
        {
            Response.StatusCode = 404;
            Error404Model model = null;

            ErrorManagerSettings settings = _settingsProvider.GetSettings<ErrorManagerSettings>("ErrorManager");

            if (settings.RandomQuotes)
            {
                // grab a random quote
                Random rnd = new Random(Convert.ToInt32(DateTime.Now.ToString("Hmsffff")));
                int quote = rnd.Next(settings.Count());
                model = new Error404Model(Languages.LanguageStrings.PageNotFound, settings.GetQuote(quote), GetImageFile(quote));
            }
            else
            {
                int index = 0;

                // sequential, save current state to cookie
                if (CookieExists("Error404"))
                {
                    // get index from cookie
                    string cookieValue = Decrypt(CookieValue("Error404"), settings.EncryptionKey);
                    index = StrToInt(cookieValue, 0) + 1;
                }

                if (index < 0 || index > settings.Count())
                    index = 0;

                CookieAdd("Error404", Encrypt(Convert.ToString(index), settings.EncryptionKey), 30);

                model = new Error404Model(Languages.LanguageStrings.PageNotFound, settings.GetQuote(index), GetImageFile(index));
            }

            model.Breadcrumbs = GetBreadcrumbs();

            return (View(model));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.HighVolume))]
        public IActionResult HighVolume()
        {
            return (View(new BaseModel(GetBreadcrumbs())));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.NotAcceptable))]
        public IActionResult NotAcceptable()
        {
            return (View(new BaseModel(GetBreadcrumbs())));
        }

#if DEBUG
        public IActionResult Raise(string s)
        {
            if (String.IsNullOrEmpty(s))
                s = "Oopsies";

            throw new Exception(s);
        }
#endif
        #endregion Public Action Methods

        #region Private Methods

        private string GetImageFile(in int index)
        {
            string rootPath = $"{_hostingEnvironment.ContentRootPath}\\wwwroot\\images\\error\\";

            if (Directory.Exists(rootPath))
            {
                string[] errorImageFiles = Directory.GetFiles(rootPath);

                foreach (string file in errorImageFiles)
                {
                    if (Path.GetFileName(file).StartsWith(index.ToString()))
                        return ($"/images/error/{Path.GetFileName(file)}");
                }

                if (System.IO.File.Exists(rootPath + "Default404.png"))
                    return ("/images/error/Default404.png");
            }

            return String.Empty;
        }

        #endregion Private Methods
    }
}