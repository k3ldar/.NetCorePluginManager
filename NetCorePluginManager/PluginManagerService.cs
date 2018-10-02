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
using System.Reflection;
using System.IO;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCore.PluginManager
{
    public static class PluginManagerService
    {
        #region Private Members

        private static PluginManager _pluginManagerInstance;

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
                _pluginManagerInstance = new PluginManager(logger, GetPluginSettings());

                _currentPath = Directory.GetCurrentDirectory();

                //load config and get settings
                _pluginConfiguration = GetPluginSettings();

                if (_pluginConfiguration.Disabled)
                    return (false);

                // are any plugins specifically mentioned in the config, load them
                // first so we have some control on the load order
                foreach (string file in _pluginConfiguration.PluginFiles)
                {
                    if (String.IsNullOrEmpty(file) || !File.Exists(file))
                    {
                        if (!String.IsNullOrEmpty(file))
                        {
                            _logger.AddToLog($"Could not find plugin: {file}");
                        }

                        continue;
                    }

                    _pluginManagerInstance.LoadPlugin(file);
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
                _logger.AddToLog(error, $"{MethodBase.GetCurrentMethod().Name}");
                return (false);
            }

            return (true);
        }

        public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            if (env == null)
                throw new ArgumentNullException(nameof(env));

            _pluginManagerInstance.Configure(app, env);
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            _pluginManagerInstance.ConfigureServices(services);
        }

        public static List<T> GetPluginClasses<T>()
        {
            return (_pluginManagerInstance.GetPluginClasses<T>());
        }

        #endregion Static Methods

        #region Private Static Methods

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

        private static PluginSettings GetPluginSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            PluginSettings Result = new PluginSettings();
            config.GetSection("PluginConfiguration").Bind(Result);
            
            return (Result);
        }

        #endregion Private Static Methods
    }
}
