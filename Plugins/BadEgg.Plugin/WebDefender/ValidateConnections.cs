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
 *  Copyright (c) 2012 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  BadEgg.Plugin
 *  
 *  File: ValidateConnections.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  9/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

using SharedPluginFeatures;

using static System.Web.HttpUtility;
using static SharedPluginFeatures.Enums;

namespace BadEgg.Plugin.WebDefender
{
    internal sealed class ValidateConnections : ThreadManager
    {
        #region Private Members / Constants

        private const byte MinimumSpiderUniqueRequests = 95;
        private const int DefaultConnectionTimeoutMinutes = 5;
        private const int DefaultHackProbability = 200;
        private const int DefaultHackAttempt = 700;
        private const int DefaultBotHitsPerSecondProbability = 3;
        private const int DefaultBotHitsPerSecond = 10;
        private const int RemoveExpiredConnectionInterval = 10;
        private const int WeightMultiplier = 16;

        /// <summary>
        /// keywords used to determine sql injection
        /// </summary>
        private readonly string[] KeyWords = { "all", "alter", "and", "any", "array", "arrow", "as", "asc", "at", "begin", "between",
            "by", "case", "check", "clusters", "cluster", "colauth", "columns", "compress", "connect", "crash", "create", "current",
            "decimal", "declare", "default", "delete", "desc", "distinct", "drop", "else", "end", "exception", "exclusive", "exists",
            "fetch", "form", "for", "from", "goto", "grant", "group", "having", "identified", "if", "in", "indexes", "index", "insert",
            "intersect", "into", "is", "like", "lock", "minus", "mode", "nocompress", "not", "nowait", "null", "of", "on", "option",
            "or", "order", "overlaps", "prior", "procedure", "public", "range", "record", "resource", "revoke", "select", "share",
            "size", "sql", "start", "subtype", "tabauth", "table", "then", "to", "type", "union", "unique", "update", "use", "values",
            "view", "views", "when", "where", "with", "char", "user", "password", "--", "/*", "*/", "declare", "cursor", "varchar",
            "checksum", "replace", "abs", "data_type", "sysobjects", "sysindexes", "inner", "outer", "character_maximum_length",
            "sa", "[", "]", "(", ")" };

        /// <summary>
        /// Phrases to find / Replace within a string
        /// </summary>
        private readonly string[] PhraseFind = { "0x31303235343830303536", ",", "(", ")", "--", "&", "=", "?", "_", "]", "[" };
        private readonly string[] PhraseReplace = { "null", " ", " ( ", " ) ", " -- ", " ", " = ", " ? ", " _ ", " ] ", " [ " };

        /// <summary>
        /// Hacking keywords
        /// </summary>
        private readonly string[] HackKeyWords = { "safe", "prepend", "disable", "function", "open", "allow", "include", "file",
            "proc", "self", "environ", "editor", "browser", "filemanager", "include", "connector", ".. /", "../", "file",
            "manager", "provider", "null", "plugin", "src", "ckfinder", "core", "/ aspx /", "usg", "sa", "ved", "ei",
            "script" };

        /// <summary>
        /// Hacking phrases to find/replace
        /// </summary>
        private readonly string[] HackFind = { "?", "_", "-", "/", "=", "<", "/>" };
        private readonly string[] HackReplace = { " ? ", " _ ", " - ", " / ", " = ", " < ", " / > " };

        /// <summary>
        /// Words/Chars to replace with a space in Random Word checker
        /// </summary>
        private readonly string[] RandomFind = { "=" };

        /// <summary>
        /// Address list lock object for unique access
        /// </summary>
        private static readonly object _lockObject = new object();

        private static readonly Dictionary<string, IpConnectionInfo> _connectionInformation = new();

        private readonly HashSet<IpConnectionInfo> _connectionsAdd;

        private readonly object _eventLockObject = new object();

        internal readonly uint InternalMaximumConnectionsPerSecond;

        private ulong _iteration = 0;

