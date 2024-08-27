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
 *  Copyright (c) 2024 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Cron.Plugin
 *  
 *  File: CronExtender.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  06/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;

using Cron.Plugin.Classes;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Middleware;

using PluginManager.Abstractions;

using Shared.Classes;

namespace Cron.Plugin
{
	/// <summary>
	/// Cron middleware extender
	/// </summary>
	public static class CronExtender
	{
		/// <summary>
		/// Adds Cron functionality to the application
		/// </summary>
		/// <param name="builder">IApplicationBuilder instance</param>
		/// <param name="threadManagerServices">Thread manager services</param>
		/// <returns>IApplicationBuilder</returns>
		/// <example><c><span style="color:#1f377f;">app</span>.<span style="color:#74531f;">UseCron</span>(threadManagerServices);</c></example>
		public static IApplicationBuilder UseCron(this IApplicationBuilder builder, IThreadManagerServices threadManagerServices)
		{
			if (threadManagerServices == null)
				throw new ArgumentNullException(nameof(threadManagerServices));

			MasterCronThread cronThread = new(builder.ApplicationServices.GetRequiredService<ICronJobProvider>(),
				builder.ApplicationServices.GetRequiredService<IPluginClassesService>(),
				builder.ApplicationServices.GetRequiredService<ILogger>());
			ThreadManager.ThreadStart(cronThread, "Cron Thread", System.Threading.ThreadPriority.AboveNormal);

			return builder;
		}
	}
}
