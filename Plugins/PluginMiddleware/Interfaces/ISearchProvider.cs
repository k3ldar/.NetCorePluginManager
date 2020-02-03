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
 *  File: ISearchProvider.cs
 *
 *  Purpose:  Facilitates searching from within a website
 *
 *  Date        Name                Reason
 *  02/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware.Search;

namespace Middleware
{
    /// <summary>
    /// Search provider, provides methods used to search within a website.
    /// </summary>
    public interface ISearchProvider
    {
        /// <summary>
        /// Performs a keyword search using the search options.
        /// </summary>
        /// <param name="keywordSearchOptions">Search Options</param>
        /// <returns>SearchRespons&lt;T&gt;</returns>
        SearchResponse<T> KeywordSearch<T>(in KeywordSearchOptions keywordSearchOptions);
    }
}
