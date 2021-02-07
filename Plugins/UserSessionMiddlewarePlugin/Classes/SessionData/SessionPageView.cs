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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: SessionPageView.cs
 *
 *  Purpose:  Page view statistics
 *
 *  Date        Name                Reason
 *  02/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace UserSessionMiddleware.Plugin.Classes.SessionData
{
    /// <summary>
    /// Statistics for a page view
    /// </summary>
    public sealed class SessionPageView
    {
        /// <summary>
        /// Page hash for searching
        /// </summary>
        /// <value>string</value>
        public string Hash { get; set; }

        /// <summary>
        /// Url of page being visited
        /// </summary>
        /// <value>string</value>
        public string Url { get; set; }

        /// <summary>
        /// Year visit was made
        /// </summary>
        /// <value>int</value>
        public int Year { get; set; }

        /// <summary>
        /// Month the visit was made
        /// </summary>
        /// <value>byte</value>
        public byte Month { get; set; }

        /// <summary>
        /// Number of humans visiting the page
        /// </summary>
        /// <value>uint</value>
        public uint HumanCount { get; set; }

        /// <summary>
        /// Number of bots visiting the page
        /// </summary>
        /// <value>uint</value>
        public uint BotCount { get; set; }

        /// <summary>
        /// Number of times the page has been bounced
        /// </summary>
        /// <value>uint</value>
        public uint BounceCount { get; set; }

        /// <summary>
        /// Total time spent viewing the page (human visitors only)
        /// </summary>
        public double TotalTime { get; set; }
    }
}
