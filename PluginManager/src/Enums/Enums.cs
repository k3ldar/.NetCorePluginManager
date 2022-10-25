/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: Enums.cs
 *
 *  Purpose:  Shared Enum Values
 *
 *  Date        Name                Reason
 *  19/10/2018  Simon Carter        Initially Created
 *  04/11/2018  Simon Carter        Add Sieradelta GeoIp options to geoip 
 *  21/11/2018  Simon Carter        Add Login Enums
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager
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
        /// Indicates that an error occurred within CacheControl.Plugin module.
        /// </summary>
        CacheControlError = 10,

        /// <summary>
        /// The event was raised by the ThreadManager
        /// </summary>
        ThreadManager = 11,

        /// <summary>
        /// Indicates that an error occurred when translating a string using Localization.Plugin.
        /// 
        /// This is usually an indication that a localized string is missing.
        /// </summary>
        Localization = 12,
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
}
