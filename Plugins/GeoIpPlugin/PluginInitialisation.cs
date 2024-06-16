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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  GeoIpPlugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using Shared.Classes;

#pragma warning disable CS1591

namespace GeoIp.Plugin
{
	/// <summary>
	/// Implements IPlugin which allows the GeoIp.Plugin module to be
	/// loaded as a plugin module
	/// </summary>
	public class PluginInitialisation : IPlugin
	{
		#region Internal Static Properties

		internal static ILogger GetLogger { get; private set; }

		#endregion Internal Static Properties

		#region IPlugin Methods

		public void Initialise(ILogger logger)
		{
			ThreadManager.Initialise();
			GetLogger = logger;
		}

		public void Finalise()
		{
			if (ThreadManager.IsInitialized)
				ThreadManager.Finalise();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.UseGeoIpService();
		}

		public ushort GetVersion()
		{
			return 1;
		}

		#endregion IPlugin Methods
	}
}

#pragma warning restore CS1591