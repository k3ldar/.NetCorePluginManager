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
 *  File: MasterCronThread.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  09/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using Middleware;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace Cron.Plugin.Classes
{
	/// <summary>
	/// Primary thread which manages and processes cron jobs
	/// </summary>
	internal sealed class MasterCronThread : ThreadManager
	{
		private const int ZeroIntervals = 0;
		private const string InvalidCronJob = "Invalid CronJob Thread";
		private readonly ICronJobProvider _cronProvider;
		private readonly List<CronThreadJob> _cronJobs = [];
		private readonly ILogger _logger;

		public MasterCronThread(ICronJobProvider cronProvider, IPluginClassesService pluginClassesService, ILogger logger)
			: base(null, TimeSpan.FromMilliseconds(100), null, 0, 10, true, true)
		{
			_cronProvider = cronProvider ?? throw new ArgumentNullException(nameof(cronProvider));
			if (pluginClassesService == null)
				throw new ArgumentNullException(nameof(pluginClassesService));

			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			HangTimeoutSpan = TimeSpan.Zero;
			ContinueIfGlobalException = true;

			foreach (ICronJob cronJob in pluginClassesService.GetPluginClasses<ICronJob>())
			{
				DateTime nextRun = CalculateNextRun(cronJob);

				if (IsOverdue(nextRun, cronJob))
					nextRun = DateTime.UtcNow.AddMinutes(-1);

				_cronJobs.Add(new CronThreadJob(cronJob, _cronProvider.GetLastRun(cronJob), nextRun));
			}
		}

		protected override bool Run(object parameters)
		{
			DateTime now = DateTime.UtcNow;

			foreach (CronThreadJob job in _cronJobs)
			{
				if (job.NextRun < now)
				{
					job.NextRun = CalculateNextRun(job.CronJob);
					CronThread jobThread = new CronThread(job.CronJob, this);
					jobThread.ExceptionRaised += JobThread_ExceptionRaised;
					jobThread.ThreadCancelRequested += JobThread_ThreadCancelRequested;
					jobThread.ThreadStarting += JobThread_ThreadStarting;
					jobThread.ThreadFinishing += JobThread_ThreadFinishing;
					ThreadManager.ThreadStart(jobThread, $"Cron Job - {job.CronJob.Name} : {job.CronJob.JobId}", System.Threading.ThreadPriority.Normal, true);
					job.NextRun = CalculateNextRun(job.CronJob);
					_cronProvider.SetLastRun(job.CronJob, DateTime.UtcNow);
				}
			}

			return !base.HasCancelled();
		}

		private void JobThread_ThreadFinishing(object sender, Shared.ThreadManagerEventArgs e)
		{
			if (e.Thread is CronThread cronThread)
			{
				DetachCronThreadEvents(cronThread);
				_logger.AddToLog(PluginManager.LogLevel.Information, $"Cron: {cronThread.Name} has finished");
			}
			else
			{
				_logger.AddToLog(PluginManager.LogLevel.Error, InvalidCronJob);
			}
		}

		private void JobThread_ThreadStarting(object sender, Shared.ThreadManagerEventArgs e)
		{
			if (e.Thread is CronThread cronThread)
			{
				_logger.AddToLog(PluginManager.LogLevel.Information, $"Cron: {cronThread.Name} is starting");
			}
			else
			{
				_logger.AddToLog(PluginManager.LogLevel.Error, InvalidCronJob);
			}
		}

		private void JobThread_ThreadCancelRequested(object sender, Shared.ThreadManagerEventArgs e)
		{
			if (e.Thread is CronThread cronThread)
			{
				_logger.AddToLog(PluginManager.LogLevel.Information, $"Cron: {cronThread.Name} cancel requested");
			}
			else
			{
				_logger.AddToLog(PluginManager.LogLevel.Error, InvalidCronJob);
			}
		}

		private void JobThread_ExceptionRaised(object sender, Shared.ThreadManagerExceptionEventArgs e)
		{
			if (e.Thread is CronThread cronThread)
			{
				DetachCronThreadEvents(cronThread);
				_logger.AddToLog(PluginManager.LogLevel.Error, $"Cron: {cronThread.Name} error", e.Error);
			}
			else
			{
				_logger.AddToLog(PluginManager.LogLevel.Error, InvalidCronJob);
			}
		}

		private void DetachCronThreadEvents(CronThread cronThread)
		{
			cronThread.ExceptionRaised -= JobThread_ExceptionRaised;
			cronThread.ThreadCancelRequested -= JobThread_ThreadCancelRequested;
			cronThread.ThreadStarting -= JobThread_ThreadStarting;
			cronThread.ThreadFinishing -= JobThread_ThreadFinishing;
		}

		public static DateTime CalculateNextRun(ICronJob cronJob)
		{
			if (cronJob is null)
				throw new ArgumentNullException(nameof(cronJob));

			if (cronJob.StartDateTime.Kind != DateTimeKind.Utc)
				throw new InvalidOperationException("Cron Start date must be of type Utc");

			if (cronJob.StartDateTime > cronJob.Settings.Expires)
				throw new InvalidOperationException("Job expires before it starts");

			DateTime now = DateTime.UtcNow;

			if (!cronJob.Settings.Enabled || cronJob.Settings.Expires < now)
				return DateTime.MaxValue;

			DateTime lastRun = cronJob.StartDateTime;
			TimeSpan diff = now - lastRun;

			if (diff.TotalMilliseconds < 0)
				return lastRun;

			DateTime nextRun = lastRun;
			int intervalsPassed = CalculateIntervalsPassed(cronJob.Settings, now, lastRun, diff);

			nextRun = AddInterval(nextRun, cronJob.Settings.RepeatFrequencyType, intervalsPassed * cronJob.Settings.RepeatFrequency);

			while (nextRun <= now)
			{
				nextRun = AddInterval(nextRun, cronJob.Settings.RepeatFrequencyType, cronJob.Settings.RepeatFrequency);
			}

			return nextRun;
		}

		public static bool IsOverdue(DateTime lastRun, ICronJob cronJob)
		{
			if (cronJob == null)
				throw new ArgumentNullException(nameof(cronJob));

			if (cronJob.StartDateTime > DateTime.UtcNow)
				return false;

			DateTime nextRun = CalculateNextRun(cronJob);
			DateTime previousRun = AddInterval(nextRun, cronJob.Settings.RepeatFrequencyType, 0 - cronJob.Settings.RepeatFrequency);

			return nextRun > DateTime.UtcNow && previousRun > lastRun;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static int CalculateIntervalsPassed(ICronJobSettings cronJobSettings, DateTime now, DateTime lastRun, TimeSpan diff)
		{
			if (cronJobSettings.RepeatFrequency > 0 || cronJobSettings.RepeatFrequencyType != CronRepeatFrequencyType.None)
			{
				switch (cronJobSettings.RepeatFrequencyType)
				{
					case CronRepeatFrequencyType.Minute:
						return (int)Math.Ceiling(diff.TotalMinutes / cronJobSettings.RepeatFrequency);

					case CronRepeatFrequencyType.Hour:
						return (int)Math.Ceiling(diff.TotalHours / cronJobSettings.RepeatFrequency);

					case CronRepeatFrequencyType.Day:
						return (int)Math.Ceiling(diff.TotalDays / cronJobSettings.RepeatFrequency);

					case CronRepeatFrequencyType.Week:
						return (int)Math.Ceiling(diff.TotalDays * 7 / cronJobSettings.RepeatFrequency);

					case CronRepeatFrequencyType.Month:
						return (int)Math.Ceiling(Convert.ToDouble(((now.Year - lastRun.Year) * 12) + now.Month - lastRun.Month) / cronJobSettings.RepeatFrequency);

				}
			}

			return ZeroIntervals;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static DateTime AddInterval(DateTime nextRun, CronRepeatFrequencyType repeatFrequencyType, int intervalsPassed)
		{
			try
			{
				switch (repeatFrequencyType)
				{
					case CronRepeatFrequencyType.Minute:
						return nextRun.AddMinutes(intervalsPassed);

					case CronRepeatFrequencyType.Hour:
						return nextRun.AddHours(intervalsPassed);

					case CronRepeatFrequencyType.Day:
						return nextRun.AddDays(intervalsPassed);

					case CronRepeatFrequencyType.Week:
						return nextRun.AddDays(intervalsPassed * 7);

					case CronRepeatFrequencyType.Month:
						return nextRun.AddMonths(intervalsPassed);
				}
			}
			catch (ArgumentOutOfRangeException)
			{
				// unable to add/remove, return next run value
				return nextRun;
			}

			throw new InvalidOperationException("Interval not specified");
		}
	}
}
