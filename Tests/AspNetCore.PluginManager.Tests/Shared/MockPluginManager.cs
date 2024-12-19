/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockPluginManager.cs
 *
 *  Purpose:  Mock Plugin Manager class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PluginManager;
using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockPluginManager : BasePluginManager
    {
        private List<IInitialiseEvents> _initializablePlugins;

        internal MockPluginManager()
            : base(new PluginManagerConfiguration(), new PluginSettings())
        {

        }

        internal MockPluginManager(ILogger logger)
            : base(new PluginManagerConfiguration(logger), new PluginSettings())
        {

        }

        internal IServiceProvider GetServiceProvider()
        {
            return base.ServiceProvider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Used in exception, not required from resource.")]
        public void UsePlugin(Type iPluginType)
        {
            if (iPluginType == null)
                throw new ArgumentNullException(nameof(iPluginType));

            if (iPluginType.GetInterface(typeof(IPlugin).Name) != null)
            {
                PluginLoad(iPluginType.Assembly.Location, false);
            }
            else
            {
                throw new InvalidOperationException($"Type {nameof(iPluginType)} must implement {nameof(IPlugin)}");
            }
        }

        internal T GetRequiredService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        internal string Path()
        {
            return RootPath;
        }

        protected override bool CanExtractResource(in string resourceName)
        {
            return false;
        }

        protected override void ModifyPluginResourceName(ref string resourceName)
        {

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

        protected override void ServiceConfigurationComplete(in IServiceCollection serviceCollection)
        {

        }

    }
}
