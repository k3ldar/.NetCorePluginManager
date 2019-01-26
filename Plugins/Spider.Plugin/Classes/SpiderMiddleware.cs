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

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Http;

using Shared.Classes;

using SharedPluginFeatures;
using static SharedPluginFeatures.Enums;

namespace Spider.Plugin
{
    public sealed class SpiderMiddleware : BaseMiddleware
    {
        #region Private Members

        private byte[] _spiderData;
        private readonly List<DeniedRoute> _deniedSpiderRoutes;
        private readonly bool _userSessionManagerLoaded;
        private readonly RequestDelegate _next;
        private readonly bool _processStaticFiles;
        private readonly string _staticFileExtensions = Constants.StaticFileExtensions;
        internal static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public SpiderMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider, 
            IRouteDataService routeDataService, IPluginHelperService pluginHelperService,
            IPluginTypesService pluginTypesService, ISettingsProvider settingsProvider)
        {
            if (routeProvider == null)
                throw new ArgumentNullException(nameof(routeProvider));

            if (routeDataService == null)
                throw new ArgumentNullException(nameof(routeDataService));

            if (pluginHelperService == null)
                throw new ArgumentNullException(nameof(pluginHelperService));

            _next = next;

            _userSessionManagerLoaded = pluginHelperService.PluginLoaded("UserSessionMiddleware.Plugin.dll", out int version);

            _deniedSpiderRoutes = new List<DeniedRoute>();
            LoadSpiderData(routeProvider, routeDataService, pluginTypesService);

            SpiderSettings settings = settingsProvider.GetSettings<SpiderSettings>("Spider.Plugin");

            _processStaticFiles = settings.ProcessStaticFiles;

            if (!String.IsNullOrEmpty(settings.StaticFileExtensions))
                _staticFileExtensions = settings.StaticFileExtensions;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            string fileExtension = RouteFileExtension(context);

            if (!_processStaticFiles &&  !String.IsNullOrEmpty(fileExtension) &&
                _staticFileExtensions.Contains($"{fileExtension};"))
            {
                await _next(context);
                return;
            }

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                string route = RouteLowered(context);

                if (route.EndsWith("/robots.txt"))
                {
                    context.Response.StatusCode = 200;
                    context.Response.Body.Write(_spiderData, 0, _spiderData.Length);
                }
                else
                {
                    if (_userSessionManagerLoaded)
                    {
                        if (context.Items.ContainsKey(Constants.UserSession))
                        {
                            try
                            {
                                UserSession userSession = (UserSession)context.Items[Constants.UserSession];

                                foreach (DeniedRoute deniedRoute in _deniedSpiderRoutes)
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
                                Initialisation.GetLogger.AddToLog(LogLevel.SpiderRouteError, err, MethodBase.GetCurrentMethod().Name);
                            }
                        }
                    }
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadSpiderData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            string spiderTextFile = String.Empty;
            List<Type> spiderAttributes = pluginTypesService.GetPluginTypesWithAttribute<DenySpiderAttribute>();

            if (spiderAttributes.Count == 0)
            {
                spiderTextFile = "# Allow all from Spider.Plugin\r\n\r\nUser-agent: *";
            }
            else
            {
                // Cycle through all classes and methods which have the spider attribute
                foreach (Type type in spiderAttributes)
                {
                    // is it a class attribute
                    DenySpiderAttribute attribute = (DenySpiderAttribute)type.GetCustomAttributes(true)
                        .Where(a => a.GetType() == typeof(DenySpiderAttribute)).FirstOrDefault();

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromClass(type, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        if (!String.IsNullOrEmpty(attribute.Comment))
                            spiderTextFile += $"# {attribute.Comment}\r\n\r\n";

                        spiderTextFile += $"User-agent: {attribute.UserAgent}\r\n";
                        spiderTextFile += $"Disallow: /{route}/\r\n\r\n";

                        _deniedSpiderRoutes.Add(new DeniedRoute($"/{route.ToLower()}/", attribute.UserAgent));
                    }

                    // look for specific method disallows

                    foreach (MethodInfo method in type.GetMethods())
                    {
                        attribute = (DenySpiderAttribute)method.GetCustomAttributes(true)
                            .Where(a => a.GetType() == typeof(DenySpiderAttribute)).FirstOrDefault();

                        if (attribute != null)
                        {
                            string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                            if (String.IsNullOrEmpty(route))
                                continue;

                            if (!String.IsNullOrEmpty(attribute.Comment))
                                spiderTextFile += $"# {attribute.Comment}\r\n\r\n";

                            spiderTextFile += $"User-agent: {attribute.UserAgent}\r\n";
                            spiderTextFile += $"Disallow: {route}\r\n\r\n";

                            _deniedSpiderRoutes.Add(new DeniedRoute($"{route.ToLower()}", attribute.UserAgent));
                        }
                    }
                }
            }

            _spiderData = Encoding.UTF8.GetBytes(spiderTextFile);
        }

        #endregion Private Methods
    }
}
