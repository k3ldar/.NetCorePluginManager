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
 *  Product:  Error Manager Plugin
 *  
 *  File: Startup.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CS1591

namespace ErrorManager.Plugin
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc(
#if NET_CORE_3_X || NET_5_ABOVE
				option => option.EnableEndpointRouting = false
#endif
				);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1801:ReviewUnusedParameters", MessageId = "Reviewed and ok in this context")]
		public void Configure(IApplicationBuilder app,
#if NET_CORE_3_X || NET_5_ABOVE
			IWebHostEnvironment env)
#else
            IHostingEnvironment env)
#endif
		{
			app.UseErrorManager();

#if !NET_CORE_3_X
			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
#endif
		}
	}
}

#pragma warning restore CS1591