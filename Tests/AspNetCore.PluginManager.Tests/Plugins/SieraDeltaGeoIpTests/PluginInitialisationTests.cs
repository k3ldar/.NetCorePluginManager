﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  Tests for Image Manager Plugin Initialisation
 *
 *  Date        Name                Reason
 *  18/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

using SieraDeltaGeoIp.Plugin;

namespace AspNetCore.PluginManager.Tests.Plugins.SieraDeltaGeoIpTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string TestsCategory = "SieraDeltaGeoIp General Tests";

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

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
            MockLogger testLogger = new MockLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void AfterConfigure_DoesNotConfigurePipeline_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            Dictionary<Type, object> registeredServices = new Dictionary<Type, object>();
            registeredServices.Add(typeof(INotificationService), new MockNotificationService());
            testApplicationBuilder.ApplicationServices = new MockServiceProvider(registeredServices);

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Configure_DoesNotConfigurePipeline_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
            Assert.AreEqual(0, testApplicationBuilder.UseCalledCount);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void BeforeConfigure_DoesNotRegisterApplicationServices()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Configure_DoesRegisterApplicationServices()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
            Assert.AreEqual(0, testApplicationBuilder.UseCalledCount);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Finalise_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
			Assert.IsNotNull(sut);

			sut.Finalise();
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void BeforeConfigureServices_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.BeforeConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.ServicesRegistered);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void ConfigureServices_RegistersService_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);

            Assert.AreEqual(1, mockServiceCollection.ServicesRegistered);
            Assert.IsTrue(mockServiceCollection.HasServiceRegistered<IGeoIpProvider>(ServiceLifetime.Singleton));
        }
    }
}
