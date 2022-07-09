using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager.DAL.TextFiles.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    [Table("MockTableAddress", cachingStrategy: CachingStrategy.Memory)]
    public class MockTableForeignKeyDefaultAllowed : TableRowDefinition
    {
        private long _userId;

        public MockTableForeignKeyDefaultAllowed()
        {

        }

        public MockTableForeignKeyDefaultAllowed(long userId)
        {
            UserId = userId;
            Description = $"Address {userId}";
        }

        [ForeignKey("MockTableUser", true)]
        public long UserId
        {
            get => _userId;

            set
            {
                _userId = value;
                Update();
            }
        }

        public string Description { get; set; }
    }
}
