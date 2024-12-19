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

namespace AspNetCore.PluginManager.Tests.Plugins.Cron
{
	[ExcludeFromCodeCoverage]
	[TestClass]
	[TestCategory("Cron")]
	public sealed class CronThreadTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_NullCronJob_Throws_ArgumentNullException()
		{
			new CronThread(null, null);
		}

		[TestMethod]
		public void Construct_ValidInstance_Success()
		{
			MockCronJob mockCronJob = new(new MockCronJobSettings());
			CronThread sut = new(mockCronJob, null);

			Assert.IsNotNull(sut);
			Assert.AreSame(mockCronJob, sut.CronJob);
		}
	}
}
