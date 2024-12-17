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

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
	internal class MockCronJobSettings : ICronJobSettings
	{
		public MockCronJobSettings(bool enabled = true,
			CronRepeatFrequencyType repeatFrequencyType = CronRepeatFrequencyType.Day,
			int repeatFrequency = 1)
		{
			Enabled = enabled;
			RepeatFrequencyType = repeatFrequencyType;
			RepeatFrequency = repeatFrequency;
		}

		public bool Enabled { get; set; }

		public TimeSpan MaximumRunTime { get; set; }

		public CronRepeatFrequencyType RepeatFrequencyType { get; set; }

		public int RepeatFrequency { get; set; }

		public DateTime Expires { get; set; } = DateTime.MaxValue;
	}
}
