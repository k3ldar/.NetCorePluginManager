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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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
using System.Collections.Generic;

using AppSettings;

namespace AspNetCore.PluginManager
{
    public sealed class PluginSettings
    {
        public bool Disabled { get; set; }

        [SettingString(true)]
        [SettingValidPath]
        public string PluginPath { get; set; }

        [SettingString(true)]
        [SettingValidPath]
        public string PluginSearchPath { get; set; }

        [SettingString(true)]
        [SettingValidPath]
        public string SystemFiles { get; set; }

        [SettingString(true)]
        [SettingValidPath]
        public string CSSLocation { get; set; }

        [SettingString(true)]
        [SettingValidPath]
        public string JScriptLocation { get; set; }

        public bool PreventAreas { get; set; }

        public bool DisableRouteDataService { get; set; }

        public List<string> PluginFiles { get; set; }

        public List<PluginSetting> Plugins { get; set; }
    }
}
