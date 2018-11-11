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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
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
using System.Text;
using System.Linq;
using static System.Web.HttpUtility;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

using static SharedPluginFeatures.Enums;

namespace BadEgg.Plugin.WebDefender
{
    internal sealed class ValidateConnections : ThreadManager
    {
        #region Private Members / Constants

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
            "checksum", "replace", "abs", "when", "data_type", "sysobjects", "sysindexes", "inner", "outer", "character_maximum_length",
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
            "manager", "provider", "null", "plugin", "src", "ckfinder", "core", "/ aspx /", "usg", "sa", "ved", "ei" };

        /// <summary>
        /// Hacking phrases to find/replace
        /// </summary>
        private readonly string[] HackFind = { "?", "_", "-", "/", "=" };
        private readonly string[] HackReplace = { " ? ", " _ ", " - ", " / ", " = " };

        /// <summary>
        /// Words/Chars to replace with a space in Random Word checker
        /// </summary>
        private readonly string[] RandomFind = { "=" };

        /// <summary>
        /// Address list lock object for unique access
        /// </summary>
        private static object _lockObject = new object();

        private static Dictionary<string, IpConnectionInfo> _connectionInformation;

        private uint _maximumConnectionsPerSecond;

        private ulong _iteration = 0;

        /// <summary>
        /// Time out for each client (minutes)
        /// </summary>
        private uint _connectionTimeoutMinutes { get; set; } = 5;

        #endregion Private Members / Constants

        #region Constructors

