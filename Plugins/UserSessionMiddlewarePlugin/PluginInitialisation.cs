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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin
{
    /// <summary>
    /// Implements IPlugin which allows the UserSessionMiddleware.Plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public sealed class PluginInitialisation : IPlugin
    {
        #region Internal Static Properties

        internal static IServiceProvider GetServiceProvider { get; private set; }

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
            GetLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Finalises the Plugin module
        /// </summary>
        public void Finalise()
        {

        }

        /// <summary>
        /// Allows the Plugin module to configure the Mvc application builder
        /// </summary>
        /// <param name="app">IApplicationBuilder</param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseUserSessionMiddleware();
        }

        /// <summary>
        /// Allows the Plugin module to configure the services for the application
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IUserCultureChangeProvider, UserCultureChanged>();

            GetServiceProvider = services.BuildServiceProvider();
        }

        #endregion IPlugin Methods
    }
}
