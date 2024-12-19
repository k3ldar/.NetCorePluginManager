/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Middleware;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static Shared.Utilities;

using Constants = SharedPluginFeatures.Constants;

#pragma warning disable CS1591

namespace WebSmokeTest.Plugin
{
	/// <summary>
	/// WebSmokeTest middleware class, this module extends BaseMiddlware and is injected 
	/// into the request pipeline.
	/// </summary>
	public sealed class WebSmokeTestMiddleware : BaseMiddleware, IDisposable
	{
		#region Private Members

		private static readonly CacheManager _testCache = new("Web Smoke Test Cache", new TimeSpan(0, 10, 0), true);
		private readonly string _savedData = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.tmp");
		private readonly RequestDelegate _next;
		internal readonly static Timings _timings = new();
		private Boolean _disposedValue;
		private readonly ILogger _logger;
		private readonly WebSmokeTestSettings _settings;
		private readonly FileStream _testDataStream;

		#endregion Private Members

		#region Constructors/Destructors

		public WebSmokeTestMiddleware(RequestDelegate next,
			IPluginHelperService pluginHelperService,
			IPluginTypesService pluginTypesService,
			ISettingsProvider settingsProvider,
			ILogger logger)
		{
			if (pluginHelperService == null)
				throw new ArgumentNullException(nameof(pluginHelperService));

			if (pluginTypesService == null)
				throw new ArgumentNullException(nameof(pluginTypesService));

			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			File.WriteAllText(_savedData, String.Empty);

			_testDataStream = new FileStream(_savedData, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

			_next = next;
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_settings = settingsProvider.GetSettings<WebSmokeTestSettings>(nameof(WebSmokeTest));

			if (_settings.Enabled)
			{
				LoadSmokeTestData(pluginTypesService);
			}
		}

		#endregion Constructors/Destructors

		#region Public Methods

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "it's ok here, nothing to see, move along")]
		public async Task Invoke(HttpContext context)
		{
			if (context == null)
				throw new ArgumentNullException(nameof(context));

			if (_settings.Enabled)
			{
				string route = RouteLowered(context);

				if (route.StartsWith("/smoketest/"))
				{
					using (StopWatchTimer stopwatchTimer = StopWatchTimer.Initialise(_timings))
					{
						if (route.Equals("/smoketest/siteid/"))
						{
							byte[] siteId = Encoding.UTF8.GetBytes(
								Encrypt(String.Join(';', _settings.SiteId), _settings.EncryptionKey));
							await context.Response.Body.WriteAsync(siteId, 0, siteId.Length);
						}
						else if (route.Equals("/smoketest/start/"))
						{
							if (context.RequestServices
								.GetService(typeof(ISmokeTestProvider)) is ISmokeTestProvider smokeTestProvider)
							{
								NVPCodec codec = smokeTestProvider.SmokeTestStart();

								if (codec != null)
								{
									byte[] testNVPData = Encoding.UTF8.GetBytes(
										Encrypt(codec.Encode(), _settings.EncryptionKey));
									await context.Response.Body.WriteAsync(testNVPData, 0, testNVPData.Length);
								}
							}
						}
						else if (route.Equals("/smoketest/count/"))
						{
							byte[] siteId = Encoding.UTF8.GetBytes(
								Encrypt(SmokeTests.Count.ToString(), _settings.EncryptionKey));
							await context.Response.Body.WriteAsync(siteId, 0, siteId.Length);
						}
						else if (route.StartsWith("/smoketest/test"))
						{
							string testNumber = route[16..];
							List<WebSmokeTestItem> testItems = SmokeTests;

							if (Int32.TryParse(testNumber, out int number) &&
								number >= 0 &&
								number < testItems.Count)
							{
								context.Response.ContentType = Constants.ContentTypeApplicationJson;
								byte[] testData = Encoding.UTF8.GetBytes(
									Encrypt(JsonSerializer.Serialize(testItems[number]), _settings.EncryptionKey));
								await context.Response.Body.WriteAsync(testData, 0, testData.Length);
							}
							else
							{
								context.Response.StatusCode = Constants.HtmlResponseBadRequest;
							}
						}
						else if (route.Equals("/smoketest/end/"))
						{
							if (context.RequestServices
								.GetService(typeof(ISmokeTestProvider)) is ISmokeTestProvider smokeTestProvider)
							{
								smokeTestProvider.SmokeTestEnd();
							}
						}
						else
						{
							context.Response.StatusCode = 404;
						}

						return;
					}
				}
			}

			await _next(context);
		}

		public void Dispose()
		{
			if (!_disposedValue)
			{
				_testDataStream.Dispose();
				File.Delete(_savedData);
				_disposedValue = true;
			}

			GC.SuppressFinalize(this);
		}

		#endregion Public Methods

		#region Properties

		internal List<WebSmokeTestItem> SmokeTests
		{
			get
			{
				CacheItem smokeTests = _testCache.Get(nameof(SmokeTests));

				if (smokeTests == null)
				{
					_testDataStream.Position = 0;
					byte[] bytes = new byte[_testDataStream.Length];
					int numBytesToRead = (int)_testDataStream.Length;
					int numBytesRead = 0;
					while (numBytesToRead > 0)
					{
						int n = _testDataStream.Read(bytes, numBytesRead, numBytesToRead);

						if (n == 0)
							break;

						numBytesRead += n;
						numBytesToRead -= n;
					}

					List<WebSmokeTestItem> cacheData = JsonSerializer.Deserialize<List<WebSmokeTestItem>>(Encoding.UTF8.GetString(bytes));
					smokeTests = new CacheItem(nameof(SmokeTests), cacheData);
					_testCache.Add(nameof(SmokeTests), smokeTests, true);
				}

				return (List<WebSmokeTestItem>)smokeTests.Value;
			}

			private set
			{
				byte[] fileData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
				_testDataStream.Write(fileData, 0, fileData.Length);
				_testDataStream.Flush();

				_testCache.Add(nameof(SmokeTests), new CacheItem(nameof(SmokeTests), value));
			}
		}

