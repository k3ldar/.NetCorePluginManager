﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserAccount.Plugin
 *  
 *  File: Startup.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AspNetCore.PluginManager;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserAccount.Plugin
{
#pragma warning disable CS1591

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public static void ConfigureServices(IServiceCollection services)
		{
			// Allow plugin manager to configure all services in each plugin
			PluginManagerService.ConfigureServices(services);

			//services.Configure<CookiePolicyOptions>(options =>
			//{
			//    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
			//    options.CheckConsentNeeded = context => true;
			//    options.MinimumSameSitePolicy = SameSiteMode.None;
			//});

			services.AddDistributedMemoryCache();

			//services.AddSession(options =>
			//{
			//    // Set a short timeout for easy testing.
			//    options.IdleTimeout = TimeSpan.FromSeconds(10);
			//    options.Cookie.HttpOnly = false;
			//});


			services.AddMvc(
				option => option.EnableEndpointRouting = false
				)
				.ConfigurePluginManager();
		}

		public static void Configure(IApplicationBuilder app)
		{
			// Allow plugin manager to configure options for all plugins
			PluginManagerService.Configure(app);

			//app.UseHsts();
			//app.UseHttpsRedirection();
			//app.UseStaticFiles();
			//app.UseCookiePolicy();
			//app.UseSession();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Account}/{action=Index}/{id?}");
			});
		}
	}

#pragma warning restore CS1591
}
