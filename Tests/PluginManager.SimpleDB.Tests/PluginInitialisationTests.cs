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
 *  Product:  SimpleDB.Tests
 *  
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  PluginInitialisation Tests for SimpleDB
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using SharedPluginFeatures;
using PluginManager.Tests.Mocks;

namespace SimpleDB.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class PluginInitialisationTests
	{
		private const string GeneralTestsCategory = "Text File DAL";

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void ExtendsIPluginAndIInitialiseEvents()
		{
			PluginInitialisation sut = new();

			Assert.IsInstanceOfType(sut, typeof(IPlugin));
			Assert.IsInstanceOfType(sut, typeof(IInitialiseEvents));
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void GetVersion_ReturnsCurrentVersion_Success()
		{
			PluginInitialisation sut = new();

			Assert.AreEqual((ushort)1, sut.GetVersion());
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void Initialize_DoesNotAddItemsToLogger()
		{
			PluginInitialisation sut = new();
			MockLogger testLogger = new();
			sut.Initialise(testLogger);

			Assert.AreEqual(0, testLogger.Logs.Count);
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void AfterConfigure_DoesNotConfigurePipeline_Success()
		{
			MockApplicationBuilder testApplicationBuilder = new();
			PluginInitialisation sut = new();

			sut.AfterConfigure(testApplicationBuilder);

			Assert.IsFalse(testApplicationBuilder.UseCalled);
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void Configure_DoesNotConfigurePipeline_Success()
		{
			MockApplicationBuilder testApplicationBuilder = new();
			PluginInitialisation sut = new();

			sut.Configure(testApplicationBuilder);

			Assert.IsFalse(testApplicationBuilder.UseCalled);
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void BeforeConfigure_DoesNotRegisterApplicationServices()
		{
			MockApplicationBuilder testApplicationBuilder = new();
			PluginInitialisation sut = new();

			sut.BeforeConfigure(testApplicationBuilder);

			Assert.IsFalse(testApplicationBuilder.UseCalled);
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void Configure_DoesNotRegisterApplicationServices()
		{
			MockApplicationBuilder applicationBuilder = new();
			PluginInitialisation sut = new();

			sut.Configure(applicationBuilder);

			Assert.IsFalse(applicationBuilder.UseCalled);
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void Finalise_DoesNotThrowException()
		{
			PluginInitialisation sut = new();
			Assert.IsNotNull(sut);

			sut.Finalise();
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void BeforeConfigureServices_RegistersCorrectServices_Success()
		{
			const int RegisteredService = 4;

			PluginInitialisation sut = new();
			MockServiceCollection mockServiceCollection = [];

			sut.BeforeConfigureServices(mockServiceCollection);

			Assert.AreEqual(RegisteredService, mockServiceCollection.Count);
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void ConfigureServices_DoesNotThrowException()
		{
			PluginInitialisation sut = new();
			MockServiceCollection mockServiceCollection = [];

			sut.ConfigureServices(mockServiceCollection);

			Assert.AreEqual(0, mockServiceCollection.Count);
		}

		[TestMethod]
		[TestCategory(GeneralTestsCategory)]
		public void AfterConfigureServices_DoesNotThrowException_Success()
		{
			PluginInitialisation sut = new();
			MockServiceCollection mockServiceCollection = [];

			sut.AfterConfigureServices(mockServiceCollection);

			Assert.AreEqual(0, mockServiceCollection.Count);
		}
	}
}
