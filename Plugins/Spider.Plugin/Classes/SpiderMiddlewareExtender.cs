﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Spider.Plugin
 *  
 *  File: SpiderMiddlewareExtender.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  23/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;

namespace Spider.Plugin
{
    /// <summary>
    /// Spider Middleware extender
    /// </summary>
    public static class UserSessionMiddlewareExtender
    {
        /// <summary>
        /// IApplicationBuilder extender method.
        /// 
        /// Allows easy use of registering Spider.Plugin
        /// </summary>
        /// <param name="builder">IApplicationBuilder instance</param>
        /// <returns>IApplicationBuilder</returns>
        /// <example><pre style="font-family:Consolas;font-size:13px;color:black;background:white;"><span style="color:#1f377f;">app</span>.<span style="color:#74531f;">UseSpider</span>();</pre></example>
        public static IApplicationBuilder UseSpider(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<SpiderMiddleware>();
        }
    }
}
