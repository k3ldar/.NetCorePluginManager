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
        /// Renders the contents of a dynamic content template
        /// </summary>
        /// <param name="contentTemplate">Dynamic content template to be rendered.</param>
        /// <returns></returns>
        string RenderDynamicPage(DynamicContentTemplate contentTemplate);

        /// <summary>
        /// Retrieves a list of custom pages
        /// </summary>
        /// <returns></returns>
        List<LookupListItem> GetActiveCustomPages();

        /// <summary>
        /// Retrieves dynamic page content
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IDynamicContentPage GetCustomPage(int id);
    }
}