		internal bool Enabled
		{
			get
			{
				return _settings.Enabled;
			}

			set
			{
				_settings.Enabled = value;
			}
		}

		#endregion Properties

		#region Internal Methods

		internal static void ClearCache()
		{
			_testCache.Clear();
		}

		#endregion Internal Methods

		#region Private Methods

		private void LoadSmokeTestData(in IPluginTypesService pluginTypesService)
		{
			List<WebSmokeTestItem> allSmokeTests = [];
			List<Type> testAttributes = pluginTypesService.GetPluginTypesWithAttribute<SmokeTestAttribute>();
			int testCount = 0;

			// Cycle through all methods which have the SmokeTestAttribute attribute
			foreach (Type type in testAttributes)
			{
				// look for specific method smoke test
				foreach (MethodInfo method in type.GetMethods())
				{
					List<object> attributes = method.GetCustomAttributes(true)
						.Where(a => a is SmokeTestAttribute).ToList();

					foreach (object attr in attributes)
					{
						if (attr is SmokeTestAttribute attribute)
						{
							WebSmokeTestItem smokeTestItem = GetSmokeTestFromAttribute(type, method, attribute);

							if (smokeTestItem != null)
							{
								smokeTestItem.Index = testCount++;
								allSmokeTests.Add(smokeTestItem);
							}
						}
					}
				}
			}

			SmokeTests = allSmokeTests;
		}

		private WebSmokeTestItem GetSmokeTestFromAttribute(in Type type, in MethodInfo method, in SmokeTestAttribute attribute)
		{
			WebSmokeTestItem Result;

			if (type.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.Controller)))
			{
				Result = GetSmokeTestFromControllerAction(type, method, attribute);
			}
			else
			{
				Result = GetSmokeTestFromStandardClassMethod(type, method);
			}

			Result?.AuthorHistory.Add(DateTime.UtcNow, "Managed Test");

			return Result;
		}

		private WebSmokeTestItem GetSmokeTestFromStandardClassMethod(in Type type, in MethodInfo method)
		{
			if (method.ReturnType == typeof(WebSmokeTestItem) && method.GetParameters().Length == 0)
			{
				try
				{
					ConstructorInfo constructorInfo = type.GetConstructor([]);

					if (constructorInfo != null)
					{
						object inst = constructorInfo.Invoke([]);

						if (inst != null)
						{
							return (WebSmokeTestItem)method.Invoke(inst, []);
						}
					}
				}
				catch (Exception err)
				{
					_logger.AddToLog(PluginManager.LogLevel.Error, err, $"Failed to retrieve WebSmokeTestItem from {method.Name}");
					throw;
				}
			}

			return null;
		}

		private static WebSmokeTestItem GetSmokeTestFromControllerAction(in Type type, in MethodInfo method, in SmokeTestAttribute attribute)
		{
			string name = attribute.Name;
			StringBuilder route = new($"{type.Name[..^10]}/{method.Name}/");

			if (String.IsNullOrEmpty(attribute.Name))
				name = $"{route}";

			string httpMethod = GetHttpMethodFromMethodInfo(method.CustomAttributes);
			bool hasQuestion = false;

			foreach (ParameterInfo param in method.GetParameters())
			{
				if (!hasQuestion)
				{
					hasQuestion = true;
					route.Append($"?{param.Name}={{{param.Name}}}");
				}
				else
				{
					route.Append($"&{param.Name}={{{param.Name}}}");
				}
			}

			return new WebSmokeTestItem(route.ToString(),
				httpMethod,
				attribute.FormId,
				attribute.Response,
				attribute.PostType,
				attribute.Position,
				name,
				attribute.InputData,
				attribute.Parameters,
				attribute.RedirectUrl,
				[.. attribute.SearchData.Split(';', StringSplitOptions.RemoveEmptyEntries)],
				[.. attribute.SubmitSearchData.Split(';', StringSplitOptions.RemoveEmptyEntries)]);
		}

		private static string GetHttpMethodFromMethodInfo(in IEnumerable<CustomAttributeData> attributes)
		{
			if (attributes.Any(a => a.AttributeType.Name.Equals("HttpGetAttribute")))
				return "GET";

			if (attributes.Any(a => a.AttributeType.Name.Equals("HttpPostAttribute")))
				return "POST";

			if (attributes.Any(a => a.AttributeType.Name.Equals("HttpPutAttribute")))
				return "PUT";

			if (attributes.Any(a => a.AttributeType.Name.Equals("HttpHeadAttribute")))
				return "HEAD";

			if (attributes.Any(a => a.AttributeType.Name.Equals("HttpDeleteAttribute")))
				return "DELETE";

			if (attributes.Any(a => a.AttributeType.Name.Equals("HttpPatchAttribute")))
				return "PATCH";

			if (attributes.Any(a => a.AttributeType.Name.Equals("HttpOptionsAttribute")))
				return "OPTIONS";

			return "GET";
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591