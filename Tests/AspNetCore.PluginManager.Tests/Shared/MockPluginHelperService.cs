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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockPluginHelperService.cs
 *
 *  Purpose:  Mock IPluginHelperService class
 *
 *  Date        Name                Reason
 *  02/12/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using PluginManager;
using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests.Shared
{
	[ExcludeFromCodeCoverage]
	public class MockPluginHelperService : IPluginHelperService
    {
        private readonly List<string> _loadedPlugins;

        public MockPluginHelperService(List<string> loadedPlugins)
        {
            _loadedPlugins = loadedPlugins ?? throw new ArgumentNullException(nameof(loadedPlugins));
        }

        public DynamicLoadResult AddAssembly(in Assembly assembly)
        {
            throw new NotImplementedException();
        }

        public bool PluginLoaded(in string pluginLibraryName, out int version)
        {
            version = 1;
            return _loadedPlugins.Contains(pluginLibraryName);
        }
    }
}
