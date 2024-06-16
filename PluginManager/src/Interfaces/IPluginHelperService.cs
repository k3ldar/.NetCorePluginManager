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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: IPluginHelperService.cs
 *
 *  Purpose:  Provides interface for retrieving plugin specific data from the plugin manager
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *  28/04/2019  Simon Carter        #63 Allow plugin to be dynamically added.
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Reflection;

namespace PluginManager.Abstractions
{
	/// <summary>
	/// Provides a mechanism for the host application or other plugin modules to query the 
	/// AspNetCore.PluginManager for specific data.
	/// 
	/// This interface is implemented by the Plugin Manager and registered for use within the 
	/// DI contianer when loading.
	/// </summary>
	public interface IPluginHelperService
	{
		/// <summary>
		/// Determines whether a plugin module has been loaded.
		/// </summary>
		/// <param name="pluginLibraryName">The name of the plugin module, i.e. SeoPlugin.dll</param>
		/// <param name="version">out int.  Returns the internal plugin version of the plugin if found.</param>
		/// <returns>bool</returns>
		bool PluginLoaded(in string pluginLibraryName, out int version);

		/// <summary>
		/// Dynamically adds a non plugin assembly to the list of managed plugins.
		/// </summary>
		/// <param name="assembly">Assembly instance of assembly to be added to the list of available plugins.</param>
		/// <returns></returns>
		DynamicLoadResult AddAssembly(in Assembly assembly);
	}
}
