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
 *  File: SessionStatisticsProvider.cs
 *
 *  Purpose:  Session statistics provider
 *
 *  Date        Name                Reason
 *  11/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;
using Middleware.SessionData;

using PluginManager.DAL.TextFiles.Tables;

using Shared.Classes;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
	internal class SessionStatisticsProvider : ISessionStatisticsProvider
	{
		private readonly object _lockObject = new object();
		private readonly ISimpleDBOperations<SessionStatsHourlyDataRow> _sessionDataHourly;
		private readonly ISimpleDBOperations<SessionStatsDailyDataRow> _sessionDataDaily;
		private readonly ISimpleDBOperations<SessionStatsWeeklyDataRow> _sessionDataWeekly;
		private readonly ISimpleDBOperations<SessionStatsMonthlyDataRow> _sessionDataMonthly;
		private readonly ISimpleDBOperations<SessionStatsYearlyDataRow> _sessionDataYearly;

		#region Constructors

		public SessionStatisticsProvider(ISimpleDBOperations<SessionStatsHourlyDataRow> sessionDataHourly,
			ISimpleDBOperations<SessionStatsDailyDataRow> sessionDataDaily,
			ISimpleDBOperations<SessionStatsWeeklyDataRow> sessionDataWeekly,
			ISimpleDBOperations<SessionStatsMonthlyDataRow> sessionDataMonthly,
			ISimpleDBOperations<SessionStatsYearlyDataRow> sessionDataYearly)
		{
			_sessionDataHourly = sessionDataHourly ?? throw new ArgumentNullException(nameof(sessionDataHourly));
			_sessionDataDaily = sessionDataDaily ?? throw new ArgumentNullException(nameof(sessionDataDaily));
			_sessionDataWeekly = sessionDataWeekly ?? throw new ArgumentNullException(nameof(sessionDataWeekly));
			_sessionDataMonthly = sessionDataMonthly ?? throw new ArgumentNullException(nameof(sessionDataMonthly));
			_sessionDataYearly = sessionDataYearly ?? throw new ArgumentNullException(nameof(sessionDataYearly));
		}

		#endregion Constructors

		#region ISessionStatisticsProvider Methods

		/// <summary>
		/// Retrieves daily data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionDaily> GetDailyData(bool isBot)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				return CopySessionData<SessionDaily, SessionStatsDailyDataRow>(_sessionDataDaily.Select().Where(d => d.IsBot.Equals(isBot)), isBot);
			}
		}

		/// <summary>
		/// Retrieves hourly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionHourly> GetHourlyData(bool isBot)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				return CopySessionData<SessionHourly, SessionStatsHourlyDataRow>(_sessionDataHourly.Select().Where(d => d.IsBot.Equals(isBot)), isBot);
			}
		}

		/// <summary>
		/// Retrieves weekly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionWeekly> GetWeeklyData(bool isBot)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				return CopySessionData<SessionWeekly, SessionStatsWeeklyDataRow>(_sessionDataWeekly.Select().Where(d => d.IsBot.Equals(isBot)), isBot);
			}
		}

		/// <summary>
		/// Retrieves monthly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionMonthly> GetMonthlyData(bool isBot)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				return CopySessionData<SessionMonthly, SessionStatsMonthlyDataRow>(_sessionDataMonthly.Select().Where(d => d.IsBot.Equals(isBot)), isBot);
			}
		}

		/// <summary>
		/// Retrieves yearly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionYearly> GetYearlyData(bool isBot)
		{
			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				return CopySessionData<SessionYearly, SessionStatsYearlyDataRow>(_sessionDataYearly.Select().Where(d => d.IsBot.Equals(isBot)), isBot);
			}
		}

		/// <summary>
		/// Retrieves user agent data
		/// </summary>
		/// <returns></returns>
		public List<SessionUserAgent> GetUserAgents()
		{
			List<SessionYearly> yearlySessions = CopySessionData<SessionYearly, SessionStatsYearlyDataRow>(_sessionDataYearly.Select(), false);
			return AmalgamateSessionData(yearlySessions);
		}

		#endregion ISessionStatisticsProvider Methods

		#region Private Methods

		private static List<T> CopySessionData<T, D>(IEnumerable<D> baseData, bool isBot)
			where T : SessionBaseData 
			where D : SessionStatsBaseData
		{
			List<T> Result = new List<T>();

			foreach (SessionStatsBaseData data in baseData)
			{
				T resultDataRow = (T)Activator.CreateInstance(typeof(T));

				resultDataRow.ReferBing = data.ReferBing;
				resultDataRow.ReferDirect = data.ReferDirect;
				resultDataRow.ReferFacebook = data.ReferFacebook;
				resultDataRow.ReferGoogle = data.ReferGoogle;
				resultDataRow.ReferFacebook = data.ReferFacebook;
				resultDataRow.ReferOrganic = data.ReferOrganic;
				resultDataRow.ReferOther = data.ReferOther;
				resultDataRow.ReferTwitter = data.ReferTwitter;
				resultDataRow.ReferYahoo = data.ReferYahoo;
				resultDataRow.ReferrerUnknown = data.ReferUnknown;
				resultDataRow.BotVisits = data.BotVisits;
				resultDataRow.HumanVisits = data.HumanVisits;
				resultDataRow.TotalVisits = data.TotalVisits;
				resultDataRow.MobileVisits = data.MobileVisits;
				resultDataRow.Bounced = data.Bounced;
				resultDataRow.TotalPages = data.TotalPages;
				resultDataRow.TotalSales = data.TotalSales;
				resultDataRow.Conversions = data.Conversions;

				foreach (string key in data.UserAgents.Keys)
				{
					resultDataRow.UserAgents.Add(new SessionUserAgent()
					{
						IsBot = isBot,
						Count = data.UserAgents[key],
						UserAgent = key,
					});
				}

				foreach (string key in data.CountryData.Keys)
				{
					resultDataRow.CountryData.Add(key, data.CountryData[key]);
				}
				
				if (resultDataRow is SessionDaily sessionDaily)
				{
					sessionDaily.Date = ((SessionStatsDailyDataRow)data).Date;
				}
				else if (resultDataRow is SessionHourly sessionHourly)
				{
					sessionHourly.Date = ((SessionStatsHourlyDataRow)data).Date;
					sessionHourly.Hour = ((SessionStatsHourlyDataRow)data).Hour;
					sessionHourly.Quarter = ((SessionStatsHourlyDataRow)data).Quarter;
				}
				else if (resultDataRow is SessionWeekly sessionWeekly)
				{
					sessionWeekly.Year = ((SessionStatsWeeklyDataRow)data).Year;
					sessionWeekly.Week = ((SessionStatsWeeklyDataRow)data).Week;
				}
				else if (resultDataRow is SessionMonthly sessionMonthly)
				{
					sessionMonthly.Year = ((SessionStatsMonthlyDataRow)data).Year;
					sessionMonthly.Month = ((SessionStatsMonthlyDataRow)data).Month;
				}
				else if (resultDataRow is SessionYearly sessionYearly)
				{
					sessionYearly.Year = ((SessionStatsYearlyDataRow)data).Year;
				}

				Result.Add(resultDataRow);
			}

			return Result;
		}


		private static List<SessionUserAgent> AmalgamateSessionData(List<SessionYearly> yearlySessions)
		{
			List<SessionUserAgent> Result = null;

			foreach (SessionYearly year in yearlySessions)
			{
				if (Result == null)
				{
					Result = year.UserAgents;
				}
				else
				{
					foreach (SessionUserAgent item in year.UserAgents)
					{
						SessionUserAgent returnAgent = Result
							.FirstOrDefault(r => r.UserAgent.Equals(item.UserAgent) && r.IsBot == item.IsBot);

						if (returnAgent == null)
						{
							returnAgent = new SessionUserAgent()
							{
								UserAgent = item.UserAgent,
								IsBot = item.IsBot,
								Count = 0
							};
							Result.Add(returnAgent);
						}

						returnAgent.Count++;
					}
				}
			}

			if (Result == null)
				return new List<SessionUserAgent>();

			return Result.OrderBy(o => o.IsBot).ThenByDescending(d => d.Count).ToList();
		}

		#endregion Private Methods
	}
}
