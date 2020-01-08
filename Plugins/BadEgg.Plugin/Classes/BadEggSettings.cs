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
    /// <summary>
    /// Settings which affect how BadEgg is configured and used.
    /// </summary>
    public class BadEggSettings
    {
        #region Properties

        /// <summary>
        /// Number of minutes until a connection is timed out and removed from the list of monitored connections.
        /// 
        /// Default value: 5
        /// Minimum value: 1
        /// Maximum value: 300
        /// </summary>
        /// <value>uint</value>
        [SettingRange(1u, 300u)]
        [SettingDefault(5u)]
        public uint ConnectionTimeOut { get; set; }

        /// <summary>
        /// Maximum average connection per minute, if this value is exceeded then http response from TooManyRequestResponseCode will be returned
        /// 
        /// Default value: 100
        /// Minimum value: 5
        /// Maximum value: uint.MaxValue
        /// </summary>
        /// <value>uint</value>
        [SettingRange(5u, uint.MaxValue)]
        [SettingDefault(100u)]
        public uint ConnectionsPerMinute { get; set; }

        /// <summary>
        /// Http response code provided should the connection be banned.
        /// 
        /// Default: 400
        /// 
        /// Must be a valid http client error response (in the range of 400)
        /// </summary>
        /// <value>int</value>
        [SettingDefault(400)]
        [SettingHttpResponse(HttpResponseType.ClientErrors)]
        public int BannedResponseCode { get; set; }

        /// <summary>
        /// Http response provided should the connection make too many requests.
        /// 
        /// Default Value: 429
        /// 
        /// Must be a valid http client error response (in the range of 400)
        /// </summary>
        /// <value>int</value>
        [SettingDefault(429)]
        [SettingHttpResponse(HttpResponseType.ClientErrors)]
        public int TooManyRequestResponseCode { get; set; }

        #endregion Properties
    }
}
