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
 *  Product:  GeoIpPlugin
 *  
 *  File: IpCity.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace GeoIp.Plugin
{
    public class IpCity : IComparable
    {
        #region Constructors

        internal IpCity(long startRange, long endRange, string countryCode)
        {
            IsComplete = false;
            IpStart = startRange;
            IpEnd = endRange;
            CountryCode = countryCode;
        }

        #endregion Constructors

        #region Properties

        internal long IpStart { get; set; }

        internal long IpEnd { get; set; }

        internal string CountryCode { get; set; }

        internal string Region { get; set; }

        internal string CityName { get; set; }

        internal decimal Latitude { get; set; }

        internal decimal Longitude { get; set; }

        internal long IpUniqueID { get; set; }

        internal bool IsComplete { get; set; }

        #endregion Properties

        #region Public Methods

        public int CompareTo(object obj)
        {
            return (IpStart.CompareTo(((IpCity)obj).IpStart));
        }

        #endregion Public Methods
    }
}
