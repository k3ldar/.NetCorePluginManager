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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: UserSearchTests.cs
 *
 *  Purpose:  IUserSearch Tests for text based storage
 *
 *  Date        Name                Reason
 *  06/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Accounts;
using Middleware.Users;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class UserSearchTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_UsersTableNull_Throws_ArgumentNullException()
        {
            new UserSearch(null);
        }

        [TestMethod]
        public void GetUsers_Page1Returns6Users_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(accountProvider);

                    for (int i = 0; i < 10; i++)
                    {
                        bool created = accountProvider.CreateAccount($"user{i}@here.com", $"Joe {i}", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                        Assert.IsTrue(created);
                    }

                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);
                    Assert.AreEqual(10, userTable.RecordCount);

                    IUserSearch sut = provider.GetRequiredService<IUserSearch>();

                    Assert.IsNotNull(sut);

                    List<SearchUser> users = sut.GetUsers(1, 6, String.Empty, String.Empty);
                    Assert.IsNotNull(users);
                    Assert.AreEqual(6, users.Count);
                    Assert.AreEqual("Joe 0 Bloggs", users[0].Name);
                    Assert.AreEqual("Joe 1 Bloggs", users[1].Name);
                    Assert.AreEqual("Joe 2 Bloggs", users[2].Name);
                    Assert.AreEqual("Joe 3 Bloggs", users[3].Name);
                    Assert.AreEqual("Joe 4 Bloggs", users[4].Name);
                    Assert.AreEqual("Joe 5 Bloggs", users[5].Name);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetUsers_Page3Returns3Users_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));
                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(accountProvider);

                    for (int i = 0; i < 10; i++)
                    {
                        bool created = accountProvider.CreateAccount($"user{i}@here.com", $"Joe {i}", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                        Assert.IsTrue(created);
                    }

                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);
                    Assert.AreEqual(10, userTable.RecordCount);

                    IUserSearch sut = provider.GetRequiredService<IUserSearch>();

                    Assert.IsNotNull(sut);

                    List<SearchUser> users = sut.GetUsers(3, 3, String.Empty, String.Empty);
                    Assert.IsNotNull(users);
                    Assert.AreEqual(3, users.Count);
                    Assert.AreEqual("Joe 6 Bloggs", users[0].Name);
                    Assert.AreEqual("Joe 7 Bloggs", users[1].Name);
                    Assert.AreEqual("Joe 8 Bloggs", users[2].Name);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
