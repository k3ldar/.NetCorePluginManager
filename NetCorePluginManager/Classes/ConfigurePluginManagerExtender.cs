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
 *  File: ConfigurePluginManagerExtender.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  30/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static System.IO.File;

using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.Extensions.DependencyInjection;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// Plugin Manager IMvcBuilder extension class.
    /// </summary>
    public static class ConfigurePluginManagerExtender
    {
        #region Public Static Methods

        /// <summary>
        /// IApplicationBuilder extender method used to provide easy access to ConfigurePluginManager 
        /// method when configuring an application.
        /// </summary>
        /// <param name="mvcBuilder">IMvcBuilder instance.</param>
        /// <returns>IMvcBuilder</returns>
        public static IMvcBuilder ConfigurePluginManager(this IMvcBuilder mvcBuilder)
        {
            ConfigurePartManager(mvcBuilder);
            ConfigureCompiledViews(mvcBuilder);

            // allow plugins to configure MvcBuilder
            ConfigurePlugins(mvcBuilder);

            return mvcBuilder;
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void ConfigurePartManager(in IMvcBuilder mvcBuilder)
        {
            // configure featue provider
            mvcBuilder.ConfigureApplicationPartManager(manager =>
            {
                var oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
                manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
                manager.FeatureProviders.Add(new PluginFeatureProvider());
            });
        }

        private static void ConfigureCompiledViews(in IMvcBuilder mvcBuilder)
        { 
            // configure Compiled Views
            Dictionary<string, IPluginModule> plugins = PluginManagerService.GetPluginManager().GetLoadedPlugins();

            foreach (KeyValuePair<string, IPluginModule> keyValuePair in plugins)
            {
                string primaryModule =  String.IsNullOrEmpty(keyValuePair.Value.Assembly.Location) ? keyValuePair.Value.Assembly.CodeBase : keyValuePair.Value.Assembly.Location;
                string compiledViewAssembly = primaryModule.Substring(0, primaryModule.Length - 4) + ".Views.dll";

                if (Exists(compiledViewAssembly))
                {
                    Assembly compiledViews = Assembly.LoadFrom(compiledViewAssembly);
                    mvcBuilder.ConfigureApplicationPartManager(apm =>
                    {
                        foreach (var part in new CompiledRazorAssemblyApplicationPartFactory().GetApplicationParts(compiledViews))
                            apm.ApplicationParts.Add(part);
                    });
                }
            }
        }

        private static void ConfigurePlugins(in IMvcBuilder mvcBuilder)
        {
            List<IConfigureMvcBuilder> appBuilderServices = PluginManagerService
                .GetPluginManager().GetPluginClasses<IConfigureMvcBuilder>();

            foreach (IConfigureMvcBuilder builder in appBuilderServices)
                builder.ConfigureMvcBuilder(mvcBuilder);
        }

        #endregion Private Static Methods
    }
}
