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
 *  Product:  Search Plugin
 *  
 *  File: SearchControllerSettings.cs
 *
 *  Purpose:  Search Controller Settings
 *
 *  Date        Name                Reason
 *  01/02/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace SearchPlugin
{
    /// <summary>
    /// Settings that determine how the Search.Plugin module is configured and used.
    /// </summary>
    public sealed class SearchControllerSettings
    {
        /// <summary>
        /// Maximum number of attempts to search before the user is prevented from logging in for several minutes.
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

        /// <summary>
        /// Minimum length of keyword search string
        /// </summary>
        [SettingDefault((byte)3)]
        public byte MinimumKeywordSearchLength { get; set; }

        /// <summary>
        /// Indicates that the keyword search result should have the search keyword highlighted in html
        /// </summary>
        [SettingDefault(true)]
        public bool HighlightQuickSearchTerms { get; set; }

        /// <summary>
        /// Total number of search results per page
        /// </summary>
        [SettingDefault(3)]
        public int ItemsPerPage { get; set; }
    }
}
