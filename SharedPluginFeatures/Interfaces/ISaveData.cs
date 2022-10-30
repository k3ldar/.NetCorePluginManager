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
 *  Product:  SharedPluginFeatures
 *  
 *  File: ISaveData.cs
 *
 *  Purpose:  Interface for saving data
 *
 *  Date        Name                Reason
 *  18/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
namespace SharedPluginFeatures
{
    /// <summary>
    /// Interface used for persisting data to storage, the type of storage used will depend on the implementation
    /// </summary>
    public interface ISaveData
    {
        /// <summary>
        /// Saves data
        /// </summary>
        /// <typeparam name="T">Type of data to be saved</typeparam>
        /// <param name="data">Data to be saved</param>
        /// <param name="location">Storage location</param>
        /// <param name="name">Name of storage</param>
        /// <returns>bool</returns>
        bool Save<T>(T data, in string location, in string name);
    }
}
