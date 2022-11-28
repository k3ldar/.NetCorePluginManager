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
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: DefaultUserSessionService.cs
 *
 *  Purpose:  Default user session service
 *
 *  Date        Name                Reason
 *  01/09/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Hosting;

using Middleware;

using Newtonsoft.Json;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using UserSessionMiddleware.Plugin.Classes.SessionData;

using Middleware.SessionData;
using System.Globalization;

namespace UserSessionMiddleware.Plugin.Classes
{
    /// <summary>
    /// Default IUserSessionService implementation that will be used if no other IUserSessionService
    /// instances have been registered
    /// </summary>
    public sealed class DefaultUserSessionService : ThreadManager, IUserSessionService, ISessionStatisticsProvider, IUrlHashProvider
	{
        #region Private Members

        private readonly string _rootPath;
        private static readonly object _lockObject = new object();
        private static readonly Stack<UserSession> _closedSessions = new Stack<UserSession>();

#pragma warning disable IDE0044
		private static SessionPageViews _sessionPageViews;
        private static SessionInitialReferrers _initialReferrers;
#pragma warning restore IDE0044

		private static List<SessionHourly> _hourlySessionDataHuman;
        private static List<SessionDaily> _dailySessionDataHuman;
        private static List<SessionWeekly> _weeklySessionDataHuman;
        private static List<SessionMonthly> _monthlySessionDataHuman;
        private static List<SessionYearly> _yearlySessionDataHuman;
        private static List<SessionHourly> _hourlySessionDataBot;
        private static List<SessionDaily> _dailySessionDataBot;
        private static List<SessionWeekly> _weeklySessionDataBot;
        private static List<SessionMonthly> _monthlySessionDataBot;
        private static List<SessionYearly> _yearlySessionDataBot;
        internal readonly static Timings _timingsDefaultSession = new Timings();
        private readonly IGeoIpProvider _geoIpProvider;
        private readonly ILogger _logger;
        private readonly string _pageViewFile;
        private readonly string _referrerFile;
        private readonly string _sessionHourlyFileHuman;
        private readonly string _sessionDailyFileHuman;
        private readonly string _sessionWeeklyFileHuman;
        private readonly string _sessionMonthlyFileHuman;
        private readonly string _sessionYearlyFileHuman;
        private readonly string _sessionHourlyFileBot;
        private readonly string _sessionDailyFileBot;
        private readonly string _sessionWeeklyFileBot;
        private readonly string _sessionMonthlyFileBot;
        private readonly string _sessionYearlyFileBot;
        private readonly uint _maxHours;
        private readonly uint _maxDays;
        private readonly uint _maxWeeks;
        private readonly uint _maxMonths;
        private readonly uint _maxYears;
        private readonly bool _enabled = false;

        #endregion Private Members

        #region Constructors

