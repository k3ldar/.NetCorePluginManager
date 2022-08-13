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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Middleware;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using UserSessionMiddleware.Plugin.Classes;

#pragma warning disable CS1591

namespace UserSessionMiddleware.Plugin
{
    /// <summary>
    /// Implements IPlugin which allows the UserSessionMiddleware.Plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public sealed class PluginInitialisation : IPlugin, IInitialiseEvents
    {
        #region Internal Static Properties

        internal static ILogger GetLogger { get; private set; }

        internal static string[] ValidCultures { get; private set; }


        #endregion Internal Static Properties

        #region IPlugin Methods

        /// <summary>
        /// Initialises the Plugin module
        /// </summary>
        /// <param name="logger">ILogger</param>
        public void Initialise(ILogger logger)
        {
            ThreadManager.Initialise();
            GetLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Finalises the Plugin module
        /// </summary>
        public void Finalise()
        {
            ThreadManager.Finalise();
        }

        /// <summary>
        /// Allows the Plugin module to configure the services for the application
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserCultureChangeProvider, UserCultureChanged>();
        }

        /// <summary>
        /// Returns the internal plugin version number
        /// </summary>
        /// <returns></returns>
        public ushort GetVersion()
        {
            return 1;
        }

        #endregion IPlugin Methods

        #region IInitialiseEvents Methods

        public void AfterConfigure(in IApplicationBuilder app)
        {
            SessionHelper.InitSessionHelper(app.ApplicationServices);
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            ISettingsProvider settingsProvider = services.GetServiceInstance<ISettingsProvider>();

            if (settingsProvider != null)
            {
                UserSessionSettings settings = settingsProvider.GetSettings<UserSessionSettings>(SharedPluginFeatures.Constants.UserSessionConfiguration);

                if (settings.EnableDefaultSessionService && !services.Any(x => x.ServiceType == typeof(IUserSessionService)))
                {
                    services.TryAddSingleton<IUserSessionService, DefaultUserSessionService>();
					services.TryAddSingleton<ISessionStatisticsProvider, DefaultUserSessionService>();
                }
            }
        }

        public void BeforeConfigure(in IApplicationBuilder app)
        {
            app.UseUserSessionMiddleware();
        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {

        }

        public void Configure(in IApplicationBuilder app)
        {

        }

        #endregion IInitialiseEvents Methods
    }
}

#pragma warning restore CS1591