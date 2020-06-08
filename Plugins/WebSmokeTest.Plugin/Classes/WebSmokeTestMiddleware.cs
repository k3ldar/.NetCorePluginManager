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
 *  Product:  WebSmokeTest.Plugin
 *  
 *  File: WebSmokeTestMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/06/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

using Newtonsoft.Json;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace WebSmokeTest.Plugin
{
    /// <summary>
    /// WebSmokeTest middleware class, this module extends BaseMiddlware and is injected 
    /// into the request pipeline.
    /// </summary>
    public sealed class WebSmokeTestMiddleware : BaseMiddleware
    {
        #region Private Members

        private static readonly CacheManager _testCache = new CacheManager("Web Smoke Test Cache", new TimeSpan(0, 20, 0), true);
        private readonly RequestDelegate _next;
        private readonly string _staticFileExtensions = Constants.StaticFileExtensions;
        internal static Timings _timings = new Timings();
        private readonly IStringLocalizer _stringLocalizer;
        private readonly ILogger _logger;
        private readonly WebSmokeTestSettings _settings;

        #endregion Private Members

        #region Constructors

        public WebSmokeTestMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginHelperService pluginHelperService,
            IPluginTypesService pluginTypesService, ISettingsProvider settingsProvider,
            IPluginClassesService pluginClassesService, ILogger logger)
        {
            if (routeProvider == null)
                throw new ArgumentNullException(nameof(routeProvider));

            if (routeDataService == null)
                throw new ArgumentNullException(nameof(routeDataService));

            if (pluginHelperService == null)
                throw new ArgumentNullException(nameof(pluginHelperService));

            if (pluginClassesService == null)
                throw new ArgumentNullException(nameof(pluginClassesService));

            if (pluginTypesService == null)
                throw new ArgumentNullException(nameof(pluginTypesService));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            if (pluginHelperService.PluginLoaded(Constants.PluginNameLocalization, out int _))
            {
                List<IStringLocalizer> stringLocalizers = pluginClassesService.GetPluginClasses<IStringLocalizer>();

                if (stringLocalizers.Count > 0)
                    _stringLocalizer = stringLocalizers[0];

            }

            _settings = settingsProvider.GetSettings<WebSmokeTestSettings>(nameof(WebSmokeTest));

            if (_settings.Enabled)
            {
                LoadSmokeTestData(routeProvider, routeDataService, pluginTypesService, _settings);
            }

            if (!String.IsNullOrEmpty(_settings.StaticFileExtensions))
                _staticFileExtensions = _settings.StaticFileExtensions;
        }

        #endregion Constructors

        #region Public Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            string fileExtension = RouteFileExtension(context);

            if (!String.IsNullOrEmpty(fileExtension) &&
                _staticFileExtensions.Contains($"{fileExtension};"))
            {
                await _next(context);
                return;
            }

            string route = RouteLowered(context);

            if (route.StartsWith("/smoketest/"))
            {
                using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
                {

                    try
                    {
                        if (route.Equals("/smoketest/siteid/"))
                        {
                            if (_settings.Enabled)
                            {
                                byte[] siteId = Encoding.UTF8.GetBytes(_settings.SiteId);
                                context.Response.Body.Write(siteId, 0, siteId.Length);
                            }
                            else
                            {
                                context.Response.StatusCode = 405;
                            }
                        }
                        else if (route.Equals("/smoketest/testcount/"))
                        {
                            if (_settings.Enabled)
                            {
                                byte[] siteId = Encoding.UTF8.GetBytes(GetTestItems().Count.ToString());
                                context.Response.Body.Write(siteId, 0, siteId.Length);
                            }
                            else
                            {
                                context.Response.StatusCode = 405;
                            }
                        }
                        else if (route.StartsWith("/smoketest/test"))
                        {
                            string testNumber = route.Substring(16);
                            List<WebSmokeTestItem> testItems = GetTestItems();

                            if (Int32.TryParse(testNumber, out int number) &&
                                number >= 0 &&
                                number <= testItems.Count)
                            {
                                byte[] testData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testItems[number]));
                                context.Response.Body.Write(testData, 0, testData.Length);
                                context.Response.ContentType = "application/json";
                            }
                            else
                            {
                                context.Response.StatusCode = 405;
                            }
                        }
                        else
                        {
                            context.Response.StatusCode = 404;
                        }

                        return;
                    }
                    catch (Exception err)
                    {
                        _logger.AddToLog(LogLevel.Error, nameof(WebSmokeTestMiddleware), err, MethodBase.GetCurrentMethod().Name);
                    }
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private List<WebSmokeTestItem> GetTestItems()
        {
            return new List<WebSmokeTestItem>();
        }

        private void LoadSmokeTestData(in IActionDescriptorCollectionProvider routeProvider,
            in IRouteDataService routeDataService,
            in IPluginTypesService pluginTypesService,
            in WebSmokeTestSettings settings)
        {
            Dictionary<string, SmokeTestAttribute> allSmokeTests = new Dictionary<string, SmokeTestAttribute>();
            List<Type> testAttributes = pluginTypesService.GetPluginTypesWithAttribute<SmokeTestAttribute>();

            // Cycle through all methods which have the SmokeTestAttribute attribute
            foreach (Type type in testAttributes)
            {
                // is it a class attribute
                SmokeTestAttribute attribute = (SmokeTestAttribute)type.GetCustomAttributes(true)
                    .Where(a => a.GetType() == typeof(SmokeTestAttribute)).FirstOrDefault();

                // look for specific method smoke test
                foreach (MethodInfo method in type.GetMethods())
                {
                    attribute = (SmokeTestAttribute)method.GetCustomAttributes(true)
                        .Where(a => a.GetType() == typeof(SmokeTestAttribute)).FirstOrDefault();

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        allSmokeTests.Add(route, attribute);
                    }
                }
            }
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591