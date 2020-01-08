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
 *  Product:  GeoIpPlugin
 *  
 *  File: GeoIpPluginExtender.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace GeoIp.Plugin
{
    /// <summary>
    /// GeoIp.Plugin extender class.
    /// </summary>
    public static class GeoIpPluginExtender
    {
        /// <summary>
        /// IApplicationBuilder extender method.
        /// 
        /// Allows easy use of registering GeoIp.Plugin
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        /// <returns>IApplicationBuilder</returns>
        /// <example><pre style="font-family:Consolas;font-size:13px;color:black;background:white;"><span style="color:#1f377f;">services</span>.<span style="color:#74531f;">UseGeoIpService</span>();</pre></example>
        public static void UseGeoIpService(this IServiceCollection services)
        {
            services.AddSingleton<IGeoIpDataService, GeoIpService>();
        }
    }
}
