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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ForeignKeyAttribute.cs
 *
 *  Purpose:  ForeignKeyAttribute for text based storage
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.DAL.TextFiles
{
    /// <summary>
    /// Provides foreign key functionality for text based storage
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class ForeignKeyAttribute : Attribute
    {
        private ForeignKeyAttribute(string tableName, string propertyName, bool allowDefault)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            TableName = tableName;
            PropertyName = propertyName;
            AllowDefaultValue = allowDefault;
        }
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="tableName">Name of table the foreign key is linked to</param>
        /// <exception cref="ArgumentNullException">Thrown if tableName is null or empty</exception>
        public ForeignKeyAttribute(string tableName)
            : this(tableName, "Id", false)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tableName">Name of table the foreign key is linked to</param>
        /// <param name="allowDefault">Allows default value of type if foreign key does not exist</param>
        public ForeignKeyAttribute(string tableName, bool allowDefault)
            : this(tableName, "Id", allowDefault)
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
        public bool AllowDefaultValue { get; }
    }
}
