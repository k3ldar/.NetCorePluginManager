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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

using Microsoft.AspNetCore.Hosting;

using Newtonsoft.Json;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using UserSessionMiddleware.Plugin.Classes.SessionData;

namespace UserSessionMiddleware.Plugin.Classes
{
    /// <summary>
    /// Default IUserSessionService implementation that will be used if no other IUserSessionService
    /// instances have been registered
    /// </summary>
    public sealed class DefaultUserSessionService : ThreadManager, IUserSessionService
    {
        #region Private Members

        private readonly string _rootPath;
        private static readonly object _lockObject = new object();
        private static readonly Stack<UserSession> _closedSessions = new Stack<UserSession>();
        private static SessionPageViews _sessionPageViews;
        private static SessionInitialReferrers _initialReferrers;
        private static List<SessionHourly> _hourlySessionData;
        private static List<SessionDaily> _dailySessionData;
        private static List<SessionWeekly> _weeklySessionData;
        private static List<SessionMonthly> _monthlySessionData;
        private static List<SessionYearly> _yearlySessionData;
        internal readonly static Timings _timingsDefaultSession = new Timings();
        private readonly IGeoIpProvider _geoIpProvider;
        private readonly ILogger _logger;
        private readonly string _pageViewFile;
        private readonly string _referrerFile;
        private readonly string _sessionHourlyFile;
        private readonly string _sessionDailyFile;
        private readonly string _sessionWeeklyFile;
        private readonly string _sessionMonthlyFile;
        private readonly string _sessionYearlyFile;
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

            UserSessionSettings settings = settingsProvider.GetSettings<UserSessionSettings>(Constants.UserSessionConfiguration);

            if (!settings.EnableDefaultSessionService)
                return;

            _enabled = true;

            ContinueIfGlobalException = true;

            if (String.IsNullOrEmpty(settings.SessionRootPath))
                settings.SessionRootPath = hostingEnvironment.ContentRootPath;

            _rootPath = Path.Combine(settings.SessionRootPath, "UserSession");

            _pageViewFile = GetFile(Path.Combine(_rootPath, "Sessions"), "PageViews.dat");
            _referrerFile = GetFile(Path.Combine(_rootPath, "Sessions"), "InitialReferrer.dat");
            _sessionHourlyFile = GetFile(Path.Combine(_rootPath, "Sessions"), "Hourly.dat");
            _sessionDailyFile = GetFile(Path.Combine(_rootPath, "Sessions"), "Daily.dat");
            _sessionWeeklyFile = GetFile(Path.Combine(_rootPath, "Sessions"), "Weekly.dat");
            _sessionMonthlyFile = GetFile(Path.Combine(_rootPath, "Sessions"), "Monthly.dat");
            _sessionYearlyFile = GetFile(Path.Combine(_rootPath, "Sessions"), "Yearly.dat");

            _maxHours = settings.MaxHourlyData;
            _maxDays = settings.MaxDailyData;
            _maxWeeks = settings.MaxWeeklyData;
            _maxMonths = settings.MaxMonthlyData;
            _maxYears = settings.MaxYearlyData;


            LoadSessionData(_pageViewFile, ref _sessionPageViews);
            LoadSessionData(_referrerFile, ref _initialReferrers);
            LoadSessionData(_sessionHourlyFile, ref _hourlySessionData);
            LoadSessionData(_sessionDailyFile, ref _dailySessionData);
            LoadSessionData(_sessionWeeklyFile, ref _weeklySessionData);
            LoadSessionData(_sessionMonthlyFile, ref _monthlySessionData);
            LoadSessionData(_sessionYearlyFile, ref _yearlySessionData);

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

        #region Internal Methods

