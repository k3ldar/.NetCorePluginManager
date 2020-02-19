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
 *  Product:  Products.Plugin
 *  
 *  File: KeywordSearchProvider.cs
 *
 *  Purpose:  Product key word search provider
 *
 *  Date        Name                Reason
 *  12/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Products;
using Middleware.Search;

using SharedPluginFeatures;

namespace ProductPlugin.Classes
{
    /// <summary>
    /// Product keyword provider
    /// </summary>
    public class KeywordSearchProvider : ISearchKeywordProvider
    {
        #region Private Members

        private readonly IProductProvider _productProvider;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="productProvider">IProductProvider instance</param>
        public KeywordSearchProvider(IProductProvider productProvider)
        {
            _productProvider = productProvider ?? throw new ArgumentNullException(nameof(productProvider));
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

            List<Product> products = _productProvider.GetProducts(1, 50000);

            if (searchOptions.ExactMatch || searchOptions.QuickSearch)
            {
                ExactMatch(Results, searchOptions, products);
            }
            else
            {
                NonExactMatch(Results, searchOptions, products);
            }

            return Results;
        }

        /// <summary>
        /// Retrieves the name of the search for advance searches
        /// </summary>
        /// <returns>string</returns>
        public string SearchName()
        {
            return "Products";
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
                "ProductName"
            };

            if (!quickSearch)
            {
                Result.Add("ProductDescription");
                Result.Add("ProductSku");
            }

            return Result;
        }

        #endregion ISearchKeywordProvider Methods

        #region Private Methods

        private void ExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
            in List<Product> products)
        {
            foreach (Product product in products)
            {
                if (results.Count > searchOptions.MaximumSearchResults)
                {
                    return;
                }

                int offset = product.Name.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
                {
                    results.Add(new SearchResponseItem("ProductName", product.Name, offset,
                        $"Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/"));
                }

                if (!searchOptions.QuickSearch)
                {
                    offset = product.Description.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
                    {
                        results.Add(new SearchResponseItem("ProductDescription", product.Name, offset,
                            $"Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/"));

                        continue;
                    }

                    offset = product.Sku.IndexOf(searchOptions.SearchTerm, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
                    {
                        results.Add(new SearchResponseItem("ProductSku", product.Name, offset,
                            $"Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/"));

                        continue;
                    }
                }
            }
        }

        private void NonExactMatch(in List<SearchResponseItem> results, in KeywordSearchOptions searchOptions,
            in List<Product> products)
        {
            string[] words = searchOptions.SearchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                if (String.IsNullOrWhiteSpace(word))
                    continue;

                foreach (Product product in products)
                {
                    if (results.Count > searchOptions.MaximumSearchResults)
                    {
                        return;
                    }

                    int offset = product.Name.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

                    if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
                    {
                        results.Add(new SearchResponseItem("ProductName", product.Name, offset,
                            $"Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/"));
                    }

                    if (!searchOptions.QuickSearch)
                    {
                        offset = product.Description.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

                        if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
                        {
                            results.Add(new SearchResponseItem("ProductDescription", product.Name, offset,
                                $"Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/"));

                            continue;
                        }

                        offset = product.Sku.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

                        if (offset > -1 && results.Count < searchOptions.MaximumSearchResults)
                        {
                            results.Add(new SearchResponseItem("ProductSku", product.Name, offset,
                                $"Product/{product.Id}/{HtmlHelper.RouteFriendlyName(product.Name)}/"));

                            continue;
                        }
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
