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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SieraDeltaGeoIpPlugin
 *  
 *  File: GeoIpStatistics.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  04/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using SharedPluginFeatures;

using Shared.Classes;

namespace SieraDeltaGeoIp.Plugin
{
    public class GeoIpStatistics : IGeoIpStatistics, IGeoIpStatisticsUpdate
    {
        #region Private Members

        private static object _lockObject = new object();

        public long _memoryMilliseconds;
        public long _databaseMilliseconds;
        public long _cacheMilliseconds;
        private uint _recordsLoaded;
        private TimeSpan _loadTime;
        private uint _memoryRetrieveQuickest;
        private uint _memoryRetrieveSlowest;
        private double _memoryRetrieveAverage;
        private uint _memoryRetrievedCount;
        private uint _databaseRetrieveQuickest;
        private uint _databaseRetrieveSlowest;
        private double _databaseRetrieveAverage;
        private uint _databaseRetrievedCount;
        private uint _cacheRetrieveQuickest;
        private uint _cacheRetrieveSlowest;
        private double _cacheRetrieveAverage;
        private uint _cacheRetrievedCount;

        #endregion Private Members

        #region Constructors

        public GeoIpStatistics()
        {
            _databaseRetrieveQuickest = uint.MaxValue;
            _databaseRetrieveSlowest = uint.MinValue;
            _memoryRetrieveQuickest = uint.MaxValue;
            _memoryRetrieveSlowest = uint.MinValue;
            _cacheRetrieveSlowest = uint.MinValue;
            _cacheRetrieveQuickest = uint.MaxValue;
        }

        #endregion Constructors

        #region IGeoIpStatistics Methods

        double IGeoIpStatistics.DatabaseRetrieveAverage()
        {
            return (_databaseRetrieveAverage);
        }

        uint IGeoIpStatistics.DatabaseRetrievedCount()
        {
            return (_databaseRetrievedCount);
        }

        uint IGeoIpStatistics.DatabaseRetrieveQuickest()
        {
            if (_databaseRetrieveQuickest == uint.MaxValue)
                return (0);

            return (_databaseRetrieveQuickest);
        }

        uint IGeoIpStatistics.DatabaseRetrieveSlowest()
        {
            if (_databaseRetrieveSlowest == uint.MinValue)
                return (0);

            return (_databaseRetrieveSlowest);
        }

        TimeSpan IGeoIpStatistics.LoadTime()
        {
            return (_loadTime);
        }

        uint IGeoIpStatistics.RecordsLoaded()
        {
            return (_recordsLoaded);
        }

        double IGeoIpStatistics.MemoryRetrieveAverage()
        {
            return (_memoryRetrieveAverage);
        }

        uint IGeoIpStatistics.MemoryRetrievedCount()
        {
            return (_memoryRetrievedCount);
        }

        uint IGeoIpStatistics.MemoryRetrieveQuickest()
        {
            if (_memoryRetrieveQuickest == uint.MaxValue)
                return (0);

            return (_memoryRetrieveQuickest);
        }

        uint IGeoIpStatistics.MemoryRetrieveSlowest()
        {
            if (_memoryRetrieveSlowest == uint.MinValue)
                return (0);

            return (_memoryRetrieveSlowest);
        }

        double IGeoIpStatistics.CacheRetrieveAverage()
        {
            return (_cacheRetrieveAverage);
        }

        uint IGeoIpStatistics.CacheRetrievedCount()
        {
            return (_cacheRetrievedCount);
        }

        uint IGeoIpStatistics.CacheRetrieveQuickest()
        {
            if (_cacheRetrieveQuickest == uint.MaxValue)
                return (0);

            return (_cacheRetrieveQuickest);
        }

        uint IGeoIpStatistics.CacheRetrieveSlowest()
        {
            if (_cacheRetrieveSlowest == uint.MinValue)
                return (0);

            return (_cacheRetrieveSlowest);
        }

        #endregion IGeoIpStatistics Methods

        #region IGeoIpStatisticsUpdate Methods

        public void DatabaseRetrieve(in long milliseconds)
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                uint newMilliSeconds = milliseconds < 0 ? 0 : (uint)milliseconds;
                _databaseRetrievedCount++;
                _databaseMilliseconds += milliseconds;

                if (milliseconds < _databaseRetrieveQuickest)
                    _databaseRetrieveQuickest = newMilliSeconds;

                if (milliseconds > _databaseRetrieveSlowest)
                    _databaseRetrieveSlowest = newMilliSeconds;

                _databaseRetrieveAverage = _databaseMilliseconds / _databaseRetrievedCount;
            }
        }

        public void MemoryRetrieve(in long milliseconds)
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                uint newMilliSeconds = milliseconds < 0 ? 0 : (uint)milliseconds;
                _memoryRetrievedCount++;
                _memoryMilliseconds += milliseconds;

                if (milliseconds < _memoryRetrieveQuickest)
                    _memoryRetrieveQuickest = newMilliSeconds;

                if (milliseconds > _memoryRetrieveSlowest)
                    _memoryRetrieveSlowest = newMilliSeconds;

                _memoryRetrieveAverage = _memoryMilliseconds / _databaseRetrievedCount;
            }
        }

        public void CacheRetrieve(in long milliseconds)
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                uint newMilliSeconds = milliseconds < 0 ? 0 : (uint)milliseconds;
                _cacheRetrievedCount++;
                _cacheMilliseconds += milliseconds;

                if (milliseconds < _cacheRetrieveQuickest)
                    _cacheRetrieveQuickest = newMilliSeconds;

                if (milliseconds > _cacheRetrieveSlowest)
                    _cacheRetrieveSlowest = newMilliSeconds;

                _cacheRetrieveAverage = _cacheMilliseconds / _cacheRetrievedCount;
            }
        }

        public void Retrieve(in long milliseconds, in uint recordCount)
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                _recordsLoaded = recordCount;
                _loadTime = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(milliseconds));
            }
        }

        #endregion IGeoIpStatisticsUpdate Methods
    }
}
