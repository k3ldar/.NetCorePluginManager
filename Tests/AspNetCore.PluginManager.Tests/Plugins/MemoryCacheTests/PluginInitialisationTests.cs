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
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  Tests for Image Manager Plugin Initialisation
 *
 *  Date        Name                Reason
 *  18/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using MemoryCache.Plugin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Abstractions;
using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.MemoryCacheTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string TestsCategory = "MemoryCache General Tests";

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void GetVersion_ReturnsCurrentVersion_Success()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.AreEqual((ushort)1, sut.GetVersion());
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Initialize_DoesNotAddItemsToLogger()
        {
            PluginInitialisation sut = new PluginInitialisation();
            TestLogger testLogger = new TestLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Finalise_DoesNotThrowException()
        {
            ThreadManager.Initialise();
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Finalise();
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigureServices_InvalidParam_Null_Throws_ArgumentNullException()
        {
            PluginInitialisation sut = new PluginInitialisation();

            sut.ConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void ConfigureServices_CallsUseMemoryCache_Success()
        {
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);

            Assert.AreEqual(1, mockServiceCollection.ServicesRegistered);
            Assert.IsTrue(mockServiceCollection.HasServiceRegistered<IMemoryCache>(ServiceLifetime.Singleton));
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Configure_InvalidParam_Null_DoesNotThrowException()
        {
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void BeforeConfigure_InvalidParam_Null_DoesNotThrowException()
        {
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void AfterConfigure_InvalidParam_Null_DoesNotThrowException()
        {
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void AfterConfigureServices_InvalidParam_Null_DoesNotThrowException()
        {
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BeforeConfigureServices_InvalidParam_Null_Throws_ArgumentNullException()
        {
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void BeforeConfigureServices_Registers_ICacheManagerFactory_Successfully()
        {
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.BeforeConfigureServices(mockServiceCollection);

            Assert.AreEqual(1, mockServiceCollection.ServicesRegistered);
            Assert.IsTrue(mockServiceCollection.HasServiceRegistered<ICacheManagerFactory>(ServiceLifetime.Scoped));

        }
    }
}
