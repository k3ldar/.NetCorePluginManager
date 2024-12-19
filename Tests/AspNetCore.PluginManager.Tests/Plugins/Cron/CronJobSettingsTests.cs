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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.Cron
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	[TestCategory("Cron")]
	public sealed class CronJobSettingsTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Construct_MaximumRuntimeLessThanZero_Throws_ArgumentOutOfRangeException()
		{
			new CronJobSettings(TimeSpan.FromSeconds(-1), CronRepeatFrequencyType.None, 0);
		}

		[TestMethod]
		public void Construct_RepeatFrequencyLessThanOne_RepeatFrequencyIsNone_Throws_ArgumentOutOfRangeException()
		{
			CronJobSettings sut = new CronJobSettings(TimeSpan.FromSeconds(0), CronRepeatFrequencyType.None, 0);
			Assert.IsNotNull(sut);
			Assert.IsTrue(sut.Enabled);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Construct_RepeatFrequencyLessThanOne_RepeatFrequencyNotNone_Throws_ArgumentOutOfRangeException()
		{
			new CronJobSettings(TimeSpan.FromSeconds(5), CronRepeatFrequencyType.Day, 0);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void Construct_ExpireDateIsNotUtc_Throws_ArgumentOutOfRangeException()
		{
			new CronJobSettings(true, TimeSpan.FromSeconds(5), CronRepeatFrequencyType.Day, 1, DateTime.Now.AddDays(5));
		}

		[TestMethod]
		public void Construct_Disabled_CreatesInstance()
		{
			CronJobSettings sut = new CronJobSettings(false, TimeSpan.FromSeconds(0), CronRepeatFrequencyType.None, 0, DateTime.UtcNow.AddDays(100));
			Assert.IsNotNull(sut);
		}

		[TestMethod]
		public void Construct_ValidInstance()
		{
			CronJobSettings sut = new CronJobSettings(true, TimeSpan.FromSeconds(180), CronRepeatFrequencyType.Month, 1, DateTime.UtcNow.AddDays(100));
			Assert.IsNotNull(sut);
			Assert.IsTrue(sut.Enabled);
			Assert.AreEqual(TimeSpan.FromSeconds(180), sut.MaximumRunTime);
			Assert.AreEqual(CronRepeatFrequencyType.Month, sut.RepeatFrequencyType);
			Assert.AreEqual(1, sut.RepeatFrequency);
			Assert.AreEqual(DateTime.UtcNow.AddDays(100).Date, sut.Expires.Date);
		}
	}
}
