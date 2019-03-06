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
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SharedPluginFeatures;

namespace SystemAdmin.Plugin.Classes
{
    public class InitialiseEvents : IInitialiseEvents
    {
        #region IInitialiseEvents Methods

        public void AfterConfigure(in IApplicationBuilder app, in IHostingEnvironment env)
        {
            
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            PluginClass.GetServiceProvider = services.BuildServiceProvider();

            IBreadcrumbService breadcrumbService = PluginClass.GetServiceProvider.GetService<IBreadcrumbService>();
            ISystemAdminHelperService systemAdminHelper = PluginClass.GetServiceProvider.GetService<ISystemAdminHelperService>();

            if (breadcrumbService != null && systemAdminHelper != null)
                RegisterBreadcrumbs(breadcrumbService, systemAdminHelper);
        }

        public void BeforeConfigure(in IApplicationBuilder app, in IHostingEnvironment env)
        {
            
        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {
            
        }

        #endregion IInitialiseEvents Methods

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
                    }

                }
            }
        }

        #endregion Private Methods
    }
}
