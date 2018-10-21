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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PluginManagerService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;
using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager
{
    public static class PluginManagerService
    {
        #region Private Members

        private const string LatestVersion = "latest";

        internal static PluginManager _pluginManagerInstance;
        private static ILogger _logger;
        private static PluginSettings _pluginConfiguration;
        private static string _currentPath;

        #endregion Private Members

        #region Static Methods

        public static bool Initialise(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            try
            {
                //load config and get settings
                ILoadSettingsService<PluginSettings> loadSettingsService = new Classes.LoadSettingsService<PluginSettings>();
                _pluginConfiguration = loadSettingsService.LoadSettings("PluginConfiguration");

                _pluginManagerInstance = new PluginManager(logger, _pluginConfiguration);

                _currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

                if (_currentPath.StartsWith(Directory.GetCurrentDirectory(), StringComparison.CurrentCultureIgnoreCase))
                    _currentPath = Directory.GetCurrentDirectory();

                if (_pluginConfiguration.Disabled)
                    return (false);

                // attempt to load the host
                _pluginManagerInstance.LoadPlugin(Assembly.GetEntryAssembly(), false);

                // are any plugins specifically mentioned in the config, load them
                // first so we have some control on the load order
                foreach (string file in _pluginConfiguration.PluginFiles)
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

                    _pluginManagerInstance.LoadPlugin(pluginFile);
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

                        _pluginManagerInstance.LoadPlugin(file);
                    }
                }
            }
            catch (Exception error)
            {
                _logger.AddToLog(LogLevel.PluginConfigureError, error, $"{MethodBase.GetCurrentMethod().Name}");
                return (false);
            }

            return (true);
        }

        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (_logger == null)
                throw new InvalidOperationException("Plugin Manager has not been initialised.");

            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            _pluginManagerInstance.Configure(app, env);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            if (_logger == null)
                throw new InvalidOperationException("Plugin Manager has not been initialised.");

            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (!_pluginConfiguration.DisableRouteDataService)
                services.AddSingleton<IRouteDataService, Classes.RouteDataServices>();

            services.AddSingleton<IPluginClassesService, PluginServices>();
            services.AddSingleton<IPluginHelperService, PluginServices>();
            services.AddSingleton<IPluginTypesService, PluginServices>();

            _pluginManagerInstance.ConfigureServices(services);
        }

        #endregion Static Methods

        #region Private Static Methods

        private static bool FindPlugin(ref string pluginFile, in PluginSetting pluginSetting)
        {
            string pluginSearchPath = _pluginConfiguration.PluginSearchPath;

            if (String.IsNullOrEmpty(pluginSearchPath) || !Directory.Exists(pluginSearchPath))
                pluginSearchPath = AddTrailingBackSlash(_currentPath);

            if (!String.IsNullOrEmpty(pluginSearchPath) && Directory.Exists(pluginSearchPath))
            {
                if (String.IsNullOrEmpty(pluginSetting.Version))
                    pluginSetting.Version = LatestVersion;

                string[] searchFiles = Directory.GetFiles(pluginSearchPath, Path.GetFileName(pluginFile), SearchOption.AllDirectories);

                if (searchFiles.Length == 0)
                    return (false);

                if (searchFiles.Length == 1)
                {
                    pluginFile = searchFiles[0];
                    return (true);
                }

                return (GetSpecificVersion(searchFiles, pluginSetting.Version, ref pluginFile));
            }

            return (false);
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
                pluginFile = fileVersions[fileVersions.Count -1].FullName;
                return (true);
            }

            // look for specific version
            foreach (FileInfo fileInfo in fileVersions)
            {
                if (FileVersionInfo.GetVersionInfo(fileInfo.FullName).FileVersion.ToString().StartsWith(version))
                {
                    pluginFile = fileInfo.FullName;
                    return (true);
                }
            }

            return (false);
        }

        private static PluginSetting GetPluginSetting(in string pluginName)
        {
            foreach (PluginSetting setting in _pluginConfiguration.Plugins)
            {
                if (pluginName.EndsWith(setting.Name))
                    return (setting);
            }

            return (new PluginSetting(pluginName));
        }

        private static string AddTrailingBackSlash(in string path)
        {
            if (path.EndsWith("\\"))
                return (path);

            return ($"{path}\\");
        }

        private static string GetPluginPath()
        {
            // is the path overridden in config
            if (!String.IsNullOrWhiteSpace(_pluginConfiguration.PluginPath) && 
                Directory.Exists(_pluginConfiguration.PluginPath))
            {
                return (_pluginConfiguration.PluginPath);
            }

            return (AddTrailingBackSlash(_currentPath) + "Plugins\\");
        }

        #endregion Private Static Methods
    }
}
