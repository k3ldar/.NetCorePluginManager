﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
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
 *  Purpose:  Tests for product plugin startup class
 *
 *  Date        Name                Reason
 *  30/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Interfaces;

using PluginManager.Abstractions;
using PluginManager.Internal;
using PluginManager.Tests.Mocks;

using ProductPlugin;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StartupTests
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestCleanup]
        [TestCategory(TestCategoryName)]
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
            Startup sut = new Startup();

            ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
            {
                new ServiceDescriptor(typeof(INotificationService), new NotificationService()),
                new ServiceDescriptor(typeof(IImageProvider), new MockImageProvider()),
            };

            MockServiceCollection serviceCollection = new MockServiceCollection(serviceDescriptors);
            sut.ConfigureServices(serviceCollection);

            Assert.IsTrue(serviceCollection.HasMvcConfigured());
            Assert.IsFalse(serviceCollection.HasMvcEndpointRouting());
            Assert.IsTrue(serviceCollection.HasSessionStateTempDataProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Configure_InvalidParamNull_Throws_ArgumentNullException()
        {
            Startup sut = new Startup();

            sut.Configure(null);
        }
    }
}
