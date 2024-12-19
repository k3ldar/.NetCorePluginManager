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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
	[ExcludeFromCodeCoverage]
	internal class MockCronJob : ICronJob
	{
		public MockCronJob(MockCronJobSettings mockCronJobSettings)
		{
			Settings = mockCronJobSettings;
		}
		
		public Guid JobId { get; set; } = Guid.NewGuid();

		/// <summary>
		/// Name of cron job
		/// </summary>
		public string Name { get; set; } = nameof(MockCronJob);

		/// <summary>
		/// Advanced settings for job
		/// </summary>
		public ICronJobSettings Settings { get; set; }

		/// <summary>
		/// Priority for cron job
		/// </summary>
		public CronPriority Priority { get; set; }

		/// <summary>
		/// Date and time job should start
		/// </summary>
		public DateTime StartDateTime { get; set; }

		/// <summary>
		/// Executes/runs the cron job
		/// </summary>
		public void Execute()
		{

		}
	}
}
