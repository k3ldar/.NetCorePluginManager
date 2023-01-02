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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ApiAuthorizationAttributeTests.cs
 *
 *  Purpose:  Tests helpers for ApiAuthorizationAttribute
 *
 *  Date        Name                Reason
 *  17/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public sealed class ApiAuthorizationAttributeTests
    {
        private const string TestCategoryName = "Shared Plugin Features";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ApiAuthorizationAttribute sut = new ApiAuthorizationAttribute();
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(ActionFilterAttribute));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void OnActionExecuting_ApiAuthorizationServiceNotRegistered_ReturnsStatusCode405()
        {
            MockActionExecutingContext mockActionExecutingContext = CreateActionExecutingRequest();
            ApiAuthorizationAttribute sut = new ApiAuthorizationAttribute();
            sut.OnActionExecuting(mockActionExecutingContext);

            Assert.AreEqual(405, mockActionExecutingContext.HttpContext.Response.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void OnActionExecuting_ApiAuthorizationServiceReturnsFalse_Success()
        {
            MockActionExecutingContext mockActionExecutingContext = CreateActionExecutingRequest(new MockApiAuthorizationService(false));

            ApiAuthorizationAttribute sut = new ApiAuthorizationAttribute();
            sut.OnActionExecuting(mockActionExecutingContext);

            Assert.AreEqual(400, mockActionExecutingContext.HttpContext.Response.StatusCode);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void OnActionExecuting_ApiAuthorizationServiceReturnsTrue_Success()
        {
            MockActionExecutingContext mockActionExecutingContext = CreateActionExecutingRequest(new MockApiAuthorizationService(true));

            ApiAuthorizationAttribute sut = new ApiAuthorizationAttribute();
            sut.OnActionExecuting(mockActionExecutingContext);

            Assert.AreEqual(200, mockActionExecutingContext.HttpContext.Response.StatusCode);
        }

        public class MockApiAuthorizationService : IApiAuthorizationService
        {
            private readonly bool _success;

            public MockApiAuthorizationService(bool success)
            {
                _success = success;
            }

            public bool ValidateApiRequest(HttpRequest httpRequest, string policyName, out int responseCode)
            {
                responseCode = _success ? 200 : 400;
                return _success;
            }
        }

        private MockActionExecutingContext CreateActionExecutingRequest(MockApiAuthorizationService mockApiAuthorizationService = null)
        {
            IServiceCollection serviceCollection = new ServiceCollection() as IServiceCollection;

            if (mockApiAuthorizationService != null)
            {
                serviceCollection.AddSingleton<IApiAuthorizationService>(mockApiAuthorizationService);
            }

            MockHttpRequest testRequest = new MockHttpRequest();
            MockHttpResponse testResponse = new MockHttpResponse();
            MockHttpContext testContext = new MockHttpContext(testRequest, testResponse, serviceCollection.BuildServiceProvider());


            return new MockActionExecutingContext(testContext);
        }
    }
}
