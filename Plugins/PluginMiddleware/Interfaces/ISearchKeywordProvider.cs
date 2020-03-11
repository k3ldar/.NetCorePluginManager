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
 *  Product:  PluginMiddleware
 *  
 *  File: ISearchKeywordProvider.cs
 *
 *  Purpose:  Keyword search provider
 *
 *  Date        Name                Reason
 *  03/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Middleware.Search;

namespace Middleware
{
    /// <summary>
    /// Keyword search provider interface is implemented by classes which provide keyword search 
    /// facilities within a plugin
    /// </summary>
    public interface ISearchKeywordProvider
    {
        /// <summary>
        /// Interface for searching keywords
        /// </summary>
        /// <param name="searchOptions"></param>
        /// <returns>List&lt;SearchResponseItem&gt;</returns>
        List<SearchResponseItem> Search(in KeywordSearchOptions searchOptions);

        /// <summary>
        /// Retrieves the available search response types for all search providers.
        /// </summary>
        /// <param name="quickSearch">indicates whether the response types are from a quick search or not.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> SearchResponseTypes(in bool quickSearch);

        /// <summary>
        /// Retrieves a string that can optionally be used by the UI to provide a paged or tabbed advance search option.
        /// 
        /// Return null or empty string if the search provider should not have a custom advanced search option.
        /// </summary>
        /// <returns>string</returns>
        Dictionary<string, string> SearchName();
    }
}
