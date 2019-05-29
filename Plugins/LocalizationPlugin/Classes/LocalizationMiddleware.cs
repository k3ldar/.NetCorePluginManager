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
 *  Product:  Localization.Plugin
 *  
 *  File: LocalizationMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  12/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Globalization;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using SharedPluginFeatures;

using Shared.Classes;

#pragma warning disable CS1591

namespace Localization.Plugin
{
    /// <summary>
    /// Localization middleware class, processes all localiztion requests whilst in the request pipeline.
    /// </summary>
    public sealed class LocalizationMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;
        private static readonly Timings _timings = new Timings();
        private readonly CacheManager _cultureCache;

        #endregion Private Members

        #region Constructors
 
        public LocalizationMiddleware(RequestDelegate next, ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next;

            ThreadManager.Initialise();
            _cultureCache = Initialisation.CultureCache;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                if (context.Items.TryGetValue(Constants.UserCulture, out object sessionCulture))
                {
                    SetUserCulture((string)sessionCulture);
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Internal Properties

        internal static Timings LocalizationTimings
        {
            get
            {
                return _timings;
            }
        }

        #endregion Internal Properties

        #region Private Methods

        private void SetUserCulture(in string culture)
        {
            if (String.IsNullOrEmpty(culture))
                throw new ArgumentNullException(nameof(culture));

            CacheItem cacheItem = _cultureCache.Get(culture);

            if (cacheItem == null)
            {
                cacheItem = new CacheItem(culture, new CultureInfo(culture));
                _cultureCache.Add(culture, cacheItem);
            }

            CultureInfo cultureInfo = (CultureInfo)cacheItem.Value;

            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591