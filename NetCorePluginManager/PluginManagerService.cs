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
 *  File: PluginManagerService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  27/10/2018  Simon Carter        Change internal plugin to internal property
 *  27/10/2018  Simon Carter        Add thread management as used by multiple plugins
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

using PluginManager;
using PluginManager.Abstractions;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// Static class containing methods that can be used to configure and initialise the Plugin Manager.
    /// </summary>
    public static class PluginManagerService
    {
        #region Private Members

        private const string LatestVersion = "latest";

        private static NetCorePluginManager _pluginManagerInstance;
        private static ILogger _logger;
        private static NetCorePluginSettings _pluginSettings;
        private static string _rootPath;
        private static List<Type> _preinitialisedPlugins = new List<Type>();
        private static PluginManagerConfiguration _configuration;

        #endregion Private Members

        #region Static Methods

        /// <summary>
        /// Initialises the PluginManager using default confguration.
        /// </summary>
        /// <returns>bool</returns>
        public static bool Initialise()
        {
            return Initialise(new PluginManagerConfiguration());
        }

        /// <summary>
        /// Initialises the PluginManager using a specific user defined configuration.
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns>bool</returns>
        public static bool Initialise(in PluginManagerConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = new Classes.LoggerStatistics();
            Classes.LoggerStatistics.SetLogger(configuration.Logger);
            configuration.ReplaceLogger(_logger);

            try
            {
                _rootPath = configuration.CurrentPath;

                //load config and get settings
                if (File.Exists(configuration.ConfigurationFile))
                {
                    _pluginSettings = configuration.LoadSettingsService.LoadSettings<NetCorePluginSettings>(
                        configuration.ConfigurationFile, "PluginConfiguration");
                }
                else
                {
                    _pluginSettings = new NetCorePluginSettings();
                    AppSettings.ValidateSettings<NetCorePluginSettings>.Validate(_pluginSettings);
                }

                _pluginManagerInstance = new NetCorePluginManager(configuration, _pluginSettings);


                if (_rootPath.StartsWith(Directory.GetCurrentDirectory(), StringComparison.CurrentCultureIgnoreCase))
                    _rootPath = Directory.GetCurrentDirectory();

                if (_pluginSettings.Disabled)
                    return false;

                if (_pluginSettings.CreateLocalCopy && String.IsNullOrEmpty(_pluginSettings.LocalCopyPath))
                {
                    _pluginSettings.LocalCopyPath = Path.Combine(_rootPath, Constants.TempPluginPath);
                    Directory.CreateDirectory(_pluginSettings.LocalCopyPath);
                }

                // Load ourselves
                _pluginManagerInstance.PluginLoad(Assembly.GetExecutingAssembly(), String.Empty, false);

                // load any pre loaded plugins from UsePlugin
                foreach (Type pluginType in _preinitialisedPlugins)
                    GetPluginManager().PluginLoad(pluginType.Assembly.Location, false);

                _preinitialisedPlugins = null;


                // are any plugins specifically mentioned in the config, load them
                // first so we have some control on the load order

                if (_pluginSettings.PluginFiles != null)
                {
                    foreach (string file in _pluginSettings.PluginFiles)
                    {
                        string pluginFile = file;

                        if (String.IsNullOrEmpty(pluginFile) || !File.Exists(pluginFile))
                        {
                            if (!String.IsNullOrEmpty(pluginFile) && !FindPlugin(ref pluginFile, GetPluginSetting(pluginFile)))
                            {
                                if (!String.IsNullOrEmpty(pluginFile))
                                {
                                    _logger.AddToLog(LogLevel.PluginLoadFailed, $"Could not find plugin: {pluginFile}");
                                }

                                continue;
                            }
                        }
#if NET_CORE_3_0
                        _logger.AddToLog(LogLevel.PluginConfigureError, $"Unable to load {pluginFile} dynamically, use UsePlugin() method instead.");
#else
                        _pluginManagerInstance.PluginLoad(pluginFile, _pluginSettings.CreateLocalCopy);
#endif
                    }
                }

                // load generic plugins next, if any exist
                string pluginPath = GetPluginPath();

                if (Directory.Exists(pluginPath))
                {
                    // load all plugins in the folder
                    string[] pluginFiles = Directory.GetFiles(pluginPath);

                    foreach (string file in pluginFiles)
                    {
                        if (String.IsNullOrEmpty(file) || !File.Exists(file))
                            continue;

                        _pluginManagerInstance.PluginLoad(file, _pluginSettings.CreateLocalCopy);
                    }
                }
            }
            catch (Exception error)
            {
                _logger.AddToLog(LogLevel.PluginConfigureError, error, $"{MethodBase.GetCurrentMethod().Name}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Finalises the PluginManger, provides an opportunity for the plugins to clean up ready for close down.
        /// </summary>
        /// <returns></returns>
        public static void Finalise()
        {
            if (_logger == null || _pluginManagerInstance == null)
                throw new InvalidOperationException("Plugin Manager has not been initialised.");

            _pluginManagerInstance.Dispose();
            _pluginManagerInstance = null;
            _logger = null;
            _pluginSettings = null;
            _configuration = null;
        }

        /// <summary>
        /// Configures all plugin modules, allowing the modules to setup services for the application.
        /// </summary>
        /// <param name="app">IApplicationBuilder instance.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the Plugin Manager has not been initialised.</exception>
        public static void Configure(IApplicationBuilder app)
        {
            if (_logger == null || _pluginManagerInstance == null)
                throw new InvalidOperationException("Plugin Manager has not been initialised.");

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (_pluginSettings.Disabled)
                return;

            _pluginManagerInstance.Configure(app);
        }

#pragma warning disable CS1591
        /// <summary>
        /// Configure all plugin modules, this method is now deprecated and will be removed in a future release
        /// </summary>
        /// <param name="app">IApplicationBuilder instance</param>
        /// <param name="env">IHostingEnvironment instance</param>
        [Obsolete("This method is obsolete and will be removed in the next version.  Use Configure(app); instead.")]
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            Configure(app);
        }
#pragma warning restore CS1591

        /// <summary>
        /// Configures all plugin module services, allowing the modules to add their own services to the application.
        /// </summary>
        /// <param name="services">IServiceCollection instance</param>
        public static void ConfigureServices(IServiceCollection services)
        {
            if (_logger == null || _pluginManagerInstance == null)
                throw new InvalidOperationException("Plugin Manager has not been initialised.");

            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (_pluginSettings.Disabled)
                return;

            if (!_pluginSettings.DisableRouteDataService)
                services.AddSingleton<IRouteDataService, RouteDataServices>();

            _pluginManagerInstance.ConfigureServices(services);
        }

        /// <summary>
        /// UsePlugin is designed to load plugins that have been statically loaded into the host application specifically nuget packages or project references.
        /// 
        /// If a plugin is required to be initialised prior to other plugins, you can alter the load order by calling UsePlugin prior to calling Initialise.
        /// </summary>
        /// <param name="iPluginType">Type of IPlugin interface.  The type passed in must inherit IPlugin interface.</param>
        /// <exception cref="System.InvalidOperationException">Thrown when the iPluginType does not implement IPlugin interface.</exception>
        public static void UsePlugin(Type iPluginType)
        {
            if (iPluginType.GetInterface(typeof(IPlugin).Name) != null)
            {
                if (_preinitialisedPlugins == null)
                    GetPluginManager().PluginLoad(iPluginType.Assembly.Location, false);
                else
                    _preinitialisedPlugins.Add(iPluginType);
            }
            else
            {
                throw new InvalidOperationException($"Type {nameof(iPluginType)} must implement {nameof(IPlugin)}");
            }
        }

        #endregion Static Methods

        #region Internal Static Methods

        internal static NetCorePluginManager GetPluginManager()
        {
            return _pluginManagerInstance;
        }

        internal static ILogger GetLogger()
        {
            return _logger;
        }

        internal static string RootPath()
        {
            return _rootPath;
        }

        internal static PluginManagerConfiguration Configuration()
        {
            return _configuration;
        }

        #endregion Internal Static Methods

        #region Private Static Methods

        private static bool FindPlugin(ref string pluginFile, in PluginSetting pluginSetting)
        {
            string pluginSearchPath = _pluginSettings.PluginSearchPath;

            if (String.IsNullOrEmpty(pluginSearchPath) || !Directory.Exists(pluginSearchPath))
                pluginSearchPath = AddTrailingBackSlash(_rootPath);

            if (!String.IsNullOrEmpty(pluginSearchPath) && Directory.Exists(pluginSearchPath))
            {
                if (String.IsNullOrEmpty(pluginSetting.Version))
                    pluginSetting.Version = LatestVersion;

                string[] searchFiles = Directory.GetFiles(pluginSearchPath, Path.GetFileName(pluginFile), SearchOption.AllDirectories);

                if (searchFiles.Length == 0)
                    return false;

                if (searchFiles.Length == 1)
                {
                    pluginFile = searchFiles[0];
                    return true;
                }

                return GetSpecificVersion(searchFiles, pluginSetting.Version, ref pluginFile);
            }

            return false;
        }

        private static bool GetSpecificVersion(string[] searchFiles, in string version, ref string pluginFile)
        {
            // get list of all version info
            List<FileInfo> fileVersions = new List<FileInfo>();

            foreach (string file in searchFiles)
                fileVersions.Add(new FileInfo(file));

            fileVersions.Sort(new FileVersionComparison());

            // are we after the latest version
            if (version == LatestVersion)
            {
                pluginFile = fileVersions[fileVersions.Count - 1].FullName;
                return true;
            }

            // look for specific version
            foreach (FileInfo fileInfo in fileVersions)
            {
                if (FileVersionInfo.GetVersionInfo(fileInfo.FullName).FileVersion.ToString().StartsWith(version))
                {
                    pluginFile = fileInfo.FullName;
                    return true;
                }
            }

            return false;
        }

        private static PluginSetting GetPluginSetting(in string pluginName)
        {
            foreach (PluginSetting setting in _pluginSettings.Plugins)
            {
                if (pluginName.EndsWith(setting.Name))
                    return setting;
            }

            return new PluginSetting(pluginName);
        }

        private static string AddTrailingBackSlash(in string path)
        {
            if (path.EndsWith("\\"))
                return path;

            return $"{path}\\";
        }

        private static string GetPluginPath()
        {
#if NET_CORE_3_X
            return String.Empty;
#else
            // is the path overridden in config
            if (!String.IsNullOrWhiteSpace(_pluginSettings.PluginPath) &&
                Directory.Exists(_pluginSettings.PluginPath))
            {
                return _pluginSettings.PluginPath;
            }

            return AddTrailingBackSlash(_rootPath) + "Plugins\\";
#endif
        }

        #endregion Private Static Methods
    }
}
