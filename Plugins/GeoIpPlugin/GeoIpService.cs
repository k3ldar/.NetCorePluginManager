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
 *  Product:  MemoryCachePlugin
 *  
 *  File: GeoIpService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Shared.Classes;

using Microsoft.Extensions.Configuration;

using SharedPluginFeatures;

namespace GeoIp.Plugin
{
    public class GeoIpService : IGeoIpDataService
    {
        #region Private Members

        private readonly CacheManager _geoIpCache;
        private readonly object _lockObject = new object();
        private readonly GeoIpPluginSettings _geoIpSettings;
        private IpCity[] _geoIpCityData;
        private List<IpCity> _tempIpCity = new List<IpCity>();

        #endregion Private Members

        #region Constructors

        public GeoIpService()
        {
            _geoIpSettings = GetSettings();

            if (System.IO.File.Exists(_geoIpSettings.Webnet77CSVData))
            {
                ThreadManager.Initialise();

                LoadWebNet77Data loadWebNet77DataThread = new LoadWebNet77Data(_geoIpSettings.Webnet77CSVData, _tempIpCity);
                loadWebNet77DataThread.ThreadFinishing += LoadWebNet77DataThread_ThreadFinishing;
                ThreadManager.ThreadStart(loadWebNet77DataThread, "Load GeoIp Data", System.Threading.ThreadPriority.Highest);
            }

            // create the caches
            _geoIpCache = new CacheManager("GeoIp Data Cache", new TimeSpan(24, 0, 0, 0), true, false);
        }

        #endregion Constructors

        #region Public Methods

        public bool GetIPAddressDetails(in string ipAddress, out string countryCode, out string region, 
            out string cityName, out decimal latitude, out decimal longitude, out long ipUniqueID)
        {
            countryCode = "ZZ";
            region = String.Empty;
            cityName = String.Empty;
            latitude = -1;
            longitude = -1;
            ipUniqueID = -1;

            // works in 2 ways, if we have loaded WebNet77 data, we have ip ranges and will supplement them
            // if we are using a geoip provider.  if no geoip provider only the country code is there lat/lon
            // is not available.
            if (_geoIpCityData != null)
            {
                IpCity memoryIp = GetMemoryCity(ipAddress);

                if (memoryIp != null && !memoryIp.IsComplete)
                {
                    IGeoIpProvider provider = null;
                    switch (_geoIpSettings.GeoIpProvider)
                    {
                        case Enums.GeoIpProvider.None:
                            memoryIp.IsComplete = true;
                            break;

                        case Enums.GeoIpProvider.IpStack:
                            provider = _geoIpSettings.IpStack as IGeoIpProvider;
                            break;

                        default:
                            throw new InvalidOperationException($"Invalid GeoIpProvider Enum");
                    }

                    if (provider != null)
                    {
                        memoryIp.IsComplete = provider.GetIpAddressDetails(ipAddress, out countryCode, out region,
                            out cityName, out latitude, out longitude);
                    }

                    memoryIp.CountryCode = countryCode;
                    memoryIp.CityName = cityName;
                    memoryIp.Longitude = longitude;
                    memoryIp.Latitude = latitude;
                    memoryIp.Region = region;

                    return (true);
                }
            }

            // WebNet77 data is not present, check to see if we have the ip in memory, if we do
            // return that, if not, try and get it from ip provider
            return (GetCachedIPAddressDetails(ipAddress, out countryCode, out region, 
                out cityName, out latitude, out longitude, out ipUniqueID));
        }

        #endregion Public Methods

        #region Private Methods

        private bool GetCachedIPAddressDetails(in string ipAddress, out string countryCode, out string region,
            out string cityName, out decimal latitude, out decimal longitude, out long ipUniqueID)
        {
            countryCode = "ZZ";
            region = String.Empty;
            cityName = String.Empty;
            latitude = -1;
            longitude = -1;
            ipUniqueID = -1;

            // WebNet77 data is not present, check to see if we have the ip in memory, if we do
            // return that, if not, try and get it from ip provider
            CacheItem geoCacheItem = _geoIpCache.Get(ipAddress);

            if (geoCacheItem == null)
            {
                IGeoIpProvider provider = null;

                switch (_geoIpSettings.GeoIpProvider)
                {
                    case Enums.GeoIpProvider.None:
                        break;

                    case Enums.GeoIpProvider.IpStack:
                        provider = _geoIpSettings.IpStack as IGeoIpProvider;
                        break;

                    default:
                        throw new InvalidOperationException($"Invalid GeoIpProvider Enum");
                }

                if (provider != null && 
                    provider.GetIpAddressDetails(ipAddress, out countryCode, out region, out cityName, out latitude, out longitude))
                {
                    _geoIpCache.Add(ipAddress, new CacheItem(ipAddress, new IpCity(0, 0, countryCode)
                    {
                        Region = region,
                        CityName = cityName,
                        Latitude = latitude,
                        Longitude = longitude
                    }));

                    return (true);
                }
            }

            if (geoCacheItem == null)
                return (false);

            IpCity city = (IpCity)geoCacheItem.Value;
            countryCode = city.CountryCode;
            region = city.Region;
            cityName = city.CityName;
            latitude = city.Latitude;
            longitude = city.Longitude;
            ipUniqueID = -1;

            return (true);
        }

        private void LoadWebNet77DataThread_ThreadFinishing(object sender, Shared.ThreadManagerEventArgs e)
        {
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

        private GeoIpPluginSettings GetSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            GeoIpPluginSettings Result = new GeoIpPluginSettings();
            config.GetSection("GeoIpPluginConfiguration").Bind(Result);

            return (Result);
        }

        #endregion Private Methods
    }
}
