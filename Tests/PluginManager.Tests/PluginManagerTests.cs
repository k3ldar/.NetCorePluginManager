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
 *  File: PluginManagerTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  15/01/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

using AppSettings;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
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
        public void CreatePluginManagerAddSinglePluginGetSettingRangeAttributeClasses()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);

                List<Type> list = pluginManager.PluginGetTypesWithAttribute<SettingRangeAttribute>();

                Assert.AreNotEqual(list.Count, 0);
            }

            Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[1].LogLevel, LogLevel.PluginConfigureError);
            Assert.AreEqual(testLogger.Logs[2].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[2].Data, "BadEgg.Plugin.dll");
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

        [TestMethod]
        public void CreatePluginAddSinglePluginConfigureCustomServices()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);

                pluginManager.ConfigureServices();
            }

            Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[1].LogLevel, LogLevel.PluginConfigureError);
            Assert.AreEqual(testLogger.Logs[2].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[2].Data, "BadEgg.Plugin.dll");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreatePluginManagerCallGetParameterInstances_ThrowsArgumentNullException()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                pluginManager.GetParameterInstances(null);
            }
        }

        [TestMethod]
        public void CreatePluginAddSinglePluginConfigureCustomServicesCreateInstanceOf()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);

                pluginManager.ConfigureServices();

                // using inbuild DI container, create a mock instance
                MockPluginHelperClass mockPluginHelper = (MockPluginHelperClass)Activator.CreateInstance(
                    typeof(MockPluginHelperClass),
                    pluginManager.GetParameterInstances(typeof(MockPluginHelperClass)));

                Assert.IsNotNull(mockPluginHelper);
            }

            Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[1].LogLevel, LogLevel.PluginConfigureError);
            Assert.AreEqual(testLogger.Logs[2].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[2].Data, "BadEgg.Plugin.dll");
        }

        [TestMethod]
        public void CreatePluginAddSinglePluginConfigureCustomServicesGetPluginClass()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                // using inbuild DI container, create a mock instance
                List<MockPluginHelperClass> mockPluginHelpers = pluginManager.PluginGetClasses<MockPluginHelperClass>();

                Assert.IsNotNull(mockPluginHelpers);

                Assert.AreNotEqual(mockPluginHelpers.Count, 0);
            }

            Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[1].LogLevel, LogLevel.PluginConfigureError);
            Assert.AreEqual(testLogger.Logs[2].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[2].Data, "BadEgg.Plugin.dll");
        }

        [TestMethod]
        public void CreatePluginAddSinglePluginensureItIsLoaded()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
            }

            Assert.AreEqual(testLogger.Logs[0].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[1].LogLevel, LogLevel.PluginConfigureError);
            Assert.AreEqual(testLogger.Logs[2].LogLevel, LogLevel.PluginLoadSuccess);
            Assert.AreEqual(testLogger.Logs[2].Data, "BadEgg.Plugin.dll");
        }

        [TestMethod]
        public void CreatePluginLoadSelfFindMockPluginHelper()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                List<Type> classes = pluginManager.PluginGetClassTypes<MockPluginHelperClass>();
                Assert.AreEqual(classes.Count, 1);
            }
        }

        [TestMethod]
        public void CreatePluginEnsureINotificationServiceRegistered()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                List<INotificationService> list = pluginManager.PluginGetClasses<INotificationService>();
                Assert.AreEqual(list.Count, 1);
            }
        }

        [TestMethod]
        public void CreatePluginEnsureIPluginClassesServiceRegistered()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                Object serviceType = pluginManager.GetServiceProvider().GetService(typeof(IPluginClassesService));
                Assert.IsNotNull(serviceType);
            }
        }

        [TestMethod]
        public void CreatePluginEnsureIPluginHelperServiceRegistered()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                Object serviceType = pluginManager.GetServiceProvider().GetService(typeof(IPluginHelperService));
                Assert.IsNotNull(serviceType);
            }
        }

        [TestMethod]
        public void CreatePluginEnsureIPluginTypesServiceRegistered()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                Object serviceType = pluginManager.GetServiceProvider().GetService(typeof(IPluginTypesService));
                Assert.IsNotNull(serviceType);
            }
        }

        [TestMethod]
        public void CreatePluginEnsureILoggerRegistered()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                Object serviceType = pluginManager.GetServiceProvider().GetService(typeof(ILogger));
                Assert.IsNotNull(serviceType);
            }
        }

        [TestMethod]
        public void CreatePluginEnsureISettingsProviderRegistered()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                Object serviceType = pluginManager.GetServiceProvider().GetService(typeof(ISettingsProvider));
                Assert.IsNotNull(serviceType);
            }
        }

        [TestMethod]
        public void CreatePluginValidateRootPath()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                string root = pluginManager.Path();

                Assert.IsFalse(String.IsNullOrWhiteSpace(root));

                string executingAssemblyPath = Assembly.GetExecutingAssembly().Location;

                Assert.IsTrue(executingAssemblyPath.StartsWith(root, StringComparison.InvariantCultureIgnoreCase));
            }
        }
    }
}
