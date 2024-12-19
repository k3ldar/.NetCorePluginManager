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
 *  Product:  SimpleDB
 *  
 *  File: ISimpleDBInitializer.cs
 *
 *  Purpose:  ISimpleDBInitializer interface for SimpleDB
 *
 *  Date        Name                Reason
 *  23/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace SimpleDB
{
	/// <summary>
	/// Interface for managing SimpleDB initialization and other key areas of operation
	/// </summary>
	public interface ISimpleDBManager
	{
		/// <summary>
		/// Path to database files
		/// </summary>
		string Path { get; }

		/// <summary>
		/// Registers a table with DB Manager
		/// </summary>
		/// <param name="simpleDBTable"></param>
		void RegisterTable(ISimpleDBTable simpleDBTable);

		/// <summary>
		/// Unregisters a table from the DB Manager
		/// </summary>
		/// <param name="simpleDBTable"></param>
		void UnregisterTable(ISimpleDBTable simpleDBTable);

		/// <summary>
		/// List of all tables tht have been registered
		/// </summary>
		IReadOnlyDictionary<string, ISimpleDBTable> Tables { get; }

		/// <summary>
		/// Initializes all tables after they have been loaded
		/// </summary>
		void Initialize(IPluginClassesService pluginClassesService);

		/// <summary>
		/// Clears all memory used by tables that have a strategy to retain data in memory
		/// </summary>
		void ClearMemory();

		/// <summary>
		/// Event raised when memory is cleared
		/// </summary>
		event SimpleDbEvent OnMemoryCleared;
	}
}
