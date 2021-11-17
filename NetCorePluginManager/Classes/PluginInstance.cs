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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PluginInstance.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager
{
    internal sealed class PluginInstance : IPlugin, IInitialiseEvents
    {
        public void AfterConfigure(in IApplicationBuilder app)
        {

        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            //services.AddSingleton<IAuthorizationHandler, Handlers.ApiAuthorizationHandler>();
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy(Constants.PolicyNameApiAuthorization,
            //        policy => policy.AddRequirements(new Handlers.ApiAuthorizationHandler()));
            //});
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

        public void ConfigureServices(IServiceCollection services)
        {
        }

        public void Finalise()
        {

        }

        public ushort GetVersion()
        {
            return 1;
        }

        public void Initialise(ILogger logger)
        {

        }
    }
}
