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
 *  File: PluginManager.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  28/04/2019  Simon Carter        #63 Allow plugin to be dynamically added.
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using PluginManager.Abstractions;
using PluginManager.Internal;

namespace PluginManager
{
    /// <summary>
    /// Base plugin manager, contains methods and properties to load and interact
    /// with plugins within an application
    /// </summary>
    public abstract class BasePluginManager : IDisposable
    {
        #region Private Members

        private const ushort MaxPluginVersion = 1;

        private readonly Dictionary<string, IPluginModule> _plugins;
        private readonly PluginSettings _pluginSettings;
        private readonly PluginManagerConfiguration _configuration;
        private bool _disposed;
        private static IServiceProvider _serviceProvider;
        private bool _serviceConfigurationComplete;

        #endregion Private Members

        #region Constructors / Destructors

        /// <summary>
        /// Private constructor, used internally by the BasePluginManager to initialise the class internals
        /// </summary>
        private BasePluginManager()
        {
            _plugins = new Dictionary<string, IPluginModule>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
            _disposed = false;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="configuration">Plugin configuration</param>
        /// <param name="pluginSettings">Plugin Settings</param>
        public BasePluginManager(in PluginManagerConfiguration configuration, in PluginSettings pluginSettings)
            : this()
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _pluginSettings = pluginSettings ?? throw new ArgumentNullException(nameof(pluginSettings));

            if (_configuration.ServiceConfigurator != null)
                SetServiceConfigurator(_configuration.ServiceConfigurator);

            Logger = configuration.Logger;

            ThreadManagerInitialisation.Initialise(Logger);

            // Load ourselves as a plugin
            PluginLoad(Assembly.GetExecutingAssembly(), String.Empty, false);

            // attempt to load the host
            PluginLoad(Assembly.GetEntryAssembly(), String.Empty, false);
        }


        /// <summary>
        /// Destructor
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Has a flag")]
        ~BasePluginManager()
        {
            if (!_disposed)
            {
                Dispose(false);
            }
        }

        #endregion Constructors / Destructors

        #region Properties

        /// <summary>
        /// Current service configurator, this will be set to null after configuration is complete
        /// </summary>
        /// <value>IServiceConfigurator</value>
        protected IServiceConfigurator ServiceConfigurator { get; private set; }

        /// <summary>
        /// Internal property for retrieving the application defined root path
        /// </summary>
        /// <value>string</value>
        protected string RootPath
        {
            get
            {
                return _configuration.CurrentPath;
            }
        }

        /// <summary>
        /// Protected ILogger instance that can be retrieved via a descendant class
        /// </summary>
        protected ILogger Logger { get; private set; }

        /// <summary>
        /// Returns the active IServiceProvider to descendant classes
        /// </summary>
        /// <value>IServiceProvider</value>
        protected IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }


        /// <summary>
        /// Sets the IServiceConfigurator instance which will be called after service configurtion 
        /// is completed by the host and all plugins.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Configuration issues should be handled by the host app when starting.")]
        protected void SetServiceConfigurator(in IServiceConfigurator serviceConfigurator)
        {
            if (ServiceConfigurator != null)
                throw new InvalidOperationException("Only one IServiceConfigurator can be loaded");

            if (_serviceConfigurationComplete)
                throw new InvalidOperationException("The plugin manager has already configured its services");

            ServiceConfigurator = serviceConfigurator ?? throw new ArgumentNullException(nameof(serviceConfigurator));
        }

        #endregion Properties

        #region Abstract Methods

        /// <summary>
        /// Indicates that a plugin is about to be loaded.
        /// </summary>
        /// <param name="pluginFile">Assembly of the plugin that is about to load.</param>
        protected abstract void PluginLoading(in Assembly pluginFile);

        /// <summary>
        /// Indicates a plugin has been loaded.
        /// </summary>
        /// <param name="pluginFile">Assembly of the plugin that is about to load.</param>
        protected abstract void PluginLoaded(in Assembly pluginFile);

        /// <summary>
        /// Indicates that the plugin module has been initialised.
        /// </summary>
        /// <param name="pluginModule">IPluginModule instance for the plugin that has been loaded.</param>
        protected abstract void PluginInitialised(in IPluginModule pluginModule);

        /// <summary>
        /// Indicates that the plugin module has been configured.
        /// </summary>
        /// <param name="pluginModule">IPluginModule instance for the plugin that has been configured.</param>
        protected abstract void PluginConfigured(in IPluginModule pluginModule);

