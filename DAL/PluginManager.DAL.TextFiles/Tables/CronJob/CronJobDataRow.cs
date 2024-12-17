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
 *  File: CronJobDataRow.cs
 *
 *  Purpose:  Row definition for Table for cron jobs
 *
 *  Date        Name                Reason
 *  26/08/2024  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.TableNameCronJobs, CompressionType.None, CachingStrategy.SlidingMemory, WriteStrategy.Forced)]
	internal sealed class CronJobDataRow : TableRowDefinition
	{
		private Guid _jobId;
		private string _name;
		private long _lastRunTicks;

		public Guid JobId
		{
			get => _jobId;

			set
			{
				if (_jobId == value)
					return;

				_jobId = value;
				Update();
			}
		}

		public string Name
		{
			get => _name;

			set
			{
				if (_name == value)
					return;

				_name = value;
				Update();
			}
		}

		public long LastRunTicks
		{
			get => _lastRunTicks;

			set
			{
				if (_lastRunTicks == value)
					return;

				_lastRunTicks = value;
				Update();
			}
		}
	}
}
