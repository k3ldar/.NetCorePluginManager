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
 *  File: GeoIpService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  04/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;

using Shared.Classes;

using SharedPluginFeatures;

namespace SieraDeltaGeoIp.Plugin
{
    public class GeoIpService : BaseCoreClass, IGeoIpDataService
    {
        #region Private Members

        private readonly CacheManager _geoIpCache;
        private readonly object _lockObject = new object();
        private readonly GeoIpPluginSettings _geoIpSettings;
        private readonly IGeoIpProvider _geoIpProvider;
        private readonly IGeoIpStatisticsUpdate _geoIpStatistics;
        private IpCity[] _geoIpCityData;
        private List<IpCity> _tempIpCity = new List<IpCity>();
        internal static Timings _timingsIpCache = new Timings();
        internal static Timings _timingsIpMemory = new Timings();
        internal static Timings _timingsIpDatabase = new Timings();

        #endregion Private Members

        #region Constructors

        public GeoIpService(ISettingsProvider settingsProvider)
        {
            _geoIpStatistics = Initialisation.GeoIpUpdate ?? throw new InvalidOperationException();

            ThreadManager.Initialise();
            _geoIpSettings = settingsProvider.GetSettings<GeoIpPluginSettings>("SieraDeltaGeoIpPluginConfiguration");

            // if the connection string is a file, it contains the actual connection string, load it
            if (_geoIpSettings.DatabaseConnectionString.IndexOfAny(Path.GetInvalidPathChars()) == -1 &&
                File.Exists(_geoIpSettings.DatabaseConnectionString))
            {
                using (StreamReader rdr = new StreamReader(_geoIpSettings.DatabaseConnectionString))
                {
                    _geoIpSettings.DatabaseConnectionString = rdr.ReadToEnd();
                }
            }

            ThreadManager dataThread = null;

            switch (_geoIpSettings.GeoIpProvider)
            {
                case Enums.GeoIpProvider.Firebird:
                    dataThread = new FirebirdDataProvider(_geoIpSettings, _tempIpCity);
                    break;

                case Enums.GeoIpProvider.MySql:
                    dataThread = new MySqlProvider(_geoIpSettings, _tempIpCity);
                    break;

                case Enums.GeoIpProvider.MSSql:
                    dataThread = new MSSQLProvider(_geoIpSettings, _tempIpCity);
                    break;

                default:
                    throw new InvalidOperationException();

            }

            _geoIpProvider = dataThread as IGeoIpProvider;

            if (_geoIpSettings.CacheAllData)
            {
                dataThread.ThreadFinishing += Thread_ThreadFinishing;
                ThreadManager.ThreadStart(dataThread, "Load GeoIp Data", System.Threading.ThreadPriority.Highest);
            }

            // create the cache
            _geoIpCache = new CacheManager("GeoIp Data Cache", new TimeSpan(24, 0, 0), true, false);
        }

        #endregion Constructors

        #region Public Methods

        public bool GetIPAddressDetails(in string ipAddress, out string countryCode, out string region, 
            out string cityName, out decimal latitude, out decimal longitude, out long uniqueID)
        {
            countryCode = "ZZ";
            region = String.Empty;
            cityName = String.Empty;
            latitude = -1;
            longitude = -1;
            uniqueID = -1;

            if (_geoIpCityData != null)
            {
                IpCity memoryIp = null;
                using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timingsIpMemory))
                {
                    memoryIp = GetMemoryCity(ipAddress);
                }

                if (memoryIp != null && !memoryIp.IsComplete)
                {
                    IGeoIpProvider provider = null;
                    switch (_geoIpSettings.GeoIpProvider)
                    {
                        case Enums.GeoIpProvider.None:
                            memoryIp.IsComplete = true;
                            break;
                    }

                    if (provider != null)
                    {
                        memoryIp.IsComplete = provider.GetIpAddressDetails(ipAddress, out countryCode, out region,
                            out cityName, out latitude, out longitude, out uniqueID, out long ipFrom, out long ipTo);
                    }

                    memoryIp.CountryCode = countryCode;
                    memoryIp.CityName = cityName;
                    memoryIp.Longitude = longitude;
                    memoryIp.Latitude = latitude;
                    memoryIp.Region = region;

                    return (true);
                }
            }

            return (GetCachedIPAddressDetails(ipAddress, out countryCode, out region, 
                out cityName, out latitude, out longitude, out uniqueID));
        }

        #endregion Public Methods

        #region Private Methods

        private bool GetCachedIPAddressDetails(in string ipAddress, out string countryCode, out string region,
            out string cityName, out decimal latitude, out decimal longitude, out long uniqueID)
        {
            CacheItem geoCacheItem = null;
            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timingsIpCache))
            {
                countryCode = "ZZ";
                region = String.Empty;
                cityName = String.Empty;
                latitude = -1;
                longitude = -1;
                uniqueID = -1;

                // Check to see if we have the ip in memory, if we do return that, if not, try and get it from ip provider
                geoCacheItem = _geoIpCache.Get(ipAddress);
            }

            if (geoCacheItem == null)
            {
                using (StopWatchTimer stopwatchDatabase = StopWatchTimer.Initialise(_timingsIpDatabase))
                {
                    if (_geoIpProvider != null &&
                        _geoIpProvider.GetIpAddressDetails(ipAddress, out countryCode,
                            out region, out cityName, out latitude, out longitude, out uniqueID, out long ipFrom, out long ipTo))
                    {
                        _geoIpCache.Add(ipAddress, new CacheItem(ipAddress, new IpCity(uniqueID, countryCode, region, cityName,
                            latitude, longitude, ipFrom, ipTo)));
                        return (true);
                    }
                }
            }

            // if not found in database
            if (geoCacheItem == null)
                return (false);

            IpCity city = (IpCity)geoCacheItem.Value;
            countryCode = city.CountryCode;
            region = city.Region;
            cityName = city.CityName;
            latitude = city.Latitude;
            longitude = city.Longitude;
            uniqueID = city.IpUniqueID;

            return (true);
        }

        private void Thread_ThreadFinishing(object sender, Shared.ThreadManagerEventArgs e)
        {
            TimeSpan span = DateTime.Now - e.Thread.TimeStart;
            _geoIpStatistics.Retrieve(Convert.ToInt64(span.TotalMilliseconds), (uint)_tempIpCity.Count);

            _geoIpCityData = _tempIpCity.ToArray();
            _tempIpCity.Clear();
            _tempIpCity = null;
        }

        private IpCity GetMemoryCity(string ipAddress)
        {
            long ip = Shared.Utilities.IPToLong(ipAddress);

            long min = 0;
            long max = _geoIpCityData.Length - 1;
            long mid = 0;

            while (min <= max)
            {
                mid = (min + max) / 2;

                if (ip <= _geoIpCityData[mid].IpEnd && ip >= _geoIpCityData[mid].IpStart)
                {
                    return (_geoIpCityData[mid]);
                }

                if (ip < _geoIpCityData[mid].IpStart)
                {
                    max = mid - 1;
                    continue;
                }

                if (ip > _geoIpCityData[mid].IpEnd)
                {
                    min = mid + 1;
                }
            }

            return (null);
        }

        #endregion Private Methods
    }
}
