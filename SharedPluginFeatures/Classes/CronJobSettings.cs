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
 *  Product:  SharedPluginFeatures
 *  
 *  File: CronJobSettings.cs
 *
 *  Purpose:  Default implementation of ICronJobSettings
 *
 *  Date        Name                Reason
 *  11/07/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Implementation for ICronJobSettings
	/// </summary>
	public sealed class CronJobSettings : ICronJobSettings
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="maximumRunTime"></param>
		/// <param name="repeatFrequencyType"></param>
		/// <param name="repeatFrequency"></param>
		public CronJobSettings(TimeSpan maximumRunTime,
			CronRepeatFrequencyType repeatFrequencyType,
			int repeatFrequency)
			: this(true, maximumRunTime, repeatFrequencyType, repeatFrequency, DateTime.UtcNow.AddYears(100))
		{

		}

		/// <summary>
		/// Default Constructor
		/// </summary>
		public CronJobSettings(bool enabled, TimeSpan maximumRunTime, 
			CronRepeatFrequencyType repeatFrequencyType,
			int repeatFrequency,
			DateTime expires)
		{
			if (maximumRunTime < TimeSpan.Zero)
				throw new ArgumentOutOfRangeException(nameof(maximumRunTime));

			if (repeatFrequencyType != CronRepeatFrequencyType.None && repeatFrequency < 1)
				throw new ArgumentOutOfRangeException(nameof(repeatFrequency));
			
			if (expires.Kind != DateTimeKind.Utc)
				throw new ArgumentOutOfRangeException(nameof(expires));

			Enabled = enabled;
			MaximumRunTime = maximumRunTime;
			RepeatFrequencyType = repeatFrequencyType;
			RepeatFrequency = repeatFrequency;
			Expires = expires;
		}

		/// <summary>
		/// Indicates whether the job is active or not
		/// </summary>
		public bool Enabled { get; }

		/// <summary>
		/// Maximum run time for task
		/// </summary>
		public TimeSpan MaximumRunTime { get; }

		/// <summary>
		/// Repeat frequency type
		/// </summary>
		public CronRepeatFrequencyType RepeatFrequencyType { get; }

		/// <summary>
		/// Repeat frequency
		/// </summary>
		public int RepeatFrequency { get; }

		/// <summary>
		/// Expire date time or null for no expirery
		/// </summary>
		public DateTime Expires { get; }
	}
}
