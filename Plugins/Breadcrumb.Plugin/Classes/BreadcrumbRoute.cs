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
 *  Product:  Breadcrumb.Plugin
 *  
 *  File: BreadcrumbRoute.cs
 *
 *  Purpose:  Breadcrumb routes
 *
 *  Date        Name                Reason
 *  21/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace Breadcrumb.Plugin
{
    internal sealed class BreadcrumbRoute
    {
        #region Constructors

        internal BreadcrumbRoute(in string route, in bool hasParams)
        {
            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            Route = route;
            Breadcrumbs = new List<BreadcrumbItem>(4);
            HasParameters = hasParams;

            if (hasParams)
                PartialRoute = $"{Route.ToLower()}/";
            else
                PartialRoute = route.ToLower();
        }

        #endregion Constructors

        #region Properties

        internal string Route { get; private set; }

        internal string PartialRoute { get; set; }

        internal List<BreadcrumbItem> Breadcrumbs { get; private set; }

        internal bool HasParameters { get; private set; }

        #endregion Properties
    }
}
