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
    public class Enums
    {
        /// <summary>
        /// Log Levels, defines the type of log entry being made by ILogger interface.
        /// </summary>
        public enum LogLevel
        {
            /// <summary>
            /// Log entry is information only.
            /// </summary>
            Information = 1,

            /// <summary>
            /// Log entry is a warning condition.
            /// </summary>
            Warning = 2,

            /// <summary>
            /// Log entry represents an error that has occurred.
            /// </summary>
            Error = 3,

            /// <summary>
            /// Log entry is a critical error within the system.
            /// </summary>
            Critical = 4,

            /// <summary>
            /// Log entry is informing that a plugin module has been successfully loaded.
            /// </summary>
            PluginLoadSuccess = 5,

            /// <summary>
            /// Log entry is informing that a plugin module has failed to load.
            /// </summary>
            PluginLoadFailed = 6,

            /// <summary>
            /// Log entry is informing that a generic error occurred when loading a plugin module.
            /// </summary>
            PluginLoadError = 7,

            /// <summary>
            /// Log entry is informing that there is a configuration error with a plugin module.
            /// </summary>
            PluginConfigureError = 8,

            /// <summary>
            /// Log entry informing that an Ip address has had restriction imposed upon it within the RestrictIp.Plugin module.
            /// </summary>
            IpRestricted = 9,

            /// <summary>
            /// Log entry informing that an error occurred when imposing a restriction upon an Ip address within the RestrictIp.Plugin module.
            /// </summary>
            IpRestrictedError = 10,

            /// <summary>
            /// Indicates that an error occurred within the UserSessionMiddleware.Plugin module.
            /// </summary>
            UserSessionManagerError = 11,

            /// <summary>
            /// Indicates that an error occurred within Spider.Plugin module.
            /// </summary>
            SpiderError = 12,

            /// <summary>
            /// Indicates that an there is an error condition for a route within Spider.Plugin module.
            /// </summary>
            SpiderRouteError = 13,

            /// <summary>
            /// Indicates that an error occurred within CacheControl.Plugin module.
            /// </summary>
            CacheControlError = 14,

            /// <summary>
            /// Indicates that an error took place within the GeoIpPlugin module whilst implementing GeoIpProvider.IpStack.
            /// </summary>
            GeoIpStackError = 15,

            /// <summary>
            /// The event was raised by the ThreadManager
            /// </summary>
            ThreadManager = 16,

            /// <summary>
            /// Indicates that an error occurred when translating a string using Localization.Plugin.
            /// 
            /// This is usually an indication that a localized string is missing.
            /// </summary>
            Localization = 17,

            /// <summary>
            /// Indicates that an error took place when creating the breadcrumb for a route within the Breadcrumb.Plugin module.
            /// </summary>
            BreadcrumbError = 18,

            /// <summary>
            /// Indicates that an error took place within SeoPlugin module.
            /// </summary>
            SeoError = 19,
        }

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
        }

        /// <summary>
        /// Validate Request Results
        /// </summary>
        [Flags]
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
    /// Result enum for dynamically adding plugin modules using IPluginHelperService interface.
    /// </summary>
    public enum DynamicLoadResult
    {
        /// <summary>
        /// Plugin module was dynamically added to the list of available plugins.
        /// </summary>
        Success,

        /// <summary>
        /// Failed to load the module as an available plugin.
        /// </summary>
        Failed,

        /// <summary>
        /// The module was already loaded as a plugin module within the PluginManager.
        /// </summary>
        AlreadyLoaded
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
