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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: PluginManagerConfiguration.cs
 *
 *  Purpose:  Wrapper around appsettings.json, used only if no other provider is specified
 *
 *  Date        Name                Reason
 *  28/01/2019  Simon Carter        Initially Created
 *  28/04/2019  Simon Carter        #66 Add config file to PluginManagerConfiguration
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using PluginManager.Abstractions;
using PluginManager.Internal;

namespace PluginManager
{
    /// <summary>
    /// Plugin Manager configuration class.  Used when Initialising the Plugin Manager.
    /// </summary>
    public class PluginManagerConfiguration
    {
        #region Constructors

        /// <summary>
        /// Default constructor, uses all default settings.
        /// </summary>
        public PluginManagerConfiguration()
            : this(new DefaultLogger(), new LoadSettingsService())
        {

        }

        /// <summary>
        /// Constructor allowing host application to supply a custom ILogger implementation.
        /// </summary>
        /// <param name="logger">Valid instance of ILogger.</param>
        public PluginManagerConfiguration(in ILogger logger)
            : this(logger, new LoadSettingsService())
        {

        }

        /// <summary>
        /// Constructor allowing host application to supply a custom ILoadSettingsService implementation
        /// that can be used to obtain settings for AspNetCore.PluginManager from any data store.
        /// </summary>
        /// <param name="loadSettingsService">Valid instance of ILoadSettingsService.</param>
        public PluginManagerConfiguration(in ILoadSettingsService loadSettingsService)
            : this(new DefaultLogger(), loadSettingsService)
        {

        }

        /// <summary>
        /// Constructor allowing host application to supply a custom ILogger and ILoadSettingsService
        /// implementation.
        /// </summary>
        /// <param name="logger">Valid instance of ILogger</param>
        /// <param name="loadSettingsService">Valid instance of ILoadSettingsService.</param>
        public PluginManagerConfiguration(in ILogger logger, in ILoadSettingsService loadSettingsService)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            LoadSettingsService = loadSettingsService ?? throw new ArgumentNullException(nameof(loadSettingsService));

            CurrentPath = Directory.GetCurrentDirectory();
            ConfigFileName = "appsettings.json";
        }

        /// <summary>
        /// Constructor allowing host application to supply a custom ILogger and ILoadSettingsService
        /// implementation.
        /// </summary>
        /// <param name="logger">Valid instance of <see cref="ILogger"/></param>
        /// <param name="loadSettingsService">Valid instance of <see cref="ILoadSettingsService"/>.</param>
        /// <param name="serviceConfigurator">Valid instance of <see cref="IServiceConfigurator"/></param>
        public PluginManagerConfiguration(in ILogger logger, in ILoadSettingsService loadSettingsService,
            in IServiceConfigurator serviceConfigurator)
            : this(logger, loadSettingsService)
        {
            ServiceConfigurator = serviceConfigurator ?? throw new ArgumentNullException(nameof(serviceConfigurator));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// ILogger instance used by the AspNetCore.PluginManager and plugin modules to 
        /// log information to a default log storage.
        /// 
        /// This can be within a database or file based.  The standard ILogger implementation 
        /// saves data to a log file.
        /// </summary>
        /// <value>ILogger</value>
        public ILogger Logger { get; private set; }

        /// <summary>
        /// ILoadSettingsService instance is used by the AspNetCore.PluginManager to load
        /// settings and configuration data for plugins that it will load.
        /// </summary>
        /// <value>ILoadSettingsService</value>
        public ILoadSettingsService LoadSettingsService { get; private set; }

        /// <summary>
        /// Current root path of the application.
        /// </summary>
        /// <value>string</value>
        public string CurrentPath { get; set; }

        /// <summary>
        /// Configuration file name that will be used by the default implementation of 
        /// ILoadSettingsService to obtain data.
        /// </summary>
        /// <value>string</value>
        public string ConfigFileName { get; set; }

        /// <summary>
        /// Configuration file name that will be used by the default implementation of 
        /// ILoadSettingsService to obtain data.
        /// </summary>
        /// <value>string</value>
        public string ConfigurationFile
        {
            get
            {
                return Path.Combine(CurrentPath, ConfigFileName);
            }
        }

        /// <summary>
        /// Allow the host, or a specific plugin with the ability to get notified after all services have been created.
        /// </summary>
        public IServiceConfigurator ServiceConfigurator { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Allow the ILogger instance to be replaced, for internal use only.
        /// </summary>
        /// <param name="logger"></param>
        public void ReplaceLogger(in ILogger logger)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion Methods
    }
}
