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
 *  File: MarketingMiddlewareTests.cs
 *
 *  Purpose:  Test units for MVC Marketing Middleware class
 *
 *  Date        Name                Reason
 *  14/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using MarketingPlugin;

using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests.MiddlewareTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class MarketingMiddlewareTests : BaseMiddlewareTests
    {
        [TestInitialize]
        public void InitializeLoginTests()
        {
            InitializeLoginPluginManager();
        }


        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public void Construct_InvalidParam_NextRequestDelegateNull_DoesNotThrowException()
        {
            ISettingsProvider settingsProvider = new TestSettingsProvider("{}");
            MarketingMiddleware sut = new MarketingMiddleware(null, settingsProvider);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProviderNull_DoesNotThrowException()
        {
            MarketingMiddleware sut = new MarketingMiddleware(null, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        [ExpectedException(typeof(System.ArgumentNullException))]
        public async Task Invoke_InvalidParam_HttpContextNull_Throws_ArgumentNullException()
        {
            RequestDelegate nextDelegate = async (context) => { await Task.Delay(0); };

            ISettingsProvider settingsProvider = new TestSettingsProvider("{}");
            MarketingMiddleware sut = new MarketingMiddleware(null, settingsProvider);

            await sut.Invoke(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryMiddleware)]
        public async Task Invoke_StaticFileExtension_ContinuesWithoutProcessingMarketingData()
        {
            bool delegateCalled = false;
            RequestDelegate nextDelegate = async (context) => { delegateCalled = true; await Task.Delay(0); };
            TestHttpRequest httpRequest = new TestHttpRequest()
            {
                Path = "/blog/"
            };

            httpRequest.SetHost(new HostString("www.pluginmanager.website"));
            TestHttpResponse httpResponse = new TestHttpResponse();
            TestHttpContext httpContext = new TestHttpContext(httpRequest, httpResponse);
            httpRequest.SetContext(httpContext);

            ISettingsProvider settingsProvider = new TestSettingsProvider("{}");
            MarketingMiddleware sut = new MarketingMiddleware(null, settingsProvider);

            await sut.Invoke(httpContext);

            Assert.IsTrue(delegateCalled);
        }
    }
}
