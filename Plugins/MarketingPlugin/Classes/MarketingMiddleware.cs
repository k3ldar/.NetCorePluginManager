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
 *  Product:  MarketingPlugin
 *  
 *  File: MarketingMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Extensions.Localization;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;

using Shared.Classes;

using SharedPluginFeatures;
using static SharedPluginFeatures.Enums;

namespace MarketingPlugin
{
    public sealed class MarketingMiddleware : BaseMiddleware
    {
        #region Private Members

        private static readonly CacheManager _marketingCache = new CacheManager("Marketing Cache", new TimeSpan(0, 20, 0), true);
        private readonly RequestDelegate _next;
        private readonly string _staticFileExtensions = Constants.StaticFileExtensions;

        #endregion Private Members

        #region Constructors

        public MarketingMiddleware(RequestDelegate next, ISettingsProvider settingsProvider)
        {
            _next = next;

            ThreadManager.Initialise();

            MarketingSettings settings = settingsProvider.GetSettings<MarketingSettings>(Constants.PluginNameMarketing);

            if (!String.IsNullOrEmpty(settings.StaticFileExtensions))
                _staticFileExtensions = settings.StaticFileExtensions;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            string fileExtension = RouteFileExtension(context);

            if (!String.IsNullOrEmpty(fileExtension) &&
                _staticFileExtensions.Contains($"{fileExtension};"))
            {
                await _next(context);
                return;
            }

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(MarketingTimings))
            {
                CacheItem cache = _marketingCache.Get(Constants.CacheMarketing);

                if (cache == null)
                {

                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Internal Properties

        internal static Timings MarketingTimings { get; } = new Timings();

        #endregion Internal Properties

        #region Private Methods

        #endregion Private Methods
    }
}
