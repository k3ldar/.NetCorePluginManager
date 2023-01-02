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
 *  Product:  Resources.Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Resources.Plugin
{
    /// <summary>
    /// Implements IPlugin which allows the Resources.Plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public sealed class PluginInitialisation : IPlugin, IInitialiseEvents, IClaimsService
	{
        #region Constructors

        public PluginInitialisation()
        {
        }

        #endregion Constructors

        #region IInitialiseEvents Methods

        public void AfterConfigure(in IApplicationBuilder app)
        {
			// from interface but unused in this context
		}

		public void AfterConfigureServices(in IServiceCollection services)
        {
			if (services == null)
				throw new ArgumentNullException(nameof(services));

			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					Constants.PolicyNameAddResources,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameAddResources)
						.RequireClaim(Constants.ClaimNameUserId)
						.RequireClaim(Constants.ClaimNameUserEmail));
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					Constants.PolicyNameManageResources,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameManageResources)
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

		#region IPlugin Methods

		public void Initialise(ILogger logger)
        {
			// from interface but unused in this context
		}

		public void Finalise()
        {
			// from interface but unused in this context
		}

		public void ConfigureServices(IServiceCollection services)
        {
			// from interface but unused in this context
		}

		public ushort GetVersion()
        {
            return 1;
        }

		#endregion IPlugin Methods

		#region IClaimsService

		public List<string> GetClaims()
		{
			return new List<string>()
			{
				Constants.ClaimNameAddResources,
				Constants.ClaimNameManageResources,
			};
		}

		#endregion IClaimsService
	}
}

#pragma warning restore CS1591