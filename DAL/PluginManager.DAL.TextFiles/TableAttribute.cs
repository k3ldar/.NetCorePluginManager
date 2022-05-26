using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        public TableAttribute(string tableName, CompressionType compression = CompressionType.None)
        {
            if (String.IsNullOrEmpty(tableName))
                throw new ArgumentNullException(nameof(tableName));

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                if (tableName.Contains(c))
                    throw new ArgumentException($"Tablename contains invalid character: {c}", nameof(tableName));
            }

            TableName = tableName;
            Compression = compression;
        }

        public string TableName { get; }

        public CompressionType Compression { get; }
    }
}
