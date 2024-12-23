﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  AspNetCore.PluginManager
 *  
 *  File: PluginManager.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  28/04/2019  Simon Carter        #63 Allow plugin to be dynamically added.
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using AspNetCore.PluginManager.Classes.Minify;
using AspNetCore.PluginManager.Internal;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable IDE0034

namespace AspNetCore.PluginManager
{
	internal sealed class NetCorePluginManager : BasePluginManager
	{
		#region Private Members

		private List<IInitialiseEvents> _initializablePlugins;
		private readonly List<string> _extractedFiles;
		private readonly NetCorePluginSettings _netCorePluginSettings;

		#endregion Private Members

		#region Constructors

		internal NetCorePluginManager(in PluginManagerConfiguration configuration, in NetCorePluginSettings pluginSettings)
			: base(configuration, pluginSettings)
		{
			_netCorePluginSettings = pluginSettings ?? throw new ArgumentNullException(nameof(pluginSettings));
			_extractedFiles = [];
		}

		#endregion Constructors

		#region Overridden Methods

		protected override bool CanExtractResource(in string resourceName)
		{
			return true;
		}

		protected override void ModifyPluginResourceName(ref string resourceName)
		{
			// special case for minified js files which have naming convention of library.min.js
			if (resourceName.EndsWith("\\min.js"))
			{
				resourceName = resourceName.Replace("\\min.js", ".min.js");
			}

			// special case for minified css files which have naming convention of library.min.css
			if (resourceName.EndsWith("\\min.css"))
			{
				resourceName = resourceName.Replace("\\min.css", ".min.cs");
			}

			_extractedFiles.Add(resourceName);
		}

		protected override void PluginConfigured(in IPluginModule pluginModule)
		{
			// required by interface, not used in this context
		}

		protected override void PluginInitialised(in IPluginModule pluginModule)
		{
			// required by interface, not used in this context
		}

		protected override void PluginLoaded(in Assembly pluginFile)
		{
			// required by interface, not used in this context
		}

		protected override void PluginLoading(in Assembly pluginFile)
		{

		}

		protected override void PostConfigurePluginServices(in IServiceCollection serviceProvider)
		{
			foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
				initialiseEvents.AfterConfigureServices(serviceProvider);

			// if no minification engine has been added, add a default one now
			serviceProvider.TryAddTransient<IMinificationEngine, MinificationEngine>();

			// if no save/load data has been registered, add new default file storage options
			serviceProvider.TryAddSingleton<ISaveData>(new FileStorageSaveData(Logger, RootPath));
			serviceProvider.TryAddSingleton<ILoadData>(new FileStorageLoadData(Logger, RootPath));

			// if no custom virus scanner has been registered, add the default Microsoft virus scanner if on windows, otherwise a placebo scanner
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				serviceProvider.TryAddTransient<IVirusScanner, MicrosoftDefenderVirusScanner>();
			}
			else
			{
				serviceProvider.TryAddTransient<IVirusScanner, PlaceboVirusScanner>();
			}
		}

		protected override void PreConfigurePluginServices(in IServiceCollection serviceProvider)
		{
			_initializablePlugins = PluginGetClasses<IInitialiseEvents>();

			foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
				initialiseEvents.BeforeConfigureServices(serviceProvider);
		}

		protected override void ServiceConfigurationComplete(in IServiceCollection serviceCollection)
		{
			if (_netCorePluginSettings.MinifyFiles)
			{
				MinifyFilesIfRequired(serviceCollection);
			}
		}

		#endregion Overridden Methods

		#region Internal Methods

		/// <summary>
		/// Allows plugins to configure with the current App Builder
		/// </summary>
		/// <param name="app"></param>
		internal void Configure(in IApplicationBuilder app)
		{
			ServiceProvider = app.ApplicationServices;

			if (_netCorePluginSettings.MonitorRouteLoadTimes)
			{
				app.UseRouteLoadTimes();
			}

			foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
				initialiseEvents.BeforeConfigure(app);

			foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
				initialiseEvents.Configure(app);

			foreach (IInitialiseEvents initialiseEvents in _initializablePlugins)
				initialiseEvents.AfterConfigure(app);

			ConfigurationComplete();
		}

		#endregion Internal Methods

		#region Private Methods

		private void MinifyFilesIfRequired(in IServiceCollection serviceCollection)
		{
			ILogger logger = serviceCollection.GetServiceInstance<ILogger>();
			IMinificationEngine minifyEngine = serviceCollection.GetServiceInstance<IMinificationEngine>();
			INotificationService notificationService = serviceCollection.GetServiceInstance<INotificationService>();

			if (logger == null || minifyEngine == null || notificationService == null)
				return;

			object files = new();

			if (notificationService.RaiseEvent(Constants.NotificationEventMinifyFiles, null, null, ref files))
			{
				List<string> additionalFiles = (List<string>)files;

				foreach (string file in additionalFiles)
				{
					if (File.Exists(file) && !_extractedFiles.Contains(file))
						_extractedFiles.Add(file);
				}
			}

			MinificationThread minificationThread = new(_extractedFiles, logger, minifyEngine);
			ThreadManager.ThreadStart(minificationThread, Constants.MinificationThread, System.Threading.ThreadPriority.Normal);
		}

		#endregion Private Methods
	}
}
