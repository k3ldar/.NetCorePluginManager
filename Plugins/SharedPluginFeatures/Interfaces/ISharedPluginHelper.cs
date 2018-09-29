using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace SharedPluginFeatures
{
    public interface ISharedPluginHelper
    {
        List<MainMenuItem> BuildMainMenu();
    }
}
