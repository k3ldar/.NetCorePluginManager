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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SystemAdminController.Seo.cs
 *
 *  Purpose:  Seo specific methods for system admin
 *
 *  Date        Name                Reason
 *  28/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Models;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Controllers
{
    public partial class SystemAdminController
    {
        [HttpGet]
        [Authorize(Policy = Constants.PolicyNameAlterSeoData)]
        [Route("SystemAdmin/SeoData/{routeName}/")]
        public IActionResult SeoData(string routeName)
        {
            if (String.IsNullOrEmpty(routeName))
                return new StatusCodeResult(Constants.HtmlResponseBadRequest);

            string seoRoute = System.Net.WebUtility.UrlDecode(routeName);
            SeoDataModel model = new SeoDataModel(seoRoute);

            if (_seoProvider.GetSeoDataForRoute(seoRoute, out string title, out string metaDescription,
                out string author, out List<string> keywords))
            {
                model.SeoAuthor = author;
                model.SeoMetaDescription = metaDescription;
                model.SeoTitle = title;
                model.SeoTags = String.Join(' ', keywords);
            }
			else
			{
				if (IsUserLoggedIn())
				{
					model.SeoAuthor = UserName();
				}
			}

            return PartialView("_SeoUpdate", model);
        }

        [HttpPost]
        [Authorize(Policy = Constants.PolicyNameAlterSeoData)]
        public IActionResult SeoUpdateData(SeoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (!_seoProvider.GetSeoDataForRoute(model.SeoUrl, out string title, out string metaDescription, out string author, out List<string> keywords))
            {
                keywords = new List<string>();
            }

            if (!author.Equals(model.SeoAuthor))
                _seoProvider.UpdateAuthor(ValidateUserInput(model.SeoUrl, ValidationType.RouteName), ValidateUserInput(model.SeoAuthor, ValidationType.Name));

            if (!metaDescription.Equals(model.SeoMetaDescription))
                _seoProvider.UpdateDescription(model.SeoUrl, model.SeoMetaDescription);

            if (!title.Equals(model.SeoTitle))
                _seoProvider.UpdateTitle(model.SeoUrl, model.SeoTitle);

            _seoProvider.RemoveKeywords(model.SeoUrl, keywords);

			if (model.SeoTags == null)
				model.SeoTags = String.Empty;

            _seoProvider.AddKeywords(model.SeoUrl, model.SeoTags.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());
            return Redirect(model.SeoUrl);
        }
    }
}

#pragma warning restore CS1591