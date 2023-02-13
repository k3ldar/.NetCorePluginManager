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
 *  File: ForeignKeyManager.cs
 *
 *  Purpose:  Manages foreign keys
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Foreign key manager interface
	/// </summary>
    public interface IForeignKeyManager
    {
		/// <summary>
		/// Adds a relationship between two tables
		/// </summary>
		/// <param name="sourceTable">Source table</param>
		/// <param name="targetTable">Target table that contains the foreign key</param>
		/// <param name="propertyName">Name of propertyy</param>
		/// <param name="targetPropertyName">Name of property used as foreign key</param>
		void AddRelationShip(string sourceTable, string targetTable, string propertyName, string targetPropertyName);

		/// <summary>
		/// Registers a table with foreign key manager
		/// </summary>
		/// <param name="table">Table</param>
        void RegisterTable(ISimpleDBTable table);

		/// <summary>
		/// Unregisters a table
		/// </summary>
		/// <param name="table">Table</param>
        void UnregisterTable(ISimpleDBTable table);

		/// <summary>
		/// Validates whether a value that is or could be used for a foreign key exists
		/// </summary>
		/// <param name="tableName">Name of table</param>
		/// <param name="id">value</param>
		/// <returns>bool</returns>
        bool ValueExists(string tableName, long id);

		/// <summary>
		/// Determines whether a value is being used or not
		/// </summary>
		/// <param name="tableName"></param>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <param name="table"></param>
		/// <param name="property"></param>
		/// <returns>bool</returns>
        bool ValueInUse(string tableName, string propertyName, long value, out string table, out string property);
    }
}
