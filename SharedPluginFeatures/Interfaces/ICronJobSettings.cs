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
 *  Product:  SharedPluginFeatues
 *  
 *  File: ICronJobSettings.cs
 *
 *  Purpose:  Interface for settings used for each cron job
 *
 *  Date        Name                Reason
 *  11/07/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Settings applied to each cron job
	/// </summary>
	public interface ICronJobSettings
	{
		/// <summary>
		/// Indicates whether the job is enabled or not
		/// </summary>
		bool Enabled { get; }

		/// <summary>
		/// Maximum amount of time the job can run before being closed
		/// </summary>
		TimeSpan MaximumRunTime { get; }

		/// <summary>
		/// Frequency at which the job repeats
		/// </summary>
		CronRepeatFrequencyType RepeatFrequencyType { get; }

		/// <summary>
		/// Frequency value at which the job repeats, used in conjunction with <seealso cref="RepeatFrequencyType"/>
		/// </summary>
		int RepeatFrequency { get; }

		/// <summary>
		/// Date and time job expires
		/// </summary>
		DateTime Expires { get; }
	}
}
