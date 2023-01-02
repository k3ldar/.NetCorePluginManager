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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PageLoadSpeedMiddlewareExtender.cs
 *
 *  Purpose:  Middleware extender to add page load times to pipeline
 *
 *  Date        Name                Reason
 *  29/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AspNetCore.PluginManager.Middleware;

using Microsoft.AspNetCore.Builder;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// Static class for easyily adding route load times to the pipeline
    /// </summary>
    public static class RouteLoadTimeMiddlewareExtender
    {
        private static bool _hasLoaded = false;

        /// <summary>
        /// IApplicationBuilder extender method.
        /// 
        /// Allows easy use of registering RouteLoadTimeMiddleware
        /// </summary>
        /// <param name="builder">IApplicationBuilder instance</param>
        /// <returns>IApplicationBuilder</returns>
        /// <example><pre style="font-family:Consolas;font-size:13px;color:black;background:white;"><span style="color:#1f377f;">app</span>.<span style="color:#74531f;">UseRouteLoadTimes</span>();</pre></example>
        public static IApplicationBuilder UseRouteLoadTimes(this IApplicationBuilder builder)
        {
            if (_hasLoaded)
                return builder;

            _hasLoaded = true;

            return builder.UseMiddleware<RouteLoadTimeMiddleware>();
        }
    }
}
