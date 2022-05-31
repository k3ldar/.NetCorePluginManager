﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  File: AccountProviderTests.cs
 *
 *  Purpose:  AccountProvider Tests Tests for text based storage
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PluginManager.DAL.TextFiles.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Accounts;
using PluginManager.Abstractions;
using AspNetCore.PluginManager.Tests.Shared;
using System.IO;
using PluginManager.DAL.TextFiles.Tables;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AccountProviderTests
    {
        private const string TestPathSettings = "{\"TextFileSettings\":{\"Path\":\"$$\"}}";

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_UsersNull_Throws_ArgumentNullException()
        {
            new AccountProvider(null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                    
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void CreateAccount_VerifyEmailAndTelephone_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    ITextReaderWriter<TableUserRow> userTable = (ITextReaderWriter<TableUserRow>)provider.GetService(typeof(ITextReaderWriter<TableUserRow>));
                    Assert.IsNotNull(userTable);
                    TableUserRow userRow = userTable.Select(userId);
                    sut.ConfirmEmailAddress(userId, userRow.EmailConfirmCode.ToString());
                    sut.ConfirmTelephoneNumber(userId, userRow.TelephoneConfirmCode.ToString());

                    bool retrieved = sut.GetUserAccountDetails(userId, out string firstName,
                        out string lastName, out string email, out bool emailConfirmed, 
                        out string telephone, out bool telephoneConfirmed);
                    Assert.IsTrue(retrieved);
                    Assert.AreEqual("Joe", firstName);
                    Assert.AreEqual("Bloggs", lastName);
                    Assert.AreEqual("me@here.com", email);
                    Assert.IsTrue(emailConfirmed);
                    Assert.AreEqual("", telephone);
                    Assert.IsTrue(telephoneConfirmed);

                    userRow = userTable.Select(userId);
                    Assert.AreEqual("", userRow.EmailConfirmCode);
                    Assert.AreEqual("", userRow.TelephoneConfirmCode);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ChangePassword_PasswordUpdated_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool changePassword = sut.ChangePassword(userId, "bla12345");

                    Assert.IsTrue(changePassword);

                    ITextReaderWriter<TableUserRow> userTable = (ITextReaderWriter<TableUserRow>)provider.GetService(typeof(ITextReaderWriter<TableUserRow>));
                    Assert.IsNotNull(userTable);
                    TableUserRow userRow = userTable.Select(userId);

                    Assert.IsNotNull(userRow);

                    Assert.AreEqual("bla12345", userRow.Password);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SetUserAccountDetails_DetailsUpdated_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool setDetails = sut.SetUserAccountDetails(userId, "John", "Snow", "john@snow.org", "123456");

                    Assert.IsTrue(setDetails);

                    ITextReaderWriter<TableUserRow> userTable = (ITextReaderWriter<TableUserRow>)provider.GetService(typeof(ITextReaderWriter<TableUserRow>));
                    Assert.IsNotNull(userTable);
                    TableUserRow userRow = userTable.Select(userId);

                    Assert.IsNotNull(userRow);

                    Assert.AreEqual("John", userRow.FirstName);
                    Assert.AreEqual("Snow", userRow.Surname);
                    Assert.AreEqual("john@snow.org", userRow.Email);
                    Assert.AreEqual("123456", userRow.Telephone);
                    Assert.IsFalse(userRow.EmailConfirmed);
                    Assert.IsFalse(userRow.TelephoneConfirmed);
                    Assert.IsFalse(String.IsNullOrEmpty(userRow.TelephoneConfirmCode));
                    Assert.AreEqual(6, userRow.TelephoneConfirmCode.Length);
                    Assert.IsFalse(String.IsNullOrEmpty(userRow.EmailConfirmCode));
                    Assert.AreEqual(6, userRow.EmailConfirmCode.Length);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void DeleteAccount_AccountDeleted_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool deleteAccount = sut.DeleteAccount(userId);

                    Assert.IsTrue(deleteAccount);

                    ITextReaderWriter<TableUserRow> userTable = (ITextReaderWriter<TableUserRow>)provider.GetService(typeof(ITextReaderWriter<TableUserRow>));
                    Assert.IsNotNull(userTable);
                    TableUserRow userRow = userTable.Select(userId);

                    Assert.IsNull(userRow);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void AccountLock_AndUnlocked_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextReaderWriter<TableUserRow> userTable = (ITextReaderWriter<TableUserRow>)provider.GetService(typeof(ITextReaderWriter<TableUserRow>));
                    Assert.IsNotNull(userTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    TableUserRow userRow = userTable.Select(userId);

                    Assert.IsFalse(userRow.Locked);

                    bool accountLocked = sut.AccountLock(userId);

                    Assert.IsTrue(accountLocked);

                    userRow = userTable.Select(userId);
                    Assert.IsNotNull(userRow);

                    Assert.IsTrue(userRow.Locked);

                    bool unlockAccount = sut.AccountUnlock(userId);
                    Assert.IsTrue(unlockAccount);

                    userRow = userTable.Select(userId);
                    Assert.IsNotNull(userRow);

                    Assert.IsFalse(userRow.Locked);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SetMarketingPreferences_Remembered_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextReaderWriter<TableUserRow> userTable = (ITextReaderWriter<TableUserRow>)provider.GetService(typeof(ITextReaderWriter<TableUserRow>));
                    Assert.IsNotNull(userTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    TableUserRow userRow = userTable.Select(userId);

                    Assert.IsFalse(userRow.MarketingEmail);
                    Assert.IsFalse(userRow.MarketingPostal);
                    Assert.IsFalse(userRow.MarketingSms);
                    Assert.IsFalse(userRow.MarketingTelephone);

                    bool marketingUpdated = sut.SetMarketingPreferences(userId, new Marketing(true, true, true, true));

                    Assert.IsTrue(marketingUpdated);

                    userRow = userTable.Select(userId);
                    Assert.IsNotNull(userRow);
                    Assert.IsTrue(userRow.MarketingEmail);
                    Assert.IsTrue(userRow.MarketingPostal);
                    Assert.IsTrue(userRow.MarketingSms);
                    Assert.IsTrue(userRow.MarketingTelephone);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}