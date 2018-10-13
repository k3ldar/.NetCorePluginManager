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
using static System.IO.Path;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;

using static AspNetCore.PluginManager.PluginManagerService;

using Microsoft.AspNetCore.Http;

using Shared.Classes;

using SharedPluginFeatures;

namespace Spider.Plugin
{
    public sealed class SpiderMiddleware
    {
        #region Private Members

        private byte[] _spiderData;
        private readonly List<DeniedRoute> _deniedSpiderRoutes;
        private readonly bool _userSessionManagerLoaded;
        private readonly string DefaultController = "Home";
        private readonly RequestDelegate _next;
        private readonly bool _processStaticFiles;
        private readonly string _staticFileExtensions = ".less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;";

        #endregion Private Members

        #region Constructors

        public SpiderMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider, 
            IRouteDataService routeDataService)
        {
            if (routeProvider == null)
                throw new ArgumentNullException(nameof(routeProvider));

            if (routeDataService == null)
                throw new ArgumentNullException(nameof(routeDataService));

            _next = next;

            _userSessionManagerLoaded = PluginLoaded("UserSessionMiddleware.Plugin.dll", out int version);

            _deniedSpiderRoutes = new List<DeniedRoute>();
            LoadSpiderData(routeProvider, routeDataService);

            SpiderSettings settings = GetSpiderSettings();

            _processStaticFiles = settings.ProcessStaticFiles;

            if (!String.IsNullOrEmpty(settings.StaticFileExtensions))
                _staticFileExtensions = settings.StaticFileExtensions;
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            try
            {
                string fileExtension = GetExtension(context.Request.Path.ToString().ToLower());

                if (!_processStaticFiles &&  !String.IsNullOrEmpty(fileExtension) &&
                    _staticFileExtensions.Contains($"{fileExtension};"))
                {
                    await _next(context);
                    return;
                }

                string route = context.Request.Path.ToString().ToLower();

                if (route.EndsWith("/robots.txt"))
                {
                    context.Response.StatusCode = 200;
                    context.Response.Body.Write(_spiderData, 0, _spiderData.Length);
                }
                else
                {
                    if (_userSessionManagerLoaded)
                    {
                        if (context.Items.ContainsKey("UserSession"))
                        {
                            try
                            {
                                UserSession userSession = (UserSession)context.Items["UserSession"];

                                foreach (DeniedRoute deniedRoute in _deniedSpiderRoutes)
                                {
                                    if (userSession.IsBot &&
                                        deniedRoute.Route.StartsWith(route) &&
                                        (
                                            deniedRoute.UserAgent == "*" ||
#if NETCORE2_0
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
                                Initialisation.GetLogger.AddToLog(err);
                            }
                        }
                    }

                    await _next(context);
                }
            }
            catch (Exception error)
            {
                if (Initialisation.GetLogger != null)
                    Initialisation.GetLogger.AddToLog(error, MethodBase.GetCurrentMethod().Name);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private SpiderSettings GetSpiderSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            SpiderSettings Result = new SpiderSettings();
            config.GetSection("Spider.Plugin").Bind(Result);

            return (Result);
        }

        private void LoadSpiderData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService)
        {
            string spiderTextFile = String.Empty;
            List<Type> spiderAttributes = GetPluginTypesWithAttribute<DenySpiderAttribute>();

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
