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

using SharedPluginFeatures;

namespace GeoIp.Plugin
{
    public class IpStackSettings : IGeoIpProvider
    {
        #region Properties

        public string ApiKey { get; set; }

        #endregion Properties

        #region Internal Methods

        /// <summary>
        /// Method which obtains geo ip data from IpStack
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="countryCode"></param>
        /// <param name="region"></param>
        /// <param name="cityName"></param>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="ipUniqueID"></param>
        /// <returns></returns>
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

            System.Net.WebClient wc = new System.Net.WebClient();
            try
            {
                byte[] raw = wc.DownloadData($"http://api.ipstack.com/{ipAddress}?access_key={ApiKey}");

                string webData = System.Text.Encoding.UTF8.GetString(raw);

                IpStackData city = Newtonsoft.Json.JsonConvert.DeserializeObject<IpStackData>(webData);

                if (!city.success)
                {
                    Initialisation.GetLogger.AddToLog(SharedPluginFeatures.Enums.LogLevel.GeoIpStackError,
                        $"{city.error.code} {city.error.info} {city.error.type}");
                    return (false);
                }

                countryCode = city.country_code ?? "ZZ";
                region = city.region_name ?? String.Empty;
                cityName = city.city ?? String.Empty;
                latitude = city.latitude.Value;
                longitude = city.longitude.Value;

                return (true);
            }
            catch
            {
                return (false);
            }
            finally
            {
                wc.Dispose();
                wc = null;
            }
        }

        #endregion Internal Methods
    }


    public sealed class IpStackData
    {
        public IpStackData()
        {
            error = new IpStackError();
        }

        public bool success { get; set; }
        public string ip { get; set; }
        public string type { get; set; }
        public string continent_code { get; set; }
        public string continent_name { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public string region_code { get; set; }
        public string region_name { get; set; }
        public string city { get; set; }
        public string zip { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public IpStackError error { get; set; }
    }

    public class IpStackError
    {
        public string code { get; set; }
        public string type { get; set; }
        public string info { get; set; }
    }

}
