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
 *  Product:  Documentation Plugin
 *  
 *  File: KeywordSearchProvider.cs
 *
 *  Purpose:  Documentation keyword search provider
 *
 *  Date        Name                Reason
 *  19/05/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;

using Middleware;
using Middleware.Search;

using Shared;
using Shared.Docs;

using SharedPluginFeatures;

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Documentation keyword provider
    /// </summary>
    public class KeywordSearchProvider : ISearchKeywordProvider
    {
        #region Private Members

        private readonly IDocumentationService _documentationService;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="documentationService">IDocumentationService instance</param>
        public KeywordSearchProvider(IDocumentationService documentationService)
        {
            _documentationService = documentationService ?? throw new ArgumentNullException(nameof(documentationService));
        }

        #endregion Constructors

        #region ISearchKeywordProvider Methods

        /// <summary>
        /// Search keywords
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns>List&lt;SearchResponseItem&gt;</returns>
        public List<SearchResponseItem> Search(in KeywordSearchOptions searchOptions)
        {
            if (searchOptions == null)
                throw new ArgumentNullException(nameof(searchOptions));

            List<SearchResponseItem> Results = new List<SearchResponseItem>();

            List<Document> documents = _documentationService.GetDocuments()
                .Where(d => d.DocumentType == DocumentType.Assembly ||
                    d.DocumentType == DocumentType.Custom)
                .OrderBy(o => o.SortOrder).ThenBy(o => o.Title)
                .ToList();

            foreach (Document document in documents)
            {
                if (Results.Count > searchOptions.MaximumSearchResults)
                    return Results;

                int offset = document.Title.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                if (offset > -1 && Results.Count < searchOptions.MaximumSearchResults)
                {
                    Results.Add(new SearchResponseItem("DocumentTitle", document.Title, offset,
                        $"docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/"));
                }

                if (!searchOptions.QuickSearch)
                {
                    offset = document.Summary.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && Results.Count < searchOptions.MaximumSearchResults)
                    {
                        Results.Add(new SearchResponseItem("DocumentSummary", document.Title, offset,
                            $"docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/"));

                        continue;
                    }

                    offset = document.ShortDescription.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && Results.Count < searchOptions.MaximumSearchResults)
                    {
                        Results.Add(new SearchResponseItem("DocumentLongShortDescription", document.Title, offset,
                            $"docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/"));

                        continue;
                    }

                    offset = document.LongDescription.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && Results.Count < searchOptions.MaximumSearchResults)
                    {
                        Results.Add(new SearchResponseItem("DocumentLongDescription", document.Title, offset,
                            $"docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/"));

                        continue;
                    }
                }
            }

            return Results;
        }

        #endregion ISearchKeywordProvider Methods
    }
}
