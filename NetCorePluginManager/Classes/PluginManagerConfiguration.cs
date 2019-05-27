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
 *  File: PluginManagerConfiguration.cs
 *
 *  Purpose:  Wrapper around appsettings.json, used only if no other provider is specified
 *
 *  Date        Name                Reason
 *  28/01/2019  Simon Carter        Initially Created
 *  28/04/2019  Simon Carter        #66 Add config file to PluginManagerConfiguration
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using SharedPluginFeatures;

using AspNetCore.PluginManager.Classes;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// Plugin Manager configuration class.  Used when Initialising the Plugin Manager.
    /// </summary>
    public sealed class PluginManagerConfiguration
    {
        #region Constructors

        public PluginManagerConfiguration()
            : this (new DefaultLogger(), new LoadSettingsService())
        {

        }

        public PluginManagerConfiguration(in ILogger logger)
            : this (logger, new LoadSettingsService())
        {

        }

        public PluginManagerConfiguration(in ILoadSettingsService loadSettingsService)
            : this (new DefaultLogger(), loadSettingsService)
        {

        }

        public PluginManagerConfiguration(in ILogger logger, in ILoadSettingsService loadSettingsService)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            LoadSettingsService = loadSettingsService ?? throw new ArgumentNullException(nameof(loadSettingsService));

            CurrentPath = Directory.GetCurrentDirectory();
            ConfigFileName = "appsettings.json";
        }

        #endregion Constructors

        #region Properties

        public ILogger Logger { get; private set; }

        public ILoadSettingsService LoadSettingsService { get; private set; }

        public string CurrentPath { get; set; }

        public string ConfigFileName { get; set; }

        public string ConfigurationFile
        {
            get
            {
                return Path.Combine(CurrentPath, ConfigFileName);
            }
        }

        #endregion Properties
    }
}
