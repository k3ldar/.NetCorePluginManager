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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: UrlHashProviderTests.cs
 *
 *  Purpose:  Url hash provider tests Tests for text based storage
 *
 *  Date        Name                Reason
 *  11/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.DAL.TextFiles.Providers;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class UrlHashProviderTests : BaseProviderTests
	{
		[TestMethod]
		public void Construct_ValidInstance_Success()
		{
			string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
			try
			{
				ThreadManager.Initialise();
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);
				MockGeoIpProvider geoIp = new MockGeoIpProvider();
				services.AddSingleton<IGeoIpProvider>(geoIp);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					UrlHashProvider sut = provider.GetRequiredService<IUrlHashProvider>() as UrlHashProvider;
					Assert.IsNotNull(sut);


				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetUrlHash_ConvertsToSameHash_Success()
		{
			string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
			try
			{
				ThreadManager.Initialise();
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);
				MockGeoIpProvider geoIp = new MockGeoIpProvider();
				services.AddSingleton<IGeoIpProvider>(geoIp);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					UrlHashProvider sut = provider.GetRequiredService<IUrlHashProvider>() as UrlHashProvider;
					Assert.IsNotNull(sut);

					string hash = sut.GetUrlHash("https://Some.url/");
					Assert.AreEqual("ad13bc64bee70d6ba5df9afc6c3e2db8cafb6a8d557ad088a8a6373544b5b960", hash);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}
	}
}
