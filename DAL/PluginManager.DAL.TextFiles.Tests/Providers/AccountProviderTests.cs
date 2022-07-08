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
 *  File: AccountProviderTests.cs
 *
 *  Purpose:  AccountProviderTests Tests for text based storage
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
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
using Middleware.Accounts.Orders;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class AccountProviderTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_UsersNull_Throws_ArgumentNullException()
        {
            new AccountProvider(null, null, null);
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

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

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

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);
                    UserDataRow userRow = userTable.Select(userId);
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

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool changePassword = sut.ChangePassword(userId, "bla12345");

                    Assert.IsTrue(changePassword);

                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);
                    UserDataRow userRow = userTable.Select(userId);

                    Assert.IsNotNull(userRow);

                    Assert.AreEqual(Shared.Utilities.Encrypt("bla12345", "DSFOIRTEWRasd/flkqw409r sdaedf2134A"), userRow.Password);
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

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool setDetails = sut.SetUserAccountDetails(userId, "John", "Snow", "john@snow.org", "123456");

                    Assert.IsTrue(setDetails);

                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);
                    UserDataRow userRow = userTable.Select(userId);

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

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool deleteAccount = sut.DeleteAccount(userId);

                    Assert.IsTrue(deleteAccount);

                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);
                    UserDataRow userRow = userTable.Select(userId);

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

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    UserDataRow userRow = userTable.Select(userId);

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

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    UserDataRow userRow = userTable.Select(userId);

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

        [TestMethod]
        public void SetBillingAddress_Remembered_Success()
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
                    ITextTableOperations<AddressDataRow> addressTable = (ITextTableOperations<AddressDataRow>)provider.GetService(typeof(ITextTableOperations<AddressDataRow>));
                    Assert.IsNotNull(addressTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    Address address = new Address(-1, 1.99m, "business", "add 1", "add 2", "add 3", "city", "county", "postcode", "NL");
                    bool setBillingAddress = sut.SetBillingAddress(userId, address);

                    AddressDataRow addressRow = addressTable.Select(0);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(0, addressRow.Id);
                    Assert.AreEqual(userId, addressRow.UserId);
                    Assert.AreEqual(1.99m, addressRow.PostageCost);
                    Assert.AreEqual("business", addressRow.BusinessName);
                    Assert.AreEqual("add 1", addressRow.AddressLine1);
                    Assert.AreEqual("add 2", addressRow.AddressLine2);
                    Assert.AreEqual("add 3", addressRow.AddressLine3);
                    Assert.AreEqual("city", addressRow.City);
                    Assert.AreEqual("county", addressRow.County);
                    Assert.AreEqual("postcode", addressRow.Postcode);
                    Assert.AreEqual("NL", addressRow.Country);
                    Assert.IsFalse(addressRow.IsDelivery);


                    address = new Address(Convert.ToInt32(addressRow.Id), 1.99m, "", "add 1a", "add 2a", "add 3a", "citya", "countya", "postcodea", "FR");
                    setBillingAddress = sut.SetBillingAddress(userId, address);

                    addressRow = addressTable.Select(0);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(0, addressRow.Id);
                    Assert.AreEqual(userId, addressRow.UserId);
                    Assert.AreEqual(1.99m, addressRow.PostageCost);
                    Assert.AreEqual("", addressRow.BusinessName);
                    Assert.AreEqual("add 1a", addressRow.AddressLine1);
                    Assert.AreEqual("add 2a", addressRow.AddressLine2);
                    Assert.AreEqual("add 3a", addressRow.AddressLine3);
                    Assert.AreEqual("citya", addressRow.City);
                    Assert.AreEqual("countya", addressRow.County);
                    Assert.AreEqual("postcodea", addressRow.Postcode);
                    Assert.AreEqual("FR", addressRow.Country);
                    Assert.IsFalse(addressRow.IsDelivery);

                    address = sut.GetBillingAddress(userId);
                    Assert.IsNotNull(address);

                    Assert.AreEqual(0, address.Id);
                    Assert.AreEqual(1.99m, address.ShippingCost);
                    Assert.AreEqual("", address.BusinessName);
                    Assert.AreEqual("add 1a", address.AddressLine1);
                    Assert.AreEqual("add 2a", address.AddressLine2);
                    Assert.AreEqual("add 3a", address.AddressLine3);
                    Assert.AreEqual("citya", address.City);
                    Assert.AreEqual("countya", address.County);
                    Assert.AreEqual("postcodea", address.Postcode);
                    Assert.AreEqual("FR", address.Country);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void OrdersGet_UserDoesNotExist_ReturnsEmptyList_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    List<Order> orders = sut.OrdersGet(-1);

                    Assert.IsNotNull(orders);
                    Assert.AreEqual(0, orders.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void OrdersGet_RetrievesOrders_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();

                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));
                services.AddSingleton<IPluginClassesService>(new MockPluginClassesService(new List<object>() { new UserDataRowTriggers() }));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<UserDataRow> userTable = (ITextTableOperations<UserDataRow>)provider.GetService(typeof(ITextTableOperations<UserDataRow>));
                    Assert.IsNotNull(userTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    List<Order> orders = sut.OrdersGet(0);

                    Assert.IsNotNull(orders);
                    Assert.AreEqual(2, orders.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void OrderPaid_UserDoesNotExist_ReturnsEmptyList_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void OrderPaid_OrderDoesNotExist_ReturnsEmptyList_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void OrderPaid_MessageIsNullOrEmpty_Throws_ArgumentNullException()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void OrderPaid_RetrievesOrders_Success()
        {
            Assert.IsTrue(false);
        }
        [TestMethod]
        public void InvoicesGet_UserDoesNotExist_ReturnsEmptyList_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void InvoicesGet_RetrievesOrders_Success()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void SetDeliveryAddress_CorrectlySet_ReturnsTrue()
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
                    ITextTableOperations<AddressDataRow> addressTable = (ITextTableOperations<AddressDataRow>)provider.GetService(typeof(ITextTableOperations<AddressDataRow>));
                    Assert.IsNotNull(addressTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    DeliveryAddress address = new DeliveryAddress(-1, "business", "add 1", "add 2", "add 3", "city", "county", "postcode", "NL", 2.99m);
                    bool addBillingAddress = sut.AddDeliveryAddress(userId, address);
                    Assert.IsTrue(addBillingAddress);

                    AddressDataRow addressRow = addressTable.Select(0);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(0, addressRow.Id);
                    Assert.AreEqual(userId, addressRow.UserId);
                    Assert.AreEqual(2.99m, addressRow.PostageCost);
                    Assert.AreEqual("business", addressRow.BusinessName);
                    Assert.AreEqual("add 1", addressRow.AddressLine1);
                    Assert.AreEqual("add 2", addressRow.AddressLine2);
                    Assert.AreEqual("add 3", addressRow.AddressLine3);
                    Assert.AreEqual("city", addressRow.City);
                    Assert.AreEqual("county", addressRow.County);
                    Assert.AreEqual("postcode", addressRow.Postcode);
                    Assert.AreEqual("NL", addressRow.Country);
                    Assert.IsTrue(addressRow.IsDelivery);


                    address = new DeliveryAddress(Convert.ToInt32(addressRow.Id), "", "add 1a", "add 2a", "add 3a", "citya", "countya", "postcodea", "FR", 1.99m);
                    addBillingAddress = sut.AddDeliveryAddress(userId, address);
                    Assert.IsTrue(addBillingAddress);

                    addressRow = addressTable.Select(0);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(0, addressRow.Id);
                    Assert.AreEqual(userId, addressRow.UserId);
                    Assert.AreEqual(1.99m, addressRow.PostageCost);
                    Assert.AreEqual("", addressRow.BusinessName);
                    Assert.AreEqual("add 1a", addressRow.AddressLine1);
                    Assert.AreEqual("add 2a", addressRow.AddressLine2);
                    Assert.AreEqual("add 3a", addressRow.AddressLine3);
                    Assert.AreEqual("citya", addressRow.City);
                    Assert.AreEqual("countya", addressRow.County);
                    Assert.AreEqual("postcodea", addressRow.Postcode);
                    Assert.AreEqual("FR", addressRow.Country);
                    Assert.IsTrue(addressRow.IsDelivery);

                    address.ShippingCost = 3.99m;
                    sut.SetDeliveryAddress(userId, address);

                    List<DeliveryAddress> deliveryAddresses = sut.GetDeliveryAddresses(userId);
                    Assert.IsNotNull(deliveryAddresses);
                    Assert.AreEqual(2, deliveryAddresses.Count);

                    address = deliveryAddresses[1];
                    Assert.AreEqual(0, address.Id);
                    Assert.AreEqual(3.99m, address.ShippingCost);
                    Assert.AreEqual("", address.BusinessName);
                    Assert.AreEqual("add 1a", address.AddressLine1);
                    Assert.AreEqual("add 2a", address.AddressLine2);
                    Assert.AreEqual("add 3a", address.AddressLine3);
                    Assert.AreEqual("citya", address.City);
                    Assert.AreEqual("countya", address.County);
                    Assert.AreEqual("postcodea", address.Postcode);
                    Assert.AreEqual("FR", address.Country);

                    sut.DeleteDeliveryAddress(userId, deliveryAddresses[0]);

                    deliveryAddresses = sut.GetDeliveryAddresses(userId);
                    Assert.IsNotNull(deliveryAddresses);
                    Assert.AreEqual(1, deliveryAddresses.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
