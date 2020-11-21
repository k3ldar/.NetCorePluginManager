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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: IGeoIpData.cs
 *
 *  Purpose:  Provides interface for retrievin geo ip data
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Interface to obtain GeoIp details for a specific Ip Address
    /// </summary>
    [Obsolete("This interface is obsolete and will be removed, please use IGeoIpProvider instead")]
    public interface IGeoIpDataService
    {
        /// <summary>
        /// Obtains the Geo Ip details pertaining to an Ip Address
        /// </summary>
        /// <param name="ipAddress">Ip Address</param>
        /// <param name="countryCode">Country code where the Ip address is located.</param>
        /// <param name="region">Region where the Ip address is located.</param>
        /// <param name="cityName">Name of the city where the Ip address is located.</param>
        /// <param name="latitude">Latitude for the Ip address.</param>
        /// <param name="longitude">Longitude for the Ip address.</param>
        /// <param name="ipUniqueID">Unique Id for the Ip address.</param>
        /// <returns></returns>
        [Obsolete("This method is obsolete and will be removed, please use IGeoIpProvider.GetIPAddressDetails() instead")]
        bool GetIPAddressDetails(in string ipAddress, out string countryCode, out string region,
            out string cityName, out decimal latitude, out decimal longitude, out long ipUniqueID);
    }
}
