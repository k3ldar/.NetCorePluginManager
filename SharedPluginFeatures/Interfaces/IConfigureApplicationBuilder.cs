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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: IConfigureApplicationBuilder.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  09/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Plugin modules which need to configure the IApplicationBuilder should implement an instance
    /// of this interface.  When the AspNetCore.PluginManager loads it will call each instance
    /// as part of the startup configuration.
    /// </summary>
    public interface IConfigureApplicationBuilder
    {
        /// <summary>
        /// Provides an opportunity for plugins to configure the application builder.
        /// </summary>
        /// <param name="applicationBuilder">IApplicationBuilder instance.</param>
        void ConfigureApplicationBuilder(in IApplicationBuilder applicationBuilder);
    }
}
