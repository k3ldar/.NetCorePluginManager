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
 *  Product:  Blog.Plugin
 *  
 *  File: BlogKeywordSearchProvider.cs
 *
 *  Purpose:  Blog key word search provider
 *
 *  Date        Name                Reason
 *  09/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Threading;

using Middleware;
using Middleware.Blog;
using Middleware.Search;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Blog.Plugin.Classes
{
    /// <summary>
    /// Blog search keyword provider.  Provides search facilities for blogs.
    /// </summary>
    public class BlogSearchKeywordProvider : ISearchKeywordProvider
    {
        #region Private Members

        private readonly IBlogProvider _blogProvider;

        #endregion Private Members

        #region Constructors

        public BlogSearchKeywordProvider(IBlogProvider blogProvider)
        {
            _blogProvider = blogProvider ?? throw new ArgumentNullException(nameof(blogProvider));
        }

        #endregion Constructors

        #region ISearchKeywordProvider Methods

        public Dictionary<String, AdvancedSearchOptions> AdvancedSearch()
        {
            return null;
        }

        public List<SearchResponseItem> Search(in KeywordSearchOptions searchOptions)
        {
            if (searchOptions == null)
                throw new ArgumentNullException(nameof(searchOptions));

            List<SearchResponseItem> Result = new();

            List<BlogItem> foundBlogs = new();

            string[] words = searchOptions.SearchTerm.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            foreach (string word in words)
            {
                List<BlogItem> blogs = _blogProvider.Search(word);

                foreach (BlogItem item in blogs)
                {
                    if (foundBlogs.Count == searchOptions.MaximumSearchResults)
                        break;

                    if (!foundBlogs.Contains(item))
                        foundBlogs.Add(item);
                }
            }

            foreach (BlogItem blogItem in foundBlogs)
            {
                Result.Add(CreateSearchResponseItem(blogItem));
            }

            return Result;
        }

        public List<String> SearchResponseTypes(in Boolean quickSearch)
        {
            return new List<string>() { "Blog" };
        }

        #endregion ISearchKeywordProvider Methods

        #region Private Methods

        private static SearchResponseItem CreateSearchResponseItem(BlogItem blogItem)
        {
            string url = $"/Blog/{HtmlHelper.RouteFriendlyName(blogItem.Username)}/{blogItem.Id}/" +
                $"{blogItem.LastModified.ToString("dd-MM-yyyy")}/{HtmlHelper.RouteFriendlyName(blogItem.Title)}";

            SearchResponseItem responseItem = new("Blog", blogItem.Title, -1,
                url, blogItem.Title, "~/Views/Blog/_BlogSearchResult.cshtml");

            responseItem.Properties.Add(nameof(Languages.LanguageStrings.Author), blogItem.Username);
            responseItem.Properties.Add(nameof(Languages.LanguageStrings.Published),
                blogItem.PublishDateTime.ToString(Thread.CurrentThread.CurrentUICulture.DateTimeFormat.ShortDatePattern));

            return responseItem;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591