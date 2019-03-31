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
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

using SharedPluginFeatures;

using Middleware;

namespace LoginPlugin
{
    public class LoginMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;
        private readonly ILoginProvider _loginProvider;
        private readonly LoginControllerSettings _loginControllerSettings;
        internal static Timings _loginTimings = new Timings();
        internal static Timings _autoLoginTimings = new Timings();

        #endregion Private Members

        #region Constructors

        public LoginMiddleware(RequestDelegate next, ILoginProvider loginProvider,
            ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _loginProvider = loginProvider ?? throw new ArgumentNullException(nameof(loginProvider));

            _loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>("LoginPlugin");
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
                                userSession.Login(userId, loginDetails.Username, loginDetails.Email);
                            else
                                CookieDelete(context, _loginControllerSettings.RememberMeCookieName);
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
