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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Subdomain.Plugin
 *  
 *  File: SubdomainMiddleware.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  13/02/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

using PluginManager;
using PluginManager.Abstractions;

using SharedPluginFeatures;

#pragma warning disable CS1591, IDE0056, IDE0057

namespace Subdomain.Plugin
{
	/// <summary>
	/// Middleware provider which processes subdomain requests and ensures that any routes identified
	/// as subdomains are correctly routed
	/// </summary>
	public sealed class SubdomainMiddleware : BaseMiddleware
	{
		#region Constants

		private const string InvalidController = "Subdomain can only be used on classes descending from Controller, class {0} is invalid";
		private const string ConfigurationMissing = "Configuration for subdomain {0} is missing";
		private const string ConfigurationDisabled = "Configuration for subdomain {0} is disabled";
		private const string SubdomainMiddlewareDisabled = "Subdomain Middleware is disabled";
		private const string ResponseStartedUnableToRedirect = "Subdomain Middleware is unable to redirect the request because the response has started";
		private const string WwwRedirect = "www.";

		#endregion Constants

		#region Private Members

		private readonly Dictionary<string, SubdomainSetting> _subdomainMappings;
		private readonly List<string> _routesWithoutSubdomains;
		private readonly RequestDelegate _next;
		private readonly bool _enabled;
		private readonly bool _disableWwwRedirect;
		private readonly string _domainName;
		private readonly bool _processStaticFiles;
		private readonly string _staticFileExtensions = Constants.StaticFileExtensions;
		private readonly ILogger _logger;
		internal readonly static Timings _timings = new();

		#endregion Private Members

		#region Constructors

		public SubdomainMiddleware(RequestDelegate next, IActionDescriptorCollectionProvider routeProvider,
			IRouteDataService routeDataService, IPluginHelperService pluginHelperService,
			IPluginClassesService pluginClassesService, IPluginTypesService pluginTypesService,
			ISettingsProvider settingsProvider, ILogger logger)
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
			_subdomainMappings = [];
			_routesWithoutSubdomains = [];


			SubdomainSettings settings = settingsProvider.GetSettings<SubdomainSettings>(nameof(SubdomainSettings));
			_enabled = settings.Enabled;
			_disableWwwRedirect = settings.DisableRedirectWww;

			if (_enabled)
			{
				_domainName = settings.DomainName.ToLower();
				_processStaticFiles = settings.ProcessStaticFiles;

				if (!String.IsNullOrEmpty(settings.StaticFileExtensions))
					_staticFileExtensions = settings.StaticFileExtensions;

				LoadSubdomainRouteData(routeProvider, routeDataService,
					pluginTypesService, pluginClassesService, settings);
			}
			else
			{
				_logger.AddToLog(LogLevel.Information, SubdomainMiddlewareDisabled);
			}
		}

		#endregion Constructors

		#region Properties

		public IReadOnlyList<string> RoutesWithoutSubdomain => _routesWithoutSubdomains.AsReadOnly();

		public ReadOnlyDictionary<string, SubdomainSetting> RoutesWithSubdomain => new(_subdomainMappings);

		#endregion Properties

		#region Public Methods

