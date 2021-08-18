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
 *  Purpose:  Tests for dynamic content plugin initialisation
 *
 *  Date        Name                Reason
 *  08/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using LoginPlugin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.LoginTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string GeneralTestsCategory = "Login";

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void GetVersion_ReturnsCurrentVersion_Success()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.AreEqual((ushort)1, sut.GetVersion());
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void Initialize_DoesNotAddItemsToLogger()
        {
            PluginInitialisation sut = new PluginInitialisation();
            TestLogger testLogger = new TestLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void AfterConfigure_DoesNotConfigurePipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void Configure_ConfiguresPipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsTrue(testApplicationBuilder.UseCalled);
            Assert.AreEqual(1, testApplicationBuilder.UseCalledCount);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void BeforeConfigure_DoesNotRegisterApplicationServices()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void Finalise_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Finalise();
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void BeforeConfigureServices_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.BeforeConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.Count);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void ConfigureServices_Registers_LoginMiddleware()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);
            Assert.IsFalse(testApplicationBuilder.UseCalled);
            Assert.AreEqual(0, testApplicationBuilder.UseCalledCount);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void AfterConfigureServices_DoesNotRegisterServices()
        {
            ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
            {
                new ServiceDescriptor(typeof(INotificationService), new TestNotificationService()),
                new ServiceDescriptor(typeof(IPluginClassesService), new TestPluginClassesService()),
                new ServiceDescriptor(typeof(ISettingsProvider), new TestSettingsProvider("{}")),
            };

            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection(serviceDescriptors);

            sut.AfterConfigureServices(mockServiceCollection);

            Assert.AreEqual(3, mockServiceCollection.Count);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void AfterConfigureServices_ValidateDynamicContentThreadRunning_Success()
        {
            ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
            {
                new ServiceDescriptor(typeof(INotificationService), new TestNotificationService()),
                new ServiceDescriptor(typeof(IPluginClassesService), new TestPluginClassesService()),
                new ServiceDescriptor(typeof(ISettingsProvider), new TestSettingsProvider("{}")),
            };

            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection(serviceDescriptors);

            sut.AfterConfigureServices(mockServiceCollection);
        }
    }
}
