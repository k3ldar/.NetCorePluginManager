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
 *  File: PluginSetting.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace PluginManager
{
    /// <summary>
    /// Individual plugin module settings, used to define how a plugin is treated by the PluginManager.
    /// </summary>
    public class PluginSetting
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PluginSetting()
        {

        }

        /// <summary>
        /// Constructor requesting setting for individual plugin module.
        /// </summary>
        /// <param name="pluginName">Name of plugin module whose settings are requested.</param>
        public PluginSetting(in string pluginName)
        {
            if (String.IsNullOrWhiteSpace(pluginName))
                throw new ArgumentNullException(nameof(pluginName));

            Name = pluginName;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Name of the plugin module, i.e. MyPlugin.dll
        /// </summary>
        /// <value>string</value>
        public string Name { get; set; }

        /// <summary>
        /// Indicates whether the plugin module is disabled and will not be loaded.
        /// </summary>
        /// <value>bool</value>
        public bool Disabled { get; set; }

        /// <summary>
        /// Prevents PluginManager from extracting resources from the plugin module.
        /// 
        /// If true then no resources will be extracted, this can be usefule if the project has been given to a 
        /// web designer and they are making changes and don't want them overridden.
        /// </summary>
        /// <value>bool</value>
        public bool PreventExtractResources { get; set; }

        /// <summary>
        /// Prevents PluginManager from replacing resources that have previously been extracted from the plugin module.
        /// 
        /// If true then no resources will be extracted, this can be usefule if the project has been given to a 
        /// web designer and they are making changes and don't want them overridden.
        /// </summary>
        /// <value>bool</value>
        public bool ReplaceExistingResources { get; set; }

        /// <summary>
        /// Specifies the specific version number of the plugin to be loaded.
        /// 
        /// Use this value should multiple copies of the plugin module be within PluginSearchPath
        /// </summary>
        public string Version { get; set; }

        #endregion Properties
    }
}
