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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IRouteDataService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SharedPluginFeatures
{
    /// <summary>
    /// This interface is implemented by the AspNetCore.PluginManager and will determine a valid
    /// route based on the class or public action method for a Type.
    /// </summary>
    public interface IRouteDataService
    {
        /// <summary>
        /// Provides the route associated with a class, this will be based on the controller name
        /// and if supplied the Route attributes placed on the class.
        /// </summary>
        /// <param name="type">Type to be checked for route data.</param>
        /// <param name="routeProvider">IActionDescriptorCollectionProvider instance obtained using DI.</param>
        /// <returns>string</returns>
        string GetRouteFromClass(Type type, IActionDescriptorCollectionProvider routeProvider);

        /// <summary>
        /// Provides the route associated with an action method, this will be based on the name of the action and 
        /// controller and if supplied the Route attributes placed on the class and method in question.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="routeProvider">IActionDescriptorCollectionProvider instance obtained using DI.</param>
        /// <returns></returns>
        string GetRouteFromMethod(in MethodInfo method, in IActionDescriptorCollectionProvider routeProvider);
    }
}
