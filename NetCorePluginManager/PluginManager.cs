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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PluginManager.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using SharedPluginFeatures;
using static SharedPluginFeatures.Enums;

#pragma warning disable IDE0034

namespace AspNetCore.PluginManager
{
    public sealed class PluginManager : IDisposable
    {
        #region Private Members

        private const ushort MaxPluginVersion = 1;

        private readonly ILogger _logger;
        private readonly Dictionary<string, IPluginModule> _plugins;
        private readonly PluginSettings _pluginSettings;

        private static IServiceProvider _serviceProvider;

        #endregion Private Members

        #region Constructors

        private PluginManager ()
        {
            _plugins = new Dictionary<string, IPluginModule>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
        }

        internal PluginManager(in ILogger logger, in PluginSettings pluginSettings)
            : this()
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _pluginSettings = pluginSettings ?? throw new ArgumentNullException(nameof(pluginSettings));

            if (_pluginSettings.Plugins == null)
                _pluginSettings.Plugins = new List<PluginSetting>();
        }

        #endregion Constructors

        #region Internal Methods

        /// <summary>
        /// Returns all loaded plugin data
        /// </summary>
        /// <returns></returns>
        internal Dictionary<string, IPluginModule> GetLoadedPlugins()
        {
            return (_plugins);
        }

        /// <summary>
        /// Loads and configures an individual plugin
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="extractResources"></param>
        internal void LoadPlugin(in Assembly assembly, in string fileLocation, in bool extractResources)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            string assemblyName = Path.GetFileName(assembly.ManifestModule.ScopeName);

            if (_plugins.ContainsKey(assemblyName))
                return;

            if (String.IsNullOrEmpty(assemblyName))
            {
                assemblyName = assembly.ManifestModule.ScopeName; 
            }

            PluginSetting pluginSetting = GetPluginSetting(assemblyName);

            if (pluginSetting.Disabled)
            {
                _logger.AddToLog(LogLevel.Warning, "PluginManager is disabled");
                return;
            }

            bool interfaceFound = false;

