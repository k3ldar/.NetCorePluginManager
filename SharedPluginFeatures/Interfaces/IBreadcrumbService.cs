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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: IBreadcrumbService.cs
 *
 *  Purpose:  Add breadcrumbs dynamically
 *
 *  Date        Name                Reason
 *  26/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// This interface is implemented by the Breadcrumb.Plugin and allows plugins to add dynamic plugins 
    /// to the list of available plugins managed.  Especially useful if you have dynamic data that can
    /// not use the BreadcrumbAttribute.
    /// 
    /// An instance of this interface is available using the DI container.
    /// </summary>
    public interface IBreadcrumbService
    {
        /// <summary>
        /// Adds a dynamic breadcrumb to managed list of breadcrumbs.
        /// </summary>
        /// <param name="name">Name of breadcrumb</param>
        /// <param name="route">Route the breadcrumb will use.</param>
        /// <param name="hasParameters">Indicates that the route contains parameters.</param>
        void AddBreadcrumb(in string name, in string route, in bool hasParameters);

        /// <summary>
        /// Adds a dynamic breadcrumb to managed list of breadcrumbs.
        /// </summary>
        /// <param name="name">Name of breadcrumb</param>
        /// <param name="route">Route the breadcrumb will use.</param>
        /// <param name="parentRoute">Route used by the parent breadcrumb.</param>
        /// <param name="hasParameters">Indicates that the route contains parameters.</param>
        void AddBreadcrumb(in string name, in string route, in string parentRoute, in bool hasParameters);
    }
}
