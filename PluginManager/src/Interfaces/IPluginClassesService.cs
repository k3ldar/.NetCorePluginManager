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
 *  File: IPluginClassesService.cs
 *
 *  Purpose:  Provides interface for retrieving plugin specific data from the plugin manager
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace PluginManager.Abstractions
{
	/// <summary>
	/// IPluginClassesService is implemented by the AspNetCore.PluginManager and is available
	/// using DI.
	/// 
	/// This interface provides a conduit for the application or other plugins to quickly 
	/// search all plugins for classes that implement or extend a specific class or interface.
	/// 
	/// This can be particularly useful if for instance you define menu items within plugins, 
	/// the host can search all plugins for the menu class, and get a list of all objects, 
	/// instantiated or not.
	/// 
	/// The plugin can then use the interfaces to dynamically create menu items.
	/// </summary>
	public interface IPluginClassesService
	{
		/// <summary>
		/// Retrieves a list of instantiated classes that either descend from, or 
		/// implement T.
		/// 
		/// If a class requires parameters for instantiating, then they will be sought
		/// from the DI container, as long as the parameters are available then an 
		/// instance of the class will be created.
		/// </summary>
		/// <typeparam name="T">Class or interface to be searched for.</typeparam>
		/// <returns>List&lt;T&gt;</returns>
		List<T> GetPluginClasses<T>();

		/// <summary>
		/// Retrieves a list of classes as types, that either descend from or implement
		/// T.
		/// 
		/// This method will not create instances of the classes found, instead it will 
		/// return the list and the calling module can choose how it wants to use them.
		/// </summary>
		/// <typeparam name="T">Class or interface to be searched for.</typeparam>
		/// <returns>List&lt;Type&gt;</returns>
		List<Type> GetPluginClassTypes<T>();

		/// <summary>
		/// Retrieves an array of instantiated parameters for a class type
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		object[] GetParameterInstances(Type type);
	}
}
