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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
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
    public class UserSessionCore : UserSession
    {
        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        public UserSessionCore()
            : base()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="created"></param>
        /// <param name="sessionID"></param>
        /// <param name="userAgent"></param>
        /// <param name="initialReferer"></param>
        /// <param name="ipAddress"></param>
        /// <param name="hostName"></param>
        /// <param name="isMobile"></param>
        /// <param name="isBrowserMobile"></param>
        /// <param name="mobileRedirect"></param>
        /// <param name="referralType"></param>
        /// <param name="bounced"></param>
        /// <param name="isBot"></param>
        /// <param name="mobileManufacturer"></param>
        /// <param name="mobileModel"></param>
        /// <param name="userID"></param>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="saleCurrency"></param>
        /// <param name="saleAmount"></param>
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
        /// <param name="Session">Current User Session</param>
        /// <param name="Request">Current Web Request</param>
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
        /// <param name="Session">Current User Session</param>
        /// <param name="Request">Current Web Request</param>
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
