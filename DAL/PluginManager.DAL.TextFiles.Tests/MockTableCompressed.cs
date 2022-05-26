using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles.Tests
{
    [ExcludeFromCodeCoverage]
    [Table("MockTable", CompressionType.Brotli)]
    public class MockTableCompressed : BaseTable
    {
        public MockTableCompressed()
        {

        }

        public MockTableCompressed(int id)
            : this()
        {
            Id = id;
        }
    }
}
