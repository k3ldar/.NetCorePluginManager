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
 *  File: PluginServices.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *  28/04/2019  Simon Carter        #63 Allow plugin to be dynamically added.
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Reflection;

using PluginManager.Abstractions;

namespace PluginManager.Internal
{
    internal sealed class PluginServices : IPluginClassesService, IPluginHelperService, IPluginTypesService
    {
        #region Private Members

        private readonly BasePluginManager _pluginManager;

        #endregion Private Members

        #region Constructors

        internal PluginServices(in BasePluginManager pluginManager)
        {
            _pluginManager = pluginManager ?? throw new ArgumentNullException(nameof(pluginManager));
        }

        #endregion Constructors

        #region IPluginClassesService Methods

        public List<Type> GetPluginClassTypes<T>()
        {
            return _pluginManager.PluginGetClassTypes<T>();
        }

        public List<T> GetPluginClasses<T>()
        {
            return _pluginManager.PluginGetClasses<T>();
        }

        #endregion IPluginClassesService Methods

        #region IPluginTypesService Methods

        public List<Type> GetPluginTypesWithAttribute<T>()
        {
            return _pluginManager.PluginGetTypesWithAttribute<T>();
        }

        #endregion IPluginTypesService Methods

        #region IPluginHelperService Methods

        public bool PluginLoaded(in string pluginLibraryName, out int version)
        {
            return _pluginManager.PluginLoaded(pluginLibraryName, out version, out _);
        }

        public DynamicLoadResult AddAssembly(in Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            return _pluginManager.AddAssembly(assembly);
        }

        #endregion IPluginHelperService Methods
    }
}
