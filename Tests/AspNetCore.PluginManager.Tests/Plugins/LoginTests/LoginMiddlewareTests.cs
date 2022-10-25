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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.MiddlewareTests;
using AspNetCore.PluginManager.Tests.Shared;

using LoginPlugin;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using static Shared.Utilities;
using static SharedPluginFeatures.Constants;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LoginMiddlewareTests : BaseMiddlewareTests
    {
        private const string TestCategoryName = "Login";

        [TestInitialize]
        public void InitializeLoginTests()
        {
            InitializeLoginPluginManager();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public async Task LoginNullContextValue()
        {
            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(null, authenticationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullLoginProviderValue()
        {
            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, null, settingsProvider,
                claimsProvider);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullSettingsProviderValue()
        {
            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, null,
                claimsProvider);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public async Task LoginNullAuthenticationValueOnInvoke()
        {
            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", "1");

            MockHttpRequest httpRequest = new MockHttpRequest(cookies);
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullClaimsProviderValue()
        {
            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void LoginNullRequestDelegateValue()
        {
            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);

            LoginMiddleware login = new LoginMiddleware(null, loginProvider, settingsProvider,
                claimsProvider);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.FormatException))]
        public async Task LoginFromCookieValueCookieValueNotEncrypted()
        {
            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", "1");

            MockHttpRequest httpRequest = new MockHttpRequest(cookies);
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.FormatException))]
        public async Task LoginFromCookieValueCookieValueNotValid()
        {
            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", "asdfasdfasf");

            MockHttpRequest httpRequest = new MockHttpRequest(cookies);
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            MockLoginProvider loginProvider = new MockLoginProvider();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task LoginFromCookieValueCookieInvalidLoginUserNotFound()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));
            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", Encrypt("1", loginControllerSettings.EncryptionKey));

            MockHttpRequest httpRequest = new MockHttpRequest(cookies);
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            MockResponseCookies responseCookies = httpResponse.Cookies as MockResponseCookies;

            Assert.IsNotNull(responseCookies);
            Assert.IsTrue(responseCookies.Get("RememberMe").CookieOptions.Expires < DateTime.Now.AddMinutes(-1439));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task LoginFromCookieValueCookieValidLoginUserFound()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));
            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", Encrypt("123", loginControllerSettings.EncryptionKey));

            MockHttpRequest httpRequest = new MockHttpRequest(cookies);
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            MockResponseCookies responseCookies = httpResponse.Cookies as MockResponseCookies;

            Assert.IsNotNull(responseCookies);
            Assert.IsTrue(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Login_FromCookieValue_CookieInvalid_CookieDeleted()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));
            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", Encrypt("adfasfd", loginControllerSettings.EncryptionKey));

            MockHttpRequest httpRequest = new MockHttpRequest(cookies);
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            MockResponseCookies responseCookies = httpResponse.Cookies as MockResponseCookies;

            Assert.IsNotNull(responseCookies);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);

            MockResponseCookie rememberMeCookie = responseCookies.Get("RememberMe");

            Assert.IsNotNull(rememberMeCookie);
            Assert.IsTrue(rememberMeCookie.CookieOptions.Expires < DateTime.Now);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Login_AutoLoginFromHeaders_ValidUsernameAndPassword()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("login:success"));
            httpRequest.Headers.Add(HeaderAuthorizationName, "Basic " + encoded);

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreNotEqual(400, httpContext.Response.StatusCode);
            Assert.AreNotEqual(401, httpContext.Response.StatusCode);
            Assert.AreEqual(200, httpContext.Response.StatusCode);
            Assert.IsTrue(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Login_AutoLoginFromHeaders_ValidUsernameAndPassword_InvalidSession()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpContext.CreateSession = false;
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("admin:password"));
            httpRequest.Headers.Add(HeaderAuthorizationName, "Basic " + encoded);

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreNotEqual(400, httpContext.Response.StatusCode);
            Assert.AreNotEqual(401, httpContext.Response.StatusCode);
            Assert.AreEqual(200, httpContext.Response.StatusCode);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Login_AutoLoginFromHeaders_InvalidSplitter()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            string encoded = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes("MileyCyrus"));
            httpRequest.Headers.Add(HeaderAuthorizationName, "Basic " + encoded);

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreEqual(400, httpContext.Response.StatusCode);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Login_AutoLoginFromHeaders_InvalidValueBasicMissing()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            httpRequest.Headers.Add(HeaderAuthorizationName, "blahblahblah");

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreEqual(400, httpContext.Response.StatusCode);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Login_AutoLoginFromHeaders_InvalidEncoding()
        {
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();

            //IPluginClassesService pluginServices = _testPluginLogin as IPluginClassesService;
            MockHttpContext httpContext = new MockHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);
            MockLoginProvider loginProvider = new MockLoginProvider();

            MockClaimsProvider claimsProvider = new MockClaimsProvider(/*pluginServices*/);
            MockAuthenticationService authenticationService = new MockAuthenticationService();
            RequestDelegate requestDelegate = async (context) => { await Task.Delay(0); };

            httpRequest.Headers.Add(HeaderAuthorizationName, "Basic blahblahblah");

            LoginMiddleware login = new LoginMiddleware(requestDelegate, loginProvider, settingsProvider,
                claimsProvider);

            await login.Invoke(httpContext, authenticationService);

            Assert.AreEqual(400, httpContext.Response.StatusCode);
            Assert.IsFalse(authenticationService.SignInAsyncCalled);
        }
    }
}
