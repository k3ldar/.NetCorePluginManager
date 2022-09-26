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
 *  File: BaseControllerTests.cs
 *
 *  Purpose:  Helper methods for testing controllers
 *
 *  Date        Name                Reason
 *  01/10/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

using MockPluginManager = AspNetCore.PluginManager.Tests.Shared.MockPluginManager;

namespace AspNetCore.PluginManager.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class BaseControllerTests : BaseController
    {
        protected readonly static DateTime DefaultActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0);
        protected readonly static DateTime DefaultActiveTo = new DateTime(2050, 12, 31, 23, 59, 59);


        protected static MockPluginManager _testSpiderPlugin = new MockPluginManager();
        protected static bool? _pluginLoadedSpiderPlugin = null;
        protected static IPluginClassesService _pluginServicesSpiderPlugin;

        protected static MockPluginManager _testDynamicContentPlugin = new MockPluginManager();
        protected static bool? _pluginLoadedDynamicContentPlugin = null;
        protected static IPluginClassesService _pluginServicesDynamicContent;

        protected static MockPluginManager _testImageManagerPlugin = new MockPluginManager();
        protected static bool? _pluginLoadedImageManagerPlugin = null;
        protected static IPluginClassesService _pluginServicesImageManager;

        protected void WaitForThreadToFinish(string threadName, int millisecondsToWait = 5000)
        {
            DateTime startTime = DateTime.Now;

            while (true)
            {
                if (!ThreadManager.Exists(threadName))
                    break;

                TimeSpan timeTaken = DateTime.Now - startTime;

                if (timeTaken.TotalMilliseconds > millisecondsToWait)
                    break;
            } 
        }

        protected ControllerContext CreateTestControllerContext(List<BreadcrumbItem> breadcrumbs = null,
            MockRequestCookieCollection testCookieCollection = null,
            MockServiceProvider testServiceProvider = null,
            MockHttpResponse testHttpResponse = null,
			MockHttpContext requestContext = null)
        {
            MockHttpRequest httpRequest = testCookieCollection == null ? new MockHttpRequest() : new MockHttpRequest(testCookieCollection);

			if (requestContext != null)
				httpRequest.SetContext(requestContext);

            MockHttpResponse httpResponse = testHttpResponse ?? new MockHttpResponse();
            ControllerContext Result = new ControllerContext();
            Result.HttpContext = testServiceProvider == null ? new MockHttpContext(httpRequest, httpResponse, breadcrumbs) : new MockHttpContext(httpRequest, httpResponse, testServiceProvider, breadcrumbs);

            return Result;
        }

        protected void SetTestControllerContext()
        {
            MockHttpRequest httpRequest = new MockHttpRequest();
            MockHttpResponse httpResponse = new MockHttpResponse();
            ControllerContext Result = new ControllerContext();
            Result.HttpContext = new MockHttpContext(httpRequest, httpResponse);

            ControllerContext = Result;
        }

        public bool ClassHasAttribute<T>(Type classType) where T : Attribute
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            return classType.IsDefined(typeof(T));
        }

        public bool ClassAuthorizeAttributeHasCorrectPolicy(Type classType, string expectedPolicyName)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (!ClassHasAttribute<AuthorizeAttribute>(classType))
                return false;

            foreach (Attribute attr in classType.GetCustomAttributes(false))
            {
                if (attr.GetType().Equals(typeof(AuthorizeAttribute)))
                {
                    AuthorizeAttribute authorizeAttribute = (AuthorizeAttribute)attr;

                    if (authorizeAttribute.Policy.Equals(expectedPolicyName))
                        return true;
                }
            }

            return false;
        }

        public bool AssemblyHasViewResource(Type classType, string location, string viewName)
        {
            string assemblyName = classType.Assembly.FullName.Split(',')[0];

            foreach (string resource in classType.Assembly.GetManifestResourceNames())
            {
                if (String.IsNullOrEmpty(resource))
                    continue;

                if (resource.Equals($"{assemblyName}.Views.{location}.{viewName}.cshtml"))
                    return true;
            }


            return false;
        }

        public bool MethodHasAttribute<T>(Type classType, string methodName) where T : Attribute
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            return methodInfo.IsDefined(typeof(T));
        }
        public bool MethodHasAttribute<T>(Type classType, string methodName, Type[] methodParams) where T : Attribute
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName, methodParams);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            return methodInfo.IsDefined(typeof(T));
        }

        public bool MethodHasRouteAttribute(Type classType, string methodName, string routeValue)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            bool isDefined = methodInfo.IsDefined(typeof(RouteAttribute));

            if (!isDefined)
                return false;

            List<RouteAttribute> routeAttributes = methodInfo.GetCustomAttributes(true).OfType<RouteAttribute>().ToList();

            foreach (RouteAttribute attribute in routeAttributes)
            {
                if (attribute.Template.Equals(routeValue))
                    return true;
            }

            return false;
        }

        public bool MethodHasAuthorizeAttribute(Type classType, string methodName, string policyName)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            bool isDefined = methodInfo.IsDefined(typeof(AuthorizeAttribute));

            if (!isDefined)
                return false;

            AuthorizeAttribute routeAttribute = methodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().FirstOrDefault();

            return routeAttribute.Policy.Equals(policyName);
        }

        public bool MethodHasBreadcrumbAttribute(Type classType, string methodName, string breadcrumbValue, string parentName = "", bool hasParams = false)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            BreadcrumbAttribute routeAttribute = methodInfo.GetCustomAttributes(true).OfType<BreadcrumbAttribute>().FirstOrDefault();

            if (routeAttribute == null)
                return false;

            if (!routeAttribute.Name.Equals(breadcrumbValue))
                return false;

            if (!String.IsNullOrEmpty(parentName) && !routeAttribute.ParentRoute.Equals(parentName))
                return false;

            if (!routeAttribute.HasParams.Equals(hasParams))
                return false;

            return true;
        }

        public bool MethodHasBreadcrumbAttribute(Type classType, string methodName, string breadcrumbValue, Type[] paramTypes, string parentName = "", bool hasParams = false)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName, paramTypes);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            BreadcrumbAttribute routeAttribute = methodInfo.GetCustomAttributes(true).OfType<BreadcrumbAttribute>().FirstOrDefault();

            if (routeAttribute == null)
                return false;

            if (!routeAttribute.Name.Equals(breadcrumbValue))
                return false;

            if (!String.IsNullOrEmpty(parentName) && !routeAttribute.ParentRoute.Equals(parentName))
                return false;

            if (!routeAttribute.HasParams.Equals(hasParams))
                return false;

            return true;
        }

        public bool MethodHasDenySpiderAttribute(Type classType, string methodName, string userAgent)
        {
            if (classType == null)
                throw new ArgumentNullException(nameof(classType));

            if (String.IsNullOrEmpty(methodName))
                throw new ArgumentNullException(nameof(methodName));

            MethodInfo methodInfo = classType.GetMethod(methodName);

            if (methodInfo == null)
                throw new InvalidOperationException($"Method {methodName} does not exist");

            bool isDefined = methodInfo.IsDefined(typeof(DenySpiderAttribute));

            if (!isDefined)
                return false;

            List<DenySpiderAttribute> spiderAttributes = methodInfo.GetCustomAttributes(true).OfType<DenySpiderAttribute>().ToList();

            foreach (DenySpiderAttribute spiderAttribute in spiderAttributes)
            {
                if (spiderAttribute.UserAgent.Equals(userAgent))
                    return true;
            }

            return false;
        }

        protected void ValidateBaseModel(ViewResult viewResult)
        {
            Assert.IsInstanceOfType(viewResult.Model, typeof(BaseModel));
        }

        protected void ValidateBaseModel(PartialViewResult viewResult)
        {
            Assert.IsInstanceOfType(viewResult.Model, typeof(BaseModel));
        }

        protected bool ViewResultContainsModelStateError(ViewResult viewResult, string name, string value)
        {
            if (viewResult == null)
                throw new ArgumentNullException(nameof(viewResult));

            if (name == null)
                throw new ArgumentException(nameof(name));

            if (String.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            if (!viewResult.ViewData.ModelState.ContainsKey(name))
                return false;

            return viewResult.ViewData.ModelState[name].Errors.Where(e => e.ErrorMessage.Equals(value)).Any();
        }

        protected BaseModelData GenerateTestBaseModelData(bool isloggedIn = false)
        {
            BaseModelData Result = new BaseModelData(new List<BreadcrumbItem>(),
                new ShoppingCartSummary(1, 0, 0, 0, 0, 20, Thread.CurrentThread.CurrentUICulture, "GBP"),
                "The Title", "The Author", "The Description", "The Tags", false, isloggedIn, true);


            return Result;
        }

        protected void InitializeSpiderPluginPluginManager()
        {
            lock (_testSpiderPlugin)
            {
                while (_pluginLoadedSpiderPlugin.HasValue && !_pluginLoadedSpiderPlugin.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedSpiderPlugin.HasValue && _pluginLoadedSpiderPlugin.Value)
                {
                    return;
                }

                if (_pluginLoadedSpiderPlugin == null)
                {
                    _pluginLoadedSpiderPlugin = false;
                }

                _testSpiderPlugin.AddAssembly(Assembly.GetExecutingAssembly());
                _testSpiderPlugin.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testSpiderPlugin.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testSpiderPlugin.UsePlugin(typeof(LoginPlugin.PluginInitialisation));
                _testSpiderPlugin.UsePlugin(typeof(Spider.Plugin.PluginInitialisation));
                _testSpiderPlugin.UsePlugin(typeof(LoginPlugin.PluginInitialisation));

                _testSpiderPlugin.ConfigureServices();

                _pluginServicesSpiderPlugin = _testSpiderPlugin as IPluginClassesService;

                _pluginLoadedSpiderPlugin = true;
            }

            Assert.IsNotNull(_pluginServicesSpiderPlugin);

        }

        protected void InitializeDynamicContentPluginManager()
        {
            lock (_testDynamicContentPlugin)
            {
                while (_pluginLoadedDynamicContentPlugin.HasValue && !_pluginLoadedDynamicContentPlugin.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedDynamicContentPlugin.HasValue && _pluginLoadedDynamicContentPlugin.Value)
                {
                    return;
                }

                if (_pluginLoadedDynamicContentPlugin == null)
                {
                    _pluginLoadedDynamicContentPlugin = false;
                }

                _testDynamicContentPlugin.AddAssembly(Assembly.GetExecutingAssembly());
                _testDynamicContentPlugin.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testDynamicContentPlugin.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testDynamicContentPlugin.UsePlugin(typeof(LoginPlugin.PluginInitialisation));
                _testDynamicContentPlugin.UsePlugin(typeof(Spider.Plugin.PluginInitialisation));
                _testDynamicContentPlugin.UsePlugin(typeof(DynamicContent.Plugin.PluginInitialisation));

                _testDynamicContentPlugin.ConfigureServices();

                _pluginServicesDynamicContent = _testDynamicContentPlugin as IPluginClassesService;

                _pluginLoadedDynamicContentPlugin = true;
            }

            Assert.IsNotNull(_pluginServicesDynamicContent);

        }

        protected void InitializeImageManagerPluginManager()
        {
            lock (_testImageManagerPlugin)
            {
                while (_pluginLoadedImageManagerPlugin.HasValue && !_pluginLoadedImageManagerPlugin.Value)
                {
                    System.Threading.Thread.Sleep(30);
                }

                if (_pluginLoadedImageManagerPlugin.HasValue && _pluginLoadedImageManagerPlugin.Value)
                {
                    return;
                }

                if (_pluginLoadedImageManagerPlugin == null)
                {
                    _pluginLoadedImageManagerPlugin = false;
                }

                _testImageManagerPlugin.AddAssembly(Assembly.GetExecutingAssembly());
                _testImageManagerPlugin.UsePlugin(typeof(DemoWebsite.Classes.PluginInitialisation));
                _testImageManagerPlugin.UsePlugin(typeof(MemoryCache.Plugin.PluginInitialisation));
                _testImageManagerPlugin.UsePlugin(typeof(LoginPlugin.PluginInitialisation));
                _testImageManagerPlugin.UsePlugin(typeof(Spider.Plugin.PluginInitialisation));

                _testImageManagerPlugin.ConfigureServices();

                _pluginServicesImageManager = _testImageManagerPlugin as IPluginClassesService;

                _pluginLoadedImageManagerPlugin = true;
            }

            Assert.IsNotNull(_pluginServicesImageManager);
        }
    }
}
