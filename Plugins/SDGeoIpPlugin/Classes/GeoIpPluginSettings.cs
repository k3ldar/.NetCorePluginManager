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
 *  Product:  SieraDeltaGeoIpPlugin
 *  
 *  File: GeoIpPluginSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  04/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using SharedPluginFeatures;

namespace SieraDeltaGeoIp.Plugin
{
    /// <summary>
    /// Contains GeoIp settings that are used to connect to MySql, MSSql or Firebird.
    /// </summary>
    public class GeoIpPluginSettings
    {
        #region Properties

        /// <summary>
        /// Determines whether all Geo Ip data is cached in memory or not.
        /// </summary>
        public bool CacheAllData { get; set; }

        /// <summary>
        /// Database connection string, this can also point to a file that contains the connection string.
        /// </summary>
        [SettingString(false)]
        public string DatabaseConnectionString { get; set; }

        /// <summary>
        /// Type of provider to be used.
        /// </summary>
        public Enums.GeoIpProvider GeoIpProvider { get; set; }

        /// <summary>
        /// Array of country data that will be loaded in the background whilst the middleware is initialised.  This allows
        /// for a faster response for specific countries that the website serves.  For instance, if your primary customer
        /// base is from the USA you could load all Geo Ip address data for that country so it is cached.
        /// </summary>
        /// <value>string[]</value>
        public string[] CountryList { get; set; }

        #endregion Properties
    }
}
