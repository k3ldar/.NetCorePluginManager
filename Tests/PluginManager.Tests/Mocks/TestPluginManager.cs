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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: TestPluginManager.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    internal class TestPluginManager : BasePluginManager
    {
        internal TestPluginManager()
            : base(new PluginManagerConfiguration(), new PluginSettings())
        {

        }

        internal TestPluginManager(ILogger logger)
            : base(new PluginManagerConfiguration(logger), new PluginSettings())
        {

        }

        internal TestPluginManager(in PluginManagerConfiguration configuration, in PluginSettings pluginSettings)
            : base(configuration, pluginSettings)
        {

        }

        internal IServiceProvider GetServiceProvider()
        {
            return base.ServiceProvider;
        }

        internal string Path()
        {
            return RootPath;
        }

        internal IServiceConfigurator GetServiceConfigurator()
        {

            return ServiceConfigurator;
        }

        internal void TestSetServiceConfigurator(IServiceConfigurator serviceConfigurator)
        {
            SetServiceConfigurator(serviceConfigurator);
        }

        protected override bool CanExtractResource(in string resourceName)
        {
            return TestCanExtractResources;
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

        }

        protected override void PreConfigurePluginServices(in IServiceCollection serviceProvider)
        {

        }

        protected override void ServiceConfigurationComplete(in IServiceProvider serviceProvider)
        {

        }

        internal void RegisterServiceConfigurator(MockServiceConfigurator serviceConfigurator)
        {
            base.SetServiceConfigurator(serviceConfigurator as IServiceConfigurator);
        }

        internal bool TestCanExtractResources { get; set; }
    }
}
