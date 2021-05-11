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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    public class BaseControllerTests : BaseController
    {
        protected static TestPluginManager _testSpiderPlugin = new TestPluginManager();
        protected static bool? _pluginLoadedSpiderPlugin = null;
        protected static IPluginClassesService _pluginServicesSpiderPlugin;

        protected static TestPluginManager _testDynamicContentPlugin = new TestPluginManager();
        protected static bool? _pluginLoadedDynamicContentPlugin = null;
        protected static IPluginClassesService _pluginServicesDynamicContent;

        protected static TestPluginManager _testImageManagerPlugin = new TestPluginManager();
        protected static bool? _pluginLoadedImageManagerPlugin = null;
        protected static IPluginClassesService _pluginServicesImageManager;

        protected ControllerContext CreateTestControllerContext(List<BreadcrumbItem> breadcrumbs = null)
        {
            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();
            ControllerContext Result = new ControllerContext();
            Result.HttpContext = new TestHttpContext(httpRequest, httpResponse, breadcrumbs);

            return Result;
        }

        protected void SetTestControllerContext()
        {
            TestHttpRequest httpRequest = new TestHttpRequest();
            TestHttpResponse httpResponse = new TestHttpResponse();
            ControllerContext Result = new ControllerContext();
            Result.HttpContext = new TestHttpContext(httpRequest, httpResponse);

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

        public bool MethodRouteAttribute(Type classType, string methodName, string routeValue)
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

            RouteAttribute routeAttribute = methodInfo.GetCustomAttributes(true).OfType<RouteAttribute>().FirstOrDefault();

            return routeAttribute.Template.Equals(routeValue);
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

        protected void ValidateBaseModel(ViewResult viewResult)
        {
            Assert.IsInstanceOfType(viewResult.Model, typeof(BaseModel));
        }

        protected void ValidateBaseModel(PartialViewResult viewResult)
        {
            Assert.IsInstanceOfType(viewResult.Model, typeof(BaseModel));
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

                _pluginServicesSpiderPlugin = new pm.PluginServices(_testSpiderPlugin) as IPluginClassesService;

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

                _testDynamicContentPlugin.ConfigureServices();

                _pluginServicesDynamicContent = new pm.PluginServices(_testDynamicContentPlugin) as IPluginClassesService;

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

                _pluginServicesImageManager = new pm.PluginServices(_testImageManagerPlugin) as IPluginClassesService;

                _pluginLoadedImageManagerPlugin = true;
            }

            Assert.IsNotNull(_pluginServicesImageManager);

        }
    }
}
