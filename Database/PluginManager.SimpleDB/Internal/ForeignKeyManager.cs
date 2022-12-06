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
 *  File: ForeignKeyManager.cs
 *
 *  Purpose:  Manages foreign keys
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Shared.Classes;

#pragma warning disable CA2208

namespace SimpleDB.Internal
{
    internal class ForeignKeyManager : IForeignKeyManager
    {
        private readonly object _lock = new object();
        private readonly Dictionary<string, ISimpleDBTable> _foreignKeys = new Dictionary<string, ISimpleDBTable>();
        private readonly List<ForeignKeyRelationship> _foreignKeyRelationships = new List<ForeignKeyRelationship>();

        public void AddRelationShip(string sourceTable, string targetTable, string propertyName, string targetPropertyName)
        {
            if (String.IsNullOrEmpty(sourceTable))
                throw new ArgumentNullException(nameof(sourceTable));

            if (String.IsNullOrEmpty(targetTable))
                throw new ArgumentNullException(nameof(targetTable));

            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if (String.IsNullOrEmpty(targetPropertyName))
                throw new ArgumentNullException(nameof(targetPropertyName));

            _foreignKeyRelationships.Add(new ForeignKeyRelationship(sourceTable, targetTable, propertyName, targetPropertyName));
        }

        public void RegisterTable(ISimpleDBTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            using (TimedLock tl = TimedLock.Lock(_lock))
            {
                if (_foreignKeys.ContainsKey(table.TableName))
                    throw new ArgumentException("$Table is already registered", nameof(table.TableName));

                _foreignKeys[table.TableName] = table;
            }
        }

        public void UnregisterTable(ISimpleDBTable table)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            using (TimedLock tl = TimedLock.Lock(_lock))
            {
                if (!_foreignKeys.ContainsKey(table.TableName))
                    throw new ArgumentException("$Table is not registered", nameof(table.TableName));

                _foreignKeys.Remove(table.TableName);
            }
        }

        public bool ValueExists(string tableName, long id)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            if (!_foreignKeys.ContainsKey(tableName))
                throw new ForeignKeyException($"Foreign key table {tableName} does not exist");

            return _foreignKeys[tableName].IdExists(id);
        }

        public bool ValueInUse(string tableName, string propertyName, long value, out string table, out string property)
        {
            foreach (ForeignKeyRelationship relationship in _foreignKeyRelationships)
            {
                if (relationship.TargetTable.Equals(tableName) && _foreignKeys.ContainsKey(relationship.Table))
                {
                    if (_foreignKeys[relationship.Table].IdIsInUse(relationship.PropertyName, value))
                    {
                        table = relationship.Table;
                        property = relationship.PropertyName;
                        return true;
                    }
                }
            }

            table = null;
            property = null;

            return false;
        }
    }
}

#pragma warning restore CA2208