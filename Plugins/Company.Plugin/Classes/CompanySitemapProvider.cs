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
 *  Product:  Company Plugin
 *  
 *  File: CompanySitemapProvider.cs
 *
 *  Purpose:  Provides sitemap functionality for Company information
 *
 *  Date        Name                Reason
 *  27/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Company.Plugin.Controllers;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace Company.Plugin.Classes
{
    /// <summary>
    /// Company sitemap provider, provides sitemap information for company information
    /// </summary>
    public class CompanySitemapProvider : ISitemapProvider
    {
        #region Private Members

        private readonly CompanySettings _settings;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settingsProvider">ISettingsProvider instance</param>
        public CompanySitemapProvider(ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _settings = settingsProvider.GetSettings<CompanySettings>(nameof(CompanySettings));
        }

        #endregion Constructors

        /// <summary>
        /// Retrieve a list of all company information that will be included in the sitemap
        /// </summary>
        /// <returns>List&lt;ISitemapItem&gt;</returns>
        public List<SitemapItem> Items()
        {
            List<SitemapItem> Result = new List<SitemapItem>();

            if (_settings.ShowAbout)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.About)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }

            if (_settings.ShowAffiliates)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Affiliate)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }

            if (_settings.ShowCareers)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Careers)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Weekly));
            }

            if (_settings.ShowContact)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Contact)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }

            if (_settings.ShowCookies)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Cookies)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }

            if (_settings.ShowDelivery)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Delivery)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }

            if (_settings.ShowNewsletter)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.NewsLetter)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Weekly));
            }

            if (_settings.ShowPrivacy)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Privacy)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }

            if (_settings.ShowReturns)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Returns)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }

            if (_settings.ShowTerms)
            {
                Result.Add(new SitemapItem(
                    new Uri($"{CompanyController.Name}/{nameof(CompanyController.Terms)}", UriKind.RelativeOrAbsolute),
                        SitemapChangeFrequency.Monthly));
            }


            return Result;
        }
    }
}
