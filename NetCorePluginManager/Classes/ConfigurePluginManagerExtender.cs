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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;

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
#if NET_CORE_2_2 || NET_CORE_2_1 || NET_CORE_2_0 || NET461
            ConfigurePartManager(mvcBuilder);
            ConfigureCompiledViews(mvcBuilder);
#endif

#if NET_CORE_3_X
            AddApplicationParts(mvcBuilder);
#endif

            // allow plugins to configure MvcBuilder
            ConfigurePlugins(mvcBuilder);

            return mvcBuilder;
        }

        #endregion Public Static Methods

        #region Private Static Methods

#if NET_CORE_3_X
        private static void AddApplicationParts(in IMvcBuilder mvcBuilder)
        {
            foreach (KeyValuePair<string, IPluginModule> plugin in PluginManagerService.GetPluginManager().PluginsGetLoaded())
                mvcBuilder.AddApplicationPart(plugin.Value.Assembly);
        }
#endif

#if NET_CORE_2_2 || NET_CORE_2_1 || NET_CORE_2_0 || NET461

#pragma warning disable CS0618
        private static void ConfigurePartManager(in IMvcBuilder mvcBuilder)
        {
            // configure featue provider
            mvcBuilder.ConfigureApplicationPartManager(manager =>
            {
                IApplicationFeatureProvider oldMetadataReferenceFeatureProvider = manager.FeatureProviders.First(f => f is MetadataReferenceFeatureProvider);
                manager.FeatureProviders.Remove(oldMetadataReferenceFeatureProvider);
                manager.FeatureProviders.Add(new PluginFeatureProvider());
            });
        }
#pragma warning restore CS0618

        private static void ConfigureCompiledViews(in IMvcBuilder mvcBuilder)
        {
            // configure Compiled Views
            Dictionary<string, IPluginModule> plugins = PluginManagerService.GetPluginManager().PluginsGetLoaded();

            foreach (KeyValuePair<string, IPluginModule> keyValuePair in plugins)
            {
                string primaryModule = String.IsNullOrEmpty(keyValuePair.Value.Assembly.Location) ? keyValuePair.Value.Assembly.CodeBase : keyValuePair.Value.Assembly.Location;
                string compiledViewAssembly = Path.ChangeExtension(primaryModule, Constants.ViewsFileExtension);

                if (File.Exists(compiledViewAssembly))
                {
                    Assembly compiledViews = Assembly.LoadFrom(compiledViewAssembly);
                    mvcBuilder.ConfigureApplicationPartManager(apm =>
                    {
                        foreach (ApplicationPart part in new CompiledRazorAssemblyApplicationPartFactory().GetApplicationParts(compiledViews))
                            apm.ApplicationParts.Add(part);
                    });
                }
            }
        }
#endif

        private static void ConfigurePlugins(in IMvcBuilder mvcBuilder)
        {
            List<IConfigureMvcBuilder> appBuilderServices = PluginManagerService
                .GetPluginManager().PluginGetClasses<IConfigureMvcBuilder>();

            foreach (IConfigureMvcBuilder builder in appBuilderServices)
                builder.ConfigureMvcBuilder(mvcBuilder);
        }

        #endregion Private Static Methods
    }
}
