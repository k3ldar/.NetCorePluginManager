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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: DeniedRoutes.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Contains information on routes that are denied to robots
    /// </summary>
    public sealed class DeniedRoute
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="route"></param>
        /// <param name="userAgent"></param>
        /// <exception cref="ArgumentNullException">Raised if route is null or empty</exception>
        /// <exception cref="ArgumentNullException">Raised if userAgent is null or empty</exception>
        /// <exception cref="ArgumentException">Raised if route is not a valid partial uri</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Exception message is for developers")]
        public DeniedRoute(in string route, in string userAgent)
        {
            if (string.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(userAgent))
                throw new ArgumentNullException(nameof(userAgent));

            if (!Uri.TryCreate(route, UriKind.Relative, out _))
                throw new ArgumentException("route must be a partial Uri", nameof(route));

            Route = route;
            UserAgent = userAgent;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Route being denied
        /// </summary>
        public string Route { get; private set; }

        /// <summary>
        /// User agent name being denied
        /// </summary>
        public string UserAgent { get; private set; }

        #endregion Properties
    }
}
