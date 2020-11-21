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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager;
using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

namespace AspNetCore.PluginManager.Tests.PluginServices
{
    [TestClass]
    public class PluginManagerServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConfigureServicesInvalidParam()
        {
            PluginManagerService.ConfigureServices(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FinaliseWithoutInitialiseFailWithException()
        {
            PluginManagerService.Finalise();
        }

        [TestMethod]
        public void InitialiseWithoutParametersAndFinalise()
        {
            PluginManagerService.Initialise();
            PluginManagerService.Finalise();
        }


        [TestMethod]
        [ExpectedExceptionAttribute(typeof(ArgumentNullException))]
        public void InitialiseWithNullParametersRaiseException()
        {
            PluginManagerService.Initialise(null);
        }

        [TestMethod]
        public void InitialiseWithDefaultParameters()
        {
            PluginManagerService.Initialise(new PluginManagerConfiguration());
        }

        [TestMethod]
        public void InitialiseWithCustomILogger()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration configuration = new PluginManagerConfiguration(testLogger);

            PluginManagerService.Initialise(configuration);

            ILogger pluginManagerLogger = PluginManagerService.GetLogger();

            Assert.AreNotEqual(pluginManagerLogger.GetType().TypeHandle.Value, testLogger.GetType().TypeHandle.Value);
        }
    }
}
