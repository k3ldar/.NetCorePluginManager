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

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SieraDeltaGeoIp.Plugin
{
	/// <summary>
	/// Implements IPlugin which allows the SieraDeltaGeoIp.Plugin module to be
	/// loaded as a plugin module
	/// </summary>
	[Obsolete("This package is no longer supported", true)]
    public class PluginInitialisation : IPlugin, IInitialiseEvents
    {
        #region Internal Static Properties

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
			// from interface but unused in this context
		}

		public void ConfigureServices(IServiceCollection services)
        {
            services.UseSieraDeltaGeoIpService();
        }

        public ushort GetVersion()
        {
            return 1;
        }

        #endregion IPlugin Methods

        #region IInitialiseEvents Methods

        public void BeforeConfigure(in IApplicationBuilder app)
        {
			// from interface but unused in this context
		}

		public void AfterConfigure(in IApplicationBuilder app)
        {
            INotificationService notificationService = app.ApplicationServices.GetRequiredService<INotificationService>();
            GeoIpStatistics = new GeoIpStatistics();
            notificationService.RegisterListener(GeoIpStatistics);
        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {
			// from interface but unused in this context
		}

		public void AfterConfigureServices(in IServiceCollection services)
        {
			// from interface but unused in this context
		}

		public void Configure(in IApplicationBuilder app)
        {
			// from interface but unused in this context
		}

		#endregion IInitialiseEvents Methods
	}
}

#pragma warning restore CS1591