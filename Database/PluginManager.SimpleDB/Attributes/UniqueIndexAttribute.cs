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
 *  File: UniqueIndexAttribute.cs
 *
 *  Purpose:  UniqueIndexAttribute for SimpleDB
 *
 *  Date        Name                Reason
 *  04/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Attribute indicating the value/property is unique within the table
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class UniqueIndexAttribute : Attribute
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Name of index</param>
		/// <param name="indexType">Type of index</param>
		/// <exception cref="ArgumentNullException"></exception>
		public UniqueIndexAttribute(string name, IndexType indexType = IndexType.Ascending)
			: this(indexType)
		{
			if (String.IsNullOrEmpty(name))
				throw new ArgumentNullException(nameof(name));

			Name = name;
		}

		/// <summary>
		/// Constructor uses property name as index name
		/// </summary>
		/// <param name="indexType">Type of index</param>
		public UniqueIndexAttribute(IndexType indexType = IndexType.Ascending)
		{
			IndexType = indexType;
		}

		/// <summary>
		/// Type of index
		/// </summary>
		/// <value>IndexType</value>
		public IndexType IndexType { get; }

		/// <summary>
		/// Name of index
		/// </summary>
		/// <value>string</value>
		public string Name { get; }
	}
}
