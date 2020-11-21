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
 *  Product:  MemoryCachePlugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using Shared.Classes;

#pragma warning disable CS1591

namespace MemoryCache.Plugin
{
    /// <summary>
    /// Implements IPlugin which allows the MemoryCache.Plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public class PluginInitialisation : IPlugin
    {
        #region IPlugin Methods

        public void Initialise(ILogger logger)
        {
            ThreadManager.Initialise();
        }

        public void Finalise()
        {
            ThreadManager.Finalise();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.UseMemoryCache();
        }

        public ushort GetVersion()
        {
            return 1;
        }

        #endregion IPlugin Methods
    }
}

#pragma warning restore CS1591