        internal static readonly Dictionary<string, bool> InternalIpAddressList = new Dictionary<string, bool>();

        internal static readonly object InternalIpAddressLock = new object();

        /// <summary>
        /// Period of inactivity in which a connection times out
        /// </summary>
        public TimeSpan ConnectionTimeout { get; }

        private readonly HashSet<ConnectionReportEventArgs> _reports = new HashSet<ConnectionReportEventArgs>();

        private readonly object _reportsLock = new object();

        #endregion Private Members / Constants

        #region Constructors

        internal ValidateConnections()
            : this(new TimeSpan(0, DefaultConnectionTimeoutMinutes, 0), uint.MaxValue)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal ValidateConnections(in TimeSpan connectionTimeout, in uint maximumConnectionsPerSecond)
            : base(null, new TimeSpan(0, 0, 1), null, 1000, 200, false, true)
        {
            ContinueIfGlobalException = true;

            _connectionsAdd = new HashSet<IpConnectionInfo>();
            ConnectionTimeout = connectionTimeout;
            InternalMaximumConnectionsPerSecond = maximumConnectionsPerSecond;
            HackProbability = DefaultHackProbability;
            HackAttempt = DefaultHackAttempt;
            BotHitsPerSecondProbability = DefaultBotHitsPerSecondProbability;
            BotHitsPerSecond = DefaultBotHitsPerSecond;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Probability of web attack/hacking
        /// </summary>
        public int HackProbability { get; set; }

        /// <summary>
        /// Almost certain web attack/hacking
        /// </summary>
        public int HackAttempt { get; set; }

        /// <summary>
        /// Propability of bot Number of hit's per second 
        /// </summary>
        public int BotHitsPerSecondProbability { get; set; }

        /// <summary>
        /// Propability of bot Number of hit's per second 
        /// </summary>
        public int BotHitsPerSecond { get; set; }


        #endregion Properties

        #region Overridden Methods

        protected override bool Run(object parameters)
        {
            return ProcessAllConnectionData();
        }

        #endregion Overridden Methods

        #region Public Methods

        public bool ProcessAllConnectionData()
        {
            ValidateAndBanIPAddresses();

            // every 10th iteration, remove expired connections
            if (++_iteration % RemoveExpiredConnectionInterval == 0)
            {
                HashSet<IpConnectionInfo> removedConnections = new HashSet<IpConnectionInfo>();

                //remove expired connections
                using (TimedLock lck = TimedLock.Lock(_lockObject))
                {
                    if (_connectionInformation != null)
                    {
                        List<KeyValuePair<string, IpConnectionInfo>> keys = _connectionInformation
                            .Where(p => p.Value.LastEntry.AddMilliseconds(ConnectionTimeout.TotalMilliseconds) < DateTime.Now).ToList();
                        try
                        {
                            foreach (KeyValuePair<string, IpConnectionInfo> item in keys)
                            {
                                removedConnections.Add(item.Value);
                                _connectionInformation.Remove(item.Key);
                            }
                        }
                        finally
                        {
                            keys = null;
                        }
                    }
                }

                // raise events outside of the lock
                foreach (IpConnectionInfo connection in removedConnections)
                {
                    RaiseConnectionRemoved(connection);
                }
            }

            // add/remove events done using seperate list so as not to slow down processing
            // when connections add/remove
            HashSet<IpConnectionInfo> _eventList = new HashSet<IpConnectionInfo>();

            using (TimedLock lck = TimedLock.Lock(_eventLockObject))
            {
                foreach (IpConnectionInfo ipConnection in _connectionsAdd)
                    _eventList.Add(ipConnection);

                _connectionsAdd.Clear();
            }

            // raise connect events
            foreach (IpConnectionInfo conn in _eventList)
                RaiseConnectionAdd(conn);

            using (TimedLock lck = TimedLock.Lock(_reportsLock))
            {
                foreach (ConnectionReportEventArgs item in _reports)
                {
                    RaiseReportEvent(item.IPAddress, item.QueryString, item.Result);
                }

                _reports.Clear();
            }

            return !HasCancelled();
        }

        /// <summary>
        /// Validates a string
        /// </summary>
        /// <param name="request">String request being validated</param>
        /// <param name="count">number of occurances of *possible* attacks detected in the string </param>
        /// <param name="hostAddress">IP Address for connection</param>
        /// <returns>Results of probability that the attack is an attempt hack</returns>
        public ValidateRequestResult ValidateRequest(in string request, in string hostAddress, out int count)
        {
            count = 0;

            //verify against white/black listed addresses
            ValidateRequestResult Result = VerifyAddress(hostAddress);

            IpConnectionInfo info = GetConnectionInformation(hostAddress);

            // add previous results
            Result |= info.Results;

            // increment the number of requests
            info.AddRequest(UrlDecode(request).ToLower(), String.Empty);

            if (Result.HasFlag(ValidateRequestResult.IpBlackListed) || Result.HasFlag(ValidateRequestResult.IpWhiteListed))
                return Result;

            //Determine if a SQL Injection Attack
            DetermineSQLInjectionAttack(request, ref count, ref Result);

            //Determine a hack attempt
            DetermineHackAttemt(request, ref count, ref Result);

            //Is there a spider/bot at work
            DetermineSpiderBot(info, ref Result);

            UpdateConnectionInfo(ref Result, info);

            ReportWebData(hostAddress, request, Result);

            return Result;
        }

        /// <summary>
        /// Validates a web request
        /// </summary>
        /// <param name="request">Request being validated</param>
        /// <param name="validatePostValues">Validates form post values if available</param>
        /// <param name="count">number of occurances of *possible* attacks detected in the Uri </param>
        /// <returns>Weight of probability that the attack is an attempt hack</returns>
        public ValidateRequestResult ValidateRequest(in HttpRequest request, in bool validatePostValues, out int count)
        {
            count = 0;
            string hostAddress = GetIpAddress(request);

            //verify against white/black listed addresses
            ValidateRequestResult Result = VerifyAddress(hostAddress);

            if (Result.HasFlag(ValidateRequestResult.IpBlackListed) || Result.HasFlag(ValidateRequestResult.IpWhiteListed))
                return Result;

            string url = request.Path;
            string physicalPath = request.PathBase.ToString();
            string agent = request.Headers["User-Agent"].ToString();
            string query = UrlDecode(request.QueryString.ToString()).ToLower();

            if (validatePostValues && request.HasFormContentType)
            {
                string postValues = String.Empty;

                foreach (string value in request.Form.Keys)
                    if (!IgnoreFormValue(value))
                        postValues += $"{request.Form[value]} ";

                query += $" {postValues}";
            }

            IpConnectionInfo info = GetConnectionInformation(hostAddress);

            // add previous results
            Result |= info.Results;

            // increment the number of requests
            info.AddRequest(url + query, agent);

            //Determine if a SQL Injection Attack
            DetermineSQLInjectionAttack(query, ref count, ref Result);

            //Determine a hack attempt
            DetermineHackAttemt(UrlDecode(request.QueryString.ToString()), ref count, ref Result);

            //if the file does not exist then check for a hack attempt in the url
            if (!String.IsNullOrEmpty(physicalPath) && !System.IO.File.Exists(physicalPath))
                DetermineHackAttemt(url, ref count, ref Result);

            //Is there a spider/bot is at work
            DetermineSpiderBot(info, ref Result);

            //it has some weight so report it
            ReportWebData(hostAddress, query, Result);

            UpdateConnectionInfo(ref Result, info);


            return Result;
        }

		/// <summary>
		/// Adds an IP address to a black list
		/// </summary>
		/// <param name="ipAddress">Ip address to add to black list</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Public and used in other places")]
		public void AddToBlackList(in string ipAddress)
        {
            if (String.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress));

            using (TimedLock lockobj = TimedLock.Lock(InternalIpAddressLock))
            {
                InternalIpAddressList[ipAddress] = true;
            }
        }

