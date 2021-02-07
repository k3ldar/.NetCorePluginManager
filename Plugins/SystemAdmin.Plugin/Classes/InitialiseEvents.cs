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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: InitialiseEvents.cs
 *
 *  Purpose:  Initialisation events
 *
 *  Date        Name                Reason
 *  05/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SystemAdmin.Plugin.Classes
{
    public class InitialiseEvents : IInitialiseEvents, IClaimsService
    {
        #region IInitialiseEvents Methods

        public void AfterConfigure(in IApplicationBuilder app)
        {
            ThreadManager.ThreadStart(new GCAnalysis(), nameof(GCAnalysis), System.Threading.ThreadPriority.Lowest);
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            IBreadcrumbService breadcrumbService = serviceProvider.GetService<IBreadcrumbService>();
            ISystemAdminHelperService systemAdminHelper = serviceProvider.GetService<ISystemAdminHelperService>();

            if (breadcrumbService != null && systemAdminHelper != null)
                RegisterBreadcrumbs(breadcrumbService, systemAdminHelper);

#if NET_CORE_3_X || NET_CORE_5_X
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
                    policyBuilder => policyBuilder.RequireClaim(Constants.ClaimNameAdministrator)
                        .RequireClaim(Constants.ClaimNameStaff)
                        .RequireClaim(Constants.ClaimNameUserId));
            });
#endif
        }

        public void BeforeConfigure(in IApplicationBuilder app)
        {

        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {

        }

        public void Configure(in IApplicationBuilder app)
        {

        }

        #endregion IInitialiseEvents Methods

        #region IClaimsService

        public List<string> GetClaims()
        {
            return new List<string>()
            {
                Constants.ClaimNameStaff,
                Constants.ClaimNameUsername,
                Constants.ClaimNameUserId,
                Constants.ClaimNameUserEmail,
                Constants.ClaimNameAdministrator,
                Constants.ClaimNameManageSeo,
                Constants.ClaimNameUserPermissions,
            };
        }

        #endregion IClaimsService

        #region Private Methods

        private void RegisterBreadcrumbs(in IBreadcrumbService breadcrumbService,
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