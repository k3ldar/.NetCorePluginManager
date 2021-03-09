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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.TestSettingsProvider.Tests
 *  
 *  File: TestActionDescriptorCollectionProvider.cs
 *
 *  Purpose:  Mock TestSettingsProvider class
 *
 *  Date        Name                Reason
 *  05/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */using System;
using System.IO;

using Microsoft.Extensions.Configuration;

using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests.Shared
{
    public class TestSettingsProvider : ISettingsProvider
    {
        private readonly string _jsonData;

        public TestSettingsProvider(string jsonData)
        {
            if (String.IsNullOrEmpty(jsonData))
                throw new ArgumentNullException(nameof(jsonData));

            _jsonData = jsonData;
        }

        public T GetSettings<T>(in string storage, in string sectionName)
        {
            if (String.IsNullOrEmpty(storage))
                throw new ArgumentNullException(nameof(storage));

            if (String.IsNullOrEmpty(sectionName))
                throw new ArgumentNullException(nameof(sectionName));

            string tempDataFile = Path.GetTempFileName();
            File.WriteAllText(tempDataFile, _jsonData);
            try
            {
                ConfigurationBuilder builder = new ConfigurationBuilder();
                IConfigurationBuilder configBuilder = builder.SetBasePath(Path.GetDirectoryName(tempDataFile));
                configBuilder.AddJsonFile(Path.GetFileName(tempDataFile));
                IConfigurationRoot config = builder.Build();
                T Result = (T)Activator.CreateInstance(typeof(T));
                config.GetSection(sectionName).Bind(Result);

                return AppSettings.ValidateSettings<T>.Validate(Result);
            }
            finally
            {
                File.Delete(tempDataFile);
            }
        }

        public T GetSettings<T>(in string sectionName)
        {
            return GetSettings<T>("appsettings.json", sectionName);
        }
    }
}
