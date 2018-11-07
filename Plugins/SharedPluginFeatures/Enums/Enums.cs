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
 *  Product:  SharedPluginFeatures
 *  
 *  File: Enums.cs
 *
 *  Purpose:  Shared Enum Values
 *
 *  Date        Name                Reason
 *  19/10/2018  Simon Carter        Initially Created
 *  04/11/2018  Simon Carter        Add Sieradelta GeoIp options to geoip enum
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    public class Enums
    {
        public enum LogLevel
        {
            Information = 1,

            Warning = 2,

            Error = 3,

            Critical = 4,

            PluginLoadSuccess = 5,

            PluginLoadFailed = 6,

            PluginLoadError = 7,

            PluginConfigureError = 8,

            IpRestricted = 9,

            IpRestrictedError = 10,

            UserSessionManagerError = 11,

            SpiderError = 12,

            SpiderRouteError = 13,

            CacheControlError = 14,

            GeoIpStackError = 15,

            ThreadManager = 16,
        }

        public enum GeoIpProvider
        {
            /// <summary>
            /// No Geo Ip Provider
            /// </summary>
            None = 0,

            /// <summary>
            /// IpStack https://ipstack.com/
            /// </summary>
            IpStack = 1,

            /// <summary>
            /// MySql Database
            /// </summary>
            MySql = 2,

            /// <summary>
            /// MS Sql Server Database
            /// </summary>
            MSSql = 3,

            /// <summary>
            /// Firebird database
            /// </summary>
            Firebird = 4,
        }

        public enum SystemAdminMenuType
        {
            FirstChild = 0,

            Text = 1,

            Grid = 2,

            PartialView = 3,

            Map = 4,

            FormattedText = 5,
        }

    }
}