        internal static string GetUrlHash(in string url)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return GetHash(sha256Hash, url.ToLower());
            }
        }

        internal static List<SessionDaily> GetDailyData()
        {
            return CopySessionData<List<SessionDaily>>(_dailySessionData);
        }

        internal static List<SessionHourly> GetHourlyData()
        {
            return CopySessionData<List<SessionHourly>>(_hourlySessionData);
        }

        internal static List<SessionWeekly> GetWeeklyData()
        {
            return CopySessionData<List<SessionWeekly>>(_weeklySessionData);
        }

        internal static List<SessionMonthly> GetMonthlyData()
        {
            return CopySessionData<List<SessionMonthly>>(_monthlySessionData);
        }

        internal static List<SessionYearly> GetYearlyData()
        {
            return CopySessionData<List<SessionYearly>>(_yearlySessionData);
        }

        internal static List<SessionUserAgent> GetUserAgents()
        {
            List<SessionYearly> yearlySessions = CopySessionData<List<SessionYearly>>(_yearlySessionData);

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

            return Result.OrderBy(o => o.IsBot).ThenByDescending(d => d.Count).ToList();
        }

        #endregion Internal Methods

        #region Private Methods

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

                SaveSessionData(_sessionHourlyFile, _hourlySessionData);
                SaveSessionData(_sessionDailyFile, _dailySessionData);
                SaveSessionData(_sessionWeeklyFile, _weeklySessionData);
                SaveSessionData(_sessionMonthlyFile, _monthlySessionData);
                SaveSessionData(_sessionYearlyFile, _yearlySessionData);

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

                SessionHourly hourly = _hourlySessionData
                    .Where(h => h.Date.Date.Equals(currentDate.Date) && h.Hour == hour && h.Quarter == quarter)
                    .FirstOrDefault();

                if (hourly == null)
                {
                    hourly = new SessionHourly();
                    hourly.Date = currentDate.Date;
                    hourly.Hour = hour;
                    hourly.Quarter = quarter;
                    _hourlySessionData.Add(hourly);
                }

                UpdateSessionData(session, hourly);

                while (_hourlySessionData.Count > _maxHours)
                    _hourlySessionData.RemoveAt(0);
            }
        }

        private void UpdateDailySessionData(UserSession session)
        {
            if (session == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                DateTime sessionDate = session.Created;

                SessionDaily daily = _dailySessionData
                    .Where(h => h.Date.Date.Equals(sessionDate.Date))
                    .FirstOrDefault();

                if (daily == null)
                {
                    daily = new SessionDaily();
                    daily.Date = sessionDate.Date;
                    _dailySessionData.Add(daily);
                }

                UpdateSessionData(session, daily);

                while (_dailySessionData.Count > _maxDays)
                    _dailySessionData.RemoveAt(0);
            }
        }

        private void UpdateWeeklySessionData(UserSession session)
        {
            if (session == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                DateTime sessionDate = session.Created;

#if NET_CORE_3X
                int week = ISOWeek.GetWeekOfYear(sessionDate);
#else
                int week = (sessionDate.DayOfYear / 7) + 1;
#endif

                SessionWeekly weekly = _weeklySessionData
                    .Where(w => w.Week.Equals(week) && w.Year == sessionDate.Year)
                    .FirstOrDefault();

                if (weekly == null)
                {
                    weekly = new SessionWeekly();
                    weekly.Week = week;
                    weekly.Year = sessionDate.Year;
                    _weeklySessionData.Add(weekly);
                }

                UpdateSessionData(session, weekly);

                while (_weeklySessionData.Count > _maxWeeks)
                    _weeklySessionData.RemoveAt(0);
            }
        }

        private void UpdateMonthlySessionData(UserSession session)
        {
            if (session == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                DateTime sessionDate = session.Created;

                SessionMonthly monthly = _monthlySessionData
                    .Where(w => w.Month.Equals(sessionDate.Month) && w.Year == sessionDate.Year)
                    .FirstOrDefault();

                if (monthly == null)
                {
                    monthly = new SessionMonthly();
                    monthly.Month = sessionDate.Month;
                    monthly.Year = sessionDate.Year;
                    _monthlySessionData.Add(monthly);
                }

                UpdateSessionData(session, monthly);

                while (_monthlySessionData.Count > _maxMonths)
                    _monthlySessionData.RemoveAt(0);
            }
        }

        private void UpdateYearlySessionData(UserSession session)
        {
            if (session == null)
                return;

            using (TimedLock timedLock = TimedLock.Lock(_lockObject))
            {
                DateTime sessionDate = session.Created;

                SessionYearly yearly = _yearlySessionData
                    .Where(y => y.Year.Equals(sessionDate.Year))
                    .FirstOrDefault();

                if (yearly == null)
                {
                    yearly = new SessionYearly();
                    yearly.Year = sessionDate.Year;
                    _yearlySessionData.Add(yearly);
                }

                UpdateSessionData(session, yearly);

                while (_yearlySessionData.Count > _maxYears)
                    _yearlySessionData.RemoveAt(0);
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

            if (session.Bounced)
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
