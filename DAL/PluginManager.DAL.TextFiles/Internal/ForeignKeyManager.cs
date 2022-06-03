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
 *  File: ForeignKeyManager.cs
 *
 *  Purpose:  Manages foreign keys
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Internal
{
    internal class ForeignKeyManager : IForeignKeyManager
    {
        private readonly object _lock = new object();
        private readonly Dictionary<string, string> _foreignKeys = new Dictionary<string, string>();
        private readonly Dictionary<string, ITextTable> _writer = new Dictionary<string, ITextTable>();

        public void AddRelationShip(string sourceTable, string targetTable)
        {
            throw new NotImplementedException();
        }

        public void RegisterTable(ITextTable table, string tableName)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            using (TimedLock tl = TimedLock.Lock(_lock))
            {
                if (_writer.ContainsKey(tableName))
                    throw new ArgumentException("$Table is already registered", nameof(tableName));

                _writer[tableName] = table;
            }
        }

        public void UnregisterTable(ITextTable table, string tableName)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            using (TimedLock tl = TimedLock.Lock(_lock))
            {
                if (!_writer.ContainsKey(tableName))
                    throw new ArgumentException("$Table is not registered", nameof(tableName));

                _writer.Remove(tableName);
            }
        }
    }
}
