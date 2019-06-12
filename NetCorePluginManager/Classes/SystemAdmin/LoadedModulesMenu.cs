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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
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
using System.Linq;
using System.Reflection;
using System.IO;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace AspNetCore.PluginManager.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of all assemblies currently loaded by the application and can 
    /// be viewed within SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public sealed class LoadedModulesMenu : SystemAdminSubMenu
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
        /// Returns delimited data on all loaded assemblies and their version.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            Dictionary<string, IPluginModule> plugins = PluginManagerService.GetPluginManager().GetLoadedPlugins();

            string Result = "Module|FileVersion";
            Dictionary<string, string> files = new Dictionary<string, string>();

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                string fileVersion = String.Empty;
                string file = String.Empty;
                try
                {
                    string path = String.IsNullOrEmpty(assembly.Location) ? assembly.CodeBase : assembly.Location;

                    if (path.StartsWith("file:///"))
                        path = path.Substring(8);

                    file = Path.GetFullPath(path);

                    if (File.Exists(file))
                        fileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(file).FileVersion;

                    file = Path.GetFileName(file);
                }
                catch (NotSupportedException)
                {
                    
                }


                if (!String.IsNullOrEmpty(file) && !files.ContainsKey(file))
                    files.Add(file, fileVersion);
            }

            foreach (KeyValuePair<string, string> valuePair in files.OrderBy(key => key.Key.ToLower()))
                Result += $"\r{valuePair.Key}|{valuePair.Value}";

            return Result;
        }

        public override string Name()
        {
            return "Loaded Modules";
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