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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Blog Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  Net Core Plugin Manager Integration
 *
 *  Date        Name                Reason
 *  20/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

#pragma warning disable CS1591

#if NET_CORE_2_2
#pragma warning disable CS0618, IDE0060
#endif

namespace Blog.Plugin
{
    /// <summary>
    /// Implements IPlugin and IPluginVersion which allows the Blog.Plugin module to be
    /// loaded as a plugin module
    /// </summary>
    public class PluginInitialisation : IPlugin, IPluginVersion
    {
        public void ConfigureServices(IServiceCollection services)
        {
			return;
		}

		public void Finalise()
        {
			return;
		}

		public ushort GetVersion()
        {
            return (1);
        }

        public void Initialise(ILogger logger)
        {
			return;
		}
	}
}

#if NET_CORE_2_2
#pragma warning restore CS0618
#endif

#pragma warning restore CS1591, IDE0060