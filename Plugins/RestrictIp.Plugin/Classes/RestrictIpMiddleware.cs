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
 *  Product:  RestrictIp.Plugin
 *  
 *  File: RestrictIpMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  16/10/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using PluginManager;
using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591, IDE0056, IDE0057

namespace RestrictIp.Plugin
{
	/// <summary>
	/// Middleware provider which processes requests and ensures that only authorised 
	/// connections can connect to routes decorated with RestrictedIpRouteAttribute.
	/// </summary>
	public sealed class RestrictIpMiddleware : BaseMiddleware
	{

		#region Constants

		private const string ProfileNotFound = "The requested restriction profile '{0}' was not found!  " +
			"Route '{1}' will be restricted to localhost only";
		private const string RouteRestricted = "Profile '{0}' is restricting route '{1}' to Ip Address {2}";
		private const string RouteForbidden = "Ip Address '{0}' forbidden to access route '{1}'";
		private const string LocalHost = "localhost";
		private const string RestrictedIpDisabled = "RestrictIp Middleware is disabled";

		#endregion Constants

		#region Private Members

		private readonly Dictionary<string, List<string>> _restrictedRoutes;
		private readonly RequestDelegate _next;
		private readonly HashSet<string> _localIpAddresses;
		private readonly bool _disabled;
		private readonly ILogger _logger;
		internal readonly static Timings _timings = new();

		#endregion Private Members

		#region Constructors

		public RestrictIpMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider,
			IRouteDataService routeDataService, IPluginHelperService pluginHelperService,
			IPluginTypesService pluginTypesService, ISettingsProvider settingsProvider, ILogger logger)
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

			_next = next;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_localIpAddresses = [];
			_restrictedRoutes = [];
			GetLocalIpAddresses(_localIpAddresses);
			RestrictIpSettings settings = settingsProvider.GetSettings<RestrictIpSettings>("RestrictedIpRoutes.Plugin");
			_disabled = settings.Disabled;

			if (_disabled)
				_logger.AddToLog(LogLevel.Information, RestrictedIpDisabled);

			LoadRestrictedIpRouteData(routeProvider, routeDataService, pluginTypesService, settings);
		}

		#endregion Constructors

		#region Public Methods

		public async Task Invoke(HttpContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			bool passRequestOn = true;

			try
			{
				if (_disabled)
					return;

				using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
				{
					string route = RouteLowered(context);

					string userIpAddress = GetIpAddress(context);
					bool isLocalAddress = _localIpAddresses.Contains(userIpAddress);

#if !DEBUG
                    // always allow local connections
                    if (isLocalAddress)
                        return;
#endif

					foreach (KeyValuePair<string, List<string>> restrictedRoute in _restrictedRoutes)
					{
						if (route.StartsWith(restrictedRoute.Key))
						{
							foreach (string restrictedIp in restrictedRoute.Value)
							{
								if (String.IsNullOrEmpty(restrictedIp) || (isLocalAddress && (restrictedIp == LocalHost)))
									return;

								if (userIpAddress.StartsWith(restrictedIp))
									return;
							}

							// if we get here, we are in a restricted route and ip does not match, so fail with forbidden
							passRequestOn = false;
							context.Response.StatusCode = 403;
							_logger.AddToLog(LogLevel.IpRestricted, String.Format(RouteForbidden, userIpAddress, route));
						}
					}
				}
			}
			finally
			{
				if (passRequestOn)
					await _next(context);
			}
		}

		#endregion Public Methods

		#region Private Methods

		private void LoadRestrictedIpRouteData(in IActionDescriptorCollectionProvider routeProvider,
			in IRouteDataService routeDataService, in IPluginTypesService pluginTypesService,
			in RestrictIpSettings settings)
		{
			List<Type> classesWithIpAttributes = pluginTypesService.GetPluginTypesWithAttribute<RestrictedIpRouteAttribute>();

			// Cycle through all classes and methods which have the restricted route attribute
			foreach (Type type in classesWithIpAttributes)
			{
				foreach (Attribute attribute in type.GetCustomAttributes())
				{
					if (attribute is RestrictedIpRouteAttribute restrictedIpRouteAttribute)
					{
						string route = routeDataService.GetRouteFromClass(type, routeProvider);

						if (String.IsNullOrEmpty(route))
							continue;

						// if the route ends with / remove it
						if (route[route.Length - 1] == '/')
							route = route.Substring(0, route.Length - 1);

						if (!_restrictedRoutes.ContainsKey(route.ToLower()))
							_restrictedRoutes.Add(route.ToLower(), []);

						if (settings.RouteRestrictions.ContainsKey(restrictedIpRouteAttribute.ProfileName))
						{
							foreach (string ipAddress in settings.RouteRestrictions[restrictedIpRouteAttribute.ProfileName].Split(';'))
							{
								if (String.IsNullOrEmpty(ipAddress))
									continue;

								_restrictedRoutes[route.ToLower()].Add(ipAddress.Replace("*", String.Empty));

								_logger.AddToLog(LogLevel.Information,
									String.Format(RouteRestricted, restrictedIpRouteAttribute.ProfileName,
										route, ipAddress));
							}
						}
						else
						{
							_logger.AddToLog(LogLevel.Warning,
								String.Format(ProfileNotFound, restrictedIpRouteAttribute.ProfileName, route));
							_restrictedRoutes[route.ToLower()].Add(LocalHost);
						}
					}
				}
			}
		}

		#endregion Private Methods
	}

}

#pragma warning restore CS1591, IDE0056, IDE0057
