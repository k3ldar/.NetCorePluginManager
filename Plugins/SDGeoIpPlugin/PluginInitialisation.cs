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
 *  Product:  SieraDeltaGeoIpPlugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SieraDeltaGeoIp.Plugin
{
    /// <summary>
    /// Implements IPlugin which allows the SieraDeltaGeoIp.Plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public class PluginInitialisation : IPlugin, IInitialiseEvents
    {
        #region Internal Static Properties

        internal static IServiceProvider GetServiceProvider { get; private set; }

        internal static ILogger GetLogger { get; private set; }

        internal static GeoIpStatistics GeoIpStatistics { get; private set; }

        #endregion Internal Static Properties

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

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseSieraDeltaGeoIpService();

            GetServiceProvider = services.BuildServiceProvider();
        }

        #endregion IPlugin Methods

        #region IInitialiseEvents Methods

        public void BeforeConfigure(in IApplicationBuilder app, in IHostingEnvironment env)
        {

        }

        public void AfterConfigure(in IApplicationBuilder app, in IHostingEnvironment env)
        {

        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {

        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            INotificationService notificationService = serviceProvider.GetRequiredService<INotificationService>();
            GeoIpStatistics = new GeoIpStatistics();
            notificationService.RegisterListener(GeoIpStatistics);
        }

        #endregion IInitialiseEvents Methods
    }
}

#pragma warning restore CS1591