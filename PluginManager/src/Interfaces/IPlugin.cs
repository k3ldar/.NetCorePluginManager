/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Product:  PluginManager
 *  
 *  File: IPlugin.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  14/10/2018  Simon Carter        Moved to SharedPluginFeatures
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.Extensions.DependencyInjection;

namespace PluginManager.Abstractions
{
	/// <summary>
	/// This interface should be implemented by each plugin module, without this interface being 
	/// implemented AspNetCore.PluginManager would not automatically load it when the appliction
	/// starts.
	/// 
	/// This class will be called by the Plugin Manager and will provide an opportunity for the 
	/// plugin to configure its services and application requirements within the MVC application.
	/// </summary>
	public interface IPlugin : IPluginVersion
	{
		/// <summary>
		/// Notifies the plugin that the plugin module is being initialised.
		/// </summary>
		/// <param name="logger">ILogger instance used to hold log data, should the plugin
		/// need to log any intialisation data.  Each plugin should store this instance
		/// for later use if logging is a requirement.</param>
		void Initialise(ILogger logger);

		/// <summary>
		/// Notifies the plugin module that it is being closed and removed and it should
		/// uninitialise any resources that it has used.
		/// </summary>
		void Finalise();

		/// <summary>
		/// Provides the plugin module with an opportunity to register any services
		/// that it provides.
		/// </summary>
		/// <param name="services">IServiceCollection instance where further services can be registerd.</param>
		void ConfigureServices(IServiceCollection services);
	}
}
