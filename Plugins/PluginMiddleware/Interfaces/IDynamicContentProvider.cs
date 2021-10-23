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
 *  Product:  PluginMiddleware
 *  
 *  File: IDynamicContentProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;

using SharedPluginFeatures.DynamicContent;

namespace Middleware.DynamicContent
{
    /// <summary>
    /// Dynamic content provider for integrating with the DynamicContent.Plugin module.
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IDynamicContentProvider
    {
        /// <summary>
        /// Creates a new custom page and returns the id of the new page
        /// </summary>
        /// <returns>int</returns>
        int CreateCustomPage();

        /// <summary>
        /// Retrieves a list of custom pages
        /// </summary>
        /// <returns>List&lt;DynamicContentTemplate&gt;</returns>
        List<LookupListItem> GetCustomPageList();

        /// <summary>
        /// Retrieves all custom pages
        /// </summary>
        /// <returns>List&lt;IDynamicContentPage&gt;</returns>
        List<IDynamicContentPage> GetCustomPages();

        /// <summary>
        /// Retrieves dynamic page content by id
        /// </summary>
        /// <param name="id">Id of page to find</param>
        /// <returns>IDynamicContentPage</returns>
        IDynamicContentPage GetCustomPage(int id);

        /// <summary>
        /// Retrieves all dynamic content templates
        /// </summary>
        /// <returns>List&lt;DynamicContentTemplate&gt;</returns>
        List<DynamicContentTemplate> Templates();

        /// <summary>
        /// Determines whether the page name already exists
        /// </summary>
        /// <param name="id">Id of current page</param>
        /// <param name="pageName">Name of page to be validated.</param>
        /// <returns>bool</returns>
        bool PageNameExists(int id, string pageName);

        /// <summary>
        /// Determines whether a route name already exists
        /// </summary>
        /// <param name="id">Id of current page</param>
        /// <param name="routeName">Name of route to be validated.</param>
        /// <returns></returns>
        bool RouteNameExists(int id, string routeName);

        /// <summary>
        /// Saves the dynamic content page
        /// </summary>
        /// <param name="dynamicContentPage">Dynamic content page to be saved</param>
        /// <returns>bool</returns>
        bool Save(IDynamicContentPage dynamicContentPage);

        /// <summary>
        /// Provides an opportunity for user input that is input via custom forms to be saved
        /// </summary>
        /// <param name="data">Form data to be saved</param>
        /// <returns>bool</returns>
        bool SaveUserInput(string data);
    }
}
