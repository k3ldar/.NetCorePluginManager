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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Breadcrumb.Plugin
 *  
 *  File: BreadcrumbMiddleware.cs
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
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Localization;

using PluginManager;
using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591

namespace Breadcrumb.Plugin
{
    /// <summary>
    /// Breadcrumb middleware class, this module extends BaseMiddlware and is injected 
    /// into the request pipeline.
    /// </summary>
    public sealed class BreadcrumbMiddleware : BaseMiddleware
    {
        #region Private Members

        private static readonly Dictionary<string, BreadcrumbRoute> _breadcrumbRoutes = new Dictionary<string, BreadcrumbRoute>();
        private static readonly CacheManager _breadcrumbCache = new CacheManager("Breadcrumb Cache", new TimeSpan(0, 20, 0), true);
        private readonly RequestDelegate _next;
        private readonly string _staticFileExtensions = Constants.StaticFileExtensions;
        internal readonly static Timings _timings = new Timings();
        private readonly IStringLocalizer _stringLocalizer;
        private readonly BreadcrumbItem _homeBreadCrumb;
        private readonly ILogger _logger;

        #endregion Private Members

        #region Constructors

        public BreadcrumbMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider,
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

            BreadcrumbSettings settings = settingsProvider.GetSettings<BreadcrumbSettings>(Constants.PluginSettingBreadcrumb);

            _homeBreadCrumb = new BreadcrumbItem(settings.HomeName,
                $"{Constants.ForwardSlash}{settings.HomeController}{Constants.ForwardSlash}{settings.DefaultAction}", false);

            LoadBreadcrumbData(routeProvider, routeDataService, pluginTypesService, settings);

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

            if (!String.IsNullOrEmpty(fileExtension) &&
                _staticFileExtensions.Contains($"{fileExtension};"))
            {
                await _next(context);
                return;
            }

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                string route = RouteLowered(context);

                if (route.Length > 1 && route[route.Length - 1] == Constants.ForwardSlashChar)
                    route = route.Substring(0, route.Length - 1);

