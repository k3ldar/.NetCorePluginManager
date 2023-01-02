/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  RestrictIp.Plugin
 *  
 *  File: MemoryCacheMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  27/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Text;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace MemoryCache.Plugin.Classes
{
    /// <summary>
    /// Returns statistical data on the usage of available memory caches and can 
    /// be viewed within SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public class MemoryCacheMenu : SystemAdminSubMenu
    {
        public override string Action()
        {
            return String.Empty;
        }

        public override string Area()
        {
            return String.Empty;
        }

        public override string Controller()
        {
            return String.Empty;
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.Grid;
        }

        /// <summary>
        /// Returns data on the number of MemoryCaches in use, the number of items and age available in MemoryCachePlugin requests.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            StringBuilder Result = new StringBuilder("Name|Age|Item Count\r");

            for (int i = 0; i < CacheManager.GetCount(); i++)
            {
                Result.Append($"{CacheManager.GetCacheName(i)}|");
                Result.Append($"{CacheManager.GetCacheAge(i)}|");
                Result.Append($"{CacheManager.GetCacheCount(i)}\r");
            }

            return Result.ToString().Trim();
        }

        public override string Name()
        {
            return "Memory Cache";
        }

        public override string ParentMenuName()
        {
            return "System";
        }

        public override int SortOrder()
        {
            return 0;
        }

        public override string Image()
        {
            return String.Empty;
        }
    }
}

#pragma warning restore CS1591