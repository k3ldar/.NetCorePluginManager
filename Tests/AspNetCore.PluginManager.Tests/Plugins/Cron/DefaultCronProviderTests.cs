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

using Cron.Plugin.Provider;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AspNetCore.PluginManager.Tests.Plugins.Cron
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	[TestCategory("Cron")]
	public sealed class DefaultCronProviderTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidLoadData_Throws_ArgumentNullException()
		{
			new DefaultCronProvider(null, new MockSaveData());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidSaveData_Throws_ArgumentNullException()
		{
			new DefaultCronProvider(new MockLoadData(), null);
		}

		[TestMethod]
		public void Construct_ValidInstance_Success()
		{
			MockLoadData mockLoadData = new MockLoadData();
			DefaultCronProvider sut = new DefaultCronProvider(mockLoadData, new MockSaveData());
			Assert.IsNotNull(sut);
			Assert.IsTrue(mockLoadData.LoadDataCalled);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetLastRun_NullCronJob_Throws_ArgumentNullException()
		{
			DefaultCronProvider sut = new DefaultCronProvider(new MockLoadData(), new MockSaveData());
			sut.GetLastRun(null);
		}

		[TestMethod]
		public void GetLastRun_JobNotPreviouslySaved_Returns_DateTimeMinValue_Success()
		{
			DefaultCronProvider sut = new DefaultCronProvider(new MockLoadData(), new MockSaveData());
			DateTime lastRun = sut.GetLastRun(new MockCronJob(new MockCronJobSettings()));

			Assert.AreEqual(DateTime.MinValue, lastRun);
		}

		[TestMethod]
		public void GetLastRun_JobPreviouslySaved_Returns_LastRunDateTime_Success()
		{
			DateTime lastRunTest = DateTime.UtcNow;
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());

			DefaultCronProvider sut = new DefaultCronProvider(new MockLoadData(), new MockSaveData());
			sut.SetLastRun(cronJob, lastRunTest);
			DateTime lastRun = sut.GetLastRun(cronJob);

			Assert.AreEqual(DateTime.MinValue, lastRun);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetLastRun_NullCronJob_Throws_ArgumentNullException()
		{
			DefaultCronProvider sut = new DefaultCronProvider(new MockLoadData(), new MockSaveData());
			sut.SetLastRun(null, DateTime.UtcNow);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SetLastRun_InvalidDateNotUtc_Throws_InvalidOperationException()
		{
			DefaultCronProvider sut = new DefaultCronProvider(new MockLoadData(), new MockSaveData());
			sut.SetLastRun(new MockCronJob(new MockCronJobSettings()), DateTime.Now);
		}

		[TestMethod]
		public void SetLastRun_JobPreviouslySaved_Returns_LastRunDateTime_Success()
		{
			DateTime lastRunTest = DateTime.UtcNow.AddDays(-1);
			MockCronJob cronJob = new MockCronJob(new MockCronJobSettings());
			MockLoadData mockLoadData = new MockLoadData();
			MockSaveData mockSaveData = new MockSaveData();

			DefaultCronProvider sut = new DefaultCronProvider(mockLoadData, mockSaveData);
			sut.SetLastRun(cronJob, lastRunTest);
			DateTime lastRun = sut.GetLastRun(cronJob);

			Assert.AreEqual(DateTime.MinValue, lastRun);
			Assert.IsTrue(mockLoadData.LoadDataCalled);
			Assert.IsTrue(mockSaveData.SaveDataCalled);
		}
	}
}
