/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Products.Plugin
 *  
 *  File: Startup.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using AspNetCore.PluginManager;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Shared.Classes;

#pragma warning disable CS1591, CA1822

namespace ProductPlugin
{
	public class Startup
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public Startup()
		{
			ThreadManager.Initialise();

			if (!PluginManagerService.HasInitialised)
				PluginManagerService.Initialise();

			PluginManagerService.UsePlugin(typeof(PluginInitialisation));
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			PluginManagerService.ConfigureServices(services);

			services.AddMvc(
				option => option.EnableEndpointRouting = false
				)
				.ConfigurePluginManager();
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="app"></param>
		public void Configure(IApplicationBuilder app)
		{
			if (app == null)
				throw new ArgumentNullException(nameof(app));

			PluginManagerService.Configure(app);

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Product}/{action=Index}/{id?}");
			})
			.UsePluginManager();
		}
	}
}

#pragma warning disable CS1591, CA1822