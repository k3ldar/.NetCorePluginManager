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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
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
using System.IO;
using System.Reflection;

using AspNetCore.PluginManager.Tests.Shared;

using Localization.Plugin;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.LocalizationTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string TestsCategory = "Localization General Tests";

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

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsTrue(testApplicationBuilder.UseCalled);
            Assert.AreEqual(2, testApplicationBuilder.UseCalledCount);
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
        public void Configure_DoesNotRegisterApplicationServices()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Finalise_DoesNotThrowException()
        {
            ThreadManager.Initialise();
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

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
        public void AfterConfigureServices_InvalidParam_Services_Null_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.AfterConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void ConfigureServices_ConfigureServices_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            MockHostEnvironment testHostEnvironment = new MockHostEnvironment(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            MockServiceCollection mockServiceCollection = new MockServiceCollection();
            mockServiceCollection.Add(new ServiceDescriptor(typeof(IHostEnvironment), testHostEnvironment));
            sut.ConfigureServices(mockServiceCollection);

            Assert.IsTrue(mockServiceCollection.HasServiceRegistered<ICultureProvider>(ServiceLifetime.Singleton));
            Assert.IsTrue(mockServiceCollection.HasServiceRegistered<IStringLocalizerFactory>(ServiceLifetime.Singleton));
            Assert.IsTrue(mockServiceCollection.HasServiceRegistered<IConfigureOptions<RequestLocalizationOptions>>(ServiceLifetime.Singleton));

            IServiceProvider serviceProvider = mockServiceCollection.BuildServiceProvider();

            var optionsSnapshot = serviceProvider.GetService(typeof(IOptionsSnapshot<RequestLocalizationOptions>));

            Assert.IsNotNull(optionsSnapshot);

            RequestLocalizationOptions localizationOptions = ((OptionsManager<RequestLocalizationOptions>)optionsSnapshot).Value;

            Assert.IsNotNull(localizationOptions);
            Assert.AreEqual(16, localizationOptions.SupportedCultures.Count);
            Assert.AreEqual(16, localizationOptions.SupportedUICultures.Count);
            Assert.AreEqual("en-GB", localizationOptions.DefaultRequestCulture.Culture.Name);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigureMvcBuilder_InvalidParam_Null_Throws_ArgumentNullException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.ConfigureMvcBuilder(null);
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void ConfigureMvcBuilder_AddsViewLocalization_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection serviceCollection = new MockServiceCollection();
            MockMvcBuilder mockMvcBuilder = new MockMvcBuilder(serviceCollection);

            sut.ConfigureMvcBuilder(mockMvcBuilder);

            Assert.AreEqual(13, serviceCollection.ServicesRegistered);
            Assert.IsTrue(serviceCollection.HasServiceRegistered<IHtmlLocalizerFactory>(ServiceLifetime.Singleton));
            Assert.IsTrue(serviceCollection.HasServiceRegistered<IHtmlLocalizer>(ServiceLifetime.Transient));
            Assert.IsTrue(serviceCollection.HasServiceRegistered<IConfigureOptions<MvcDataAnnotationsLocalizationOptions>>(ServiceLifetime.Transient));
            Assert.IsTrue(serviceCollection.HasConfigurationOptions(typeof(IConfigureOptions<MvcDataAnnotationsLocalizationOptions>), ServiceLifetime.Transient));
        }
    }
}
