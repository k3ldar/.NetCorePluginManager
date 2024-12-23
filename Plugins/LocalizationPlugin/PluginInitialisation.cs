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
 *  Product:  Localization.Plugin
 *  
 *  File: PluginInitialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  09/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Globalization;

using Languages;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Localization.Plugin
{
	/// <summary>
	/// Implements IPlugin which allows the Localization.Plugin module to be
	/// loaded as a plugin module
	/// </summary>
	public class PluginInitialisation : IPlugin, IConfigureMvcBuilder, IInitialiseEvents
	{
		#region Internal Static Properties / Members

		internal readonly static CacheManager CultureCacheManager = new("Available Cultures", new TimeSpan(24, 0, 0), true, true);

		internal static string[] InstalledCultures { get; private set; }

		#endregion Internal Static Properties / Members

		#region IPlugin Methods

		public void Initialise(ILogger logger)
		{
			ThreadManager.Initialise();
		}

		public void Finalise()
		{
			if (ThreadManager.IsInitialized)
				ThreadManager.Finalise();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ICultureProvider, CultureProvider>();
			services.AddSingleton<IStringLocalizerFactory, StringLocalizerFactory>();

			IHostEnvironment environment = services.GetServiceInstance<IHostEnvironment>();

			InstalledCultures = LanguageWrapper.GetInstalledLanguages(environment.ContentRootPath);

			List<CultureInfo> cultures = [];

			for (int i = 0; i < InstalledCultures.Length; i++)
				cultures.Add(new CultureInfo(InstalledCultures[i]));

			services.Configure<RequestLocalizationOptions>(
				opts =>
				{
					opts.DefaultRequestCulture = new RequestCulture(InstalledCultures[0]);
					opts.SupportedCultures = cultures;
					opts.SupportedUICultures = cultures;
				});
		}

		public ushort GetVersion()
		{
			return 1;
		}

		#endregion IPlugin Methods

		#region IInitialiseEvents Methods

		public void BeforeConfigure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		public void AfterConfigure(in IApplicationBuilder app)
		{
			app.UseRequestLocalization();
			app.UseLocalizationMiddleware();
		}

		public void Configure(in IApplicationBuilder app)
		{
			// from interface but unused in this context
		}

		public void BeforeConfigureServices(in IServiceCollection services)
		{
			// from interface but unused in this context
		}

		public void AfterConfigureServices(in IServiceCollection services)
		{
			// from interface but unused in this context
		}

		#endregion IInitialiseEvents Methods

		#region IConfigureMvcBuilder Methods

		/// <summary>
		/// Configures localization within Mvc Builder
		/// </summary>
		/// <param name="mvcBuilder">IMvcBuilder instance</param>
		public void ConfigureMvcBuilder(in IMvcBuilder mvcBuilder)
		{
			if (mvcBuilder == null)
				throw new ArgumentNullException(nameof(mvcBuilder));

			mvcBuilder
				.AddViewLocalization(
					LanguageViewLocationExpanderFormat.Suffix,
					opts => { opts.ResourcesPath = "Resources"; })
				.AddDataAnnotationsLocalization();
		}

		#endregion IConfigureMvcBuilder Methods
	}
}

#pragma warning restore CS1591
