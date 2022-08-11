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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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
using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
	internal class SessionStatisticsProvider : ISessionStatisticsProvider
	{
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
			//using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			//{
			//    if (isBot)
			//        return CopySessionData<List<SessionDaily>>(_dailySessionDataBot);

			//    return CopySessionData<List<SessionDaily>>(_dailySessionDataHuman);
			//}

			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves hourly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionHourly> GetHourlyData(bool isBot)
		{
			//using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			//{
			//    if (isBot)
			//        return CopySessionData<List<SessionHourly>>(_hourlySessionDataBot);

			//    return CopySessionData<List<SessionHourly>>(_hourlySessionDataHuman);
			//}
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves weekly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionWeekly> GetWeeklyData(bool isBot)
		{
			//using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			//{
			//    if (isBot)
			//        return CopySessionData<List<SessionWeekly>>(_weeklySessionDataBot);

			//    return CopySessionData<List<SessionWeekly>>(_weeklySessionDataHuman);
			//}
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves monthly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionMonthly> GetMonthlyData(bool isBot)
		{
			//using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			//{
			//    if (isBot)
			//        return CopySessionData<List<SessionMonthly>>(_monthlySessionDataBot);

			//    return CopySessionData<List<SessionMonthly>>(_monthlySessionDataHuman);
			//}
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves yearly data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionYearly> GetYearlyData(bool isBot)
		{
			//using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			//{
			//    if (isBot)
			//        return CopySessionData<List<SessionYearly>>(_yearlySessionDataBot);

			//    return CopySessionData<List<SessionYearly>>(_yearlySessionDataHuman);
			//}
			throw new NotImplementedException();
		}

		/// <summary>
		/// Retrieves user agent data
		/// </summary>
		/// <returns></returns>
		public List<SessionUserAgent> GetUserAgents()
		{
			//List<SessionYearly> yearlySessions = CopySessionData<List<SessionYearly>>(_yearlySessionDataBot);

			//List<SessionUserAgent> Result = null;

			//AmalgamateSessionData(yearlySessions, ref Result);

			//yearlySessions = CopySessionData<List<SessionYearly>>(_yearlySessionDataHuman);
			//AmalgamateSessionData(yearlySessions, ref Result);

			//return Result;
			throw new NotImplementedException();
		}

		#endregion ISessionStatisticsProvider Methods

		#region Private Methods

		private static List<SessionUserAgent> AmalgamateSessionData(List<SessionYearly> yearlySessions, ref List<SessionUserAgent> Result)
		{
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
							.Where(r => r.UserAgent.Equals(item.UserAgent) && r.IsBot == item.IsBot)
							.FirstOrDefault();

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
