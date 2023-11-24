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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: ApplicationSettingsProvider.cs
 *
 *  Purpose:  Application Settings provider
 *
 *  Date        Name                Reason
 *  02/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Reflection;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class SettingsProvider : IApplicationSettingsProvider
    {
        private readonly ISimpleDBOperations<SettingsDataRow> _settingsData;

        public sealed class SettingValue
        {
            public string Value { get; set; }
        }

        public SettingsProvider(ISimpleDBOperations<SettingsDataRow> settingsData)
        {
            _settingsData = settingsData ?? throw new ArgumentNullException(nameof(settingsData));
        }

        public string RetrieveSetting(string name)
        {
            return RetrieveSetting(name, null);
        }

        public string RetrieveSetting(string name, string defaultValue)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            SettingsDataRow settingsDataRow = _settingsData.Select().FirstOrDefault(sd => sd.Name.Equals(name));

            if (settingsDataRow == null)
                return defaultValue;

            return settingsDataRow.Value;
        }

        public T RetrieveSetting<T>(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            SettingsDataRow settingsDataRow = _settingsData.Select().FirstOrDefault(sd => sd.Name.Equals(name));

            if (settingsDataRow == null)
                return default;

            MethodInfo methodInfo = typeof(T).GetMethod("Parse", new Type[] { typeof(string) });

            if (methodInfo == null)
                return default;

            return (T)methodInfo.Invoke(null, new object[] { settingsDataRow.Value });
        }

        public void UpdateSetting<T>(string name, T value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            SettingsDataRow settingsDataRow = _settingsData.Select().FirstOrDefault(sd => sd.Name.Equals(name)) ?? new SettingsDataRow()
                {
                    Name = name,
                };
			settingsDataRow.Value = value.ToString();

            _settingsData.InsertOrUpdate(settingsDataRow);
        }

        public void DeleteSetting(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            SettingsDataRow settingsDataRow = _settingsData.Select().FirstOrDefault(sd => sd.Name.Equals(name));

            if (settingsDataRow == null)
                return;

            _settingsData.Delete(settingsDataRow);
        }
    }
}
