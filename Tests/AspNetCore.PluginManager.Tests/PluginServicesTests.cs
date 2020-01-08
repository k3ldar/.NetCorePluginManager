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
 *  File: PluginServicesTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  28/04/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager;
using PluginManager.Abstractions;

namespace AspNetCore.PluginManager.Tests
{
#if DEBUG
    [TestClass]
    public class PluginServicesTests
    {
        [TestInitialize]
        public void TestInitialise()
        {
            PluginManagerService.Initialise();
        }

        [TestMethod]
        public void TestAddAssembly()
        {
            IPluginHelperService pluginServices = UnitTestHelper.GetPluginServices();

            Assembly current = Assembly.GetExecutingAssembly();

            DynamicLoadResult loadResult = pluginServices.AddAssembly(current);

            Assert.IsTrue(loadResult == DynamicLoadResult.Success);

            pluginServices.PluginLoaded(System.IO.Path.GetFileName(current.Location), out int version);

            Assert.IsTrue(version == 1);
        }

        [TestMethod]
        public void TestAddAssemblyTwice()
        {
            IPluginHelperService pluginServices = UnitTestHelper.GetPluginServices();

            Assembly current = Assembly.GetExecutingAssembly();

            DynamicLoadResult loadResult = pluginServices.AddAssembly(current);

            Assert.IsTrue(loadResult == DynamicLoadResult.Success);

            pluginServices.PluginLoaded(System.IO.Path.GetFileName(current.Location), out int version);

            Assert.IsTrue(version == 1);

            loadResult = pluginServices.AddAssembly(current);

            Assert.IsTrue(loadResult == DynamicLoadResult.AlreadyLoaded);
        }
    }
#endif
}
