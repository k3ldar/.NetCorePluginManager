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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IGeoIpData.cs
 *
 *  Purpose:  Provides interface for retrievin geo ip data
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace SharedPluginFeatures
{
    /// <summary>
    /// Provides a mechanism whereby the host application can obtain GeoIp related data for an Ip Address.
    /// 
    /// This is typically implemented by the GeoIpPlugin and SieraDeltaGeoIpPlugin modules.
    /// </summary>
    public interface IGeoIpProvider
    {
        /// <summary>
        /// Method for obtaing Geo Ip specific data for an Ip address.
        /// </summary>
        /// <param name="ipAddress">in string.  Ip address.  Geo Ip information specific to this address will be returned.</param>
        /// <param name="countryCode">out string.  Code of country where Ip address is located.</param>
        /// <param name="region">out string.  Region within a country where the Ip address is located.</param>
        /// <param name="cityName">out string.  Name of the city within a region where the Ip address is located.</param>
        /// <param name="latitude">out decimal.  Latitude  where the Ip address is located.</param>
        /// <param name="longitude">out decimal.  Longitude  where the Ip address is located.</param>
        /// <param name="uniqueId">out long.  Unique Id used internally by the provider to refer to the Geo Ip data.</param>
        /// <param name="ipFrom">out long.  Start of range which the Ip address belongs.</param>
        /// <param name="ipTo">out long.  End of range which the Ip address belongs.</param>
        /// <returns></returns>
        bool GetIpAddressDetails(in string ipAddress, out string countryCode, out string region,
            out string cityName, out decimal latitude, out decimal longitude, out long uniqueId,
            out long ipFrom, out long ipTo);
    }
}
