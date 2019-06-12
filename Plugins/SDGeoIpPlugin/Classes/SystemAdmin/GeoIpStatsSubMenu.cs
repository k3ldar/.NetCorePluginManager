using System;
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

        public override Enums.SystemAdminMenuType MenuType()
        {
            return (Enums.SystemAdminMenuType.Grid);
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

            return (Result);
        }

        public override string Name()
        {
            return ("GeoIP");
        }

        public override string ParentMenuName()
        {
            return ("Timings");
        }

        public override int SortOrder()
        {
            return (0);
        }

        public override string Image()
        {
            return ("stopwatch");
        }
    }
}

#pragma warning restore CS1591