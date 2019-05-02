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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: RouteDataServices.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Reflection;
using System.Linq;

using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes
{
    public sealed class RouteDataServices : IRouteDataService
    {
        public string GetRouteFromClass(Type type, IActionDescriptorCollectionProvider routeProvider)
        {
            // does the class have a route attribute
            RouteAttribute classRouteAttribute = (RouteAttribute)type.GetCustomAttributes(true)
                .Where(r => r.GetType() == typeof(RouteAttribute)).FirstOrDefault();

            if (classRouteAttribute != null && !String.IsNullOrEmpty(classRouteAttribute.Template))
            {
                return classRouteAttribute.Template;
            }

            ActionDescriptor route = routeProvider.ActionDescriptors.Items.Where(ad => ad
                .DisplayName.StartsWith(type.FullName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (route == null)
                return String.Empty;

            if (route.AttributeRouteInfo != null)
            {
                return $"/{route.AttributeRouteInfo.Template}/{route.AttributeRouteInfo.Name}";
            }
            else if (route.AttributeRouteInfo == null)
            {
                ControllerActionDescriptor controllerDescriptor = route as ControllerActionDescriptor;
                return $"/{controllerDescriptor.ControllerName}";
            }

            return String.Empty;
        }

        public string GetRouteFromMethod(in MethodInfo method, in IActionDescriptorCollectionProvider routeProvider)
        {
            // does the class have a route attribute
            RouteAttribute classRouteAttribute = (RouteAttribute)method.GetCustomAttributes(true)
                .Where(r => r.GetType() == typeof(RouteAttribute)).FirstOrDefault();

            if (classRouteAttribute != null && !String.IsNullOrEmpty(classRouteAttribute.Template))
            {
                string template = classRouteAttribute.Template;

                while (template.IndexOf('{') > -1)
                    template = template.Substring(0, template.Length - 1);

                if (template.EndsWith("/"))
                    template = template.Substring(0, template.Length - 1);

                return template;
            }

            string routeName = $"{method.DeclaringType.ToString()}.{method.Name}";

            ActionDescriptor route = routeProvider.ActionDescriptors.Items.Where(ad => ad
                .DisplayName.StartsWith(routeName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (route == null)
                return String.Empty;

            if (route.AttributeRouteInfo != null)
            {
                return $"/{route.AttributeRouteInfo.Template}/{route.AttributeRouteInfo.Name}";
            }

            if (route.RouteValues["controller"].ToString() == "Home")
            {
                return $"/{route.RouteValues["action"]}";
            }
            else
            {
                return $"/{route.RouteValues["controller"]}/{route.RouteValues["action"]}";
            }
        }
    }
}
