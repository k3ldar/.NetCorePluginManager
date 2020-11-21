﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  SystemAdmin.Plugin
 *  
 *  File: SystemAdminSettings.cs
 *
 *  Purpose:  SystemAdmin Settings
 *
 *  Date        Name                Reason
 *  28/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace SystemAdmin.Plugin
{
    /// <summary>
    /// Contains settings and configuration data for displaying items within the SystemAdmin.Plugin module.
    /// </summary>
    public class SystemAdminSettings
    {
        /// <summary>
        /// Google maps api key, should map data be viewed.
        /// 
        /// Must be between 15 and 80 characters long.
        /// </summary>
        /// <value>string</value>
        [SettingString(true, 15, 80)]
        public string GoogleMapApiKey { get; set; }

        /// <summary>
        /// Determines whether appsettings.json file can be viewed or not
        /// 
        /// Default value:  false.
        /// </summary>
        /// <value>bool.  If true the appsettings.json file can be viewed, this could potentially be a security vulnerability depending on what data is stored in there.</value>
        [SettingDefault(false)]
        public bool ShowAppSettingsJson { get; set; }

        /// <summary>
        /// Prevents formatted text being displayed.
        /// </summary>
        /// <value>bool.  If true the formatted text is disabled.</value>
        [SettingDefault(true)]
        public bool DisableFormattedText { get; set; }
    }
}