        internal ValidateConnections()
            : this (10, uint.MaxValue)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        internal ValidateConnections(uint connectionTimeoutMinutes, uint maximumConnectionsPerSecond)
            : base(null, new TimeSpan(0, 0, 1), null, 1000, 200, false, true)
        {
            ContinueIfGlobalException = true;

            if (_connectionInformation == null)
            {
                _connectionInformation = new Dictionary<string, IpConnectionInfo>();
            }

            _connectionTimeoutMinutes = connectionTimeoutMinutes;
            _maximumConnectionsPerSecond = maximumConnectionsPerSecond;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Probability of web attack/hacking
        /// </summary>
        private int HackProbability { get; set; } = 200;

        /// <summary>
        /// Almost certain web attack/hacking
        /// </summary>
        private int HackAttempt { get; set; } = 700;

        /// <summary>
        /// Propability of bot Number of hit's per second 
        /// </summary>
        private int BotHitsPerSecondProbability { get; set; } = 2;

        /// <summary>
        /// Propability of bot Number of hit's per second 
        /// </summary>
        private int BotHitsPerSecond { get; set; } = 10;


        private byte MinimumSpiderUniqueRequests = 95;

        #endregion Properties

        #region Overridden Methods

        protected override bool Run(object parameters)
        {
            ValidateAndBanIPAddresses();

            // every 10 seconds, remove expired connections
            if (++_iteration % 10 == 0)
            {
                HashSet<IpConnectionInfo> removedConnections = new HashSet<IpConnectionInfo>();

                //remove expired connections
                using (TimedLock.Lock(_lockObject))
                {
                    if (_connectionInformation != null)
                    {
                        List<KeyValuePair<string, IpConnectionInfo>> keys = _connectionInformation
                            .Where(p => p.Value.LastEntry.AddMinutes(_connectionTimeoutMinutes) < DateTime.Now).ToList();
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

            return !HasCancelled();
        }

        #endregion Overridden Methods

        #region Public Methods

        /// <summary>
        /// Validates a string
        /// </summary>
        /// <param name="request">String request being validated</param>
        /// <param name="count">number of occurances of *possible* attacks detected in the string </param>
        /// <param name="hostAddress">IP Address for connection</param>
        /// <returns>Results of probability that the attack is an attempt hack</returns>
        public ValidateRequestResult ValidateRequest(string request, out int count, string hostAddress)
        {
            ValidateRequestResult Result = ValidateRequestResult.Undetermined;
            count = 0;

            //verify against white/black listed addresses
            Result = VerifyAddress(hostAddress);

            IpConnectionInfo info = GetConnectionInformation(hostAddress);

            // add previous results
            Result |= info.Results;

            // increment the number of requests
            info.AddRequest(UrlDecode(request).ToLower(), String.Empty);

            if (Result.HasFlag(ValidateRequestResult.IPBlackListed) || Result.HasFlag(ValidateRequestResult.IPWhiteListed))
                return (Result);

            //Determine if a SQL Injection Attack
            DetermineSQLInjectionAttack(request, ref count, ref Result);

            //Determine a hack attempt
            DetermineHackAttemt(request, ref count, ref Result);

            //Is there a spider/bot is at work
            DetermineSpiderBot(info, ref Result);

            //it has some weight so report it
            ReportWebData(hostAddress, request, Result);

#warning does it save automatically
            // save the object back into the dictionary
            //_connectionInformation.UpdateConnectionInfo(ref Result, info);

            //remove undetermined if other values exist
            if ((int)Result > (int)ValidateRequestResult.Undetermined)
                Result &= ~ValidateRequestResult.Undetermined;

            return (Result);
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
            string url = request.Path;
            string hostAddress = request.HttpContext.Connection.RemoteIpAddress.ToString();
            string physicalPath = request.PathBase.ToString();
            string agent = request.Headers["User-Agent"].ToString();
            string query = UrlDecode(request.QueryString.ToString()).ToLower();
            string postValues = String.Empty;

            if (validatePostValues && request.HasFormContentType)
            {
                foreach (string value in request.Form.Keys)
                    postValues += $"{request.Form[value]} ";
            }

            ValidateRequestResult Result = ValidateRequestResult.Undetermined;
            count = 0;

            //verify against white/black listed addresses
            Result = VerifyAddress(hostAddress);

            if (Result.HasFlag(ValidateRequestResult.IPBlackListed) || Result.HasFlag(ValidateRequestResult.IPWhiteListed))
                return (Result);

            IpConnectionInfo info = GetConnectionInformation(hostAddress);

            // add previous results
            Result |= info.Results;

            // increment the number of requests
            info.AddRequest(url + query, agent);

            //Determine if a SQL Injection Attack
            DetermineSQLInjectionAttack(query, ref count, ref Result);

            //Determine a hack attempt
            DetermineHackAttemt(query, ref count, ref Result);

            //if the file does not exist then check for a hack attempt in the url
            if (!System.IO.File.Exists(physicalPath))
                DetermineHackAttemt(url, ref count, ref Result);

            //Is there a spider/bot is at work
            DetermineSpiderBot(info, ref Result);

            //it has some weight so report it
            ReportWebData(hostAddress, query, Result);

#warning does it save automatically
            UpdateConnectionInfo(ref Result, info);

            //remove undetermined if other values exist
            if ((int)Result > 2)
                Result &= ~ValidateRequestResult.Undetermined;

            return (Result);
        }

        /// <summary>
        /// Adds an IP address to a black list
        /// </summary>
        /// <param name="hostAddress">IP address to add to black list</param>
        public void AddToBlackList(string ipAddress, string description, bool canExpire = true)
        {
#warning finish
            //IPAddress address = _addressList.Find(ipAddress);

            //if (address == null)
            //    _addressList.Add(new IPAddress(-1, ipAddress, canExpire, true, description, false, DateTime.Now, AddressType.UserDefined, true, String.Empty));
        }

        /// <summary>
        /// Adds an IP address to a black list
        /// </summary>
        /// <param name="hostAddress">IP address to add to black list</param>
        public void AddToWhiteList(string ipAddress, string description, bool canExpire = true, bool searchEngine = false)
        {
#warning finish
            //if (_addressList.Find(ipAddress) == null)
            //    _addressList.Add(new IPAddress(-1, ipAddress, canExpire, false, description, searchEngine, DateTime.Now, AddressType.UserDefined, true, String.Empty));
        }

        /// <summary>
        /// Gets connection information object for the IP Address
        /// </summary>
        /// <param name="ipAddress">IP Address</param>
        /// <returns>ConnectionInfo object associated with the IP Address</returns>
        public IpConnectionInfo GetConnectionInformation(string ipAddress)
        {
            using (TimedLock.Lock(_lockObject))
            {
                if (!_connectionInformation.ContainsKey(ipAddress))
                    _connectionInformation.Add(ipAddress, new IpConnectionInfo(ipAddress));

                return (_connectionInformation[ipAddress]);
            }
        }

        #endregion Public Methods

        #region Internal Methods

        internal static string GetMemoryStatus()
        {
            StringBuilder Result = new StringBuilder();

            using (TimedLock.Lock(_lockObject))
            {
                foreach (KeyValuePair<string, IpConnectionInfo> item in _connectionInformation)
                {
                    Result.Append('\n');
                    Result.Append(item.Value.ToString());
                }
            }

            return (Result.ToString().Trim());
        }

        #endregion Internal Methods

        #region Events Raise Methods

        /// <summary>
        /// Raise connection removed event
        /// </summary>
        /// <param name="connection">connection info being removed</param>
        private void RaiseConnectionAdd(IpConnectionInfo connection)
        {
            if (ConnectionAdd != null)
                ConnectionAdd(null, new DefenderConnectionEventArgs(connection));
        }

        /// <summary>
        /// Raise connection removed event
        /// </summary>
        /// <param name="connection">connection info being removed</param>
        internal void RaiseConnectionRemoved(IpConnectionInfo connection)
        {
            if (ConnectionRemoved != null)
                ConnectionRemoved(null, new DefenderConnectionEventArgs(connection));
        }

        /// <summary>
        /// Raises an event for banning an IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="hits"></param>
        /// <param name="requests"></param>
        /// <param name="span"></param>
        /// <returns>True if the IP Address should be black listed and added to banned list, otherwise false</returns>
        private bool RaiseOnBanIPAddress(string ipAddress, double hits, int requests, TimeSpan span)
        {
            LogAuditRequireBanArgs args = new LogAuditRequireBanArgs(ipAddress, hits, requests, span);

            if (OnBanIPAddress != null)
                OnBanIPAddress(null, args);

            return (args.AddToBlackList);
        }

        /// <summary>
        /// Raise an OnException event
        /// </summary>
        /// <param name="err">Error being raised</param>
        internal void RaiseException(Exception err)
        {
            if (OnException != null)
                OnException(null, err);
        }

        #endregion Event Raise Methods

        #region Events

        /// <summary>
        /// Event raised when an IP Address needs to be banned
        /// </summary>
        public event LogAuditBanDelegate OnBanIPAddress;

        /// <summary>
        /// Event raised when connection info object add
        /// </summary>
        internal event DefenderConnectionRemovedEventHandler ConnectionAdd;

        /// <summary>
        /// Event raised when connection info object removed
        /// </summary>
        public event DefenderConnectionRemovedEventHandler ConnectionRemoved;

        /// <summary>
        /// Event raised when there is an error
        /// </summary>
        public event DefenderException OnException;

        #endregion Events

        #region Private Methods

        /// <summary>
        /// Updates result flags on connection info object
        /// </summary>
        /// <param name="Result">Result of this validation</param>
        /// <param name="connection">connection info object</param>
        private void UpdateConnectionInfo(ref ValidateRequestResult Result, in IpConnectionInfo connection)
        {
            if (Result.HasFlag(ValidateRequestResult.SpiderBot) && !connection.Results.HasFlag(ValidateRequestResult.SpiderBot))
                connection.Results |= ValidateRequestResult.SpiderBot;

            if (Result.HasFlag(ValidateRequestResult.PossibleSpiderBot) && !connection.Results.HasFlag(ValidateRequestResult.PossibleSpiderBot))
                connection.Results |= ValidateRequestResult.PossibleSpiderBot;

            if (Result.HasFlag(ValidateRequestResult.SQLInjectionAttack) && !connection.Results.HasFlag(ValidateRequestResult.SQLInjectionAttack))
                connection.Results |= ValidateRequestResult.SQLInjectionAttack;

            if (Result.HasFlag(ValidateRequestResult.PossibleSQLInjectionAttack) && !connection.Results.HasFlag(ValidateRequestResult.PossibleSQLInjectionAttack))
                connection.Results |= ValidateRequestResult.PossibleSQLInjectionAttack;

            if (Result.HasFlag(ValidateRequestResult.HackAttempt) && !connection.Results.HasFlag(ValidateRequestResult.HackAttempt))
                connection.Results |= ValidateRequestResult.HackAttempt;

            if (Result.HasFlag(ValidateRequestResult.PossibleHackAttempt) && !connection.Results.HasFlag(ValidateRequestResult.PossibleHackAttempt))
                connection.Results |= ValidateRequestResult.PossibleHackAttempt;

            // if connection info previously holds info about spiderbot, add this to the result
            if (!Result.HasFlag(ValidateRequestResult.SpiderBot) && connection.Results.HasFlag(ValidateRequestResult.SpiderBot))
                Result |= ValidateRequestResult.SpiderBot;

            if (!Result.HasFlag(ValidateRequestResult.PossibleSpiderBot) && connection.Results.HasFlag(ValidateRequestResult.PossibleSpiderBot))
                Result |= ValidateRequestResult.PossibleSpiderBot;

#warning does it save
            //Set(connection.IPAddress, connection);
        }

        private void ValidateAndBanIPAddresses()
        {
            HashSet<IpConnectionInfo> banRequests = new HashSet<IpConnectionInfo>();

            using (TimedLock.Lock(_lockObject))
            {
                foreach (KeyValuePair<string, IpConnectionInfo> item in _connectionInformation)
                {
                    // if the connection has too many hits per minute, add the flag, otherwise remove it
                    double hitsPerMinute = item.Value.Requests / (item.Value.TotalTime().TotalMinutes + 1);
                    bool hasTooManyRequests = item.Value.Results.HasFlag(ValidateRequestResult.TooManyRequests);

                    if (hitsPerMinute > _maximumConnectionsPerSecond && !hasTooManyRequests)
                        item.Value.Results |= ValidateRequestResult.TooManyRequests;
                    else if (hitsPerMinute < _maximumConnectionsPerSecond && hasTooManyRequests)
                        item.Value.Results &= ~ValidateRequestResult.TooManyRequests;

                    // if the connection has hackattempt flag, log it for banning
                    if (item.Value.Results.HasFlag(ValidateRequestResult.HackAttempt))
                    {
                        // NEVER ban a local IP Address, but log what is happening
                        if (Shared.Utilities.LocalIPAddress(item.Value.IPAddress))
                            continue;

                        if (!item.Value.Results.HasFlag(ValidateRequestResult.BanRequested) &&
                            item.Value.Results.HasFlag(ValidateRequestResult.HackAttempt))
                        {
                            item.Value.Results = ValidateRequestResult.BanRequested;
                            banRequests.Add(item.Value);
                        }
                    }
                }
            }

            foreach (IpConnectionInfo connection in banRequests)
                RaiseOnBanIPAddress(connection.IPAddress, connection.HitsPerMinute(), (int)connection.Requests, connection.TotalTime());
        }

        /// <summary>
        /// Verify's an address to see if it's black/white listed
        /// </summary>
        /// <param name="IPAddress">IPAddress to verify</param>
        /// <returns>ValidateRequestResult enum with results</returns>
        private ValidateRequestResult VerifyAddress(string IPAddress)
        {
            ValidateRequestResult Result;

#warning finish
            //is the address white/black listed
            //IPAddress address = _addressList.Find(IPAddress);

            //if (address != null)
            //{
            //    if (address.BlackListed)
            //        Result = ValidateRequestResult.IPBlackListed;
            //    else
            //        Result = ValidateRequestResult.IPWhiteListed;

            //    if (address.SearchEngine)
            //        Result |= ValidateRequestResult.SearchEngine;

            //    return (Result);
            //}

            return (ValidateRequestResult.Undetermined);
        }

        private void ReportWebData(string ipAddress, string queryString, ValidateRequestResult validation)
        {
            //do something later
#warning finish
        }

        /// <summary>
        /// Determines the probability that user is bot/spider 
        /// </summary>
        /// <param name="ipAddress">IP Address of user</param>
        /// <param name="validation">validation Results</param>
        private void DetermineSpiderBot(in IpConnectionInfo connectionInfo, ref ValidateRequestResult validation)
        {
            // look at how many hits from a single location
            double hitsPerSecond = connectionInfo.HitsPerSecond();
            double uniquePages = connectionInfo.UniquePages();

            if (connectionInfo.Requests > 3 && uniquePages > MinimumSpiderUniqueRequests)
            {
                if (hitsPerSecond > BotHitsPerSecondProbability)
                    validation |= ValidateRequestResult.PossibleSpiderBot;

                if (hitsPerSecond > BotHitsPerSecond)
                    validation |= ValidateRequestResult.SpiderBot;
            }
        }

        /// <summary>
        /// Determines the probability of a hacking attempt
        /// </summary>
        /// <param name="request">data to be checked</param>
        /// <param name="count">Number of attempts found</param>
        /// <param name="validation">Result of the check</param>
        private void DetermineHackAttemt(string request, ref int count, ref ValidateRequestResult validation)
        {
            int Weight = 0;

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

                while (((n = url.IndexOf(word, n, StringComparison.InvariantCulture)) != -1))
                {
                    n += word.Trim().Length;
                    count++;
                    Weight += count * 16;

                    if (Weight > HackAttempt)
                        break;

                }

                if (Weight > HackAttempt)
                    break;
            }

            // if it's a hack are random words being used as parameters
            if (Weight < HackAttempt)
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
                        Weight += count * 16;

                        if (Weight > HackAttempt)
                            break;
                    }
                }
            }

            if (Weight >= HackProbability)
                validation |= ValidateRequestResult.PossibleHackAttempt;

            if (Weight >= HackAttempt)
                validation |= ValidateRequestResult.HackAttempt;
        }

