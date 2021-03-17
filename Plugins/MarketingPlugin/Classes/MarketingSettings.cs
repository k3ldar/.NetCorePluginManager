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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  MarketingPlugin
 *  
 *  File: MarketingSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using SharedPluginFeatures;

 namespace MarketingPlugin
{
    /// <summary>
    /// Settings which affect the marketing plugin
    /// </summary>
    public class MarketingSettings
    {
        #region Properties

        /// <summary>
        /// Indicates whether static files are processed or not when managing marketing routes
        /// </summary>
        /// <value>bool</value>
        public bool ProcessStaticFiles { get; set; }

        /// <summary>
        /// Semicolon seperated list of static file extensions to be ignored when processing requests for marketing
        /// </summary>
        [SettingDefault(Constants.StaticFileExtensions)]
        [SettingString(false)]
        [SettingDelimitedString(';', 1)]
        public string StaticFileExtensions { get; set; }

        #endregion Properties
    }
}
