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
 *  File: SessionStatisticsProviderTests.cs
 *
 *  Purpose:  User statistics provider Tests for text based storage
 *
 *  Date        Name                Reason
 *  13/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.SessionData;

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
	public class SessionStatisticsProviderTests : BaseProviderTests
	{
		[Ignore("Temporarily ignored due to potential build issue on azure")]
		[TestMethod]
		public void TestAllMethods_InOneHit_Success()
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

					UserSessionService userSessionService = provider.GetRequiredService<IUserSessionService>() as UserSessionService;
					Assert.IsNotNull(userSessionService);

					ISimpleDBOperations<SessionDataRow> sessionData = provider.GetRequiredService<ISimpleDBOperations<SessionDataRow>>();
					Assert.IsNotNull(sessionData);
					Assert.AreEqual(0, sessionData.RecordCount);

					ISimpleDBOperations<SessionPageDataRow> sessionPageData = provider.GetRequiredService<ISimpleDBOperations<SessionPageDataRow>>();
					Assert.IsNotNull(sessionPageData);
					Assert.AreEqual(0, sessionPageData.RecordCount);

					ISimpleDBOperations<InitialReferralsDataRow> referrer = provider.GetRequiredService<ISimpleDBOperations<InitialReferralsDataRow>>();
					Assert.IsNotNull(referrer);
					Assert.AreEqual(0, referrer.RecordCount);

					ISimpleDBOperations<SessionStatsHourlyDataRow> hourlyStats = provider.GetRequiredService<ISimpleDBOperations<SessionStatsHourlyDataRow>>();
					Assert.IsNotNull(hourlyStats);
					Assert.AreEqual(0, hourlyStats.RecordCount);

					ISimpleDBOperations<SessionStatsDailyDataRow> dailyStats = provider.GetRequiredService<ISimpleDBOperations<SessionStatsDailyDataRow>>();
					Assert.IsNotNull(dailyStats);
					Assert.AreEqual(0, dailyStats.RecordCount);

					ISimpleDBOperations<SessionStatsWeeklyDataRow> weeklyStats = provider.GetRequiredService<ISimpleDBOperations<SessionStatsWeeklyDataRow>>();
					Assert.IsNotNull(weeklyStats);
					Assert.AreEqual(0, weeklyStats.RecordCount);

					ISimpleDBOperations<SessionStatsMonthlyDataRow> monthlyStats = provider.GetRequiredService<ISimpleDBOperations<SessionStatsMonthlyDataRow>>();
					Assert.IsNotNull(monthlyStats);
					Assert.AreEqual(0, monthlyStats.RecordCount);

					ISimpleDBOperations<SessionStatsYearlyDataRow> yearlyStats = provider.GetRequiredService<ISimpleDBOperations<SessionStatsYearlyDataRow>>();
					Assert.IsNotNull(yearlyStats);
					Assert.AreEqual(0, yearlyStats.RecordCount);

					DateTime newSessionData = new DateTime(2000, 2, 12, 5, 23, 0);
					int month = 0;

					for (int k = 0; k < 100; k++)
					{
						UserSession userSession = new UserSession(-1, newSessionData.AddMonths(month).AddMinutes(k * 10), $"SN123P1{k}", $"The agent {month}", $"referrer site {month}", $"10.2.3.{k}",
							"the host", true, true, false, ReferalType.Google, false, false, "Samsung", "Galaxy S7", 0, 1, 1, "GBP", 0);

						for (int j = 0; j < 100; j++)
						{
							userSession.PageView($"/MyPages/{j}", $"Referrer {k}", false);
						}

						Assert.AreEqual(100, userSession.Pages.Count);
						Assert.AreEqual(SaveStatus.Saved, userSession.SaveStatus);
						Assert.AreEqual(100, userSession.Pages.Count);
						Assert.AreEqual(SaveStatus.RequiresSave, userSession.Pages[0].SaveStatus);

						userSessionService.Closing(userSession);

						if (k % 8 == 0)
							month++;
					}


					int i = 0;

					while (i < 100)
					{
						System.Threading.Thread.Sleep(300);

						if (sessionPageData.RecordCount == 10000)
							break;

						i++;
					}

					Assert.IsTrue(i < 100, "timed out waiting!");

					Assert.AreEqual(100, sessionData.RecordCount);
					Assert.AreEqual(100, sessionData.PrimarySequence);
					Assert.AreEqual(10000, sessionPageData.RecordCount);
					Assert.AreEqual(1, referrer.RecordCount);
					
					Assert.AreEqual(71, hourlyStats.RecordCount);
					SessionStatsHourlyDataRow hourly = hourlyStats.Select(0);
					Assert.AreEqual(1u, hourly.HumanVisits);
					Assert.AreEqual(1, hourly.UserAgents.Count);

					Assert.AreEqual(14, dailyStats.RecordCount);
					SessionStatsDailyDataRow daily = dailyStats.Select(0);
					Assert.AreEqual(3u, daily.HumanVisits);
					Assert.AreEqual(1, daily.CountryData.Count);
					Assert.AreEqual(3u, daily.CountryData["ZZ"]);
					Assert.AreEqual(1, daily.UserAgents.Count);

					Assert.AreEqual(14, weeklyStats.RecordCount);
					SessionStatsWeeklyDataRow weekly = weeklyStats.Select(0);
					Assert.AreEqual(3u, weekly.HumanVisits);
					Assert.AreEqual(1, weekly.CountryData.Count);
					Assert.AreEqual(3u, weekly.CountryData["ZZ"]);
					Assert.AreEqual(1, weekly.UserAgents.Count);

					Assert.AreEqual(14, monthlyStats.RecordCount);
					SessionStatsMonthlyDataRow monthly = monthlyStats.Select(0);
					Assert.AreEqual(3u, monthly.HumanVisits);
					Assert.AreEqual(1, monthly.CountryData.Count);
					Assert.AreEqual(3u, monthly.CountryData["ZZ"]);
					Assert.AreEqual(1, monthly.UserAgents.Count);

					Assert.AreEqual(2, yearlyStats.RecordCount);
					SessionStatsYearlyDataRow yearly = yearlyStats.Select(0);
					Assert.AreEqual(19u, yearly.HumanVisits);
					Assert.AreEqual(1, yearly.CountryData.Count);
					Assert.AreEqual(19u, yearly.CountryData["ZZ"]);
					Assert.AreEqual(3, yearly.UserAgents.Count);

					SessionStatisticsProvider sut = provider.GetRequiredService<ISessionStatisticsProvider>() as SessionStatisticsProvider;

					List<SessionHourly> sessionHourly = sut.GetHourlyData(true);
					Assert.AreEqual(0, sessionHourly.Count);
					sessionHourly = sut.GetHourlyData(false);
					Assert.AreEqual(71, sessionHourly.Count);

					List<SessionDaily> sessionDaily = sut.GetDailyData(true);
					Assert.AreEqual(0, sessionDaily.Count);
					sessionDaily = sut.GetDailyData(false);
					Assert.AreEqual(14, sessionDaily.Count);

					List<SessionWeekly> sessionWeekly = sut.GetWeeklyData(true);
					Assert.AreEqual(0, sessionWeekly.Count);
					sessionWeekly = sut.GetWeeklyData(false);
					Assert.AreEqual(14, sessionWeekly.Count);

					List<SessionMonthly> sessionMonthly = sut.GetMonthlyData(true);
					Assert.AreEqual(0, sessionMonthly.Count);
					sessionMonthly = sut.GetMonthlyData(false);
					Assert.AreEqual(14, sessionMonthly.Count);

					List<SessionYearly> sessionYearly = sut.GetYearlyData(true);
					Assert.AreEqual(0, sessionYearly.Count);
					sessionYearly = sut.GetYearlyData(false);
					Assert.AreEqual(2, sessionYearly.Count);
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
