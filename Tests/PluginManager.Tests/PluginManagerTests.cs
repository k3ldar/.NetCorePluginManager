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
using System.IO;
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
        [TestInitialize]
        public void StartTest()
        {
            //plugin test dir
            string path = Path.Combine(Path.GetTempPath(), "plugintest");

            if (Directory.Exists(path))
                Directory.Delete(path, true);
        }

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
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamConfiguration_Null_Throws_ArgumentNullException()
        {
            PluginSettings pluginSettings = new PluginSettings();
            new TestPluginManager(null, pluginSettings);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParamPluginSettings_Null_Throws_ArgumentNullException()
        {
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration();
            new TestPluginManager(pluginManagerConfiguration, null);
        }

        [TestMethod]
        public void Construct_CustomServiceConfigurator_SuccesfullyRegistersWithPlugin()
        {
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration()
            {
                ServiceConfigurator = new MockServiceConfigurator()
            };

            PluginSettings pluginSettings = new PluginSettings();

            TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings);

            Assert.AreEqual(pluginManagerConfiguration.ServiceConfigurator, sut.GetServiceConfigurator());
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetServiceConfigurator_ServiceConfiguratorNull_Throws_ArgumentNullException()
        {
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration();

            PluginSettings pluginSettings = new PluginSettings();

            TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings);

            sut.TestSetServiceConfigurator(null);
        }

        [TestMethod]
        public void SetServiceConfigurator_ServiceConfiguratorAlreadyRegisterd_Throws_InvalidOperationException()
        {
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration()
            {
                ServiceConfigurator = new MockServiceConfigurator()
            };

            PluginSettings pluginSettings = new PluginSettings();

            TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings);

            try
            {
                sut.TestSetServiceConfigurator(pluginManagerConfiguration.ServiceConfigurator);
            }
            catch (InvalidOperationException ioe)
            {
                Assert.AreEqual("Only one IServiceConfigurator can be loaded", ioe.Message);
                return;
            }

            throw new Exception("Did not throw invalidoperation exception as expected");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConfigurServices_InvalidParamServices_Null_Throws_ArgumentNullException()
        {
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration();

            PluginSettings pluginSettings = new PluginSettings();

            TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings);

            sut.ConfigureServices(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddAssembly_InvalidParamAssembly_Null_Throws_ArgumentNullException()
        {
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration();

            PluginSettings pluginSettings = new PluginSettings();

            TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings);

            sut.AddAssembly(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LoadAssembly_InvalidParamAssemblyName_Null_Throws_ArgumentNullException()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger);


            PluginSettings pluginSettings = new PluginSettings()
            {
                PluginFiles = new List<string>()
            };


            TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings);

            sut.PluginLoad(null, false);
        }

        [TestMethod]
        public void SetServiceConfigurator_ServiceConfiguratorAlreadyRegisterdServices_Throws_InvalidOperationException()
        {
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration();

            PluginSettings pluginSettings = new PluginSettings();

            TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings);

            sut.TestSetServiceConfigurator(new MockServiceConfigurator());
            sut.ConfigureServices(new MockServiceCollection());

            try
            {
                sut.TestSetServiceConfigurator(new MockServiceConfigurator());
            }
            catch (InvalidOperationException ioe)
            {
                Assert.AreEqual("The plugin manager has already configured its services", ioe.Message);
                return;
            }

            throw new Exception("Did not throw invalidoperation exception as expected");
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
        public void PluginLoaded_PluginNotFound_Returns_False()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                bool pluginLoaded = pluginManager.PluginLoaded("no.such.library.dll", out int _, out string _);

                Assert.IsFalse(pluginLoaded);
            }
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
        public void AddAssembly_PluginAlreadyLoaded_Returns_DynamicLoadResultAlreadyLoaded()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));

                DynamicLoadResult loadResult = pluginManager.AddAssembly(Assembly.LoadFrom("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll"));

                Assert.AreEqual(DynamicLoadResult.AlreadyLoaded, loadResult);
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PluginLoad_InvalidParamAssembly_Throws_ArgumentNullException()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad(null, "", false);
            }
        }

        [TestMethod]
        public void PluginLoad_AssemblyAlreadyLoaded_ReturnsWithoutError()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager sut = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(sut.PluginsGetLoaded().Count, 1);

                Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

                sut.PluginLoad(pluginAssembly, "", false);
            }
        }

        [TestMethod]
        public void PluginLoad_PluginManagerDisabled_LogsMessageAndReturns()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger);
            PluginSettings pluginSettings = new PluginSettings()
            {
                Disabled = true
            };

            using (TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                Assert.AreEqual(sut.PluginsGetLoaded().Count, 0);

                Assert.AreEqual(2, testLogger.Logs.Count);
                Assembly pluginAssembly = Assembly.LoadFrom("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll");

                sut.PluginLoad(pluginAssembly, "", false);

                Assert.IsTrue(testLogger.ContainsMessage("Warning PluginManager is disabled"));
                Assert.AreEqual(3, testLogger.Logs.Count);
            }
        }

        [TestMethod]
        public void PluginLoad_PluginIsDisabled_LogsMessageAndReturns()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger);
            PluginSettings pluginSettings = new PluginSettings()
            {
                Disabled = false
            };

            PluginSetting badEggSetting = new PluginSetting("Badegg.Plugin.dll")
            {
                Disabled = true
            };

            pluginSettings.Plugins.Add(badEggSetting);

            using (TestPluginManager sut = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                Assert.AreEqual(sut.PluginsGetLoaded().Count, 1);

                Assert.AreEqual(2, testLogger.Logs.Count);
                Assembly pluginAssembly = Assembly.LoadFrom("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll");

                sut.PluginLoad(pluginAssembly, "", false);

                Assert.IsTrue(testLogger.ContainsMessage("Warning Badegg.Plugin.dll PluginManager is disabled"));
                Assert.AreEqual(3, testLogger.Logs.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PluginLoad_InvalidPluginName_Null_DoNotCopyLocal_Throws_ArgumentNullException()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager sut = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(sut.PluginsGetLoaded().Count, 1);

                Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

                sut.PluginLoad(null, false);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PluginLoad_InvalidPluginName_EmptyString_DoNotCopyLocal_Throws_ArgumentNullException()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager sut = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(sut.PluginsGetLoaded().Count, 1);

                Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

                sut.PluginLoad("", false);
            }
        }

        [TestMethod]
        public void PluginLoad_PluginSettingDisabled_DoesNoLoad()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger);
            PluginSettings pluginSettings = new PluginSettings();
            
            PluginSetting badEggSetting = new PluginSetting("BadEgg.Plugin.dll")
            {
                Disabled = true
            };

            pluginSettings.Plugins.Add(badEggSetting);

            using (TestPluginManager pluginManager = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);
                Assert.IsFalse(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
            }
        }

        [TestMethod]
        public void PluginLoad_PluginSettingNotFound_PluginLoads()
        {
            TestLogger testLogger = new TestLogger();

            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", false);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
            }
        }

        [TestMethod]
        public void PluginLoad_PluginSettingNotFound_GetLocalCopyOfPluginToLoad_PluginLoads()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger);
            PluginSettings pluginSettings = new PluginSettings()
            {
                LocalCopyPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
            };

            using (TestPluginManager pluginManager = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", true);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
            }

            Assert.IsTrue(File.Exists(Path.Combine(pluginSettings.LocalCopyPath, "BadEgg.Plugin.dll")));
            Directory.Delete(pluginSettings.LocalCopyPath, true);
        }

        [TestMethod]
        public void PluginLoad_PluginSettingNotFound_GetLocalCopyOfPluginToLoad_PluginAlreadyExists_PluginLoads()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger);
            PluginSettings pluginSettings = new PluginSettings()
            {
                LocalCopyPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
            };

            if (!Directory.Exists(pluginSettings.LocalCopyPath))
                Directory.CreateDirectory(pluginSettings.LocalCopyPath);

            File.Copy("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", Path.Combine(pluginSettings.LocalCopyPath, "BadEgg.Plugin.dll"));

            using (TestPluginManager pluginManager = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.PluginLoad("..\\..\\..\\..\\..\\Plugins\\BadEgg.Plugin\\bin\\Debug\\netcoreapp3.1\\BadEgg.Plugin.dll", true);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
            }
        }

        [TestMethod]
        public void PluginLoad_InvalidPluginName_DoesNotExist_DoNotCopyLocal_LogsError()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager sut = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(sut.PluginsGetLoaded().Count, 1);

                Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

                sut.PluginLoad("c:\\plugin.asdf.qwer.dll", false);
            }

            Assert.IsTrue(testLogger.ContainsMessage("PluginLoadError  Assembly file not found: pluginName c:\\plugin.asdf.qwer.dllPluginLoad"));
        }

        [TestMethod]
        public void PluginLoad_RetrieveAllResources_Success()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger)
            {
                CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
            };

            PluginSettings pluginSettings = new PluginSettings();

            PluginSetting companySetting = new PluginSetting("Company.Plugin.dll")
            {
                Disabled = false,
            };

            pluginSettings.Plugins.Add(companySetting);

            using (TestPluginManager pluginManager = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                pluginManager.TestCanExtractResources = true;
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                Assembly companyPlugin = Assembly.LoadFrom("..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\Company.Plugin.dll");
                pluginManager.PluginLoad(companyPlugin,
                    "..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\", true);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
                Assert.IsTrue(File.Exists(Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml")));
            }
        }

        [TestMethod]
        public void PluginLoad_RetrieveAllResources_DoesNotReplaceExisting_Success()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger)
            {
                CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
            };

            string viewNotToBeReplaced = Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml");

            if (!Directory.Exists(Path.GetDirectoryName(viewNotToBeReplaced)))
                Directory.CreateDirectory(Path.GetDirectoryName(viewNotToBeReplaced));

            File.WriteAllText(viewNotToBeReplaced, "Do not replace");

            PluginSettings pluginSettings = new PluginSettings();

            PluginSetting companySetting = new PluginSetting("Company.Plugin.dll")
            {
                Disabled = false,
                ReplaceExistingResources = false
            };

            pluginSettings.Plugins.Add(companySetting);

            using (TestPluginManager pluginManager = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                pluginManager.TestCanExtractResources = true;
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                Assembly companyPlugin = Assembly.LoadFrom("..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\Company.Plugin.dll");
                pluginManager.PluginLoad(companyPlugin,
                    "..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\", true);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
                Assert.IsTrue(File.Exists(viewNotToBeReplaced));
                Assert.AreEqual("Do not replace", File.ReadAllText(viewNotToBeReplaced));
            }
        }

        [TestMethod]
        public void PluginLoad_RetrieveAllResources_ReplaceExisting_Success()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger)
            {
                CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
            };

            string viewNotToBeReplaced = Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml");

            if (!Directory.Exists(Path.GetDirectoryName(viewNotToBeReplaced)))
                Directory.CreateDirectory(Path.GetDirectoryName(viewNotToBeReplaced));

            File.WriteAllText(viewNotToBeReplaced, "Do not replace");

            PluginSettings pluginSettings = new PluginSettings();

            PluginSetting companySetting = new PluginSetting("Company.Plugin.dll")
            {
                Disabled = false,
                ReplaceExistingResources = true
            };

            pluginSettings.Plugins.Add(companySetting);

            using (TestPluginManager pluginManager = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                pluginManager.TestCanExtractResources = true;
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                Assembly companyPlugin = Assembly.LoadFrom("..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\Company.Plugin.dll");
                pluginManager.PluginLoad(companyPlugin,
                    "..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\", true);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
                Assert.IsTrue(File.Exists(viewNotToBeReplaced));
                Assert.AreNotEqual("Do not replace", File.ReadAllText(viewNotToBeReplaced));
            }
        }

        [TestMethod]
        public void PluginLoad_RetrieveAllResources_PluginManagerPreventsExtraction_Success()
        {
            TestLogger testLogger = new TestLogger();
            PluginManagerConfiguration pluginManagerConfiguration = new PluginManagerConfiguration(testLogger)
            {
                CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
            };

            string viewNotToBeReplaced = Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml");

            if (!Directory.Exists(Path.GetDirectoryName(viewNotToBeReplaced)))
                Directory.CreateDirectory(Path.GetDirectoryName(viewNotToBeReplaced));

            File.WriteAllText(viewNotToBeReplaced, "Do not replace");

            PluginSettings pluginSettings = new PluginSettings();

            PluginSetting companySetting = new PluginSetting("Company.Plugin.dll")
            {
                Disabled = false,
                ReplaceExistingResources = true
            };

            pluginSettings.Plugins.Add(companySetting);

            using (TestPluginManager pluginManager = new TestPluginManager(pluginManagerConfiguration, pluginSettings))
            {
                pluginManager.TestCanExtractResources = false;
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                Assembly companyPlugin = Assembly.LoadFrom("..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\Company.Plugin.dll");
                pluginManager.PluginLoad(companyPlugin,
                    "..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\netcoreapp3.1\\", true);

                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 2);
                Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
                Assert.IsTrue(File.Exists(viewNotToBeReplaced));
                Assert.AreEqual("Do not replace", File.ReadAllText(viewNotToBeReplaced));
            }
        }

        [TestMethod]
        public void PluginGetClassTypes_CreatePluginLoadSelf_FindClassDescendingFromClass()
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
        public void PluginGetClassTypes_CreatePluginLoadSelfFindInterfaceDescendents()
        {
            TestLogger testLogger = new TestLogger();
            using (TestPluginManager pluginManager = new TestPluginManager(testLogger))
            {
                Assert.AreEqual(pluginManager.PluginsGetLoaded().Count, 1);

                pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
                pluginManager.ConfigureServices();

                List<Type> classes = pluginManager.PluginGetClassTypes<IPlugin>();
                Assert.AreEqual(classes.Count, 3);
            }
        }

        [TestMethod]
        public void PluginGetClasses_CreatePluginEnsureINotificationServiceRegistered()
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
