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
 *  File: UserSessionMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using static System.IO.Path;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using Shared.Classes;

namespace UserSessionMiddleware.Plugin
{
    public sealed class UserSessionMiddleware
    {
        #region Private Members

        private static int _cookieID;

        private readonly RequestDelegate _next;
        private readonly string _cookieName = "user_session";
        private readonly string _cookieEncryptionKey = "Dfklaosre;lnfsdl;jlfaeu;dkkfcaskxcd3jf";
        private readonly string _staticFileExtension = ".less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;";

        #endregion Private Members

        #region Constructors

        public UserSessionMiddleware(RequestDelegate next)
        {
            _next = next;

            UserSessionSettings Settings = GetUserSessionSettings();

            Settings.SessionTimeout = Shared.Utilities.CheckMinMax(Settings.SessionTimeout, 15, 200);

            ThreadManager.Initialise();
            UserSessionManager.InitialiseSessionManager(new TimeSpan(0, (int)Settings.SessionTimeout, 0));

            SessionHelper.InitSessionHelper();

            if (!String.IsNullOrWhiteSpace(Settings.CookieName))
                _cookieName = Settings.CookieName;

            if (!String.IsNullOrWhiteSpace(Settings.EncryptionKey))
                _cookieEncryptionKey = Settings.EncryptionKey;

            if (!String.IsNullOrWhiteSpace(Settings.StaticFileExtensions))
                _staticFileExtension = Settings.StaticFileExtensions;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                string fileExtension = GetExtension(context.Request.Path.ToString().ToLower());

                // if it's a static file, don't add user session data to the context
                if (!String.IsNullOrEmpty(fileExtension) && _staticFileExtension.Contains($"{fileExtension};"))
                    return;

                string cookieSessionID;
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = false
                };

                if (context.Request.Cookies.ContainsKey(_cookieName))
                {
                    cookieSessionID = Shared.Utilities.Decrypt(context.Request.Cookies[_cookieName], _cookieEncryptionKey);
                    context.Response.Cookies.Append(_cookieName,
                        Shared.Utilities.Encrypt(cookieSessionID, _cookieEncryptionKey), options);
                }
                else
                {
                    cookieSessionID = GetNextID(context);
                    context.Response.Cookies.Append(_cookieName,
                        Shared.Utilities.Encrypt(cookieSessionID, _cookieEncryptionKey), options);
                }

                CacheItem currentSession = UserSessionManager.UserSessions.Get(cookieSessionID);
                UserSession userSession = null;

                if (currentSession != null)
                {
                    userSession = (UserSession)currentSession.Value;
                }
                else
                {
                    userSession = GetUserSession(context, cookieSessionID);
                }

                string referrer = context.Request.Headers["Referer"];
                userSession.PageView(GetAbsoluteUri(context).ToString(), referrer ?? String.Empty, false);

                context.Items.Add("UserSession", userSession);
            }
            catch (Exception error)
            {
                if (Initialisation.GetLogger != null)
                    Initialisation.GetLogger.AddToLog(error, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                await _next(context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private string GetNextID(HttpContext context)
        {
            return ($"SN{DateTime.Now.ToFileTimeUtc()}{_cookieID++}");
        }

        private Uri GetAbsoluteUri(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            UriBuilder uriBuilder = new UriBuilder(context.Request.Scheme, context.Request.Host.Host)
            {
                Path = context.Request.Path.ToString(),
                Query = context.Request.QueryString.ToString()
            };

            return (uriBuilder.Uri);
        }

        private UserSession GetUserSession(in HttpContext context, in string sessionId)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (String.IsNullOrEmpty(sessionId))
                throw new ArgumentNullException(nameof(sessionId));

            try
            {
                UserSession Result = new UserSessionCore(context, sessionId);

                UserSessionManager.Add(Result);

                UserSessionManager.Instance.InitialiseSession(Result);

                return (Result);
            }
            catch (Exception err)
            {
                if (Initialisation.GetLogger != null)
                    Initialisation.GetLogger.AddToLog(err, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            return (null);
        }

        private UserSessionSettings GetUserSessionSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            UserSessionSettings Result = new UserSessionSettings();
            config.GetSection("UserSessionConfiguration").Bind(Result);

            return (Result);
        }

        #endregion Private Methods
    }
}
