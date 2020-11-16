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
 *  File: ILoadData.cs
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
    /// Interface used for reading data from storage, the type of storage used will depend on the implementation
    /// </summary>
    public interface ILoadData
    {
        /// <summary>
        /// Loads data of type T
        /// </summary>
        /// <typeparam name="T">Type of data to be loaded</typeparam>
        /// <param name="location">Storage location</param>
        /// <param name="name">Name of storage</param>
        /// <returns>Type of data to be loaded or default type if exception occurs</returns>
        T Load<T>(in string location, in string name);
    }
}
