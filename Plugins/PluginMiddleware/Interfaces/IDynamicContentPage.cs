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
 *  File: IDynamicContentPage.cs
 *
 *  Purpose:  Dynamic content page
 *
 *  Date        Name                Reason
 *  05/12/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;

using SharedPluginFeatures.DynamicContent;

namespace Middleware
{
    /// <summary>
    /// Interface for dynamic content page
    /// </summary>
    public interface IDynamicContentPage
    {
        /// <summary>
        /// Unique page id
        /// </summary>
        /// <value>int</value>
        int Id { get; set; }

        /// <summary>
        /// Name of dynamic page
        /// </summary>
        /// <value>string</value>
        string Name { get; set; }

        /// <summary>
        /// Date and time the page becomes active
        /// </summary>
        /// <value>DateTime</value>
        DateTime ActiveFrom { get; set; }

        /// <summary>
        /// Date and time the page expires
        /// </summary>
        /// <value>DateTime</value>
        DateTime ActiveTo { get; set; }

        /// <summary>
        /// Route name that will form part of the Uri
        /// </summary>
        /// <value>string</value>
        string RouteName { get; set; }

        /// <summary>
        /// Dynamic content that will be displayed within the page
        /// </summary>
        /// <value>List&lt;DynamicContentTemplate&gt;</value>
        List<DynamicContentTemplate> Content { get; }
    }
}