		public async Task Invoke(HttpContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			bool passRequestOn = true;

			try
			{
				if (!_enabled)
					return;

				string fileExtension = RouteFileExtension(context);

				if (!_processStaticFiles && !String.IsNullOrEmpty(fileExtension) &&
					_staticFileExtensions.Contains(fileExtension))
				{
					return;
				}

				using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
				{
					if (context.Response.HasStarted)
					{
						_logger.AddToLog(LogLevel.Warning, ResponseStartedUnableToRedirect);
						return;
					}

					if (!context.Request.Host.HasValue)
					{
						return;
					}

					string route = RouteLowered(context);
					string strippedRoute = route;
					int seperatorIndex = strippedRoute.IndexOf('/', 1);

					if (seperatorIndex > -1)
						strippedRoute = strippedRoute.Substring(0, seperatorIndex);

					UriBuilder uriBuilder = null;

					if (context.Request.Host.Port.HasValue)
						uriBuilder = new UriBuilder(context.Request.Scheme, context.Request.Host.Host, context.Request.Host.Port.Value);
					else
						uriBuilder = new UriBuilder(context.Request.Scheme, context.Request.Host.Host);

					Uri uri = uriBuilder.Uri;
					bool isSubdomain = false;
					string subdomainName = String.Empty;

					if (uri.Host.Length > _domainName.Length)
					{
						subdomainName = uri.Host.Remove(uri.Host.Length - _domainName.Length - 1);
						isSubdomain = _subdomainMappings.ContainsKey($"/{subdomainName}");
					}

					if (isSubdomain && _routesWithoutSubdomains.Contains(strippedRoute))
					{
						// currently in a subdomain but routing to a path that should not be a subdomin
						string normalSubdomain = _disableWwwRedirect ? String.Empty : WwwRedirect;
						string port = context.Request.Host.Port.HasValue ? $":{context.Request.Host.Port.Value}" : String.Empty;
						string redirectUri = $"{uri.Scheme}://{normalSubdomain}{_domainName}{port}{Route(context)}";
						context.Response.Redirect(redirectUri, true);
						passRequestOn = false;
						return;
					}

					foreach (KeyValuePair<string, SubdomainSetting> subdomain in _subdomainMappings)
					{
						if (isSubdomain && subdomainName.Equals(subdomain.Value.RedirectedRoute))
						{
							if (!route.StartsWith(subdomain.Key))
							{
								context.Request.Path = subdomain.Key;
							}

							return;
						}

						if (route.StartsWith(subdomain.Key))
						{

							string port = context.Request.Host.Port.HasValue ? $":{context.Request.Host.Port.Value}" : String.Empty;
							string redirectUri = $"{context.Request.Scheme}://{subdomain.Value.RedirectedRoute}.{_domainName}{port}/";
							context.Response.Redirect(redirectUri, subdomain.Value.PermanentRedirect);
							passRequestOn = false;
							return;
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

		private void LoadSubdomainRouteData(in IActionDescriptorCollectionProvider routeProvider,
			in IRouteDataService routeDataService, in IPluginTypesService pluginTypesService,
			in IPluginClassesService pluginClassesService, in SubdomainSettings settings)
		{
			List<Type> classesWithSubdomainAttribute = pluginTypesService.GetPluginTypesWithAttribute<SubdomainAttribute>();
			List<Type> classesWithValidSubdomain = [];

			// Cycle through all classes which have the subdomain attribute
			foreach (Type classType in classesWithSubdomainAttribute)
			{
				if (!classType.IsSubclassOf(typeof(Controller)))
				{
					_logger.AddToLog(LogLevel.Warning, String.Format(InvalidController, classType.FullName));
					continue;
				}

				LoadSubdomainAttributeDate(classType, classesWithValidSubdomain, settings, routeProvider, routeDataService);
			}

			foreach (Type controllerType in pluginClassesService.GetPluginClassTypes<Controller>())
			{
				if (classesWithValidSubdomain.Contains(controllerType))
					continue;

				LoadRouteDataForControllersWithoutSubdomains(controllerType, routeDataService, routeProvider);
			}
		}

		private void LoadRouteDataForControllersWithoutSubdomains(Type controllerType,
			in IRouteDataService routeDataService,
			in IActionDescriptorCollectionProvider routeProvider)
		{
			string route = RouteFromRouteProvider(controllerType, routeProvider, routeDataService).ToLower();

			if (!String.IsNullOrEmpty(route) && !_routesWithoutSubdomains.Contains(route))
				_routesWithoutSubdomains.Add(route);
		}

		private void LoadSubdomainAttributeDate(Type classType, List<Type> classesWithValidSubdomain,
			in SubdomainSettings settings, in IActionDescriptorCollectionProvider routeProvider,
			in IRouteDataService routeDataService)
		{
			foreach (Attribute attribute in classType.GetCustomAttributes(false).Cast<Attribute>())
			{
				if (attribute is SubdomainAttribute subdomainAttribute)
				{
					if (!settings.Subdomains.ContainsKey(subdomainAttribute.ConfigurationName))
					{
						_logger.AddToLog(LogLevel.Warning, String.Format(ConfigurationMissing,
							subdomainAttribute.ConfigurationName));
						continue;
					}

					if (settings.Subdomains[subdomainAttribute.ConfigurationName].Disabled)
					{
						_logger.AddToLog(LogLevel.Warning, String.Format(ConfigurationDisabled,
							subdomainAttribute.ConfigurationName));
						continue;
					}

					string route = RouteFromRouteProvider(classType, routeProvider, routeDataService);

					if (String.IsNullOrEmpty(route) || route.Equals("/"))
						continue;

					if (!_subdomainMappings.ContainsKey(route.ToLower()))
					{
						classesWithValidSubdomain.Add(classType);
						_subdomainMappings.Add(route.ToLower(), settings.Subdomains[subdomainAttribute.ConfigurationName]);
					}
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static string RouteFromRouteProvider(in Type classType,
			in IActionDescriptorCollectionProvider routeProvider,
			in IRouteDataService routeDataService)
		{
			string route = routeDataService.GetRouteFromClass(classType, routeProvider);

			// if the route ends with / remove it
			if (route.Length > 1 && route[route.Length - 1] == '/')
				route = route.Substring(0, route.Length - 1);

			if (route.Length == 1 && route[0] == '/')
				route = String.Empty;

			return route;
		}

		#endregion Private Methods
	}

}

#pragma warning restore CS1591, IDE0056, IDE0057
