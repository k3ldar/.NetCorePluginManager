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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SimpleDB
 *  
 *  File: ITextTable.cs
 *
 *  Purpose:  ISimpleDBTable interface for SimpleDB
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{


    /// <summary>
    /// add before/after insert/delete/update and add foreign key attributes and unique index attributes which can be validated
    /// </summary>
    public interface ISimpleDBTable
    {
		/// <summary>
		/// Name of the table
		/// </summary>
        string TableName { get; }

		/// <summary>
		/// Determines whether an index works or not
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        bool IdExists(long id);

		/// <summary>
		/// Determines whether an ID is in use on a specific property or not
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="value"></param>
		/// <returns></returns>
        bool IdIsInUse(string propertyName, long value);


		/// <summary>
		/// Instructs the class to clear all cached items
		/// </summary>
		void ClearAllMemory();

	}
}
