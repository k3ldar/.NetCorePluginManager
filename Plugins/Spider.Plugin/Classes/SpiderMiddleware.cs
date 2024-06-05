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
 *  Product:  Spider.Plugin
 *  
 *  File: SpiderMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *  13/10/2018  Simon Carter
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Spider.Plugin
{
    /// <summary>
    /// Spider middleware, serves robots.txt on request and denies access to route for spider connections.
    /// </summary>
    public sealed class SpiderMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly bool _userSessionManagerLoaded;
        private readonly RequestDelegate _next;
        private readonly bool _processStaticFiles;
        private readonly string _staticFileExtensions = Constants.StaticFileExtensions;
        private readonly string _botTrap;
        private readonly bool _useBotTrap;
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;
        private readonly IRobots _robots;

        internal readonly static Timings _timings = new();
        internal readonly static Timings _botTrapTimings = new();

        #endregion Private Members

        #region Constructors

        public SpiderMiddleware(RequestDelegate next, IPluginHelperService pluginHelperService,
            ISettingsProvider settingsProvider, ILogger logger,
            INotificationService notificationService, IRobots robots)
        {

            if (pluginHelperService == null)
                throw new ArgumentNullException(nameof(pluginHelperService));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _robots = robots ?? throw new ArgumentNullException(nameof(robots));

            _next = next;

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userSessionManagerLoaded = pluginHelperService.PluginLoaded(Constants.PluginNameUserSession, out int _);

            SpiderSettings settings = settingsProvider.GetSettings<SpiderSettings>(Constants.SpiderSettings);
            _botTrap = settings.BotTrapRoute;
            _useBotTrap = !String.IsNullOrEmpty(_botTrap);

            _processStaticFiles = settings.ProcessStaticFiles;

            if (!String.IsNullOrEmpty(settings.StaticFileExtensions))
                _staticFileExtensions = settings.StaticFileExtensions;
        }

        #endregion Constructors

        #region Public Methods

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            string fileExtension = RouteFileExtension(context);

            if (!_processStaticFiles && !String.IsNullOrEmpty(fileExtension) &&
                _staticFileExtensions.Contains($"{fileExtension};"))
            {
                await _next(context);
                return;
            }

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                string route = RouteLowered(context);

                if (_useBotTrap && route.StartsWith(_botTrap))
                {
                    using (StopWatchTimer botTrapTimings = StopWatchTimer.Initialise(_botTrapTimings))
                    {
                        IBotTrap botTrapLogger = context.RequestServices.GetService(typeof(IBotTrap)) as IBotTrap;

                        if (botTrapLogger == null)
                        {
                            context.Response.StatusCode = 403;
                        }
                        else
                        {
                            try
                            {
                                botTrapLogger.OnTrapEntered(GetIpAddress(context), GetUserAgent(context));
                            }
                            catch (Exception err)
                            {
                                _logger.AddToLog(LogLevel.Error, err, nameof(IBotTrap));
                            }

                            context.Response.StatusCode = 405;
                        }

                        return;
                    }
                }
                else if (route.EndsWith("/robots.txt"))
                {
                    context.Response.StatusCode = Constants.HtmlResponseSuccess;

                    // append sitemaps if there are any
                    object notificationResult = new();

                    StringBuilder stringBuilder = new(LoadSpiderData());

                    if (_notificationService.RaiseEvent(Constants.NotificationSitemapNames, context, null, ref notificationResult))
                    {
                        string[] sitemaps = ((System.Collections.IEnumerable)notificationResult)
                          .Cast<object>()
                          .Select(x => x.ToString())
                          .ToArray();

                        if (sitemaps != null)
                        {

                            string url = GetHost(context);

                            for (int i = 0; i < sitemaps.Length; i++)
                            {
                                stringBuilder.Append($"\r\n\r\nSitemap: {url}{sitemaps[i].Substring(1)}\n");
                            }
                        }
                    }

                    byte[] response = Encoding.UTF8.GetBytes(stringBuilder.ToString());
                    await context.Response.Body.WriteAsync(response, 0, response.Length);
                    return;
                }
                else
                {
                    if (_userSessionManagerLoaded && context.Items.ContainsKey(Constants.UserSession))
                    {
                        try
                        {
                            UserSession userSession = (UserSession)context.Items[Constants.UserSession];

                            foreach (DeniedRoute deniedRoute in _robots.DeniedRoutes)
                            {
                                if (userSession.IsBot &&
                                    deniedRoute.Route.StartsWith(route) &&
                                    (
                                        deniedRoute.UserAgent == "*" ||
#if NET_CORE
                                        userSession.UserAgent.Contains(deniedRoute.UserAgent, StringComparison.CurrentCultureIgnoreCase)
#else 
                                        userSession.UserAgent.ToLower().Contains(deniedRoute.UserAgent.ToLower())
#endif
                                    ))
                                {
                                    context.Response.StatusCode = 403;
                                    return;
                                }
                            }
                        }
                        catch (Exception err)
                        {
                            _logger.AddToLog(LogLevel.Error, nameof(SpiderMiddleware), err, MethodBase.GetCurrentMethod().Name);
                            throw;
                        }
                    }
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private string LoadSpiderData()
        {
            StringBuilder spiderTextFile = new();

            if (_robots.Agents.Count == 0)
            {
                spiderTextFile.Append("# Allow all from Spider.Plugin\r\n\r\nUser-agent: *\r\nAllow: /\r\n");
            }
            else
            {
                string lastAgent = String.Empty;
                spiderTextFile.Append("# Automatically genterated by Spider.Plugin\r\n");

                foreach (string agent in _robots.Agents)
                {
                    List<string> routes = _robots.GetRoutes(agent);

                    if (routes.Count == 0)
                        continue;

                    if (_useBotTrap && agent.Equals("*"))
                        AddBotTrap(routes);

                    if (!lastAgent.Equals(agent))
                    {
                        lastAgent = agent;
                        spiderTextFile.Append($"\r\nUser-agent: {lastAgent}\r\n");
                    }

                    foreach (string value in routes)
                    {
                        spiderTextFile.Append($"{value}\r\n");
                    }
                }
            }

            return spiderTextFile.ToString();
        }

        private void AddBotTrap(List<string> agents)
        {
            int pos = agents.Count == 0 ? 0 : agents.Count / 2;

            agents.Insert(pos, $"Disallow: {_botTrap}");
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591