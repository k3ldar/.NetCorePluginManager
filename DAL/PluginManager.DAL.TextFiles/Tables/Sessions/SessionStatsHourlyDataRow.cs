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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: SessionStatsHourlyDataRow.cs
 *
 *  Purpose:  Hourly session statistics
 *
 *  Date        Name                Reason
 *  11/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainSessions, Constants.TableNameSessionStatsHourly, CompressionType.Brotli, CachingStrategy.SlidingMemory, WriteStrategy.Lazy, SlidingMemoryTimeoutMilliseconds = 300)]
	internal class SessionStatsHourlyDataRow : SessionStatsBaseData
	{
		private int _hour;
		private int _quarter;
		private long _dateTicks;
		private bool _isBot;

		[UniqueIndex("HourlySessionData")]
		public bool IsBot
		{
			get => _isBot;

			set
			{
				if (value == _isBot)
					return;

				_isBot = value;
				Update();
			}
		}

		[UniqueIndex("HourlySessionData")]
		public long DateTicks
		{
			get => _dateTicks;

			set
			{
				if (_dateTicks == value)
					return;

				_dateTicks = value;
				Update();
			}
		}

		public DateTime Date
		{
			get => new(_dateTicks, DateTimeKind.Utc);
		}

		[UniqueIndex("HourlySessionData")]
		public int Hour
		{
			get => _hour;

			set
			{
				if (_hour == value)
					return;

				_hour = value;
				Update();
			}
		}

		[UniqueIndex("HourlySessionData")]
		public int Quarter
		{
			get => _quarter;

			set
			{
				if (value == _quarter)
					return;

				_quarter = value;
				Update();
			}
		}
	}
}