        /// <summary>
        /// Provides an opportunity for the PluginManager descendant to pre configure plugin modules, if desired.
        /// </summary>
        /// <param name="serviceProvider">IServiceCollection instance that can be used for pre configuration.</param>
        protected abstract void PreConfigurePluginServices(in IServiceCollection serviceProvider);

        /// <summary>
        /// Provides an opportunity for the PluginManager descendant to post configure plugin modules, if desired.
        /// </summary>
        /// <param name="serviceProvider">IServiceCollection instance that can be used for post configuration.</param>
        protected abstract void PostConfigurePluginServices(in IServiceCollection serviceProvider);

        /// <summary>
        /// Provides an opportunity for the PluginManager descendant to validate whether a resource can be extracted from within a plugin module.
        /// </summary>
        /// <param name="resourceName">string name of the resource to be extracted.</param>
        /// <returns></returns>
        protected abstract bool CanExtractResource(in string resourceName);

        /// <summary>
        /// Provides an opportunity for the PluginManager descendant to modify the name of the resource being extracted.
        /// </summary>
        /// <param name="resourceName">string name of the resource to be extracted.</param>
        protected abstract void ModifyPluginResourceName(ref string resourceName);

        /// <summary>
        /// Indicates that configuration of the IServiceCollection is now complete
        /// </summary>
        protected abstract void ServiceConfigurationComplete(in IServiceProvider serviceProvider);

        #endregion Abstract Methods

        #region Public Methods

        /// <summary>
        /// Returns all loaded plugin data
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, IPluginModule> PluginsGetLoaded()
        {
            return _plugins;
        }

        /// <summary>
        /// Loads and configures an individual plugin
        /// </summary>
        /// <param name="assembly">Assembly being loaded.</param>
        /// <param name="fileLocation">Location of assembly on physical disk.</param>
        /// <param name="extractResources">Determines whether resources are extracted from the plugin module or not.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public void PluginLoad(in Assembly assembly, in string fileLocation, in bool extractResources)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            if (_pluginSettings.Disabled)
            {
                Logger.AddToLog(LogLevel.Warning, "PluginManager is disabled");
                return;
            }

