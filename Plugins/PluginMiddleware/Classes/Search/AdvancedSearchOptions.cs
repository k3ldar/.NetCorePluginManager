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
 *  File: AdvancedSearchOptions.cs
 *
 *  Purpose:  Advanced search options
 *
 *  Date        Name                Reason
 *  05/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace Middleware.Search
{
    /// <summary>
    /// Options that can be provided by Keyword search providers for advanced searching
    /// </summary>
    public sealed class AdvancedSearchOptions
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actionName">Name of the action used for advanced searching.</param>
        /// <param name="controllerName">Name of the controller used for advanced searching.</param>
        /// <param name="searchName">Route for default search options, to be displayed to center.</param>
        /// <param name="searchOption">Route for Search options if required, to be displayed on left column.</param>
        /// <param name="styleSheet">Optional style sheet that can be embedded on the search page for advanced searches.</param>
        public AdvancedSearchOptions(in string actionName, in string controllerName,
            in string searchName, in string searchOption, in string styleSheet)
        {
            if (String.IsNullOrEmpty(actionName))
                throw new ArgumentNullException(nameof(actionName));

            if (String.IsNullOrEmpty(controllerName))
                throw new ArgumentNullException(nameof(controllerName));

            if (String.IsNullOrEmpty(searchName))
                throw new ArgumentNullException(nameof(searchName));

            ActionName = actionName;
            ControllerName = controllerName;
            SearchName = searchName;
            SearchOption = searchOption ?? String.Empty;
            StyleSheet = styleSheet ?? String.Empty;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actionName">Name of the action used for advanced searching.</param>
        /// <param name="controllerName">Name of the controller used for advanced searching.</param>
        /// <param name="searchName">Route for default search options, to be displayed to center.</param>
        public AdvancedSearchOptions(in string actionName, in string controllerName,
            in string searchName)
            : this(actionName, controllerName, String.Empty, searchName, String.Empty)
        {

        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Name of the action used for advanced searching.
        /// </summary>
        /// <value>string</value>
        public string ActionName { get; private set; }

        /// <summary>
        /// Name of the controller used for advanced searching.
        /// </summary>
        /// <value>string</value>
        public string ControllerName { get; private set; }

        /// <summary>
        /// Route for Search options if required, to be displayed on left column.
        /// </summary>
        /// <value>string</value>
        public string SearchOption { get; private set; }

        /// <summary>
        /// Route for default search options, to be displayed to center.
        /// </summary>
        /// <value>string</value>
        public string SearchName { get; private set; }

        /// <summary>
        /// Optional style sheet that can be embedded on the search page for advanced searches.
        /// </summary>
        /// <value>string</value>
        public string StyleSheet { get; private set; }

        #endregion Properties
    }
}
