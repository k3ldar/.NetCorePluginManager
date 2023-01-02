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
 *  Product:  CacheControl Plugin
 *  
 *  File: CacheControlRoute.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AppSettings;

namespace CacheControl.Plugin
{
    /// <summary>
    /// Defines a rule for adding cache headers to an individual or collection of routes.
    /// 
    /// A route defined with a CacheMinutes which is less than 1 will have a header of no no-cache applied.
    /// </summary>
    public sealed class CacheControlRoute
    {
        /// <summary>
        /// String array of routes that will cache headers added to them.
        /// 
        /// Minimum 1 route, maximum 1500 routes
        /// </summary>
        /// <value>string[]</value>
        [SettingString(false, 1, 1500)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "ok on this occasion")]
        public string[] Route { get; set; }

        /// <summary>
        /// Number of minutes the route or file cache header will last
        /// 
        /// Minimum 0 minute
        /// Maximum Int32.MaxValue
        /// Default 120 minutes
        /// </summary>
        /// <value>int</value>
        [SettingRange(0, int.MaxValue)]
        [SettingDefault(120)]
        public int CacheMinutes { get; set; }

        internal string CacheValue { get; set; }
    }
}
