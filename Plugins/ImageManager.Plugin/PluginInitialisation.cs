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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Image Manager Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  Net Core Plugin Manager Integration
 *
 *  Date        Name                Reason
 *  15/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using ImageManager.Plugin.Classes;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Middleware.Interfaces;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace ImageManager.Plugin
{
	/// <summary>
	/// Implements IPlugin and IInitialiseEvents which allows the ImageManager.Plugin module to be
	/// loaded as a plugin module
	/// </summary>
	public class PluginInitialisation : IPlugin, IInitialiseEvents, IClaimsService
	{
		#region IPlugin Methods

		public void ConfigureServices(IServiceCollection services)
		{
			// from interface but unused in this context
		}

		public void Finalise()
		{
			if (ThreadManager.IsInitialized)
				ThreadManager.Finalise();
		}

		public ushort GetVersion()
		{
			return 1;
		}

		public void Initialise(ILogger logger)
		{
			ThreadManager.Initialise();
		}

		#endregion IPlugin

		#region IInitialiseEvents Methods

		public void AfterConfigure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		public void AfterConfigureServices(in IServiceCollection services)
		{
			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					Constants.PolicyNameViewImageManager,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameViewImageManager)
						.RequireClaim(Constants.ClaimNameStaff)
						.RequireClaim(Constants.ClaimNameUsername)
						.RequireClaim(Constants.ClaimNameUserId)
						.RequireClaim(Constants.ClaimNameUserEmail));

				options.AddPolicy(
					Constants.PolicyNameImageManagerManage,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameManageImages)
						.RequireClaim(Constants.ClaimNameStaff)
						.RequireClaim(Constants.ClaimNameUsername)
						.RequireClaim(Constants.ClaimNameUserId)
						.RequireClaim(Constants.ClaimNameUserEmail));
			});

			services.TryAddSingleton<IImageProvider, DefaultImageProvider>();
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
				Constants.ClaimNameViewImageManager,
				Constants.ClaimNameManageImages,
			};
		}

		#endregion IClaimsService
	}
}

#pragma warning restore CS1591