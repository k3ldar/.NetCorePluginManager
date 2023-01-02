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
 *  Product:  Documentation Plugin
 *  
 *  File: DocumentationSitemapProvider.cs
 *
 *  Purpose:  Provides sitemap functionality for documentation items
 *
 *  Date        Name                Reason
 *  27/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Shared;
using Shared.Docs;

using SharedPluginFeatures;

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Documentation sitemap provider, provides sitemap information for documentation items
    /// </summary>
    public class DocumentationSitemapProvider : ISitemapProvider
    {
        #region Private Members

        private readonly IDocumentationService _documentationService;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="documentationService">IDocumentationService instance</param>
        public DocumentationSitemapProvider(IDocumentationService documentationService)
        {
            _documentationService = documentationService ?? throw new ArgumentNullException(nameof(documentationService));
        }

        #endregion Constructors

        /// <summary>
        /// Retrieve a list of all documents that will be included in the sitemap
        /// </summary>
        /// <returns>List&lt;ISitemapItem&gt;</returns>
        public List<SitemapItem> Items()
        {
            List<SitemapItem> Result = new List<SitemapItem>();

            List<Document> documents = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Assembly || d.DocumentType == DocumentType.Custom)
                .OrderBy(o => o.SortOrder).ThenBy(o => o.Title)
                .ToList();

            foreach (Document document in documents)
            {
                Uri blogUrl = new Uri($"docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/",
                    UriKind.RelativeOrAbsolute);

                Result.Add(new SitemapItem(blogUrl, SitemapChangeFrequency.Daily));
            }

            return Result;
        }
    }
}
