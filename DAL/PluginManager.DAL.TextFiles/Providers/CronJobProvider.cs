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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: CronProvider.cs
 *
 *  Purpose:  ICronProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;

using PluginManager.DAL.TextFiles.Tables;

using SharedPluginFeatures;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
	internal sealed class CronJobProvider : ICronJobProvider
	{
		private readonly ISimpleDBOperations<CronJobDataRow> _cronJobSettings;

		public CronJobProvider(ISimpleDBOperations<CronJobDataRow> cronJobSettings)
		{
			_cronJobSettings = cronJobSettings ?? throw new ArgumentNullException(nameof(cronJobSettings));
		}

		public DateTime GetLastRun(ICronJob cronJob)
		{
			CronJobDataRow cronJobDataRow = GetOrCreateCronJobDataRow(cronJob);

			return new DateTime(cronJobDataRow.LastRunTicks, DateTimeKind.Utc);
		}

		public void SetLastRun(ICronJob cronJob, DateTime lastRun)
		{
			CronJobDataRow cronJobDataRow = GetOrCreateCronJobDataRow(cronJob);
			cronJobDataRow.LastRunTicks = lastRun.Ticks;
			_cronJobSettings.Update(cronJobDataRow);
		}

		private CronJobDataRow GetOrCreateCronJobDataRow(ICronJob cronJob)
		{
			CronJobDataRow cronJobDataRow = _cronJobSettings.Select().FirstOrDefault(cj => cj.JobId.Equals(cronJob.JobId) && cj.Name.Equals(cronJob.Name));

			if (cronJobDataRow == null)
			{
				cronJobDataRow = new()
				{
					JobId = cronJob.JobId,
					Name = cronJob.Name,
					UpdatedTicks = DateTime.MinValue.Ticks
				};

				_cronJobSettings.Insert(cronJobDataRow);
			}

			return cronJobDataRow;
		}
	}
}
