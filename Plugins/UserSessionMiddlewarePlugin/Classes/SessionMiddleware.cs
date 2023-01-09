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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace UserSessionMiddleware.Plugin
{
    /// <summary>
    /// UserSession Middleware.
    /// 
    /// This class is inserted into the request pipeline and manages all User Session data.
    /// </summary>
    public sealed class SessionMiddleware : BaseMiddleware
    {
        #region Private Members

        private static int _cookieID;

        private readonly RequestDelegate _next;
        private readonly string _cookieName = Constants.DefaultSessionCookie;
        private readonly string _cookieEncryptionKey = "Dfklaosre;lnfsdl;jlfaeu;dkkfcaskxcd3jf";
        private readonly string _staticFileExtension = Constants.StaticFileExtensions;
        private readonly List<RouteData> _routeData;
        private readonly string _defaultCulture;
        internal readonly static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public SessionMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService, ISettingsProvider settingsProvider)
        {
            if (routeProvider == null)
                throw new ArgumentNullException(nameof(routeProvider));

            if (routeDataService == null)
                throw new ArgumentNullException(nameof(routeDataService));

            if (pluginTypesService == null)
                throw new ArgumentNullException(nameof(pluginTypesService));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next;

            _routeData = new List<RouteData>();

            UserSessionSettings Settings = settingsProvider.GetSettings<UserSessionSettings>(Constants.UserSessionConfiguration);

            Settings.SessionTimeout = Shared.Utilities.CheckMinMax(Settings.SessionTimeout, 15, 200);
            UserSessionManager.InitialiseSessionManager(new TimeSpan(0, (int)Settings.SessionTimeout, 0));

            if (!String.IsNullOrWhiteSpace(Settings.CookieName))
                _cookieName = Settings.CookieName;

            if (!String.IsNullOrWhiteSpace(Settings.EncryptionKey))
                _cookieEncryptionKey = Settings.EncryptionKey;

            if (!String.IsNullOrWhiteSpace(Settings.StaticFileExtensions))
                _staticFileExtension = Settings.StaticFileExtensions;

            _defaultCulture = Settings.DefaultCulture;

            LoadLoggedInOutData(routeProvider, routeDataService, pluginTypesService);
            LoadLoggedInData(routeProvider, routeDataService, pluginTypesService);
            LoadLoggedOutData(routeProvider, routeDataService, pluginTypesService);
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

			string fileExtension = RouteFileExtension(context);

            // if it's a static file, don't add user session data to the context
            if (!String.IsNullOrEmpty(fileExtension) &&
                _staticFileExtension.Contains($"{fileExtension};", StringComparison.InvariantCultureIgnoreCase))
            {
                await _next(context);
                return;
            }

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                string cookieSessionID;
                CookieOptions options = new CookieOptions()
                {
                    HttpOnly = false,
                    SameSite = SameSiteMode.Lax,
                };

                if (context.Request.Cookies.ContainsKey(_cookieName))
                {
                    cookieSessionID = Shared.Utilities.Decrypt(context.Request.Cookies[_cookieName], _cookieEncryptionKey);
                    context.Response.Cookies.Append(_cookieName,
                        Shared.Utilities.Encrypt(cookieSessionID, _cookieEncryptionKey), options);
                }
                else
                {
                    cookieSessionID = GetNextID();
                    context.Response.Cookies.Append(_cookieName,
                        Shared.Utilities.Encrypt(cookieSessionID, _cookieEncryptionKey), options);
                }

                CacheItem currentSession = UserSessionManager.UserSessions.Get(cookieSessionID);
                UserSession userSession;

                if (currentSession != null)
                {
                    userSession = (UserSession)currentSession.Value;
                }
                else
                {
                    userSession = GetUserSession(context, cookieSessionID);
                    GetSessionCulture(context, userSession);
                }

                string referrer = context.Request.Headers[Constants.PageReferrer];
                userSession.PageView(GetAbsoluteUri(context).ToString(), referrer ?? String.Empty, false);

                context.Items.Add(Constants.UserSession, userSession);
                context.Items.Add(Constants.UserCulture, userSession.Culture);

                string route = RouteLowered(context);
                bool loggedIn = !String.IsNullOrEmpty(userSession.UserName);

                // is the route a loggedin/loggedout route
                RouteData partialMatch = null;

                foreach (RouteData routeData in _routeData)
                {
                    if (routeData.Matches(route))
                    {
                        if (routeData.Ignore)
                            partialMatch = null;
                        else
                            partialMatch = routeData;

                        break;
                    }

                    if (partialMatch == null && route.StartsWith(routeData.Route, StringComparison.InvariantCultureIgnoreCase))
                        partialMatch = routeData;
                }

                // if we have a match check if we need to redirect
                if (partialMatch != null)
                {
                    if (!partialMatch.LoggedIn && loggedIn)
                    {
                        context.Response.Redirect(partialMatch.RedirectPath, false);
                        return;
                    }
                    else if (partialMatch.LoggedIn && !loggedIn)
                    {
                        context.Response.Redirect($"{partialMatch.RedirectPath}?returnUrl={context.Request.Path.ToString()}", false);
                        return;
                    }

                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private static string GetNextID()
        {
            return $"SN{DateTime.UtcNow.ToFileTimeUtc()}{_cookieID++}";
        }

        private static Uri GetAbsoluteUri(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            UriBuilder uriBuilder = new UriBuilder(context.Request.Scheme, context.Request.Host.Host)
            {
                Path = context.Request.Path.ToString(),
                Query = context.Request.QueryString.ToString()
            };

            return uriBuilder.Uri;
        }

        private void GetSessionCulture(in HttpContext context, in UserSession userSession)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (userSession == null)
                throw new ArgumentNullException(nameof(userSession));

            userSession.Culture = CookieValue(context, $"{_cookieName}_{Constants.UserCulture}", _cookieEncryptionKey, _defaultCulture);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
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

                GetSessionCulture(context, Result);

                return Result;
            }
            catch (Exception err)
            {
                if (PluginInitialisation.GetLogger != null)
                    PluginInitialisation.GetLogger.AddToLog(PluginManager.LogLevel.Error, nameof(SessionMiddleware), err,
                        MethodBase.GetCurrentMethod().Name);
            }

            return null;
        }

        private void LoadLoggedInData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            List<Type> loggedInAttributes = pluginTypesService.GetPluginTypesWithAttribute<LoggedInAttribute>();

            // Cycle through all classes and methods which have the logged in attribute
            foreach (Type type in loggedInAttributes)
            {
                // is it a class attribute
                LoggedInAttribute attribute = (LoggedInAttribute)type.GetCustomAttributes(true)
                    .FirstOrDefault(a => a is LoggedInAttribute);

                if (attribute != null)
                {
                    string route = routeDataService.GetRouteFromClass(type, routeProvider);

                    if (String.IsNullOrEmpty(route))
                        continue;

                    _routeData.Add(new RouteData(route.ToLower(), true, attribute.LoginPage));
                }

                // look for specific method disallows

                foreach (MethodInfo method in type.GetMethods())
                {
                    attribute = (LoggedInAttribute)method.GetCustomAttributes(true).FirstOrDefault(a => a is LoggedInAttribute);

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        _routeData.Add(new RouteData(route.ToLower(), true, attribute.LoginPage));
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
                    .FirstOrDefault(a => a is LoggedOutAttribute);

                if (attribute != null)
                {
                    string route = routeDataService.GetRouteFromClass(type, routeProvider);

                    if (String.IsNullOrEmpty(route))
                        continue;

                    _routeData.Add(new RouteData(route.ToLower(), false, attribute.RedirectPage));
                }

                // look for specific method disallows

                foreach (MethodInfo method in type.GetMethods())
                {
                    attribute = (LoggedOutAttribute)method.GetCustomAttributes(true)
                        .FirstOrDefault(a => a is LoggedOutAttribute);

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        _routeData.Add(new RouteData(route.ToLower(), false, attribute.RedirectPage));
                    }
                }
            }
        }

        private void LoadLoggedInOutData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            List<Type> loggedInOutAttributes = pluginTypesService.GetPluginTypesWithAttribute<LoggedInOutAttribute>();

            // Cycle through all classes and methods which have the spider attribute
            foreach (Type type in loggedInOutAttributes)
            {
                foreach (MethodInfo method in type.GetMethods())
                {
                    LoggedInOutAttribute attribute = (LoggedInOutAttribute)method.GetCustomAttributes(true)
						.FirstOrDefault(a => a.GetType() == typeof(LoggedInOutAttribute));

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        _routeData.Add(new RouteData(route.ToLower()));
                    }
                }
            }
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591