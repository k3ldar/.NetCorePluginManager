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
 *  Product:  SimpleDB
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace SimpleDB
{
	/// <summary>
	/// Plugin initialization class
	/// </summary>
	public class PluginInitialisation : IPlugin, IInitialiseEvents
	{
		#region IPlugin Methods

		/// <summary>
		/// Configure services method
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			// from interface but unused in this context
		}

		/// <summary>
		/// Finalise method
		/// </summary>
		public void Finalise()
		{
			// from interface but unused in this context
		}

		/// <summary>
		/// Initialise method
		/// </summary>
		/// <param name="logger"></param>
		public void Initialise(ILogger logger)
		{
			// from interface but unused in this context
		}

		/// <summary>
		/// Retrieve the plugin version
		/// </summary>
		/// <returns></returns>
		public ushort GetVersion()
		{
			return 1;
		}

		#endregion IPlugin Methods

		#region IInitialiseEvents Methods

		/// <summary>
		/// Method called before configuration of the app
		/// </summary>
		/// <param name="app"></param>
		public void BeforeConfigure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		/// <summary>
		/// Method called after the app has been configured
		/// </summary>
		/// <param name="app"></param>
		public void AfterConfigure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		/// <summary>
		/// Method called when the app is being configured
		/// </summary>
		/// <param name="app"></param>
		public void Configure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		/// <summary>
		/// Method called before services are configured
		/// </summary>
		/// <param name="services"></param>
		public void BeforeConfigureServices(in IServiceCollection services)
		{
			services.AddSimpleDB();
		}

		/// <summary>
		/// Method called after services are being configured
		/// </summary>
		/// <param name="services"></param>
		public void AfterConfigureServices(in IServiceCollection services)
		{
			// from interface but unused in this context
		}

		#endregion IInitialiseEvents Methods
	}
}
