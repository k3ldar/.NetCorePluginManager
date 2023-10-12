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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: PageViewsDataRow.cs
 *
 *  Purpose:  Table definition for page view data
 *
 *  Date        Name                Reason
 *  02/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tables
{
	[Table(Constants.DomainSessions, Constants.TableNamePageViews, CompressionType.None, CachingStrategy.SlidingMemory, WriteStrategy.Lazy, SlidingMemoryTimeoutMilliseconds = 300)]
	internal class PageViewsDataRow : TableRowDefinition
	{
		private string _hash;
		private string _url;
		private int _year;
		private int _month;
		private uint _humanVisits;
		private uint _botVisits;
		private uint _bounceCount;
		private double _totalTime;

		public string Hash
		{
			get => _hash;

			set
			{
				if (Immutable)
					throw new InvalidOperationException();

				if (value == _hash)
					return;

				_hash = value;
				Update();
			}
		}

		public string Url
		{
			get => _url;

			set
			{
				if (value == _url)
					return;

				_url = value;
				Update();
			}
		}

		public int Year
		{
			get => _year;

			set
			{
				if (value == _year)
					return;

				_year = value;
				Update();
			}
		}

		public int Month
		{
			get => _month;

			set
			{
				if (value == _month)
					return;

				_month = value;
				Update();
			}
		}

		public uint HumanVisits
		{
			get => _humanVisits;

			set
			{
				if (value == _humanVisits)
					return;

				_humanVisits = value;
				Update();
			}
		}

		public uint BotVisits
		{
			get => _botVisits;

			set
			{
				if (value == _botVisits)
					return;

				_botVisits = value;
				Update();
			}
		}

		public uint BounceCount
		{
			get => _bounceCount;

			set
			{
				if (value == _bounceCount)
					return;

				_bounceCount = value;
				Update();
			}
		}

		public double TotalTime
		{
			get => _totalTime;

			set
			{
				if (value == _totalTime)
					return;

				_totalTime = value;
				Update();
			}
		}
	}
}
