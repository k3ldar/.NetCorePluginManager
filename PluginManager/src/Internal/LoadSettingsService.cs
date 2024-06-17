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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager
 *  
 *  File: LoadSettingsService.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *  28/12/2019  Simon Carter        Converted to generic plugin that can be used by any 
 *                                  application type.  Originally part of .Net 
 *                                  Core Plugin Manager (AspNetCore.PluginManager)
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using System.Reflection;

using AppSettings;

using Microsoft.Extensions.Configuration;

using PluginManager.Abstractions;

namespace PluginManager.Internal
{
	internal class LoadSettingsService : ILoadSettingsService
	{
		public T LoadSettings<T>(in string jsonFile, in string name)
		{
			ConfigurationBuilder builder = new();
			IConfigurationBuilder configBuilder = builder.SetBasePath(Path.GetDirectoryName(jsonFile));
			configBuilder.AddJsonFile(jsonFile);
			IConfigurationRoot config = builder.Build();

			T Result = (T)Activator.CreateInstance(typeof(T));

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
