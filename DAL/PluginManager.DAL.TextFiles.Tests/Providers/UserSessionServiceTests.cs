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
 *  File: UserSessionServiceTests.cs
 *
 *  Purpose:  User session service tests Tests for text based storage
 *
 *  Date        Name                Reason
 *  06/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class UserSessionServiceTests : BaseProviderTests
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
					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
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
		public void Created_SessionSavedToDatabase_Success()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					UserSession userSession = new UserSession(-1, DateTime.Now, "SN123", "The agent", "referrer site", "10.2.3.1", 
						"the host", true, true, false, ReferalType.Google, false, false, "Samsung", "Galax S7", 0, 1, 1, "GBP", 0);
					sut.Created(userSession);

					Assert.AreEqual(1, sessionData.RecordCount);
					Assert.AreEqual(1, sessionData.PrimarySequence);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Created_UserSessionNull_NoDataSaved()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					sut.Created(null);

					Assert.AreEqual(0, sessionData.RecordCount);
					Assert.AreEqual(0, sessionData.PrimarySequence);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Retrieve_SessionNull_ReturnsNullReference()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					UserSession userSession = null;
					sut.Retrieve(null, ref userSession);

					Assert.IsNull(userSession);
					Assert.AreEqual(0, sessionData.RecordCount);
					Assert.AreEqual(0, sessionData.PrimarySequence);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Retrieve_SessionNotFound_ReturnsNullReference()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					UserSession userSession = null;
					sut.Retrieve("SN-ABC", ref userSession);

					Assert.AreEqual(0, sessionData.RecordCount);
					Assert.AreEqual(0, sessionData.PrimarySequence);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Retrieve_SessionFoundInDatabase_Success()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					UserSession userSession = new UserSession(-1, DateTime.Now, "SN123", "The agent", "referrer site", "10.2.3.1",
						"the host", true, true, false, ReferalType.Google, false, false, "Samsung", "Galax S7", 0, 1, 1, "GBP", 0);
					sut.Created(userSession);

					Assert.AreEqual(1, sessionData.RecordCount);
					Assert.AreEqual(1, sessionData.PrimarySequence);

					UserSession session = null;
					sut.Retrieve("SN123", ref session);
					Assert.IsNotNull(session);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Save_SessionNull_ReturnsNullReference()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					sut.Save(null);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Save_SessionFoundInDatabase_Success()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					UserSession userSession = new UserSession(-1, DateTime.Now, "SN123", "The agent", "referrer site", "10.2.3.1",
						"the host", true, true, false, ReferalType.Google, false, false, "Samsung", "Galax S7", 0, 1, 1, "GBP", 0);
					Assert.AreEqual(0, userSession.Pages.Count);

					userSession.PageView("/test", "ref", false);
					Assert.AreEqual(SaveStatus.Saved, userSession.SaveStatus);
					Assert.AreEqual(1, userSession.Pages.Count);
					Assert.AreEqual(SaveStatus.Pending, userSession.Pages[0].SaveStatus);
					sut.Save(userSession);

					Assert.AreEqual(SaveStatus.Saved, userSession.SaveStatus);
					Assert.AreEqual(1, sessionData.RecordCount);
					Assert.AreEqual(1, sessionData.PrimarySequence);
					Assert.AreEqual(SaveStatus.Saved, userSession.Pages[0].SaveStatus);
					Assert.AreEqual(-9223372036854775808, userSession.Pages[0].ID);
				}
			}
			finally
			{
				ThreadManager.Finalise();
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[Timeout(3000)]
		public void Closing_SessionIsSavedToDatabase_Success()
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

					UserSessionService sut = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(sut);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					ISimpleDBOperations<SessionPageDataRow> sessionPageData = provider.GetRequiredService<ISimpleDBOperations<SessionPageDataRow>>();
					Assert.IsNotNull(sessionPageData);
					Assert.AreEqual(0, sessionPageData.RecordCount);

					ISimpleDBOperations<InitialReferralsDataRow> referrer = provider.GetRequiredService<ISimpleDBOperations<InitialReferralsDataRow>>();
					Assert.IsNotNull(referrer);
					Assert.AreEqual(0, referrer.RecordCount);

					ISimpleDBOperations<SessionStatsYearlyDataRow> yearlyStats = provider.GetRequiredService<ISimpleDBOperations<SessionStatsYearlyDataRow>>();
					Assert.IsNotNull(yearlyStats);
					Assert.AreEqual(0, yearlyStats.RecordCount);

					UserSession userSession = new UserSession(-1, DateTime.Now, "SN123", "The agent", "referrer site", "10.2.3.1",
						"the host", true, true, false, ReferalType.Google, false, false, "Samsung", "Galaxy S7", 0, 1, 1, "GBP", 0);
					Assert.AreEqual(0, userSession.Pages.Count);

					userSession.PageView("/test", "ref", false);
					Assert.AreEqual(SaveStatus.Saved, userSession.SaveStatus);
					Assert.AreEqual(1, userSession.Pages.Count);
					Assert.AreEqual(SaveStatus.Pending, userSession.Pages[0].SaveStatus);
					sut.Closing(userSession);

					int i = 0;

					while (i < 10)
					{
						System.Threading.Thread.Sleep(300);

						if (sessionData.RecordCount > 0 && sessionPageData.RecordCount > 0 && referrer.RecordCount > 0 && yearlyStats.RecordCount > 0)
							break;

						i++;
					}

					Assert.IsTrue(i < 10, "timed out waiting!");

					Assert.AreEqual(SaveStatus.Saved, userSession.SaveStatus);
					Assert.AreEqual(1, sessionData.RecordCount);
					Assert.AreEqual(1, sessionData.PrimarySequence);
					Assert.AreEqual(SaveStatus.Saved, userSession.Pages[0].SaveStatus);
					Assert.AreEqual(1, sessionPageData.RecordCount);
					Assert.AreEqual(1, referrer.RecordCount);
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
