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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
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
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.Server.IISIntegration;

using SharedPluginFeatures;

using Middleware;
using Middleware.Accounts;
using Middleware.Helpdesk;

using AspNetCore.PluginManager.DemoWebsite.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
    public class PluginInitialisation : IPlugin, IPluginVersion
    {
        #region IPlugin Methods

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("DefaultAuthSchemeName")
                .AddCookie("DefaultAuthSchemeName", options =>
                {
                    options.AccessDeniedPath = "/Error/AccessDenied";
                    options.LoginPath = "/Login/";
                    options.SlidingExpiration = true;
                    options.Cookie.Expiration = new TimeSpan(7, 0, 0, 0);
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
            services.AddTransient<IClaimsProvider, MockClaimsProvider>();
        }

        public void Finalise()
        {

        }

        public void Initialise(ILogger logger)
        {

        }

        #endregion IPlugin Methods

        #region IPluginVersion Methods

        public ushort GetVersion()
        {
            return (1);
        }

        #endregion IPluginVersion Methods
    }
}