            PluginLoading(assembly);

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
                Logger.AddToLog(LogLevel.Warning, pluginSetting.Name, "PluginManager is disabled");
                return;
            }

            bool interfaceFound = false;

            foreach (Type type in assembly.GetTypes())
            {
                try
                {
                    if (type.GetInterface(nameof(IPlugin)) != null)
                    {
                        interfaceFound = true;
                        IPlugin pluginService = (IPlugin)Activator.CreateInstance(type, GetParameterInstances(type));

                        if (extractResources && !pluginSetting.PreventExtractResources)
                        {
                            ExtractResources(assembly, pluginSetting);
                        }

                        PluginModule pluginModule = new PluginModule(assembly, assemblyName, pluginService);

                        IPluginVersion version = GetPluginClass<IPluginVersion>(pluginModule);

                        pluginModule.Version = version == null ? (ushort)1 :
                            GetMinMaxValue(version.GetVersion(), 1, MaxPluginVersion);

                        try
                        {
                            string file = Path.GetFullPath(
                                String.IsNullOrEmpty(assembly.Location) ? fileLocation : assembly.Location);

                            if (File.Exists(file))
                                pluginModule.FileVersion = FileVersionInfo.GetVersionInfo(file).FileVersion;
                            else
                                pluginModule.FileVersion = "unknown";
                        }
                        catch (Exception err)
                        {
                            Logger.AddToLog(LogLevel.PluginLoadError, err, $"Failed to get version information {assembly.FullName}");
                        }

                        pluginModule.Plugin.Initialise(Logger);

                        //indicate that the plugin has been initialised
                        PluginInitialised(pluginModule);

                        _plugins.Add(assemblyName, pluginModule);

                        Logger.AddToLog(LogLevel.PluginLoadSuccess, assemblyName);


                        // only interested in first reference of IPlugin
                        break;
                    }
                }
                catch (MissingMethodException missingMethodException)
                {
                    Logger.AddToLog(LogLevel.PluginLoadError, missingMethodException,
                        $"Could not initialise IPlugin Instance: {assembly.FullName}{MethodBase.GetCurrentMethod().Name}");
                }
                catch (Exception typeLoader)
                {
                    Logger.AddToLog(LogLevel.PluginLoadError, typeLoader,
                        $"{assembly.FullName}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            if (!interfaceFound)
                Logger.AddToLog(LogLevel.PluginConfigureError, $"{assemblyName} contains no IPlugin Interface");

            PluginLoaded(assembly);
        }

        /// <summary>
        /// Loads and configures an individual plugin
        /// </summary>
        /// <param name="pluginName">Filename of plugin to be loaded.</param>
        /// <param name="copyLocal">If true, copies the plugin to a local temp area to load from.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public void PluginLoad(in string pluginName, in bool copyLocal)
        {
            if (String.IsNullOrEmpty(pluginName))
                throw new ArgumentNullException(nameof(pluginName));

            try
            {
                if (!File.Exists(pluginName))
                    throw new FileNotFoundException($"Assembly file not found: {nameof(pluginName)}");

                string pluginFile = copyLocal ? GetLocalCopyOfPlugin(pluginName) : pluginName;

                PluginSetting setting = GetPluginSetting(pluginName);

                if (!setting.Disabled)
                {
                    PluginLoad(LoadAssembly(pluginFile), pluginFile, true);
                }
            }
            catch (Exception error)
            {
                Logger.AddToLog(LogLevel.PluginLoadError, error, $"{pluginName}{MethodBase.GetCurrentMethod().Name}");
            }
        }

        /// <summary>
        /// Allows plugins to configure the services for all plugins
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            NotificationService notificationService = new NotificationService();
            services.AddSingleton<INotificationService>(notificationService);

            _serviceProvider = services.BuildServiceProvider();

            // run pre-initialise events
            PreConfigurePluginServices(services);

            Shared.Classes.ThreadManager.ThreadStart(notificationService,
                Constants.ThreadNotificationService,
                System.Threading.ThreadPriority.Lowest);

            PluginServices pluginServices = new PluginServices(this);
            services.AddSingleton<IPluginClassesService>(pluginServices);
            services.AddSingleton<IPluginHelperService>(pluginServices);
            services.AddSingleton<IPluginTypesService>(pluginServices);

            foreach (IPluginModule pluginModule in _plugins.Values)
            {
                pluginModule.Plugin.ConfigureServices(services);
            }

            // if no ILogger instance has been registered, register the default instance now.
            services.TryAddSingleton<ILogger>(Logger);

            // if no plugin has registered a setting provider, add the default appsettings json provider
            DefaultSettingProvider settingProvider = new DefaultSettingProvider(RootPath);
            services.TryAddSingleton<ISettingsProvider>(settingProvider);

            _serviceProvider = services.BuildServiceProvider();

            PostConfigurePluginServices(services);

            if (ServiceConfigurator != null)
            {
                ServiceConfigurator.RegisterServices(services);
                ServiceConfigurator = null;
                _serviceConfigurationComplete = true;
            }

            _serviceProvider = services.BuildServiceProvider();

            ServiceConfigurationComplete(_serviceProvider);
        }

        /// <summary>
        /// Provides an opportunity for plugins to configure services that can be used in IOC, this method creates 
        /// a custom IServiceCollection class and should only be used where the host does not natively include
        /// it's own IServiceCollection
        /// </summary>
        public void ConfigureServices()
        {
            ConfigureServices(new ServiceCollection() as IServiceCollection);
        }


        /// <summary>
        /// Retrieves the non instantiated classes which have attribute T, or if any of
        /// the methods or properties have attribute T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public List<Type> PluginGetTypesWithAttribute<T>()
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
                            Logger.AddToLog(LogLevel.Error, typeLoader, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                        }
                    }
                }
                catch (Exception error)
                {
                    Logger.AddToLog(LogLevel.Error, error, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            return Result;
        }

        /// <summary>
        /// Retrieves the non instantiated classes which inherit from T or implement 
        /// interface T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public List<Type> PluginGetClassTypes<T>()
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
                            if ((type.GetInterface(typeof(T).Name) != null) || type.IsSubclassOf(typeof(T)))
                            {
                                Result.Add(type);
                            }
                        }
                        catch (Exception typeLoader)
                        {
                            Logger.AddToLog(LogLevel.Error, typeLoader, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                        }
                    }
                }
                catch (Exception error)
                {
                    Logger.AddToLog(LogLevel.Error, error, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            return Result;
        }

        /// <summary>
        /// Retreives an instantiated specific type of class which inherits from a specific class 
        /// or interface from within the plugin modules
        /// </summary>
        /// <typeparam name="T">Type of interface/class</typeparam>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public List<T> PluginGetClasses<T>()
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
                            if ((type.GetInterface(typeof(T).Name) != null) || type.IsSubclassOf(typeof(T)))
                            {
                                Result.Add((T)Activator.CreateInstance(type, GetParameterInstances(type)));
                            }
                        }
                        catch (Exception typeLoader)
                        {
                            Logger.AddToLog(LogLevel.Error, typeLoader, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                        }
                    }
                }
                catch (Exception error)
                {
                    Logger.AddToLog(LogLevel.Error, error, $"{plugin.Value.Module}{MethodBase.GetCurrentMethod().Name}");
                }
            }

            return Result;
        }

        /// <summary>
        /// Determines whether a plugin is loaded, and retrieves the version
        /// </summary>
        /// <param name="pluginLibraryName"></param>
        /// <param name="version"></param>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool PluginLoaded(in string pluginLibraryName, out int version, out string module)
        {
            version = -1;
            module = pluginLibraryName;

            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                if (plugin.Value.Module.EndsWith(pluginLibraryName, StringComparison.CurrentCultureIgnoreCase))
                {
                    version = plugin.Value.Version;
                    module = plugin.Value.Assembly.Location;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Allows plugin descendents to load an Assembly, even if it is not a true plugin module, this will ensure
        /// it's classes and types can be found with other searches in other ways like when using IPluginClassesService etc.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public DynamicLoadResult AddAssembly(in Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));

            foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
            {
                if (plugin.Value.Assembly.ManifestModule.ModuleHandle == assembly.ManifestModule.ModuleHandle)
                {
                    return DynamicLoadResult.AlreadyLoaded;
                }
            }

            IPluginModule pluginModule = new PluginModule(assembly, assembly.ManifestModule.ScopeName, new DynamicPlugin())
            {
                Version = 1,
            };

            _plugins.Add(Path.GetFileName(assembly.ManifestModule.ScopeName), pluginModule);

            return DynamicLoadResult.Success;
        }

        #endregion Public Methods

        #region Internal Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal object[] GetParameterInstances(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            List<object> Result = new List<object>();

            if (_serviceProvider != null)
            {
                //grab a list of all constructors in the class, start with the one with most parameters
                List<ConstructorInfo> constructors = type.GetConstructors()
                    .Where(c => c.IsPublic && !c.IsStatic && c.GetParameters().Length > 0)
                    .OrderByDescending(c => c.GetParameters().Length)
                    .ToList();

                foreach (ConstructorInfo constructor in constructors)
                {
                    foreach (ParameterInfo param in constructor.GetParameters())
                    {
                        object paramClass = _serviceProvider.GetService(param.ParameterType);

                        // if we didn't find a specific param type for this constructor, try the next constructor
                        if (paramClass == null)
                        {
                            Result.Clear();
                            break;
                        }

                        Result.Add(paramClass);
                    }

                    if (Result.Count > 0)
                        return Result.ToArray();
                }
            }

            return Result.ToArray();
        }

        #endregion Internal Methods

        #region IDisposable Methods

        /// <summary>
        /// IDisposable Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposable method, notify all plugins to finalise
        /// </summary>
        /// <param name="disposing">Indicates that the method has been called from IDispose.Dispose() </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing || !_disposed)
            {
                if (_plugins != null && _plugins.Count > 0)
                {
                    foreach (KeyValuePair<string, IPluginModule> plugin in _plugins)
                    {
                        try
                        {
                            plugin.Value.Plugin.Finalise();
                        }
                        catch (Exception error)
                        {
                            Logger.AddToLog(LogLevel.Error, error, $"{plugin.Key}{MethodBase.GetCurrentMethod().Name}");
                        }
                    }

                    _plugins.Clear();
                }

                _disposed = true;
            }
        }

        #endregion IDisposable Methods

        #region Private Methods

        /// <summary>
        /// Copies the plugin file to a local temp area, that will be used to load the plugin from.
        /// </summary>
        /// <param name="pluginFile">Path and File name of the plugin that will be loaded.</param>
        /// <returns>string</returns>
        private string GetLocalCopyOfPlugin(string pluginFile)
        {
            string pluginCopy = Path.Combine(_pluginSettings.LocalCopyPath, Path.GetFileName(pluginFile));

            if (!File.Exists(pluginCopy))
            {
                if (!Directory.Exists(_pluginSettings.LocalCopyPath))
                    Directory.CreateDirectory(_pluginSettings.LocalCopyPath);

                File.Copy(pluginFile, pluginCopy, false);
                return pluginCopy;
            }

            FileVersionComparison versionComparison = new FileVersionComparison();
            FileInfo pluginFileInfo = new FileInfo(pluginFile);
            FileInfo pluginCopyInfo = new FileInfo(pluginCopy);

            if (versionComparison.Equals(pluginFileInfo, pluginCopyInfo))
            {
                return pluginCopy;
            }
            else if (versionComparison.Newer(pluginFileInfo, pluginCopyInfo))
            {
                File.Copy(pluginFile, pluginCopy, true);
            }

            return pluginCopy;
        }

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
                return minValue;
            else if (value > maxValue)
                return maxValue;

            return value;
        }

        /// <summary>
        /// Returns the first class/interface of type T within the assembly
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="pluginModule">plugin module</param>
        /// <returns>instantiated instance of Type T if found, otherwise null</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        private T GetPluginClass<T>(in PluginModule pluginModule)
        {
            try
            {
                foreach (Type type in pluginModule.Assembly.GetTypes())
                {
                    try
                    {
                        if ((type.GetInterface(typeof(T).Name) != null) || type.IsSubclassOf(typeof(T)))
                        {
                            return (T)Activator.CreateInstance(type, GetParameterInstances(type));
                        }
                    }
                    catch (Exception typeLoader)
                    {
                        Logger.AddToLog(LogLevel.Error, typeLoader, $"{pluginModule.Module}{MethodBase.GetCurrentMethod().Name}");
                    }
                }
            }
            catch (Exception error)
            {
                Logger.AddToLog(LogLevel.Error, error, $"{pluginModule.Module}{MethodBase.GetCurrentMethod().Name}");
            }

            return default;
        }

        private string GetLiveFilePath(in string assemblyName, in string resourceName)
        {
            // remove the first part of the name which is the library
            string Result = resourceName.Replace(assemblyName, String.Empty);

            int lastIndex = Result.LastIndexOf('.');

            if (lastIndex > 0)
            {
                Result = Result.Substring(0, lastIndex).Replace(".", "\\") + Result.Substring(lastIndex);
            }

            Result = Path.Combine(_configuration.CurrentPath, Result);
            ModifyPluginResourceName(ref Result);

            return Result;
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
                    string resourceFileName = GetLiveFilePath(assemblyName, resource);

                    if (!CanExtractResource(resourceFileName))
                        continue;

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "Move along, nothing to see here")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "I wanted to...")]
        private Assembly LoadAssembly(in string assemblyName)
        {
            string assembly = assemblyName;

            if (!Path.IsPathRooted(assembly))
                assembly = Path.GetFullPath(assembly);

            return Assembly.LoadFrom(assembly);
        }


        /// <summary>
        /// If associated/required dll's are not found, and settings are configured, 
        /// attempt to load them from the configured path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns>Resolved assemble, if found, otherwise null.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        private Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (String.IsNullOrWhiteSpace(_pluginSettings.SystemFiles) ||
                !Directory.Exists(_pluginSettings.SystemFiles))
            {
                return null;
            }

            string filename = args.Name.ToLower().Split(',')[0];
            string assembly = Path.Combine(_pluginSettings.SystemFiles, filename);

            if (!assembly.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase))
                assembly += ".dll";

            try
            {
                if (File.Exists(assembly))
                    return Assembly.LoadFrom(assembly);
            }
            catch (Exception error)
            {
                Logger.AddToLog(LogLevel.PluginLoadError, error, $"{MethodBase.GetCurrentMethod().Name}");
            }

            return null;
        }

        /// <summary>
        /// Retrieve plugin settings for an individual plugin module
        /// </summary>
        /// <param name="pluginName">Name of plugin</param>
        /// <returns></returns>
        private PluginSetting GetPluginSetting(in string pluginName)
        {
            string name = Path.GetFileName(pluginName);

            foreach (PluginSetting setting in _pluginSettings.Plugins)
            {
                if (setting.Name.EndsWith(name, StringComparison.CurrentCultureIgnoreCase))
                    return setting;
            }

            return new PluginSetting(pluginName);
        }

        #endregion Private Methods
    }
}