        private DefaultUserSessionService()
            : base(null, new TimeSpan(0, 0, 30))
        {

        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="hostingEnvironment">IHostingEnvironment instance</param>
        /// <param name="settingsProvider">ISettingsProvider instance</param>
        /// <param name="geoIpProvider">IGeoIpProvider instance</param>
        /// <param name="logger">ILogger instance</param>
        public DefaultUserSessionService(IHostingEnvironment hostingEnvironment,
            ISettingsProvider settingsProvider,
            IGeoIpProvider geoIpProvider,
            ILogger logger)
            : this()
        {
            if (hostingEnvironment == null)
                throw new ArgumentNullException(nameof(hostingEnvironment));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _geoIpProvider = geoIpProvider ?? throw new ArgumentNullException(nameof(geoIpProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            UserSessionSettings settings = settingsProvider.GetSettings<UserSessionSettings>(SharedPluginFeatures.Constants.UserSessionConfiguration);

            if (!settings.EnableDefaultSessionService)
                return;

            _enabled = true;

            ContinueIfGlobalException = true;

            if (String.IsNullOrEmpty(settings.SessionRootPath))
                settings.SessionRootPath = hostingEnvironment.ContentRootPath;

            _rootPath = Path.Combine(settings.SessionRootPath, "UserSession");

            _pageViewFile = GetFile(Path.Combine(_rootPath, "Sessions"), "PageViews.dat");
            _referrerFile = GetFile(Path.Combine(_rootPath, "Sessions"), "InitialReferrer.dat");
            _sessionHourlyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "HourlyHuman.dat");
            _sessionDailyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "DailyHuman.dat");
            _sessionWeeklyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "WeeklyHuman.dat");
            _sessionMonthlyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "MonthlyHuman.dat");
            _sessionYearlyFileHuman = GetFile(Path.Combine(_rootPath, "Sessions"), "YearlyHuman.dat");
            _sessionHourlyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "HourlyBot.dat");
            _sessionDailyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "DailyBot.dat");
            _sessionWeeklyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "WeeklyBot.dat");
            _sessionMonthlyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "MonthlyBot.dat");
            _sessionYearlyFileBot = GetFile(Path.Combine(_rootPath, "Sessions"), "YearlyBot.dat");

            _maxHours = settings.MaxHourlyData;
            _maxDays = settings.MaxDailyData;
            _maxWeeks = settings.MaxWeeklyData;
            _maxMonths = settings.MaxMonthlyData;
            _maxYears = settings.MaxYearlyData;


			LoadSessionData(_pageViewFile, ref _sessionPageViews);
			LoadSessionData(_referrerFile, ref _initialReferrers);
            LoadSessionData(_sessionHourlyFileHuman, ref _hourlySessionDataHuman);
            LoadSessionData(_sessionDailyFileHuman, ref _dailySessionDataHuman);
            LoadSessionData(_sessionWeeklyFileHuman, ref _weeklySessionDataHuman);
            LoadSessionData(_sessionMonthlyFileHuman, ref _monthlySessionDataHuman);
            LoadSessionData(_sessionYearlyFileHuman, ref _yearlySessionDataHuman);
            LoadSessionData(_sessionHourlyFileBot, ref _hourlySessionDataBot);
            LoadSessionData(_sessionDailyFileBot, ref _dailySessionDataBot);
            LoadSessionData(_sessionWeeklyFileBot, ref _weeklySessionDataBot);
            LoadSessionData(_sessionMonthlyFileBot, ref _monthlySessionDataBot);
            LoadSessionData(_sessionYearlyFileBot, ref _yearlySessionDataBot);

            ThreadManager.ThreadStart(this, "Default User Session Service", System.Threading.ThreadPriority.BelowNormal);
        }

        #endregion Constructors

        #region IUserSessionService Methods

        /// <summary>
        /// User session is being closed, if enabled using <see cref="UserSessionMiddleware.Plugin.UserSessionSettings.EnableDefaultSessionService"/> then 
        /// the session is added to the list of closed sessions to process
        /// </summary>
        /// <param name="userSession"></param>
        public void Closing(in UserSession userSession)
        {
            if (userSession == null || !_enabled)
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
            if (userSession == null || !_enabled)
                return;
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
        }

        /// <summary>
        /// A user session is being requested to be saved
        /// </summary>
        /// <param name="userSession"></param>
        public void Save(in UserSession userSession)
        {
            if (userSession == null)
                throw new ArgumentNullException(nameof(userSession));

            userSession.SaveStatus = SaveStatus.Saved;
        }

        /// <summary>
        /// A page view is requested to be saved
        /// </summary>
        /// <param name="pageView"></param>
        public void SavePage(in UserSession pageView)
        {
            if (pageView == null)
                return;

            pageView.SaveStatus = SaveStatus.Saved;
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

		#region ISessionStatisticsProvider Methods

		/// <summary>
		/// Retreives a hash of a url
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public string GetUrlHash(in string url)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return GetHash(sha256Hash, url.ToLower());
            }
        }

		/// <summary>
		/// Retrieves daily data
		/// </summary>
		/// <param name="isBot"></param>
		/// <returns></returns>
		public List<SessionDaily> GetDailyData(bool isBot)
        {
            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                if (isBot)
                    return CopySessionData<List<SessionDaily>>(_dailySessionDataBot);

                return CopySessionData<List<SessionDaily>>(_dailySessionDataHuman);
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
                if (isBot)
                    return CopySessionData<List<SessionHourly>>(_hourlySessionDataBot);

                return CopySessionData<List<SessionHourly>>(_hourlySessionDataHuman);
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
                if (isBot)
                    return CopySessionData<List<SessionWeekly>>(_weeklySessionDataBot);

                return CopySessionData<List<SessionWeekly>>(_weeklySessionDataHuman);
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
                if (isBot)
                    return CopySessionData<List<SessionMonthly>>(_monthlySessionDataBot);

                return CopySessionData<List<SessionMonthly>>(_monthlySessionDataHuman);
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
                if (isBot)
                    return CopySessionData<List<SessionYearly>>(_yearlySessionDataBot);

                return CopySessionData<List<SessionYearly>>(_yearlySessionDataHuman);
            }
        }

		/// <summary>
		/// Retrieves user agent data
		/// </summary>
		/// <returns></returns>
		public List<SessionUserAgent> GetUserAgents()
        {
            List<SessionUserAgent> Result = null;

            List<SessionYearly> yearlySessions = CopySessionData<List<SessionYearly>>(_yearlySessionDataBot);
            AmalgamateSessionData(yearlySessions, ref Result);

            yearlySessions = CopySessionData<List<SessionYearly>>(_yearlySessionDataHuman);
            AmalgamateSessionData(yearlySessions, ref Result);

            return Result;
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
                        SessionUserAgent returnAgent = Result.FirstOrDefault(r => r.UserAgent.Equals(item.UserAgent) && r.IsBot == item.IsBot);

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
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

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

        private static string GetFile(in string path, in string fileName)
        {
            string Result = Path.Combine(path, fileName);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Result;
        }

        private string GetPath(in string pathName)
        {
            string Result = Path.Combine(_rootPath, pathName);

            if (!Directory.Exists(Result))
                Directory.CreateDirectory(Result);

            return Result;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Allow for logging exceptions if problems arise")]
        private void LoadSessionData<T>(string filename, ref T sessionData) where T : new()
        {
            if (sessionData == null)
            {
                try
                {
                    if (File.Exists(filename))
                    {
                        byte[] fileBytes = File.ReadAllBytes(filename);
                        sessionData = JsonConvert.DeserializeObject<T>(Encoding.Unicode.GetString(fileBytes));
                        return;
                    }
                }
                catch (Exception err)
                {
                    _logger.AddToLog(PluginManager.LogLevel.Error, err);
                }

                sessionData = new T();
            }

			if (sessionData is IUrlHash)
				((IUrlHash)sessionData).SetUrlHash(this);

		}

		private static T CopySessionData<T>(object value)
        {
            if (value == null)
                return default;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(value));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Allow for logging exceptions if problems arise")]
        private void SaveSessionData(string filename, object value)
        {
            try
            {
                byte[] fileBytes = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(value));
                File.WriteAllBytes(filename, fileBytes);
            }
            catch (Exception err)
            {
                _logger.AddToLog(PluginManager.LogLevel.Error, err);
            }
        }

        private void ProcessClosedSessions()
        {
            List<UserSession> readySessions = new List<UserSession>();

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                while (_closedSessions.TryPop(out UserSession session))
                    readySessions.Add(session);
            }

            if (readySessions.Count < 1)
                return;

            using (StopWatchTimer timer = StopWatchTimer.Initialise(_timingsDefaultSession))
            {
                foreach (UserSession session in readySessions)
                {
                    // update the country data if not already set
                    if (session.CountryCode.Equals("zz", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (_geoIpProvider.GetIpAddressDetails(session.IPAddress, out string countryCode,
                            out string regionName, out string cityName, out decimal lat, out decimal lon,
                            out long _, out long _, out long _))
                        {
                            session.UpdateIPDetails(0, lat, lon, regionName, cityName, countryCode);
                        }
                    }

                    foreach (PageViewData pageView in session.Pages)
                    {
                        _sessionPageViews.Add(pageView.URL, pageView.TimeStamp, pageView.TotalTime.TotalSeconds, session.IsBot, session.Bounced);
                    }

                    if (session.Pages.Count > 0)
                    {
                        _initialReferrers.Add(session.Pages[0].Referrer);
                    }

                    UpdateHourlySessionData(session);
                    UpdateDailySessionData(session);
                    UpdateWeeklySessionData(session);
                    UpdateMonthlySessionData(session);
                    UpdateYearlySessionData(session);
                }

                SaveSessionData(_sessionHourlyFileHuman, _hourlySessionDataHuman);
                SaveSessionData(_sessionDailyFileHuman, _dailySessionDataHuman);
                SaveSessionData(_sessionWeeklyFileHuman, _weeklySessionDataHuman);
                SaveSessionData(_sessionMonthlyFileHuman, _monthlySessionDataHuman);
                SaveSessionData(_sessionYearlyFileHuman, _yearlySessionDataHuman);
                SaveSessionData(_sessionHourlyFileBot, _hourlySessionDataBot);
                SaveSessionData(_sessionDailyFileBot, _dailySessionDataBot);
                SaveSessionData(_sessionWeeklyFileBot, _weeklySessionDataBot);
                SaveSessionData(_sessionMonthlyFileBot, _monthlySessionDataBot);
                SaveSessionData(_sessionYearlyFileBot, _yearlySessionDataBot);

                if (_sessionPageViews.IsDirty)
                {
                    SaveSessionData(_pageViewFile, _sessionPageViews);
                    _sessionPageViews.IsDirty = false;
                }

                if (_initialReferrers.IsDirty)
                {
                    SaveSessionData(_referrerFile, _initialReferrers);
                    _initialReferrers.IsDirty = false;
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

                List<SessionHourly> hourlySessionData = session.IsBot ? _hourlySessionDataBot : _hourlySessionDataHuman;

                SessionHourly hourly = hourlySessionData.FirstOrDefault(h => h.Date.Date.Equals(currentDate.Date) && h.Hour == hour && h.Quarter == quarter);

                if (hourly == null)
                {
                    hourly = new SessionHourly();
                    hourly.Date = currentDate.Date;
                    hourly.Hour = hour;
                    hourly.Quarter = quarter;
                    hourlySessionData.Add(hourly);
                }

                UpdateSessionData(session, hourly);

                while (hourlySessionData.Count > _maxHours)
                    hourlySessionData.RemoveAt(0);
            }
        }

        private void UpdateDailySessionData(UserSession session)
        {
            if (session == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                DateTime sessionDate = session.Created;
                List<SessionDaily> hourlySessionData = session.IsBot ? _dailySessionDataBot : _dailySessionDataHuman;

                SessionDaily daily = hourlySessionData.FirstOrDefault(h => h.Date.Date.Equals(sessionDate.Date));

                if (daily == null)
                {
                    daily = new SessionDaily();
                    daily.Date = sessionDate.Date;
                    hourlySessionData.Add(daily);
                }

                UpdateSessionData(session, daily);

                while (hourlySessionData.Count > _maxDays)
                    hourlySessionData.RemoveAt(0);
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

                List<SessionWeekly> weeklySessionData = session.IsBot ? _weeklySessionDataBot : _weeklySessionDataHuman;
                SessionWeekly weekly = weeklySessionData.FirstOrDefault(w => w.Week.Equals(week) && w.Year == sessionDate.Year);

                if (weekly == null)
                {
                    weekly = new SessionWeekly();
                    weekly.Week = week;
                    weekly.Year = sessionDate.Year;
                    weeklySessionData.Add(weekly);
                }

                UpdateSessionData(session, weekly);

                while (weeklySessionData.Count > _maxWeeks)
                    weeklySessionData.RemoveAt(0);
            }
        }

        private void UpdateMonthlySessionData(UserSession session)
        {
            if (session == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                DateTime sessionDate = session.Created;

                List<SessionMonthly> monthlySessionData = session.IsBot ? _monthlySessionDataBot : _monthlySessionDataHuman;
                SessionMonthly monthly = monthlySessionData.FirstOrDefault(w => w.Month.Equals(sessionDate.Month) && w.Year == sessionDate.Year);

                if (monthly == null)
                {
                    monthly = new SessionMonthly();
                    monthly.Month = sessionDate.Month;
                    monthly.Year = sessionDate.Year;
                    monthlySessionData.Add(monthly);
                }

                UpdateSessionData(session, monthly);

                while (monthlySessionData.Count > _maxMonths)
                    monthlySessionData.RemoveAt(0);
            }
        }

        private void UpdateYearlySessionData(UserSession session)
        {
            if (session == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                DateTime sessionDate = session.Created;
                List<SessionYearly> yearlySessionData = session.IsBot ? _yearlySessionDataBot : _yearlySessionDataHuman;

                SessionYearly yearly = yearlySessionData
                    .FirstOrDefault(y => y.Year.Equals(sessionDate.Year));

                if (yearly == null)
                {
                    yearly = new SessionYearly();
                    yearly.Year = sessionDate.Year;
                    yearlySessionData.Add(yearly);
                }

                UpdateSessionData(session, yearly);

                while (yearlySessionData.Count > _maxYears)
                    yearlySessionData.RemoveAt(0);
            }
        }

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
                .FirstOrDefault(r => r.UserAgent.Equals(session.UserAgent) && r.IsBot == session.IsBot);

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
