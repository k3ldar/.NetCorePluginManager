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
 *  File: UserSessionService.cs
 *
 *  Purpose:  User session service using SimpleDB
 *
 *  Date        Name                Reason
 *  01/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System.Globalization;
using System.Runtime.CompilerServices;

using Middleware;

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
		private static readonly object _lockObject = new();
		private static readonly object _sesionDataLock = new();
		private static readonly Stack<UserSession> _closedSessions = new();

		private readonly ISimpleDBOperations<UserDataRow> _users;
		private readonly ISimpleDBOperations<SessionDataRow> _sessionData;
		private readonly ISimpleDBOperations<SessionPageDataRow> _sessionPageData;
		private readonly ISimpleDBOperations<InitialReferralsDataRow> _initialRefererData;
		private readonly ISimpleDBOperations<SessionStatsHourlyDataRow> _sessionDataHourly;
		private readonly ISimpleDBOperations<SessionStatsDailyDataRow> _sessionDataDaily;
		private readonly ISimpleDBOperations<SessionStatsWeeklyDataRow> _sessionDataWeekly;
		private readonly ISimpleDBOperations<SessionStatsMonthlyDataRow> _sessionDataMonthly;
		private readonly ISimpleDBOperations<SessionStatsYearlyDataRow> _sessionDataYearly;
		private readonly IUrlHashProvider _urlHashProvider;
		internal readonly static Timings _timingsSaveSessions = new();
		internal readonly static Timings _timingsUpdateAllSessions = new();
		private readonly IGeoIpProvider _geoIpProvider;
		private bool _InitialProcessing = true;

		#endregion Private Members

		#region Constructors

		private UserSessionService()
			: base(null, new TimeSpan(0, 0, 0, 0, 400), null, 0, 200, false)
		{

		}

		public UserSessionService(
			IUrlHashProvider urlHashProvider,
			ISimpleDBOperations<UserDataRow> users,
			ISimpleDBOperations<SettingsDataRow> settingsData,
			ISimpleDBOperations<SessionDataRow> sessionData,
			ISimpleDBOperations<SessionPageDataRow> sessionPageData,
			ISimpleDBOperations<InitialReferralsDataRow> initialRefererData,
			ISimpleDBOperations<SessionStatsHourlyDataRow> sessionDataHourly,
			ISimpleDBOperations<SessionStatsDailyDataRow> sessionDataDaily,
			ISimpleDBOperations<SessionStatsWeeklyDataRow> sessionDataWeekly,
			ISimpleDBOperations<SessionStatsMonthlyDataRow> sessionDataMonthly,
			ISimpleDBOperations<SessionStatsYearlyDataRow> sessionDataYearly)
			: this(null, urlHashProvider, users, settingsData, sessionData, sessionPageData,
				  initialRefererData, sessionDataHourly, sessionDataDaily, sessionDataWeekly,
				  sessionDataMonthly, sessionDataYearly)
		{
		}

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="settingsProvider">ISettingsProvider instance</param>
		/// <param name="geoIpProvider">IGeoIpProvider instance</param>
		/// <param name="logger">ILogger instance</param>
		public UserSessionService(IGeoIpProvider geoIpProvider,
			IUrlHashProvider urlHashProvider,
			ISimpleDBOperations<UserDataRow> users,
			ISimpleDBOperations<SettingsDataRow> settingsData,
			ISimpleDBOperations<SessionDataRow> sessionData,
			ISimpleDBOperations<SessionPageDataRow> sessionPageData,
			ISimpleDBOperations<InitialReferralsDataRow> initialRefererData,
			ISimpleDBOperations<SessionStatsHourlyDataRow> sessionDataHourly,
			ISimpleDBOperations<SessionStatsDailyDataRow> sessionDataDaily,
			ISimpleDBOperations<SessionStatsWeeklyDataRow> sessionDataWeekly,
			ISimpleDBOperations<SessionStatsMonthlyDataRow> sessionDataMonthly,
			ISimpleDBOperations<SessionStatsYearlyDataRow> sessionDataYearly)
			: this()
		{
			if (settingsData == null)
				throw new ArgumentNullException(nameof(settingsData));

			_geoIpProvider = geoIpProvider;
			_urlHashProvider = urlHashProvider ?? throw new ArgumentNullException(nameof(urlHashProvider));

			_users = users ?? throw new ArgumentNullException(nameof(users));
			_sessionData = sessionData ?? throw new ArgumentNullException(nameof(sessionData));
			_sessionPageData = sessionPageData ?? throw new ArgumentNullException(nameof(sessionPageData));
			_initialRefererData = initialRefererData ?? throw new ArgumentNullException(nameof(initialRefererData));
			_sessionDataHourly = sessionDataHourly ?? throw new ArgumentNullException(nameof(sessionDataHourly));
			_sessionDataDaily = sessionDataDaily ?? throw new ArgumentNullException(nameof(sessionDataDaily));
			_sessionDataWeekly = sessionDataWeekly ?? throw new ArgumentNullException(nameof(sessionDataWeekly));
			_sessionDataMonthly = sessionDataMonthly ?? throw new ArgumentNullException(nameof(sessionDataMonthly));
			_sessionDataYearly = sessionDataYearly ?? throw new ArgumentNullException(nameof(sessionDataYearly));

			ContinueIfGlobalException = true;

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

			SessionDataRow sessionDataRow = new()
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
				UserId = userSession.UserID < 0 ? 0 : userSession.UserID
			};

			UserDataRow user = _users.Select(sessionDataRow.UserId);

			if (user != null)
			{
				userSession.UserEmail = user.Email;
				userSession.UserName = user.FullName;
			}

			userSession.SaveStatus = SaveStatus.Saved;

			using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
				_sessionData.Insert(sessionDataRow);

			userSession.InternalSessionID = sessionDataRow.Id;
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
			SessionDataRow sessionData = null;

			using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
				sessionData = _sessionData.Select().FirstOrDefault(s => s.SessionId.Equals(session));

			if (sessionData == null)
				return;

			userSession = new UserSession(sessionData.Id, sessionData.Created, sessionData.SessionId, sessionData.UserAgent,
				sessionData.InitialReferrer, sessionData.IpAddress, sessionData.HostName, sessionData.IsMobile, sessionData.IsBrowserMobile,
				sessionData.MobileRedirect, (ReferalType)sessionData.ReferralType, sessionData.Bounced, sessionData.IsBot,
				sessionData.MobileManufacturer, sessionData.MobileModel, sessionData.UserId, -1, -1, sessionData.SaleCurrency, sessionData.SaleAmount);

			UserDataRow user = _users.Select(sessionData.UserId);

			if (user != null)
			{
				userSession.UserEmail = user.Email;
				userSession.UserName = user.FullName;
			}
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

			string sessionId = userSession.SessionID;
			SessionDataRow currentSessionData = null;

			using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
				currentSessionData = _sessionData.Select(userSession.InternalSessionID) ?? _sessionData.Select().FirstOrDefault(s => s.SessionId.Equals(sessionId));

			if (currentSessionData == null)
			{
				Created(userSession);

				using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
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
			currentSessionData.UserId = userSession.UserID < 0 ? 0 : userSession.UserID;

			using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
				_sessionData.Update(currentSessionData);

			SavePage(userSession);
			userSession.SaveStatus = SaveStatus.Saved;
		}

		/// <summary>
		/// A page view is requested to be saved, the actual saving will only happen when 
		/// the session is closing and in another thread
		/// </summary>
		/// <param name="pageView"></param>
		public void SavePage(in UserSession pageView)
		{
			if (pageView == null)
				return;

			pageView.SaveStatus = SaveStatus.Saved;

			foreach (PageViewData page in pageView.Pages)
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
			if (_InitialProcessing)
			{
				// rebuild all session data when first run
				ProcessAllSessions();
			}
			else
			{
				ProcessClosedSessions();
			}

			return !base.HasCancelled();
		}

		#endregion ThreadManager Methods

		#region Private Methods

		private void ProcessClosedSessions()
		{
			List<UserSession> sessionsToSave = new();

			using (TimedLock timedLock = TimedLock.Lock(_lockObject))
			{
				while (_closedSessions.TryPop(out UserSession session))
					sessionsToSave.Add(session);
			}

			if (sessionsToSave.Count < 1)
				return;

			ProcessSessions(sessionsToSave);
		}

		private void ProcessAllSessions()
		{
			_initialRefererData.Truncate();
			_sessionDataHourly.Truncate();
			_sessionDataDaily.Truncate();
			_sessionDataWeekly.Truncate();
			_sessionDataMonthly.Truncate();
			_sessionDataYearly.Truncate();

			IReadOnlyList<SessionPageDataRow> sessionPages = _sessionPageData.Select();

			using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
			{
				foreach (SessionDataRow session in _sessionData.Select())
				{
					using (StopWatchTimer timer = StopWatchTimer.Initialise(_timingsUpdateAllSessions))
					{
						UserSession sessionData = new(session.Id, session.Created, session.SessionId, session.UserAgent,
						session.InitialReferrer, session.IpAddress, session.HostName, session.IsMobile, session.IsBrowserMobile,
						session.MobileRedirect, (ReferalType)session.ReferralType, session.Bounced, session.IsBot, session.MobileManufacturer,
						session.MobileModel, session.UserId, 0, 0, session.SaleCurrency, session.SaleAmount)
						{
							SaveStatus = SaveStatus.Saved,
						};

						UpdateGeoIpDataForSession(sessionData);


						SessionPageDataRow firstPage = sessionPages.FirstOrDefault(spd => spd.SessionId.Equals(session.Id));

						if (firstPage != null)
						{
							sessionData.Pages.Add(new PageViewData(firstPage.Url, firstPage.Referrer, firstPage.IsPostBack)
							{
								SaveStatus = SaveStatus.Saved,
							});

							UpdateInitialReferrer(sessionData);
						}

						UpdateHourlySessionData(sessionData);
						UpdateDailySessionData(sessionData);
						UpdateWeeklySessionData(sessionData);
						UpdateMonthlySessionData(sessionData);
						UpdateYearlySessionData(sessionData);
					}
				}
			}

			_initialRefererData.ForceWrite();
			_sessionDataHourly.ForceWrite();
			_sessionDataDaily.ForceWrite();
			_sessionDataWeekly.ForceWrite();
			_sessionDataMonthly.ForceWrite();
			_sessionDataYearly.ForceWrite();

			_InitialProcessing = false;
		}

		private void ProcessSessions(List<UserSession> sessionsToSave)
		{
			using (StopWatchTimer timer = StopWatchTimer.Initialise(_timingsSaveSessions))
			{
				foreach (UserSession session in sessionsToSave)
				{
					UpdateGeoIpDataForSession(session);

					SessionDataRow sessionData = null;

					using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
						sessionData = _sessionData.Select(session.InternalSessionID);

					sessionData ??= new SessionDataRow()
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

					sessionData.UserId = session.UserID;
					sessionData.IsBot = session.IsBot;
					sessionData.Bounced = false;
					sessionData.SaleAmount = session.CurrentSale;
					sessionData.SaleCurrency = session.CurrentSaleCurrency;

					using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
						_sessionData.InsertOrUpdate(sessionData);

					session.SaveStatus = SaveStatus.Saved;

					if (session.Pages.Count > 0)
					{
						List<SessionPageDataRow> pages = new();

						foreach (PageViewData page in session.Pages)
						{
							SessionPageDataRow pageData = new()
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

						UpdateInitialReferrer(session);
					}

					UpdateHourlySessionData(session);
					UpdateDailySessionData(session);
					UpdateWeeklySessionData(session);
					UpdateMonthlySessionData(session);
					UpdateYearlySessionData(session);
				}
			}
		}

		private void UpdateGeoIpDataForSession(UserSession session)
		{
			// update the country data if not already set
			if (_geoIpProvider != null && (session.CountryCode == null || session.CountryCode.Equals("zz", StringComparison.InvariantCultureIgnoreCase)) &&
				_geoIpProvider.GetIpAddressDetails(session.IPAddress, out string countryCode,
					out string regionName, out string cityName, out decimal lat, out decimal lon,
					out long _, out long _, out long _))
			{
				session.UpdateIPDetails(0, lat, lon, regionName, cityName, countryCode);
			}
		}

		private void UpdateInitialReferrer(UserSession session)
		{
			if (session.Pages.Count > 0)
			{
				string pageHash = _urlHashProvider.GetUrlHash(session.Pages[0].URL);
				InitialReferralsDataRow referrer = _initialRefererData.Select().FirstOrDefault(rd => rd.Hash.Equals(pageHash));

				referrer ??= new InitialReferralsDataRow()
				{
					Hash = pageHash,
					Url = session.Pages[0].URL,
				};

				referrer.Usage++;

				_initialRefererData.InsertOrUpdate(referrer);
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

				SessionStatsHourlyDataRow hourly = null;

				using (TimedLock sessionLock = TimedLock.Lock(_sesionDataLock))
					hourly = _sessionDataHourly.Select()
					.FirstOrDefault(h => h.IsBot.Equals(session.IsBot) && h.Date.Date.Equals(currentDate.Date) && h.Hour == hour && h.Quarter == quarter);

				hourly ??= new SessionStatsHourlyDataRow()
				{
					IsBot = session.IsBot,
					DateTicks = currentDate.Date.Ticks,
					Hour = hour,
					Quarter = quarter,
				};

				UserSessionService.UpdateSessionData(session, hourly);
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
					.FirstOrDefault(d => d.IsBot.Equals(session.IsBot) && d.Date.Date.Equals(sessionDate.Date));

				daily ??= new SessionStatsDailyDataRow()
				{
					IsBot = session.IsBot,
					DateTicks = sessionDate.Date.Ticks,
				};

				UserSessionService.UpdateSessionData(session, daily);
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

#if NET_5_ABOVE
				int week = ISOWeek.GetWeekOfYear(sessionDate);
#else
				int week = (sessionDate.DayOfYear / 7) + 1;
#endif

				SessionStatsWeeklyDataRow weekly = _sessionDataWeekly.Select()
					.FirstOrDefault(w => w.IsBot.Equals(session.IsBot) && w.Week.Equals(week) && w.Year == sessionDate.Year);

				weekly ??= new SessionStatsWeeklyDataRow()
				{
					IsBot = session.IsBot,
					Year = sessionDate.Year,
					Week = week,
				};

				UserSessionService.UpdateSessionData(session, weekly);
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
					.FirstOrDefault(m => m.IsBot.Equals(session.IsBot) && m.Month.Equals(sessionDate.Month) && m.Year == sessionDate.Year);

				monthly ??= new SessionStatsMonthlyDataRow()
				{
					IsBot = session.IsBot,
					Month = sessionDate.Month,
					Year = sessionDate.Year,
				};

				UserSessionService.UpdateSessionData(session, monthly);
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
					.FirstOrDefault(y => y.IsBot.Equals(session.IsBot) && y.Year.Equals(sessionDate.Year));

				yearly ??= new SessionStatsYearlyDataRow()
				{
					IsBot = session.IsBot,
					Year = sessionDate.Year,
				};

				UserSessionService.UpdateSessionData(session, yearly);
				_sessionDataYearly.InsertOrUpdate(yearly);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void UpdateSessionData(UserSession session, SessionStatsBaseData baseSessionData)
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

		#endregion Private Methods
	}
}
