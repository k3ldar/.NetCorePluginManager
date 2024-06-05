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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PageLoadSpeedMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  30/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Text;

using AspNetCore.PluginManager.Middleware;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace AspNetCore.PluginManager.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of load times for individual pages and can be viewed within SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public sealed class RouteLoadTimeMenu : SystemAdminSubMenu
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

        public override Enums.SystemAdminMenuType MenuType()
        {
            return Enums.SystemAdminMenuType.Grid;
        }

        /// <summary>
        /// Returns delimited data on all loaded assemblies and their version.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            Dictionary<string, Timings> routeTimings = RouteLoadTimeMiddleware.ClonePageTimings();
            StringBuilder Result = new("Route ms|Total Requests|Fastest ms|Slowest ms|Average ms|Trimmed Avg ms|Total ms");

            foreach (KeyValuePair<string, Timings> route in routeTimings)
            {
                Result.AppendFormat("\r{0}|{1}|{2}|{3}|{4}|{5}|{6}",
                    route.Key,
                    route.Value.Requests,
                    route.Value.Fastest,
                    route.Value.Slowest,
                    route.Value.Average,
                    route.Value.TrimmedAverage,
                    route.Value.Total);
            }


            return Result.ToString();
        }

        public override string Name()
        {
            return "Route Load Times";
        }

        public override string ParentMenuName()
        {
            return "Timings";
        }

        public override int SortOrder()
        {
            return 0;
        }

        public override string Image()
        {
            return Constants.SystemImageStopWatch;
        }
    }
}

#pragma warning restore CS1591