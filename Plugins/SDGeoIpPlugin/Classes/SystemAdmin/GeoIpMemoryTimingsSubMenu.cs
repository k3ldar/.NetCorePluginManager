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
 *  Product:  SieraDeltaGeoIp.Plugin
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

namespace SieraDeltaGeoIp.Plugin.Classes.SystemAdmin
{
    public class GeoIpMemoryTimingsSubMenu : SystemAdminSubMenu
    {
        public override string Action()
        {
            return (String.Empty);
        }

        public override string Area()
        {
            return (String.Empty);
        }

        public override string Controller()
        {
            return (String.Empty);
        }

        public override string Data()
        {
            string Result = "Setting|Value";

            Result += $"\rTotal Requests|{GeoIpService._timingsIpMemory.Requests}";
            Result += $"\rFastest ms|{GeoIpService._timingsIpMemory.Fastest}";
            Result += $"\rSlowest ms|{GeoIpService._timingsIpMemory.Slowest}";
            Result += $"\rAverage ms|{GeoIpService._timingsIpMemory.Average}";
            Result += $"\rTotal ms|{GeoIpService._timingsIpMemory.Total}";

            return (Result);
        }

        public override string Image()
        {
            return ("stopwatch");
        }

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
        }

        public override string Name()
        {
            return ("GeoIp Memory");
        }

        public override string ParentMenuName()
        {
            return ("Timings");
        }

        public override int SortOrder()
        {
            return (0);
        }
    }
}