                try
                {
                    bool found = false;

                    if (_breadcrumbRoutes.TryGetValue(route, out BreadcrumbRoute value))
                    {
                        context.Items.Add(Constants.Breadcrumbs,
                            GetBreadCrumbs(route, value.Breadcrumbs, String.Empty));
                        found = true;
                    }
                    else
                    {
                        foreach (KeyValuePair<string, BreadcrumbRoute> kvp in _breadcrumbRoutes)
                        {
                            if (route.StartsWith(kvp.Value.PartialRoute) && kvp.Value.HasParameters)
                            {
                                context.Items.Add(Constants.Breadcrumbs,
                                    GetBreadCrumbs(route, kvp.Value.Breadcrumbs, Route(context).Substring(kvp.Value.PartialRoute.Length - 1)));
                                found = true;
                                break;
                            }
                        }
                    }

                    if (!found)
                    {
                        BreadcrumbItem homeBreadCrumb;

                        if (_stringLocalizer != null)
                            homeBreadCrumb = new BreadcrumbItem(_stringLocalizer[_homeBreadCrumb.Name], _homeBreadCrumb.Route, _homeBreadCrumb.HasParameters);
                        else
                            homeBreadCrumb = _homeBreadCrumb;

                        context.Items.Add(Constants.Breadcrumbs, new List<BreadcrumbItem>() { homeBreadCrumb });
                    }
                }
                catch (Exception err)
                {
                    _logger.AddToLog(LogLevel.Error, nameof(BreadcrumbMiddleware), err, MethodBase.GetCurrentMethod().Name);
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Internal Properties

        internal static Dictionary<string, BreadcrumbRoute> Routes
        {
            get
            {
                return _breadcrumbRoutes;
            }
        }

        #endregion Internal Properties

        #region Private Methods

        private List<BreadcrumbItem> GetBreadCrumbs(in string route, in List<BreadcrumbItem> breadcrumbs,
            string routeParameters)
        {
            string cacheName = $"{route} {System.Threading.Thread.CurrentThread.CurrentUICulture} {routeParameters}";

            CacheItem cacheItem = _breadcrumbCache.Get(cacheName);

            if (cacheItem == null)
            {
                List<BreadcrumbItem> Result = new List<BreadcrumbItem>();

                for (int i = 0; i < breadcrumbs.Count; i++)
                {
                    BreadcrumbItem item = breadcrumbs[i];

                    if (i == breadcrumbs.Count - 1)
                    {
                        if (_stringLocalizer != null)
                            Result.Add(new BreadcrumbItem(_stringLocalizer[item.Name], item.Route + routeParameters, item.HasParameters));
                        else
                            Result.Add(new BreadcrumbItem(item.Name, item.Route + routeParameters, item.HasParameters));
                    }
                    else
                    {
                        if (_stringLocalizer != null)
                            Result.Add(new BreadcrumbItem(_stringLocalizer[item.Name], item.Route, item.HasParameters));
                        else
                            Result.Add(new BreadcrumbItem(item.Name, item.Route, item.HasParameters));
                    }
                }

                cacheItem = new CacheItem(cacheName, Result);
                _breadcrumbCache.Add(cacheName, cacheItem);
            }

            return (List<BreadcrumbItem>)cacheItem.Value;
        }

        private void LoadBreadcrumbData(in IActionDescriptorCollectionProvider routeProvider,
            in IRouteDataService routeDataService,
            in IPluginTypesService pluginTypesService,
            in BreadcrumbSettings settings)
        {
            Dictionary<string, BreadcrumbAttribute> allBreadcrumbs = new Dictionary<string, BreadcrumbAttribute>();
            List<Type> breadcrumbAttributes = pluginTypesService.GetPluginTypesWithAttribute<BreadcrumbAttribute>();

            // Cycle through all methods which have the breadcrumb attribute
            foreach (Type type in breadcrumbAttributes)
            {
                // look for specific method breadcrumbs
                foreach (MethodInfo method in type.GetMethods())
                {
					BreadcrumbAttribute attribute = (BreadcrumbAttribute)method.GetCustomAttributes(true)
						.FirstOrDefault(a => a.GetType() == typeof(BreadcrumbAttribute));

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        attribute.HasParams = method.GetParameters().Length > 0 ||
                            method.ContainsGenericParameters ||
                            attribute.HasParams;

                        // sanity check
                        if (route.Equals(attribute.ParentRoute, StringComparison.CurrentCultureIgnoreCase))
                        {
                            _logger.AddToLog(LogLevel.Error, nameof(BreadcrumbMiddleware),
                                String.Format(Constants.BreadcrumbRoutEqualsParentRoute, route, attribute.ParentRoute));
                        }
                        else
                        {
                            allBreadcrumbs.Add(route, attribute);
                        }
                    }
                }

                // now we have all the routes with parents, build up a heirarchy for each route
                foreach (KeyValuePair<string, BreadcrumbAttribute> keyValuePair in allBreadcrumbs)
                {
                    BreadcrumbRoute route = new BreadcrumbRoute(keyValuePair.Key, keyValuePair.Value.HasParams);
                    route.Breadcrumbs.Add(new BreadcrumbItem(keyValuePair.Value.Name, keyValuePair.Key, keyValuePair.Value.HasParams));

                    BreadcrumbAttribute breadcrumbItem = keyValuePair.Value;

                    byte loopCounter = 0;

                    do
                    {
                        breadcrumbItem = GetParentRoute(ref allBreadcrumbs,
                            breadcrumbItem.ParentRoute, out string parentRoute);

                        if (breadcrumbItem != null)
                        {
                            route.Breadcrumbs.Insert(0, new BreadcrumbItem(breadcrumbItem.Name, parentRoute, breadcrumbItem.HasParams));
                        }

                        loopCounter++;

                        if (loopCounter > 40)
                        {
                            _logger.AddToLog(LogLevel.Error, nameof(BreadcrumbMiddleware), Constants.TooManyBreadcrumbs);
                            break;
                        }

                    } while (breadcrumbItem != null);

                    if (_breadcrumbRoutes.ContainsKey($"{route.Route.ToLower()}"))
                        continue;

                    _breadcrumbRoutes.Add($"{route.Route.ToLower()}", route);
                    bool isDefaultAction = route.Route.EndsWith($"/{settings.DefaultAction}", StringComparison.InvariantCultureIgnoreCase);
                    bool isDefaultController = type.FullName.EndsWith($".{settings.HomeController}Controller",
                        StringComparison.InvariantCultureIgnoreCase);

                    // insert root as Home if it is not the root node
                    if ((!isDefaultController && isDefaultAction) ||
                        (isDefaultController && !isDefaultAction) ||
                        (!isDefaultController && !isDefaultAction))
                    {
                        route.Breadcrumbs.Insert(0, _homeBreadCrumb);
                    }

                    // is it the default method for controller
                    if (isDefaultAction)
                    {
                        string routeDesc = route.Route.Substring(0, route.Route.Length - 6);

                        if (String.IsNullOrEmpty(routeDesc))
                            routeDesc = Constants.ForwardSlash;

                        AddDefaultRoute(routeDesc, route.Breadcrumbs, route.HasParameters);
                    }

                    // is it the default controller
                    if (isDefaultController)
                    {
                        AddDefaultRoute($"{Constants.ForwardSlash}{settings.HomeController}{route.Route}", route.Breadcrumbs,
                            route.HasParameters);

                        if (isDefaultAction)
                            AddDefaultRoute($"{Constants.ForwardSlash}{settings.HomeController}", route.Breadcrumbs,
                                route.HasParameters);
                    }
                }
            }
        }

        private static void AddDefaultRoute(in string routeDescription, in List<BreadcrumbItem> breadcrumbs,
            in bool hasParameters)
        {
            BreadcrumbRoute defaultRoute = new BreadcrumbRoute(routeDescription, hasParameters);

            foreach (BreadcrumbItem item in breadcrumbs)
            {
                defaultRoute.Breadcrumbs.Add(new BreadcrumbItem(item.Name,
                    $"{item.Route}", item.HasParameters));
            }

            _breadcrumbRoutes.Add(defaultRoute.Route.ToLower(), defaultRoute);
        }

        private static BreadcrumbAttribute GetParentRoute(ref Dictionary<string, BreadcrumbAttribute> allBreadcrumbs,
            in string route, out string parentRoute)
        {
            if (!String.IsNullOrEmpty(route))
            {
                foreach (KeyValuePair<string, BreadcrumbAttribute> breadcrumb in allBreadcrumbs)
                {
                    if (breadcrumb.Key == route)
                    {
                        parentRoute = breadcrumb.Key;
                        return breadcrumb.Value;
                    }
                }
            }

            parentRoute = String.Empty;

            return null;
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591