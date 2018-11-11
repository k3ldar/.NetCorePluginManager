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
 *  File: Events.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  10/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

namespace BadEgg.Plugin.WebDefender
{

    /// <summary>
    /// Connection info event arguments
    /// </summary>
    public sealed class DefenderConnectionEventArgs : EventArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">Connection info</param>
        internal DefenderConnectionEventArgs(IpConnectionInfo connection)
        {
            Connection = connection;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Connection info being removed
        /// </summary>
        public IpConnectionInfo Connection { get; private set; }

        #endregion Properties
    }

    public sealed class DefenderExceptionEventArgs : EventArgs
    {
        #region Constructors

        internal DefenderExceptionEventArgs(Exception exception)
        {
            Error = exception;
        }

        #endregion Constructors

        #region Properties

        public Exception Error { private set; get; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate for connection info object being removed
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">arguments</param>
    public delegate void DefenderConnectionRemovedEventHandler(object sender, DefenderConnectionEventArgs e);

    /// <summary>
    /// Delegate for exceptions
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">exception being raised</param>
    public delegate void DefenderException(object sender, Exception e);

    public sealed class IPAddressUnBanArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddresss"></param>
        internal IPAddressUnBanArgs(string ipAddresss)
        {
            IPAddress = ipAddresss;
            Allow = true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// IP Address to be unbanned
        /// </summary>
        public string IPAddress { get; private set; }

        /// <summary>
        /// Determines wether an address can be un banned
        /// </summary>
        public bool Allow { get; set; }

        #endregion Properties
    }

    public delegate void IPAddressUnBanDelegate(object sender, IPAddressUnBanArgs e);

    /// <summary>
    /// Class used as arguments for banning request
    /// </summary>
    public sealed class LogAuditRequireBanArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">IP Address being validated</param>
        /// <param name="hits">Average Hits per minute for IP Address</param>
        /// <param name="requests">Total number of requests for IP Address</param>
        /// <param name="span">Total time IP Address attempting to connect</param>
        internal LogAuditRequireBanArgs(string ipAddress, double hits, int requests, TimeSpan span)
        {
            IPAddress = ipAddress;
            Hits = hits;
            Span = span;
            Requests = requests;
            AddToBlackList = true;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// IP Address being validated
        /// </summary>
        public string IPAddress { get; private set; }

        /// <summary>
        /// Average Hits per minute for IP Address
        /// </summary>
        public double Hits { get; private set; }

        /// <summary>
        /// Length of time for all requests by IP Address
        /// </summary>
        public TimeSpan Span { get; private set; }

        /// <summary>
        /// Number of requests by the IP Address
        /// </summary>
        public int Requests { get; private set; }

        /// <summary>
        /// Determines wether the IP Address should be added to the database of failures
        /// </summary>
        public bool AddToBlackList { get; set; }

        #endregion Properties
    }

    public delegate void LogAuditBanDelegate(object sender, LogAuditRequireBanArgs e);

    public sealed class LogAuditFailureArgs
    {
        #region Constructor

        internal LogAuditFailureArgs(string ipAddress, DateTime timeGenerated, string source)
        {
            IPAddress = ipAddress;
            TimeGenerated = timeGenerated;
            Source = source;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// IP Address for connection
        /// </summary>
        public string IPAddress { get; private set; }

        /// <summary>
        /// Time first log generated
        /// </summary>
        public DateTime TimeGenerated { get; private set; }

        /// <summary>
        /// Source of log audit failure
        /// </summary>
        public string Source { get; private set; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate for log audit failure events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void LogAuditFailureDelegate(object sender, LogAuditFailureArgs e);
}
