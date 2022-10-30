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
 *  File: PluginManagerServiceTests.cs
 *
 *  Purpose:  Tests for PluginManagerService
 *
 *  Date        Name                Reason
 *  14/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager;
using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

namespace AspNetCore.PluginManager.Tests.AspNetCore.PluginManager
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginManagerServiceTests
    {
        private const string TestCategoryName = "AspNetCore Plugin Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureServicesInvalidParam()
        {
            PluginManagerService.ConfigureServices(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FinaliseWithoutInitialiseFailWithException()
        {
            PluginManagerService.Finalise();
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void InitialiseWithoutParametersAndFinalise()
        {
            PluginManagerService.Initialise();
            PluginManagerService.Finalise();
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedExceptionAttribute(typeof(ArgumentNullException))]
        public void InitialiseWithNullParametersRaiseException()
        {
            PluginManagerService.Initialise(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void InitialiseWithDefaultParameters()
        {
            PluginManagerService.Initialise(new PluginManagerConfiguration());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void InitialiseWithCustomILogger()
        {
            MockLogger testLogger = new MockLogger();
            PluginManagerConfiguration configuration = new PluginManagerConfiguration(testLogger);

            PluginManagerService.Initialise(configuration);

            ILogger pluginManagerLogger = PluginManagerService.GetLogger();

            Assert.AreNotEqual(pluginManagerLogger.GetType().TypeHandle.Value, testLogger.GetType().TypeHandle.Value);
        }
    }
}
