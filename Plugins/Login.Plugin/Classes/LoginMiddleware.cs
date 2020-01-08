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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Login Plugin
 *  
 *  File: LoginMiddleware.cs
 *
 *  Purpose:  Auto Login Middleware
 *
 *  Date        Name                Reason
 *  17/02/2019  Simon Carter        Initially Created
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
        private readonly IAuthenticationService _authenticationService;
        private readonly IClaimsProvider _claimsProvider;
        internal static Timings _loginTimings = new Timings();
        internal static Timings _autoLoginTimings = new Timings();

        #endregion Private Members

        #region Constructors

        public LoginMiddleware(RequestDelegate next, ILoginProvider loginProvider,
            ISettingsProvider settingsProvider, IAuthenticationService authenticationService,
            IClaimsProvider claimsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _loginProvider = loginProvider ?? throw new ArgumentNullException(nameof(loginProvider));
            _authenticationService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
            _claimsProvider = claimsProvider ?? throw new ArgumentNullException(nameof(claimsProvider));
            _loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_loginTimings))
            {
                UserSession userSession = GetUserSession(context);

                if (userSession != null && String.IsNullOrEmpty(userSession.UserName) &&
                    CookieExists(context, _loginControllerSettings.RememberMeCookieName))
                {
                    using (StopWatchTimer stopwatchAutoLogin = StopWatchTimer.Initialise(_autoLoginTimings))
                    {
                        string cookieValue = CookieValue(context, _loginControllerSettings.RememberMeCookieName,
                            _loginControllerSettings.EncryptionKey, String.Empty);

                        if (Int64.TryParse(cookieValue, out long userId))
                        {
                            UserLoginDetails loginDetails = new UserLoginDetails(userId, true);

                            LoginResult loginResult = _loginProvider.Login(String.Empty, String.Empty,
                                base.GetIpAddress(context), 1, ref loginDetails);

                            if (loginResult == LoginResult.Remembered)
                            {
                                userSession.Login(userId, loginDetails.Username, loginDetails.Email);
                                await _authenticationService.SignInAsync(context,
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
            }

            await _next(context);
        }

        #endregion Public Methods
    }
}
