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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.DemoWebsite
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AspNetCore.PluginManager.DemoWebsite.Helpers;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Middleware;
using Middleware.Accounts;
using Middleware.Helpdesk;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class PluginInitialisation : IPlugin, IPluginVersion, IInitialiseEvents
    {
        #region IPlugin Methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("DefaultAuthSchemeName")
                .AddCookie("DefaultAuthSchemeName", options =>
                {
                    options.AccessDeniedPath = "/Error/AccessDenied";
                    options.LoginPath = "/Login/";
#if NET_CORE_2_2 || NET_CORE_2_1 || NET_CORE_2_0 || NET461
                    options.SlidingExpiration = true;
                    options.Cookie.Expiration = new TimeSpan(7, 0, 0, 0);
#endif
                });

            services.AddSingleton<IIpValidation, IPValidation>();
            services.AddSingleton<IApplicationProvider, MockApplicationProvider>();
            services.AddSingleton<ILoginProvider, MockLoginProvider>();
            services.AddSingleton<IAccountProvider, MockAccountProvider>();
            services.AddSingleton<ICountryProvider, MockCountryLists>();
            services.AddSingleton<IDownloadProvider, MockDownloads>();
            services.AddSingleton<ILicenceProvider, MockLicenceProvider>();
            services.AddSingleton<IProductProvider, MockProductProvider>();
            services.AddSingleton<IShoppingCartProvider, MockShoppingCartPluginProvider>();
            services.AddSingleton<IShoppingCartService, MockShoppingCartPluginProvider>();
            services.AddSingleton<IErrorManager, ErrorManager>();
            services.AddSingleton<ISharedPluginHelper, SharedPluginHelper>();
            services.AddSingleton<IHelpdeskProvider, MockHelpdeskProvider>();
            services.AddSingleton<ISeoProvider, MockSeoProvider>();
            services.AddSingleton<IStockProvider, MockStockProvider>();
            services.AddSingleton<IBlogProvider, MockBlogProvider>();
            services.AddSingleton<IClaimsProvider, MockClaimsProvider>();
            services.AddSingleton<IUserSearch, MockUserSearch>();
        }

        public void Finalise()
        {

        }

        public void Initialise(ILogger logger)
        {

        }

        public ushort GetVersion()
        {
            return 1;
        }

        #endregion IPlugin Methods

        #region IInitialiseEvents Methods

        public void BeforeConfigure(in IApplicationBuilder app)
        {

        }

        public void AfterConfigure(in IApplicationBuilder app)
        {

        }

        public void Configure(in IApplicationBuilder app)
        {
            app.UseAuthentication();
        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {

        }

        public void AfterConfigureServices(in IServiceCollection services)
        {

        }

        #endregion IInitialiseEvents Methods
    }
}
