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
 *  Product:  SharedPluginFeatures
 *  
 *  File: Enums.cs
 *
 *  Purpose:  Shared Enum Values
 *
 *  Date        Name                Reason
 *  19/10/2018  Simon Carter        Initially Created
 *  04/11/2018  Simon Carter        Add Sieradelta GeoIp options to geoip 
 *  21/11/2018  Simon Carter        Add Login Enums
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Standard Enum values shared across all plugin modules.
    /// </summary>
    public static class Enums
    {
        /// <summary>
        /// Geo Ip provider types.
        /// 
        /// GeoIpPlugin module can use a variety of methods to implement Geo Ip lookup functionality via the IGeoIpProvider interface.
        /// </summary>
        public enum GeoIpProvider
        {
            /// <summary>
            /// No Geo Ip Provider
            /// </summary>
            None = 0,

            /// <summary>
            /// Geo Ip data provided by IpStack https://ipstack.com/
            /// </summary>
            IpStack = 1,

            /// <summary>
            /// Geo Ip data is provided by a MySql Database 
            /// </summary>
            MySql = 2,

            /// <summary>
            /// Geo Ip data is provided by a MS Sql Server Database
            /// </summary>
            MSSql = 3,

            /// <summary>
            /// Geo Ip data is provided by a Firebird database
            /// </summary>
            Firebird = 4,
        }

        /// <summary>
        /// System Admin menu types
        /// </summary>
        public enum SystemAdminMenuType
        {
            /// <summary>
            /// Not used at present time.
            /// </summary>
            FirstChild = 0,

            /// <summary>
            /// Data to be shown within SystemAdmin.Plugin is raw data.
            /// </summary>
            Text = 1,

            /// <summary>
            /// Data to be shown within SystemAdmin.Plugin is grid based data.
            /// </summary>
            Grid = 2,

            /// <summary>
            /// Data to be shown within SystemAdmin.Plugin comes from a partial view provided by the implementor.
            /// </summary>
            PartialView = 3,

            /// <summary>
            /// Data to be shown within SystemAdmin.Plugin is map related data.
            /// </summary>
            Map = 4,

            /// <summary>
            /// Data to be shown within SystemAdmin.Plugin is raw text formatted using html.
            /// </summary>
            FormattedText = 5,

            /// <summary>
            /// Data to be shown within the plugin comes from a view
            /// </summary>
            View = 6,
        }

        /// <summary>
        /// Validate Request Results
        /// </summary>
        [Flags]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1714:Flags enums should have plural names", Justification = "yeak ok, I hear you but not this time!")]
        public enum ValidateRequestResult
        {
            /// <summary>
            /// State unknown
            /// </summary>
            Undetermined = 1,

            /// <summary>
            /// Ip has too many requests
            /// </summary>
            TooManyRequests = 2,

            /// <summary>
            /// Enough keywords to suggest may be a SQL injection attack
            /// </summary>
            PossibleSQLInjectionAttack = 4,

            /// <summary>
            /// Enough keywords to determine this is a SQL injection attack
            /// </summary>
            SQLInjectionAttack = 8,

            /// <summary>
            /// Determines that the request is probably generated from a spider or bot
            /// </summary>
            PossibleSpiderBot = 16,

            /// <summary>
            /// Determines that the request is generated from a spider or bot
            /// </summary>
            SpiderBot = 32,

            /// <summary>
            /// Enough keywords to suggest this maybe a hack attempt
            /// </summary>
            PossibleHackAttempt = 64,

            /// <summary>
            /// Enough keywords to determine this is a hack attempt
            /// </summary>
            HackAttempt = 128,

            /// <summary>
            /// IP Address is white listed
            /// </summary>
            IpWhiteListed = 256,

            /// <summary>
            /// IP Address is black listed
            /// </summary>
            IpBlackListed = 512,

            /// <summary>
            /// IP address is a search engine
            /// </summary>
            SearchEngine = 1024,

            /// <summary>
            /// A Ban has been requested on the IP Address
            /// </summary>
            BanRequested = 2048,
        }
    }

    /// <summary>
    /// The frequency of which a sitemap item is updated.
    /// </summary>
    public enum SitemapChangeFrequency
    {
        /// <summary>
        /// The item is continually updated
        /// </summary>
        Always,

        /// <summary>
        /// The item is updated on an hourly basis
        /// </summary>
        Hourly,

        /// <summary>
        /// The item is updated daily
        /// </summary>
        Daily,

        /// <summary>
        /// The item is updated on a weekly basis
        /// </summary>
        Weekly,

        /// <summary>
        /// The item is updated on a monthly basis
        /// </summary>
        Monthly,

        /// <summary>
        /// The item is updated very rarely
        /// </summary>
        Yearly,

        /// <summary>
        /// The item is archived and will never be updated again
        /// </summary>
        Never,
    }
}
