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
 *  Product:  CacheControl Plugin
 *  
 *  File: CacheControlMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *  27/10/2018  Simon Carter        Add Locking to dictionary
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace CacheControl.Plugin
{
    /// <summary>
    /// Implements rule based cache headers for individual routes
    /// </summary>
    public class CacheControlMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;

        private readonly Dictionary<string, CacheControlRoute> _routePaths;
        private readonly HashSet<string> _ignoredRoutes;
        private bool _disabled;
        private readonly object _lockObject = new object();
        internal readonly static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public CacheControlMiddleware(RequestDelegate next, ISettingsProvider settingsProvider)
        {
            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _routePaths = new Dictionary<string, CacheControlRoute>();
            _ignoredRoutes = new HashSet<string>();

            LoadSettings(settingsProvider.GetSettings<CacheControlSettings>("CacheControlRoute"));
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            try
            {
                if (_disabled)
                    return;

                using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
                {
                    string routeLowered = RouteLowered(context);

                    using (TimedLock lck = TimedLock.Lock(_lockObject))
                    {
                        if (_ignoredRoutes.Contains(routeLowered))
                            return;
                    }

                    if (!context.Response.Headers.ContainsKey("Cache-Control"))
                    {

                        foreach (KeyValuePair<string, CacheControlRoute> keyValuePair in _routePaths)
                        {
                            if (routeLowered.StartsWith(keyValuePair.Key))
                            {
                                context.Response.Headers["Cache-Control"] = $"max-age={keyValuePair.Value.CacheValue}";
                                return;
                            }
                        }
                    }

                    using (TimedLock lck = TimedLock.Lock(_lockObject))
                    {
                        _ignoredRoutes.Add(routeLowered);
                    }
                }
            }

            finally
            {
                await _next(context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadSettings(CacheControlSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            _disabled = settings.Disabled;

            foreach (KeyValuePair<string, CacheControlRoute> keyValue in settings.CacheControlRoutes)
            {
                if (keyValue.Value.CacheMinutes < 1)
                    keyValue.Value.CacheValue = "no-cache";
                else
                    keyValue.Value.CacheValue = Convert.ToString(keyValue.Value.CacheMinutes * 60);

                foreach (string route in keyValue.Value.Route)
                {
                    _routePaths.Add(route.ToLower(), keyValue.Value);
                }
            }
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591