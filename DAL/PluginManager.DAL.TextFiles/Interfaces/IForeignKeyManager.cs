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

namespace PluginManager.DAL.TextFiles
{
    public interface IForeignKeyManager
    {
        void AddRelationShip(string sourceTable, string targetTable, string propertyName, string targetPropertyName);

        void RegisterTable(ITextTable table);

        void UnregisterTable(ITextTable table);

        bool ValueExists(string tableName, long id);

        bool ValueInUse(string tableName, string propertyName, long value, out string table, out string property);
    }
}
