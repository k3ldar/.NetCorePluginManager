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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: UserSessionSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

using Microsoft.AspNetCore.Hosting;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin
{
    /// <summary>
    /// Contains settings that are required by the UserSessionMiddleware.Plugin module.
    /// </summary>
    public sealed class UserSessionSettings
    {
        /// <summary>
        /// Contains a delimited list of static file extensions
        /// </summary>
        /// <value>string.  SettingDefault(Constants.StaticFileExtensions)</value>
        [SettingDefault(Constants.StaticFileExtensions)]
        public string StaticFileExtensions { get; set; }

        /// <summary>
        /// Name of cookie used to store user session data.
        /// </summary>
        /// <value>SettingDefault(Constants.UserSession)</value>
        [SettingDefault(Constants.UserSession)]
        public string CookieName { get; set; }

        /// <summary>
        /// Encryption key used for encrypting user session data that is stored within a cookie.
        /// </summary>
        /// <value>string</value>
        [SettingDefault("Dfklaosre;lnfsdl;jlfaeu;dkkfcaskxcd3jf")]
        [SettingString(false, Constants.MinimumKeyLength, Constants.MaximumKeyLength)]
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Number of minutes the sessions is valid for.
        /// 
        /// Default: 30 minutes.
        /// Minimuum: 15 minutes.
        /// Maximum: 200 minutes.
        /// </summary>
        /// <value>uint</value>
        [SettingDefault(30u)]
        [SettingRange(15u, 200u)]
        public uint SessionTimeout { get; set; }

        /// <summary>
        /// Default culture used for the user session.
        /// </summary>
        /// <value>string</value>
        [SettingDefault("en-GB")]
        [SettingString(false, 2u, 5u)]
        public string DefaultCulture { get; set; }

        /// <summary>
        /// Determines whether the default session service is enabled or not, default is the service is disabled.
        /// </summary>
        /// <value>bool</value>
        public bool EnableDefaultSessionService { get; set; }

        /// <summary>
        /// Maximum number of hourly data to keep
        /// 
        /// This value can range from 24 (6 hours) up to 2880 (30 days).  The default is 96 (24 hours)
        /// </summary>
        /// <value>uint</value>
        [SettingDefault(96u)]
        [SettingRange(24u, 2880u)]
        public uint MaxHourlyData { get; set; }

        /// <summary>
        /// Maximum number of daily session data to keep
        /// 
        /// This value can range from 30 to 730
        /// </summary>
        /// <value>uint</value>
        [SettingDefault(360u)]
        [SettingRange(30u, 730u)]
        public uint MaxDailyData { get; set; }

        /// <summary>
        /// Maximum number of weekly session data to keep
        /// 
        /// This value can range from 26 to 520
        /// </summary>
        /// <value>uint</value>
        [SettingDefault(520u)]
        [SettingRange(26u, 520u)]
        public uint MaxWeeklyData { get; set; }

        /// <summary>
        /// Maximum number of Monthly session data to keep
        /// 
        /// This value can range from 12 to 120
        /// </summary>
        /// <value>uint</value>
        [SettingDefault(120u)]
        [SettingRange(12u, 120u)]
        public uint MaxMonthlyData { get; set; }

        /// <summary>
        /// Maximum number of Yearly session data to keep
        /// 
        /// This value can range from 1 to 50
        /// </summary>
        /// <value>uint</value>
        [SettingDefault(20u)]
        [SettingRange(10u, 50u)]
        public uint MaxYearlyData { get; set; }

        /// <summary>
        /// Rootpath for session data, if left empty then the default value will be <see cref="IHostingEnvironment"/>.ContentRootPath
        /// </summary>
        /// <value>string</value>
        [SettingOptional]
        public string SessionRootPath { get; set; }
    }
}
