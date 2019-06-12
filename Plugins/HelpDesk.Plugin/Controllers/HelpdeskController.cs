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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Helpdesk Plugin
 *  
 *  File: HelpdeskController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using Microsoft.AspNetCore.Mvc;

using HelpdeskPlugin.Classes;
using HelpdeskPlugin.Models;

using Middleware.Helpdesk;

using Shared.Classes;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Controllers
{
    /// <summary>
    /// Helpdesk controller, provides Helpdesk functionality for any website.
    /// </summary>
    public partial class HelpdeskController : BaseController
    {
        #region Constants 

        public const string Name = "Helpdesk";

        #endregion Constants

        #region Private Members

        private static readonly CacheManager _helpdeskCache = new CacheManager("Helpdesk Cache", new TimeSpan(0, 30, 0));
        private readonly IHelpdeskProvider _helpdeskProvider;
        private readonly HelpdeskSettings _settings;

        #endregion Private Members

        #region Constructors

        public HelpdeskController(IHelpdeskProvider helpdeskProvider, ISettingsProvider settingsProvider)
        {
            _helpdeskProvider = helpdeskProvider ?? throw new ArgumentNullException(nameof(helpdeskProvider));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settings = settingsProvider.GetSettings<HelpdeskSettings>(nameof(HelpdeskSettings));
        }

        #endregion Constructors

        #region Public Controller Methods

        [Breadcrumb(nameof(Languages.LanguageStrings.Helpdesk))]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel(GetBreadcrumbs(), GetCartSummary(),
                _settings.ShowTickets, _settings.ShowFaq, _settings.ShowFeedback, GrowlGet());

            return View(model);
        }


        [HttpGet]
        public ActionResult GetCaptchaImage()
        {
            HelpdeskCacheItem loginCacheItem = GetCachedHelpdeskItem(false);

            if (loginCacheItem == null)
                return StatusCode(400);

            CaptchaImage ci = new CaptchaImage(loginCacheItem.CaptchaText, 240, 60, "Century Schoolbook");
            try
            {
                // Write the image to the response stream in JPEG format.
                using (MemoryStream ms = new MemoryStream())
                {
                    ci.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                    return File(ms.ToArray(), "image/png");
                }
            }
            catch (Exception err)
            {
                if (!err.Message.Contains("Specified method is not supported."))
                    throw;
            }
            finally
            {
                ci.Dispose();
            }

            return (null);
        }

        #endregion Public Controller Methods

        #region Private Methods

        private string FormatTextForDisplay(string message)
        {
            message = Shared.Utilities.RemoveHTMLElements(message);

            message = message.Replace("\r", String.Empty);
            message = message.Replace("\n", "<br />");

            return $"<p>{message}</p>";
        }

        private HelpdeskCacheItem GetCachedHelpdeskItem(bool createIfNotExist)
        {
            HelpdeskCacheItem Result = null;

            string cacheId = GetSessionId();

            CacheItem helpdeskCache = _helpdeskCache.Get(cacheId);

            if (helpdeskCache != null)
            {
                Result = (HelpdeskCacheItem)helpdeskCache.Value;
            }
            else if (createIfNotExist && helpdeskCache == null)
            {
                Result = new HelpdeskCacheItem();
                helpdeskCache = new CacheItem(cacheId, Result);
                _helpdeskCache.Add(cacheId, helpdeskCache);
            }

            return (Result);
        }

        #endregion Private Methods
    }
}
