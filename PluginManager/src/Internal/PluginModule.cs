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
 *  File: PluginModule.cs
 *
 *  Purpose:  Internal implementation of IPluginModule
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Reflection;

using PluginManager.Abstractions;

namespace PluginManager.Internal
{
    internal sealed class PluginModule : IPluginModule, IPluginVersion
    {
        #region Constructors

        public PluginModule(Assembly assembly, string module, IPlugin pluginService)
        {
            if (String.IsNullOrEmpty(module))
                throw new ArgumentException(nameof(module));

            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            Module = module;
            Plugin = pluginService ?? throw new ArgumentNullException(nameof(pluginService));
        }

        #endregion Constructors

        #region IPluginModule Properties

        public ushort Version { get; set; }

        public string Module { get; private set; }

        public Assembly Assembly { get; private set; }

        public IPlugin Plugin { get; private set; }

        public string FileVersion { get; internal set; }

        #endregion IPluginModule Properties

        #region IPluginVersion Methods

        public ushort GetVersion()
        {
            return Version;
        }

        #endregion IPluginVersion Methods
    }
}
