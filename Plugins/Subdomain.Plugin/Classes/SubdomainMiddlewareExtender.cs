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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Subdomain.Plugin
 *  
 *  File: SubdomainMiddlewareExtender.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;

namespace Subdomain.Plugin
{
	/// <summary>
	/// Sitemap middleware extender
	/// </summary>
	public static class SubdomainMiddlewareExtender
	{
		/// <summary>
		/// IApplicationBuilder extender method.
		/// 
		/// Allows easy use of registering SitemapMiddleware.Plugin
		/// </summary>
		/// <param name="builder">IApplicationBuilder instance</param>
		/// <returns>IApplicationBuilder</returns>
		/// <example><pre style="font-family:Consolas;font-size:13px;color:black;background:white;"><span style="color:#1f377f;">app</span>.<span style="color:#74531f;">UseSubdomainRouting</span>();</pre></example>
		public static IApplicationBuilder UseSubdomainRouting(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<SubdomainMiddleware>();
		}
	}
}
