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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PluginManager.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  28/04/2019  Simon Carter        #63 Allow plugin to be dynamically added.
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using PluginManager;
using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable IDE0034

namespace AspNetCore.PluginManager
{
    internal sealed class NetCorePluginManager : BasePluginManager
    {
        #region Private Members

        private List<IInitialiseEvents> _initializablePlugins;

        #endregion Private Members

        #region Constructors

        internal NetCorePluginManager(in PluginManagerConfiguration configuration, in NetCorePluginSettings pluginSettings)
            : base(configuration, pluginSettings)
        {

        }

        #endregion Constructors

        #region Overridden Methods

        protected override bool CanExtractResource(in string resourceName)
        {
            return true;
        }

        protected override void ModifyPluginResourceName(ref string resourceName)
        {
            // special case for minified js files which have naming convention of library.min.js
            if (resourceName.EndsWith("\\min.js"))
            {
                resourceName = resourceName.Replace("\\min.js", ".min.js");
            }

            // special case for minified css files which have naming convention of library.min.css
            if (resourceName.EndsWith("\\min.css"))
            {
                resourceName = resourceName.Replace("\\min.css", ".min.cs");
            }
        }

        protected override void PluginConfigured(in IPluginModule pluginModule)
        {

        }

        protected override void PluginInitialised(in IPluginModule pluginModule)
        {

        }

        protected override void PluginLoaded(in Assembly pluginFile)
        {

        }

        protected override void PluginLoading(in Assembly pluginFile)
        {

        }

        protected override void PostConfigurePluginServices(in IServiceCollection serviceProvider)
        {
            foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
                initialiseEvents.AfterConfigureServices(serviceProvider);
        }

        protected override void PreConfigurePluginServices(in IServiceCollection serviceProvider)
        {
            _initializablePlugins = PluginGetClasses<IInitialiseEvents>();

            foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
                initialiseEvents.BeforeConfigureServices(serviceProvider);
        }

        #endregion Overridden Methods

        #region Internal Methods

        /// <summary>
        /// Allows plugins to configure with the current App Builder
        /// </summary>
        /// <param name="app"></param>
        internal void Configure(in IApplicationBuilder app)
        {
            foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
                initialiseEvents.BeforeConfigure(app);

            foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
                initialiseEvents.Configure(app);

            foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
                initialiseEvents.AfterConfigure(app);
        }

        #endregion Internal Methods
    }
}
