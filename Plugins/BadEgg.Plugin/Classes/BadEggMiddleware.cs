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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  BadEgg.Plugin
 *  
 *  File: BadEggMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  08/11/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using BadEgg.Plugin.WebDefender;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static SharedPluginFeatures.Enums;

#pragma warning disable CS1591

namespace BadEgg.Plugin
{
    /// <summary>
    /// BadEgg Middleware
    /// </summary>
    public sealed class BadEggMiddleware : BaseMiddleware
    {
        #region Private Members

        private readonly List<ManagedRoute> _managedRoutes;
        private readonly bool _userSessionManagerLoaded;
        private readonly RequestDelegate _next;
        private readonly ValidateConnections _validateConnections;
        private readonly IIpValidation _ipValidation;
        private readonly string _staticFileExtensions = Constants.StaticFileExtensions;
        private readonly BadEggSettings _badEggSettings;
        private readonly INotificationService _notificationService;
        internal static Timings _timings = new Timings();

        #endregion Private Members

        #region Constructors

        public BadEggMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginHelperService pluginHelperService,
            IPluginTypesService pluginTypesService, IIpValidation ipValidation,
            ISettingsProvider settingsProvider, INotificationService notificationService)
        {
            if (routeProvider == null)
                throw new ArgumentNullException(nameof(routeProvider));

            if (routeDataService == null)
                throw new ArgumentNullException(nameof(routeDataService));

            if (pluginHelperService == null)
                throw new ArgumentNullException(nameof(pluginHelperService));

            if (pluginTypesService == null)
                throw new ArgumentNullException(nameof(pluginTypesService));

            if (settingsProvider == null)
                throw new ArgumentNullException(nameof(settingsProvider));

            _next = next ?? throw new ArgumentNullException(nameof(next));

            _userSessionManagerLoaded = pluginHelperService.PluginLoaded(Constants.PluginNameUserSession, out int version);
            _ipValidation = ipValidation ?? throw new ArgumentNullException(nameof(ipValidation));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

            _managedRoutes = new List<ManagedRoute>();
            LoadRouteData(routeProvider, routeDataService, pluginTypesService);

            _badEggSettings = settingsProvider.GetSettings<BadEggSettings>(Constants.BadEggSettingsName);

            _validateConnections = new ValidateConnections(
                _badEggSettings.ConnectionTimeOut,
                _badEggSettings.ConnectionsPerMinute);
            _validateConnections.ConnectionAdd += ValidateConnections_ConnectionAdd;
            _validateConnections.ConnectionRemove += ValidateConnections_ConnectionRemove;
            _validateConnections.OnReportConnection += ValidateConnections_OnReportConnection;
            _validateConnections.OnBanIPAddress += ValidateConnections_OnBanIPAddress;

            ThreadManager.ThreadStart(_validateConnections, Constants.BadEggValidationThread, ThreadPriority.Lowest);
        }

        #endregion Constructors

        #region Public Methods

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            string fileExtension = RouteFileExtension(context);

            if (!String.IsNullOrEmpty(fileExtension) && _staticFileExtensions.Contains($"{fileExtension};"))
            {
                await _next(context);
                return;
            }

            if (context.Request.Headers.ContainsKey(Constants.BadEggValidationIgnoreHeaderName) &&
                context.Request.Headers[Constants.BadEggValidationIgnoreHeaderName].Equals(_badEggSettings.IgnoreValidationHeaderCode))
            {
                context.Response.Headers.Add(Constants.BadEggValidationIgnoreHeaderName, Boolean.TrueString);
                await _next(context);
                return;
            }


            string route = RouteLowered(context);

            using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
            {
                bool validateFormInput = false;

                foreach (ManagedRoute restrictedRoute in _managedRoutes)
                {
                    if (restrictedRoute.ValidateFormFields && restrictedRoute.Route.StartsWith(route))
                    {
                        validateFormInput = true;
                        break;
                    }
                }

                ValidateRequestResult validateResult = _validateConnections.ValidateRequest(context.Request, validateFormInput, out int _);

                if (!validateResult.HasFlag(ValidateRequestResult.IpWhiteListed))
                {
                    if (validateResult.HasFlag(ValidateRequestResult.IpBlackListed))
                    {
                        context.Response.StatusCode = _badEggSettings.BannedResponseCode;
                        return;
                    }
                    else if (validateResult.HasFlag(ValidateRequestResult.TooManyRequests))
                    {
                        _notificationService.RaiseEvent(nameof(ValidateRequestResult.TooManyRequests), GetIpAddress(context));
                        context.Response.StatusCode = _badEggSettings.TooManyRequestResponseCode;
                        return;
                    }
                }
            }

            await _next(context);
        }

        #endregion Public Methods

        #region Private Methods

        private void LoadRouteData(IActionDescriptorCollectionProvider routeProvider,
            IRouteDataService routeDataService, IPluginTypesService pluginTypesService)
        {
            List<Type> badeggAttributes = pluginTypesService.GetPluginTypesWithAttribute<BadEggAttribute>();

            if (badeggAttributes.Count > 0)
            {
                // Cycle through all classes and methods which have the bad egg attribute
                foreach (Type type in badeggAttributes)
                {
                    // is it a class attribute
                    BadEggAttribute attribute = (BadEggAttribute)type.GetCustomAttributes(true)
                        .Where(a => a.GetType() == typeof(BadEggAttribute)).FirstOrDefault();

                    if (attribute != null)
                    {
                        string route = routeDataService.GetRouteFromClass(type, routeProvider);

                        if (String.IsNullOrEmpty(route))
                            continue;

                        _managedRoutes.Add(new ManagedRoute($"{route.ToLower()}", attribute.ValidateQueryFields, attribute.ValidateFormFields));
                    }

                    // look for specific method disallows

                    foreach (MethodInfo method in type.GetMethods())
                    {
                        attribute = (BadEggAttribute)method.GetCustomAttributes(true)
                            .Where(a => a.GetType() == typeof(BadEggAttribute)).FirstOrDefault();

                        if (attribute != null)
                        {
                            string route = routeDataService.GetRouteFromMethod(method, routeProvider);

                            if (String.IsNullOrEmpty(route))
                                continue;


                            _managedRoutes.Add(new ManagedRoute($"{route.ToLower()}", attribute.ValidateQueryFields, attribute.ValidateFormFields));
                        }
                    }
                }
            }
        }

        #region IIpValidation Methods

        private void ValidateConnections_OnBanIPAddress(object sender, RequestBanEventArgs e)
        {
            e.AddToBlackList = _ipValidation.ConnectionBan(e.IPAddress, e.Hits, e.Requests, e.Duration);
        }

        private void ValidateConnections_OnReportConnection(object sender, ConnectionReportEventArgs e)
        {
            _ipValidation.ConnectionReport(e.IPAddress, e.QueryString, e.Result);
        }

        private void ValidateConnections_ConnectionRemove(object sender, ConnectionRemoveEventArgs e)
        {
            _ipValidation.ConnectionRemove(e.IPAddress, e.Hits, e.Requests, e.Duration);
        }

        private void ValidateConnections_ConnectionAdd(object sender, ConnectionEventArgs e)
        {
            _ipValidation.ConnectionAdd(e.IPAddress);
        }

        #endregion IIpValidation Methods

        #endregion Private Methods
    }
}

#pragma warning restore CS1591