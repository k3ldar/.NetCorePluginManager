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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: LoadedModulesMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  31/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace AspNetCore.PluginManager.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of all plugin modules that are loaded and can be viewed within 
    /// SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public class LoadedPluginsMenu : SystemAdminSubMenu
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
        /// Returns delimited data on all plugin modules that have been loaded by AspNetCore.PluginManager.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            Dictionary<string, IPluginModule> plugins = PluginManagerService.GetPluginManager().PluginsGetLoaded();

            StringBuilder Result = new("Module|Plugin Version|File Version");

            foreach (KeyValuePair<string, IPluginModule> keyValuePair in plugins)
            {
                Result.AppendFormat("\r{0}|{1}|{2}", keyValuePair.Value.Module, keyValuePair.Value.Version.ToString(), keyValuePair.Value.FileVersion);
            }

            return Result.ToString();
        }

        public override string Name()
        {
            return "Loaded Plugins";
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