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
 *  File: InsertOptions.cs
 *
 *  Purpose:  Insert options for SimpleDB
 *
 *  Date        Name                Reason
 *  06/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Insert options class containing options to be used when inserting records
	/// </summary>
	public sealed class InsertOptions
	{
		/// <summary>
		/// Constructor, default to assigning primary key
		/// </summary>
		public InsertOptions()
			: this(true)
		{

		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="assignPrimaryKey">Assignes the primary key</param>
		public InsertOptions(bool assignPrimaryKey)
		{
			AssignPrimaryKey = assignPrimaryKey;
		}

		/// <summary>
		/// Indicates whether the primary key is assigned or not
		/// </summary>
		public bool AssignPrimaryKey { get; }
	}
}
