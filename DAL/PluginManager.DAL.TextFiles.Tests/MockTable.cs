using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles.Tests
{
    [ExcludeFromCodeCoverage]
    [Table("MockTable")]
    internal class MockTable : BaseTable
    {
        public MockTable()
        {

        }

        public MockTable(int id)
            : this()
        {
            Id = id;
        }
    }
}
