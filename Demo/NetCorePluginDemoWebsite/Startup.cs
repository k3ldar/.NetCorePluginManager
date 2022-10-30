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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: Startup.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using AspNetCore.PluginManager.DemoWebsite.Helpers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.PluginManager.DemoWebsite
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
            // Allow plugin manager to configure all services in each plugin
            PluginManagerService.ConfigureServices(services);

            SessionHelper.InitSessionHelper();

            services.AddAuthentication("DefaultAuthSchemeName")
                .AddCookie("DefaultAuthSchemeName", options =>
                {
                    options.Cookie.Name = "pm_session";
                    options.AccessDeniedPath = "/Error/AccessDenied";
                    options.LoginPath = "/Login/";

#if NET_CORE_2_2 || NET_CORE_2_1 || NET_CORE_2_0 || NET461
                    options.SlidingExpiration = true;
                    options.Cookie.Expiration = new TimeSpan(7, 0, 0, 0);
#else
                    options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);
#endif
                });

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                //options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = false;
            });

            services.AddLogging();
            services.AddMvc(
                option => option.EnableEndpointRouting = false
                )
                .ConfigurePluginManager()
                .AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // Allow plugin manager to configure options for all plugins
            PluginManagerService.Configure(app);

            app.UseStaticFiles();

            app.UseHsts();
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            }).UsePluginManager();
        }
    }
}
