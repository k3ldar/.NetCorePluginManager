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
 *  Product:  BadEgg.Plugin
 *  
 *  File: BadEggSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace BadEgg.Plugin
{
    public class BadEggSettings
    {
        #region Properties

        [SettingRange(1u, 300u)]
        [SettingDefault(5u)]
        public uint ConnectionTimeOut { get; set; }

        [SettingRange(5u, uint.MaxValue)]
        [SettingDefault(100u)]
        public uint ConnectionsPerMinute { get; set; }

        [SettingDefault(400)]
        [SettingHttpResponse(HttpResponseType.ClientErrors)]
        public int BannedResponseCode { get; set; }

        [SettingDefault(429)]
        [SettingHttpResponse(HttpResponseType.ClientErrors)]
        public int TooManyRequestResponseCode { get; set; }

        #endregion Properties
    }
}