        /// <summary>
        /// Determines if the url contains sql which could be a SQL Injection
        /// </summary>
        /// <param name="ipAddress">url being passed</param>
        /// <param name="count">number of attempts found</param>
        /// <param name="validation">Result of the check</param>
        private void DetermineSQLInjectionAttack(string request, ref int count, ref ValidateRequestResult validation)
        {
            int Weight = 0;

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

                while (((n = url.IndexOf(word, n, StringComparison.InvariantCulture)) != -1))
                {
                    n += word.Trim().Length;
                    count++;
                    Weight += count * 16;

                    if (Weight > HackAttempt)
                        break;

                }

                if (Weight > HackAttempt)
                    break;
            }

            if (Weight >= HackProbability)
                validation |= ValidateRequestResult.PossibleSQLInjectionAttack;

            if (Weight >= HackAttempt)
                validation |= ValidateRequestResult.SQLInjectionAttack;
        }

        /// <summary>
        /// Determines if a word is made up of random characters
        /// 
        /// Assumption is if has 2 or more upper/lower case chars and at least 1 number then it's random
        /// </summary>
        /// <param name="word">Word which is being tested</param>
        /// <returns>true if contains 2 or more upper/lower case chars and at least 1 number, otherwise false</returns>
        private bool IsRandomWord(string word)
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

            return (Result);
        }

        #endregion Private Methods
    }
}
