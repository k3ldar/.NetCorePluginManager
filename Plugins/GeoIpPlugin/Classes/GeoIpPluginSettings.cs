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
 *  Product:  GeoIpPlugin
 *  
 *  File: GeoIpPluginSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SharedPluginFeatures;

namespace GeoIp.Plugin
{
    /// <summary>
    /// Settings to determine how GeoIp.Plugin module is configured.
    /// </summary>
    public class GeoIpPluginSettings
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public GeoIpPluginSettings()
        {
            IpStack = new IpStackSettings();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Webnet77CSVData.  The filename and path for Webnet77 Ip Address data held in CSV format.
        /// </summary>
        public string Webnet77CSVData { get; set; }

        /// <summary>
        /// GeoIpProvider used by GeoIp.Plugin module.
        /// </summary>
        public Enums.GeoIpProvider GeoIpProvider { get; set; }

        /// <summary>
        /// IpStackSettings, individual settings for retrieving data from Ip Stack.
        /// </summary>
        /// <value>IpStackSettings</value>
        public IpStackSettings IpStack { get; set; }

        #endregion Properties
    }
}
