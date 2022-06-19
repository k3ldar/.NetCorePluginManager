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
 *  File: TableUserTriggers.cs
 *
 *  Purpose:  Triggers for user table
 *
 *  Date        Name                Reason
 *  17/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PluginManager.DAL.TextFiles.Interfaces;
using PluginManager.DAL.TextFiles.Internal;

namespace PluginManager.DAL.TextFiles.Tables
{
    internal class TableUserTriggers : ITableTriggers<TableUser>
    {
        public string TableName => Constants.TableNameUsers;

        public int Position => 0;

        public void AfterDelete(List<TableUser> records)
        {

        }

        public void AfterInsert(List<TableUser> records)
        {

        }

        public void AfterUpdate(List<TableUser> records)
        {

        }

        public void BeforeDelete(List<TableUser> records)
        {

        }

        public void BeforeInsert(List<TableUser> records)
        {
            records.ForEach(r => r.PasswordExpire = DateTime.Now.AddYears(1));
        }

        public void BeforeUpdate(List<TableUser> records)
        {

        }
    }
}
