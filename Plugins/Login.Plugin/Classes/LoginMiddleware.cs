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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Login Plugin
 *  
 *  File: LoginMiddleware.cs
 *
 *  Purpose:  Auto Login Middleware
 *
 *  Date        Name                Reason
 *  17/02/2019  Simon Carter        Initially Created
 *  23/04/2020  Simon Carter        Add Basic authentication
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using Middleware;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace LoginPlugin
{
    /// <summary>
    /// Login middleware ensures that users don't visit routes that are marked as LoggedIn or LoggedOut depending on their login status.
    /// </summary>
    public class LoginMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;
        private readonly ILoginProvider _loginProvider;
        private readonly LoginControllerSettings _loginControllerSettings;
        private readonly IClaimsProvider _claimsProvider;
        internal static Timings _loginTimings = new Timings();
        internal static Timings _autoLoginCookieTimings = new Timings();
        internal static Timings _autoLoginBasicAuthLogin = new Timings();

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="next">Next RequestDelegate to be called after <see cref="Invoke(HttpContext, IAuthenticationService)"/> has been called.</param>
        /// <param name="loginProvider">Login provider instance.</param>
        /// <param name="settingsProvider">Settings provider instance.</param>
        /// <param name="claimsProvider">Claims provider</param>
        /// <exception cref="ArgumentNullException">Raised is next is null.</exception>
        /// <exception cref="ArgumentNullException">Raised if loginProvider is null.</exception>
        /// <exception cref="ArgumentNullException">Raised if settingsProvider is null.</exception>
        /// <exception cref="ArgumentNullException">Raised if authenticationService is null.</exception>
        /// <exception cref="ArgumentNullException">Raised if claimsProvider is null.</exception>
        public LoginMiddleware(RequestDelegate next, ILoginProvider loginProvider,
            ISettingsProvider settingsProvider,
            IClaimsProvider claimsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _loginProvider = loginProvider ?? throw new ArgumentNullException(nameof(loginProvider));
            _claimsProvider = claimsProvider ?? throw new ArgumentNullException(nameof(claimsProvider));
            _loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Method called during middleware processing of requests
        /// </summary>
        /// <param name="context">HttpContext for the request.</param>
        /// <param name="authenticationService"></param>
        /// <returns>Task</returns>
        /// <exception cref="ArgumentNullException">Raised if context is null</exception>
        public async Task Invoke(HttpContext context, IAuthenticationService authenticationService)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (authenticationService == null)
                throw new ArgumentNullException(nameof(authenticationService));

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_loginTimings))
            {
                UserSession userSession = GetUserSession(context);

                if (userSession != null &&
                    context.Request.Headers.ContainsKey(SharedPluginFeatures.Constants.HeaderAuthorizationName))
                {
                    if (!await LoginUsingBasicAuth(userSession, context, authenticationService))
                    {
                        return;
                    }
                }
                else if (userSession != null && String.IsNullOrEmpty(userSession.UserName) &&
                    CookieExists(context, _loginControllerSettings.RememberMeCookieName))
                {
                    await LoginUsingCookieValue(userSession, context, authenticationService);
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private async Task<bool> LoginUsingBasicAuth(UserSession userSession, HttpContext context,
            IAuthenticationService authenticationService)
        {
            using (StopWatchTimer stopWatchTimer = StopWatchTimer.Initialise(_autoLoginBasicAuthLogin))
            {
                string authData = context.Request.Headers[SharedPluginFeatures.Constants.HeaderAuthorizationName];

                if (!authData.StartsWith("Basic ", StringComparison.InvariantCultureIgnoreCase))
                {
                    context.Response.StatusCode = SharedPluginFeatures.Constants.HtmlResponseBadRequest;
                    return false;
                }

                try
                {
                    authData = System.Text.Encoding.GetEncoding("ISO-8859-1").GetString(Convert.FromBase64String(authData.Substring(6)));
                }
                catch (FormatException)
                {
                    context.Response.StatusCode = SharedPluginFeatures.Constants.HtmlResponseBadRequest;
                    return false;
                }

                string[] authParts = authData.Split(':', StringSplitOptions.RemoveEmptyEntries);

                if (authParts.Length != 2)
                {
                    context.Response.StatusCode = SharedPluginFeatures.Constants.HtmlResponseBadRequest;
                    return false;
                }

                UserLoginDetails loginDetails = new UserLoginDetails();

                LoginResult loginResult = _loginProvider.Login(ValidateUserInput(authParts[0], ValidationType.Name), ValidateUserInput(authParts[1], ValidationType.Password),
                    GetIpAddress(context), 1, ref loginDetails);

                if (loginResult == LoginResult.Success)
                {
                    userSession.Login(loginDetails.UserId, loginDetails.Username, loginDetails.Email);
                    await authenticationService.SignInAsync(context,
                        _loginControllerSettings.AuthenticationScheme,
                        new ClaimsPrincipal(_claimsProvider.GetUserClaims(loginDetails.UserId)),
                        _claimsProvider.GetAuthenticationProperties());

                    return true;
                }
                else
                {
                    context.Response.StatusCode = 401;
                    return false;
                }
            }
        }

        private async Task LoginUsingCookieValue(UserSession userSession, HttpContext context,
            IAuthenticationService authenticationService)
        {
            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_autoLoginCookieTimings))
            {
                string cookieValue = CookieValue(context, _loginControllerSettings.RememberMeCookieName,
                    _loginControllerSettings.EncryptionKey, String.Empty);

                if (Int64.TryParse(cookieValue, out long userId))
                {
                    UserLoginDetails loginDetails = new UserLoginDetails(userId, true);

                    LoginResult loginResult = _loginProvider.Login(String.Empty, String.Empty,
                        GetIpAddress(context), 1, ref loginDetails);

                    if (loginResult == LoginResult.Remembered)
                    {
                        userSession.Login(userId, loginDetails.Username, loginDetails.Email);
                        await authenticationService.SignInAsync(context,
                            _loginControllerSettings.AuthenticationScheme,
                            new ClaimsPrincipal(_claimsProvider.GetUserClaims(loginDetails.UserId)),
                            _claimsProvider.GetAuthenticationProperties());
                    }
                    else
                    {
                        CookieDelete(context, _loginControllerSettings.RememberMeCookieName);
                    }
                }
                else
                {
                    CookieDelete(context, _loginControllerSettings.RememberMeCookieName);
                }
            }
        }

        #endregion Private Methods
    }
}
