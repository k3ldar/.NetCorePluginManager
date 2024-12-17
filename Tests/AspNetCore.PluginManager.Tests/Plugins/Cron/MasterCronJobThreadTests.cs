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
 *  Copyright (c) 2024 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *
 *  Purpose:  Unit Tests
 *
 *  Date        Name                Reason
 *  11/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Cron.Plugin.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.Cron
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	[TestCategory("Cron")]
	public class MasterCronJobThreadTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_NullPluginClassServices_Throws_ArgumentNullException()
		{
			MasterCronThread sut = new MasterCronThread(new MockICronProvider(), null, new MockLogger());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_NullCronProvider_Throws_ArgumentNullException()
		{
			MasterCronThread sut = new MasterCronThread(null, new MockPluginClassesService(), new MockLogger());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_NullLogger_Throws_ArgumentNullException()
		{
			MasterCronThread sut = new MasterCronThread(new MockICronProvider(), new MockPluginClassesService(), null);
		}

		[TestMethod]
		public void CalculateNextRun_JobIsDisabled_Returns_DateTimeMaxValue()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(false));
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);
			Assert.AreEqual(DateTime.MaxValue, nextRun);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CalculateNextRun_CronJobDateIsLocal_Throws_InvalidOperationException()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Local);
			MasterCronThread.CalculateNextRun(cronJob);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CalculateNextRun_CronJobDateIsUnspecified_Throws_InvalidOperationException()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Unspecified);
			MasterCronThread.CalculateNextRun(cronJob);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Day_CalculateNextRun_JobExpiresBeforeFirstRun_Throws_InvalidOperationException()
		{
			DateTime expectedRun = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);

			MockCronJobSettings settings = new MockCronJobSettings()
			{
				Expires = DateTime.UtcNow,
			};

			MockCronJob cronJob = new MockCronJob(settings);
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);
		}

		[TestMethod]
		public void Day_CalculateNextRun_JobIsExpired_Returns_DateTime_MaxValue()
		{
			DateTime expectedRun = DateTime.MaxValue;

			MockCronJobSettings settings = new MockCronJobSettings()
			{
				Expires = DateTime.UtcNow.AddMinutes(-1),
			};

			MockCronJob cronJob = new MockCronJob(settings);
			cronJob.StartDateTime = settings.Expires.AddDays(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Day_CalculateNextRun_StartTimeInFuture_CalculatesCorrectStartTime()
		{
			DateTime expectedRun = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);

			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Day_CalculateNextRun_StartTimeInPast_FrequencyOne_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddDays(1);
			cronJob.StartDateTime = now.AddDays(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Day_CalculateNextRun_StartTimeInPast_FrequencySixteen_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Day, 16));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddDays(15);
			cronJob.StartDateTime = now.AddDays(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Minute_CalculateNextRun_StartTimeInFuture_CalculatesCorrectStartTime()
		{
			DateTime expectedRun = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);

			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Minute, 1));
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Minute_CalculateNextRun_StartTimeInPast_FrequencyOne_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Minute, 1));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddMinutes(1);
			cronJob.StartDateTime = now.AddMinutes(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Minute_CalculateNextRun_StartTimeInPast_FrequencyTen_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Minute, 10));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddMinutes(9);
			cronJob.StartDateTime = now.AddMinutes(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Hour_CalculateNextRun_StartTimeInFuture_CalculatesCorrectStartTime()
		{
			DateTime expectedRun = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Hour, 1));
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Hour_CalculateNextRun_StartTimeInPast_FrequencyOne_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Hour, 1));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddHours(1);
			cronJob.StartDateTime = now.AddHours(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Hour_CalculateNextRun_StartTimeInPast_FrequencyTen_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Hour, 10));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddHours(9);
			cronJob.StartDateTime = now.AddHours(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Week_CalculateNextRun_StartTimeInFuture_CalculatesCorrectStartTime()
		{
			DateTime expectedRun = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Week, 1));
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Week_CalculateNextRun_StartTimeInPast_FrequencyOne_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Week, 1));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddHours(-1).AddDays(7);
			cronJob.StartDateTime = now.AddHours(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Week_CalculateNextRun_StartTimeInPast_FrequencyTen_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Week, 10));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddHours(-1).AddDays(70);
			cronJob.StartDateTime = now.AddHours(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Month_CalculateNextRun_StartTimeInFuture_CalculatesCorrectStartTime()
		{
			DateTime expectedRun = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Week, 1));
			cronJob.StartDateTime = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Month_CalculateNextRun_StartTimeInPast_FrequencyOne_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Month, 1));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddHours(-1).AddMonths(1);
			cronJob.StartDateTime = now.AddHours(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		public void Month_CalculateNextRun_StartTimeInPast_FrequencyTen_CalculatesCorrectStartTime()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings(true, CronRepeatFrequencyType.Month, 10));
			DateTime now = DateTime.UtcNow;
			DateTime expectedRun = now.AddHours(-1).AddMonths(10);
			cronJob.StartDateTime = now.AddHours(-1);
			DateTime nextRun = MasterCronThread.CalculateNextRun(cronJob);

			Assert.AreEqual(expectedRun, nextRun);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsOverdue_NullCronJob_Throws_ArgumentNullException()
		{
			DateTime expectedRun = new DateTime(2052, 1, 1, 12, 17, 34, DateTimeKind.Utc);

			bool isOverdue = MasterCronThread.IsOverdue(DateTime.MinValue, null);

			Assert.IsTrue(isOverdue);
		}

		[TestMethod]
		public void IsOverdue_CronjobInitiallyScheduledForFutureDate_Returns_False()
		{
			DateTime expectedRun = new DateTime(2352, 1, 1, 12, 17, 34, DateTimeKind.Utc);

			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());
			cronJob.StartDateTime = new DateTime(2352, 1, 1, 12, 17, 34, DateTimeKind.Utc);
			bool isOverdue = MasterCronThread.IsOverdue(DateTime.MinValue, cronJob);

			Assert.IsFalse(isOverdue);
		}

		[TestMethod]
		public void IsOverdue_LastRunIsMinDateTime_Returns_True()
		{
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());
			cronJob.StartDateTime = DateTime.UtcNow.AddDays(-5);
			bool isOverdue = MasterCronThread.IsOverdue(DateTime.MinValue, cronJob);

			Assert.IsTrue(isOverdue);
		}
	}
}
