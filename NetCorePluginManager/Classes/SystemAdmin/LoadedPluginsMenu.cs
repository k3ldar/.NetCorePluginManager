using System;
using System.Collections.Generic;
using System.Text;

using AspNetCore.PluginManager;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes.SystemAdmin
{
    public class LoadedPluginsMenu : SystemAdminSubMenu
    {
        public override string Action()
        {
            return (String.Empty);
        }

        public override string Area()
        {
            return (String.Empty);
        }

        public override string Controller()
        {
            return (String.Empty);
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
        }

        public override string Data()
        {
            Dictionary<string, IPluginModule> plugins = PluginManagerService.GetPluginManager().GetLoadedPlugins();

            string Result = "Module|Plugin Version|FileVersion";

            foreach (KeyValuePair<string, IPluginModule> keyValuePair in plugins)
            {
                Result += $"\r{keyValuePair.Value.Module}|{keyValuePair.Value.Version.ToString()}|" +
                    $"{keyValuePair.Value.FileVersion}";
            }

            return (Result);
        }

        public override string Name()
        {
            return ("Loaded Plugins");
        }

        public override string ParentMenuName()
        {
            return ("System");
        }

        public override int SortOrder()
        {
            return (0);
        }

        public override string Image()
        {
            return (String.Empty);
        }
    }
}
