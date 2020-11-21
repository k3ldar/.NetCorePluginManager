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
 *  Product:  GeoIpPlugin
 *  
 *  File: GeoIpMemoryTimingsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace GeoIp.Plugin.Classes.SystemAdmin
{
    /// <summary>
    /// Returns Timings information for all time spent loading Ip data from a data store and can 
    /// be viewed within SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public class GeoIpMemoryTimingsSubMenu : SystemAdminSubMenu
    {
        public override string Action()
        {
            return String.Empty;
        }

        public override string Area()
        {
            return String.Empty;
        }

        public override string Controller()
        {
            return String.Empty;
        }

        /// <summary>
        /// Returns Timings data in milliseconds for time spent processing by GeoIpPlugin memory loading.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            string Result = "Setting|Value";

            Result += $"\rTotal Requests|{GeoIpService._timingsIpMemory.Requests}";
            Result += $"\rFastest ms|{GeoIpService._timingsIpMemory.Fastest}";
            Result += $"\rSlowest ms|{GeoIpService._timingsIpMemory.Slowest}";
            Result += $"\rAverage ms|{GeoIpService._timingsIpMemory.Average}";
            Result += $"\rTrimmed Avg ms|{GeoIpService._timingsIpMemory.TrimmedAverage}";
            Result += $"\rTotal ms|{GeoIpService._timingsIpMemory.Total}";

            return Result;
        }

        public override string Image()
        {
            return Constants.SystemImageStopWatch;
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.Grid;
        }

        public override string Name()
        {
            return "GeoIp Memory";
        }

        public override string ParentMenuName()
        {
            return "Timings";
        }

        public override int SortOrder()
        {
            return 0;
        }
    }
}

#pragma warning restore CS1591