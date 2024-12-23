﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  PluginManager.Tests
 *  
 *  File: PluginManagerConfigurationTests.cs
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Internal;
using PluginManager.Tests.Mocks;

namespace PluginManager.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class PluginManagerConfigurationTests
	{
		[TestMethod]
		public void Construct_EmptyConstructor_CreatesDefaultItems_Success()
		{
			PluginManagerConfiguration sut = new();

			Assert.IsNotNull(sut);
			Assert.IsNotNull(sut.Logger);
			Assert.IsNotNull(sut.LoadSettingsService);
		}

		[TestMethod]
		public void Construct_CustomLogger_CreatesDefaultItems_Success()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration sut = new(testLogger);

			Assert.IsNotNull(sut);
			Assert.IsNotNull(sut.Logger);
			Assert.IsNotNull(sut.LoadSettingsService);

			Assert.AreEqual(testLogger, sut.Logger);
		}

		[TestMethod]
		public void Construct_CustomLoadSettings_CreatesDefaultItems_Success()
		{
			LoadSettingsService loadSettingsService = new();

			PluginManagerConfiguration sut = new(loadSettingsService);

			Assert.IsNotNull(sut);
			Assert.IsNotNull(sut.Logger);
			Assert.IsNotNull(sut.LoadSettingsService);

			Assert.AreEqual(loadSettingsService, sut.LoadSettingsService);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParamLogger_Null_Throws_ArgumentNullException()
		{
			LoadSettingsService loadSettingsService = new();

			new PluginManagerConfiguration(null, loadSettingsService);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParamLoadSettingsService_Null_Throws_ArgumentNullException()
		{
			new PluginManagerConfiguration(new MockLogger(), null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidServiceConfigurator_Null_Throws_ArgumentNullException()
		{
			new PluginManagerConfiguration(new MockLogger(), new LoadSettingsService(), null);
		}

		[TestMethod]
		public void PathAndFileName_ContainDefaultSettings()
		{
			MockLogger testLogger = new();
			LoadSettingsService loadSettingsService = new();
			MockServiceConfigurator serviceConfigurator = new();

			PluginManagerConfiguration sut = new(testLogger, loadSettingsService, serviceConfigurator);

			Assert.IsNotNull(sut);
			Assert.IsNotNull(sut.Logger);
			Assert.IsNotNull(sut.LoadSettingsService);
			Assert.IsNotNull(sut.ServiceConfigurator);

			Assert.AreEqual(testLogger, sut.Logger);
			Assert.AreEqual(loadSettingsService, sut.LoadSettingsService);
			Assert.AreEqual(serviceConfigurator, sut.ServiceConfigurator);

			Assert.IsNotNull(sut.CurrentPath);
			Assert.IsNotNull(sut.ConfigFileName);
			Assert.IsFalse(String.IsNullOrEmpty(sut.CurrentPath));
			Assert.IsFalse(String.IsNullOrEmpty(sut.ConfigFileName));

			Assert.AreEqual(Path.Combine(sut.CurrentPath, sut.ConfigFileName), sut.ConfigurationFile);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ReplaceLogger_InvalidLogger_NullThrows()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration sut = new(testLogger);

			Assert.IsNotNull(sut);
			Assert.IsNotNull(sut.Logger);
			Assert.IsNotNull(sut.LoadSettingsService);
			Assert.AreEqual(testLogger, sut.Logger);

			sut.ReplaceLogger(null);
		}

		[TestMethod]
		public void ReplaceLogger_CreatesDefaultItems_Success()
		{
			MockLogger testLogger = new();
			PluginManagerConfiguration sut = new(testLogger);

			Assert.IsNotNull(sut);
			Assert.IsNotNull(sut.Logger);
			Assert.IsNotNull(sut.LoadSettingsService);
			Assert.AreEqual(testLogger, sut.Logger);

			MockLogger replaceMentLogger = new();
			sut.ReplaceLogger(replaceMentLogger);

			Assert.AreEqual(replaceMentLogger, sut.Logger);
			Assert.AreNotEqual(testLogger, sut.Logger);
		}
	}
}