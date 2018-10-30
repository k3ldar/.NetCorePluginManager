using System;
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin.Classes.SystemAdmin
{
    public class UserSessionMenu : SystemAdminMainMenu
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
            return ("User Sessions");
        }

        public override int SortOrder()
        {
            return (10);
        }

        public override string Image()
        {
            return (String.Empty);
        }
    }
}
