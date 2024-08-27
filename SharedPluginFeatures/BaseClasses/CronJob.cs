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
 *  Product:  Cron.Plugin
 *  
 *  File: CronJob.cs
 *
 *  Purpose:  Base class for any cron job
 *
 *  Date        Name                Reason
 *  11/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures.BaseClasses
{
	/// <summary>
	/// Represents a cron job which is run daily
	/// </summary>
	public abstract class CronJob : ICronJob
	{
		/// <summary>
		/// Unique guid identifier for the job
		/// </summary>
		public abstract Guid JobId { get; }

		/// <summary>
		/// Name of job
		/// </summary>
		public abstract string Name { get; }

		/// <summary>
		/// Cron job settings
		/// </summary>
		public abstract ICronJobSettings Settings { get; }

		/// <summary>
		/// Priority for the cron job
		/// </summary>
		public abstract CronPriority Priority { get; }

		/// <summary>
		/// Date and time the job starts
		/// </summary>
		public abstract DateTime StartDateTime { get; }

		/// <summary>
		/// Method called by cron manager for job to execute
		/// </summary>
		public abstract void Execute();
	}
}
