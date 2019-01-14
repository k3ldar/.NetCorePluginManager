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

namespace UserSessionMiddleware.Plugin
{
    public sealed class UserSessionSettings
    {
        [SettingDefault(".less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;")]
        public string StaticFileExtensions { get; set; }

        [SettingDefault("user_session")]
        public string CookieName { get; set; }

        [SettingDefault("Dfklaosre;lnfsdl;jlfaeu;dkkfcaskxcd3jf")]
        [SettingString(false, 20, 60)]
        public string EncryptionKey { get; set; }

        [SettingDefault(30u)]
        [SettingRange(15u, 200u)]
        public uint SessionTimeout { get; set; }

        [SettingDefault("en-GB")]
        [SettingString(false, 2u, 5u)]
        public string DefaultCulture { get; set; }
    }
}
