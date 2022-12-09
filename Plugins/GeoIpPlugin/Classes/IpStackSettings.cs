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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  GeoIpPlugin
 *  
 *  File: IpStackSettings.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Net.Http;
using System.Text.Json;

using AppSettings;

using PluginManager;

using SharedPluginFeatures;

#pragma warning disable CA1707, IDE1006

namespace GeoIp.Plugin
{
    /// <summary>
    /// IpStackSettings, used to retrieve IpStack data.  Implements IGeoIpProvider interface.
    /// </summary>
    public class IpStackSettings : IGeoIpProvider
    {
        #region Properties

        /// <summary>
        /// IpStack Api Key.
        /// </summary>
        [SettingString(0, 100)]
        public string ApiKey { get; set; }

        #endregion Properties

        #region Internal Methods

        /// <summary>
        /// Method which obtains geo ip data from IpStack
        /// </summary>
        /// <param name="ipAddress">in string.  Ip address whos data is sought.</param>
        /// <param name="countryCode">out string.  Country code for Ip Address</param>
        /// <param name="region">out string.  Region within country where the Ip address is located.</param>
        /// <param name="cityName">out string.  Name of city within the region where the Ip address is located.</param>
        /// <param name="latitude">out decimal.  Latitude where Ip address is located.</param>
        /// <param name="longitude">out decimal.  Longitude where Ip address is located.</param>
        /// <param name="uniqueId">out long.  Unique id for Ip address.</param>
        /// <param name="ipFrom">out long.  Ip from range.</param>
        /// <param name="ipTo">out long.  Ip to range.</param>
        /// <returns>bool</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public bool GetIpAddressDetails(in string ipAddress, out string countryCode, out string region,
            out string cityName, out decimal latitude, out decimal longitude, out long uniqueId,
            out long ipFrom, out long ipTo)
        {
            countryCode = "ZZ";
            region = String.Empty;
            cityName = String.Empty;
            latitude = -1;
            longitude = -1;
            uniqueId = -1;
            ipFrom = 0;
            ipTo = 0;

            HttpClient client = new HttpClient();
            try
            {
                byte[] response = client.GetByteArrayAsync($"http://api.ipstack.com/{ipAddress}?access_key={ApiKey}").Result;

                string webData = System.Text.Encoding.UTF8.GetString(response);
                IpStackData city = JsonSerializer.Deserialize<IpStackData>(webData);

                if (!city.success)
                {
                    PluginInitialisation.GetLogger.AddToLog(LogLevel.Error, String.Empty,
                        $"{city.error.code} {city.error.info} {city.error.type}");
                    return false;
                }

                countryCode = city.country_code ?? "ZZ";
                region = city.region_name ?? String.Empty;
                cityName = city.city ?? String.Empty;
                latitude = city.latitude.Value;
                longitude = city.longitude.Value;

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                client.Dispose();
            }
        }

        #endregion Internal Methods
    }

    /// <summary>
    /// Class to hold Ip Stack data returned after ip request.
    /// </summary>
    public sealed class IpStackData
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public IpStackData()
        {
            error = new IpStackError();
        }

        /// <summary>
        /// Determines whether the request was successful or not.
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// Ip Address
        /// </summary>
        public string ip { get; set; }

        /// <summary>
        /// Type of Ip Address
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Code for continent where ip is located
        /// </summary>
        public string continent_code { get; set; }

        /// <summary>
        /// Name of continent where ip is located.
        /// </summary>
        public string continent_name { get; set; }

        /// <summary>
        /// Country code
        /// </summary>
        public string country_code { get; set; }

        /// <summary>
        /// Name of country
        /// </summary>
        public string country_name { get; set; }

        /// <summary>
        /// Region Code
        /// </summary>
        public string region_code { get; set; }

        /// <summary>
        /// Region Name
        /// </summary>
        public string region_name { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Zip / Postcode
        /// </summary>
        public string zip { get; set; }

        /// <summary>
        /// Latitude where Ip address is located.
        /// </summary>
        public decimal? latitude { get; set; }

        /// <summary>
        /// Longitude where the Ip address is located.
        /// </summary>
        public decimal? longitude { get; set; }

        /// <summary>
        /// IpStackError information if an error occurred during the request.
        /// </summary>
        public IpStackError error { get; set; }
    }

    /// <summary>
    /// IpStackError class holds error information if a request is not successful.
    /// </summary>
    public class IpStackError
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// Error type.
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// Error information
        /// </summary>
        public string info { get; set; }
    }
}

#pragma warning restore CA1707, IDE1006