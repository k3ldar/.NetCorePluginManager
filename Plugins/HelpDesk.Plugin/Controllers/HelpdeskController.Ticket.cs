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
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

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
        [Breadcrumb(nameof(SubmitTicket), HelpdeskController.Name, nameof(Index))]
        public IActionResult SubmitTicket()
        {
            if (!_settings.ShowTickets)
                return RedirectToAction(nameof(Index), Name);

            return View(GetSubmitTicketViewModel(String.Empty, String.Empty, String.Empty, String.Empty));
        }

        [HttpPost]
        [BadEgg]
        public IActionResult SubmitTicket(SubmitTicketViewModel model)
        {
            if (!_settings.ShowTickets)
                return RedirectToAction(nameof(Index), Name);

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
                if (_helpdeskProvider.SubmitTicket(UserId(), model.Department, model.Priority,
                    model.Username, model.Email, model.Subject, model.Message, out HelpdeskTicket ticket))
                {
                    return RedirectToAction(nameof(ViewTicket), Name, new { ticket.Id });
                }

                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.OopsError);
            }

            return View(GetSubmitTicketViewModel(model.Subject, model.Message, model.Username, model.Email));
        }

        [HttpGet]
        [Breadcrumb(nameof(ViewTicket), HelpdeskController.Name, nameof(Index))]
        public IActionResult ViewTicket(int id)
        {
            HelpdeskTicket ticket = _helpdeskProvider.GetTicket(id);

            if (ticket == null)
            {
                GrowlAdd(Languages.LanguageStrings.TicketNotFound);
                return RedirectToAction(nameof(FindTicket), Name);
            }

            return View(GetTicketViewModel(ticket));
        }

        [HttpPost]
        [BadEgg]
        public IActionResult ViewTicket(string email, string ticketKey)
        {
            if (!_settings.ShowTickets)
                return RedirectToAction(nameof(Index), Name);

            HelpdeskTicket ticket = _helpdeskProvider.GetTicket(email, ticketKey);

            if (ticket == null)
            {
                GrowlAdd(Languages.LanguageStrings.TicketNotFound);
                return RedirectToAction(nameof(FindTicket), Name);
            }

            return View(GetTicketViewModel(ticket));
        }

        [HttpPost]
        public IActionResult TicketRespond(TicketResponseViewModel model)
        {
            if (!_settings.ShowTickets)
                return RedirectToAction(nameof(Index), Name);

            HelpdeskTicket ticket = _helpdeskProvider.GetTicket(model.Id);

            if (ticket == null)
                return RedirectToAction(nameof(FindTicket), Name);

            _helpdeskProvider.TicketRespond(ticket, model.Name, model.Message);

            return RedirectToAction(nameof(ViewTicket), Name, new { id = model.Id });
        }

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.FindATicket), Name, nameof(Index))]
        public IActionResult FindTicket()
        {
            if (!_settings.ShowTickets)
                return RedirectToAction(nameof(Index), Name);

            return View(GetFindTicketViewModel());
        }

        [HttpPost]
        [BadEgg]
        public IActionResult FindTicket(FindTicketViewModel model)
        {
            if (!_settings.ShowTickets)
                return RedirectToAction(nameof(Index), Name);

            if (!Shared.Utilities.IsValidEmail(model.Email))
                ModelState.AddModelError(nameof(model.Email), Languages.LanguageStrings.InvalidEmailAddress);

            HelpdeskCacheItem helpdeskCache = GetCachedHelpdeskItem(true);

            if (_settings.ShowCaptchaText && helpdeskCache.Requests > 1)
            {
                if (!model.CaptchaText.Equals(helpdeskCache.CaptchaText, StringComparison.CurrentCultureIgnoreCase))
                    ModelState.AddModelError(nameof(model.CaptchaText), Languages.LanguageStrings.CodeNotValid);
            }

            if (ModelState.IsValid)
            {
                HelpdeskTicket ticket = _helpdeskProvider.GetTicket(model.Email, model.Key);

                if (ticket != null)
                    return RedirectToAction(nameof(ViewTicket), Name, new { ticket.Id });

                GrowlAdd(Languages.LanguageStrings.TicketNotFound);
            }

            return View(GetFindTicketViewModel());
        }

        #endregion Public Action Methods

        #region Private Methods

        private ViewTicketViewModel GetTicketViewModel(HelpdeskTicket ticket)
        {
            List<ViewTicketResponseViewModel> messages = new List<ViewTicketResponseViewModel>();

            foreach (HelpdeskTicketMessage item in ticket.Messages)
                messages.Add(new ViewTicketResponseViewModel(item.DateCreated, item.UserName, FormatTextForDisplay(item.Message)));

            return new ViewTicketViewModel(GetModelData(),
                ticket.Id, ticket.Priority.Description,
                ticket.Department.Description, ticket.Status.Description,
                ticket.Key, ticket.Subject, ticket.DateCreated, ticket.DateLastUpdated,
                ticket.CreatedBy, ticket.LastReplier, messages);
        }

        private FindTicketViewModel GetFindTicketViewModel()
        {
            HelpdeskCacheItem helpdeskCache = GetCachedHelpdeskItem(true);
            helpdeskCache.CaptchaText = GetRandomWord(_settings.CaptchaWordLength, CaptchaCharacters);
            helpdeskCache.Requests++;

            return new FindTicketViewModel(GetModelData(),
                _settings.ShowCaptchaText && helpdeskCache.Requests > 1);
        }

        private SubmitTicketViewModel GetSubmitTicketViewModel(in string subject, in string message,
            in string userName, in string email)
        {
            HelpdeskCacheItem helpdeskCache = GetCachedHelpdeskItem(true);
            helpdeskCache.CaptchaText = GetRandomWord(_settings.CaptchaWordLength, CaptchaCharacters);

            UserSession userSession = GetUserSession();

            SubmitTicketViewModel Result = new SubmitTicketViewModel(GetModelData(),
                _helpdeskProvider.GetTicketDepartments(),
                _helpdeskProvider.GetTicketPriorities(),
                String.IsNullOrEmpty(userName) ? userSession.UserName : userName,
                String.IsNullOrEmpty(email) ? userSession.UserEmail : email,
                subject,
                message,
                !String.IsNullOrEmpty(userSession.UserName));

            return Result;
        }

        #endregion Private Methods
    }
}
