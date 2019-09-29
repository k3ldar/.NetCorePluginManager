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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatues
 *  
 *  File: IInitialiseEvents.cs
 *
 *  Purpose:  Configure Events
 *
 *  Date        Name                Reason
 *  05/03/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Provides initialisation events that can be used by plugins to affect the configuration
    /// of services and the application.
    /// 
    /// This interface is designed to give pre and post notifications, each plugin module that 
    /// implements IPlugin will automatically receive the events as part of initialisation.
    /// 
    /// This is particularly useful should a plugin module wish to register an interface for 
    /// example that is required when another plugin module is being configured or to provide
    /// a default implementation of an interface should no plugin have registered one whilst 
    /// being configured.
    /// </summary>
    public interface IInitialiseEvents
    {
        /// <summary>
        /// Indicates that the Configure method will be called on IPlugin instances.
        /// </summary>
        /// <param name="app">IApplicationBuilder instance.</param>
        void BeforeConfigure(in IApplicationBuilder app);

        /// <summary>
        /// Indicates that all plugins have had an opportunity to load configuration data.
        /// </summary>
        /// <param name="app">IApplicationBuilder instance.</param>
        void AfterConfigure(in IApplicationBuilder app);

        /// <summary>
        /// Indicates that the ConfigureServices method on each IPlugin implementation will
        /// be called.
        /// </summary>
        /// <param name="services">IServiceCollection instance.</param>
        void BeforeConfigureServices(in IServiceCollection services);

        /// <summary>
        /// Indicates that all IPlugin instances have had an opportunity to configure services.
        /// </summary>
        /// <param name="services">IServiceCollection instance.</param>
        void AfterConfigureServices(in IServiceCollection services);
    }
}
