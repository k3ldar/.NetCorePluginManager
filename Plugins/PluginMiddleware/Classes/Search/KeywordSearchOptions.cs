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
 *  File: KeywordSearchOptions.cs
 *
 *  Purpose:  Keyword search options
 *
 *  Date        Name                Reason
 *  02/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware.Search
{
    /// <summary>
    /// Keyword search request
    /// </summary>
    public class KeywordSearchOptions : BaseSearchOptions
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isLoggedIn">Indicates that the search is being completed from a user who is logged in.</param>
        /// <param name="searchTerm">The search term being sought.</param>
        public KeywordSearchOptions(in bool isLoggedIn, in string searchTerm)
            : base(isLoggedIn, searchTerm)
        {
            Timeout = 30000;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Timeout in milliseconds, if implemented then after the timeout period an error should be raised.
        /// </summary>
        public int Timeout { get; set; }

        #endregion Properties
    }
}
