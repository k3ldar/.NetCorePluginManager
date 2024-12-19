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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: CronJobProviderTests.cs
 *
 *  Purpose:  Download provider test for text based storage
 *
 *  Date        Name                Reason
 *  27/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;
using SharedPluginFeatures.BaseClasses;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class CronJobProviderTests : BaseProviderTests
	{
		[TestInitialize]
		public void Setup()
		{
			ThreadManager.Initialise();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_NullSimpleDB_Throws_ArgumentNullException()
		{
			_ = new CronJobProvider(null);
		}

		[TestMethod]
		public void GetLastRun_JobNotFound_JobAddedToTable_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out _);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					CronJobProvider sut = (CronJobProvider)provider.GetService<ICronJobProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<CronJobDataRow> cronJobTable = provider.GetService<ISimpleDBOperations<CronJobDataRow>>();
					Assert.IsNotNull(cronJobTable);

					ICronJob cronJob = new TestCronJob(Guid.NewGuid(), "Test new cron job");
					DateTime lastRun = sut.GetLastRun(cronJob);

					Assert.AreEqual(DateTime.MinValue, lastRun);
					Assert.AreEqual(DateTimeKind.Utc, lastRun.Kind);

					CronJobDataRow savedCronData = cronJobTable.Select(cj => cj.Name.Equals(cronJob.Name) && cj.JobId.Equals(cronJob.JobId)).First();
					Assert.IsNotNull(savedCronData);
					Assert.AreEqual(cronJob.Name, savedCronData.Name);
					Assert.AreEqual(cronJob.JobId, savedCronData.JobId);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetLastRun_JobFound_JobLastRun_ReturnsLastRunDateTime()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				DateTime lastRunMain = DateTime.UtcNow.AddMinutes(23);
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out _);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					CronJobProvider sut = (CronJobProvider)provider.GetService<ICronJobProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<CronJobDataRow> cronJobTable = provider.GetService<ISimpleDBOperations<CronJobDataRow>>();
					Assert.IsNotNull(cronJobTable);

					ICronJob cronJob = new TestCronJob(Guid.NewGuid(), "Test new cron job");

					cronJobTable.Insert(new CronJobDataRow()
					{
						JobId = cronJob.JobId,
						Name = cronJob.Name,
						LastRunTicks = lastRunMain.Ticks,
					});

					DateTime lastRun = sut.GetLastRun(cronJob);

					Assert.AreEqual(lastRunMain.Ticks, lastRun.Ticks);
					Assert.AreEqual(DateTimeKind.Utc, lastRun.Kind);

					CronJobDataRow savedCronData = cronJobTable.Select(cj => cj.Name.Equals(cronJob.Name) && cj.JobId.Equals(cronJob.JobId)).First();
					Assert.IsNotNull(savedCronData);
					Assert.AreEqual(cronJob.Name, savedCronData.Name);
					Assert.AreEqual(cronJob.JobId, savedCronData.JobId);
					Assert.AreEqual(lastRunMain.Ticks, savedCronData.LastRunTicks);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void SetLastRun_JobNotFound_JobAddedToTable_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				DateTime lastRunMain = DateTime.UtcNow.AddMinutes(-14);
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out _);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					CronJobProvider sut = (CronJobProvider)provider.GetService<ICronJobProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<CronJobDataRow> cronJobTable = provider.GetService<ISimpleDBOperations<CronJobDataRow>>();
					Assert.IsNotNull(cronJobTable);

					ICronJob cronJob = new TestCronJob(Guid.NewGuid(), "Test new cron job");
					sut.SetLastRun(cronJob, lastRunMain);

					CronJobDataRow savedCronData = cronJobTable.Select(cj => cj.Name.Equals(cronJob.Name) && cj.JobId.Equals(cronJob.JobId)).First();
					Assert.IsNotNull(savedCronData);
					Assert.AreEqual(cronJob.Name, savedCronData.Name);
					Assert.AreEqual(cronJob.JobId, savedCronData.JobId);
					Assert.AreEqual(lastRunMain.Ticks, savedCronData.LastRunTicks);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void SetLastRun_JobFound_JobLastRun_ReturnsLastRunDateTime()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				DateTime lastRunMain = DateTime.UtcNow.AddYears(-1);
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out _);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					CronJobProvider sut = (CronJobProvider)provider.GetService<ICronJobProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<CronJobDataRow> cronJobTable = provider.GetService<ISimpleDBOperations<CronJobDataRow>>();
					Assert.IsNotNull(cronJobTable);

					ICronJob cronJob = new TestCronJob(Guid.NewGuid(), "Test new cron job");

					cronJobTable.Insert(new CronJobDataRow()
					{
						JobId = cronJob.JobId,
						Name = cronJob.Name,
						LastRunTicks = DateTime.MinValue.Ticks,
					});

					sut.SetLastRun(cronJob, lastRunMain);
					DateTime lastRun = sut.GetLastRun(cronJob);

					Assert.AreEqual(lastRunMain.Ticks, lastRun.Ticks);
					Assert.AreEqual(DateTimeKind.Utc, lastRun.Kind);

					CronJobDataRow savedCronData = cronJobTable.Select(cj => cj.Name.Equals(cronJob.Name) && cj.JobId.Equals(cronJob.JobId)).First();
					Assert.IsNotNull(savedCronData);
					Assert.AreEqual(cronJob.Name, savedCronData.Name);
					Assert.AreEqual(cronJob.JobId, savedCronData.JobId);
					Assert.AreEqual(lastRunMain.Ticks, savedCronData.LastRunTicks);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[ExcludeFromCodeCoverage]
		private sealed class TestCronJob : CronJob
		{
			public TestCronJob(Guid jobId, string name)
			{
				JobId = jobId;
				Name = name;
			}

			public override Guid JobId { get; }

			public override string Name { get; }

			public override ICronJobSettings Settings => throw new NotImplementedException();

			public override CronPriority Priority => throw new NotImplementedException();

			public override DateTime StartDateTime => throw new NotImplementedException();

			public override void Execute()
			{
				throw new NotImplementedException();
			}
		}
	}
}
