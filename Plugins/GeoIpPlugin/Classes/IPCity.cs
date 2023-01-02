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
    /// <summary>
    /// IpCity data.  Used internally to cache data retrieved.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1036:Override methods on comparable types", Justification = "Compare method already implemented and that is all that's needed for this class.")]
    public class IpCity : IComparable
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startRange">Start of Ip range.</param>
        /// <param name="endRange">End of Ip range.</param>
        /// <param name="countryCode">Country code.</param>
        public IpCity(long startRange, long endRange, string countryCode)
        {
            IsComplete = false;
            IpStart = startRange;
            IpEnd = endRange;
            CountryCode = countryCode;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Start of Ip range.
        /// </summary>
        /// <value>long</value>
        public long IpStart { get; set; }

        /// <summary>
        /// End of Ip range.
        /// </summary>
        /// <value>long</value>
        public long IpEnd { get; set; }

        /// <summary>
        /// Country code for Ip Address.
        /// </summary>
        /// <value>string</value>
        public string CountryCode { get; set; }

        /// <summary>
        /// Region where Ip Address is located.
        /// </summary>
        /// <value>string</value>
        public string Region { get; set; }

        /// <summary>
        /// Name of city where Ip Address is located.
        /// </summary>
        /// <value>string</value>
        public string CityName { get; set; }

        /// <summary>
        /// Latitude of Ip Address.
        /// </summary>
        /// <value>decimal</value>
        public decimal Latitude { get; set; }

        /// <summary>
        /// Longitude of Ip Address.
        /// </summary>
        /// <value>decimal</value>
        public decimal Longitude { get; set; }

        /// <summary>
        /// Unique Id for Ip addressl
        /// </summary>
        /// <value>long</value>
        internal long IpUniqueID { get; set; }

        /// <summary>
        /// Determines whether the record is complete or not.
        /// </summary>
        /// <value>bool</value>
        public bool IsComplete { get; set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Compare Method for comparing existing Ip addresses cached in memory.
        /// </summary>
        /// <value>int</value>
        public int CompareTo(object obj)
        {
            if (obj == null)
                return 0;

            return IpStart.CompareTo(((IpCity)obj).IpStart);
        }

        #endregion Public Methods
    }
}
