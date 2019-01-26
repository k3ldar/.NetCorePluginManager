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

namespace SharedPluginFeatures
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class BreadcrumbAttribute : Attribute
    {
        #region Constructors

        public BreadcrumbAttribute(string name, string parentRoute)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException(nameof(name));

            Name = name;
            ParentRoute = parentRoute;
        }

        public BreadcrumbAttribute(string name)
            : this (name, String.Empty)
        {

        }

        public BreadcrumbAttribute(string name, string controllerName, string actionName)
            : this (name, String.Empty)
        {
            if (String.IsNullOrEmpty(controllerName))
                throw new ArgumentNullException(nameof(controllerName));

            if (String.IsNullOrEmpty(actionName))
                throw new ArgumentNullException(nameof(actionName));

            if (controllerName.EndsWith("Controller"))
                controllerName = controllerName.Substring(0, controllerName.Length - 10);

            ParentRoute = $"/{controllerName}/{actionName}";
        }

        #endregion Constructors

        #region Properties

        public string Name { get; private set; }

        public string ParentRoute { get; private set; }

        public bool HasParams { get; set; }

        #endregion Properties
    }
}
