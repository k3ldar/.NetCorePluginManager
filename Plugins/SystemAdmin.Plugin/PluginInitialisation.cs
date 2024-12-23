﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using SystemAdmin.Plugin.Classes;
using SystemAdmin.Plugin.Classes.MenuItems;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin
{
	/// <summary>
	/// Implements IPlugin and IPluginVersion which allows the SystemAdmin.Plugin module to be
	/// loaded as a plugin module
	/// </summary>
	public class PluginInitialisation : IPlugin, IInitialiseEvents, IClaimsService
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ISystemAdminHelperService, SystemAdminHelper>();
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
			// ensure load time is set
			SystemUptimeMenu uptimeMenu = new();
			GC.KeepAlive(uptimeMenu.Data());
		}

		#region IInitialiseEvents Methods

		public void AfterConfigure(in IApplicationBuilder app)
		{
			ThreadManager.ThreadStart(new GCAnalysis(), nameof(GCAnalysis), System.Threading.ThreadPriority.Lowest);

			IBreadcrumbService breadcrumbService = app.ApplicationServices.GetService<IBreadcrumbService>();
			ISystemAdminHelperService systemAdminHelper = app.ApplicationServices.GetService<ISystemAdminHelperService>();

			if (breadcrumbService != null && systemAdminHelper != null)
				RegisterBreadcrumbs(breadcrumbService, systemAdminHelper);
		}

		public void AfterConfigureServices(in IServiceCollection services)
		{
			if (services == null)
				throw new ArgumentNullException(nameof(services));
#if NET_6_0
			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					Constants.PolicyNameAlterSeoData,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameManageSeo)
						.RequireClaim(Constants.ClaimNameStaff)
						.RequireClaim(Constants.ClaimNameUsername)
						.RequireClaim(Constants.ClaimNameUserId)
						.RequireClaim(Constants.ClaimNameUserEmail));
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					Constants.PolicyNameManagePermissions,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameAdministrator)
						.RequireClaim(Constants.ClaimNameUserPermissions)
						.RequireClaim(Constants.ClaimNameStaff));

				options.AddPolicy(
					Constants.PolicyNameStaff,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameStaff)
						.RequireClaim(Constants.ClaimNameUserId));
			});

			services.AddAuthorization(options =>
			{
				options.AddPolicy(
					Constants.PolicyNameManageSystemSettings,
					policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameManageSystemSettings)
						.RequireClaim(Constants.ClaimNameAdministrator)
						.RequireClaim(Constants.ClaimNameStaff)
						.RequireClaim(Constants.ClaimNameUsername)
						.RequireClaim(Constants.ClaimNameUserId)
						.RequireClaim(Constants.ClaimNameUserEmail));
			});
#else
			services.AddAuthorizationBuilder()
				.AddPolicy(Constants.PolicyNameAlterSeoData, policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameManageSeo)
					.RequireClaim(Constants.ClaimNameStaff)
					.RequireClaim(Constants.ClaimNameUsername)
					.RequireClaim(Constants.ClaimNameUserId)
					.RequireClaim(Constants.ClaimNameUserEmail));

			services.AddAuthorizationBuilder()
				.AddPolicy(Constants.PolicyNameManagePermissions, policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameAdministrator)
					.RequireClaim(Constants.ClaimNameUserPermissions)
					.RequireClaim(Constants.ClaimNameStaff))
				.AddPolicy(Constants.PolicyNameStaff, policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameStaff)
					.RequireClaim(Constants.ClaimNameUserId));

			services.AddAuthorizationBuilder()
				.AddPolicy(Constants.PolicyNameManageSystemSettings, policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameManageSystemSettings)
					.RequireClaim(Constants.ClaimNameAdministrator)
					.RequireClaim(Constants.ClaimNameStaff)
					.RequireClaim(Constants.ClaimNameUsername)
					.RequireClaim(Constants.ClaimNameUserId)
					.RequireClaim(Constants.ClaimNameUserEmail));
#endif
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
			return
			[
				Constants.ClaimNameStaff,
				Constants.ClaimNameUsername,
				Constants.ClaimNameUserId,
				Constants.ClaimNameUserEmail,
				Constants.ClaimNameAdministrator,
				Constants.ClaimNameManageSeo,
				Constants.ClaimNameUserPermissions,
				Constants.ClaimNameManageSystemSettings,
			];
		}

		#endregion IClaimsService

		#region Private Methods

		private static void RegisterBreadcrumbs(in IBreadcrumbService breadcrumbService,
			in ISystemAdminHelperService systemAdminHelper)
		{
			string parentRoute = "/SystemAdmin";
			breadcrumbService.AddBreadcrumb(nameof(Languages.LanguageStrings.SystemAdmin), parentRoute, false);

			foreach (SystemAdminMainMenu item in systemAdminHelper.GetSystemAdminMainMenu())
			{
				string route = $"/SystemAdmin/Index/{item.UniqueId}";
				breadcrumbService.AddBreadcrumb(item.Name, route, parentRoute, false);

				foreach (SystemAdminSubMenu childItem in item.ChildMenuItems)
				{
					switch (childItem.MenuType())
					{
						case Enums.SystemAdminMenuType.Grid:
							breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/Grid/{childItem.UniqueId}", route, false);
							break;
						case Enums.SystemAdminMenuType.Text:
							breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/Text/{childItem.UniqueId}", route, false);
							break;
						case Enums.SystemAdminMenuType.PartialView:
							breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/View/{childItem.UniqueId}", route, false);
							break;
						case Enums.SystemAdminMenuType.Map:
							breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/Map/{childItem.UniqueId}", route, false);
							break;
						case Enums.SystemAdminMenuType.FormattedText:
							breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/TextEx/{childItem.UniqueId}", route, false);
							break;
						case Enums.SystemAdminMenuType.View:
							breadcrumbService.AddBreadcrumb(childItem.Name(), $"/{childItem.Controller()}/{childItem.Action()}/", route, false);
							break;
						case Enums.SystemAdminMenuType.Chart:
							breadcrumbService.AddBreadcrumb(childItem.Name(), $"/SystemAdmin/Chart/{childItem.UniqueId}", route, false);
							break;
					}

				}
			}
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591