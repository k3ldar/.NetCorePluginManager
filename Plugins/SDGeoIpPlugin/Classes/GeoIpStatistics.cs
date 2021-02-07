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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
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
using System.Collections.Generic;

using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace SieraDeltaGeoIp.Plugin
{
    internal class GeoIpStatistics : INotificationListener
    {
        #region Private Members

        private uint _recordsLoaded;
        private TimeSpan _loadTime;

        #endregion Private Members

        #region INotificationListener Methods

        public bool EventRaised(in string eventId, in object param1, in object param2, ref object result)
        {
            switch (eventId)
            {
                case Constants.NotificationEventGeoIpLoadTime:
                    result = _loadTime;
                    return true;

                case Constants.NotificationEventGeoIpRecordCount:
                    result = _recordsLoaded;
                    return true;
            }

            return false;
        }

        public void EventRaised(in string eventId, in object param1, in object param2)
        {
            switch (eventId)
            {
                case Constants.NotificationEventGeoIpLoadTime:
                    _loadTime = (TimeSpan)param1;
                    return;

                case Constants.NotificationEventGeoIpRecordCount:
                    _recordsLoaded = (uint)param1;
                    return;
            }
        }

        public List<string> GetEvents()
        {
            return new List<string>
            {
                Constants.NotificationEventGeoIpLoadTime,
                Constants.NotificationEventGeoIpRecordCount,
            };
        }

        #endregion INotificationListener Methods
    }
}
