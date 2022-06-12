using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles.Interfaces
{
    public interface ITableTriggers<T>
    {
        string TableName { get; }

        int Position { get; }

        bool BeforeInsert(List<T> records);

        void AfterInsert(List<T> records);

        bool BeforeDelete(List<T> records);

        void AfterDelete(List<T> records);

        bool BeforeUpdate(List<T> records);

        void AfterUpdate(List<T> records);
    }
}
