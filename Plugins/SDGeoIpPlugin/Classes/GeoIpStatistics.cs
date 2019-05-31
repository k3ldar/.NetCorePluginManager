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
    internal class GeoIpStatistics : IGeoIpStatistics, IGeoIpStatisticsUpdate
    {
        #region Private Members

        private static readonly object _lockObject = new object();

        public long _memoryMilliseconds;
        public long _databaseMilliseconds;
        public long _cacheMilliseconds;
        private uint _recordsLoaded;
        private TimeSpan _loadTime;

        #endregion Private Members

        #region Constructors

        public GeoIpStatistics()
        {

        }

        #endregion Constructors

        #region IGeoIpStatistics Methods

        TimeSpan IGeoIpStatistics.LoadTime()
        {
            return (_loadTime);
        }

        uint IGeoIpStatistics.RecordsLoaded()
        {
            return (_recordsLoaded);
        }

        #endregion IGeoIpStatistics Methods

        #region IGeoIpStatisticsUpdate Methods

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
