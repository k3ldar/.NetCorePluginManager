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
 *  Product:  Plugin Middleware
 *  
 *  File: BaseSearchOptions.cs
 *
 *  Purpose:  Base search options
 *
 *  Date        Name                Reason
 *  02/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Search
{
    /// <summary>
    /// Base options for completing searches
    /// </summary>
    public class BaseSearchOptions
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isLoggedIn">Indicates that the search is being completed from a user who is logged in.</param>
        /// <param name="searchTerm">The search term being sought.</param>
        public BaseSearchOptions(in bool isLoggedIn, in string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm))
                throw new ArgumentNullException(nameof(searchTerm));

            IsLoggedIn = isLoggedIn;
            SearchTerm = searchTerm;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Indicates whether the search is being made from a logged in user or not.
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        /// The search term being searched
        /// </summary>
        public string SearchTerm { get; private set; }

        #endregion Properties
    }
}
