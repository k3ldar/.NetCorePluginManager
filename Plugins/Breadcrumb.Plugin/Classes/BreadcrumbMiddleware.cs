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
 *  Product:  Breadcrumb.Plugin
 *  
 *  File: SpiderMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  20/01/2019  Simon Carter        Initially Created
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

namespace Breadcrumb.Plugin
{
    public sealed class BreadcrumbMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly Dictionary<string, SharedPluginFeatures.Breadcrumb> _breadcrumbRoutes;
        private readonly RequestDelegate _next;
        private readonly string _staticFileExtensions = ".less;.ico;.css;.js;.svg;.jpg;.jpeg;.gif;.png;.eot;";
        internal static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public BreadcrumbMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider, 
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

            _breadcrumbRoutes = new Dictionary<string, SharedPluginFeatures.Breadcrumb>();
            LoadBreadcrumbData(routeProvider, routeDataService, pluginTypesService);

            BreadcrumbSettings settings = settingsProvider.GetSettings<BreadcrumbSettings>("Breadcrumb.Plugin");

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

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                string route = RouteLowered(context);

            try
                {
                    if (_breadcrumbRoutes.ContainsKey(route))
                    {

                    }
                }
                catch (Exception err)
                {
                    Initialisation.GetLogger.AddToLog(LogLevel.BreadcrumbError, err, MethodBase.GetCurrentMethod().Name);
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadBreadcrumbData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            Dictionary<string, BreadcrumbAttribute> allBreadcrumbs = new Dictionary<string, BreadcrumbAttribute>();
            List<Type> breadcrumbAttributes = pluginTypesService.GetPluginTypesWithAttribute<DenySpiderAttribute>();

            // Cycle through all methods which have the spider attribute
            foreach (Type type in breadcrumbAttributes)
            {
                // is it a class attribute
                BreadcrumbAttribute attribute = (BreadcrumbAttribute)type.GetCustomAttributes(true)
                    .Where(a => a.GetType() == typeof(BreadcrumbAttribute)).FirstOrDefault();

                //if (attribute != null)
                //{
                //    string route = routeDataService.GetRouteFromClass(type, routeProvider);

                //    if (String.IsNullOrEmpty(route))
                //        continue;

                //    if (!String.IsNullOrEmpty(attribute.Comment))
                //        spiderTextFile += $"# {attribute.Comment}\r\n\r\n";

                //    spiderTextFile += $"User-agent: {attribute.UserAgent}\r\n";
                //    spiderTextFile += $"Disallow: /{route}/\r\n\r\n";

                //    _breadcrumbRoutes.Add(new BreadcrumbRoute($"/{route.ToLower()}/", attribute.UserAgent));
                //}

                // look for specific method disallows

                foreach (MethodInfo method in type.GetMethods())
                {
                    attribute = (BreadcrumbAttribute)method.GetCustomAttributes(true)
                        .Where(a => a.GetType() == typeof(BreadcrumbAttribute)).FirstOrDefault();

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        allBreadcrumbs.Add(route.ToLower(), attribute);


                    }
                }



                //_breadcrumbRoutes.Add(new BreadcrumbRoute($"{route.ToLower()}"));
            }
        }

        #endregion Private Methods
    }
}
