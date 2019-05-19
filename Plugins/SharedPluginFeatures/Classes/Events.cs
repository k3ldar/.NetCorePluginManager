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
    /// <param name="sender">object sender</param>
    /// <param name="e">arguments</param>
    public delegate void DefenderConnectionAddEventHandler(object sender, ConnectionArgs e);

    /// <summary>
    /// Connection info event arguments
    /// </summary>
    public class ConnectionRemoveArgs : ConnectionArgs
    {
        #region Constructor

        public ConnectionRemoveArgs(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
            : base (ipAddress)
        {
            Hits = hits;
            Requests = requests;
            Duration = duration;
        }

        #endregion Constructor

        #region Properties

        public double Hits { get; private set; }

        public ulong Requests { get; private set; }

        public TimeSpan Duration { get; private set; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate for connection remove
    /// </summary>
    /// <param name="sender">object sender</param>
    /// <param name="e">arguments</param>
    public delegate void DefenderConnectionRemoveEventHandler(object sender, ConnectionRemoveArgs e);


    public sealed class RequestBanArgs : ConnectionRemoveArgs
    {
        #region Constructor

        public RequestBanArgs(in string ipAddress, in double hits, in ulong requests, in TimeSpan duration)
            : base (ipAddress, hits, requests, duration)
        {

        }

        #endregion Constructor

        #region Properties

        public bool AddToBlackList { get; set; }

        #endregion Properties
    }

    /// <summary>
    /// Delegate for log audit failure events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DefenderRequestBan(object sender, RequestBanArgs e);

    public sealed class ConnectionReportArgs : ConnectionArgs
    {
        #region Constructors

        public ConnectionReportArgs(in string ipAddress, in string queryString, in ValidateRequestResult validation)
            : base (ipAddress)
        {
            QueryString = queryString;
            Result = validation;
        }

        #endregion Constructors

        #region Properties

        public string QueryString { get; private set; }

        public ValidateRequestResult Result { get; private set; }

        #endregion Properties
    }

    public delegate void DefenderReportConnection(object sender, ConnectionReportArgs e);
}
