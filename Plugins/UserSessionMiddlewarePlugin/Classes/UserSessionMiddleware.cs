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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using SharedPluginFeatures;
using static SharedPluginFeatures.Enums;

using Shared.Classes;

namespace UserSessionMiddleware.Plugin
{
    public sealed class UserSessionMiddleware : BaseMiddleware
    {
        #region Private Members

        private static int _cookieID;

        private readonly RequestDelegate _next;
        private readonly string _cookieName = "user_session";
        private readonly string _cookieEncryptionKey = "Dfklaosre;lnfsdl;jlfaeu;dkkfcaskxcd3jf";
        private readonly string _staticFileExtension = ".less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;";
        private readonly Dictionary<string, string> _loggedInRoutes;
        private readonly Dictionary<string, string> _loggedOutRoutes;
        internal static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public UserSessionMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            if (routeProvider == null)
                throw new ArgumentNullException(nameof(routeProvider));

            if (routeDataService == null)
                throw new ArgumentNullException(nameof(routeDataService));

            if (pluginTypesService == null)
                throw new ArgumentNullException(nameof(pluginTypesService));

            _next = next;

            _loggedInRoutes = new Dictionary<string, string>();
            _loggedOutRoutes = new Dictionary<string, string>();

            UserSessionSettings Settings = GetSettings<UserSessionSettings>("UserSessionConfiguration");

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

            LoadLoggedInData(routeProvider, routeDataService, pluginTypesService);
            LoadLoggedOutData(routeProvider, routeDataService, pluginTypesService);
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            string fileExtension = base.RouteFileExtension(context);

            // if it's a static file, don't add user session data to the context
            if (!String.IsNullOrEmpty(fileExtension) && _staticFileExtension.Contains($"{fileExtension};"))
            {
                await _next(context);
                return;
            }

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
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

                string route = RouteLowered(context);

                // is the route a loggedin route
            }

            await _next(context);
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
                    Initialisation.GetLogger.AddToLog(LogLevel.UserSessionManagerError, err, 
                        System.Reflection.MethodBase.GetCurrentMethod().Name);
            }

            return (null);
        }

        private void LoadLoggedInData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            List<Type> loggedInAttributes = pluginTypesService.GetPluginTypesWithAttribute<LoggedInAttribute>();

            // Cycle through all classes and methods which have the spider attribute
            foreach (Type type in loggedInAttributes)
            {
                // is it a class attribute
                LoggedInAttribute attribute = (LoggedInAttribute)type.GetCustomAttributes(true)
                    .Where(a => a.GetType() == typeof(LoggedInAttribute)).FirstOrDefault();

                if (attribute != null)
                {
                    string route = routeDataService.GetRouteFromClass(type, routeProvider);

                    if (String.IsNullOrEmpty(route))
                        continue;

                    _loggedInRoutes.Add(route.ToLower(), attribute.LoginPage);
                }

                // look for specific method disallows

                foreach (MethodInfo method in type.GetMethods())
                {
                    attribute = (LoggedInAttribute)method.GetCustomAttributes(true)
                        .Where(a => a.GetType() == typeof(LoggedInAttribute)).FirstOrDefault();

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        _loggedInRoutes.Add(route.ToLower(), attribute.LoginPage);
                    }
                }
            }
        }

        private void LoadLoggedOutData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            List<Type> loggedOutAttributes = pluginTypesService.GetPluginTypesWithAttribute<LoggedOutAttribute>();

            // Cycle through all classes and methods which have the spider attribute
            foreach (Type type in loggedOutAttributes)
            {
                // is it a class attribute
                LoggedOutAttribute attribute = (LoggedOutAttribute)type.GetCustomAttributes(true)
                    .Where(a => a.GetType() == typeof(LoggedOutAttribute)).FirstOrDefault();

                if (attribute != null)
                {
                    string route = routeDataService.GetRouteFromClass(type, routeProvider);

                    if (String.IsNullOrEmpty(route))
                        continue;

                    _loggedOutRoutes.Add(route.ToLower(), attribute.RedirectPage);
                }

                // look for specific method disallows

                foreach (MethodInfo method in type.GetMethods())
                {
                    attribute = (LoggedOutAttribute)method.GetCustomAttributes(true)
                        .Where(a => a.GetType() == typeof(LoggedOutAttribute)).FirstOrDefault();

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        _loggedOutRoutes.Add(route.ToLower(), attribute.RedirectPage);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
