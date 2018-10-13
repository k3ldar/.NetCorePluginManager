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
 *  Product:  Request Manager Plugin
 *  
 *  File: Initialisation.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using static System.IO.Path;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

using AspNetCore.PluginManager;

using Shared.Classes;

using SharedPluginFeatures;

using Shared.Classes;

namespace ManagedRoute.Plugin
{
    public class ManagedRouteMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;

        private readonly ManagedRouteSettings _settings;

        private readonly Dictionary<string, ManagedRoute> _routePaths;

        #endregion Private Members

        #region Constructors

        public ManagedRouteMiddleware(RequestDelegate next)
        {
            _next = next;
            _settings = GetSettings();
            _routePaths = new Dictionary<string, ManagedRoute>();

            LoadRoutes();

            ThreadManager.Initialise();
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            bool passRequestOn = true;

            try
            {
                string routeLowered = context.Request.Path.ToString().ToLower();

                if (_routePaths.ContainsKey(routeLowered))
                {
                    // we is managing this route
                    ManagedRoute managedRoute = _routePaths[routeLowered];

                    string userAgent = context.Request.Headers["User-Agent"].ToString();
                    string ipAddress = context.Connection.RemoteIpAddress.ToString();

                    CacheItem cacheItem = managedRoute.CacheManager.Get(ipAddress);

                    if (cacheItem == null)
                    {
                        cacheItem = new CacheItem(ipAddress, new RouteRequests());
                        managedRoute.CacheManager.Add(ipAddress, cacheItem);
                    }

                    RouteRequests currentRequests = (RouteRequests)cacheItem.Value;
                    currentRequests.CurrentRequests++;

                    if (currentRequests.CurrentRequests > managedRoute.MaximumRequests &&
                        userAgent.Contains(managedRoute.UserAgent))
                    {
                        passRequestOn = false;
                        context.Response.StatusCode = managedRoute.ResponseCode;
                    }
                }
            }
            catch (Exception error)
            {
                if (Initialisation.GetLogger != null)
                    Initialisation.GetLogger.AddToLog(error, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            finally
            {
                if (passRequestOn)
                    await _next(context);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadRoutes()
        {
            foreach (KeyValuePair<string, ManagedRoute> keyValue in _settings.ManagedRoutes)
            {
                keyValue.Value.CacheManager = new CacheManager($"Managed Route {keyValue.Key}",
                    CreateCacheAge(keyValue.Value.RequestPeriod), true, false);

                if (keyValue.Value.UserAgent == "*")
                    keyValue.Value.UserAgent = String.Empty;

                foreach (string route in keyValue.Value.Route)
                {
                    _routePaths.Add(route.ToLower(), keyValue.Value);
                }
            }
        }

        private TimeSpan CreateCacheAge(Enums.RequestPeriod requestPeriod)
        {
            switch (requestPeriod)
            {
                case Enums.RequestPeriod.Second:
                    return new TimeSpan(0, 0, 1);

                case Enums.RequestPeriod.Minute:
                    return new TimeSpan(0, 0, 60);

                case Enums.RequestPeriod.Hour:
                    return new TimeSpan(0, 1, 0);

                case Enums.RequestPeriod.Month:
                    return new TimeSpan(31 * 24, 0, 0);

                case Enums.RequestPeriod.Week:
                    return new TimeSpan(7 * 24, 0, 0);

                default:
                    throw new ArgumentOutOfRangeException(nameof(requestPeriod));
            }
        }

        private ManagedRouteSettings GetSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            ManagedRouteSettings Result = new ManagedRouteSettings();
            config.GetSection("ManagedRoute").Bind(Result);

            return (Result);
        }

        #endregion Private Methods
    }
}
