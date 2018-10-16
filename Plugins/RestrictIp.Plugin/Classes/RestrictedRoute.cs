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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  RestrictIp.Plugin
 *  
 *  File: RestrictedRoutes.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace RestrictIp.Plugin
{
    internal sealed class RestrictedRoute
    {
        #region Constructors

        internal RestrictedRoute(in string route, in string userAgent)
        {
            if (string.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            if (String.IsNullOrEmpty(userAgent))
                throw new ArgumentNullException(nameof(userAgent));

            Route = route;
            UserAgent = userAgent;
        }

        #endregion Constructors

        #region Properties

        internal string Route { get; set; }

        internal string UserAgent { get; set; }

        #endregion Properties
    }
}
