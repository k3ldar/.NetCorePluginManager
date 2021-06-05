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
 *  Purpose:  Tests for Product Manager Plugin Initialisation
 *
 *  Date        Name                Reason
 *  31/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using ProductPlugin;
using ProductPlugin.Classes;

using SharedPluginFeatures;
using Middleware.Interfaces;
using AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests.Mocks;
using PluginManager.Internal;
using System;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string TestCategoryName = "Product Manager Tests";
        private const string DemoWebsiteImagePath = "..\\..\\..\\..\\..\\..\\.NetCorePluginManager\\Demo\\NetCorePluginDemoWebsite\\wwwroot\\images";


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetVersion_ReturnsCurrentVersion_Success()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.AreEqual((ushort)1, sut.GetVersion());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Initialize_DoesNotAddItemsToLogger()
        {
            PluginInitialisation sut = new PluginInitialisation();
            TestLogger testLogger = new TestLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AfterConfigure_DoesNotConfigurePipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Configure_DoesNotConfigurePipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void BeforeConfigure_DoesNotRegisterApplicationServices()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Configure_DoesNotRegisterApplicationServices()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Finalise_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Finalise();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void BeforeConfigureServices_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.BeforeConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.ServicesRegistered);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ConfigureServices_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.ServicesRegistered);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ConfigureServices_RegistersImageUploadNotificationListener_Success()
        {
            ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
            {
                new ServiceDescriptor(typeof(INotificationService), new NotificationService()),
                new ServiceDescriptor(typeof(IImageProvider), new MockImageProvider()),
                new ServiceDescriptor(typeof(ISettingsProvider), new TestSettingsProvider("{}")),
            };

            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection(serviceDescriptors);

            sut.AfterConfigureServices(mockServiceCollection);

            Assert.IsTrue(mockServiceCollection.HasListenerRegistered<ImageUploadNotificationListener>());
            Assert.IsTrue(mockServiceCollection.HasListenerRegisteredEvent<ImageUploadNotificationListener>("ImageUploadedEvent"));
            Assert.IsTrue(mockServiceCollection.HasListenerRegisteredEvent<ImageUploadNotificationListener>("ImageUploadOptions"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigureServices_InvalidParam_Services_Null_Throws_ArgumentNullException()
        {
            ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
            {
                new ServiceDescriptor(typeof(INotificationService), new NotificationService()),
                new ServiceDescriptor(typeof(IImageProvider), new MockImageProvider()),
                new ServiceDescriptor(typeof(ISettingsProvider), new TestSettingsProvider("{}")),
            };

            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection(serviceDescriptors);

            sut.AfterConfigureServices(null);
        }
    }
}
