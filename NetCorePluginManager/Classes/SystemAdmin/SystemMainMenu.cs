using System;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes
{
    public class SystemMainMenu : SystemAdminMainMenu
    {
        public override string Data()
        {
            return (String.Empty);
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.FirstChild);
        }

        public override string Name()
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
