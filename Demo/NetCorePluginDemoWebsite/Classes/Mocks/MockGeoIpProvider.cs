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
 *  Product:  Demo Website
 *  
 *  File: MockGeoIpProvider.cs
 *
 *  Purpose:  Mock IGeoIpProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  09/12/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.DemoWebsite.Classes.Mocks
{
    [ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
    public sealed class MockGeoIpProvider : IGeoIpProvider
    {
        public Boolean GetIpAddressDetails(in String ipAddress, out String countryCode, out String region,
            out String cityName, out Decimal latitude, out Decimal longitude, out Int64 uniqueId,
            out Int64 ipFrom, out Int64 ipTo)
        {
            countryCode = null;
            region = null;
            cityName = null;
            latitude = 0;
            longitude = 0;
            uniqueId = 0;
            ipFrom = 0;
            ipTo = 0;
            return false;
        }
    }
}
