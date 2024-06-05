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
 *  File: SimpleDBHelpersTests.cs
 *
 *  Purpose:  Tests for SimpleDB Helper Methods
 *
 *  Date        Name                Reason
 *  14/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;
using PluginManager.Tests.Mocks;

using SimpleDB.Internal;

namespace SimpleDB.Tests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class SimpleDBHelpersTests
	{
		[TestMethod]
		public void AddSimpleDB_RegistersServices_UsesSettingsFromISettingsProvider_Success()
		{
			ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
			{
				new(typeof(ISettingsProvider), new MockSettingsProvider()),
			};

			MockServiceCollection mockServiceCollection = new(serviceDescriptors);

			MockServiceCollection Result = SimpleDBHelper.AddSimpleDB(mockServiceCollection) as MockServiceCollection;
			Assert.AreEqual(4, Result.ServicesRegistered);
			Assert.AreSame(Result, mockServiceCollection);
			Assert.IsTrue(Result.HasServiceRegistered<IForeignKeyManager>(ServiceLifetime.Singleton));
			Assert.IsTrue(Result.HasServiceRegistered<ISimpleDBManager>(ServiceLifetime.Singleton));
			Assert.IsTrue(Result.HasServiceRegistered(ServiceLifetime.Singleton, typeof(ISimpleDBOperations<>)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddSimpleDB_RegistersServices_PathDoesNotExist_Throws_ArgumentException()
		{
			string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());

			ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
			{
				new(typeof(ISettingsProvider), new MockSettingsProvider()),
			};

			MockServiceCollection mockServiceCollection = new(serviceDescriptors);

			MockServiceCollection Result = SimpleDBHelper.AddSimpleDB(mockServiceCollection, directory, "EncKey") as MockServiceCollection;
			Assert.AreEqual(3, Result.ServicesRegistered);
			Assert.AreSame(Result, mockServiceCollection);
			Assert.IsTrue(Result.HasServiceRegistered<IForeignKeyManager>(ServiceLifetime.Singleton));
			Assert.IsTrue(Result.HasServiceRegistered<ISimpleDBManager>(ServiceLifetime.Singleton));
			Assert.IsTrue(Result.HasServiceRegistered(ServiceLifetime.Singleton, typeof(ISimpleDBOperations<>)));
		}

		[TestMethod]
		public void AddSimpleDB_RegistersServices_UsesProvidesPathAndEncryptionKey_Success()
		{
			string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
			try
			{
				Directory.CreateDirectory(directory);
				ServiceDescriptor[] serviceDescriptors = new ServiceDescriptor[]
				{
					new(typeof(ISettingsProvider), new MockSettingsProvider()),
				};

				MockServiceCollection mockServiceCollection = new(serviceDescriptors);

				MockServiceCollection Result = SimpleDBHelper.AddSimpleDB(mockServiceCollection, directory, "EncKey") as MockServiceCollection;
				Assert.AreEqual(4, Result.ServicesRegistered);
				Assert.AreSame(Result, mockServiceCollection);
				Assert.IsTrue(Result.HasServiceRegistered<IForeignKeyManager>(ServiceLifetime.Singleton));
				Assert.IsTrue(Result.HasServiceRegistered<ISimpleDBManager>(ServiceLifetime.Singleton));
				Assert.IsTrue(Result.HasServiceRegistered(ServiceLifetime.Singleton, typeof(ISimpleDBOperations<>)));

				SimpleDBManager dbinitializer = mockServiceCollection.GetServiceInstance<ISimpleDBManager>(ServiceLifetime.Singleton) as SimpleDBManager;
				Assert.IsNotNull(dbinitializer);
				Assert.AreEqual(directory, dbinitializer.Path);
				Assert.AreEqual("EncKey", dbinitializer.EncryptionKey);
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}
