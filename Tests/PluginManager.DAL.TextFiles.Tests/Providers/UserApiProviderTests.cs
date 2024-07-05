using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Accounts;

using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	public class UserApiProviderTests : BaseProviderTests
	{
		[TestMethod]
		public void AddApi_NewApiAddedSucessfully_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out PluginInitialisation pluginInitialisation, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					pluginInitialisation.AfterConfigure(new MockApplicationBuilder(provider));

					IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
					Assert.IsNotNull(accountProvider);

					bool created = accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

					Assert.IsTrue(created);
					Assert.AreEqual(2u, userId);

					IUserApiProvider sut = provider.GetService(typeof(IUserApiProvider)) as IUserApiProvider;
					Assert.IsNotNull(sut);

					Assert.IsNull(sut.GetMerchantId(userId));

					bool hasAdded = sut.AddApi(userId, "testkey", "testsecret");
					Assert.IsTrue(hasAdded);

					string merchantId = sut.GetMerchantId(userId);
					Assert.IsNotNull(merchantId);

					IUserApiQueryProvider userApiQueryProvider = provider.GetService<IUserApiQueryProvider>();
					Assert.IsNotNull(userApiQueryProvider);

					bool secretFound = userApiQueryProvider.ApiSecret(merchantId, "testkey", out string secret);
					Assert.IsTrue(secretFound);

					Assert.AreEqual("testsecret", secret);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

	}
}
