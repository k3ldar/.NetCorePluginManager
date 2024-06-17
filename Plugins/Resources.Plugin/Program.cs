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
 *  Product:  Resources.Plugin
 *  
 *  File: Program.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

using PluginManager;

using Shared.Classes;

#pragma warning disable CS1591

namespace Resources.Plugin
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			ThreadManager.Initialise(new SharedLib.Win.WindowsCpuUsage());
			ThreadManager.MaximumPoolSize = 500;
			ThreadManager.MaximumRunningThreads = 50;
			ThreadManager.ThreadCpuChangeNotification = 0;

			// add plugins which need to be loaded first in a specific order
			//PluginManagerService.UsePlugin(typeof(ErrorManager.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(SystemAdmin.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(BadEgg.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(RestrictIp.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(UserSessionMiddleware.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(CacheControl.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(Spider.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(SeoPlugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(Localization.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(Breadcrumb.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(LoginPlugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(WebSmokeTest.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(GeoIp.Plugin.PluginInitialisation));
			//PluginManagerService.UsePlugin(typeof(ImageManager.Plugin.PluginInitialisation));

			PluginManagerConfiguration configuration = new();

			// Initialise the plugin manager service
			PluginManagerService.Initialise(configuration);
			try
			{
				// Add generic plugins where load order does not matter
				//PluginManagerService.UsePlugin(typeof(HelpdeskPlugin.PluginInitialisation));
				//PluginManagerService.UsePlugin(typeof(UserAccount.Plugin.PluginInitialisation));
				//PluginManagerService.UsePlugin(typeof(Sitemap.Plugin.PluginInitialisation));
				//PluginManagerService.UsePlugin(typeof(DownloadPlugin.PluginInitialisation));
				//PluginManagerService.UsePlugin(typeof(Company.Plugin.PluginInitialisation));
				//PluginManagerService.UsePlugin(typeof(DynamicContent.Plugin.PluginInitialisation));

				CreateWebHostBuilder(args).Build().Run();
			}
			finally
			{
				PluginManagerService.Finalise();
			}
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}

#pragma warning restore CS1591