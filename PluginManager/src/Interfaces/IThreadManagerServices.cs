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
 *  File: IThreadManagerServices.cs
 *
 *  Purpose:  Provides interface for managing threads descending from ThreadManager
 *
 *  Date        Name                Reason
 *  26/07/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace PluginManager.Abstractions
{
    /// <summary>
    /// Interface managed by plugin manager which allows plugins to register threads for starting
    /// after the plugin application has been initialised.
    /// 
    /// Registered threads must descend from ThreadManager class.
    /// 
    /// DI is used to obtain any required classes needed to instantiate the type
    /// </summary>
    public interface IThreadManagerServices
    {
        /// <summary>
        /// Registers a ThreadManager class that will be instantiated and run once the plugin application manager
        /// has finished loading.
        /// </summary>
        /// <param name="threadName">Name of thread to be created.  This must be a unique name</param>
        /// <param name="type">Type of class, this must descend from ThreadManager</param>
        void RegisterStartupThread(string threadName, Type type);
    }
}
