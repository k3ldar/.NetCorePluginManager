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
 *  Product:  WebSmokeTest.Plugin
 *  
 *  File: WebSmokeTestSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/06/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using AppSettings;

using SharedPluginFeatures;

namespace WebSmokeTest.Plugin
{
    /// <summary>
    /// Settings which affect how WebSmokeTest data is served.
    /// </summary>
    public class WebSmokeTestSettings
    {
        #region Properties

        /// <summary>
        /// Delimited list of file extensions to ignore
        /// </summary>
        /// <value>string</value>
        [SettingDefault(Constants.StaticFileExtensions)]
        [SettingString(false)]
        [SettingDelimitedString(';', 1)]
        public string StaticFileExtensions { get; set; }

        /// <summary>
        /// Determines whether smoke testing is available or not, should be set to false 
        /// when used on a live site
        /// </summary>
        [SettingDefault(false)]
        public bool Enabled { get; set; }

        /// <summary>
        /// A list of unique site id's for Smoke Testing
        /// </summary>
        /// <value>List&lt;string&gt;</value>
        [SettingOptional]
        public List<string> SiteId { get; set; }

        /// <summary>
        /// The key used to encrypt smoke test data prior to sending
        /// </summary>
        [SettingString(15, 250)]
        public string EncryptionKey { get; set; }

        #endregion Properties
    }
}
