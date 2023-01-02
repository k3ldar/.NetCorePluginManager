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
 *  Product:  SieraDeltaGeoIp.Plugin
 *  
 *  File: GeoIpStatsSubMenu.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace SieraDeltaGeoIp.Plugin.Classes
{
    /// <summary>
    /// Returns a summary of all preloaded and cached GeoIp records and the load time and can be viewed within 
    /// SystemAdmin.Plugin.  
    /// 
    /// This class descends from SystemAdminSubMenu.
    /// </summary>
    public sealed class GeoIpStatsSubMenu : SystemAdminSubMenu
    {
        #region Private Members

        private readonly INotificationService _notificationService;

        #endregion Private Members

        #region Constructors

        public GeoIpStatsSubMenu(INotificationService notificationService)
        {
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        #endregion Constructors

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
        /// Returns summary data and load time in milliseconds for time spent loading GeoIp data into cache.
        /// </summary>
        /// <returns>string</returns>
        public override string Data()
        {
            TimeSpan loadTime = new TimeSpan();
            uint recordsLoaded = 0;
            object result = null;

            if (_notificationService.RaiseEvent(Constants.NotificationEventGeoIpLoadTime, null, null, ref result))
                loadTime = (TimeSpan)result;

            if (_notificationService.RaiseEvent(Constants.NotificationEventGeoIpRecordCount, null, null, ref result))
                recordsLoaded = (uint)result;

            string Result = "Name|Value\r";

            Result += $"Provider|Firebird\r";
            Result += $"Loaded Records|{recordsLoaded}\r";
            Result += $"Load Time|{Convert.ToInt32(loadTime.TotalMilliseconds)} ms";

            return Result;
        }

        public override string Name()
        {
            return "GeoIP";
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