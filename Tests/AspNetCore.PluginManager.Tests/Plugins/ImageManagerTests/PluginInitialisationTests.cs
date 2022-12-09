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
 *  16/04/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using ImageManager.Plugin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Interfaces;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ImageManagerTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        private const string ImageManagerTestsCategory = "Image Manager General Tests";
        private const string DemoWebsiteImagePath = "..\\..\\..\\..\\..\\..\\.NetCorePluginManager\\Demo\\NetCorePluginDemoWebsite\\wwwroot\\images";


        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
            Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void GetVersion_ReturnsCurrentVersion_Success()
        {
            PluginInitialisation sut = new PluginInitialisation();

            Assert.AreEqual((ushort)1, sut.GetVersion());
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Initialize_DoesNotAddItemsToLogger()
        {
            PluginInitialisation sut = new PluginInitialisation();
            MockLogger testLogger = new MockLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void AfterConfigure_DoesNotConfigurePipeline_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.AfterConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Configure_DoesNotConfigurePipeline_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void BeforeConfigure_DoesNotRegisterApplicationServices()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.BeforeConfigure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void Configure_DoesNotRegisterApplicationServices()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();

            sut.Configure(testApplicationBuilder);

            Assert.IsFalse(testApplicationBuilder.UseCalled);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
		public void Finalise_DoesNotThrowException()
        {
			ThreadManager.Initialise();
			MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
			PluginInitialisation sut = new PluginInitialisation();
			Assert.IsNotNull(sut);

			sut.Finalise();
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void BeforeConfigureServices_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.BeforeConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.ServicesRegistered);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void ConfigureServices_DoesNotThrowException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);

            Assert.AreEqual(0, mockServiceCollection.ServicesRegistered);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AfterConfigureServices_InvalidParam_Services_Null_Throws_ArgumentNullException()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();
            string imageManagerSettingsJson = "{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}";
            mockServiceCollection.AddSingleton<ISettingsProvider>(new MockSettingsProvider(imageManagerSettingsJson));

            sut.AfterConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void AfterConfigureServices_CreatesContentEditorPolicy_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();
            string imageManagerSettingsJson = "{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}";
            mockServiceCollection.AddSingleton<ISettingsProvider>(new MockSettingsProvider(imageManagerSettingsJson));

            sut.AfterConfigureServices(mockServiceCollection);

            string[] claims = { "ManageImages", "StaffMember", "Name", "UserId", "Email" };
            Assert.IsTrue(mockServiceCollection.HasPolicyConfigured("ImageManager", claims));
        }

        [TestMethod]
        [TestCategory(ImageManagerTestsCategory)]
        public void AfterConfigureServices_RegistersDefaultImageProvider_Success()
        {
            MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
            PluginInitialisation sut = new PluginInitialisation();
            MockServiceCollection mockServiceCollection = new MockServiceCollection();
            string imageManagerSettingsJson = "{\"ImageManager\": {\"ImagePath\": \"" + DemoWebsiteImagePath.Replace("\\", "\\\\") + "\"}}";
            mockServiceCollection.AddSingleton<ISettingsProvider>(new MockSettingsProvider(imageManagerSettingsJson));

            sut.AfterConfigureServices(mockServiceCollection);

            Assert.IsTrue(mockServiceCollection.HasServiceRegistered<IImageProvider>(ServiceLifetime.Singleton));
        }
    }
}
