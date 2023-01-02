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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: IPluginTypesService.cs
 *
 *  Purpose:  Provides interface for retrieving plugin specific data from the plugin manager
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace PluginManager.Abstractions
{
    /// <summary>
    /// Allows plugin modules to retrieve a list of class Types that implement a specific attribute
    /// either at a class or method level.
    /// 
    /// None of the Types returned are instantiated instances.
    /// 
    /// This can only return Types that have been made available from within a plugin module that
    /// has been registered with AspNetCore.PluginManager.
    /// </summary>
    public interface IPluginTypesService
    {
        /// <summary>
        /// Return a list of all class types that implement a specific attribute T at the class
        /// or method level.
        /// </summary>
        /// <typeparam name="T">Attribute that is being sought.</typeparam>
        /// <returns>List&lt;Type&gt;</returns>
        List<Type> GetPluginTypesWithAttribute<T>();
    }
}
