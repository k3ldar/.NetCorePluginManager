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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: UserApiQueryProviderTests.cs
 *
 *  Purpose:  User api query provider tests for text based storage
 *
 *  Date        Name                Reason
 *  23/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Accounts;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;
using SimpleDB.Internal;
using SimpleDB.Tests.Mocks;

using Shared.Classes;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	public class UserApiQueryProviderTests : BaseProviderTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidInstance_TableUserNull_Throws_ArgumentNullException()
		{
			new UserApiQueryProvider(null, new MockMemoryCache());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidInstance_MemoryCacheNull_Throws_ArgumentNullException()
		{
			new UserApiQueryProvider(new MockTextTableOperations<UserApiDataRow>(), null);
		}

		[TestMethod]
		public void ApiSecret_InvalidParam_MerchantIDNull_Returns_False()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					IPluginClassesService pluginClassesService = new MockPluginClassesService(new List<object>() { new ExternalUsersDataRowDefaults() });

					ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);
					IForeignKeyManager keyManager = new ForeignKeyManager();

					IUserApiQueryProvider sut = provider.GetService<IUserApiQueryProvider>();
					Assert.IsNotNull(sut);

					bool result = sut.ApiSecret(null, "apikey", out string secret);
					Assert.IsFalse(result);
					Assert.AreEqual("", secret);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ApiSecret_InvalidParam_ApiKeyNull_Returns_False()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					IPluginClassesService pluginClassesService = new MockPluginClassesService(new List<object>() { new ExternalUsersDataRowDefaults() });

					ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);
					IForeignKeyManager keyManager = new ForeignKeyManager();

					IUserApiQueryProvider sut = provider.GetService<IUserApiQueryProvider>();
					Assert.IsNotNull(sut);

					bool result = sut.ApiSecret("merchant", null, out string secret);
					Assert.IsFalse(result);
					Assert.AreEqual("", secret);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ApiSecret_RetrievedFromMemoryCache_Returns_True()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					IPluginClassesService pluginClassesService = new MockPluginClassesService(new List<object>() { new ExternalUsersDataRowDefaults() });

					ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);
					IForeignKeyManager keyManager = new ForeignKeyManager();

					IMemoryCache memoryCache = provider.GetService<IMemoryCache>();
					Assert.IsNotNull(memoryCache);

					memoryCache.GetShortCache().Add("api testMerch testKey", new Shared.Classes.CacheItem("api testMerch testKey", "the secret"));

					IUserApiQueryProvider sut = provider.GetService<IUserApiQueryProvider>();
					Assert.IsNotNull(sut);

					bool result = sut.ApiSecret("testMerch", "testKey", out string secret);
					Assert.IsTrue(result);
					Assert.AreEqual("the secret", secret);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ApiSecret_NotFound_Returns_False()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					IPluginClassesService pluginClassesService = new MockPluginClassesService(new List<object>() { new ExternalUsersDataRowDefaults() });

					ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);
					IForeignKeyManager keyManager = new ForeignKeyManager();

					IUserApiQueryProvider sut = provider.GetService<IUserApiQueryProvider>();
					Assert.IsNotNull(sut);

					bool result = sut.ApiSecret("testMerch", "testKey", out string secret);
					Assert.IsFalse(result);
					Assert.AreEqual("", secret);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ApiSecret_SecretFound_AddedToCache_Returns_True()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					IPluginClassesService pluginClassesService = new MockPluginClassesService(new List<object>() { new ExternalUsersDataRowDefaults() });

					ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);

					IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

					Assert.IsNotNull(accountProvider);

					bool created = accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

					using (SimpleDBOperations<UserApiDataRow> apiTable = new(simpleDBManager, provider.GetService<IForeignKeyManager>()))
					{
						simpleDBManager.Initialize(pluginClassesService);

						Assert.IsNotNull(apiTable);

						UserApiDataRow userApiDataRow = new()
						{
							MerchantId = "TestMerch",
							ApiKey = "testKey",
							Secret = "my secret",
							UserId = userId,
						};

						apiTable.Insert(userApiDataRow);
					}

					IUserApiQueryProvider sut = provider.GetService<IUserApiQueryProvider>();
					Assert.IsNotNull(sut);

					bool result = sut.ApiSecret("testMerch", "testKey", out string secret);
					Assert.IsTrue(result);
					Assert.AreEqual("my secret", secret);

					IMemoryCache memoryCache = provider.GetService<IMemoryCache>();
					Assert.IsNotNull(memoryCache);

					CacheItem cacheItem = memoryCache.GetShortCache().Get("api testMerch testKey");
					Assert.IsNotNull(cacheItem);
					Assert.AreEqual("my secret", (string)cacheItem.Value);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}