		/// <summary>
		/// Adds an IP address to a black list
		/// </summary>
		/// <param name="ipAddress">Ip address to add to black list</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Public and used in other places")]
		public void AddToWhiteList(in string ipAddress)
        {
            if (String.IsNullOrEmpty(ipAddress))
                throw new ArgumentNullException(nameof(ipAddress));

            using (TimedLock lockobj = TimedLock.Lock(InternalIpAddressLock))
            {
                InternalIpAddressList[ipAddress] = false;
            }
        }

        /// <summary>
        /// Gets connection information object for the IP Address
        /// </summary>
        /// <param name="ipAddress">IP Address</param>
        /// <returns>ConnectionInfo object associated with the IP Address</returns>
        public IpConnectionInfo GetConnectionInformation(in string ipAddress)
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                if (!_connectionInformation.ContainsKey(ipAddress))
                {
                    IpConnectionInfo newConnection = new IpConnectionInfo(ipAddress);
                    _connectionInformation.Add(ipAddress, newConnection);

                    using (TimedLock elck = TimedLock.Lock(_eventLockObject))
                        _connectionsAdd.Add(newConnection);
                }

                return _connectionInformation[ipAddress];
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal static void InternalClearAllConnectionInformation()
        {
            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                _connectionInformation?.Clear();
            }

