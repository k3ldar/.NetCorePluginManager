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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
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
using AppSettings;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// Settings that affect how PluginManager works.
    /// </summary>
    public sealed class NetCorePluginSettings : global::PluginManager.PluginSettings
    {
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
    }
}
