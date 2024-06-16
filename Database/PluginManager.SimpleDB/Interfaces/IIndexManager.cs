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
 *  File: IIndexManager.cs
 *
 *  Purpose:  Interface for index managers
 *
 *  Date        Name                Reason
 *  09/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures.Interfaces;

namespace SimpleDB
{
	/// <summary>
	/// Index manager definition
	/// </summary>
	public interface IIndexManager : IBatchUpdate
	{
		/// <summary>
		/// Names of properties/columns
		/// </summary>
		List<string> PropertyNames { get; }

		/// <summary>
		/// Type of index
		/// </summary>
		IndexType IndexType { get; }

		/// <summary>
		/// Determines whether the index already exists or not
		/// </summary>
		/// <param name="value">value to be checked</param>
		/// <returns>bool</returns>
		bool Contains(object value);

		/// <summary>
		/// Adds a new index value to the table
		/// </summary>
		/// <param name="value"></param>
		void Add(object value);

		/// <summary>
		/// Adds a list of new data to the table
		/// </summary>
		/// <param name="items"></param>
		void Add(List<object> items);

		/// <summary>
		/// Removes an index value from the table
		/// </summary>
		/// <param name="value"></param>
		void Remove(object value);
	}
}
