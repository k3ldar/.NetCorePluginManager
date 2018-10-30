using System;
using System.Collections.Generic;
using System.Text;

using Shared.Classes;
using SharedPluginFeatures;

namespace MemoryCache.Plugin.Classes
{
    public class MemoryCacheMenu : SystemAdminSubMenu
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
            StringBuilder Result = new StringBuilder("Name|Age|Item Count\r");

            for (int i = 0; i < CacheManager.GetCount(); i++)
            {
                Result.Append($"{CacheManager.GetCacheName(i)}|");
                Result.Append($"{CacheManager.GetCacheAge(i)}|");
                Result.Append($"{CacheManager.GetCacheCount(i)}\r");
            }

            return (Result.ToString().Trim());
        }

        public override string Name()
        {
            return ("Memory Cache");
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
