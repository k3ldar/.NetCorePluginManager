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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: InitialiseEvents.cs
 *
 *  Purpose:  Initialisation events
 *
 *  Date        Name                Reason
 *  07/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace UserSessionMiddleware.Plugin.Classes
{
    public class InitialiseEvents : IInitialiseEvents
    {
        #region IInitialiseEvents Methods

        public void AfterConfigure(in IApplicationBuilder app)
        {

        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            ISettingsProvider settingsProvider = serviceProvider.GetService<ISettingsProvider>();

            if (settingsProvider != null)
            {
                UserSessionSettings settings = settingsProvider.GetSettings<UserSessionSettings>(Constants.UserSessionConfiguration);

                if (settings.EnableDefaultSessionService && !services.Any(x => x.ServiceType == typeof(IUserSessionService)))
                {
                    services.TryAddSingleton<IUserSessionService, DefaultUserSessionService>();
                }
            }

            SessionHelper.InitSessionHelper(services.BuildServiceProvider());
        }

        public void BeforeConfigure(in IApplicationBuilder app)
        {
            app.UseUserSessionMiddleware();
        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {

        }

        public void Configure(in IApplicationBuilder app)
        {

        }

        #endregion IInitialiseEvents Methods
    }
}

#pragma warning restore CS1591