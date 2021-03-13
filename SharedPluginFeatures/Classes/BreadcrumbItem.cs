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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: BreadcrumbItem.cs
 *
 *  Purpose:  Contains breadcrumb item data 
 *
 *  Date        Name                Reason
 *  21/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// The breadcrumb item class is used extensively by the Breadcrumb.Plugin module to depict a breadcrumb item.
    /// </summary>
    public class BreadcrumbItem
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name of breadcrumb item.</param>
        /// <param name="route">Route to which the breadcrumb item is linked.</param>
        /// <param name="hasParameters">Indicates whether the route has parameters or not.</param>
        public BreadcrumbItem(in string name, in string route, in bool hasParameters)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(route))
                throw new ArgumentNullException(nameof(route));

            Name = name;
            Route = route;
            HasParameters = hasParameters;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Name of breadcrumb item.
        /// </summary>
        /// <value>string</value>
        public string Name { get; private set; }

        /// <summary>
        ///  Route to which the breadcrumb item is linked.
        /// </summary>
        /// <value>string</value>
        public string Route { get; private set; }

        /// <summary>
        /// Indicates whether the route has parameters or not.
        /// </summary>
        /// <value>bool</value>
        public bool HasParameters { get; private set; }

        #endregion Properties
    }
}
