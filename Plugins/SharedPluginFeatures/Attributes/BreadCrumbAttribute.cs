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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: DeniedRoutes.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  20/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace SharedPluginFeatures
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class BreadcrumbAttribute : Attribute
    {
        #region Constructors

        public BreadcrumbAttribute(in bool home, params string[] routes)
        {
            if (routes.Length == 0)
                throw new ArgumentException(nameof(routes));

            Routes = new List<Breadcrumb>();

            if (home)
                Routes.Add(new Breadcrumb("Home", "/"));


        }

        public BreadcrumbAttribute(params string[] routes)
            : this (true, routes)
        {

        }

        #endregion Constructors

        #region Properties

        internal List<Breadcrumb> Routes { get; set; }

        #endregion Properties
    }
}
