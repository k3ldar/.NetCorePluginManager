using System;
using System.Collections.Generic;

namespace PluginManager.DAL.TextFiles.Tests
{
    internal class MockForeignKeyManager : IForeignKeyManager
    {
        public List<string> RegisteredTables = new List<string>();

        public void AddRelationShip(string table, string targetTable, string propertyName, string targetPropertyName)
        {
            throw new NotImplementedException();
        }

        public bool ValueExists(string tableName, long id)
        {
            throw new NotImplementedException();
        }

        public void RegisterTable(ITextTable table)
        {
            RegisteredTables.Add(table.TableName);
        }

        public void UnregisterTable(ITextTable table)
        {
            RegisteredTables.Remove(table.TableName);
        }

        public bool ValueInUse(string tableName, string propertyName, long value, out string table, out string property)
        {
            throw new NotImplementedException();
        }
    }
}
