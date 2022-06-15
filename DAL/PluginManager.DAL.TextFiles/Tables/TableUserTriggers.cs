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
