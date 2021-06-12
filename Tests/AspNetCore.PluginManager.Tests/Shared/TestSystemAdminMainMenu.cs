using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public sealed class TestSystemAdminMainMenu : SystemAdminMainMenu
    {
        public TestSystemAdminMainMenu(in string name, in int uniqueId) 
            : base(name, uniqueId)
        {
        }
    }
}
