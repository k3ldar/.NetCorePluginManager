/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.Tests
 *  
 *  File: MockPluginManagerConfiguration.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  126/12/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;

using PluginManager;

namespace AspNetCore.PluginManager.Tests.Shared
{
	public sealed class MockPluginManagerConfiguration : IPluginManagerConfiguration, IDisposable
	{
		private readonly string _configurationFile;

		public MockPluginManagerConfiguration()
			: this(Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp"))
		{

		}

		public MockPluginManagerConfiguration(string configurationFile)
		{
			_configurationFile = configurationFile;

			if (!File.Exists(_configurationFile))
			{
				string appSettingsFile = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "AppSettings.json");
				File.WriteAllText(_configurationFile, File.ReadAllText(appSettingsFile));
			}
		}

		~MockPluginManagerConfiguration()
		{
			Dispose();
		}

		public string CurrentPath => Path.GetFullPath(_configurationFile);

		public string ConfigFileName => Path.GetFileName(_configurationFile);

		public string ConfigurationFile => _configurationFile;

		public void Dispose()
		{
			if (File.Exists(_configurationFile))
			{
				File.Delete(_configurationFile);
			}
		}
	}
}