            foreach (Type type in assembly.GetTypes())
            {
                try
                {
                    if (type.GetInterface("IPlugin") != null)
                    {
                        interfaceFound = true;
                        IPlugin pluginService = (IPlugin)Activator.CreateInstance(type);

                        if (extractResources && !pluginSetting.PreventExtractResources)
                        {
                            ExtractResources(assembly, pluginSetting);
                        }

                        IPluginModule pluginModule = new IPluginModule()
                        {
                            Assembly = assembly,
                            Module = assemblyName,
                            Plugin = pluginService
                        };

                        IPluginVersion version = GetPluginClass<IPluginVersion>(pluginModule);

                        pluginModule.Version = version == null ? (ushort)1 :
                            GetMinMaxValue(version.GetVersion(), 1, MaxPluginVersion);

                        try
                        {
                            string file = Path.GetFullPath(
                                String.IsNullOrEmpty(assembly.Location) ? fileLocation : assembly.Location);

                            if (File.Exists(file))
                                pluginModule.FileVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(file).FileVersion;
                            else
                                pluginModule.FileVersion = "unknown";
                        }
                        catch (Exception err)
                        {
                            _logger.AddToLog(LogLevel.PluginLoadError, err, $"Failed to get version information {assembly.FullName}");
                        }

                        pluginModule.Plugin.Initialise(_logger);

                        _plugins.Add(assemblyName, pluginModule);

                        _logger.AddToLog(LogLevel.PluginLoadSuccess, assemblyName);


                        // only interested in first reference of IPlugin
                        break;
                    }
                }
                catch (Exception typeLoader)
                {
                    _logger.AddToLog(LogLevel.PluginLoadError, typeLoader, 
                        $"{assembly.FullName}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            if (!interfaceFound)
                _logger.AddToLog(LogLevel.PluginConfigureError, $"{assemblyName} contains no IPlugin Interface");
        }

        /// <summary>
        /// Loads and configures an individual plugin
        /// </summary>
        /// <param name="pluginName"></param>
        internal void LoadPlugin(in string pluginName)
        {
            try
            {

                PluginSetting setting = GetPluginSetting(pluginName);

                if (setting != null && !setting.Disabled)
                    LoadPlugin(LoadAssembly(pluginName), pluginName, true);
            }
            catch (Exception error)
            {
                _logger.AddToLog(LogLevel.PluginLoadError, error, $"{pluginName}{MethodBase.GetCurrentMethod().Name}");
            }
        }

        /// <summary>
        /// Allows plugins to configure with the current 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        internal void Configure(in IApplicationBuilder app, in IHostingEnvironment env)
        {
            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                try
                {
                    plugin.Value.Plugin.Configure(app, env);
                }
                catch (Exception error)
                {
                    _logger.AddToLog(LogLevel.PluginConfigureError, error, $"{plugin.Key}{MethodBase.GetCurrentMethod().Name}");
                }
            }
        }

        /// <summary>
        /// Allows plugins to configure the services for all plugins
        /// </summary>
        /// <param name="services"></param>
        internal void ConfigureServices(IServiceCollection services)
        {
            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                try
                {
                    plugin.Value.Plugin.ConfigureServices(services);
                    services.AddMvc().AddApplicationPart(plugin.Value.Assembly);
                }
                catch (Exception error)
                {
                    _logger.AddToLog(LogLevel.PluginConfigureError, error, $"{plugin.Key}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            // if no plugin has registered a setting provider, add the default appsettings json provider
            services.TryAddSingleton<ISettingsProvider, Classes.DefaultSettingProvider>();

            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Retrieves the non instantiated classes which have attribute T, or if any of
        /// the methods or properties have attribute T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal List<Type> GetPluginTypesWithAttribute<T>()
        {
            List<Type> Result = new List<Type>();

            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                try
                {
                    foreach (Type type in plugin.Value.Assembly.GetTypes())
                    {
                        try
                        {
                            if (type.GetCustomAttributes().Where(t => t.GetType() == typeof(T)).FirstOrDefault() != null)
                            {
                                Result.Add(type);
                                continue;
                            }

                            if (type.IsClass || type.IsInterface)
                            {
                                // cycle through all properties and methods to see if they have the attibute
                                foreach (MethodInfo method in type.GetMethods())
                                {
                                    if (method.GetCustomAttributes().Where(t => t.GetType() == typeof(T)).FirstOrDefault() != null)
                                    {
                                        Result.Add(type);
                                        break;
                                    }
                                }

                                foreach (PropertyInfo property in type.GetProperties())
                                {
                                    if (property.GetCustomAttributes().Where(t => t.GetType() == typeof(T)).FirstOrDefault() != null)
                                    {
                                        Result.Add(type);
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception typeLoader)
                        {
                            _logger.AddToLog(LogLevel.Error, typeLoader, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                        }
                    }
                }
                catch (Exception error)
                {
                    _logger.AddToLog(LogLevel.Error, error, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            return (Result);
        }

        /// <summary>
        /// Retrieves the non instantiated classes which inherit from T or implement 
        /// interface T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal List<Type> GetPluginClassTypes<T>()
        {
            List<Type> Result = new List<Type>();

            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                try
                {
                    foreach (Type type in plugin.Value.Assembly.GetTypes())
                    {
                        try
                        {
                            if ((type.GetInterface(typeof(T).Name) != null) || (type.IsSubclassOf(typeof(T))))
                            {
                                Result.Add(type);
                            }
                        }
                        catch (Exception typeLoader)
                        {
                            _logger.AddToLog(LogLevel.Error, typeLoader, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                        }
                    }
                }
                catch (Exception error)
                {
                    _logger.AddToLog(LogLevel.Error, error, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            return (Result);
        }

        /// <summary>
        /// Retreives an instantiated specific type of class which inherits from a specific class 
        /// or interface from within the plugin modules
        /// </summary>
        /// <typeparam name="T">Type of interface/class</typeparam>
        /// <returns></returns>
        internal List<T> GetPluginClasses<T>()
        {
            List<T> Result = new List<T>(); 

            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                try
                {
                    foreach (Type type in plugin.Value.Assembly.GetTypes())
                    {
                        try
                        {

                            if ((type.GetInterface(typeof(T).Name) != null) || (type.IsSubclassOf(typeof(T))))
                            {
                                // the only other supported constructor is one that supports only ISettingsProvider
                                // does this type have one of those constructors
                                ConstructorInfo settingsProviderConstructor = type.GetConstructors()
                                    .Where(c => c.IsPublic && !c.IsStatic && c.GetParameters().Length == 1)
                                    .FirstOrDefault();

                                if (settingsProviderConstructor != null)
                                {
                                    if (settingsProviderConstructor.GetParameters()[0].ParameterType == typeof(ISettingsProvider) ||
                                        settingsProviderConstructor.GetParameters()[0].ParameterType.Name.StartsWith(typeof(ISettingsProvider).Name))
                                    {
                                        Result.Add((T)Activator.CreateInstance(type,
                                            _serviceProvider.GetRequiredService(typeof(ISettingsProvider))));
                                    }
                                }
                                else
                                { 
                                    // does the type have a parameterless constructor?  If not then
                                    // an exception will be raised and logged
                                    Result.Add((T)Activator.CreateInstance(type));
                                }
                            }
                        }
                        catch (Exception typeLoader)
                        {
                            _logger.AddToLog(LogLevel.Error, typeLoader, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                        }
                    }
                }
                catch (Exception error)
                {
                    _logger.AddToLog(LogLevel.Error, error, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            return (Result);
        }

        /// <summary>
        /// Determines whether a plugin is loaded, and retrieves the version
        /// </summary>
        /// <param name="pluginLibraryName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        internal bool PluginLoaded(in string pluginLibraryName, out int version, out string module)
        {
            version = -1;
            module = pluginLibraryName;

            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                if (plugin.Value.Module.EndsWith(pluginLibraryName, StringComparison.CurrentCultureIgnoreCase))
                {
                    version = plugin.Value.Version;
                    module = plugin.Value.Assembly.Location;

                    return (true);
                }
            }

            return (false);
        }

        #endregion Internal Methods

        #region IDisposable Methods

        /// <summary>
        /// Disposable method, notify all plugins to finalise
        /// </summary>
        public void Dispose()
        {
            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                try
                {
                    plugin.Value.Plugin.Finalise();
                }
                catch (Exception error)
                {
                    _logger.AddToLog(LogLevel.Error, error, $"{plugin.Key}{MethodBase.GetCurrentMethod().Name}");
                }
            }
        }

        #endregion IDisposable Methods

        #region Private Methods

        /// <summary>
        /// Checks a value, to ensure it is between min/max Value
        /// </summary>
        /// <param name="value">Value to check</param>
        /// <param name="minValue">Min value allowed</param>
        /// <param name="maxValue">Max value allowed</param>
        /// <returns></returns>
        private ushort GetMinMaxValue(in ushort value, in ushort minValue, in ushort maxValue)
        {
            if (value < minValue)
                return (minValue);
            else if (value > maxValue)
                return (maxValue);

            return (value);
        }

        /// <summary>
        /// Returns the first class/interface of type T within the assembly
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="pluginModule">plugin module</param>
        /// <returns>instantiated instance of Type T if found, otherwise null</returns>
        private T GetPluginClass<T>(in IPluginModule pluginModule)
        {
            try
            {
                foreach (Type type in pluginModule.Assembly.GetTypes())
                {
                    try
                    {
                        if ((type.GetInterface(typeof(T).Name) != null) || (type.IsSubclassOf(typeof(T))))
                        {
                            return ((T)Activator.CreateInstance(type));
                        }
                    }
                    catch (Exception typeLoader)
                    {
                        _logger.AddToLog(LogLevel.Error, typeLoader, $"{pluginModule.Module}{MethodBase.GetCurrentMethod().Name}");
                    }
                }
            }
            catch (Exception error)
            {
                _logger.AddToLog(LogLevel.Error, error, $"{pluginModule.Module}{MethodBase.GetCurrentMethod().Name}");
            }

            return (default(T));
        }

        /// <summary>
        /// Retrieves the file path of the host website
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="pluginSetting"></param>
        /// <returns></returns>
        private string GetLiveFilePath(in string assemblyName, in string resourceName, in PluginSetting pluginSetting)
        {
            // remove the first part of the name which is the library
            string Result = resourceName.Replace(assemblyName, String.Empty);

            int lastIndex = Result.LastIndexOf('.');

            if (lastIndex > 0)
            {
                Result = Result.Substring(0, lastIndex).Replace(".", "\\") + Result.Substring(lastIndex);
            }

            return (Path.Combine(PluginManagerService.RootPath(), Result));
        }

        /// <summary>
        /// Extract Views/CSS/JS files from resources
        /// </summary>
        /// <param name="pluginAssembly"></param>
        /// <param name="pluginSetting"></param>
        private void ExtractResources(in Assembly pluginAssembly, in PluginSetting pluginSetting)
        {
            string assemblyName = pluginAssembly.FullName.Split(',')[0] + ".";

            foreach (string resource in pluginAssembly.GetManifestResourceNames())
            {
                if (String.IsNullOrEmpty(resource))
                    continue;

                using (Stream stream = pluginAssembly.GetManifestResourceStream(resource))
                {
                    string resourceFileName = GetLiveFilePath(assemblyName, resource, pluginSetting);

                    if (File.Exists(resourceFileName) && !pluginSetting.ReplaceExistingResources)
                        continue;

                    string directory = Path.GetDirectoryName(resourceFileName);

                    if (!Directory.Exists(directory))
                        Directory.CreateDirectory(directory);

                    if (File.Exists(resourceFileName))
                        File.Delete(resourceFileName);

                    using (Stream fileStream = File.OpenWrite(resourceFileName))
                    {
                        byte[] buffer = new byte[stream.Length];

                        stream.Read(buffer, 0, buffer.Length);
                        fileStream.Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Dynamically loads an assembly
        /// </summary>
        /// <param name="assemblyName">name of assembly</param>
        /// <returns>Assembly instance</returns>
        private Assembly LoadAssembly(in string assemblyName)
        {
            if (String.IsNullOrEmpty(assemblyName))
                throw new ArgumentException(nameof(assemblyName));

            if (!File.Exists(assemblyName))
                throw new ArgumentException(nameof(assemblyName));

            string assembly = assemblyName;

            if (!Path.IsPathRooted(assembly))
                assembly = Path.GetFullPath(assembly);

            return (Assembly.LoadFrom(assembly));
        }

        /// <summary>
        /// If associated/required dll's are not found, and settings are configured, 
        /// attempt to load them from the configured path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (String.IsNullOrWhiteSpace(_pluginSettings.SystemFiles) || 
                !Directory.Exists(_pluginSettings.SystemFiles))
            {
                return (null);
            }

            AssemblyName assyName = new AssemblyName(args.Name);
            string filename = args.Name.ToLower().Split(',')[0];
            string assembly = Path.Combine(_pluginSettings.SystemFiles, filename);

            if (!assembly.EndsWith(".dll"))
                assembly += ".dll";

            try
            {
                if (File.Exists(assembly))
                    return (Assembly.LoadFrom(assembly));
            }
            catch (Exception error)
            {
                _logger.AddToLog(LogLevel.PluginLoadError, error, $"{MethodBase.GetCurrentMethod().Name}");
            }

            return (null);
        }

        /// <summary>
        /// Retrieve plugin settings for an individual plugin module
        /// </summary>
        /// <param name="pluginName">Name of plugin</param>
        /// <returns></returns>
        private PluginSetting GetPluginSetting(in string pluginName)
        {
            if (_pluginSettings == null || _pluginSettings.PluginFiles == null)
                return (new PluginSetting(pluginName));

            string name = Path.GetFileName(pluginName);

            foreach (PluginSetting setting in _pluginSettings.Plugins)
            {
                if (setting.Name.EndsWith(name, StringComparison.CurrentCultureIgnoreCase))
                    return (setting);
            }

            return (new PluginSetting(pluginName));
        }

        #endregion Private Methods
    }
}
