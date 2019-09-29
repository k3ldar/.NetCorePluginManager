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
 *  File: PluginSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using AppSettings;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// Settings that affect how PluginManager works.
    /// </summary>
    public sealed class PluginSettings
    {
        /// <summary>
        /// Opionally disable plugin manager and prevent it from loading any plugins.
        /// </summary>
        /// <value>bool</value>
        public bool Disabled { get; set; }

        /// <summary>
        /// Path where plugin assembly modules are located.
        /// </summary>
        /// <value>string</value>
        [SettingString(true)]
        [SettingDefault("%AppPath%\\Plugins")]
        public string PluginPath { get; set; }

        /// <summary>
        /// Root path that is used to search for plugin assembly modules.
        /// </summary>
        /// <value>string</value>
        [SettingString(true)]
        [SettingValidPath]
        [SettingDefault("%AppPath%\\Bin")]
        public string PluginSearchPath { get; set; }

        /// <summary>
        /// Root path that will be searched for dll's that can not be found when loading plugin modules.
        /// </summary>
        /// <value>string</value>
        [SettingString(true)]
        public string SystemFiles { get; set; }

        /// <summary>
        /// Path where .css files will be placed when being extracted from plugin modules.
        /// </summary>
        /// <value>string</value>
        [SettingString(true)]
        [SettingValidPath]
        [SettingDefault("%AppPath%\\wwwroot\\css")]
        public string CSSLocation { get; set; }

        /// <summary>
        /// Path where .js files will be placed when being extracted from plugin modules.
        /// </summary>
        /// <value>string</value>
        [SettingString(true)]
        [SettingValidPath]
        [SettingDefault("%AppPath%\\wwwroot\\js")]
        public string JScriptLocation { get; set; }

        /// <summary>
        /// Prevents PluginManager from creating an IRouteDataService instance that can be obtained from IoC
        /// </summary>
        /// <value>bool</value>
        public bool DisableRouteDataService { get; set; }

        /// <summary>
        /// Indicates whether a copy of the plugin will be sent to <seealso cref="LocalCopyPath"/>, where it will be loaded from.
        /// </summary>
        /// <value>bool</value>
        [SettingDefault(false)]
        public bool CreateLocalCopy { get; set; }

        /// <summary>
        /// If CreateLocalCopy is true, this path will be used to store and load the plugins from.
        /// </summary>
        /// <value>string</value>
        [SettingOptional]
        [SettingValidPath]
        public string LocalCopyPath { get; set; }

        /// <summary>
        /// User defined list of plugin modules that will be loaded in order prior to generic loading of plugins.
        /// 
        /// If you need to specify the load order of plugins the assembly names (with or without path) need to be included in this list.
        /// </summary>
        /// <value>List&lt;string&gt;</value>
        public List<string> PluginFiles { get; set; }

        /// <summary>
        /// Individual plugin module settings.
        /// </summary>
        /// <value>List&lt;PluginSetting&gt;</value>
        public List<PluginSetting> Plugins { get; set; }
    }
}
