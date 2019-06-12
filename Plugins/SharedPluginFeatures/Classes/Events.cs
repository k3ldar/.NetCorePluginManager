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
 *  Product:  SharedPluginFeatues
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

using static SharedPluginFeatures.Enums;

namespace SharedPluginFeatures
{
    /// <summary>
    /// Connection arguments for DefenderConnectionAddEventHandler
    /// </summary>
    public class ConnectionArgs : EventArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">The Ip address being used by the connection.</param>
        public ConnectionArgs(in string ipAddress)
        {
            IPAddress = ipAddress;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// IP Address for connection
        /// </summary>
        public string IPAddress { get; private set; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate for connection add
    /// </summary>
    /// <param name="sender">The class that raised the event.</param>
    /// <param name="e">Event parameters.</param>
    public delegate void DefenderConnectionAddEventHandler(object sender, ConnectionArgs e);

    /// <summary>
    /// Connection info event arguments
    /// </summary>
    public class ConnectionRemoveArgs : ConnectionArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">Ip Address being reported on.</param>
        /// <param name="hits">Average hits per minute.</param>
        /// <param name="requests">Total number of reuests.</param>
        /// <param name="duration">Total duration the Ip address remained active.</param>
        public ConnectionRemoveArgs(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
            : base (ipAddress)
        {
            Hits = hits;
            Requests = requests;
            Duration = duration;
        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Average hits per minute.
        /// </summary>
        /// <value>double</value>
        public double Hits { get; private set; }

        /// <summary>
        /// Total number of reuests.
        /// </summary>
        /// <value>ulong</value>
        public ulong Requests { get; private set; }

        /// <summary>
        /// Total duration the Ip address remained active.
        /// </summary>
        /// <value>TimeSpan</value>
        public TimeSpan Duration { get; private set; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate for connection remove
    /// </summary>
    /// <param name="sender">The class that raised the event.</param>
    /// <param name="e">Event parameters.</param>
    public delegate void DefenderConnectionRemoveEventHandler(object sender, ConnectionRemoveArgs e);

    /// <summary>
    /// Arguments used in DefenderRequestBan event in order to notify that an Ip Address should be banned.
    /// </summary>
    public sealed class RequestBanArgs : ConnectionRemoveArgs
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">Ip address being reported on.</param>
        /// <param name="hits">Average requests per minute.</param>
        /// <param name="requests">Total number of requests</param>
        /// <param name="duration">Total duration the Ip address remained active.</param>
        public RequestBanArgs(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
            : base (ipAddress, hits, requests, duration)
        {

        }

        #endregion Constructor

        #region Properties

        /// <summary>
        /// Determines whether the ip address should be black listed or note.
        /// </summary>
        /// <value>bool</value>
        public bool AddToBlackList { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate for log audit failure events
    /// </summary>
    /// <param name="sender">The class that raised the event.</param>
    /// <param name="e">Event parameters.</param>
    public delegate void DefenderRequestBan(object sender, RequestBanArgs e);

    /// <summary>
    /// Arguments used to report a connection.
    /// </summary>
    public sealed class ConnectionReportArgs : ConnectionArgs
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ipAddress">Ip address.</param>
        /// <param name="queryString">Query or Form values used when validating the request.</param>
        /// <param name="validation">Result of validation.</param>
        public ConnectionReportArgs(in string ipAddress, in string queryString, in ValidateRequestResult validation)
            : base (ipAddress)
        {
            QueryString = queryString;
            Result = validation;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Query and form data associated with the event.
        /// </summary>
        public string QueryString { get; private set; }

        /// <summary>
        /// Result determined when validating a request.
        /// </summary>
        public ValidateRequestResult Result { get; private set; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate used for events when the Ip address is being reported upon.
    /// </summary>
    /// <param name="sender">The class that raised the event.</param>
    /// <param name="e">Event parameters.</param>
    public delegate void DefenderReportConnection(object sender, ConnectionReportArgs e);
}
