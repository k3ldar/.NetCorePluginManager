﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  CacheControl Plugin
 *  
 *  File: CacheControlMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  14/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CacheControl.Plugin
{
    public class CacheControlMiddleware
    {
        #region Private Members

        private readonly RequestDelegate _next;

        private readonly Dictionary<string, CacheControlRoute> _routePaths;
        private readonly HashSet<string> _ignoredRoutes;
        private bool _disabled;

        #endregion Private Members

        #region Constructors

        public CacheControlMiddleware(RequestDelegate next)
        {
            _next = next;
            _routePaths = new Dictionary<string, CacheControlRoute>();
            _ignoredRoutes = new HashSet<string>();

            LoadSettings(GetSettings());
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (_disabled)
                    return;

                string routeLowered = context.Request.Path.ToString().ToLower();

                if (_ignoredRoutes.Contains(routeLowered))
                    return;

                if (!context.Response.Headers.ContainsKey("Cache-Control"))
                {

                    foreach (KeyValuePair<string, CacheControlRoute> keyValuePair in _routePaths)
                    {
                        if (routeLowered.StartsWith(keyValuePair.Key))
                        {
                            context.Response.Headers.Add("Cache-Control", $"max-age={keyValuePair.Value.CacheValue}");
                            return;
                        }
                    }
                }

                _ignoredRoutes.Add(routeLowered);
            }
            catch (Exception error)
            {
                if (Initialisation.GetLogger != null)
                    Initialisation.GetLogger.AddToLog(error, System.Reflection.MethodBase.GetCurrentMethod().Name);
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

        private CacheControlSettings GetSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            CacheControlSettings Result = new CacheControlSettings();
            config.GetSection("CacheControlRoute").Bind(Result);

            return (Result);
        }

        #endregion Private Methods
    }
}