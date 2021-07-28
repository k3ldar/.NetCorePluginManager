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
 *  29/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AppSettings;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin;
using DynamicContent.Plugin.Internal;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.DynamicContent;

using PluginManager.Abstractions;
using PluginManager.Internal;
using PluginManager.Tests.Mocks;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.DynamicContentTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string GeneralTestsCategory = "Dynamic Content General Tests";

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PluginInitialization_InvalidParam_Null_Throws_ArgumentNullException()
        {
            PluginInitialisation sut = new PluginInitialisation(null);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void GetVersion_ReturnsCurrentVersion_Success()
        {
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());

            Assert.AreEqual((ushort)1, sut.GetVersion());
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void Initialize_DoesNotAddItemsToLogger()
        {
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());
            TestLogger testLogger = new TestLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void AfterConfigure_DoesNotConfigurePipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void Configure_DoesNotConfigurePipeline_Success()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void BeforeConfigure_DoesNotRegisterApplicationServices()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void Configure_DoesNotRegisterApplicationServices()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void Finalise_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());

            sut.Finalise();
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void BeforeConfigureServices_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.BeforeConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.Count);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void ConfigureServices_DoesNotThrowException()
        {
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.Count);
        }

        [TestMethod]
        [TestCategory(GeneralTestsCategory)]
        public void AfterConfigureServices_CreatesContentEditorPolicy_Success()
        {
                ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
                {
                    new ServiceDescriptor(typeof(INotificationService), new TestNotificationService()),
                    new ServiceDescriptor(typeof(IPluginClassesService), new TestPluginClassesService()),
                    new ServiceDescriptor(typeof(ISettingsProvider), new TestSettingsProvider("{}")),
                };

                TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
                PluginInitialisation sut = new PluginInitialisation(new TestThreadManagerServices());
                MockServiceCollection mockServiceCollection = new MockServiceCollection(serviceDescriptors);

                sut.AfterConfigureServices(mockServiceCollection);

                Assert.IsTrue(mockServiceCollection.HasServiceRegistered<IDynamicContentProvider>(ServiceLifetime.Singleton));

                string[] claims = { "ManageContent", "StaffMember", "Name", "UserId", "Email" };
                Assert.IsTrue(mockServiceCollection.HasPolicyConfigured("ContentEditor", claims));
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

            TestThreadManagerServices tmServices = new TestThreadManagerServices();
            TestApplicationBuilder testApplicationBuilder = new TestApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation(tmServices);
            MockServiceCollection mockServiceCollection = new MockServiceCollection(serviceDescriptors);

            sut.AfterConfigureServices(mockServiceCollection);

            Assert.IsTrue(tmServices.ContainsRegisteredStartupThread("DynamicContentThreadManager", typeof(DynamicContentThreadManager)));
        }
    }
}
