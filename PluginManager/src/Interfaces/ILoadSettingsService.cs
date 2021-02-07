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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: ILoadSettingsService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

namespace PluginManager.Abstractions
{
    /// <summary>
    /// This interface is used when initially loading the AspNetCore.PluginManager to load 
    /// settings it requires.  A custom implementation can be supplied during initialisation
    /// of the Plugin Manager which can enable the settings to be loaded from a custom data
    /// source.
    /// 
    /// This interface will be used to load PluginSetting and PluginSettings but may be
    /// extended in future versions to include other initialisation data.
    /// </summary>
    public interface ILoadSettingsService
    {
        /// <summary>
        /// Requests that setting data be loaded for T, this could be PluginSetting or PluginSettings.
        /// </summary>
        /// <typeparam name="T">Class type</typeparam>
        /// <param name="jsonFile">jsonFile to use, this can be altered for custom implementations.</param>
        /// <param name="name">Name of settings to be loaded.</param>
        /// <returns>Instance of T</returns>
        T LoadSettings<T>(in string jsonFile, in string name);

        /// <summary>
        /// Requests that setting data be loaded for T, this could be PluginSetting or PluginSettings, the default jsonFile name is used (appsettings.json).
        /// </summary>
        /// <typeparam name="T">Class type</typeparam>
        /// <param name="name">Name of settings to be loaded.</param>
        /// <returns>Instance of T</returns>
        T LoadSettings<T>(in string name);
    }
}
