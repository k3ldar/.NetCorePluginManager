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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: LoginControllerTests.cs
 *
 *  Purpose:  Unit tests for Login Controller
 *
 *  Date        Name                Reason
 *  09/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using LoginPlugin;
using LoginPlugin.Controllers;
using LoginPlugin.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Abstractions;
using Shared.Classes;

using SharedPluginFeatures;

using pm = PluginManager.Internal;

using utils = Shared.Utilities;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class LoginControllerTests : BaseControllerTests
    {
        private const string TestCategoryName = "Login Plugin";

        private const string SettingsEmpty = "{}";
        private const string SettingsWithLoginTimeout = "{\"LoginDays\":10}";

        [TestInitialize]
        public void InitialiseImageManagerPlugin()
        {
            //InitializeImageManagerPluginManager();
        }

        [TestCleanup]
        public void FinalizeTest()
        {
            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.ClearAllCaches();
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_ControllerHasCorrectAttributes_Success()
        {
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(LoginController)));
            Assert.IsTrue(ClassHasAttribute<LoggedOutAttribute>(typeof(LoginController)));
            Assert.IsTrue(ClassHasAttribute<SubdomainAttribute>(typeof(LoginController)));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_LoginProvider_Null_Throws_ArgumentNullException()
        {
            LoginController sut = new LoginController(null, new MockSettingsProvider(SettingsEmpty), new MockClaimsProvider(), new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProvider_Null_Throws_ArgumentNullException()
        {
            LoginController sut = new LoginController(new MockLoginProvider(), null, new MockClaimsProvider(), new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ClaimsProvider_Null_Throws_ArgumentNullException()
        {
            LoginController sut = new LoginController(new MockLoginProvider(), new MockSettingsProvider(SettingsEmpty), null, new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            LoginController sut = new LoginController(new MockLoginProvider(), new MockSettingsProvider(SettingsEmpty), new MockClaimsProvider(), new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_Get_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "Index";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(LoginController), MethodName, "Login", new Type[] { typeof(string) }));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_LoginNotRemembered_ReturnsValidViewAndModel_Success()
        {
            LoginController sut = CreateLoginController();
            IActionResult response = sut.Index(returnUrl: null);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            LoginViewModel loginViewModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(loginViewModel);
            Assert.AreEqual("/", loginViewModel.ReturnUrl);
            Assert.IsNull(loginViewModel.CaptchaText);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_LoginNotRemembered_WithValidReturnUrl_ReturnsValidViewAndModel_Success()
        {
            LoginController sut = CreateLoginController();
            IActionResult response = sut.Index("/Home");

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            LoginViewModel loginViewModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(loginViewModel);
            Assert.AreEqual("/Home", loginViewModel.ReturnUrl);
            Assert.IsNull(loginViewModel.CaptchaText);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_LoginRemembered_WithValidReturnUrl_ReturnsValidViewAndModel_Success()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", utils.Encrypt("123", loginControllerSettings.EncryptionKey));

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, cookies, testServiceProvider);
            IActionResult response = sut.Index("/Home");

            Assert.IsNotNull(response);

            LocalRedirectResult redirectResult = response as LocalRedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("/Home", redirectResult.Url);
            Assert.IsFalse(redirectResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_LoginRemembered_WithInvalidReturnUrl_ReturnsValidViewAndModel_Success()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", utils.Encrypt("123", loginControllerSettings.EncryptionKey));

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);
            //TestLoginProvider testLoginProvider = new TestLoginProvider();

            LoginController sut = CreateLoginController(null, null, null, null, cookies, testServiceProvider);
            IActionResult response = sut.Index(returnUrl: null);

            Assert.IsNotNull(response);

            LocalRedirectResult redirectResult = response as LocalRedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("/", redirectResult.Url);
            Assert.IsFalse(redirectResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_LoginRemembered_ExceptionRaisedAndCookieDeleted_Success()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", utils.Encrypt("999", loginControllerSettings.EncryptionKey));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);
            //TestLoginProvider testLoginProvider = new TestLoginProvider();

            LoginController sut = CreateLoginController(null, null, null, null, cookies, testServiceProvider, testHttpResponse);
            IActionResult response = sut.Index(returnUrl: null);

            Assert.IsNotNull(response);

            MockResponseCookies testResponseCookies = testHttpResponse.Cookies as MockResponseCookies;

            Assert.IsNotNull(testResponseCookies);
            MockResponseCookie testResponseCookie = testResponseCookies.Get("RememberMe");
            Assert.IsNotNull(testResponseCookie);

            Assert.IsTrue(testResponseCookie.CookieOptions.Expires < DateTime.Now);
            Assert.AreEqual("", testResponseCookie.Value);


            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            LoginViewModel loginViewModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(loginViewModel);
            Assert.AreEqual("/", loginViewModel.ReturnUrl);
            Assert.IsNull(loginViewModel.CaptchaText);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_Post_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "Index";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));
            Assert.IsTrue(MethodHasAttribute<BadEggAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));

            Assert.IsFalse(MethodHasBreadcrumbAttribute(typeof(LoginController), MethodName, "Login", new Type[] { typeof(LoginViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(LoginViewModel) }));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Index_InvalidLoginModel_Null_Throws_ArgumentNullException()
        {
            LoginController sut = CreateLoginController();
            sut.Index(model: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InvalidModel_CapctchText_Null_AddsModelErrorToState()
        {
            LoginController sut = CreateLoginController();
            IActionResult response = sut.Index(new LoginViewModel() { CaptchaText = "123" });
            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            LoginViewModel loginViewModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(loginViewModel);
            Assert.AreEqual("/", loginViewModel.ReturnUrl);
            Assert.IsNull(loginViewModel.CaptchaText);


            Assert.IsTrue(viewResult.ViewData.ModelState.ErrorCount > 0);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_UserAccountLocked_RedirectsToAction_AccountLocked_Success()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.Index(new LoginViewModel()
            {
                Username = "Account",
                Password = "Locked"
            });

            Assert.IsNotNull(response);

            RedirectToActionResult redirectViewResult = response as RedirectToActionResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("AccountLocked", redirectViewResult.ActionName);
            Assert.IsNull(redirectViewResult.ControllerName);
            Assert.IsFalse(redirectViewResult.Permanent);
            Assert.AreEqual(1, redirectViewResult.RouteValues.Count);
            Assert.IsTrue(redirectViewResult.RouteValues.ContainsKey("username"));
            Assert.AreEqual("Account", redirectViewResult.RouteValues["username"]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_UserAccountPasswordChangeRequired_WithRememberMeNotSet_RedirectsToAction_ChangePassword()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.Index(new LoginViewModel()
            {
                Username = "password",
                Password = "change required"
            });

            Assert.IsNotNull(response);

            LocalRedirectResult redirectViewResult = response as LocalRedirectResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("/Account/ChangePassword", redirectViewResult.Url);
            Assert.IsFalse(redirectViewResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_UserAccountPasswordChangeRequired_WithRememberMeSet_RedirectsToAction_ChangePassword()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.Index(new LoginViewModel()
            {
                Username = "password",
                Password = "change required",
                RememberMe = true
            });

            Assert.IsNotNull(response);

            LocalRedirectResult redirectViewResult = response as LocalRedirectResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("/Account/ChangePassword", redirectViewResult.Url);
            Assert.IsFalse(redirectViewResult.Permanent);

            MockResponseCookies testResponseCookies = testHttpResponse.Cookies as MockResponseCookies;

            Assert.IsNotNull(testResponseCookies);

            MockResponseCookie testResponseCookie = testResponseCookies.Get("RememberMe");
            Assert.IsNotNull(testResponseCookie);

            Assert.IsTrue(testResponseCookie.CookieOptions.Expires > DateTime.Now.AddDays(29));
            Assert.AreEqual("hQtD2ACcAGkHOYCkq16apA==", testResponseCookie.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_UserAccountLoginSuccess_WithRememberMeSet_RedirectsTo_UrlProvided()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.Index(new LoginViewModel()
            {
                Username = "login",
                Password = "success",
                RememberMe = true,
                ReturnUrl = "/Home"
            });

            Assert.IsNotNull(response);

            LocalRedirectResult redirectViewResult = response as LocalRedirectResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("/Home", redirectViewResult.Url);
            Assert.IsFalse(redirectViewResult.Permanent);

            MockResponseCookies testResponseCookies = testHttpResponse.Cookies as MockResponseCookies;

            Assert.IsNotNull(testResponseCookies);

            MockResponseCookie testResponseCookie = testResponseCookies.Get("RememberMe");
            Assert.IsNotNull(testResponseCookie);

            Assert.IsTrue(testResponseCookie.CookieOptions.Expires > DateTime.Now.AddDays(29));
            Assert.AreEqual("hQtD2ACcAGkHOYCkq16apA==", testResponseCookie.Value);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_UserAccountLoginFailed_ShowsCaptchaTextAfterRequiredAttempts()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "invalid",
                Password = "credentials",
                RememberMe = true,
                ReturnUrl = "/Home"
            };

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);

            IActionResult response = sut.Index(loginViewModel);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            LoginViewModel responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.IsFalse(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/Home", responseModel.ReturnUrl);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));


            response = sut.Index(loginViewModel);

            Assert.IsNotNull(response);

            viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/Home", responseModel.ReturnUrl);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InitialLoginFails_ShowCaptchaText_IncorrectCaptchaSupplied_ModelStateErrorAdded()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "invalid",
                Password = "credentials",
                RememberMe = true,
                ReturnUrl = "/Home"
            };

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);

            IActionResult response = sut.Index(loginViewModel);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            LoginViewModel responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.IsFalse(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/Home", responseModel.ReturnUrl);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));


            response = sut.Index(loginViewModel);

            Assert.IsNotNull(response);

            viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/Home", responseModel.ReturnUrl);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));

            loginViewModel.Username = "login";
            loginViewModel.Password = "success";
            loginViewModel.CaptchaText = "-1-2-3";

            response = sut.Index(loginViewModel);

            Assert.IsNotNull(response);

            viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNotNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.CaptchaText.Length > 4);
            Assert.IsTrue(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/Home", responseModel.ReturnUrl);
            Assert.AreEqual(2, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "The code you entered is not valid!"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InitialLoginFails_ShowCaptchaText_CallLogin_Get_CaptchaTextShown()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Username = "invalid",
                Password = "credentials",
                RememberMe = true,
                ReturnUrl = "/Home"
            };

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);

            IActionResult response = sut.Index(loginViewModel);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            LoginViewModel responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.IsFalse(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/Home", responseModel.ReturnUrl);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));


            response = sut.Index(loginViewModel);

            Assert.IsNotNull(response);

            viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/Home", responseModel.ReturnUrl);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));

            sut = CreateLoginController(null, null, null, null, null, testServiceProvider, null);
            response = sut.Index("/ALink");

            Assert.IsNotNull(response);

            viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            responseModel = viewResult.Model as LoginViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.ShowCaptchaImage);
            Assert.AreEqual("/ALink", responseModel.ReturnUrl);
            Assert.AreEqual(0, viewResult.ViewData.ModelState.ErrorCount);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AccountLocked_Get_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "AccountLocked";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(LoginController), MethodName, "AccountLocked", new Type[] { typeof(string) }));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(string) }));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AccountLocked_Get_InvalidParamUsername_Null_RedirectsToIndex()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.AccountLocked(username: null);

            Assert.IsNotNull(response);

            RedirectToActionResult redirectViewResult = response as RedirectToActionResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("Index", redirectViewResult.ActionName);
            Assert.IsNull(redirectViewResult.ControllerName);
            Assert.IsFalse(redirectViewResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AccountLocked_Get_InvalidParamUsername_EmptyString_RedirectsToIndex()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.AccountLocked(username: "");

            Assert.IsNotNull(response);

            RedirectToActionResult redirectViewResult = response as RedirectToActionResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("Index", redirectViewResult.ActionName);
            Assert.IsNull(redirectViewResult.ControllerName);
            Assert.IsFalse(redirectViewResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AccountLocked_Get_ReturnsValidViewAndModel()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.AccountLocked(username: "test user");

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);

            AccountLockedViewModel viewModel = viewResult.Model as AccountLockedViewModel;
            Assert.IsNotNull(viewModel);
            Assert.AreEqual("test user", viewModel.Username);
            Assert.IsNull(viewModel.UnlockCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AccountLocked_Post_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "AccountLocked";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
            Assert.IsTrue(MethodHasAttribute<BadEggAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));

            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(AccountLockedViewModel) }));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AccountLocked_Post_InvalidModel_Null_Throws_ArgumentNullException()
        {
            LoginController sut = CreateLoginController();
            IActionResult response = sut.AccountLocked(model: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AccountLocked_Post_AccountUnlocked_RedirectsToLogin_Success()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            IActionResult response = sut.AccountLocked(model: new AccountLockedViewModel(GenerateTestBaseModelData(), "unlock me") { UnlockCode = "123" });

            Assert.IsNotNull(response);

            RedirectResult redirectResult = response as RedirectResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("/Login", redirectResult.Url);
            Assert.IsFalse(redirectResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AccountLocked_Post_InvalidUnlockCode_ReturnsViewAndModel_WithModelStateError()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);

            AccountLockedViewModel model = new AccountLockedViewModel(GenerateTestBaseModelData(), "a user")
            {
                UnlockCode = "1234"
            };

            IActionResult response = sut.AccountLocked(model: model);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            AccountLockedViewModel responseModel = viewResult.Model as AccountLockedViewModel;
            Assert.IsNotNull(responseModel);
            Assert.AreEqual("a user", responseModel.Username);
            Assert.AreEqual("", responseModel.UnlockCode);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "The code you entered is not valid!"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Get_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "ForgotPassword";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName, new Type[] { }));
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(LoginController), MethodName, "ForgotPassword", new Type[] { }));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName, new Type[] { }));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName, new Type[] { }));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName, new Type[] { }));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName, new Type[] { }));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName, new Type[] { }));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName, new Type[] { }));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Get_ReturnsViewAndModel_WithCaptchaTest()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);

            IActionResult response = sut.ForgotPassword();

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            ForgotPasswordViewModel responseModel = viewResult.Model as ForgotPasswordViewModel;
            Assert.IsNotNull(responseModel);
            Assert.IsNull(responseModel.Username);
            Assert.IsNull(responseModel.CaptchaText);
            Assert.AreEqual(0, viewResult.ViewData.ModelState.ErrorCount);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Post_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "ForgotPassword";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));
            Assert.IsTrue(MethodHasAttribute<BadEggAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));

            Assert.IsFalse(MethodHasBreadcrumbAttribute(typeof(LoginController), MethodName, "ForgotPassword", new Type[] { typeof(ForgotPasswordViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName, new Type[] { typeof(ForgotPasswordViewModel) }));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForgotPassword_Post_InvalidModel_Null_Throws_ArgumentNullException()
        {
            LoginController sut = CreateLoginController();
            sut.ForgotPassword(model: null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Post_InvalidCaptchaText_Null_ReturnsViewResult_WithModelStateError()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Username = "user not found"
            };

            IActionResult response = sut.ForgotPassword(model: model);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            ForgotPasswordViewModel responseModel = viewResult.Model as ForgotPasswordViewModel;
            Assert.IsNotNull(responseModel);
            Assert.AreEqual("user not found", responseModel.Username);
            Assert.IsNotNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.CaptchaText.Length > 5);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "The code you entered is not valid!"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Post_CaptchaValueNotFoundInMemory_ReturnsViewResult_WithModelStateError()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Username = "user not found",
                CaptchaText = "1-2-3-4-5"
            };

            IActionResult response = sut.ForgotPassword(model: model);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            ForgotPasswordViewModel responseModel = viewResult.Model as ForgotPasswordViewModel;
            Assert.IsNotNull(responseModel);
            Assert.AreEqual("user not found", responseModel.Username);
            Assert.IsNotNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.CaptchaText.Length > 5);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "The code you entered is not valid!"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Post_WrontCaptchaValue_ReturnsViewResult_WithModelStateError()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            sut.ForgotPassword();
            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Username = "valid user",
                CaptchaText = sut.GetCacheValue("abc123").CaptchaText + "_wrong"
            };

            IActionResult response = sut.ForgotPassword(model: model);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            ForgotPasswordViewModel responseModel = viewResult.Model as ForgotPasswordViewModel;
            Assert.IsNotNull(responseModel);
            Assert.AreEqual("valid user", responseModel.Username);
            Assert.IsNotNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.CaptchaText.Length > 5);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "The code you entered is not valid!"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Post_UserNotFound_ReturnsViewResult_WithModelStateError()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            sut.ForgotPassword();
            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Username = "user not found",
                CaptchaText = sut.GetCacheValue("abc123").CaptchaText
            };

            IActionResult response = sut.ForgotPassword(model: model);

            Assert.IsNotNull(response);

            ViewResult viewResult = response as ViewResult;
            Assert.IsNotNull(viewResult);

            ForgotPasswordViewModel responseModel = viewResult.Model as ForgotPasswordViewModel;
            Assert.IsNotNull(responseModel);
            Assert.AreEqual("user not found", responseModel.Username);
            Assert.IsNotNull(responseModel.CaptchaText);
            Assert.IsTrue(responseModel.CaptchaText.Length > 5);
            Assert.AreEqual(1, viewResult.ViewData.ModelState.ErrorCount);
            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Invalid User Name/Password.  Please try again"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ForgotPassword_Post_UserFound_ForgotPasswordProcessedCorrectly_RedirectedToLogin()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockHttpResponse testHttpResponse = new MockHttpResponse();

            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), new MockAuthenticationService());

            ITempDataProvider tempDataProvider = new MockTempDataProvider();
            services.Add(typeof(ITempDataDictionaryFactory), new TempDataDictionaryFactory(tempDataProvider));

            MockUrlHelperFactory testUrlHelperFactory = new MockUrlHelperFactory();
            services.Add(typeof(IUrlHelperFactory), testUrlHelperFactory);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);

            LoginController sut = CreateLoginController(null, null, null, null, null, testServiceProvider, testHttpResponse);
            sut.ForgotPassword();
            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Username = "valid user",
                CaptchaText = sut.GetCacheValue("abc123").CaptchaText
            };

            IActionResult response = sut.ForgotPassword(model: model);

            Assert.IsNotNull(response);

            RedirectToActionResult redirectViewResult = response as RedirectToActionResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("Index", redirectViewResult.ActionName);
            Assert.IsNull(redirectViewResult.ControllerName);
            Assert.IsFalse(redirectViewResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCacheValue_NullCacheName_Returns_Null()
        {
            LoginController sut = CreateLoginController();
            Assert.IsNull(sut.GetCacheValue(null));
            Assert.IsNull(sut.GetCacheValue(""));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCacheValue_CacheItemNotFound_Returns_Null()
        {
            LoginController sut = CreateLoginController();
            Assert.IsNull(sut.GetCacheValue("invalid cache item"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCacheValue_CacheItemIsNotLoginCacheItem_Returns_Null()
        {

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Logout_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "Logout";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName));
            Assert.IsTrue(MethodHasBreadcrumbAttribute(typeof(LoginController), MethodName, "Logout"));
            Assert.IsTrue(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName));

            Assert.IsFalse(MethodHasAttribute<BadEggAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Logout_UserSessionNotFound_CookieDeleted_RedirectedToHome()
        {
            ThreadManager.Initialise();

            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());
            LoginControllerSettings loginControllerSettings = settingsProvider.GetSettings<LoginControllerSettings>(nameof(LoginPlugin));

            MockRequestCookieCollection cookies = new MockRequestCookieCollection();
            cookies.AddCookie("RememberMe", utils.Encrypt("123", loginControllerSettings.EncryptionKey));

            MockAuthenticationService testAuthenticationService = new MockAuthenticationService();
            Dictionary<Type, object> services = new Dictionary<Type, object>();
            services.Add(typeof(IAuthenticationService), testAuthenticationService);

            MockServiceProvider testServiceProvider = new MockServiceProvider(services);
            MockHttpResponse testHttpResponse = new MockHttpResponse();

            LoginController sut = CreateLoginController(null, null, null, null, cookies, testServiceProvider, testHttpResponse);
            IActionResult indexResponse = sut.Index("/Home");
            Assert.IsNotNull(indexResponse);

            IActionResult response = sut.Logout();

            Assert.IsNotNull(response);

            Assert.IsTrue(testAuthenticationService.SignOutAsyncCalled);

            RedirectResult redirectViewResult = response as RedirectResult;
            Assert.IsNotNull(redirectViewResult);
            Assert.AreEqual("/", redirectViewResult.Url);
            Assert.IsFalse(redirectViewResult.Permanent);


            MockResponseCookies testResponseCookies = testHttpResponse.Cookies as MockResponseCookies;

            Assert.IsNotNull(testResponseCookies);
            MockResponseCookie testResponseCookie = testResponseCookies.Get("RememberMe");
            Assert.IsNotNull(testResponseCookie);

            Assert.IsTrue(testResponseCookie.CookieOptions.Expires < DateTime.Now);
            Assert.AreEqual("", testResponseCookie.Value);

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCaptchaImage_Validate_MethodHasCorrectAttributes_Success()
        {
            const string MethodName = "GetCaptchaImage";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(LoginController), MethodName));
            Assert.IsTrue(MethodHasDenySpiderAttribute(typeof(LoginController), MethodName, "*"));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<BreadcrumbAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<BadEggAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPatchAttribute>(typeof(LoginController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AuthorizeAttribute>(typeof(LoginController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCaptchaImage_LoginCacheItemNotFound_ReturnsHttp400()
        {
            LoginController sut = CreateLoginController();
            IActionResult actionResult = sut.GetCaptchaImage();

            Assert.IsNotNull(actionResult);

            StatusCodeResult statusCodeResult = actionResult as StatusCodeResult;

            Assert.IsNotNull(statusCodeResult);
            Assert.AreEqual(400, statusCodeResult.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetCaptchaImage_LoginCacheItemFound_ReturnsHttp400()
        {
            LoginController sut = CreateLoginController();

            sut.ForgotPassword();

            IActionResult actionResult = sut.GetCaptchaImage();

            Assert.IsNotNull(actionResult);

            FileContentResult fileContentResult = actionResult as FileContentResult;

            Assert.AreEqual("image/png", fileContentResult.ContentType);
            Assert.IsTrue(fileContentResult.FileContents.Length > 100);
            Assert.AreEqual("", fileContentResult.FileDownloadName);
        }

        private LoginController CreateLoginController(MockLoginProvider testLoginProvider = null,
            List<BreadcrumbItem> breadcrumbs = null,
            MockSettingsProvider testSettingsProvider = null,
            MockClaimsProvider testClaimsProvider = null,
            MockRequestCookieCollection cookieCollection = null,
            MockServiceProvider testServiceProvider = null,
            MockHttpResponse testHttpResponse = null,
            MockMemoryCache mockMemoryCache = null)
        {
            IPluginClassesService pluginServices = _testDynamicContentPlugin as IPluginClassesService;
            IPluginHelperService pluginHelperService = _testDynamicContentPlugin as IPluginHelperService;
            ISettingsProvider settingsProvider = new pm.DefaultSettingProvider(Directory.GetCurrentDirectory());

            LoginController Result = new LoginController(
                testLoginProvider ?? new MockLoginProvider(),
                testSettingsProvider ?? new MockSettingsProvider(SettingsEmpty),
                testClaimsProvider ?? new MockClaimsProvider(),
                mockMemoryCache ?? new MockMemoryCache());

            Result.ControllerContext = CreateTestControllerContext(breadcrumbs, cookieCollection, testServiceProvider, testHttpResponse);


            return Result;

        }
    }
}
