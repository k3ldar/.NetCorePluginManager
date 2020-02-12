using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using PluginManager;
using PluginManager.Abstractions;
using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Search
{
    internal class TestSearchPluginManager : BasePluginManager
    {
        private List<IInitialiseEvents> _initializablePlugins;

        internal TestSearchPluginManager()
            : base(new PluginManagerConfiguration(), new PluginSettings())
        {

        }

        internal TestSearchPluginManager(ILogger logger)
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

        protected override void ServiceConfigurationComplete(in IServiceProvider serviceProvider)
        {

        }
    }
}
