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
 *  Copyright (c) 2019 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Helpdesk Plugin
 *  
 *  File: HelpdeskController.Feedback.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using HelpdeskPlugin.Classes;
using HelpdeskPlugin.Models;

using Microsoft.AspNetCore.Mvc;

using Middleware.Helpdesk;

using SharedPluginFeatures;

using static Middleware.Constants;
using static Shared.Utilities;

#pragma warning disable CS1591

namespace HelpdeskPlugin.Controllers
{
    public partial class HelpdeskController : BaseController
    {
        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.Feedback), Name, nameof(Index))]
        public IActionResult Feedback()
        {
            if (!_settings.ShowFeedback)
                return RedirectToAction(nameof(Index), Name);

            List<FeedbackItemViewModel> feedback = new List<FeedbackItemViewModel>();

            foreach (Feedback item in _helpdeskProvider.GetFeedback(true))
            {
                string username = item.Username;

                if (String.IsNullOrEmpty(username))
                    username = Languages.LanguageStrings.Unknown;

                feedback.Add(new FeedbackItemViewModel(username, item.Message));
            }

            return View(new FeedbackViewModel(GetModelData(), feedback));
        }

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.LeaveFeedback), Name, nameof(Feedback))]
        [DenySpider]
        public IActionResult LeaveFeedback()
        {
            if (!_settings.ShowFeedback)
                return RedirectToAction(nameof(Index), Name);

            return View(GetFeedbackModel());
        }

        [HttpPost]
        [BadEgg]
        public IActionResult LeaveFeedback(LeaveFeedbackViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!_settings.ShowFeedback)
                return RedirectToAction(nameof(Index), Name);

            if (_settings.ShowCaptchaText)
            {
                HelpdeskCacheItem helpdeskCache = GetCachedHelpdeskItem(true);

                if (!model.CaptchaText.Equals(helpdeskCache.CaptchaText, StringComparison.CurrentCultureIgnoreCase))
                    ModelState.AddModelError(nameof(model.CaptchaText), Languages.LanguageStrings.CodeNotValid);
            }

            if (ModelState.IsValid)
            {
                if (_helpdeskProvider.SubmitFeedback(UserId(), model.Name, model.Feedback))
                {
                    GrowlAdd(Languages.LanguageStrings.FeedbackSubmitted);
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.FeedbackFailed);
            }

            return View(GetFeedbackModel());
        }

        #endregion Public Action Methods

        #region Private Methods

        private LeaveFeedbackViewModel GetFeedbackModel()
        {
            HelpdeskCacheItem helpdeskCache = GetCachedHelpdeskItem(true);
            helpdeskCache.CaptchaText = GetRandomWord(_settings.CaptchaWordLength, CaptchaCharacters);

            return new LeaveFeedbackViewModel(GetModelData(), _settings.ShowCaptchaText);
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591