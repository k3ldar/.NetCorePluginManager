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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

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
    public sealed class WebSmokeTestMiddleware : BaseMiddleware, IDisposable
    {
        #region Private Members

        private static readonly CacheManager _testCache = new CacheManager("Web Smoke Test Cache", new TimeSpan(0, 10, 0), true);
        private readonly string _savedData = Path.GetTempFileName();
        private readonly RequestDelegate _next;
        private readonly string _staticFileExtensions = Constants.StaticFileExtensions;
        internal static Timings _timings = new Timings();
        private Boolean disposedValue;
        private readonly ILogger _logger;
        private readonly WebSmokeTestSettings _settings;
        private static FileStream _testDataStream;

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

            _testDataStream = new FileStream(_savedData, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            _next = next;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _settings = settingsProvider.GetSettings<WebSmokeTestSettings>(nameof(WebSmokeTest));

            if (_settings.Enabled)
            {
                LoadSmokeTestData(pluginTypesService);
            }

            if (!String.IsNullOrEmpty(_settings.StaticFileExtensions))
                _staticFileExtensions = _settings.StaticFileExtensions;
        }

        ~WebSmokeTestMiddleware()
        {
            Dispose(false);
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
                            byte[] siteId = Encoding.UTF8.GetBytes(_settings.SiteId);
                            await context.Response.Body.WriteAsync(siteId, 0, siteId.Length);
                        }
                        else if (route.Equals("/smoketest/count/"))
                        {
                            byte[] siteId = Encoding.UTF8.GetBytes(SmokeTests.Count.ToString());
                            await context.Response.Body.WriteAsync(siteId, 0, siteId.Length);
                        }
                        else if (route.StartsWith("/smoketest/test"))
                        {
                            string testNumber = route.Substring(16);
                            List<WebSmokeTestItem> testItems = SmokeTests;

                            if (Int32.TryParse(testNumber, out int number) &&
                                number >= 0 &&
                                number < testItems.Count)
                            {
                                context.Response.ContentType = "application/json";
                                byte[] testData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(testItems[number]));
                                await context.Response.Body.WriteAsync(testData, 0, testData.Length);
                            }
                            else
                            {
                                context.Response.StatusCode = 400;
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
            Dispose(disposing: true);
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

                    List<WebSmokeTestItem> cacheData = JsonConvert.DeserializeObject<List<WebSmokeTestItem>>(Encoding.UTF8.GetString(bytes));
                    smokeTests = new CacheItem(nameof(SmokeTests), cacheData);
                    _testCache.Add(nameof(SmokeTests), smokeTests, true);
                }

                return (List<WebSmokeTestItem>)smokeTests.Value;
            }

            private set
            {
                byte[] fileData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value));
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

        internal void ClearCache()
        {
            _testCache.Clear();
        }

        #endregion Internal Methods

        #region Private Methods

        private void LoadSmokeTestData(in IPluginTypesService pluginTypesService)
        {
            List<WebSmokeTestItem> allSmokeTests = new List<WebSmokeTestItem>();
            List<Type> testAttributes = pluginTypesService.GetPluginTypesWithAttribute<SmokeTestAttribute>();

            // Cycle through all methods which have the SmokeTestAttribute attribute
            foreach (Type type in testAttributes)
            {
                // look for specific method smoke test
                foreach (MethodInfo method in type.GetMethods())
                {
                    List<object> attributes = method.GetCustomAttributes(true)
                        .Where(a => a.GetType() == typeof(SmokeTestAttribute)).ToList();

                    foreach (object attr in attributes)
                    {
                        SmokeTestAttribute attribute = attr as SmokeTestAttribute;

                        if (attribute != null)
                        {
                            WebSmokeTestItem smokeTestItem = GetSmokeTestFromAttribute(type, method, attribute);

                            if (smokeTestItem != null)
                                allSmokeTests.Add(smokeTestItem);
                        }
                    }
                }
            }

            SmokeTests = allSmokeTests;
        }

        private WebSmokeTestItem GetSmokeTestFromAttribute(Type type, MethodInfo method, SmokeTestAttribute attribute)
        {

            if (type.IsSubclassOf(typeof(Microsoft.AspNetCore.Mvc.Controller)))
            {
                return GetSmokeTestFromControllerAction(type, method, attribute);
            }

            return GetSmokeTestFromStandardClassMethod(type, method);
        }

        private WebSmokeTestItem GetSmokeTestFromStandardClassMethod(Type type, MethodInfo method)
        {
            if (method.ReturnType == typeof(WebSmokeTestItem) && method.GetParameters().Length == 0)
            {
                try
                {
                    ConstructorInfo constructorInfo = type.GetConstructor(Array.Empty<Type>());

                    if (constructorInfo != null)
                    {
                        object inst = constructorInfo.Invoke(Array.Empty<object>());

                        if (inst != null)
                        {
                            return (WebSmokeTestItem)method.Invoke(inst, Array.Empty<object>());
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

        private WebSmokeTestItem GetSmokeTestFromControllerAction(Type type, MethodInfo method, SmokeTestAttribute attribute)
        {
            string name = attribute.Name;
            string route = $"{type.Name.Substring(0, type.Name.Length - 10)}/{method.Name}/";

            if (String.IsNullOrEmpty(attribute.Name))
                name = route;

            string httpMethod = GetHttpMethodFromMethodInfo(method.CustomAttributes);

            return new WebSmokeTestItem(route,
                httpMethod,
                attribute.Response,
                attribute.Position,
                name,
                attribute.InputData,
                attribute.SearchData.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
        }

        private string GetHttpMethodFromMethodInfo(IEnumerable<CustomAttributeData> attributes)
        {
            if (attributes.Where(a => a.AttributeType.Name.Equals("HttpGetAttribute")).Any())
                return "GET";

            if (attributes.Where(a => a.AttributeType.Name.Equals("HttpPostAttribute")).Any())
                return "POST";

            if (attributes.Where(a => a.AttributeType.Name.Equals("HttpPutAttribute")).Any())
                return "PUT";

            if (attributes.Where(a => a.AttributeType.Name.Equals("HttpHeadAttribute")).Any())
                return "HEAD";

            if (attributes.Where(a => a.AttributeType.Name.Equals("HttpDeleteAttribute")).Any())
                return "DELETE";

            if (attributes.Where(a => a.AttributeType.Name.Equals("HttpPatchAttribute")).Any())
                return "PATCH";

            if (attributes.Where(a => a.AttributeType.Name.Equals("HttpOptionsAttribute")).Any())
                return "OPTIONS";

            return "GET";
        }

        private void Dispose(Boolean disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                _testDataStream.Dispose();
                File.Delete(_savedData);
                disposedValue = true;
            }
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591