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
 *  File: BadEggTimingsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  10/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace BadEgg.Plugin.Classes.SystemAdmin
{
    /// <summary>
    /// Returns a list of current Timings and can be viewed within SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public sealed class BadEggTimingsSubMenu : SystemAdminSubMenu
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
        /// Returns Timings data in milliseconds for time spent processing by BadEgg.Plugin.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            string Result = "Setting|Value";

            Result += $"\rTotal Requests|{BadEggMiddleware._timings.Requests}";
            Result += $"\rFastest ms|{BadEggMiddleware._timings.Fastest}";
            Result += $"\rSlowest ms|{BadEggMiddleware._timings.Slowest}";
            Result += $"\rAverage ms|{BadEggMiddleware._timings.Average}";
            Result += $"\rTrimmed Avg ms|{BadEggMiddleware._timings.TrimmedAverage}";
            Result += $"\rTotal ms|{BadEggMiddleware._timings.Total}";

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
            return "Bad Egg";
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