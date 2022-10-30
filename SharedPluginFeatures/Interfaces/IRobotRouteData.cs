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
 *  Product:  SharedPluginFeatues
 *  
 *  File: IRobotRouteData.cs
 *
 *  Purpose:  IRobotRouteData interface for creating robot route data
 *
 *  Date        Name                Reason
 *  03/11/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


namespace SharedPluginFeatures
{
    /// <summary>
    /// Interface for custom robot route data
    /// </summary>
    public interface IRobotRouteData
    {
        /// <summary>
        /// Agent name
        /// </summary>
        /// <value>string</value>
        public string Agent { get; set; }

        /// <summary>
        /// Custom comment applied to the user defined route data
        /// </summary>
        /// <value>string</value>
        public string Comment { get; set; }

        /// <summary>
        /// User defined route
        /// </summary>
        /// <value>string</value>
        public string Route { get; set; }

        /// <summary>
        /// Whether it is an allowed or denied route
        /// </summary>
        /// <value>bool</value>
        public bool Allowed { get; set; }

        /// <summary>
        /// Determines whether route is a custom user defined route, or one discovered via attributes
        /// </summary>
        public bool IsCustom { get; set; }
    }
}
