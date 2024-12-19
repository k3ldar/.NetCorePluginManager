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
 *  File: DefaultCronProvider.cs
 *
 *  Purpose:  Cron provider data store
 *
 *  Date        Name                Reason
 *  11/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;

using Shared.Classes;

using SharedPluginFeatures;

namespace Cron.Plugin.Provider
{
	internal sealed class DefaultCronProvider : ICronJobProvider
	{
		private const string CronDataLocation = "CronData";
		private const string CronDataFile = "CronData.dat";

		private readonly object _lock = new object();
		private readonly List<DefaultCronData> _cronData;
		private readonly ISaveData _saveData;

		public DefaultCronProvider(ILoadData loadData, ISaveData saveData)
		{
			if (loadData == null)
				throw new ArgumentNullException(nameof(loadData));

			_cronData = loadData.Load<List<DefaultCronData>>(CronDataLocation, CronDataFile) ?? new();
			_saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
		}

		public DateTime GetLastRun(ICronJob cronJob)
		{
			if (cronJob == null)
				throw new ArgumentNullException(nameof(cronJob));

			using (TimedLock timedLoc = TimedLock.Lock(_lock))
			{
				DefaultCronData savedCronJob = _cronData.Find(cd => cd.Guid.Equals(cronJob.JobId) && cd.Name.Equals(cronJob.Name));

				if (savedCronJob == null)
					return DateTime.MinValue;

				return new DateTime(savedCronJob.LastRunTicks, DateTimeKind.Utc);
			}
		}

		public void SetLastRun(ICronJob cronJob, DateTime lastRun)
		{
			if (cronJob == null)
				throw new ArgumentNullException(nameof(cronJob));

			if (lastRun.Kind != DateTimeKind.Utc)
				throw new InvalidOperationException("lastRun must be of kind Utc");

			using (TimedLock timedLoc = TimedLock.Lock(_lock))
			{
				DefaultCronData savedCronJob = _cronData.Find(cd => cd.Guid.Equals(cronJob.JobId) && cd.Name.Equals(cronJob.Name));

				savedCronJob ??= new(cronJob);
				savedCronJob.LastRunTicks = DateTime.UtcNow.Ticks;
				_saveData.Save(_cronData, CronDataLocation, CronDataFile);
			}
		}
	}
}
