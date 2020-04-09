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

        private const string DocumentSearchResultViewName = "~/Views/Docs/_DocumentSearchResult.cshtml";
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
                    Results.Add(AddDocumentToSearchResult(document, "DocumentTitle",
                        $"/docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/", offset, 5));


                    continue;
                }

                if (!searchOptions.QuickSearch)
                {
                    offset = document.Summary.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && Results.Count < searchOptions.MaximumSearchResults)
                    {
                        Results.Add(AddDocumentToSearchResult(document, "DocumentSummary",
                            $"/docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/", offset));

                        continue;
                    }

                    offset = document.ShortDescription.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && Results.Count < searchOptions.MaximumSearchResults)
                    {
                        Results.Add(AddDocumentToSearchResult(document, "DocumentLongShortDescription",
                            $"/docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/", offset));

                        continue;
                    }

                    offset = document.LongDescription.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && Results.Count < searchOptions.MaximumSearchResults)
                    {
                        Results.Add(AddDocumentToSearchResult(document, "DocumentLongDescription",
                            $"/docs/Document/{HtmlHelper.RouteFriendlyName(document.Title)}/", offset));

                        continue;
                    }
                }
            }

            return Results;
        }

        /// <summary>
        /// Retrieves the advanced search options for the provider
        /// </summary>
        /// <returns>Dictionary&lt;string, AdvancedSearchOptions&gt;</returns>
        public Dictionary<string, AdvancedSearchOptions> AdvancedSearch()
        {
            return null;
        }

        /// <summary>
        /// Returns a list of all available response types for the Documentation Plugin
        /// </summary>
        /// <param name="quickSearch">Indicates whether the response types are for quick or normal searching</param>
        /// <returns>List&lt;string&gt;</returns>
        public List<string> SearchResponseTypes(in Boolean quickSearch)
        {
            List<string> Result = new List<string>()
            {
                "DocumentTitle"
            };

            if (!quickSearch)
            {
                Result.Add("DocumentSummary");
                Result.Add("DocumentLongShortDescription");
                Result.Add("DocumentLongDescription");
            }

            return Result;
        }

        #endregion ISearchKeywordProvider Methods

        #region Private Methods

        private SearchResponseItem AddDocumentToSearchResult(in Document document, in string searchType,
            in string url, in int offset, in int relevance = 0)
        {
            SearchResponseItem Result = new SearchResponseItem(searchType, document.Title, offset,
                url, document.Title, DocumentSearchResultViewName)
            {
                Relevance = relevance
            };

            Result.Properties.Add("ShortDescription", document.ShortDescription);

            return Result;
        }

        #endregion Private Methods
    }
}
