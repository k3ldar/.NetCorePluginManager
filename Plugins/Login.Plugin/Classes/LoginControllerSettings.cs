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
 *  Product:  Login Plugin
 *  
 *  File: LoginControllerSettings.cs
 *
 *  Purpose:  Login Controller Settings
 *
 *  Date        Name                Reason
 *  28/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace LoginPlugin
{
    /// <summary>
    /// Settings that determine how the Login.Plugin module is configured and used.
    /// </summary>
    public sealed class LoginControllerSettings
    {
        /// <summary>
        /// Maximum number of attempts to login before the user is prevented from logging in for several minutes.
        /// </summary>
        /// <value>int</value>
        [SettingDefault(2)]
        public int CaptchaShowFailCount { get; set; }

        /// <summary>
        /// Length of the Captcha word.
        /// 
        /// Default: 7
        /// Minimum: 6
        /// Maximum: 12
        /// </summary>
        [SettingRange(6, 12)]
        [SettingDefault(7)]
        public int CaptchaWordLength { get; set; }

        /// <summary>
        /// The absolute or relative Uri where the user will be redirected to if the login was successful.
        /// </summary>
        /// <value>string</value>
        [SettingDefault("/")]
        public string LoginSuccessUrl { get; set; }

        /// <summary>
        /// Determines whether the ip address or unique Net Core session id is used to cache data.
        /// 
        /// This is mainly useful in a debug environment and should be set to true.
        /// </summary>
        /// <value>bool</value>
        [SettingDefault(false)]
        public bool CacheUseSession { get; set; }

        /// <summary>
        /// Determines whether the Remember Me option is shown or not.
        /// </summary>
        /// <value>bool</value>
        [SettingDefault(true)]
        public bool ShowRememberMe { get; set; }

        /// <summary>
        /// Remember me cookie name.
        /// 
        /// Must be between 6 and 20 characters long.
        /// 
        /// Default: RememberMe
        /// </summary>
        [SettingString(false, 6, 20)]
        [SettingDefault("RememberMe")]
        public string RememberMeCookieName { get; set; }

        /// <summary>
        /// Encryption key used to encrypt cookie values.
        /// 
        /// Must be between 20 and 60 characters long.
        /// </summary>
        /// <value>string</value>
        [SettingString(20, 60)]
        [SettingDefault("A^SSDFasdkl;fjanewrun[ca'ekd jf;z4sieurn;fdmmjf")]
        public string EncryptionKey { get; set; }

        /// <summary>
        /// Number of days the user can remain logged in, this is accomplished using cookies.
        /// 
        /// Default: 30
        /// Minimum: 1
        /// Maximum: 360
        /// </summary>
        /// <value>int</value>
        [SettingRange(1, 360)]
        [SettingDefault(30)]
        public int LoginDays { get; set; }

        /// <summary>
        /// Url that the user can be redirected to, in order to change their password.
        /// 
        /// This must be either a relative or absolute Uri.
        /// </summary>
        /// <value>string</value>
        [SettingUri(false, System.UriKind.RelativeOrAbsolute)]
        [SettingDefault("/Account/ChangePassword")]
        public string ChangePasswordUrl { get; set; }

        /// <summary>
        /// The name of the authentication scheme
        /// </summary>
        /// <value>string</value>
        [SettingDefault("DefaultAuthSchemeName")]
        [SettingString(false)]
        public string AuthenticationScheme { get; set; }
    }
}
