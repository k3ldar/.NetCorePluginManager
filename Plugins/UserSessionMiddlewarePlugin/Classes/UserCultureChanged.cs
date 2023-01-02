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
 *  File: UserCultureChanged.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Globalization;

using Microsoft.AspNetCore.Http;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace UserSessionMiddleware.Plugin
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used internally as part of IoC")]
    internal class UserCultureChanged : BaseMiddleware, IUserCultureChangeProvider
    {
        #region Private Members

        private readonly ISettingsProvider _settingsProvider;

        #endregion Private Members

        #region Constructors

        public UserCultureChanged(ISettingsProvider settingsProvider)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
        }

        #endregion Constructors

        #region IUserCultureChangeProvider Methods

        public void CultureChanged(in HttpContext httpContext, in UserSession userSession, in CultureInfo cultureInfo)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            if (userSession == null)
                throw new ArgumentNullException(nameof(userSession));

            if (cultureInfo == null)
                throw new ArgumentNullException(nameof(cultureInfo));

            UserSessionSettings settings = _settingsProvider.GetSettings<UserSessionSettings>(Constants.UserSessionConfiguration);

            CookieAdd(httpContext, Constants.UserCulture,
                Shared.Utilities.Encrypt(cultureInfo.Name, settings.EncryptionKey), 365);
        }

        #endregion IUserCultureChangeProvider Methods
    }
}
