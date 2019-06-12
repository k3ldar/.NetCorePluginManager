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
 *  File: LoadSettingsService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Reflection;
using System.IO;

using Microsoft.Extensions.Configuration;
using AppSettings;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes
{
    internal class LoadSettingsService : ILoadSettingsService
    {
        public T LoadSettings<T>(in string jsonFile, in string name)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(Path.GetDirectoryName(jsonFile));
            configBuilder.AddJsonFile(jsonFile);
            IConfigurationRoot config = builder.Build();

            PluginManager pluginManager = PluginManagerService.GetPluginManager();

            T Result;

            if (pluginManager == null)
                Result = (T)Activator.CreateInstance(typeof(T));
            else
                Result = (T)Activator.CreateInstance(typeof(T), pluginManager.GetParameterInstances(typeof(T)));

            config.GetSection(name).Bind(Result);

            return ValidateSettings<T>.Validate(Result);
        }

        public T LoadSettings<T>(in string name)
        {
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            int isDebugPos = path.IndexOf("\\bin\\debug\\", StringComparison.InvariantCultureIgnoreCase);

            if (isDebugPos > -1)
                path = path.Substring(0, isDebugPos);

            return LoadSettings<T>(Path.Combine(path, "appsettings.json"), name);
        }
    }
}
