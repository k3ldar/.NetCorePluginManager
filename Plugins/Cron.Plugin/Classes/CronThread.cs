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
 *  Product:  Cron.Plugin
 *  
 *  File: CronThread.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Shared.Classes;

using SharedPluginFeatures;

namespace Cron.Plugin.Classes
{
	internal sealed class CronThread : ThreadManager
	{
		public CronThread(ICronJob cronJob, ThreadManager parent)
			: base(cronJob, TimeSpan.Zero, parent, 0, 1000, true, true)
		{
			if (cronJob == null)
				throw new ArgumentNullException(nameof(cronJob));

			HangTimeoutSpan = cronJob.Settings.MaximumRunTime;
			ContinueIfGlobalException = false;
			CronJob = cronJob;
		}

		protected override bool Run(object parameters)
		{
			if (parameters is not ICronJob cron)
				throw new ArgumentException("invalid cron job");

			cron.Execute();

			return false;
		}

		internal ICronJob CronJob { get; }
	}
}
