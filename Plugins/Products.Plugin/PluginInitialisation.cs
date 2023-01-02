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
 *  Product:  Products.Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  Net Core Plugin Manager Integration
 *
 *  Date        Name                Reason
 *  31/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Middleware.Interfaces;

using PluginManager.Abstractions;

using ProductPlugin.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ProductPlugin
{
    /// <summary>
    /// Implements IPlugin and IPluginVersion which allows the ProductsPlugin module to be
    /// loaded as a plugin module
    /// </summary>
    public class PluginInitialisation : IPlugin, IInitialiseEvents, IClaimsService
	{
        #region IPlugin

        public void ConfigureServices(IServiceCollection services)
        {
			// from interface but unused in this context
		}

		public void Finalise()
        {
			// from interface but unused in this context
		}

		public ushort GetVersion()
        {
            return 1;
        }

        public void Initialise(ILogger logger)
        {
			// from interface but unused in this context
		}

		#endregion IPlugin

		#region IInitialiseEvents Methods

		public void AfterConfigure(in IApplicationBuilder app)
        {
            INotificationService notificationService = app.ApplicationServices.GetService<INotificationService>();
            IImageProvider imageProvider = app.ApplicationServices.GetService<IImageProvider>();
            ISettingsProvider settingsProvider = app.ApplicationServices.GetService<ISettingsProvider>();

            if (imageProvider != null)
                notificationService.RegisterListener(new ImageUploadNotificationListener(imageProvider, settingsProvider));
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddAuthorization(options => 
            {
                options.AddPolicy(
                    Constants.PolicyNameManageProducts,
                    policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameAdministrator)
                        .RequireClaim(Constants.ClaimNameManageProducts)
                        .RequireClaim(Constants.ClaimNameUsername)
                        .RequireClaim(Constants.ClaimNameUserId)
                        .RequireClaim(Constants.ClaimNameUserEmail)
                        .RequireClaim(Constants.ClaimNameStaff));
            });
        }

        public void BeforeConfigure(in IApplicationBuilder app)
        {
			// from interface but unused in this context
		}

		public void BeforeConfigureServices(in IServiceCollection services)
        {
			// from interface but unused in this context
		}

		public void Configure(in IApplicationBuilder app)
        {
			// from interface but unused in this context
		}

		#endregion IInitialiseEvents Methods

		#region IClaimsService

		public List<string> GetClaims()
		{
			return new List<string>()
			{ 
				Constants.ClaimNameManageProducts,
			};
		}

		#endregion IClaimsService
	}
}

#pragma warning restore CS1591