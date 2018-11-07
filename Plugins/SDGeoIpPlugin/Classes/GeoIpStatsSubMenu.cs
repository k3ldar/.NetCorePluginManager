using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace SieraDeltaGeoIp.Plugin.Classes
{
    public sealed class GeoIpStatsSubMenu : SystemAdminSubMenu
    {
        #region Private Members

        private readonly IGeoIpStatistics _geoIpStatistics;

        #endregion Private Members

        #region Constructors

        public GeoIpStatsSubMenu()
        {
            _geoIpStatistics = Initialisation.GeoIpStatistics;
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

        public override string Data()
        {
            string Result = "Name|Value\r";

            Result += $"Provider|Firebird\r";
            Result += $"Loaded Records|{_geoIpStatistics.RecordsLoaded()}\r";
            Result += $"Load Time|{Convert.ToInt32(_geoIpStatistics.LoadTime().TotalMilliseconds)} ms\r";
            Result += $"Database Average|{_geoIpStatistics.DatabaseRetrieveAverage()} ms\r";
            Result += $"Database Quickest|{_geoIpStatistics.DatabaseRetrieveQuickest()} ms\r";
            Result += $"Database Slowest|{_geoIpStatistics.DatabaseRetrieveSlowest()} ms\r";
            Result += $"Database Retrieve Count|{_geoIpStatistics.DatabaseRetrievedCount()}\r";
            Result += $"Memory Average|{_geoIpStatistics.MemoryRetrieveAverage()} ms\r";
            Result += $"Memory Quickest|{_geoIpStatistics.MemoryRetrieveQuickest()} ms\r";
            Result += $"Memory Slowest|{_geoIpStatistics.MemoryRetrieveSlowest()} ms\r";
            Result += $"Memory Retrieve Count|{_geoIpStatistics.MemoryRetrievedCount()}";

            return (Result);
        }

        public override string Name()
        {
            return ("GeoIp Statistics");
        }

        public override string ParentMenuName()
        {
            return ("System");
        }

        public override int SortOrder()
        {
            return (0);
        }

        public override string Image()
        {
            return (String.Empty);
        }
    }
}
