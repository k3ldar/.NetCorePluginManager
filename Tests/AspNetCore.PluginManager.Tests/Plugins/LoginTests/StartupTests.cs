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
 *  File: StartupTests.cs
 *
 *  Purpose:  Tests for starting up
 *
 *  Date        Name                Reason
 *  08/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using LoginPlugin;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

using Shared.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StartupTests
    {
        private const string TestCategoryName = "Login";

        [TestCleanup]
        public void CleanupStartupTests()
        {
            PluginManagerService.Finalise();
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_InitialisesPluginManager_Success()
        {
            Assert.IsFalse(PluginManagerService.HasInitialised);

            Startup sut = new Startup();
            Assert.IsNotNull(sut);

            Assert.IsTrue(PluginManagerService.HasInitialised);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigureService_InvalidParamNull_Throws_ArgumentNullException()
        {
            Startup sut = new Startup();

            sut.ConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
		public void ConfigureServices_EnableEndpointRoutingIsFalse_Success()
        {
            ThreadManager.Initialise();
            try
            {
                Startup sut = new Startup();

                MockServiceCollection serviceCollection = new MockServiceCollection();
                sut.ConfigureServices(serviceCollection);

                Assert.IsTrue(serviceCollection.HasMvcConfigured());
                Assert.IsFalse(serviceCollection.HasMvcEndpointRouting());
                Assert.IsTrue(serviceCollection.HasSessionStateTempDataProvider());
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Configure_InvalidParamNull_Throws_ArgumentNullException()
        {
            Startup sut = new Startup();

            sut.Configure(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Configure_UseMvcIsCalled_CorrectDefaultRouteAdded()
        {
            ThreadManager.Initialise();
            try
            {
                IWebHost host = WebHost.CreateDefaultBuilder(new string[] { })
                    .UseStartup<Startup>().Build();

                Startup sut = new Startup();

                MockApplicationBuilder applicationBuilder = new MockApplicationBuilder(host.Services);

                sut.Configure(applicationBuilder);

                Assert.IsTrue(applicationBuilder.UseMvcCalled);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }
    }
}
