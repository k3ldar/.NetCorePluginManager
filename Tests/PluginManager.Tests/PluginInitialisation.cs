/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  15/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

#pragma warning disable CS1591

namespace PluginManager.Tests
{
	/// <summary>
	/// Implements IPlugin which allows the RestrictIp.Plugin module to be
	/// loaded as a plugin module
	/// </summary>
	[ExcludeFromCodeCoverage]
	public sealed class PluginInitialisation : IPlugin
	{
		#region IPlugin Methods

		public void Initialise(ILogger logger)
		{
			// from interface but unused in this context
			// from interface but unused in this context
		}

		public void Finalise()
		{
			// from interface but unused in this context
		}

		public void ConfigureServices(IServiceCollection services)
		{
			// from interface but unused in this context
		}

		public ushort GetVersion()
		{
			return 1;
		}

		#endregion IPlugin Methods
	}
}

#pragma warning restore CS1591