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
*  File: IPluginModule.cs
*
*  Purpose:  
*
*  Date        Name                Reason
*  22/09/2018  Simon Carter        Initially Created
*  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
*                                  application type.  Originally part of .Net 
*                                  Core Plugin Manager (AspNetCore.PluginManager)
*
* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Reflection;

namespace PluginManager.Abstractions
{
	/// <summary>
	/// Interface representing a plugin module that has been loaded using PluginManager
	/// </summary>
	public interface IPluginModule
	{
		/// <summary>
		/// Plugin version, this is the internal plugin version not the file version
		/// </summary>
		/// <value>ushort</value>
		ushort Version { get; }


		/// <summary>
		/// Assembly name and location
		/// </summary>
		/// <value>string</value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Already out in the wild and would case breaking compatibility!")]
		string Module { get; }

		/// <summary>
		/// Assembly instance representing the Plugin module
		/// </summary>
		/// <value>Assembly</value>
		Assembly Assembly { get; }

		/// <summary>
		/// The plugin modules IPlugin interface
		/// </summary>
		/// <value>IPlugin</value>
		IPlugin Plugin { get; }

		/// <summary>
		/// Current version of assembly
		/// </summary>
		string FileVersion { get; }
	}
}
