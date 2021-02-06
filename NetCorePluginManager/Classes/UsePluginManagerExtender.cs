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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: UsePluginManagerExtender.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  09/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// Plugin Manager extender class.
    /// </summary>
    public static class UsePluginManagerExtender
    {
        #region Public Static Methods

        /// <summary>
        /// IApplicationBuilder extender method used to provide easy access to UsePluginManager 
        /// method when configuring an application.
        /// </summary>
        /// <param name="mvcApplication">IApplicationBuilder instance</param>
        /// <returns>IApplicationBuilder</returns>
        public static IApplicationBuilder UsePluginManager(this IApplicationBuilder mvcApplication)
        {
            // allow plugins to configure MvcApplication
            UsePlugins(mvcApplication);

            return mvcApplication;
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void UsePlugins(in IApplicationBuilder applicationBuilder)
        {
            List<IConfigureApplicationBuilder> appBuilderServices = PluginManagerService
                .GetPluginManager().PluginGetClasses<IConfigureApplicationBuilder>();

            foreach (IConfigureApplicationBuilder builder in appBuilderServices)
                builder.ConfigureApplicationBuilder(applicationBuilder);
        }

        #endregion Private Static Methods
    }
}
