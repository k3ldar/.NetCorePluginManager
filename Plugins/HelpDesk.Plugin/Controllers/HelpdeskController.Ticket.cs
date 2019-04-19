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
 *  File: HelpdeskController.Tickets.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  18/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

using HelpdeskPlugin.Classes;
using HelpdeskPlugin.Models;

using static Middleware.Constants;
using Middleware.Helpdesk;

using Shared.Classes;
using static Shared.Utilities;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Controllers
{
    public partial class HelpdeskController
    {
        #region Public Action Methods

        [HttpGet]
        public IActionResult SubmitTicket()
        {
            return View(GetSubmitTicketViewModel(String.Empty, String.Empty));
        }

        [HttpPost]
        public IActionResult SubmitTicket(SubmitTicketViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (String.IsNullOrEmpty(model.CaptchaText))
                ModelState.AddModelError(nameof(model.CaptchaText), Languages.LanguageStrings.CodeNotValid);

            if (_settings.ShowCaptchaText)
            {
                HelpdeskCacheItem helpdeskCache = GetCachedHelpdeskItem(true);

                if (!model.CaptchaText.Equals(helpdeskCache.CaptchaText, StringComparison.CurrentCultureIgnoreCase))
                    ModelState.AddModelError(nameof(model.CaptchaText), Languages.LanguageStrings.CodeNotValid);
            }

            if (ModelState.IsValid)
            {
                SubmitTicket here
            }

            return View(GetSubmitTicketViewModel(model.Subject, model.Message));
        }

        #endregion Public Action Methods

        #region Private Methods

        private SubmitTicketViewModel GetSubmitTicketViewModel(in string subject, in string message)
        {
            HelpdeskCacheItem helpdeskCache = GetCachedHelpdeskItem(true);
            helpdeskCache.CaptchaText = GetRandomWord(_settings.CaptchaWordLength, CaptchaCharacters);

            UserSession userSession = GetUserSession();

            SubmitTicketViewModel Result = new SubmitTicketViewModel(GetBreadcrumbs(),
                GetCartSummary(),
                _helpdeskProvider.GetTicketDepartments(),
                _helpdeskProvider.GetTicketPriorities(),
                userSession.UserName, 
                userSession.UserEmail, 
                subject, 
                message,
                !String.IsNullOrEmpty(userSession.UserName));

            return Result;
        }

        #endregion Private Methods
    }
}
