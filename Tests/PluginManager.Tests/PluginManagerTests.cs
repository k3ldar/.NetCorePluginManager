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
 *  Product:  PluginManager.Tests
 *  
 *  File: PluginManagerTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  15/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
    [TestClass]
    public class PluginManagerTests
    {
        [TestMethod]
        public void CreatePluginManagerAddNoPlugins()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);
            }

            Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[1].LogLevel, LogLevel.PluginConfigureError);
        }

        [TestMethod]
        public void CreatePluginManagerAddSinglePlugin()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
            }

            Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[1].LogLevel, LogLevel.PluginConfigureError);
            Assert.AreEqual(testLogger.Logs[2].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[2].Data, "BadEgg.Plugin.dll");
        }
    }
}
