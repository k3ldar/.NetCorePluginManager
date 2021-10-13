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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: WebSmokeTestTests.cs
 *
 *  Purpose:  Test units for MVC WebSmokeTest Middleware class
 *
 *  Date        Name                Reason
 *  09/06/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.DemoWebsite.Classes;
using AspNetCore.PluginManager.Tests.MiddlewareTests;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.Abstractions;

using Shared.Abstractions;
using Shared.Classes;

using SharedPluginFeatures;

using WebSmokeTest.Plugin;

using static Shared.Utilities;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.SmokeTestTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class WebSmokeTestTests : BaseMiddlewareTests
    {
        private string EncryptionKey;

        [TestInitialize]
        public void InitialiseSmokeTestPluginManager()
        {
            ThreadManager.Initialise();
            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.ClearAllCaches();
            InitializeSmokeTestPluginManager();
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            WebSmokeTestSettings settings = settingsProvider.GetSettings<WebSmokeTestSettings>(nameof(WebSmokeTest));
            EncryptionKey = settings.EncryptionKey;
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void LoadSmokeTests()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            ILogger logger = new Logger();

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(null, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void LoadSmokeTests_ClearCache_LoadFromFile()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();
            ILogger logger = new Logger();


            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(null, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                WebSmokeTestMiddleware.ClearCache();
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_InvalidRequest_SiteId_Returns404()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/SiteId";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();


            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(null, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);

                Assert.AreEqual(404, httpResponse.StatusCode);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_ValidRequest_SiteId_Returns200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/SiteId/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);
                Assert.IsFalse(nextDelegateCalled);

                await sut.Invoke(httpContext);

                Assert.AreEqual(200, httpResponse.StatusCode);
                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string id = Decrypt(Encoding.UTF8.GetString(data), EncryptionKey);

                Assert.IsTrue(id.Contains("8D801A6912AF939"));
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_ValidRequest_SiteId_Disabled_Returns200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/SiteId/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                sut.Enabled = false;
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsTrue(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string id = Encoding.UTF8.GetString(data);

                Assert.AreEqual("", id);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_ValidRequest_Count_Disabled_Returns200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Count/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                sut.Enabled = false;
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsTrue(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string id = Encoding.UTF8.GetString(data);

                Assert.AreEqual("", id);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_ValidRequest_Count_Enabled_Returns200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Count/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);
                Assert.IsFalse(nextDelegateCalled);

                await sut.Invoke(httpContext);

                Assert.AreEqual(200, httpResponse.StatusCode);
                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string count = Decrypt(Encoding.UTF8.GetString(data), EncryptionKey);

                if (Int32.TryParse(count, out int actualCount))
                    Assert.IsTrue(actualCount > 1);
                else
                    throw new InvalidCastException("Failed to convert returned count");
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_ValidRequest_RetrieveTest_Disabled_Returns200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Test/0";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                sut.Enabled = false;
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsTrue(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string count = Encoding.UTF8.GetString(data);

                Assert.AreEqual("", count);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_ValidRequest_RetrieveTest_Enabled_Returns200()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Test/1";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsFalse(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);
                Assert.AreEqual("application/json", httpResponse.ContentType);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string test = Decrypt(Encoding.UTF8.GetString(data), EncryptionKey);

                Assert.IsTrue(test.Contains("Please try again"), $"test data failed: {test}");
                Assert.IsTrue(test.Contains("Method\":\"POST") || test.Contains("Method\":\"GET"));
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task RetrieveUniqueId_ValidRequest_RetrieveTest_Enabled_InvalidTestId_Returns400()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Test/100000";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsFalse(nextDelegateCalled);
                Assert.AreEqual(400, httpResponse.StatusCode);
                Assert.IsNull(httpResponse.ContentType);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string test = Decrypt(Encoding.UTF8.GetString(data), EncryptionKey);

                Assert.IsTrue(String.IsNullOrEmpty(test));
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Validate_TestStartCalled_ISmokeTestProviderNotRegistered_Returns_EmptyString()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Start/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse,
                _testPluginSmokeTest.GetServiceProvider(), null);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsFalse(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);
                Assert.IsNull(httpResponse.ContentType);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string test = Decrypt(Encoding.UTF8.GetString(data), EncryptionKey);

                Assert.IsTrue(String.IsNullOrEmpty(test));
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Validate_TestStartCalled_ISmokeTestProviderRegistered_Returns_NvpCodecValues()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Start/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            NVPCodec codecValues = new NVPCodec();
            codecValues.Add("username", "admin");
            MockSmokeTestProvider smokeTestProvider = new MockSmokeTestProvider(codecValues);
            serviceCollection.AddSingleton<ISmokeTestProvider>(smokeTestProvider);

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse,
                serviceCollection.BuildServiceProvider(), null);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsFalse(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);
                Assert.IsNull(httpResponse.ContentType);
                Assert.IsTrue(smokeTestProvider.StartCalled);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string test = Decrypt(Encoding.UTF8.GetString(data), EncryptionKey);

                Assert.IsFalse(String.IsNullOrEmpty(test));
                NVPCodec codec = new NVPCodec();
                codec.Decode(test);

                Assert.AreEqual(1, codec.AllKeys.Length);

                Assert.IsTrue(codec.AllKeys.Contains("username"));
                Assert.AreEqual("admin", codec["username"]);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Validate_TestStartCalled_ISmokeTestProviderRegistered_Returns_NullNvpCodecValues()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/Start/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            MockSmokeTestProvider smokeTestProvider = new MockSmokeTestProvider();
            serviceCollection.AddSingleton<ISmokeTestProvider>(smokeTestProvider);

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse,
                serviceCollection.BuildServiceProvider(), null);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsFalse(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);
                Assert.IsNull(httpResponse.ContentType);
                Assert.IsTrue(smokeTestProvider.StartCalled);

                byte[] data = new byte[httpResponse.Body.Length];
                httpResponse.Body.Position = 0;
                httpResponse.Body.Read(data, 0, data.Length);
                string test = Decrypt(Encoding.UTF8.GetString(data), EncryptionKey);

                Assert.IsTrue(String.IsNullOrEmpty(test));
                NVPCodec codec = new NVPCodec();
                codec.Decode(test);

                Assert.AreEqual(0, codec.AllKeys.Length);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Validate_TestEndCalled_ISmokeTestProviderNotRegistered_Void()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/smokeTest/end/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse,
                serviceCollection.BuildServiceProvider(), null);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsFalse(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);
                Assert.IsNull(httpResponse.ContentType);
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Validate_TestEndCalled_ISmokeTestProviderRegistered_Void()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            MockHttpRequest httpRequest = new MockHttpRequest();
            httpRequest.Path = "/SmokeTest/End/";
            MockHttpResponse httpResponse = new MockHttpResponse();

            IPluginHelperService pluginHelperServices = _testPluginSmokeTest.GetRequiredService<IPluginHelperService>();
            IPluginTypesService pluginTypesService = _testPluginSmokeTest.GetRequiredService<IPluginTypesService>();
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;
            NVPCodec codecValues = new NVPCodec();
            codecValues.Add("username", "admin");
            MockSmokeTestProvider smokeTestProvider = new MockSmokeTestProvider(codecValues);
            serviceCollection.AddSingleton<ISmokeTestProvider>(smokeTestProvider);

            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse,
                serviceCollection.BuildServiceProvider(), null);
            ILogger logger = new Logger();
            bool nextDelegateCalled = false;
            RequestDelegate requestDelegate = async (context) => { nextDelegateCalled = true; await Task.Delay(0); };

            using (WebSmokeTestMiddleware sut = new WebSmokeTestMiddleware(requestDelegate, pluginHelperServices,
                pluginTypesService, settingsProvider, logger))
            {
                List<WebSmokeTestItem> smokeTests = sut.SmokeTests;

                Assert.IsTrue(smokeTests.Count > 1);

                await sut.Invoke(httpContext);
                Assert.IsFalse(nextDelegateCalled);
                Assert.AreEqual(200, httpResponse.StatusCode);
                Assert.IsNull(httpResponse.ContentType);
                Assert.IsTrue(smokeTestProvider.EndCalled);
            }
        }

        [SmokeTest]
        public void LoadTestData_InvalidReturnType_Void()
        {

        }

        [SmokeTest]
        [TestCategory(TestCategoryMiddleware)]
        public string LoadTestData_InvalidReturnType_String()
        {
            return String.Empty;
        }

        [SmokeTest]
        public WebSmokeTestItem LoadTestData_ValidReturnType_WebSmokeTestItem()
        {
            return new WebSmokeTestItem(
                "api/Restricted/",
                "GET",
                "",
                200,
                10,
                "Api Restricted Returns Test",
                "",
                new List<string> { "Test" },
                new List<string>());
        }
    }
}
