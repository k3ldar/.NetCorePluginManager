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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  SharedPluginFeatures
 *  
 *  File: BaseModelData.cs
 *
 *  Purpose:  Contains basic data to be loaded into BaseModel for general use.
 *
 *  Date        Name                Reason
 *  08/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Collections.Generic;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Class used to generate chart data which is displayed within System Admin
    /// </summary>
    public sealed class ChartModel
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ChartModel()
        {
            DataNames = new List<KeyValuePair<ChartDataType, string>>();
            DataValues = new Dictionary<string, List<decimal>>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Title of the chart
        /// </summary>
        public string ChartTitle { get; set; }

        /// <summary>
        /// Names of the data types being displayed
        /// </summary>
        public List<KeyValuePair<ChartDataType, string>> DataNames { get; set; }

        /// <summary>
        /// Dictionary of data values
        /// </summary>
        public Dictionary<string, List<decimal>> DataValues { get; set; }

        #endregion Properties
    }
}
