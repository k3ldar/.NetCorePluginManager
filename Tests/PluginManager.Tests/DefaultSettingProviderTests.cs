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
 *  File: DefaultSettingProviderTests.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  21/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text.Json;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;

namespace PluginManager.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class DefaultSettingProviderTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParam_Null_Throws_ArgumentNullException()
		{
			new DefaultSettingProvider(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParam_EmptyString_Throws_ArgumentNullException()
		{
			new DefaultSettingProvider("");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetSettings_InvalidParamStorage_Null_Throws_ArgumentNullException()
		{
			DefaultSettingProvider sut = new("adsf");
			sut.GetSettings<PluginSetting>(null, "name");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetSettings_InvalidParamStorage_EmptyString_Throws_ArgumentNullException()
		{
			DefaultSettingProvider sut = new("adsf");
			sut.GetSettings<PluginSetting>("", "name");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetSettings_InvalidParamSectionName_Null_Throws_ArgumentNullException()
		{
			DefaultSettingProvider sut = new("adsf");
			sut.GetSettings<PluginSetting>("myfile", null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetSettings_InvalidParamSectionName_EmptyString_Throws_ArgumentNullException()
		{
			DefaultSettingProvider sut = new("adsf");
			sut.GetSettings<PluginSetting>("myfile", "");
		}

		[TestMethod]
		public void LoadSettings_DefaultFileName_Success()
		{
			string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			int isDebugPos = path.IndexOf("\\bin\\debug\\", StringComparison.InvariantCultureIgnoreCase);

			if (isDebugPos > -1)
				path = path[..isDebugPos];

			PluginSetting testData = new("test.plugin")
			{
				Disabled = true,
				Version = "1.2.3",
				PreventExtractResources = true,
				ReplaceExistingResources = true
			};

			PluginSettings pluginSettings = new();
			pluginSettings.Plugins.Add(testData);

			string filename = Path.Combine(path, "appsettings.json");
			File.WriteAllText(filename, "{\"TestConfiguration\": " + JsonSerializer.Serialize(pluginSettings) + "}");
			try
			{
				DefaultSettingProvider sut = new(path);

				PluginSettings settings = sut.GetSettings<PluginSettings>("TestConfiguration");

				Assert.IsNotNull(settings);
				Assert.AreEqual("1.2.3", settings.Plugins[0].Version);
				Assert.IsTrue(settings.Plugins[0].Disabled);
				Assert.IsTrue(settings.Plugins[0].PreventExtractResources);
				Assert.IsTrue(settings.Plugins[0].ReplaceExistingResources);
			}
			finally
			{
				File.Delete(filename);
			}
		}

		[TestMethod]
		public void LoadSettings_UserDefinedFileName_Success()
		{
			PluginSetting testData = new("test.plugin")
			{
				Disabled = true,
				Version = "1.2.3",
				PreventExtractResources = true,
				ReplaceExistingResources = true
			};

			PluginSettings pluginSettings = new();
			pluginSettings.Plugins.Add(testData);

			string filename = Path.GetTempFileName();
			File.WriteAllText(filename, "{\"CustomConfiguration\": " + JsonSerializer.Serialize(pluginSettings) + "}");
			try
			{
				DefaultSettingProvider sut = new(Path.GetDirectoryName(filename));

				PluginSettings settings = sut.GetSettings<PluginSettings>(Path.GetFileName(filename), "CustomConfiguration");

				Assert.IsNotNull(settings);
				Assert.AreEqual("1.2.3", settings.Plugins[0].Version);
				Assert.IsTrue(settings.Plugins[0].Disabled);
				Assert.IsTrue(settings.Plugins[0].PreventExtractResources);
				Assert.IsTrue(settings.Plugins[0].ReplaceExistingResources);
			}
			finally
			{
				File.Delete(filename);
			}
		}
	}
}
