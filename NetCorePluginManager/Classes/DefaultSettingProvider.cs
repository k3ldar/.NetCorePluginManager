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
 *  File: DefaultSettingProvider.cs
 *
 *  Purpose:  Wrapper around appsettings.json, used only if no other provider is specified
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.Extensions.Configuration;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Classes
{
    public sealed class DefaultSettingProvider : ISettingsProvider
    {
        public T GetSettings<T>(in string storage, in string sectionName)
        {
            if (String.IsNullOrEmpty(storage))
                throw new ArgumentNullException(nameof(storage));

            if (String.IsNullOrEmpty(sectionName))
                throw new ArgumentNullException(nameof(sectionName));

            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(PluginManagerService.RootPath());
            configBuilder.AddJsonFile(storage);
            IConfigurationRoot config = builder.Build();
            T Result = (T)Activator.CreateInstance(typeof(T));
            config.GetSection(sectionName).Bind(Result);

            return (AppSettings.ValidateSettings<T>.Validate(Result));
        }

        public T GetSettings<T>(in string sectionName)
        {
            return GetSettings<T>("appsettings.json", sectionName);
        }
    }
}