            using (TimedLock lockobj = TimedLock.Lock(InternalIpAddressLock))
            {
                InternalIpAddressList.Clear();
            }
        }

        internal static string GetMemoryStatus()
        {
            StringBuilder Result = new StringBuilder();

            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                foreach (KeyValuePair<string, IpConnectionInfo> item in _connectionInformation)
                {
                    Result.Append('\r');
                    Result.Append(item.Value.ToString());
                }
            }

            return Result.ToString().Trim();
        }

        #endregion Internal Methods

        #region Events Raise Methods

        private void RaiseReportEvent(in string ipAddress, in string queryString, in ValidateRequestResult validation)
        {
            OnReportConnection?.Invoke(this, new ConnectionReportEventArgs(ipAddress, queryString, validation));
        }

        /// <summary>
        /// Raise connection removed event
        /// </summary>
        /// <param name="connection">connection info being removed</param>
        private void RaiseConnectionAdd(in IpConnectionInfo connection)
        {
            ConnectionAdd?.Invoke(this, new ConnectionEventArgs(connection.IPAddress));
        }

        /// <summary>
        /// Raise connection removed event
        /// </summary>
        /// <param name="connection">connection info being removed</param>
        private void RaiseConnectionRemoved(in IpConnectionInfo connection)
        {
            ConnectionRemove?.Invoke(this, new ConnectionRemoveEventArgs(connection.IPAddress,
                connection.HitsPerMinute(), connection.Requests,
                connection.Created - connection.LastEntry));
        }

        /// <summary>
        /// Raises an event for banning an IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="hits"></param>
        /// <param name="requests"></param>
        /// <param name="span"></param>
        /// <returns>True if the IP Address should be black listed and added to banned list, otherwise false</returns>
        private bool RaiseOnBanIPAddress(in string ipAddress, in double hits, in ulong requests, in TimeSpan span)
        {
            RequestBanEventArgs args = new RequestBanEventArgs(ipAddress, hits, requests, span);

            OnBanIPAddress?.Invoke(null, args);

            return args.AddToBlackList;
        }

        #endregion Event Raise Methods

        #region Events

        /// <summary>
        /// Event raised when an IP Address needs to be banned
        /// </summary>
        public event DefenderRequestBan OnBanIPAddress;

        /// <summary>
        /// Event raised when connection info object add
        /// </summary>
        public event DefenderConnectionAddEventHandler ConnectionAdd;

        /// <summary>
        /// Event raised when connection info object removed
        /// </summary>
        public event DefenderConnectionRemoveEventHandler ConnectionRemove;

        /// <summary>
        /// Reports a connection that has Result attributes
        /// </summary>
        public event DefenderReportConnection OnReportConnection;

        #endregion Events

        #region Private Methods

        private static string GetIpAddress(HttpRequest request)
        {
            foreach (string key in Constants.ForwardForHeader)
                if (request.Headers.ContainsKey(key))
                    return request.Headers[key];

            return request.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        /// <summary>
        /// Certain form values can be ignored and will not be evaluated
        /// </summary>
        /// <param name="value"></param>
        /// <returns>true if value should be ignored, otherwise false</returns>
        private static bool IgnoreFormValue(in string value)
        {
            switch (value)
            {
                case "__RequestVerificationToken":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Updates result flags on connection info object
        /// </summary>
        /// <param name="Result">Result of this validation</param>
        /// <param name="connection">connection info object</param>
        private static void UpdateConnectionInfo(ref ValidateRequestResult Result, in IpConnectionInfo connection)
        {
            // spider bot
            if (Result.HasFlag(ValidateRequestResult.SpiderBot) && !connection.Results.HasFlag(ValidateRequestResult.SpiderBot))
            {
                connection.Results |= ValidateRequestResult.SpiderBot;
            }
            else if (!Result.HasFlag(ValidateRequestResult.SpiderBot) &&
                Result.HasFlag(ValidateRequestResult.PossibleSpiderBot) && !connection.Results.HasFlag(ValidateRequestResult.PossibleSpiderBot))
            {
                connection.Results |= ValidateRequestResult.PossibleSpiderBot;
            }

            if (Result.HasFlag(ValidateRequestResult.PossibleSpiderBot) && Result.HasFlag(ValidateRequestResult.SpiderBot))
            {
                Result &= ~ValidateRequestResult.PossibleSpiderBot;
            }

            // sql injection
            if (Result.HasFlag(ValidateRequestResult.SQLInjectionAttack) && !connection.Results.HasFlag(ValidateRequestResult.SQLInjectionAttack))
            {
                connection.Results |= ValidateRequestResult.SQLInjectionAttack;
            }
            else if (Result.HasFlag(ValidateRequestResult.PossibleSQLInjectionAttack) && !connection.Results.HasFlag(ValidateRequestResult.PossibleSQLInjectionAttack))
            {
                connection.Results |= ValidateRequestResult.PossibleSQLInjectionAttack;
            }

            // hack attempt
            if (Result.HasFlag(ValidateRequestResult.HackAttempt) && !connection.Results.HasFlag(ValidateRequestResult.HackAttempt))
            {
                connection.Results |= ValidateRequestResult.HackAttempt;
            }
            else if (Result.HasFlag(ValidateRequestResult.PossibleHackAttempt) && !connection.Results.HasFlag(ValidateRequestResult.PossibleHackAttempt))
            {
                connection.Results |= ValidateRequestResult.PossibleHackAttempt;
            }

            // if connection info previously holds info about spiderbot, add this to the result
            if (!Result.HasFlag(ValidateRequestResult.SpiderBot) && connection.Results.HasFlag(ValidateRequestResult.SpiderBot))
            {
                Result |= ValidateRequestResult.SpiderBot;
            }
            else if (!Result.HasFlag(ValidateRequestResult.PossibleSpiderBot) && connection.Results.HasFlag(ValidateRequestResult.PossibleSpiderBot) && !Result.HasFlag(ValidateRequestResult.SpiderBot))
            {
                Result |= ValidateRequestResult.PossibleSpiderBot;
            }

            //remove undetermined if other values exist
            if ((int)Result > (int)ValidateRequestResult.Undetermined)
                Result &= ~ValidateRequestResult.Undetermined;
        }

        private void ValidateAndBanIPAddresses()
        {
            HashSet<IpConnectionInfo> banRequests = new HashSet<IpConnectionInfo>();

            using (TimedLock lck = TimedLock.Lock(_lockObject))
            {
                foreach (KeyValuePair<string, IpConnectionInfo> item in _connectionInformation)
                {
                    // if the connection has too many hits per minute, add the flag, otherwise remove it
                    double hitsPerSecond = item.Value.Requests / (item.Value.TotalTime().TotalSeconds + 1);
                    bool hasTooManyRequests = item.Value.Results.HasFlag(ValidateRequestResult.TooManyRequests);

                    if (hitsPerSecond > InternalMaximumConnectionsPerSecond && !hasTooManyRequests)
                        item.Value.Results |= ValidateRequestResult.TooManyRequests;
                    else if (hitsPerSecond < InternalMaximumConnectionsPerSecond && hasTooManyRequests)
                        item.Value.Results &= ~ValidateRequestResult.TooManyRequests;

                    // if the connection has hackattempt flag, log it for banning
                    if (item.Value.Results.HasFlag(ValidateRequestResult.HackAttempt) || 
                        item.Value.Results.HasFlag(ValidateRequestResult.SQLInjectionAttack))
                    {
                        // NEVER ban a local IP Address
                        if (Shared.Utilities.LocalIPAddress(item.Value.IPAddress))
                            continue;

                        if (!item.Value.Results.HasFlag(ValidateRequestResult.BanRequested))
                        {
                            item.Value.Results = ValidateRequestResult.BanRequested;
                            banRequests.Add(item.Value);
                        }
                    }
                }
            }

            foreach (IpConnectionInfo connection in banRequests)
            {
                if (RaiseOnBanIPAddress(connection.IPAddress, connection.HitsPerMinute(),
                    connection.Requests, connection.TotalTime()))
                {
                    if (!connection.Results.HasFlag(ValidateRequestResult.IpBlackListed))
                        connection.Results |= ValidateRequestResult.IpBlackListed;

                    if (connection.Results.HasFlag(ValidateRequestResult.BanRequested))
                        connection.Results &= ~ValidateRequestResult.BanRequested;

                    using (TimedLock lck = TimedLock.Lock(InternalIpAddressLock))
                    {
                        if (!InternalIpAddressList.ContainsKey(connection.IPAddress))
                            InternalIpAddressList.Add(connection.IPAddress, true);
                    }
                }
            }
        }

        /// <summary>
        /// Verify's an address to see if it's black/white listed
        /// </summary>
        /// <param name="ipAddress">Ip address to verify</param>
        /// <returns>ValidateRequestResult enum with results</returns>
        private static ValidateRequestResult VerifyAddress(in string ipAddress)
        {
            using (TimedLock lockobj = TimedLock.Lock(InternalIpAddressLock))
            {
                if (InternalIpAddressList.ContainsKey(ipAddress))
                {
                    if (InternalIpAddressList[ipAddress])
                        return ValidateRequestResult.IpBlackListed;
                    else
                        return ValidateRequestResult.IpWhiteListed;
                }
            }

            return ValidateRequestResult.Undetermined;
        }

        private void ReportWebData(in string ipAddress, in string queryString, in ValidateRequestResult validation)
        {
            using (TimedLock lck = TimedLock.Lock(_reportsLock))
            {
                _reports.Add(new ConnectionReportEventArgs(ipAddress, queryString, validation));
            }
        }

        /// <summary>
        /// Determines the probability that user is bot/spider 
        /// </summary>
        /// <param name="connectionInfo">Ip connection information</param>
        /// <param name="validation">validation Results</param>
        private void DetermineSpiderBot(in IpConnectionInfo connectionInfo, ref ValidateRequestResult validation)
        {
            // look at how many hits from a single location
            double hitsPerSecond = connectionInfo.HitsPerSecond();
            double uniquePages = connectionInfo.UniquePages();

            if (connectionInfo.Requests > 3 && uniquePages > MinimumSpiderUniqueRequests)
            {
                if (hitsPerSecond > BotHitsPerSecond)
                    validation |= ValidateRequestResult.SpiderBot;
                else if (hitsPerSecond > BotHitsPerSecondProbability)
                    validation |= ValidateRequestResult.PossibleSpiderBot;
            }
        }

        /// <summary>
        /// Determines the probability of a hacking attempt
        /// </summary>
        /// <param name="request">data to be checked</param>
        /// <param name="count">Number of attempts found</param>
        /// <param name="validation">Result of the check</param>
        private void DetermineHackAttemt(in string request, ref int count, ref ValidateRequestResult validation)
        {
            int weight = 0;

            string url = UrlDecode(request).ToLower();

            if (String.IsNullOrEmpty(url.Trim()) || url == "/")
                return;

            for (int i = 0; i < HackFind.Length; i++)
            {
                url = url.Replace(HackFind[i], HackReplace[i]);
            }

            foreach (string keyWord in HackKeyWords)
            {
                string word = keyWord;
                int n = 0;

                while ((n = url.IndexOf(word, n, StringComparison.InvariantCulture)) != -1)
                {
                    n += word.Trim().Length;
                    count++;
                    weight += count * WeightMultiplier;

                    if (weight > HackAttempt)
                        break;

                }

                if (weight > HackAttempt)
                    break;
            }

            // if it's a hack are random words being used as parameters
            if (weight < HackAttempt)
            {
                string decodedRequest = UrlDecode(request);

                for (int i = 0; i < RandomFind.Length; i++)
                {
                    decodedRequest = decodedRequest.Replace(RandomFind[i], "&");
                }

                string[] randomWords = decodedRequest.Split('&');

                foreach (string word in randomWords)
                {
                    if (IsRandomWord(word.Trim()))
                    {
                        count++;
                        weight += count * WeightMultiplier;

                        if (weight > HackAttempt)
                            break;
                    }
                }
            }



            if (weight >= HackAttempt)
                validation |= ValidateRequestResult.HackAttempt;
            else if (weight >= HackProbability)
                validation |= ValidateRequestResult.PossibleHackAttempt;
        }

        /// <summary>
        /// Determines if the url contains sql which could be a SQL Injection
        /// </summary>
        /// <param name="request">current request data</param>
        /// <param name="count">number of attempts found</param>
        /// <param name="validation">Result of the check</param>
        private void DetermineSQLInjectionAttack(in string request, ref int count, ref ValidateRequestResult validation)
        {
            int weight = 0;

            string url = String.Format(" {0} ", UrlDecode(request).ToLower());

            if (String.IsNullOrEmpty(url.Trim()))
                return;

            for (int i = 0; i < PhraseFind.Length; i++)
            {
                url = url.Replace(PhraseFind[i], PhraseReplace[i]);
            }

            foreach (string keyWord in KeyWords)
            {
                string word = String.Format(" {0} ", keyWord);
                int n = 0;

                while ((n = url.IndexOf(word, n, StringComparison.InvariantCulture)) != -1)
                {
                    n += word.Trim().Length;
                    count++;
                    weight += count * WeightMultiplier;

                    if (weight > HackAttempt)
                        break;

                }

                if (weight > HackAttempt)
                    break;
            }

            if (weight >= HackAttempt)
                validation |= ValidateRequestResult.SQLInjectionAttack;
            else if (weight >= HackProbability)
                validation |= ValidateRequestResult.PossibleSQLInjectionAttack;
        }

        /// <summary>
        /// Determines if a word is made up of random characters
        /// 
        /// Assumption is if has 2 or more upper/lower case chars and at least 1 number then it's random
        /// </summary>
        /// <param name="word">Word which is being tested</param>
        /// <returns>true if contains 2 or more upper/lower case chars and at least 1 number, otherwise false</returns>
        private static bool IsRandomWord(in string word)
        {
            bool Result = false;

            int lower = 0;
            int upper = 0;
            int number = 0;

            foreach (char c in word)
            {
                if (char.IsNumber(c))
                    number++;
                else if (char.IsUpper(c))
                    upper++;
                else if (char.IsLower(c))
                    lower++;

                if (lower > 1 && upper > 1 && number > 0)
                {
                    Result = true;
                    break;
                }
            }

            return Result;
        }

        #endregion Private Methods
    }
}
