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
 *  File: UserSessionService.cs
 *
 *  Purpose:  User session service using SimpleDB
 *
 *  Date        Name                Reason
 *  01/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Runtime.CompilerServices;

using Middleware;

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
	internal sealed class UserSessionService : ThreadManager, IUserSessionService
	{
		#region Private Members

		private const string ThreadName = "Text Based User Session Service";
		private static readonly object _lockObject = new object();
		private static readonly Stack<UserSession> _closedSessions = new Stack<UserSession>();

		private readonly ISimpleDBOperations<SessionDataRow> _sessionData;
		private readonly ISimpleDBOperations<SessionPageDataRow> _sessionPageData;
		private readonly ISimpleDBOperations<InitialReferralsDataRow> _initialRefererData;
		private readonly ISimpleDBOperations<PageViewsDataRow> _pageViewsData;
		private readonly ISimpleDBOperations<SessionStatsHourlyDataRow> _sessionDataHourly;
		private readonly ISimpleDBOperations<SessionStatsDailyDataRow> _sessionDataDaily;
		private readonly ISimpleDBOperations<SessionStatsWeeklyDataRow> _sessionDataWeekly;
		private readonly ISimpleDBOperations<SessionStatsMonthlyDataRow> _sessionDataMonthly;
		private readonly ISimpleDBOperations<SessionStatsYearlyDataRow> _sessionDataYearly;
		private readonly IUrlHashProvider _urlHashProvider;
		internal readonly static Timings _timingsSaveSessions = new Timings();
		private readonly IGeoIpProvider _geoIpProvider;
		private readonly ILogger _logger;
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
			IUrlHashProvider urlHashProvider,
			ISimpleDBOperations<SettingsDataRow> settingsData,
			ISimpleDBOperations<SessionDataRow> sessionData,
			ISimpleDBOperations<SessionPageDataRow> sessionPageData,
			ISimpleDBOperations<InitialReferralsDataRow> initialRefererData,
			ISimpleDBOperations<PageViewsDataRow> pageViewsData,
			ISimpleDBOperations<SessionStatsHourlyDataRow> sessionDataHourly,
			ISimpleDBOperations<SessionStatsDailyDataRow> sessionDataDaily,
			ISimpleDBOperations<SessionStatsWeeklyDataRow> sessionDataWeekly,
			ISimpleDBOperations<SessionStatsMonthlyDataRow> sessionDataMonthly,
			ISimpleDBOperations<SessionStatsYearlyDataRow> sessionDataYearly)
			: this()
		{
			if (settingsData == null)
				throw new ArgumentNullException(nameof(settingsData));

			_geoIpProvider = geoIpProvider ?? throw new ArgumentNullException(nameof(geoIpProvider));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_urlHashProvider = urlHashProvider ?? throw new ArgumentNullException(nameof(urlHashProvider));

			_sessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
			_sessionPageData = sessionPageData ?? throw new ArgumentNullException(nameof(sessionPageData));
			_initialRefererData = initialRefererData ?? throw new ArgumentNullException(nameof(initialRefererData));
			_pageViewsData = pageViewsData ?? throw new ArgumentNullException(nameof(pageViewsData));
			_sessionDataHourly = sessionDataHourly ?? throw new ArgumentNullException(nameof(sessionDataHourly));
			_sessionDataDaily = sessionDataDaily ?? throw new ArgumentNullException(nameof(sessionDataDaily));
			_sessionDataWeekly = sessionDataWeekly ?? throw new ArgumentNullException(nameof(sessionDataWeekly));
			_sessionDataMonthly = sessionDataMonthly ?? throw new ArgumentNullException(nameof(sessionDataMonthly));
			_sessionDataYearly = sessionDataYearly ?? throw new ArgumentNullException(nameof(sessionDataYearly));

			ContinueIfGlobalException = true;

			_maxHours = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxHours")).First().Value);
			_maxDays = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxDays")).First().Value);
			_maxWeeks = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxWeeks")).First().Value);
			_maxMonths = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxMonths")).First().Value);
			_maxYears = Convert.ToUInt32(settingsData.Select().Where(sd => sd.Name.Equals("SessionMaxYears")).First().Value);

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

		#region Private Methods

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

						string pageHash = _urlHashProvider.GetUrlHash(session.Pages[0].URL);
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

					UpdateHourlySessionData(session);
					UpdateDailySessionData(session);
					UpdateWeeklySessionData(session);
					UpdateMonthlySessionData(session);
					UpdateYearlySessionData(session);
				}
			}
		}

		private void UpdateHourlySessionData(UserSession session)
		{
			if (session == null)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				DateTime currentDate = session.Created;
				int hour = currentDate.Hour;
				int quarter = Math.Abs(currentDate.Minute / 15) + 1;

				SessionStatsHourlyDataRow hourly = _sessionDataHourly.Select()
					.Where(h => h.IsBot.Equals(session.IsBot) && h.Date.Date.Equals(currentDate.Date) && h.Hour == hour && h.Quarter == quarter)
					.FirstOrDefault();

				if (hourly == null)
				{
					hourly = new SessionStatsHourlyDataRow()
					{
						IsBot = session.IsBot,
						DateTicks = currentDate.Date.Ticks,
						Hour = hour,
						Quarter = quarter,
					};
				}

				UpdateSessionData(session, hourly);
				_sessionDataHourly.InsertOrUpdate(hourly);
			}
		}

		private void UpdateDailySessionData(UserSession session)
		{
			if (session == null)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				DateTime sessionDate = session.Created;

				SessionStatsDailyDataRow daily = _sessionDataDaily.Select()
					.Where(d => d.IsBot.Equals(session.IsBot) && d.Date.Date.Equals(sessionDate.Date))
					.FirstOrDefault();

				if (daily == null)
				{
					daily = new SessionStatsDailyDataRow()
					{
						IsBot = session.IsBot,
						DateTicks = sessionDate.Date.Ticks,
					};
				}

				UpdateSessionData(session, daily);
				_sessionDataDaily.InsertOrUpdate(daily);
			}
		}

		private void UpdateWeeklySessionData(UserSession session)
		{
			if (session == null)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				DateTime sessionDate = session.Created;

#if ISO_WEEK
		        int week = ISOWeek.GetWeekOfYear(sessionDate);
#else
				int week = (sessionDate.DayOfYear / 7) + 1;
#endif

				SessionStatsWeeklyDataRow weekly = _sessionDataWeekly.Select()
					.Where(w => w.IsBot.Equals(session.IsBot) && w.Week.Equals(week) && w.Year == sessionDate.Year)
					.FirstOrDefault();

				if (weekly == null)
				{
					weekly = new SessionStatsWeeklyDataRow()
					{
						IsBot = session.IsBot,
						Year = sessionDate.Year,
						Week = week,
					};
				}

				UpdateSessionData(session, weekly);
				_sessionDataWeekly.InsertOrUpdate(weekly);
			}
		}

		private void UpdateMonthlySessionData(UserSession session)
		{
			if (session == null)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				DateTime sessionDate = session.Created;

				SessionStatsMonthlyDataRow monthly = _sessionDataMonthly.Select()
					.Where(m => m.IsBot.Equals(session.IsBot) && m.Month.Equals(sessionDate.Month) && m.Year == sessionDate.Year)
					.FirstOrDefault();

				if (monthly == null)
				{
					monthly = new SessionStatsMonthlyDataRow()
					{
						IsBot = session.IsBot,
						Month = sessionDate.Month,
						Year = sessionDate.Year,
					};
				}

				UpdateSessionData(session, monthly);
				_sessionDataMonthly.InsertOrUpdate(monthly);
			}
		}

		private void UpdateYearlySessionData(UserSession session)
		{
			if (session == null)
				return;

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				DateTime sessionDate = session.Created;

				SessionStatsYearlyDataRow yearly = _sessionDataYearly.Select()
					.Where(y => y.IsBot.Equals(session.IsBot) && y.Year.Equals(sessionDate.Year))
					.FirstOrDefault();

				if (yearly == null)
				{
					yearly = new SessionStatsYearlyDataRow()
					{
						IsBot = session.IsBot,
						Year = sessionDate.Year,
					};
				}

				UpdateSessionData(session, yearly);
				_sessionDataYearly.InsertOrUpdate(yearly);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void UpdateSessionData(UserSession session, SessionStatsBaseData baseSessionData)
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
					baseSessionData.ReferUnknown++;
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

			string countryCode = session.CountryCode ?? "ZZ";

			if (!baseSessionData.CountryData.ContainsKey(countryCode))
				baseSessionData.CountryData.Add(countryCode, 0);

			baseSessionData.CountryData[countryCode]++;

			string userAgent = session.UserAgent ?? "Unknown";

			if (!baseSessionData.UserAgents.ContainsKey(userAgent))
				baseSessionData.UserAgents.Add(userAgent, 0);

			baseSessionData.UserAgents[userAgent]++;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string GetSessionName(in string sessionId)
		{
			return $"Session Service {sessionId}";
		}

		#endregion Private Methods
	}
}
