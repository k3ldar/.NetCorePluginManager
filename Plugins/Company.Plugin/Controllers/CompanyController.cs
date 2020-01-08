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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Company.Plugin
 *  
 *  File: CompanyController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  07/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Company.Plugin.Classes;

using Microsoft.AspNetCore.Mvc;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Company.Plugin.Controllers
{
    public class CompanyController : BaseController
    {
        #region Private Members

        public const string Name = "Company";
        private readonly CompanySettings _settings;

        #endregion Private Members

        #region Constructors

        public CompanyController(ISettingsProvider settingsProvider)
        {
            _settings = settingsProvider.GetSettings<CompanySettings>(nameof(CompanySettings));
        }

        #endregion Constructors

        #region Public Controller Methods

        [Breadcrumb(nameof(Languages.LanguageStrings.About))]
        public IActionResult About()
        {
            if (!_settings.ShowAbout)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Contact))]
        public IActionResult Contact()
        {
            if (!_settings.ShowContact)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Privacy))]
        public IActionResult Privacy()
        {
            if (!_settings.ShowPrivacy)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.TermsAndConditions))]
        public IActionResult Terms()
        {
            if (!_settings.ShowTerms)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Cookies))]
        public IActionResult Cookies()
        {
            if (!_settings.ShowCookies)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Careers))]
        public IActionResult Careers()
        {
            if (!_settings.ShowCareers)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Returns))]
        public IActionResult Returns()
        {
            if (!_settings.ShowReturns)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Delivery))]
        public IActionResult Delivery()
        {
            if (!_settings.ShowDelivery)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Newsletter))]
        public IActionResult NewsLetter()
        {
            if (!_settings.ShowNewsletter)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Affiliate))]
        public IActionResult Affiliate()
        {
            if (!_settings.ShowAffiliates)
                return Redirect("/");

            return View(new BaseModel(GetModelData()));
        }

        #endregion Public Controller Methods
    }
}

#pragma warning restore CS1591