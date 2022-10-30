/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  Plugin Manager is distributed under the GNU General Public License version 3 and  
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
 *  Product:  PluginManager
 *  
 *  File: ISettingsProvider.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  04/12/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */


namespace PluginManager.Abstractions
{
    /// <summary>
    /// This interface can be used by all plugin modules to load setting and configuration data.
    /// 
    /// The default implementation which is loaded if no other plugin registers an instance uses 
    /// appsettings.json to store configuration data to be used by Plugins.
    /// 
    /// An instance of this interface is available via the DI container, any custom implementations
    /// must be configured to be used in the DI contaner when being initialised.
    /// </summary>
    /// <remarks>
    /// This class can be customised by the host application, if no implementation is provided then
    /// a default implementation is provided.
    /// </remarks>
    public interface ISettingsProvider
    {
        /// <summary>
        /// Retrieves settings for Class T
        /// </summary>
        /// <typeparam name="T">Class who's settings are being requested.</typeparam>
        /// <param name="storage">Name of storage to be used.</param>
        /// <param name="sectionName">Name of configuration data required.</param>
        /// <returns>Instance of type T initialised with the required settings.</returns>
        T GetSettings<T>(in string storage, in string sectionName);

        /// <summary>
        /// Retrieves settings for Class T
        /// </summary>
        /// <typeparam name="T">Class who's settings are being requested.</typeparam>
        /// <param name="sectionName">Name of configuration data required.</param>
        /// <returns>Instance of type T initialised with the required settings.</returns>
        T GetSettings<T>(in string sectionName);
    }
}
