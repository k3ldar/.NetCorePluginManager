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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  UserSessionMiddleware.Plugin
 *  
 *  File: UserSessionCore.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

namespace UserSessionMiddleware.Plugin
{
    /// <summary>
    /// Descendant of UserSession that is used for Net Core Applications.
    /// </summary>
    public class UserSessionCore : UserSession
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public UserSessionCore()
            : base()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="created">Date time class created</param>
        /// <param name="sessionID">User Session Id</param>
        /// <param name="userAgent">Browser user agent</param>
        /// <param name="initialReferer">Initial referer</param>
        /// <param name="ipAddress">Ip Address of user</param>
        /// <param name="hostName">Host name</param>
        /// <param name="isMobile">Determines whether the user should be shown a mobile or standard site.</param>
        /// <param name="isBrowserMobile">Determines whether the user is on a mobile device.</param>
        /// <param name="mobileRedirect">Redirect if on a mobal device.</param>
        /// <param name="referralType">Referral Type</param>
        /// <param name="bounced">Indicates the user bounced, i.e. only visited one page.</param>
        /// <param name="isBot">Determines if the session is a bot</param>
        /// <param name="mobileManufacturer">Not Used</param>
        /// <param name="mobileModel">Not Used</param>
        /// <param name="userID">Id of user if known.</param>
        /// <param name="screenWidth">Not Used</param>
        /// <param name="screenHeight">Not Used</param>
        /// <param name="saleCurrency">The currency used for the current sale.</param>
        /// <param name="saleAmount">Amount of sale for current user session.</param>
        public UserSessionCore(long id, DateTime created, string sessionID, string userAgent, string initialReferer,
            string ipAddress, string hostName, bool isMobile, bool isBrowserMobile, bool mobileRedirect,
            ReferalType referralType, bool bounced, bool isBot, string mobileManufacturer, string mobileModel,
            long userID, int screenWidth, int screenHeight, string saleCurrency, decimal saleAmount)
            : base(id, created, sessionID, userAgent, initialReferer, ipAddress, hostName, isMobile,
                  isBrowserMobile, mobileRedirect, referralType, bounced, isBot, mobileManufacturer, mobileModel,
                  userID, screenWidth, screenHeight, saleCurrency, saleAmount)
        {
        }

        /// <summary>
        /// Constructor
        /// 
        /// Allows passing of user defined object
        /// </summary>
        /// <param name="context">HTTP Context </param>
        /// <param name="sessionId">User session Id</param>
        /// <param name="tag">User defined object</param>
        public UserSessionCore(in HttpContext context, in string sessionId, in object tag)
            : this(context, sessionId)
        {
            Tag = tag;
            InternalSessionID = -1;
        }

        /// <summary>
        /// Constructor
        /// 
        /// Allows passing of user name and email
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="sessionId">Current user session id</param>
        /// <param name="userName">Current user's name</param>
        /// <param name="userEmail">Current user's email address</param>
        /// <param name="userID">Current user's unique id</param>
        public UserSessionCore(in HttpContext context, in string sessionId, in string userName, in string userEmail,
            Int64 userID)
            : this(context, sessionId)
        {
            UserName = userName;
            UserEmail = userEmail;
            UserID = userID;
        }

        /// <summary>
        /// Constructor
        /// 
        /// Standard constructor
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="sessionId">Current user session id</param>
        public UserSessionCore(HttpContext context, string sessionId)
            : base()
        {
            Created = DateTime.Now;
            CurrentSale = 0.00m;
            CurrentSaleCurrency = String.Empty;
            Tag = null;

            SessionID = sessionId;
            IPAddress = context.Connection.RemoteIpAddress.ToString();

            if (IPAddress == "::1")
                IPAddress = "127.0.0.1";

            HostName = context.Request.Host.Host;
            UserAgent = context.Request.Headers["User-Agent"].ToString();
            IsMobileDevice = CheckIfMobileDevice(UserAgent);
            IsBrowserMobile = false;// Request.Browser.IsMobileDevice;

            MobileRedirect = IsMobileDevice | IsBrowserMobile;

            MobileManufacturer = "";// Request.Browser.MobileDeviceManufacturer;
            MobileModel = ""; // Request.Browser.MobileDeviceModel;
            ScreenHeight = 10;// Request.Browser.ScreenPixelsHeight;
            ScreenWidth = 10;// Request.Browser.ScreenPixelsWidth;

            try
            {
                string referrer = context.Request.Headers["Referer"].ToString();
                if (String.IsNullOrEmpty(referrer))
                    Referal = ReferalType.Unknown;
                else
                    InitialReferrer = referrer ?? String.Empty;
            }
            catch (Exception err)
            {
                if (!err.Message.Contains("The hostname could not be parsed"))
                    throw;
            }

            CountryCode = String.Empty;

            UserName = String.Empty;
            UserEmail = String.Empty;
            UserID = -1;

            UserSessionManager.UpdateSession(this);

            SaveStatus = SaveStatus.Pending;
            PageSaveStatus = SaveStatus.Saved;
            InternalSessionID = Int64.MinValue;
        }

        #endregion Constructor
    }
}
