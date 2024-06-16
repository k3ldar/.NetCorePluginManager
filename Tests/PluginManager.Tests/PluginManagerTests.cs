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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using Shared.Classes;

#pragma warning disable S3885

namespace PluginManager.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class PluginManagerTests
	{
#if DEBUG
		private const string Build = "Debug";
#else
		private const string Build = "Release";
#endif

		private const string BadEggPluginFilePath = "BadEgg.Plugin.dll";
		private const string CompanyPluginFilePath = "Company.Plugin.dll";
		private const string CompanyPluginPath = "";

		[TestInitialize]
		public void StartTest()
		{
			ThreadManager.Initialise();

			//plugin test dir
			string path = Path.Combine(Path.GetTempPath(), "plugintest");

			if (Directory.Exists(path))
				Directory.Delete(path, true);
		}

		[TestCleanup]
		public void EndTest()
		{
			ThreadManager.CancelAll();
			ThreadManager.Finalise();
		}

		[TestMethod]
		public void CreatePluginManagerAddNoPlugins()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);
			}

			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[0].LogLevel);
			Assert.AreEqual(LogLevel.PluginConfigureError, testLogger.Logs[1].LogLevel);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParamConfiguration_Null_Throws_ArgumentNullException()
		{
			PluginSettings pluginSettings = new();
			new MockPluginManager(null, pluginSettings);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParamPluginSettings_Null_Throws_ArgumentNullException()
		{
			PluginManagerConfiguration pluginManagerConfiguration = new();
			new MockPluginManager(pluginManagerConfiguration, null);
		}

		[TestMethod]
		public void Construct_CustomServiceConfigurator_SuccesfullyRegistersWithPlugin()
		{
			PluginManagerConfiguration pluginManagerConfiguration = new()
			{
				ServiceConfigurator = new MockServiceConfigurator()
			};

			PluginSettings pluginSettings = new();

			MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings);

			Assert.AreEqual(pluginManagerConfiguration.ServiceConfigurator, sut.GetServiceConfigurator());
		}


		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetServiceConfigurator_ServiceConfiguratorNull_Throws_ArgumentNullException()
		{
			PluginManagerConfiguration pluginManagerConfiguration = new();

			PluginSettings pluginSettings = new();

			MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings);

			sut.TestSetServiceConfigurator(null);
		}

		[TestMethod]
		public void SetServiceConfigurator_ServiceConfiguratorAlreadyRegisterd_Throws_InvalidOperationException()
		{
			PluginManagerConfiguration pluginManagerConfiguration = new()
			{
				ServiceConfigurator = new MockServiceConfigurator()
			};

			PluginSettings pluginSettings = new();

			MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings);

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
			PluginManagerConfiguration pluginManagerConfiguration = new();

			PluginSettings pluginSettings = new();

			MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings);

			sut.ConfigureServices(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddAssembly_InvalidParamAssembly_Null_Throws_ArgumentNullException()
		{
			PluginManagerConfiguration pluginManagerConfiguration = new();

			PluginSettings pluginSettings = new();

			MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings);

			sut.AddAssembly(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadAssembly_InvalidParamAssemblyName_Null_Throws_ArgumentNullException()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger);


			PluginSettings pluginSettings = new()
			{
				PluginFiles = new List<string>()
			};


			MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings);

			sut.PluginLoad(null, false);
		}

		[TestMethod]
		public void SetServiceConfigurator_ServiceConfiguratorAlreadyRegisterdServices_Throws_InvalidOperationException()
		{
			PluginManagerConfiguration pluginManagerConfiguration = new();

			PluginSettings pluginSettings = new();

			MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings);

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
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);

				List<Type> list = pluginManager.PluginGetTypesWithAttribute<SettingRangeAttribute>();

				Assert.AreNotEqual(0, list.Count);
			}

			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[0].LogLevel);
			Assert.AreEqual(LogLevel.PluginConfigureError, testLogger.Logs[1].LogLevel);
			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[2].LogLevel);
			Assert.AreEqual("BadEgg.Plugin.dll", testLogger.Logs[2].Data);
		}

		[TestMethod]
		public void CreatePluginManagerAddSinglePlugin()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
			}

			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[0].LogLevel);
			Assert.AreEqual(LogLevel.PluginConfigureError, testLogger.Logs[1].LogLevel);
			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[2].LogLevel);
			Assert.AreEqual("BadEgg.Plugin.dll", testLogger.Logs[2].Data);
		}

		[TestMethod]
		public void PluginLoaded_PluginNotFound_Returns_False()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				bool pluginLoaded = pluginManager.PluginLoaded("no.such.library.dll", out int _, out string _);

				Assert.IsFalse(pluginLoaded);
			}
		}

		[TestMethod]
		public void CreatePluginAddSinglePluginConfigureCustomServices()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);

				pluginManager.ConfigureServices();
			}

			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[0].LogLevel);
			Assert.AreEqual(LogLevel.PluginConfigureError, testLogger.Logs[1].LogLevel);
			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[2].LogLevel);
			Assert.AreEqual("BadEgg.Plugin.dll", testLogger.Logs[2].Data);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreatePluginManagerCallGetParameterInstances_ThrowsArgumentNullException()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				pluginManager.GetParameterInstances(null);
			}
		}

		[TestMethod]
		public void CreatePluginAddSinglePluginConfigureCustomServicesCreateInstanceOf()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);

				pluginManager.ConfigureServices();

				// using inbuild DI container, create a mock instance
				MockPluginHelperClass mockPluginHelper = (MockPluginHelperClass)Activator.CreateInstance(
					typeof(MockPluginHelperClass),
					pluginManager.GetParameterInstances(typeof(MockPluginHelperClass)));

				Assert.IsNotNull(mockPluginHelper);
			}

			Assert.IsTrue(testLogger.ContainsMessage("PluginLoadSuccess BadEgg.Plugin.dll"));
		}

		[TestMethod]
		public void CreatePluginAddSinglePluginConfigureCustomServicesGetPluginClass()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				// using inbuild DI container, create a mock instance
				List<MockPluginHelperClass> mockPluginHelpers = pluginManager.PluginGetClasses<MockPluginHelperClass>();

				Assert.IsNotNull(mockPluginHelpers);

				Assert.AreNotEqual(0, mockPluginHelpers.Count);
			}

			Assert.IsTrue(testLogger.ContainsMessage("PluginLoadSuccess BadEgg.Plugin.dll"));
		}

		[TestMethod]
		public void CreatePluginAddSinglePluginensureItIsLoaded()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
			}

			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[0].LogLevel);
			Assert.AreEqual(LogLevel.PluginConfigureError, testLogger.Logs[1].LogLevel);
			Assert.AreEqual(LogLevel.PluginLoadSuccess, testLogger.Logs[2].LogLevel);
			Assert.AreEqual("BadEgg.Plugin.dll", testLogger.Logs[2].Data);
		}

		[TestMethod]
		public void AddAssembly_PluginAlreadyLoaded_Returns_DynamicLoadResultAlreadyLoaded()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));

				DynamicLoadResult loadResult = pluginManager.AddAssembly(Assembly.LoadFrom(BadEggPluginFilePath));

				Assert.AreEqual(DynamicLoadResult.AlreadyLoaded, loadResult);
				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PluginLoad_InvalidParamAssembly_Throws_ArgumentNullException()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(null, "", false);
			}
		}

		[TestMethod]
		public void PluginLoad_AssemblyAlreadyLoaded_ReturnsWithoutError()
		{
			MockLogger testLogger = new();
			using (MockPluginManager sut = new(testLogger))
			{
				Assert.AreEqual(1, sut.PluginsGetLoaded().Count);

				Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

				sut.PluginLoad(pluginAssembly, "", false);
			}
		}

		[TestMethod]
		public void PluginLoad_PluginManagerDisabled_LogsMessageAndReturns()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger);
			PluginSettings pluginSettings = new()
			{
				Disabled = true
			};

			using (MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings))
			{
				Assert.AreEqual(0, sut.PluginsGetLoaded().Count);

				Assert.AreEqual(2, testLogger.Logs.Count);
				Assembly pluginAssembly = Assembly.LoadFrom(BadEggPluginFilePath);

				sut.PluginLoad(pluginAssembly, "", false);

				Assert.IsTrue(testLogger.ContainsMessage("Warning PluginManager is disabled"));
				Assert.AreEqual(3, testLogger.Logs.Count);
			}
		}

		[TestMethod]
		public void PluginLoad_PluginIsDisabled_LogsMessageAndReturns()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger);
			PluginSettings pluginSettings = new()
			{
				Disabled = false
			};

			PluginSetting badEggSetting = new("Badegg.Plugin.dll")
			{
				Disabled = true
			};

			pluginSettings.Plugins.Add(badEggSetting);

			using (MockPluginManager sut = new(pluginManagerConfiguration, pluginSettings))
			{
				Assert.AreEqual(1, sut.PluginsGetLoaded().Count);

				Assert.AreEqual(2, testLogger.Logs.Count);
				Assembly pluginAssembly = Assembly.LoadFrom(BadEggPluginFilePath);

				sut.PluginLoad(pluginAssembly, "", false);

				Assert.IsTrue(testLogger.ContainsMessage("Warning Badegg.Plugin.dll PluginManager is disabled"));
				Assert.AreEqual(3, testLogger.Logs.Count);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PluginLoad_InvalidPluginName_Null_DoNotCopyLocal_Throws_ArgumentNullException()
		{
			MockLogger testLogger = new();
			using (MockPluginManager sut = new(testLogger))
			{
				Assert.AreEqual(1, sut.PluginsGetLoaded().Count);

				Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

				sut.PluginLoad(null, false);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PluginLoad_InvalidPluginName_EmptyString_DoNotCopyLocal_Throws_ArgumentNullException()
		{
			MockLogger testLogger = new();
			using (MockPluginManager sut = new(testLogger))
			{
				Assert.AreEqual(1, sut.PluginsGetLoaded().Count);

				Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

				sut.PluginLoad("", false);
			}
		}

		[TestMethod]
		public void PluginLoad_PluginSettingDisabled_DoesNoLoad()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger);
			PluginSettings pluginSettings = new();

			PluginSetting badEggSetting = new("BadEgg.Plugin.dll")
			{
				Disabled = true
			};

			pluginSettings.Plugins.Add(badEggSetting);

			using (MockPluginManager pluginManager = new(pluginManagerConfiguration, pluginSettings))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);
				Assert.IsFalse(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
			}
		}

		[TestMethod]
		public void PluginLoad_PluginSettingNotFound_PluginLoads()
		{
			MockLogger testLogger = new();

			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, false);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
			}
		}

		[TestMethod]
		public void PluginLoad_PluginSettingNotFound_GetLocalCopyOfPluginToLoad_PluginLoads()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger);
			PluginSettings pluginSettings = new()
			{
				LocalCopyPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
			};

			using (MockPluginManager pluginManager = new(pluginManagerConfiguration, pluginSettings))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, true);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
			}

			Assert.IsTrue(File.Exists(Path.Combine(pluginSettings.LocalCopyPath, "BadEgg.Plugin.dll")));
			Directory.Delete(pluginSettings.LocalCopyPath, true);
		}

		[TestMethod]
		public void PluginLoad_PluginSettingNotFound_GetLocalCopyOfPluginToLoad_PluginAlreadyExists_PluginLoads()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger);
			PluginSettings pluginSettings = new()
			{
				LocalCopyPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
			};

			if (!Directory.Exists(pluginSettings.LocalCopyPath))
				Directory.CreateDirectory(pluginSettings.LocalCopyPath);

			File.Copy(BadEggPluginFilePath, Path.Combine(pluginSettings.LocalCopyPath, "BadEgg.Plugin.dll"));

			using (MockPluginManager pluginManager = new(pluginManagerConfiguration, pluginSettings))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.PluginLoad(BadEggPluginFilePath, true);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("BadEgg.Plugin.dll", out int _, out string _));
			}
		}

		[TestMethod]
		public void PluginLoad_InvalidPluginName_DoesNotExist_DoNotCopyLocal_LogsError()
		{
			MockLogger testLogger = new();
			using (MockPluginManager sut = new(testLogger))
			{
				Assert.AreEqual(1, sut.PluginsGetLoaded().Count);

				Assembly pluginAssembly = Assembly.LoadFrom("PluginManager.dll");

				sut.PluginLoad("c:\\plugin.asdf.qwer.dll", false);
			}

			Assert.IsTrue(testLogger.ContainsMessage("PluginLoadError  Assembly file not found: pluginName c:\\plugin.asdf.qwer.dllPluginLoad"));
		}

		[TestMethod]
		public void PluginLoad_RetrieveAllResources_Success()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger)
			{
				CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
			};

			PluginSettings pluginSettings = new();

			PluginSetting companySetting = new("Company.Plugin.dll")
			{
				Disabled = false,
			};

			pluginSettings.Plugins.Add(companySetting);

			using (MockPluginManager pluginManager = new(pluginManagerConfiguration, pluginSettings))
			{
				pluginManager.TestCanExtractResources = true;
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				Assembly companyPlugin = Assembly.LoadFrom(CompanyPluginFilePath);
				pluginManager.PluginLoad(companyPlugin,
					"..\\..\\..\\..\\..\\Plugins\\Company.Plugin\\bin\\Debug\\net8.0\\", true);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
				Assert.IsTrue(File.Exists(Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml")));
			}
		}

		[TestMethod]
		public void PluginLoad_RetrieveAllResources_DoesNotReplaceExisting_Success()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger)
			{
				CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
			};

			string viewNotToBeReplaced = Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml");

			if (!Directory.Exists(Path.GetDirectoryName(viewNotToBeReplaced)))
				Directory.CreateDirectory(Path.GetDirectoryName(viewNotToBeReplaced));

			File.WriteAllText(viewNotToBeReplaced, "Do not replace");

			PluginSettings pluginSettings = new();

			PluginSetting companySetting = new("Company.Plugin.dll")
			{
				Disabled = false,
				ReplaceExistingResources = false
			};

			pluginSettings.Plugins.Add(companySetting);

			using (MockPluginManager pluginManager = new(pluginManagerConfiguration, pluginSettings))
			{
				pluginManager.TestCanExtractResources = true;
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				Assembly companyPlugin = Assembly.LoadFrom(CompanyPluginFilePath);
				pluginManager.PluginLoad(companyPlugin, CompanyPluginPath, true);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
				Assert.IsTrue(File.Exists(viewNotToBeReplaced));
				Assert.AreEqual("Do not replace", File.ReadAllText(viewNotToBeReplaced));
			}
		}

		[TestMethod]
		public void PluginLoad_RetrieveAllResources_ReplaceExisting_Success()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger)
			{
				CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
			};

			string viewNotToBeReplaced = Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml");

			if (!Directory.Exists(Path.GetDirectoryName(viewNotToBeReplaced)))
				Directory.CreateDirectory(Path.GetDirectoryName(viewNotToBeReplaced));

			File.WriteAllText(viewNotToBeReplaced, "Do not replace");

			PluginSettings pluginSettings = new();

			PluginSetting companySetting = new("Company.Plugin.dll")
			{
				Disabled = false,
				ReplaceExistingResources = true
			};

			pluginSettings.Plugins.Add(companySetting);

			using (MockPluginManager pluginManager = new(pluginManagerConfiguration, pluginSettings))
			{
				pluginManager.TestCanExtractResources = true;
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				Assembly companyPlugin = Assembly.LoadFrom(CompanyPluginFilePath);
				pluginManager.PluginLoad(companyPlugin, CompanyPluginPath, true);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
				Assert.IsTrue(File.Exists(viewNotToBeReplaced));
				Assert.AreNotEqual("Do not replace", File.ReadAllText(viewNotToBeReplaced));
			}
		}

		[TestMethod]
		public void PluginLoad_RetrieveAllResources_PluginManagerPreventsExtraction_Success()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration pluginManagerConfiguration = new(testLogger)
			{
				CurrentPath = Path.Combine(Path.GetTempPath(), "plugintest", DateTime.UtcNow.Ticks.ToString())
			};

			string viewNotToBeReplaced = Path.Combine(pluginManagerConfiguration.CurrentPath, "Views", "Company", "About.cshtml");

			if (!Directory.Exists(Path.GetDirectoryName(viewNotToBeReplaced)))
				Directory.CreateDirectory(Path.GetDirectoryName(viewNotToBeReplaced));

			File.WriteAllText(viewNotToBeReplaced, "Do not replace");

			PluginSettings pluginSettings = new();

			PluginSetting companySetting = new("Company.Plugin.dll")
			{
				Disabled = false,
				ReplaceExistingResources = true
			};

			pluginSettings.Plugins.Add(companySetting);

			using (MockPluginManager pluginManager = new(pluginManagerConfiguration, pluginSettings))
			{
				pluginManager.TestCanExtractResources = false;
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				Assembly companyPlugin = Assembly.LoadFrom(CompanyPluginFilePath);
				pluginManager.PluginLoad(companyPlugin, CompanyPluginPath, true);

				Assert.AreEqual(2, pluginManager.PluginsGetLoaded().Count);
				Assert.IsTrue(pluginManager.PluginLoaded("Company.Plugin.dll", out int _, out string _));
				Assert.IsTrue(File.Exists(viewNotToBeReplaced));
				Assert.AreEqual("Do not replace", File.ReadAllText(viewNotToBeReplaced));
			}
		}

		[TestMethod]
		public void PluginGetClassTypes_CreatePluginLoadSelf_FindClassDescendingFromClass()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				List<Type> classes = pluginManager.PluginGetClassTypes<MockPluginHelperClass>();
				Assert.AreEqual(1, classes.Count);
			}
		}

		[TestMethod]
		public void PluginGetClassTypes_CreatePluginLoadSelfFindInterfaceDescendents()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				List<Type> classes = pluginManager.PluginGetClassTypes<IPlugin>();
				Assert.AreEqual(6, classes.Count);
			}
		}

		[TestMethod]
		public void PluginGetClasses_CreatePluginEnsureINotificationServiceRegistered()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				List<INotificationService> list = pluginManager.PluginGetClasses<INotificationService>();
				Assert.AreEqual(1, list.Count);
			}
		}

		[TestMethod]
		public void CreatePluginEnsureIPluginClassesServiceRegistered()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				object serviceType = pluginManager.GetServiceProvider().GetService(typeof(IPluginClassesService));
				Assert.IsNotNull(serviceType);
			}
		}

		[TestMethod]
		public void CreatePluginEnsureIPluginHelperServiceRegistered()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				object serviceType = pluginManager.GetServiceProvider().GetService(typeof(IPluginHelperService));
				Assert.IsNotNull(serviceType);
			}
		}

		[TestMethod]
		public void CreatePluginEnsureIPluginTypesServiceRegistered()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				object serviceType = pluginManager.GetServiceProvider().GetService(typeof(IPluginTypesService));
				Assert.IsNotNull(serviceType);
			}
		}

		[TestMethod]
		public void CreatePluginEnsureILoggerRegistered()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				object serviceType = pluginManager.GetServiceProvider().GetService(typeof(ILogger));
				Assert.IsNotNull(serviceType);
			}
		}

		[TestMethod]
		public void CreatePluginEnsureISettingsProviderRegistered()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				Assert.AreEqual(1, pluginManager.PluginsGetLoaded().Count);

				pluginManager.AddAssembly(Assembly.GetExecutingAssembly());
				pluginManager.ConfigureServices();

				object serviceType = pluginManager.GetServiceProvider().GetService(typeof(ISettingsProvider));
				Assert.IsNotNull(serviceType);
			}
		}

		[TestMethod]
		public void CreatePluginValidateRootPath()
		{
			MockLogger testLogger = new();
			using (MockPluginManager pluginManager = new(testLogger))
			{
				string root = pluginManager.Path();

				Assert.IsFalse(string.IsNullOrWhiteSpace(root));

				string executingAssemblyPath = Assembly.GetExecutingAssembly().Location;

				Assert.IsTrue(executingAssemblyPath.StartsWith(root, StringComparison.InvariantCultureIgnoreCase));
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddPluginModule_InvalidParamAssemblyName_Null_Throws_ArgumentNullException()
		{
			MockLogger testLogger = new();
			MockPluginManager sut = new(testLogger);
			sut.TestAddPluginModule(null, new TestPluginModule());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddPluginModule_InvalidParamAssemblyName_EmptyString_Throws_ArgumentNullException()
		{
			MockLogger testLogger = new();
			MockPluginManager sut = new(testLogger);
			sut.TestAddPluginModule("", new TestPluginModule());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddPluginModule_InvalidParamPluginModule_Null_Throws_ArgumentNullException()
		{
			MockLogger testLogger = new();
			MockPluginManager sut = new(testLogger);
			sut.TestAddPluginModule("test", null);
		}

		[TestMethod]
		public void AddPluginModule_SuccessfullyAdds_CannotAddTwice()
		{
			MockLogger testLogger = new();
			MockPluginManager sut = new(testLogger);
			IPluginModule testPluginModule = new TestPluginModule();

			bool added = sut.TestAddPluginModule("test", testPluginModule);
			Assert.IsTrue(added);

			added = sut.TestAddPluginModule("test", testPluginModule);
			Assert.IsFalse(added);
		}

		[TestMethod]
		public void Dispose_Disposing_FinalisesPlugins_Success()
		{
			MockLogger testLogger = new();
			MockPluginManager sut = new(testLogger);
			sut.PluginLoad(typeof(TestDisposePlugin).Assembly.Location, false);

			TestDisposePlugin testDisposePlugin = new();

			TestPluginModule testPluginModule = new()
			{
				Plugin = testDisposePlugin
			};

			bool addPlugin = sut.TestAddPluginModule("test", testPluginModule);

			Assert.IsTrue(addPlugin);

			sut.TestDispose(false);

			Assert.IsTrue(testDisposePlugin.FinaliseCalled);
		}

		[TestMethod]
		public void Dispose_Disposing_FinalisesPlugins_ThrowsExceptionAndLogsError()
		{
			MockLogger testLogger = new();
			MockPluginManager sut = new(testLogger);
			sut.PluginLoad(typeof(TestDisposePlugin).Assembly.Location, false);

			TestDisposeExceptionPlugin testDisposePlugin = new();

			TestPluginModule testPluginModule = new()
			{
				Plugin = testDisposePlugin
			};

			bool addPlugin = sut.TestAddPluginModule("test", testPluginModule);

			Assert.IsTrue(addPlugin);

			sut.TestDispose(false);

			Assert.IsTrue(testLogger.ContainsMessage("Error Specified argument was out of the range of valid values. (Parameter 'raised from test plugin') testDispose"));
		}

		[TestMethod]
		public void PluginLoad_CreatesInstanceOfPlugin_ThrowsExceptionAndLogsError()
		{
			MockLogger testLogger = new();
			MockPluginManager sut = new(testLogger);
			sut.PluginLoad(typeof(TestDisposePlugin).Assembly.Location, false);

			TestDisposeExceptionPlugin testDisposePlugin = new();

			TestPluginModule testPluginModule = new()
			{
				Plugin = testDisposePlugin
			};

			bool addPlugin = sut.TestAddPluginModule("test", testPluginModule);

			Assert.IsTrue(addPlugin);

			sut.TestDispose(false);

			Assert.IsTrue(testLogger.ContainsMessage("Error Specified argument was out of the range of valid values. (Parameter 'raised from test plugin') testDispose"));
		}

		//[TestMethod]
		//public void PluginLoad_()
		//{
		//    TestLogger testLogger = new TestLogger();
		//    TestPluginManager sut = new TestPluginManager(testLogger);
		//    sut.PluginLoad(typeof(TestDisposePlugin).Assembly.Location, false);

		//    TestDisposeExceptionPlugin testDisposePlugin = new TestDisposeExceptionPlugin();

		//    TestPluginModule testPluginModule = new TestPluginModule()
		//    {
		//        Plugin = testDisposePlugin
		//    };

		//    bool addPlugin = sut.TestAddPluginModule("test", testPluginModule);

		//    Assert.IsTrue(addPlugin);

		//    sut.TestDispose(false);

		//    Assert.IsTrue(testLogger.ContainsMessage("Error Specified argument was out of the range of valid values. (Parameter 'raised from test plugin') testDispose"));
		//}
	}

	[ExcludeFromCodeCoverage]
	public class TestPluginModule : IPluginModule
	{
		public ushort Version { get; set; }

		public string Module { get; set; }

		public Assembly Assembly { get; set; }

		public IPlugin Plugin { get; set; }

		public string FileVersion { get; set; }
	}

	[ExcludeFromCodeCoverage]
	public class TestDisposePlugin : IPlugin
	{
		public void ConfigureServices(IServiceCollection services)
		{

		}

		public void Finalise()
		{
			FinaliseCalled = true;
		}

		public ushort GetVersion()
		{
			return 0;
		}

		public void Initialise(ILogger logger)
		{

		}

		public bool FinaliseCalled { get; private set; }
	}


	[ExcludeFromCodeCoverage]
	public class TestDisposeExceptionPlugin : IPlugin
	{
		public void ConfigureServices(IServiceCollection services)
		{

		}

		public void Finalise()
		{
			throw new ArgumentOutOfRangeException("raised from test plugin");
		}

		public ushort GetVersion()
		{
			return 0;
		}

		public void Initialise(ILogger logger)
		{

		}
	}

	[ExcludeFromCodeCoverage]
	public class TestPluginMinMaxVersion : IPlugin
	{
		private readonly ushort _version;

		private TestPluginMinMaxVersion()
		{

		}

		public TestPluginMinMaxVersion(ushort version)
		{
			_version = version;
		}

		public void ConfigureServices(IServiceCollection services)
		{

		}

		public void Finalise()
		{

		}

		public ushort GetVersion()
		{
			return _version;
		}

		public void Initialise(ILogger logger)
		{

		}
	}

}
#pragma warning restore S3885