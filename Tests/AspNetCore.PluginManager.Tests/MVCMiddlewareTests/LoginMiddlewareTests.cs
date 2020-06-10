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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MVCMiddlewareLoginTests.cs
 *
 *  Purpose:  Test units for MVC Login Middleware class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using System.Threading.Tasks;

using AspNetCore.PluginManager.DemoWebsite.Classes;

using LoginPlugin;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.MiddlewareTests
{
    [TestClass]
    public class LoginMiddlewareTests : TestBasePlugin
    {
        [TestInitialize]
        public void InitializeLoginTests()
        {
            InitializeLoginPluginManager();
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public async Task LoginNullContextValue()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(null, authenticationService);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullLoginProviderValue()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, null, settingsProvider,
                claimsProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullSettingsProviderValue()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, null,
                claimsProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public async void LoginNullAuthenticationValueOnInvoke()
        {
            TestRequestCookieCollection cookies = new TestRequestCookieCollection();
            cookies.AddCookie("RememberMe", "1");

            TestHttpRequest httpRequest = new TestHttpRequest(cookies);
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullClaimsProviderValue()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                null);
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullRequestDelegateValue()
        {
            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);

            LoginMiddleware login = new LoginMiddleware(null, loginProvider, settingsProvider,
                claimsProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(System.FormatException))]
        public async Task LoginFromCookieValueCookieValueNotEncrypted()
        {
            TestRequestCookieCollection cookies = new TestRequestCookieCollection();
            cookies.AddCookie("RememberMe", "1");

            TestHttpRequest httpRequest = new TestHttpRequest(cookies);
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);
        }

        [TestMethod]
        public async Task LoginFromCookieValueCookieInvalidLoginUserNotFound()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));
            TestRequestCookieCollection cookies = new TestRequestCookieCollection();
            cookies.AddCookie("RememberMe", Shared.Utilities.Encrypt("1", loginControllerSettings.EncryptionKey));

            TestHttpRequest httpRequest = new TestHttpRequest(cookies);
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            TestResponseCookies responseCookies = httpResponse.Cookies as TestResponseCookies;

            Assert.IsNotNull(responseCookies);
            Assert.IsTrue(responseCookies.Get("RememberMe").CookieOptions.Expires < DateTime.Now.AddMinutes(-1439));
        }

        [TestMethod]
        public async Task LoginFromCookieValueCookieValidLoginUserFound()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));
            TestRequestCookieCollection cookies = new TestRequestCookieCollection();
            cookies.AddCookie("RememberMe", Shared.Utilities.Encrypt("123", loginControllerSettings.EncryptionKey));

            TestHttpRequest httpRequest = new TestHttpRequest(cookies);
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            TestResponseCookies responseCookies = httpResponse.Cookies as TestResponseCookies;

            Assert.IsNotNull(responseCookies);
            Assert.IsTrue(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        public async Task LoginAutoLoginFromHeadersValidUsernameAndPassword()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("admin:password"));
            httpRequest.Headers.Add(SharedPluginFeatures.Constants.HeaderAuthorizationName, "Basic " + encoded);

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreNotEqual(400, httpContext.Response.StatusCode);
            Assert.AreNotEqual(401, httpContext.Response.StatusCode);
            Assert.AreEqual(200, httpContext.Response.StatusCode);
            Assert.IsTrue(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        public async Task LoginAutoLoginFromHeadersInvalidSplitter()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("MileyCyrus"));
            httpRequest.Headers.Add(SharedPluginFeatures.Constants.HeaderAuthorizationName, "Basic " + encoded);

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreEqual(400, httpContext.Response.StatusCode);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        public async Task LoginAutoLoginFromHeadersInvalidUsernameAndPassword()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("Miley:Cyrus"));
            httpRequest.Headers.Add(SharedPluginFeatures.Constants.HeaderAuthorizationName, "Basic " + encoded);

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        public async Task LoginAutoLoginFromHeadersInvalidValueBasicMissing()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            httpRequest.Headers.Add(SharedPluginFeatures.Constants.HeaderAuthorizationName, "blahblahblah");

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreEqual(400, httpContext.Response.StatusCode);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        public async Task LoginAutoLoginFromHeadersInvalidEncoding()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();

            IPluginClassesService pluginServices = new pm.PluginServices(_testPluginLogin) as IPluginClassesService;
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(pluginServices);
            TestAuthenticationService authenticationService = new TestAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            httpRequest.Headers.Add(SharedPluginFeatures.Constants.HeaderAuthorizationName, "Basic blahblahblah");

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreEqual(400, httpContext.Response.StatusCode);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }
    }
}
