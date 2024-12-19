/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Blog Plugin
 *  
 *  File: InitialiseEvents.cs
 *
 *  Purpose:  Allows blog plugin to configure policies 
 *
 *  Date        Name                Reason
 *  03/08/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Blog.Plugin.Classes
{
	/// <summary>
	/// Implements IInitialiseEvents which allows the Blog module to configure policies
	/// </summary>
	public class InitialiseEvents : IInitialiseEvents, IClaimsService
	{
		#region IInitialiseEvents

		public void AfterConfigure(in IApplicationBuilder app)
		{
			// required by interface not used in this implementation
		}

		public void AfterConfigureServices(in IServiceCollection services)
		{
#if NET_6_0
			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					Constants.PolicyNameBlogCreate,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameCreateBlog));
				options.AddPolicy(
					Constants.PolicyNameBlogRespond,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameUsername)
						.RequireClaim(Constants.ClaimNameUserId)
						.RequireClaim(Constants.ClaimNameUserEmail));
			});
#else
			// Add blog specific policies
			services.AddAuthorizationBuilder()
				.AddPolicy(Constants.PolicyNameBlogCreate, policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameCreateBlog))
				.AddPolicy(Constants.PolicyNameBlogRespond, policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameUsername)
				.RequireClaim(Constants.ClaimNameUserId)
				.RequireClaim(Constants.ClaimNameUserEmail));
#endif
		}

			public void BeforeConfigure(in IApplicationBuilder app)
		{
			// required by interface not used in this implementation
		}

		public void BeforeConfigureServices(in IServiceCollection services)
		{
			// required by interface not used in this implementation
		}

		public void Configure(in IApplicationBuilder app)
		{
			// required by interface not used in this implementation
		}

#endregion IInitialiseEvents

		#region IClaimsService

		public List<string> GetClaims()
		{
			return
			[
				Constants.ClaimNameCreateBlog,
				Constants.ClaimNameUsername,
				Constants.ClaimNameUserId,
				Constants.ClaimNameUserEmail,
				Constants.ClaimIdentityBlog
			];
		}

		#endregion IClaimsService
	}
}

#pragma warning restore CS1591