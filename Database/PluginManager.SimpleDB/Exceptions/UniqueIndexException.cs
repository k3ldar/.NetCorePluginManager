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
 *  File: UniqueIndexException.cs
 *
 *  Purpose:  Unique index exception for SimpleDB
 *
 *  Date        Name                Reason
 *  09/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Runtime.Serialization;

namespace SimpleDB
{
	/// <summary>
	/// Exception raised when a record with a unique index is inserted twice
	/// </summary>
	[Serializable]
	public class UniqueIndexException : Exception
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public UniqueIndexException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">Message description</param>
		public UniqueIndexException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="message">Message description</param>
		/// <param name="innerException">Inner Exception</param>
		public UniqueIndexException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}