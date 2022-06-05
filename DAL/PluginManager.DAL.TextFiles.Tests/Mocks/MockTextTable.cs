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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: MockTextTable.cs
 *
 *  Purpose:  MockTextTable for text based storage
 *
 *  Date        Name                Reason
 *  02/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace PluginManager.DAL.TextFiles.Tests
{
    internal class MockTextTable : ITextTable
    {
        private readonly string _tableName;
        private readonly bool _idExists;

        public MockTextTable(string tableName, bool idExists)
        {
            _tableName = tableName;
            _idExists = idExists;
        }

        public string TableName => _tableName;

        public bool IdExists(long id)
        {
            return _idExists;
        }

        public bool IdIsInUse(string propertyName, long value)
        {
            throw new NotImplementedException();
        }
    }
}
