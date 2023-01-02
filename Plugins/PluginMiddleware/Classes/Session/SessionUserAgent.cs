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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Middleware.Plugin
 *  
 *  File: SessionUserAgent.cs
 *
 *  Purpose:  Session user agent
 *
 *  Date        Name                Reason
 *  28/09/2020  Simon Carter        Initially Created
 *  02/08/2022	Simon Carter		Moved to middleware
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Middleware.SessionData
{
    /// <summary>
    /// User agent session usage data
    /// </summary>
    public sealed class SessionUserAgent
    {
        /// <summary>
        /// User agent name
        /// </summary>
        /// <value>string</value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Number of times it has appeared
        /// </summary>
        /// <value>uint</value>
        public uint Count { get; set; }

        /// <summary>
        /// Indicates whether it was recognised as a bot or not
        /// </summary>
        /// <value>bool</value>
        public bool IsBot { get; set; }
    }
}
