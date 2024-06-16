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
 *  File: ForeignKeyAttribute.cs
 *
 *  Purpose:  ForeignKeyAttribute for SimpleDB
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SimpleDB
{
	/// <summary>
	/// Provides foreign key functionality for SimpleDB
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ForeignKeyAttribute : Attribute
    {
		private ForeignKeyAttribute(string tableName, string propertyName, ForeignKeyAttributes foreignKeyAttributes)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            TableName = tableName;
            PropertyName = propertyName;
            Attributes = foreignKeyAttributes;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tableName">Name of table the foreign key is linked to</param>
        /// <exception cref="ArgumentNullException">Thrown if tableName is null or empty</exception>
        public ForeignKeyAttribute(string tableName)
            : this(tableName, "Id", ForeignKeyAttributes.None)
        {
        }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="tableName">Name of table the foreign key is linked to</param>
		/// <param name="foreignKeyAttributes">Foreign key attributes</param>
		public ForeignKeyAttribute(string tableName, ForeignKeyAttributes foreignKeyAttributes)
            : this(tableName, "Id", foreignKeyAttributes)
        {
        }

		/// <summary>
		/// Name of table the foreign key is linked to
		/// </summary>
		public string TableName { get; }

        /// <summary>
        /// Name of property on foreign key table
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Allows the foreign key value to be the default for the type of property the value is linked to
        /// </summary>
        public ForeignKeyAttributes Attributes { get; }
    }
}
