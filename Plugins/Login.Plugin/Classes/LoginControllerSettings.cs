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
    public sealed class LoginControllerSettings
    {
        [SettingDefault(2)]
        public int CaptchaShowFailCount { get; set; }

        [SettingRange(6, 12)]
        [SettingDefault(6)]
        public int CaptchaWordLength { get; set; }

        [SettingDefault("/")]
        public string LoginSuccessUrl { get; set; }

        [SettingDefault(false)]
        public bool CacheUseSession { get; set; }

        [SettingDefault(true)]
        public bool ShowRememberMe { get; set; }

        [SettingString(false, 6, 20)]
        [SettingDefault("RememberMe")]
        public string RememberMeCookieName { get; set; }

        [SettingDefault("A^SSDFasdkl;fjanewrun[ca'ekd jf;z4sieurn;fdmmjf")]
        public string EncryptionKey { get; set; }

        [SettingRange(1, 360)]
        [SettingDefault(30)]
        public int LoginDays { get; set; }

        [SettingUri(false, System.UriKind.RelativeOrAbsolute)]
        [SettingDefault("/Account/ChangePassword")]
        public string ChangePasswordUrl { get; set; }
    }
}
