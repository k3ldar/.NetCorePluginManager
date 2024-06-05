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
 *  File: LoginProviderTests.cs
 *
 *  Purpose:  LoginProviderTests Tests for text based storage
 *
 *  Date        Name                Reason
 *  12/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using SimpleDB;
using SimpleDB.Internal;
using SimpleDB.Tests.Mocks;

#pragma warning disable IDE0017

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
    [ExcludeFromCodeCoverage]
    public class LoginProviderTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_TableUserNull_Throws_ArgumentNullException()
        {
            new LoginProvider(null, new MockTextTableOperations<ExternalUsersDataRow>(), new MockSettingsProvider("{ \"SimpleDBSettings\":{ \"Path\": \"c:\\temp\"} }"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_TableExternalUsersNull_Throws_ArgumentNullException()
        {
            new LoginProvider(new MockTextTableOperations<UserDataRow>(), null, new MockSettingsProvider("{ \"SimpleDBSettings\":{ \"Path\": \"c:\\temp\"} }"));
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            LoginProvider sut = new(new MockTextTableOperations<UserDataRow>(), new MockTextTableOperations<ExternalUsersDataRow>(), new MockSettingsProvider("{ \"SimpleDBSettings\":{ \"Path\": \"c:\\temp\"} }"));
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void Validate_ExternalUsersIdStartsAtInt64Minimum_Success()
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
                    using (SimpleDBOperations<ExternalUsersDataRow> sut = new(simpleDBManager, keyManager))
                    {
						simpleDBManager.Initialize(pluginClassesService);
                        Assert.IsNotNull(sut);

                        Assert.AreEqual(Int64.MinValue, sut.PrimarySequence);
                    }
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_UserFoundInExternalUserTable_Returns_Remembered()
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
                    using (SimpleDBOperations<ExternalUsersDataRow> externalUserTable = new(simpleDBManager, keyManager))
                    {
						simpleDBManager.Initialize(pluginClassesService);
						Assert.IsNotNull(externalUserTable);

                        ExternalUsersDataRow externalUser = new();
                        externalUser.Email = "test@123.net";
                        externalUser.UserName = "test user";
                        externalUser.Provider = "test provider";
                        externalUser.UserId = "provider id";

                        externalUserTable.Insert(externalUser);
                        
                    }

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new(Int64.MinValue + 1);
                    LoginResult result = sut.Login("username", "password", "10.10.10.1", 0, ref loginDetails);
                    Assert.AreEqual(LoginResult.Remembered, result);
                    Assert.AreEqual("test@123.net", loginDetails.Email);
                    Assert.AreEqual("test user", loginDetails.Username);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_WithRememberMe_UserFoundInUserTable_Returns_Remembered()
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
                    long userId = -1;
                    using (SimpleDBOperations<UserDataRow> userTable = new(simpleDBManager, keyManager))
                    {
						simpleDBManager.Initialize(pluginClassesService);

						Assert.IsNotNull(userTable);

                        UserDataRow user = new();
                        user.Email = "test@123.net";
                        user.FirstName = "test";
                        user.Surname = "User";
                        user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                        userTable.Insert(user);
                        userId = user.Id;
                    }

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new(userId, true);
                    LoginResult result = sut.Login(String.Empty, String.Empty, "10.10.10.1", 0, ref loginDetails);
                    Assert.AreEqual(LoginResult.Remembered, result);
                    Assert.AreEqual("test@123.net", loginDetails.Email);
                    Assert.AreEqual("test User", loginDetails.Username);
                    Assert.AreEqual(userId, loginDetails.UserId);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_WithEmailAndPassword_UserFoundInUserTable_Returns_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IPluginClassesService pluginClassesService = new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() });

                    ISimpleDBManager simpleDBManager = new SimpleDBManager(directory);
                    IForeignKeyManager keyManager = new ForeignKeyManager();
                    long userId = -1;
                    using (SimpleDBOperations<UserDataRow> userTable = new(simpleDBManager, keyManager))
                    {
						simpleDBManager.Initialize(pluginClassesService);
						Assert.IsNotNull(userTable);

                        UserDataRow user = new();
                        user.Email = "test@123.net";
                        user.FirstName = "test";
                        user.Surname = "User";
                        user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                        userTable.Insert(user);
                        userId = user.Id;
                    }

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    LoginResult result = sut.Login("TeSt@123.NEt", "password", "10.10.10.1", 0, ref loginDetails);
                    Assert.AreEqual(LoginResult.Success, result);
                    Assert.AreEqual("test@123.net", loginDetails.Email);
                    Assert.AreEqual("test User", loginDetails.Username);
                    Assert.AreEqual(userId, loginDetails.UserId);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_WithEmailAndPasswordAndPasswordExpired_UserFoundInUserTable_Returns_PasswordChangeRequired()
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
                    long userId = -1;
                    using (SimpleDBOperations<UserDataRow> userTable = new(simpleDBManager, keyManager))
                    {
						simpleDBManager.Initialize(pluginClassesService);
						Assert.IsNotNull(userTable);

                        UserDataRow user = new();
                        user.Email = "test@123.net";
                        user.FirstName = "test";
                        user.Surname = "User";
                        user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                        userTable.Insert(user);
                        userId = user.Id;
                    }

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    LoginResult result = sut.Login("TeSt@123.NEt", "password", "10.10.10.1", 0, ref loginDetails);
                    Assert.AreEqual(LoginResult.PasswordChangeRequired, result);
                    Assert.AreEqual("test@123.net", loginDetails.Email);
                    Assert.AreEqual("test User", loginDetails.Username);
                    Assert.AreEqual(userId, loginDetails.UserId);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_WithInvalidCredentials5Times_Returns_AccountLocked()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    long userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    LoginResult result = sut.Login("TeSt@123.NEt", "password123", "10.10.10.1", 5, ref loginDetails);
                    Assert.AreEqual(LoginResult.AccountLocked, result);

                    user = userTable.Select(userId);
                    Assert.IsNotNull(user);
                    Assert.IsTrue(user.Locked);

                    result = sut.Login("TeSt@123.NEt", "password", "10.10.10.1", 0, ref loginDetails);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnlockAccount_InvalidUserName_Null_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    long userId = -1;


                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "UnlockMe";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    bool result = sut.UnlockAccount(null, "unlock code");
                    Assert.IsFalse(result);

                    user = userTable.Select(0);
                    Assert.IsNotNull(user);
                    Assert.IsTrue(user.Locked);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UnlockAccount_InvalidUserUnlockCode_Null_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    long userId = -1;


                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "UnlockMe";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    bool result = sut.UnlockAccount("test@123.net", null);
                    Assert.IsFalse(result);

                    user = userTable.Select(0);
                    Assert.IsNotNull(user);
                    Assert.IsTrue(user.Locked);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void UnlockAccount_IncorrectUnlockCode_Returns_False()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "UnlockMe";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    long userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    bool result = sut.UnlockAccount("TeSt@123.NEt", "unlock code");
                    Assert.IsFalse(result);

                    user = userTable.Select(userId);
                    Assert.IsNotNull(user);
                    Assert.IsTrue(user.Locked);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void UnlockAccount_UserIsNotLocked_Returns_False()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
					long userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    bool result = sut.UnlockAccount("TeSt@123.NEt", "unlock code");
                    Assert.IsFalse(result);

                    user = userTable.Select(userId);
                    Assert.IsNotNull(user);
                    Assert.IsFalse(user.Locked);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void UnlockAccount_AccountUnlockedWithCorrectCode_Returns_True()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "unlock code";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
					long userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    bool result = sut.UnlockAccount("TeSt@123.NEt", "unlock code");
                    Assert.IsTrue(result);

                    user = userTable.Select(userId);
                    Assert.IsNotNull(user);
                    Assert.IsFalse(user.Locked);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Login_ExternalUser_InvalidUserName_Null_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "UnlockMe";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    long userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    bool result = sut.UnlockAccount(null, "unlock code");
                    Assert.IsFalse(result);

                    user = userTable.Select(userId);
                    Assert.IsNotNull(user);
                    Assert.IsTrue(user.Locked);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Login_ExternalUser_InvalidTokenDetails_Null_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "UnlockMe";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    long userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    sut.Login(null, ref loginDetails);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Login_ExternalUser_InvalidTokenDetails_EmailNull_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    long userId = -1;


                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "UnlockMe";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();

                    sut.Login(new MockTokenUserDetails(null, "provider"), ref loginDetails);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Login_ExternalUser_InvalidTokenDetails_ProviderNull_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    long userId = -1;


                    ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService(typeof(ISimpleDBOperations<UserDataRow>)) as ISimpleDBOperations<UserDataRow>;

                    Assert.IsNotNull(userTable);

                    UserDataRow user = new();
                    user.Email = "test@123.net";
                    user.FirstName = "test";
                    user.Surname = "User";
                    user.UnlockCode = "UnlockMe";
                    user.Password = Shared.Utilities.Encrypt("password", "DSFOIRTEWRasd/flkqw409r sdaedf2134A");

                    userTable.Insert(user);
                    userId = user.Id;


                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    UserLoginDetails loginDetails = new();
                    loginDetails.Email = "test@123.net";

                    sut.Login(new MockTokenUserDetails("test@123.net", null), ref loginDetails);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_ExternalUser_CreateLogin_Returns_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out PluginInitialisation pluginInitialisation, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					mockPluginClassesService.Provider = provider;
					pluginInitialisation.AfterConfigure(new MockApplicationBuilder(provider));
					
					ISimpleDBOperations<ExternalUsersDataRow> externalUserTable = provider.GetRequiredService(typeof(ISimpleDBOperations<ExternalUsersDataRow>)) as ISimpleDBOperations<ExternalUsersDataRow>;

                    Assert.IsNotNull(externalUserTable);

                    Assert.IsFalse(externalUserTable.Select().Any());

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    ITokenUserDetails tokenUserDetails = new MockTokenUserDetails("test@123.net", "TestProvider");
                    tokenUserDetails.Id = "Obviously this is an invalid token, but will work at the moment";

                    UserLoginDetails loginDetails = null;


                    LoginResult result = sut.Login(tokenUserDetails, ref loginDetails);
                    Assert.AreEqual(LoginResult.Success, result);
                    Assert.AreEqual("test@123.net", loginDetails.Username);
                    Assert.AreEqual("test@123.net", loginDetails.Email);
                    Assert.AreEqual(-9223372036854775807, loginDetails.UserId);
                    Assert.IsTrue(loginDetails.RememberMe);

                    ExternalUsersDataRow user = externalUserTable.Select().FirstOrDefault(eu => eu.Email.Equals("test@123.net"));

                    Assert.IsNotNull(user);
                    Assert.AreEqual(-9223372036854775807, user.Id);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_ExternalUser_VerifyUserNotFound_Returns_InvalidCredentials()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<ExternalUsersDataRow> externalUserTable = provider.GetRequiredService(typeof(ISimpleDBOperations<ExternalUsersDataRow>)) as ISimpleDBOperations<ExternalUsersDataRow>;

                    Assert.IsNotNull(externalUserTable);

                    Assert.IsFalse(externalUserTable.Select().Any());

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    ITokenUserDetails tokenUserDetails = new MockTokenUserDetails("test@123.net", "TestProvider");
                    tokenUserDetails.Id = "Obviously this is an invalid token, but will work at the moment";
                    tokenUserDetails.Verify = true;

                    UserLoginDetails loginDetails = null;

                    LoginResult result = sut.Login(tokenUserDetails, ref loginDetails);
                    Assert.AreEqual(LoginResult.InvalidCredentials, result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Login_ExternalUser_VerifyAndUserFound_Returns_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<ExternalUsersDataRow> externalUserTable = provider.GetRequiredService(typeof(ISimpleDBOperations<ExternalUsersDataRow>)) as ISimpleDBOperations<ExternalUsersDataRow>;

                    Assert.IsNotNull(externalUserTable);

                    Assert.IsFalse(externalUserTable.Select().Any());

                    ExternalUsersDataRow user = new()
                    {
                        Email = "test@321.net",
                        Provider = "Provider",
                        Token = "My token"
                    };

                    externalUserTable.Insert(user);

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    ITokenUserDetails tokenUserDetails = new MockTokenUserDetails("test@321.net", "Provider");
                    tokenUserDetails.Id = "My token";
                    tokenUserDetails.Verify = true;

                    UserLoginDetails loginDetails = null;


                    LoginResult result = sut.Login(tokenUserDetails, ref loginDetails);
                    Assert.AreEqual(LoginResult.Success, result);
                    Assert.IsNull(loginDetails);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveExternalUser_InvalidParamNull_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    sut.RemoveExternalUser(null);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void RemoveExternalUser_UserNotFound_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<ExternalUsersDataRow> externalUserTable = provider.GetRequiredService(typeof(ISimpleDBOperations<ExternalUsersDataRow>)) as ISimpleDBOperations<ExternalUsersDataRow>;

                    Assert.IsNotNull(externalUserTable);

                    Assert.IsFalse(externalUserTable.Select().Any());

                    ExternalUsersDataRow user = new()
                    {
                        Email = "test@321.net",
                        Provider = "Provider",
                        Token = "My token"
                    };

                    externalUserTable.Insert(user);

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    ITokenUserDetails tokenUserDetails = new MockTokenUserDetails("test@321a.net", "Provider");
                    tokenUserDetails.Id = "My token 1";

                    sut.RemoveExternalUser(tokenUserDetails);

                    Assert.IsTrue(externalUserTable.Select().Any());
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void RemoveExternalUser_UserFoundAndRemoved_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<ExternalUsersDataRow> externalUserTable = provider.GetRequiredService(typeof(ISimpleDBOperations<ExternalUsersDataRow>)) as ISimpleDBOperations<ExternalUsersDataRow>;

                    Assert.IsNotNull(externalUserTable);

                    Assert.IsFalse(externalUserTable.Select().Any());

                    ExternalUsersDataRow user = new()
                    {
                        Email = "test@321.net",
                        Provider = "Provider",
                        Token = "My token"
                    };

                    externalUserTable.Insert(user);

                    ILoginProvider sut = provider.GetRequiredService<ILoginProvider>();
                    Assert.IsNotNull(sut);

                    ITokenUserDetails tokenUserDetails = new MockTokenUserDetails("test@321a.net", "Provider");
                    tokenUserDetails.Id = "My token";

                    sut.RemoveExternalUser(tokenUserDetails);

                    Assert.IsFalse(externalUserTable.Select().Any());
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}

#pragma warning restore IDE0017