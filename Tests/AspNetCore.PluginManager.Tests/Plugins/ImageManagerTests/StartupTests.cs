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
 *  File: StartupTests.cs
 *
 *  Purpose:  Tests for plugin startup class
 *
 *  Date        Name                Reason
 *  17/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StartupTests
    {
        private const string ImageManagerTestsCategory = "Image Manager Tests";

        [TestCleanup]
        [TestCategory(ImageManagerTestsCategory)]
        public void CleanupStartupTests()
        {
            PluginManagerService.Finalise();
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
		public void Construct_InitialisesPluginManager_Success()
        {
            Assert.IsFalse(PluginManagerService.HasInitialised);

            Startup sut = new Startup();
            Assert.IsNotNull(sut);

            Assert.IsTrue(PluginManagerService.HasInitialised);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
		public void ConfigureService_InvalidParamNull_Throws_ArgumentNullException()
        {
			Startup.ConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
		public void ConfigureServices_EnableEndpointRoutingIsFalse_Success()
        {
            MockServiceCollection serviceCollection = new MockServiceCollection();
			Startup.ConfigureServices(serviceCollection);

            Assert.IsTrue(serviceCollection.HasMvcConfigured());
            Assert.IsFalse(serviceCollection.HasMvcEndpointRouting());
            Assert.IsTrue(serviceCollection.HasSessionStateTempDataProvider());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Configure_InvalidParamNull_Throws_ArgumentNullException()
        {
			Startup.Configure(null);
        }
    }
}
