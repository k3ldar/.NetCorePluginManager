/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: IServiceConfigurator.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/08/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

namespace PluginManager.Abstractions
{
    /// <summary>
    /// Provides an opportunity for only one plugin (or host) to register for final service configuration.
    /// 
    /// This would allow for the host to remove and re-add any specific services which it needs specific
    /// control over.
    /// </summary>
    public interface IServiceConfigurator
    {
        /// <summary>
        /// Method called when all plugins have registered services which can allow the host, or a specific plugin 
        /// with the ability to get notified after all services have been created.
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        void RegisterServices(IServiceCollection services);
    }
}
