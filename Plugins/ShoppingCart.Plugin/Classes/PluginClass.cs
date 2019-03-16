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
 *  Copyright (c) 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Shopping Cart Plugin
 *  
 *  File: PluginClass.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  06/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace ShoppingCartPlugin
{
    public class PluginClass : IPlugin, IPluginVersion, IInitialiseEvents
    {
        #region Static Internal Members

        internal static IServiceProvider GetServiceProvider { get; private set; }

        internal static ILogger Logger { get; private set; }

        #endregion Static Internal Members

        #region IPlugin Methods

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseShoppingCart();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
        }

        public void Finalise()
        {
            
        }

        public void Initialise(ILogger logger)
        {
            Logger = logger;
        }

        #endregion IPlugin Methods

        #region IPluginVersion Methods

        public ushort GetVersion()
        {
            return (1);
        }

        #endregion IPluginVersion Methods

        #region IInitialiseEvents Methods

        public void BeforeConfigure(in IApplicationBuilder app, in IHostingEnvironment env)
        {
            
        }

        public void AfterConfigure(in IApplicationBuilder app, in IHostingEnvironment env)
        {
            
        }

        public void BeforeConfigureServices(in IServiceCollection services)
        {
            
        }

        public void AfterConfigureServices(in IServiceCollection services)
        {
            PluginClass.GetServiceProvider = services.BuildServiceProvider();
        }

        #endregion IInitialiseEvents Methods
    }
}
