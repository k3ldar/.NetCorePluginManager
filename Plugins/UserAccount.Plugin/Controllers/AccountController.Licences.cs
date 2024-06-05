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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: AccountController.Licences.cs
 *
 *  Purpose:  Manages user downloads
 *
 *  Date        Name                Reason
 *  06/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Middleware.Accounts.Licences;

using SharedPluginFeatures;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
#pragma warning disable CS1591

    public partial class AccountController
    {
        #region Public Action Methods

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.MyLicences), nameof(AccountController), nameof(Index))]
        public IActionResult Licences()
        {
            List<ViewLicenceViewModel> licences = new List<ViewLicenceViewModel>();

            foreach (Licence licence in _licenceProvider.LicencesGet(UserId()))
            {
                licences.Add(new ViewLicenceViewModel(GetModelData(),
                    licence.Id, licence.DomainName, licence.LicenceType.Description,
                    Shared.Utilities.DateWithin(licence.ExpireDate, licence.StartDate, DateTime.Now) && licence.IsValid,
                    licence.IsTrial, licence.ExpireDate, licence.UpdateCount, licence.EncryptedLicence));
            }

            LicenceViewModel model = new LicenceViewModel(GetModelData(), licences, GrowlGet());

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            return View(model);
        }

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.LicenceView), nameof(AccountController), nameof(AccountController.Licences))]
        public IActionResult LicenceView(int id)
        {
            ViewLicenceViewModel model = null;
            Licence licence = _licenceProvider.LicencesGet(UserId()).Find(l => l.Id == id);

            if (licence != null)
            {
                model = new ViewLicenceViewModel(GetModelData(),
                    licence.Id, licence.DomainName, licence.LicenceType.Description,
                    Shared.Utilities.DateWithin(licence.ExpireDate, licence.StartDate, DateTime.Now) && licence.IsValid,
                    licence.IsTrial, licence.ExpireDate, licence.UpdateCount, licence.EncryptedLicence);
            }

            if (model == null)
                return RedirectToAction(nameof(Licences));

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            return View(model);
        }

        [HttpGet]
        [Breadcrumb(nameof(Languages.LanguageStrings.LicenceCreateTrial), nameof(AccountController), nameof(AccountController.Licences))]

        public IActionResult LicenceCreate()
        {
            return View(new CreateLicenceViewModel(GetModelData()));
        }

        [HttpPost]
        public IActionResult LicenceCreate(CreateLicenceViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            LicenceType licenceType = _licenceProvider.LicenceTypesGet().Find(l => l.Id == model.LicenceType);

            if (licenceType == null)
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.LicenceTypeInvalid);

            if (ModelState.IsValid)
            {
                switch (_licenceProvider.LicenceTrialCreate(UserId(), licenceType))
                {
                    case Middleware.LicenceCreate.Existing:
                        GrowlAdd(String.Format(Languages.LanguageStrings.LicenceCreateTrialExists,
                            licenceType.Description));
                        break;

                    case Middleware.LicenceCreate.Failed:
                        GrowlAdd(Languages.LanguageStrings.LicenceCreateTrialFailed);
                        break;

                    case Middleware.LicenceCreate.Success:
                        GrowlAdd(Languages.LanguageStrings.LicenceCreatedTrial);
                        break;
                }

                return RedirectToAction(nameof(Licences));
            }

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            return View(model);
        }

        [HttpPost]
        public IActionResult LicenceUpdateDomain(ViewLicenceViewModel model)
        {
            Licence licence = _licenceProvider.LicencesGet(UserId()).Find(l => l.Id == model.Id);

            if (!Shared.Utilities.ValidateIPAddress(model.Domain))
            {
                ModelState.AddModelError(nameof(model.Domain), Languages.LanguageStrings.RuleErrorIPAddressInvalid);
                return View(nameof(LicenceView), model);
            }

            if (licence != null && _licenceProvider.LicenceUpdateDomain(UserId(), licence, model.Domain))
            {
                GrowlAdd(Languages.LanguageStrings.LicenceUpdated);
                return RedirectToAction(nameof(Licences));
            }

            GrowlAdd(Languages.LanguageStrings.LicenceUpdateFailed);
            return RedirectToAction(nameof(Licences));
        }

        public IActionResult LicenceSendEmail(int id)
        {
            Licence licence = _licenceProvider.LicencesGet(UserId()).Find(l => l.Id == id);

            if (licence != null && _licenceProvider.LicenceSendEmail(UserId(), id))
            {
                GrowlAdd(Languages.LanguageStrings.EmailSent);
                return RedirectToAction(nameof(Licences));
            }

            GrowlAdd(Languages.LanguageStrings.EmailSendFailed);
            return RedirectToAction(nameof(Licences));
        }

        #endregion Public Action Methods
    }

#pragma warning restore CS1591
}
