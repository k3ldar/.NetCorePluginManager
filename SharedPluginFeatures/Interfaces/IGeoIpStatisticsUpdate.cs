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
 *  File: IGeoIpStatistics.cs
 *
 *  Purpose:  Provides interface for updating geo ip data
 *
 *  Date        Name                Reason
 *  05/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
    /// <summary>
    /// This interface is implemented internally by the GeoIpPlugin module and is not available 
    /// via DI or any other method.  Instead it is used to provide information on hown many GeoIp
    /// records were loaded and how long it took.
    /// 
    /// See IGeoIpStatistics for information on retrieving statistics.
    /// 
    /// This method is deprecated and should not be used by new modules as it will be removed in future versions.
    /// </summary>
    [Obsolete("This interface is obsolete and wil be removed from future versions.  Use INotificationService instead.")]
    public interface IGeoIpStatisticsUpdate
    {
        #region Methods

        /// <summary>
        /// Indicates that retrieval is finished
        /// </summary>
        /// <param name="milliseconds">Total number of milliseconds taken to load the data.</param>
        /// <param name="recordCount">Total number of records loaded.</param>
        void Retrieve(in long milliseconds, in uint recordCount);

        #endregion Methods
    }
}
