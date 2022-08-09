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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: UserSessionService.cs
 *
 *  Purpose:  User session service using SimpleDB
 *
 *  Date        Name                Reason
 *  01/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

using Middleware;
using Middleware.SessionData;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Tables;

using Shared.Classes;

using SharedPluginFeatures;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Providers
{
	/// <summary>
	/// IUserSessionService implementation that will be used with text based storage
	/// </summary>
	internal sealed class UserSessionService : ThreadManager, IUserSessionService, ISessionStatisticsProvider, IUrlHashProvider
	{
		#region Private Members

		private const string ThreadName = "Text Based User Session Service";
		private static readonly object _lockObject = new object();
		private static readonly Stack<UserSession> _closedSessions = new Stack<UserSession>();

		private readonly ISimpleDBOperations<SessionDataRow> _sessionData;
		private readonly ISimpleDBOperations<SessionPageDataRow> _sessionPageData;
		private readonly ISimpleDBOperations<InitialReferralsDataRow> _initialRefererData;
		private readonly ISimpleDBOperations<PageViewsDataRow> _pageViewsData;

		//#pragma warning disable IDE0044
		//		private static SessionPageViews _sessionPageViews;
		//        private static SessionInitialReferrers _initialReferrers;
		//#pragma warning restore IDE0044

		//private static List<SessionHourly> _hourlySessionDataHuman;
		//      private static List<SessionDaily> _dailySessionDataHuman;
		//      private static List<SessionWeekly> _weeklySessionDataHuman;
		//      private static List<SessionMonthly> _monthlySessionDataHuman;
		//      private static List<SessionYearly> _yearlySessionDataHuman;
		//      private static List<SessionHourly> _hourlySessionDataBot;
		//      private static List<SessionDaily> _dailySessionDataBot;
		//      private static List<SessionWeekly> _weeklySessionDataBot;
		//      private static List<SessionMonthly> _monthlySessionDataBot;
		//      private static List<SessionYearly> _yearlySessionDataBot;
		internal readonly static Timings _timingsSaveSessions = new Timings();
		private readonly IGeoIpProvider _geoIpProvider;
		private readonly ILogger _logger;
		//private readonly string _pageViewFile;
		//private readonly string _referrerFile;
		//private readonly string _sessionHourlyFileHuman;
		//private readonly string _sessionDailyFileHuman;
		//private readonly string _sessionWeeklyFileHuman;
		//private readonly string _sessionMonthlyFileHuman;
		//private readonly string _sessionYearlyFileHuman;
		//private readonly string _sessionHourlyFileBot;
		//private readonly string _sessionDailyFileBot;
		//private readonly string _sessionWeeklyFileBot;
		//private readonly string _sessionMonthlyFileBot;
		//private readonly string _sessionYearlyFileBot;
		private readonly uint _maxHours;
		private readonly uint _maxDays;
		private readonly uint _maxWeeks;
		private readonly uint _maxMonths;
		private readonly uint _maxYears;

		#endregion Private Members

		#region Constructors

		private UserSessionService()
			: base(null, new TimeSpan(0, 0, 0, 0, 400), null, 0, 200, false)
		{

		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="settingsProvider">ISettingsProvider instance</param>
		/// <param name="geoIpProvider">IGeoIpProvider instance</param>
		/// <param name="logger">ILogger instance</param>
		public UserSessionService(IGeoIpProvider geoIpProvider, 
			ILogger logger,
			ISimpleDBOperations<SettingsDataRow> settingsData, 
			ISimpleDBOperations<SessionDataRow> sessionData,
			ISimpleDBOperations<SessionPageDataRow> sessionPageData,
			ISimpleDBOperations<InitialReferralsDataRow> initialRefererData, 
			ISimpleDBOperations<PageViewsDataRow> pageViewsData)
			: this()
		{
			if (settingsData == null)
				throw new ArgumentNullException(nameof(settingsData));

			_geoIpProvider = geoIpProvider ?? throw new ArgumentNullException(nameof(geoIpProvider));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_sessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
			_sessionPageData = sessionPageData ?? throw new ArgumentNullException(nameof(sessionPageData));
			_initialRefererData = initialRefererData ?? throw new ArgumentNullException(nameof(initialRefererData));
			_pageViewsData = pageViewsData ?? throw new ArgumentNullException(nameof(pageViewsData));

			ContinueIfGlobalException = true;

			//_pageViewFile = GetFile(Path.Combine(_rootPath, "Sessions"), "PageViews.dat");
			//_referrerFile = GetFile(Path.Combine(_rootPath, "Sessions"), "InitialReferrer.dat");
			//_sessionHourlyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "HourlyHuman.dat");
			//_sessionDailyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "DailyHuman.dat");
			//_sessionWeeklyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "WeeklyHuman.dat");
			//_sessionMonthlyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "MonthlyHuman.dat");
			//_sessionYearlyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "YearlyHuman.dat");
			//_sessionHourlyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "HourlyBot.dat");
			//_sessionDailyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "DailyBot.dat");
			//_sessionWeeklyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "WeeklyBot.dat");
			//_sessionMonthlyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "MonthlyBot.dat");
			//_sessionYearlyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "YearlyBot.dat");

			_maxHours = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxHours")).First().Value);
			_maxDays = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxDays")).First().Value);
			_maxWeeks = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxWeeks")).First().Value);
			_maxMonths = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxMonths")).First().Value);
			_maxYears = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxYears")).First().Value);


			//LoadSessionData(_pageViewFile, ref _sessionPageViews);
			//LoadSessionData(_referrerFile, ref _initialReferrers);
			//         LoadSessionData(_sessionHourlyFileHuman, ref _hourlySessionDataHuman);
			//         LoadSessionData(_sessionDailyFileHuman, ref _dailySessionDataHuman);
			//         LoadSessionData(_sessionWeeklyFileHuman, ref _weeklySessionDataHuman);
			//         LoadSessionData(_sessionMonthlyFileHuman, ref _monthlySessionDataHuman);
			//         LoadSessionData(_sessionYearlyFileHuman, ref _yearlySessionDataHuman);
			//         LoadSessionData(_sessionHourlyFileBot, ref _hourlySessionDataBot);
			//         LoadSessionData(_sessionDailyFileBot, ref _dailySessionDataBot);
			//         LoadSessionData(_sessionWeeklyFileBot, ref _weeklySessionDataBot);
			//         LoadSessionData(_sessionMonthlyFileBot, ref _monthlySessionDataBot);
			//         LoadSessionData(_sessionYearlyFileBot, ref _yearlySessionDataBot);

			ThreadManager.ThreadStart(this, ThreadName, ThreadPriority.BelowNormal);
		}

		#endregion Constructors

		#region IUserSessionService Methods

		/// <summary>
		/// User session is being closed, session is moved to another thread to be saved
		/// </summary>
		/// <param name="userSession">UserSession</param>
		public void Closing(in UserSession userSession)
		{
			if (userSession == null)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				_closedSessions.Push(userSession);
			}
		}

		/// <summary>
		/// A user session is being created
		/// </summary>
		/// <param name="userSession"></param>
		public void Created(in UserSession userSession)
		{
			if (userSession == null)
				return;

			SessionDataRow sessionDataRow = new SessionDataRow()
			{
				Bounced = false,
				CountryCode = userSession.CountryCode,
				HostName = userSession.HostName,
				InitialReferrer = userSession.InitialReferrer,
				IpAddress = userSession.IPAddress,
				IsBot = userSession.IsBot,
				IsBrowserMobile = userSession.IsBrowserMobile,
				IsMobile = userSession.IsMobileDevice,
				MobileManufacturer = userSession.MobileManufacturer,
				MobileModel = userSession.MobileModel,
				MobileRedirect = userSession.MobileRedirect,
				ReferralType = (int)userSession.Referal,
				SessionId = userSession.SessionID,
				UserAgent = userSession.UserAgent,
				UserId = userSession.UserID
			};

			_sessionData.Insert(sessionDataRow);
			userSession.InternalSessionID = sessionDataRow.Id;
			userSession.SaveStatus = SaveStatus.Saved;
		}

		/// <summary>
		/// Attempt to retrieve a previously saved user session
		/// </summary>
		/// <param name="userSessionId"></param>
		/// <param name="userSession"></param>
		public void Retrieve(in String userSessionId, ref UserSession userSession)
		{
			if (String.IsNullOrEmpty(userSessionId))
				return;

			string session = userSessionId;
			SessionDataRow sessionData = _sessionData.Select().Where(s => s.SessionId.Equals(session)).FirstOrDefault();

			if (sessionData == null)
				return;

			userSession = new UserSession(sessionData.Id, sessionData.Created, sessionData.SessionId, sessionData.UserAgent,
				sessionData.InitialReferrer, sessionData.IpAddress, sessionData.HostName, sessionData.IsMobile, sessionData.IsBrowserMobile,
				sessionData.MobileRedirect, (ReferalType)sessionData.ReferralType, sessionData.Bounced, sessionData.IsBot,
				sessionData.MobileManufacturer, sessionData.MobileModel, sessionData.UserId, -1, -1, sessionData.SaleCurrency, sessionData.SaleAmount);
		}

		/// <summary>
		/// A user session is being requested to be saved, this only takes place when session
		/// is closing so mark as saved for now
		/// </summary>
		/// <param name="userSession"></param>
		public void Save(in UserSession userSession)
		{
			if (userSession == null)
				throw new ArgumentNullException(nameof(userSession));

			userSession.SaveStatus = SaveStatus.Saved;

			SessionDataRow currentSessionData = _sessionData.Select(userSession.InternalSessionID);

			if (currentSessionData == null)
			{
				Created(userSession);
				currentSessionData = _sessionData.Select(userSession.InternalSessionID);
			}

			currentSessionData.Bounced = userSession.Bounced;
			currentSessionData.CountryCode = userSession.CountryCode;
			currentSessionData.HostName = userSession.HostName;
			currentSessionData.InitialReferrer = userSession.InitialReferrer;
			currentSessionData.IpAddress = userSession.IPAddress;
			currentSessionData.IsBot = userSession.IsBot;
			currentSessionData.IsBrowserMobile = userSession.IsBrowserMobile;
			currentSessionData.IsMobile = userSession.IsMobileDevice;
			currentSessionData.MobileManufacturer = userSession.MobileManufacturer;
			currentSessionData.MobileModel = userSession.MobileModel;
			currentSessionData.MobileRedirect = userSession.MobileRedirect;
			currentSessionData.ReferralType = (int)userSession.Referal;
			currentSessionData.SaleAmount = userSession.CurrentSale;
			currentSessionData.SaleCurrency = userSession.CurrentSaleCurrency;
			currentSessionData.SessionId = userSession.SessionID;
			currentSessionData.UserAgent = userSession.UserAgent;
			currentSessionData.UserId = userSession.UserID;

			_sessionData.Update(currentSessionData);
			SavePage(userSession);
			userSession.SaveStatus = SaveStatus.Saved;
		}

		/// <summary>
		/// A page view is requested to be saved, the actual saving will only happen when 
		/// the session is closing and in another thread
		/// </summary>
		/// <param name="session"></param>
		public void SavePage(in UserSession session)
		{
			if (session == null)
				return;

			session.SaveStatus = SaveStatus.Saved;

			foreach (PageViewData page in session.Pages)
			{
				page.SaveStatus = SaveStatus.Saved;
			}
		}

		#endregion IUserSessionService

		#region ThreadManager Methods

		/// <summary>
		/// Execute the thread that processes user sessions
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		protected override Boolean Run(Object parameters)
		{
			ProcessClosedSessions();

			return !base.HasCancelled();
		}

		#endregion ThreadManager Methods

		#region IUrlHashProvider

		/// <summary>
		/// Retreives a hash of a url
		/// </summary>
		/// <param name="url"></param>
		/// <returns>string</returns>
		public string GetUrlHash(in string url)
		{
			using (SHA256 sha256Hash = SHA256.Create())
			{
				byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(url.ToLower()));

				// Create a new Stringbuilder to collect the bytes
				// and create a string.
				StringBuilder sBuilder = new StringBuilder();

				// Loop through each byte of the hashed data
				// and format each one as a hexadecimal string.
				for (int i = 0; i < data.Length; i++)
				{
					sBuilder.Append(data[i].ToString("x2"));
				}

				// Return the hexadecimal string.
				return sBuilder.ToString();
			}
		}

		#endregion IUrlHashProvider

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

		private void ProcessClosedSessions()
		{
			List<UserSession> sessionsToSave = new List<UserSession>();

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				while (_closedSessions.TryPop(out UserSession session))
					sessionsToSave.Add(session);
			}

			if (sessionsToSave.Count < 1)
				return;

			using (StopWatchTimer timer = StopWatchTimer.Initialise(_timingsSaveSessions))
			{
				foreach (UserSession session in sessionsToSave)
				{
					// update the country data if not already set
					if (session.CountryCode == null || session.CountryCode.Equals("zz", StringComparison.InvariantCultureIgnoreCase))
					{
						if (_geoIpProvider.GetIpAddressDetails(session.IPAddress, out string countryCode,
							out string regionName, out string cityName, out decimal lat, out decimal lon,
							out long _, out long _, out long _))
						{
							session.UpdateIPDetails(0, lat, lon, regionName, cityName, countryCode);
						}
					}

					SessionDataRow sessionData = _sessionData.Select(session.InternalSessionID);

					if (sessionData == null)
					{
						sessionData = new SessionDataRow()
						{
							CountryCode = session.CountryCode,
							HostName = session.HostName,
							InitialReferrer = session.InitialReferrer,
							IpAddress = session.IPAddress,
							IsBrowserMobile = session.IsBrowserMobile,
							IsMobile = session.IsMobileDevice,
							MobileManufacturer = session.MobileManufacturer,
							MobileModel = session.MobileModel,
							MobileRedirect = session.MobileRedirect,
							ReferralType = (int)session.Referal,
							SessionId = session.SessionID,
							UserAgent = session.UserAgent,
						};
					}

					sessionData.UserId = session.UserID;
					sessionData.IsBot = session.IsBot;
					sessionData.Bounced = false;
					sessionData.SaleAmount = session.CurrentSale;
					sessionData.SaleCurrency = session.CurrentSaleCurrency;

					_sessionData.InsertOrUpdate(sessionData);

					session.SaveStatus = SaveStatus.Saved;

					if (session.Pages.Count > 0)
					{
						List<SessionPageDataRow> pages = new List<SessionPageDataRow>();

						foreach (PageViewData page in session.Pages)
						{
							SessionPageDataRow pageData = new SessionPageDataRow()
							{
								IsPostBack = page.IsPostBack,
								Referrer = page.Referrer,
								SessionId = sessionData.Id,
								TotalTime = page.TotalTime.Ticks,
								Url = page.URL,
							};

							pages.Add(pageData);
							page.ID = pageData.Id;
							page.Saved();
						}
							
						_sessionPageData.Insert(pages);

						string pageHash = GetUrlHash(session.Pages[0].URL);
						InitialReferralsDataRow referrer = _initialRefererData.Select().Where(rd => rd.Hash.Equals(pageHash)).FirstOrDefault();

						if (referrer == null)
						{
							referrer = new InitialReferralsDataRow()
							{
								Hash = pageHash,
								Url = session.Pages[0].URL,
							};
						}

						referrer.Usage++;

						_initialRefererData.InsertOrUpdate(referrer);
					}

					//UpdateHourlySessionData(session);
					//UpdateDailySessionData(session);
					//UpdateWeeklySessionData(session);
					//UpdateMonthlySessionData(session);
					//UpdateYearlySessionData(session);
				}
			}
		}

		//        private void UpdateHourlySessionData(UserSession session)
		//        {
		//            if (session == null)
		//                return;

		//            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
		//            {
		//                DateTime currentDate = session.Created;
		//                int hour = currentDate.Hour;
		//                int quarter = Math.Abs(currentDate.Minute / 15) + 1;

		//                List<SessionHourly> hourlySessionData = session.IsBot ? _hourlySessionDataBot : _hourlySessionDataHuman;

		//                SessionHourly hourly = hourlySessionData
		//                    .Where(h => h.Date.Date.Equals(currentDate.Date) && h.Hour == hour && h.Quarter == quarter)
		//                    .FirstOrDefault();

		//                if (hourly == null)
		//                {
		//                    hourly = new SessionHourly();
		//                    hourly.Date = currentDate.Date;
		//                    hourly.Hour = hour;
		//                    hourly.Quarter = quarter;
		//                    hourlySessionData.Add(hourly);
		//                }

		//                UpdateSessionData(session, hourly);

		//                while (hourlySessionData.Count > _maxHours)
		//                    hourlySessionData.RemoveAt(0);
		//            }
		//        }

		//        private void UpdateDailySessionData(UserSession session)
		//        {
		//            if (session == null)
		//                return;

		//            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
		//            {
		//                DateTime sessionDate = session.Created;
		//                List<SessionDaily> hourlySessionData = session.IsBot ? _dailySessionDataBot : _dailySessionDataHuman;

		//                SessionDaily daily = hourlySessionData
		//                    .Where(h => h.Date.Date.Equals(sessionDate.Date))
		//                    .FirstOrDefault();

		//                if (daily == null)
		//                {
		//                    daily = new SessionDaily();
		//                    daily.Date = sessionDate.Date;
		//                    hourlySessionData.Add(daily);
		//                }

		//                UpdateSessionData(session, daily);

		//                while (hourlySessionData.Count > _maxDays)
		//                    hourlySessionData.RemoveAt(0);
		//            }
		//        }

		//        private void UpdateWeeklySessionData(UserSession session)
		//        {
		//            if (session == null)
		//                return;

		//            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
		//            {
		//                DateTime sessionDate = session.Created;

		//#if ISO_WEEK
		//                int week = ISOWeek.GetWeekOfYear(sessionDate);
		//#else
		//                int week = (sessionDate.DayOfYear / 7) + 1;
		//#endif

		//                List<SessionWeekly> weeklySessionData = session.IsBot ? _weeklySessionDataBot : _weeklySessionDataHuman;
		//                SessionWeekly weekly = weeklySessionData
		//                    .Where(w => w.Week.Equals(week) && w.Year == sessionDate.Year)
		//                    .FirstOrDefault();

		//                if (weekly == null)
		//                {
		//                    weekly = new SessionWeekly();
		//                    weekly.Week = week;
		//                    weekly.Year = sessionDate.Year;
		//                    weeklySessionData.Add(weekly);
		//                }

		//                UpdateSessionData(session, weekly);

		//                while (weeklySessionData.Count > _maxWeeks)
		//                    weeklySessionData.RemoveAt(0);
		//            }
		//        }

		//        private void UpdateMonthlySessionData(UserSession session)
		//        {
		//            if (session == null)
		//                return;

		//            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
		//            {
		//                DateTime sessionDate = session.Created;

		//                List<SessionMonthly> monthlySessionData = session.IsBot ? _monthlySessionDataBot : _monthlySessionDataHuman;
		//                SessionMonthly monthly = monthlySessionData
		//                    .Where(w => w.Month.Equals(sessionDate.Month) && w.Year == sessionDate.Year)
		//                    .FirstOrDefault();

		//                if (monthly == null)
		//                {
		//                    monthly = new SessionMonthly();
		//                    monthly.Month = sessionDate.Month;
		//                    monthly.Year = sessionDate.Year;
		//                    monthlySessionData.Add(monthly);
		//                }

		//                UpdateSessionData(session, monthly);

		//                while (monthlySessionData.Count > _maxMonths)
		//                    monthlySessionData.RemoveAt(0);
		//            }
		//        }

		//        private void UpdateYearlySessionData(UserSession session)
		//        {
		//            if (session == null)
		//                return;

		//            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
		//            {
		//                DateTime sessionDate = session.Created;
		//                List<SessionYearly> yearlySessionData = session.IsBot ? _yearlySessionDataBot : _yearlySessionDataHuman;

		//                SessionYearly yearly = yearlySessionData
		//                    .Where(y => y.Year.Equals(sessionDate.Year))
		//                    .FirstOrDefault();

		//                if (yearly == null)
		//                {
		//                    yearly = new SessionYearly();
		//                    yearly.Year = sessionDate.Year;
		//                    yearlySessionData.Add(yearly);
		//                }

		//                UpdateSessionData(session, yearly);

		//                while (yearlySessionData.Count > _maxYears)
		//                    yearlySessionData.RemoveAt(0);
		//            }
		//        }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UpdateSessionData(UserSession session, SessionBaseData baseSessionData)
		{
			baseSessionData.TotalVisits++;

			if (session.IsMobileDevice && !session.IsBot)
				baseSessionData.MobileVisits++;

			if (session.IsBot)
				baseSessionData.BotVisits++;
			else
				baseSessionData.HumanVisits++;

			if (!session.IsBot && session.Bounced)
				baseSessionData.Bounced++;

			baseSessionData.TotalPages += (uint)session.Pages.Count;
			baseSessionData.TotalSales += session.CurrentSale;

			if (session.CurrentSale > 0)
			{
				baseSessionData.Conversions++;

				if (session.IsMobileDevice)
					baseSessionData.MobileConversions++;
			}

			switch (session.Referal)
			{
				case ReferalType.Unknown:
					baseSessionData.ReferrerUnknown++;
					break;

				case ReferalType.Direct:
					baseSessionData.ReferDirect++;
					break;

				case ReferalType.Organic:
					baseSessionData.ReferOrganic++;
					break;

				case ReferalType.Bing:
					baseSessionData.ReferBing++;
					break;

				case ReferalType.Google:
					baseSessionData.ReferGoogle++;
					break;

				case ReferalType.Yahoo:
					baseSessionData.ReferYahoo++;
					break;

				case ReferalType.Facebook:
					baseSessionData.ReferFacebook++;
					break;

				case ReferalType.Twitter:
					baseSessionData.ReferTwitter++;
					break;

				default:
					baseSessionData.ReferOther++;
					break;
			}

			if (!baseSessionData.CountryData.ContainsKey(session.CountryCode))
				baseSessionData.CountryData.Add(session.CountryCode, 0);

			baseSessionData.CountryData[session.CountryCode]++;

			SessionUserAgent returnAgent = baseSessionData.UserAgents
				.Where(r => r.UserAgent.Equals(session.UserAgent) && r.IsBot == session.IsBot)
				.FirstOrDefault();

			if (returnAgent == null)
			{
				returnAgent = new SessionUserAgent()
				{
					UserAgent = session.UserAgent,
					IsBot = session.IsBot,
					Count = 0
				};
				baseSessionData.UserAgents.Add(returnAgent);
			}

			returnAgent.Count++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string GetSessionName(in string sessionId)
		{
			return $"Session Service {sessionId}";
		}

		#endregion Private Methods
	}
}
