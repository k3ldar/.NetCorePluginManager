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
 *  File: IpConnectionInfo.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  10/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using static SharedPluginFeatures.Enums;

namespace BadEgg.Plugin.WebDefender
{
    internal sealed class IpConnectionInfo
    {
        #region Private Members

        private uint _uniquePages;

        #endregion Private Members

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">Address of client</param>
        internal IpConnectionInfo(in string ipAddress)
            : this(ipAddress, DateTime.Now)
        {
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">Address of client</param>
        /// <param name="start">Date/time the connection was created.</param>
        internal IpConnectionInfo(in string ipAddress, in DateTime start)
        {
            Created = start;
            IPAddress = ipAddress;
            Text = String.Empty;
            UserAgents = String.Empty;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// IP Address for client
        /// </summary>
        public string IPAddress { get; private set; }

        /// <summary>
        /// DateTime this record was created
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// DateTime last entry was made
        /// </summary>
        public DateTime LastEntry { get; internal set; }

        /// <summary>
        /// Number of requests made
        /// </summary>
        public ulong Requests { get; internal set; }

        /// <summary>
        /// Results for the connection
        /// </summary>
        public ValidateRequestResult Results { get; internal set; }

        /// <summary>
        /// Unique text being scanned
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Host Information
        /// </summary>
        public string UserAgents { get; set; }

        #endregion Properties

        #region Internal Methods

        /// <summary>
        /// Increments the number of requests
        /// </summary>
        internal void AddRequest(in string RequestInformation, in string UserAgent)
        {
            Requests++;
            LastEntry = DateTime.Now;

            //only add unique strings to the list
            if (!String.IsNullOrEmpty(RequestInformation) && !Text.Contains(RequestInformation))
            {
                _uniquePages++;
                Text += String.Format("{0}\r", RequestInformation);
            }

            // add unique user agents
            if (!String.IsNullOrEmpty(UserAgent) && !UserAgents.Contains(UserAgent))
                UserAgents += String.Format("{0}\r", UserAgent);
        }

        #endregion Internal Methods

        #region Public Methods

        public double UniquePages()
        {
            if (_uniquePages > 1)
                return _uniquePages / (double)Requests * 100;
            else
                return _uniquePages;
        }

        /// <summary>
        /// Determines how many hits per second
        /// </summary>
        /// <returns></returns>
        public double HitsPerSecond()
        {
            double Result = 0;

            //how many hits per second are being generated?
            TimeSpan span = LastEntry - Created;

            if (span.TotalSeconds > 1)
                Result = Requests / span.TotalSeconds;
            else
                Result = Requests;

            return Result;
        }

        /// <summary>
        /// Determines how many hits per second
        /// </summary>
        /// <returns></returns>
        public double HitsPerMinute()
        {
            double Result = 0;

            //how many hits per second are being generated?
            TimeSpan span = LastEntry - Created;

            if (span.TotalSeconds > 1)
                Result = Requests / span.TotalMinutes;
            else
                Result = Requests;

            return Result;
        }

        /// <summary>
        /// Total time the connection has been active from when logged to last entry
        /// </summary>
        /// <returns>TimeSpan</returns>
        public TimeSpan TotalTime()
        {
            return LastEntry - Created;
        }

        private string GetResults()
        {
            string Result = String.Empty;

            foreach (Enum value in Enum.GetValues(typeof(ValidateRequestResult)))
                if (Results.HasFlag(value))
                    Result += $"{value.ToString()} ";

            return Result;
        }

        #endregion

        #region Overridden Methods

        public override string ToString()
        {
            return String.Format("{0}#{1}#{2}#{3}#{4}#{5}#{6}",
                IPAddress, Requests, TotalTime().ToString(), Created.ToString("g"), LastEntry.ToString("g"),
                HitsPerMinute(), GetResults());
        }

        #endregion Overridden Methods
    }
}
