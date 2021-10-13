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
 *  Product:  PluginManager.Tests
 *  
 *  File: PluginServiceProviderTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  17/08/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
#pragma warning disable CA1707 // Identifiers should not contain underscores
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class PluginServiceProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddTwoIServiceConfiguration_ThrowsInvalidOperationException()
        {
            using (MockPluginManager pluginManager = new MockPluginManager())
            {
                MockServiceConfigurator serviceConfigurator = new MockServiceConfigurator();

                pluginManager.RegisterServiceConfigurator(serviceConfigurator);

                try
                {
                    pluginManager.RegisterServiceConfigurator(serviceConfigurator);
                }
                catch (InvalidOperationException ioe)
                {
                    Assert.AreEqual("Only one IServiceConfigurator can be loaded", ioe.Message);
                    throw;
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNullServiceConfiguration_ThrowsArgumentNullException()
        {
            using (MockPluginManager pluginManager = new MockPluginManager())
            {
                pluginManager.RegisterServiceConfigurator(null);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ServiceConfigurationIsCalled_AfterServicesHaveBeenConfigured_ThrowsInvalidOperationException()
        {
            using (MockPluginManager pluginManager = new MockPluginManager())
            {
                MockServiceConfigurator serviceConfigurator = new MockServiceConfigurator();

                pluginManager.RegisterServiceConfigurator(serviceConfigurator);

                pluginManager.ConfigureServices();

                Assert.IsTrue(serviceConfigurator.RegisterServicesCalled);

                try
                {
                    pluginManager.RegisterServiceConfigurator(serviceConfigurator);
                }
                catch (InvalidOperationException ioe)
                {
                    Assert.AreEqual("The plugin manager has already configured its services", ioe.Message);
                    throw;
                }
            }
        }
    }
#pragma warning restore CA1707 // Identifiers should not contain underscores
}
