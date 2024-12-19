/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  Tests for Helpdesk Plugin Initialisation
 *
 *  Date        Name                Reason
 *  15/12/2022 Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using HelpdeskPlugin;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Interfaces;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.HelpdeskTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginInitialisationTests
    {
        [TestMethod]
        public void ExtendsIPluginAndIInitialiseEvents()
        {
            PluginInitialisation sut = new PluginInitialisation(new MockThreadManagerServices());

            Assert.IsInstanceOfType(sut, typeof(IPlugin));
        }

        [TestMethod]
        public void GetVersion_ReturnsCurrentVersion_Success()
        {
            PluginInitialisation sut = new PluginInitialisation(new MockThreadManagerServices());

            Assert.AreEqual((ushort)1, sut.GetVersion());
        }

        [TestMethod]
        public void Initialize_DoesNotAddItemsToLogger()
        {
            PluginInitialisation sut = new PluginInitialisation(new MockThreadManagerServices());
            MockLogger testLogger = new MockLogger();

            sut.Initialise(testLogger);

            Assert.AreEqual(0, testLogger.Logs.Count);
        }

        [TestMethod]
        public void ConfigureServices_ConfiguresIPop3Client_Success()
        {
			MockApplicationBuilder testApplicationBuilder = new MockApplicationBuilder();
			PluginInitialisation sut = new PluginInitialisation(new MockThreadManagerServices());
            MockServiceCollection mockServiceCollection = new MockServiceCollection();

            sut.ConfigureServices(mockServiceCollection);

            Assert.AreEqual(2, mockServiceCollection.ServicesRegistered);
        }
    }
}
