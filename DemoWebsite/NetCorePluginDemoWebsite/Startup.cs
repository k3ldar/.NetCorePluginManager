using System;

using AspNetCore.PluginManager.DemoWebsite.Helpers;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IServiceProvider GetServiceProvider { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            SessionHelper.InitSessionHelper();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = false;
            });

            services.AddLogging();


            // Allow plugin manager to configure all services in each plugiin
            PluginManagerService.ConfigureServices(services);

            // register internal types so we can load them or DI them into other classes later
            services.AddSingleton<ISharedPluginHelper, SharedPluginHelper>();

            // grab an instance of the service provider so we can dynamically generate 
            // objects from the service provider
            GetServiceProvider = services.BuildServiceProvider();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Allow plugin manager to configure options for all plugins
            PluginManagerService.Configure(app, env);

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
