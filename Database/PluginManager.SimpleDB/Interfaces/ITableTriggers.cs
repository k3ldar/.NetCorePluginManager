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
 *  File: ITableTriggers.cs
 *
 *  Purpose:  Triggers interface for user table
 *
 *  Date        Name                Reason
 *  17/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


namespace SimpleDB
{
	/// <summary>
	/// Definition for class containing triggers for a table
	/// </summary>
	/// <typeparam name="T">Class table to which the triggers belong</typeparam>
    public interface ITableTriggers<T>
        where T : TableRowDefinition
    {
		/// <summary>
		/// Position of triggers when being called
		/// </summary>
		/// <value>int</value>
        int Position { get; }

		/// <summary>
		/// Supported trigger types
		/// </summary>
		/// <vvalue>TriggerType</vvalue>
        TriggerType TriggerTypes { get; }

		/// <summary>
		/// Method fired for all rows, before inserting
		/// </summary>
		/// <param name="records"></param>
        void BeforeInsert(List<T> records);

		/// <summary>
		/// Method fired for all rows, after inserting
		/// </summary>
		/// <param name="records"></param>
		void AfterInsert(List<T> records);

		/// <summary>
		/// Method fired for all rows, before deleting
		/// </summary>
		/// <param name="records"></param>
		void BeforeDelete(List<T> records);

		/// <summary>
		/// Method fired for all rows after deleting
		/// </summary>
		/// <param name="records"></param>
		void AfterDelete(List<T> records);

		/// <summary>
		/// Method fired for all rows affected, before updating
		/// </summary>
		/// <param name="records"></param>
		void BeforeUpdate(List<T> records);

		/// <summary>
		/// Method fired for each row before updating, with the option of comparing the new row with the old row
		/// </summary>
		/// <param name="newRecord"></param>
		/// <param name="oldRecord"></param>
		void BeforeUpdate(T newRecord, T oldRecord);

		/// <summary>
		/// Method fired for all rows affected, after updating
		/// </summary>
		/// <param name="records"></param>
		void AfterUpdate(List<T> records);
    }
}
