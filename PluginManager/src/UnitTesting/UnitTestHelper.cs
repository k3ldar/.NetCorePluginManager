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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: UnitTestHelper.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
#if DEBUG
using System;

using PluginManager.Abstractions;
using PluginManager.Internal;

namespace PluginManager.Test
{
    /// <summary>
    /// This class is only available for Debug builds and is designed to retrieve classes
    /// that are normally only available through DI
    /// </summary>
    public static class UnitTestHelper
    {
        #region Private Members

        private static BasePluginManager _pluginManager;

        #endregion Private Members

        #region Constructors

        static UnitTestHelper()
        {
            PluginManagerConfiguration configuration = new PluginManagerConfiguration();
            //PluginSettings settings = null;
        }

        #endregion Constructors

        public static void Initialise(BasePluginManager pluginManager)
        {
            _pluginManager = pluginManager ?? throw new ArgumentNullException(nameof(pluginManager));
        }

        /// <summary>
        /// Returns an instance of IPluginHelperService for unit tests
        /// </summary>
        /// <returns>IPluginHelperService</returns>
        public static IPluginHelperService GetPluginServices()
        {
            return new PluginServices(_pluginManager) as IPluginHelperService;
        }

        /// <summary>
        /// Returns an instance of IPluginClassesService for unit tests
        /// </summary>
        /// <returns>IPluginClassesService</returns>
        public static IPluginClassesService GetPluginClassesService()
        {
            return new PluginServices(_pluginManager) as IPluginClassesService;
        }

        /// <summary>
        /// Returns an instance of IPluginTypesService for unit tests
        /// </summary>
        /// <returns>IPluginTypesService</returns>
        public static IPluginTypesService GetPluginTypesService()
        {
            return new PluginServices(_pluginManager) as IPluginTypesService;
        }

        /// <summary>
        /// Returns an instance of INotificationService for unit tests
        /// </summary>
        /// <returns>INotificationService</returns>
        public static INotificationService GetNotificationService()
        {
            return new NotificationService() as INotificationService;
        }

    }
}

#endif
