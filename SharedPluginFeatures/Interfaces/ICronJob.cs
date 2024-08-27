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
 *  Product:  SharedPluginFeatues
 *  
 *  File: ICronJob.cs
 *
 *  Purpose:  Basic cron job interface
 *
 *  Date        Name                Reason
 *  09/07/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace SharedPluginFeatures
{
	/// <summary>
	/// Basic cron job instance
	/// </summary>
	public interface ICronJob
	{
		/// <summary>
		/// Unique guid for job
		/// </summary>
		Guid JobId { get; }

		/// <summary>
		/// Name of cron job
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Advanced settings for job
		/// </summary>
		ICronJobSettings Settings { get; }

		/// <summary>
		/// Priority for cron job
		/// </summary>
		CronPriority Priority { get; }

		/// <summary>
		/// Date and time job should start
		/// </summary>
		DateTime StartDateTime { get; }

		/// <summary>
		/// Executes/runs the cron job
		/// </summary>
		void Execute();
	}
}
