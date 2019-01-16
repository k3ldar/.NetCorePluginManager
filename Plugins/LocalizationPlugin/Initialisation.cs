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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Localization.Plugin
 *  
 *  File: Initialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  09/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Localization;

using SharedPluginFeatures;

using Languages;

using Shared.Classes;

namespace Localization.Plugin
{
    public class Initialisation : IPlugin, IConfigureApplicationBuilder, IConfigureMvcBuilder
    {
        #region Constructors

        public Initialisation()
        {
        }

        #endregion Constructors

        #region Internal Static Properties / Members

        internal static CacheManager CultureCache = new CacheManager("Available Cultures", new TimeSpan(24, 0, 0), true, true);
        internal static ILogger GetLogger { get; private set; }
        internal static IServiceProvider GetServiceProvider { get; private set; }
        internal static string[] InstalledCultures { get; private set; }

        #endregion Internal Static Properties / Members

        #region IPlugin Methods

        public void Initialise(ILogger logger)
        {
            GetLogger = logger;
        }

        public void Finalise()
        {

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseRequestLocalization().UseLocalizationMiddleware();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICultureProvider, CultureProvider>();
            services.AddSingleton<IStringLocalizerFactory, StringLocalizerFactory>();

            GetServiceProvider = services.BuildServiceProvider();

            IHostingEnvironment environment = GetServiceProvider.GetRequiredService<IHostingEnvironment>();

            InstalledCultures = LanguageWrapper.GetInstalledLanguages(environment.ContentRootPath);

            List<CultureInfo> cultures = new List<CultureInfo>();

            for (int i = 1; i < InstalledCultures.Length; i++)
                cultures.Add(new CultureInfo(InstalledCultures[i]));

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    opts.DefaultRequestCulture = new RequestCulture(InstalledCultures[0]);
                    opts.SupportedCultures = cultures;
                    opts.SupportedUICultures = cultures;
                });
        }

        #endregion IPlugin Methods

        #region IConfigureApplicationBuilder Methods

        public void ConfigureApplicationBuilder(in IApplicationBuilder applicationBuilder)
        {

        }

        #endregion IConfigureApplicationBuilder Methods

        #region IConfigureMvcBuilder Methods

        public void ConfigureMvcBuilder(in IMvcBuilder mvcBuilder)
        {
            mvcBuilder
                .AddViewLocalization(
                    LanguageViewLocationExpanderFormat.Suffix,
                    opts => { opts.ResourcesPath = "Resources"; })
                .AddDataAnnotationsLocalization();
        }

        #endregion IConfigureMvcBuilder Methods
    }
}
