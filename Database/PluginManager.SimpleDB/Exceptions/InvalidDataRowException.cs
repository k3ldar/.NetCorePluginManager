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
 *  File: InvalidDataRowException.cs
 *
 *  Purpose:  InvalidDataRowException for validation issues when inserting/updating data rows
 *
 *  Date        Name                Reason
 *  28/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Invalid data row exception which is raised when data is invalid
	/// </summary>
	[Serializable]
	public sealed class InvalidDataRowException : Exception
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="dataRow">Data row name</param>
		/// <param name="property">Property/column which violates the rule</param>
		/// <param name="message">Exception message</param>
		/// <exception cref="ArgumentNullException"></exception>
		public InvalidDataRowException(string dataRow, string property, string message)
			: base($"{message}; Table: {dataRow}; Property {property}")
		{
			if (String.IsNullOrEmpty(dataRow))
				throw new ArgumentNullException(nameof(dataRow));

			if (String.IsNullOrEmpty(property))
				throw new ArgumentNullException(nameof(property));

			if (String.IsNullOrEmpty(message))
				throw new ArgumentNullException(nameof(message));

			DataRow = dataRow;
			Property = property;
			OriginalMessage = message;
		}

		/// <summary>
		/// Data row 
		/// </summary>
		public string DataRow { get; }

		/// <summary>
		/// Property/column name
		/// </summary>
		public string Property { get; }

		/// <summary>
		/// Original error message
		/// </summary>
		public string OriginalMessage { get; }
	}
}
