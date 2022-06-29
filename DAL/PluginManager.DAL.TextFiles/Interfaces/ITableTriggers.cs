using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles
{
    public interface ITableTriggers<T>
        where T : TableRowDefinition
    {
        int Position { get; }

        void BeforeInsert(List<T> records);

        void AfterInsert(List<T> records);

        void BeforeDelete(List<T> records);

        void AfterDelete(List<T> records);

        void BeforeUpdate(List<T> records);

        void AfterUpdate(List<T> records);
    }
}
