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
 *  File: AccountProviderTests.cs
 *
 *  Purpose:  Account provider tests Tests for text based storage
 *
 *  Date        Name                Reason
 *  31/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Invoices;
using Middleware.Accounts.Orders;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

using SharedPluginFeatures;
using System.Globalization;

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
            new AccountProvider(null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);
                    Assert.AreEqual(2u, userId);

                    ISimpleDBOperations<UserDataRow> userTable = (ISimpleDBOperations<UserDataRow>)provider.GetService(typeof(ISimpleDBOperations<UserDataRow>));
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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool changePassword = sut.ChangePassword(userId, "bla12345");

                    Assert.IsTrue(changePassword);

                    ISimpleDBOperations<UserDataRow> userTable = (ISimpleDBOperations<UserDataRow>)provider.GetService(typeof(ISimpleDBOperations<UserDataRow>));
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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool setDetails = sut.SetUserAccountDetails(userId, "John", "Snow", "john@snow.org", "123456");

                    Assert.IsTrue(setDetails);

                    ISimpleDBOperations<UserDataRow> userTable = (ISimpleDBOperations<UserDataRow>)provider.GetService(typeof(ISimpleDBOperations<UserDataRow>));
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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    bool deleteAccount = sut.DeleteAccount(userId);

                    Assert.IsTrue(deleteAccount);

                    ISimpleDBOperations<UserDataRow> userTable = (ISimpleDBOperations<UserDataRow>)provider.GetService(typeof(ISimpleDBOperations<UserDataRow>));
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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = (ISimpleDBOperations<UserDataRow>)provider.GetService(typeof(ISimpleDBOperations<UserDataRow>));
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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = (ISimpleDBOperations<UserDataRow>)provider.GetService(typeof(ISimpleDBOperations<UserDataRow>));
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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<AddressDataRow> addressTable = (ISimpleDBOperations<AddressDataRow>)provider.GetService(typeof(ISimpleDBOperations<AddressDataRow>));
                    Assert.IsNotNull(addressTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    Address address = new Address(-1, "business", "add 1", "add 2", "add 3", "city", "county", "postcode", "NL");
                    bool setBillingAddress = sut.SetBillingAddress(userId, address);

                    AddressDataRow addressRow = addressTable.Select(1);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(1, addressRow.Id);
                    Assert.AreEqual(userId, addressRow.UserId);
                    Assert.AreEqual(0, addressRow.PostageCost);
                    Assert.AreEqual("business", addressRow.BusinessName);
                    Assert.AreEqual("add 1", addressRow.AddressLine1);
                    Assert.AreEqual("add 2", addressRow.AddressLine2);
                    Assert.AreEqual("add 3", addressRow.AddressLine3);
                    Assert.AreEqual("city", addressRow.City);
                    Assert.AreEqual("county", addressRow.County);
                    Assert.AreEqual("postcode", addressRow.Postcode);
                    Assert.AreEqual("NL", addressRow.Country);
                    Assert.IsFalse(addressRow.IsDelivery);


                    address = new Address(Convert.ToInt32(addressRow.Id), "", "add 1a", "add 2a", "add 3a", "citya", "countya", "postcodea", "FR");
                    setBillingAddress = sut.SetBillingAddress(userId, address);

                    addressRow = addressTable.Select(1);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(1, addressRow.Id);
                    Assert.AreEqual(userId, addressRow.UserId);
                    Assert.AreEqual(0, addressRow.PostageCost);
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

                    Assert.AreEqual(1, address.Id);
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
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = (ISimpleDBOperations<UserDataRow>)provider.GetService(typeof(ISimpleDBOperations<UserDataRow>));
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
            string directory = TestHelper.GetTestPath();
            try
            {
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
				Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetService<ISimpleDBOperations<UserDataRow>>();
                    Assert.IsNotNull(userTable);

                    ISimpleDBOperations<OrderDataRow> orderData = provider.GetService<ISimpleDBOperations<OrderDataRow>>();
                    Assert.IsNotNull(orderData);

                    ISimpleDBOperations<OrderItemDataRow> orderItemsData = provider.GetService<ISimpleDBOperations<OrderItemDataRow>>();
                    Assert.IsNotNull(orderItemsData);



                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);
                    Assert.IsTrue(created);

                    DeliveryAddress deliveryAddress = new DeliveryAddress(-1, "", "Street 1", "", "", "city", "county", "postcode", "GB", 5.99m);
                    sut.AddDeliveryAddress(userId, deliveryAddress);

                    orderData.Insert(new List<OrderDataRow>()
                    {
                        new OrderDataRow() { Culture = "en-GB", DeliveryAddress = deliveryAddress.Id, Postage = 4.99m, ProcessStatus = (int)ProcessStatus.PaymentPending, UserId = userId },
                        new OrderDataRow() { Culture = "en-US", DeliveryAddress = deliveryAddress.Id, Postage = 3.99m, ProcessStatus = (int)ProcessStatus.PaymentPending, UserId = userId }
                    });

                    orderItemsData.Insert(new List<OrderItemDataRow>()
                    {
                        new OrderItemDataRow() { Description = "Order 1", Discount = 0, OrderId = 0, Price = 8.56m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 20m },
                        new OrderItemDataRow() { Description = "Order 2a", Discount = 0, OrderId = 1, Price = 8.99m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                        new OrderItemDataRow() { Description = "Order 2b", Discount = 0, OrderId = 1, Price = 4.26m, Quantity = 2, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                    });
                    

                    List<Order> orders = sut.OrdersGet(userId);

                    Assert.IsNotNull(orders);
                    Assert.AreEqual(2, orders.Count);

                    Assert.AreEqual(0, orders[0].Id);
                    Assert.AreEqual("en-GB", orders[0].Culture.Name);
                    Assert.AreEqual(0, orders[0].Discount);
                    Assert.AreEqual(0, orders[0].Id);
                    Assert.AreEqual(1, orders[0].ItemCount);
                    Assert.AreEqual(4.99m, orders[0].Postage);
                    Assert.AreEqual(10.28m, orders[0].SubTotal);
                    Assert.AreEqual(1.72m, orders[0].Tax);
                    Assert.AreEqual(15.27m, orders[0].Total);

                    Assert.AreEqual("Order 1", orders[0].OrderItems[0].Description);
                    Assert.AreEqual(0, orders[0].OrderItems[0].Discount);
                    Assert.AreEqual(0, orders[0].OrderItems[0].Order.Id);
                    Assert.AreEqual(8.56m, orders[0].OrderItems[0].Price);
                    Assert.AreEqual(1, orders[0].OrderItems[0].Quantity);
                    Assert.AreEqual(ItemStatus.Received, orders[0].OrderItems[0].Status);
                    Assert.AreEqual(DiscountType.None, orders[0].OrderItems[0].DiscountType);
                    Assert.AreEqual(20m, orders[0].OrderItems[0].TaxRate);

                    Assert.AreEqual(1, orders[1].Id);
                    Assert.AreEqual("en-US", orders[1].Culture.Name);
                    Assert.AreEqual(0, orders[1].Discount);
                    Assert.AreEqual(1, orders[1].Id);
                    Assert.AreEqual(3, orders[1].ItemCount);
                    Assert.AreEqual(3.99m, orders[1].Postage);
                    Assert.AreEqual(20.137m, orders[1].SubTotal);
                    Assert.AreEqual(2.627m, orders[1].Tax);
                    Assert.AreEqual(24.127m, orders[1].Total);

                    Assert.AreEqual("Order 2a", orders[1].OrderItems[0].Description);
                    Assert.AreEqual(0, orders[1].OrderItems[0].Discount);
                    Assert.AreEqual(1, orders[1].OrderItems[0].Order.Id);
                    Assert.AreEqual(8.99m, orders[1].OrderItems[0].Price);
                    Assert.AreEqual(1, orders[1].OrderItems[0].Quantity);
                    Assert.AreEqual(ItemStatus.Received, orders[1].OrderItems[0].Status);
                    Assert.AreEqual(DiscountType.None, orders[1].OrderItems[0].DiscountType);
                    Assert.AreEqual(15m, orders[1].OrderItems[0].TaxRate);

                    Assert.AreEqual("Order 2b", orders[1].OrderItems[1].Description);
                    Assert.AreEqual(0, orders[1].OrderItems[1].Discount);
                    Assert.AreEqual(1, orders[1].OrderItems[1].Order.Id);
                    Assert.AreEqual(4.26m, orders[1].OrderItems[1].Price);
                    Assert.AreEqual(2, orders[1].OrderItems[1].Quantity);
                    Assert.AreEqual(ItemStatus.Received, orders[1].OrderItems[1].Status);
                    Assert.AreEqual(DiscountType.None, orders[1].OrderItems[1].DiscountType);
                    Assert.AreEqual(15m, orders[1].OrderItems[1].TaxRate);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderPaid_MessageIsNullOrEmpty_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetService<ISimpleDBOperations<UserDataRow>>();
                    Assert.IsNotNull(userTable);

                    ISimpleDBOperations<OrderDataRow> orderData = provider.GetService<ISimpleDBOperations<OrderDataRow>>();
                    Assert.IsNotNull(orderData);

                    ISimpleDBOperations<OrderItemDataRow> orderItemsData = provider.GetService<ISimpleDBOperations<OrderItemDataRow>>();
                    Assert.IsNotNull(orderItemsData);



                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);
                    Assert.IsTrue(created);

                    DeliveryAddress deliveryAddress = new DeliveryAddress(-1, "", "Street 1", "", "", "city", "county", "postcode", "GB", 5.99m);
                    sut.AddDeliveryAddress(userId, deliveryAddress);

                    orderData.Insert(new List<OrderDataRow>()
                    {
                        new OrderDataRow() { Culture = "en-GB", DeliveryAddress = deliveryAddress.Id, Postage = 4.99m, ProcessStatus = (int)ProcessStatus.PaymentPending, UserId = userId },
                        new OrderDataRow() { Culture = "en-US", DeliveryAddress = deliveryAddress.Id, Postage = 3.99m, ProcessStatus = (int)ProcessStatus.PaymentPending, UserId = userId }
                    });

                    orderItemsData.Insert(new List<OrderItemDataRow>()
                    {
                        new OrderItemDataRow() { Description = "Order 1", Discount = 0, OrderId = 0, Price = 8.56m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 20m },
                        new OrderItemDataRow() { Description = "Order 2a", Discount = 0, OrderId = 1, Price = 8.99m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                        new OrderItemDataRow() { Description = "Order 2b", Discount = 0, OrderId = 1, Price = 4.26m, Quantity = 2, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                    });


                    List<Order> orders = sut.OrdersGet(userId);

                    Assert.IsNotNull(orders);
                    Assert.AreEqual(2, orders.Count);

                    sut.OrderPaid(orders[0], PaymentStatus.PaidCard, "");
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OrderPaid_OrderIsNull_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    sut.OrderPaid(null, PaymentStatus.PaidCard, "");
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void OrderPaid_OrderNotFound_Throws_ArgumentNullException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);
                    DeliveryAddress delAddress = new DeliveryAddress(-1, "", "", "", "", "", "", "", "", 0);
                    Order invalidOrder = new Order(-1, DateTime.Now, 1.22m, new System.Globalization.CultureInfo("en-GB"), ProcessStatus.Cancelled, delAddress, new List<OrderItem>());

                    sut.OrderPaid(invalidOrder, PaymentStatus.PaidCard, "order not found");
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void OrderPaid_PaymentStatusUnpaid_Throws_ArgumentOutOfRangeException()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetService<ISimpleDBOperations<UserDataRow>>();
                    Assert.IsNotNull(userTable);

                    ISimpleDBOperations<OrderDataRow> orderData = provider.GetService<ISimpleDBOperations<OrderDataRow>>();
                    Assert.IsNotNull(orderData);

                    ISimpleDBOperations<OrderItemDataRow> orderItemsData = provider.GetService<ISimpleDBOperations<OrderItemDataRow>>();
                    Assert.IsNotNull(orderItemsData);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);
                    Assert.IsTrue(created);

                    DeliveryAddress deliveryAddress = new DeliveryAddress(-1, "", "Street 1", "", "", "city", "county", "postcode", "GB", 5.99m);
                    sut.AddDeliveryAddress(userId, deliveryAddress);

                    orderData.Insert(new List<OrderDataRow>()
                    {
                        new OrderDataRow() { Culture = "en-GB", DeliveryAddress = deliveryAddress.Id, Postage = 4.99m, ProcessStatus = (int)ProcessStatus.PaymentPending, UserId = userId },
                        new OrderDataRow() { Culture = "en-US", DeliveryAddress = deliveryAddress.Id, Postage = 3.99m, ProcessStatus = (int)ProcessStatus.PaymentPending, UserId = userId }
                    });

                    orderItemsData.Insert(new List<OrderItemDataRow>()
                    {
                        new OrderItemDataRow() { Description = "Order 1", Discount = 0, OrderId = 0, Price = 8.56m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 20m },
                        new OrderItemDataRow() { Description = "Order 2a", Discount = 0, OrderId = 1, Price = 8.99m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                        new OrderItemDataRow() { Description = "Order 2b", Discount = 0, OrderId = 1, Price = 4.26m, Quantity = 2, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                    });

                    sut.OrderPaid(sut.OrdersGet(userId)[0], PaymentStatus.Unpaid, "not been paid");
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void OrderPaid_RetrievesOrdersAndMarksAsPaid_InvoiceCreated_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
				Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<UserDataRow> userTable = provider.GetService<ISimpleDBOperations<UserDataRow>>();
                    Assert.IsNotNull(userTable);

                    ISimpleDBOperations<OrderDataRow> orderData = provider.GetService<ISimpleDBOperations<OrderDataRow>>();
                    Assert.IsNotNull(orderData);
                    Assert.AreEqual(0, orderData.RecordCount);

                    ISimpleDBOperations<OrderItemDataRow> orderItemsData = provider.GetService<ISimpleDBOperations<OrderItemDataRow>>();
                    Assert.IsNotNull(orderItemsData);
                    Assert.AreEqual(0, orderItemsData.RecordCount);

                    ISimpleDBOperations<InvoiceDataRow> invoiceData = provider.GetService<ISimpleDBOperations<InvoiceDataRow>>();
                    Assert.IsNotNull(invoiceData);
                    Assert.AreEqual(0, invoiceData.RecordCount);

                    ISimpleDBOperations<InvoiceItemDataRow> invoiceItemData = provider.GetService<ISimpleDBOperations<InvoiceItemDataRow>>();
                    Assert.IsNotNull(invoiceItemData);
                    Assert.AreEqual(0, invoiceItemData.RecordCount);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);
                    Assert.IsTrue(created);

                    DeliveryAddress deliveryAddress = new DeliveryAddress(-1, "", "Street 1", "", "", "city", "county", "postcode", "GB", 5.99m);
                    sut.AddDeliveryAddress(userId, deliveryAddress);

                    orderData.Insert(new List<OrderDataRow>()
                    {
                        new OrderDataRow() { Culture = "en-GB", DeliveryAddress = deliveryAddress.Id, Postage = 4.99m, ProcessStatus = (int)ProcessStatus.OrderReceived, UserId = userId },
                    });

                    orderItemsData.Insert(new List<OrderItemDataRow>()
                    {
                        new OrderItemDataRow() { Description = "Order 1a", Discount = 0, OrderId = 0, Price = 8.56m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 20m },
                        new OrderItemDataRow() { Description = "Order 1b", Discount = 0, OrderId = 0, Price = 8.99m, Quantity = 1, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                        new OrderItemDataRow() { Description = "Order 1c", Discount = 0, OrderId = 0, Price = 4.26m, Quantity = 2, ItemStatus = 0, DiscountType = 0, TaxRate = 15m },
                    });


                    List<Order> orders = sut.OrdersGet(userId);

                    Assert.IsNotNull(orders);
                    Assert.AreEqual(1, orders.Count);

                    sut.OrderPaid(orders[0], PaymentStatus.PaidMixed, "Paid by card and cash");

                    Assert.AreEqual(1, orders.Count);

                    Assert.AreEqual(1, orderData.RecordCount);
                    Assert.AreEqual(3, orderItemsData.RecordCount);
                    Assert.AreEqual(1, invoiceData.RecordCount);
                    Assert.AreEqual(3, invoiceItemData.RecordCount);

                    List<Invoice> invoices = sut.InvoicesGet(userId);

                    Assert.AreEqual(PaymentStatus.PaidMixed, invoices[0].PaymentStatus);
                    Assert.AreEqual(ProcessStatus.OrderReceived, invoices[0].Status);
                    Assert.AreEqual(1, invoices.Count);
                    Assert.AreEqual(3, invoices[0].InvoiceItems.Count);
                    Assert.AreEqual(4, invoices[0].ItemCount);


                    Assert.AreEqual(0, invoices[0].Id);
                    Assert.AreEqual("en-GB", invoices[0].Culture.Name);
                    Assert.AreEqual(0, invoices[0].Discount);
                    Assert.AreEqual(0, invoices[0].Id);
                    Assert.AreEqual(4, invoices[0].ItemCount);
                    Assert.AreEqual(4.99m, invoices[0].Postage);
                    Assert.AreEqual(30.42m, invoices[0].SubTotal);
                    Assert.AreEqual(4.35m, invoices[0].Tax);
                    Assert.AreEqual(35.41m, invoices[0].Total);

                    Assert.AreEqual("Order 1a", invoices[0].InvoiceItems[0].Description);
                    Assert.AreEqual(0, invoices[0].InvoiceItems[0].Discount);
                    Assert.AreEqual(0, invoices[0].InvoiceItems[0].Invoice.Id);
                    Assert.AreEqual(8.56m, invoices[0].InvoiceItems[0].Price);
                    Assert.AreEqual(1, invoices[0].InvoiceItems[0].Quantity);
                    Assert.AreEqual(ItemStatus.Received, invoices[0].InvoiceItems[0].Status);
                    Assert.AreEqual(DiscountType.None, invoices[0].InvoiceItems[0].DiscountType);
                    Assert.AreEqual(20m, invoices[0].InvoiceItems[0].TaxRate);

                    Assert.AreEqual("Order 1b", invoices[0].InvoiceItems[1].Description);
                    Assert.AreEqual(0, invoices[0].InvoiceItems[1].Discount);
                    Assert.AreEqual(0, invoices[0].InvoiceItems[1].Invoice.Id);
                    Assert.AreEqual(8.99m, invoices[0].InvoiceItems[1].Price);
                    Assert.AreEqual(1, invoices[0].InvoiceItems[1].Quantity);
                    Assert.AreEqual(ItemStatus.Received, invoices[0].InvoiceItems[1].Status);
                    Assert.AreEqual(DiscountType.None, invoices[0].InvoiceItems[1].DiscountType);
                    Assert.AreEqual(15m, invoices[0].InvoiceItems[1].TaxRate);

                    Assert.AreEqual("Order 1c", invoices[0].InvoiceItems[2].Description);
                    Assert.AreEqual(0, invoices[0].InvoiceItems[2].Discount);
                    Assert.AreEqual(0, invoices[0].InvoiceItems[2].Invoice.Id);
                    Assert.AreEqual(4.26m, invoices[0].InvoiceItems[2].Price);
                    Assert.AreEqual(2, invoices[0].InvoiceItems[2].Quantity);
                    Assert.AreEqual(ItemStatus.Received, invoices[0].InvoiceItems[2].Status);
                    Assert.AreEqual(DiscountType.None, invoices[0].InvoiceItems[2].DiscountType);
                    Assert.AreEqual(15m, invoices[0].InvoiceItems[2].TaxRate);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void InvoicesGet_UserDoesNotExist_ReturnsEmptyList_Success()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    List<Invoice> results = sut.InvoicesGet(10);

                    Assert.IsNotNull(results);
                    Assert.AreEqual(0, results.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SetDeliveryAddress_CorrectlySet_ReturnsTrue()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<AddressDataRow> addressTable = (ISimpleDBOperations<AddressDataRow>)provider.GetService(typeof(ISimpleDBOperations<AddressDataRow>));
                    Assert.IsNotNull(addressTable);

                    IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

                    Assert.IsNotNull(sut);

                    bool created = sut.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    Assert.IsTrue(created);

                    DeliveryAddress address = new DeliveryAddress(-1, "business", "add 1", "add 2", "add 3", "city", "county", "postcode", "NL", 2.99m);
                    bool addBillingAddress = sut.AddDeliveryAddress(userId, address);
                    Assert.IsTrue(addBillingAddress);

                    AddressDataRow addressRow = addressTable.Select(1);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(1, addressRow.Id);
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

                    addressRow = addressTable.Select(2);
                    Assert.IsNotNull(addressRow);

                    Assert.AreEqual(2, addressRow.Id);
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

                    address = sut.GetDeliveryAddress(userId, 2);
                    address.PostageCost = 3.99m;
                    sut.SetDeliveryAddress(userId, address);

                    List<DeliveryAddress> deliveryAddresses = sut.GetDeliveryAddresses(userId);
                    Assert.IsNotNull(deliveryAddresses);
                    Assert.AreEqual(2, deliveryAddresses.Count);

                    address = deliveryAddresses[1];
                    Assert.AreEqual(2, address.Id);
                    Assert.AreEqual(3.99m, address.PostageCost);
                    Assert.AreEqual("", address.BusinessName);
                    Assert.AreEqual("add 1a", address.AddressLine1);
                    Assert.AreEqual("add 2a", address.AddressLine2);
                    Assert.AreEqual("add 3a", address.AddressLine3);
                    Assert.AreEqual("citya", address.City);
                    Assert.AreEqual("countya", address.County);
                    Assert.AreEqual("postcodea", address.Postcode);
                    Assert.AreEqual("FR", address.Country);

                    bool addressDeleted = sut.DeleteDeliveryAddress(userId, deliveryAddresses[0]);
                    Assert.IsTrue(addressDeleted);

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

		[TestMethod]
		public void GetAddressOptions_SettingValueNotFound_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SettingsDataRow> settings = provider.GetService<ISimpleDBOperations<SettingsDataRow>>();
					Assert.IsNotNull(settings);

					SettingsDataRow settingsRow = settings.Select().FirstOrDefault(n => n.Name.Equals("AddressOptions"));

					Assert.IsNotNull(settingsRow);
					settings.Delete(settingsRow);

					IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

					Assert.IsNotNull(sut);

					AddressOptions results = sut.GetAddressOptions(AddressOption.Delivery);

					Assert.IsNotNull(results);
					Assert.AreEqual(0, (int)results);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetAddressOptions_ReadsValuesFromSettings_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

					Assert.IsNotNull(sut);

					AddressOptions results = sut.GetAddressOptions(AddressOption.Delivery);

					Assert.IsNotNull(results);
					Assert.AreEqual(0, (int)results);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetAddressOptions_SettingValueSetByUser_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SettingsDataRow> settings = provider.GetService<ISimpleDBOperations<SettingsDataRow>>();
					Assert.IsNotNull(settings);

					SettingsDataRow settingsRow = settings.Select().FirstOrDefault(n => n.Name.Equals("AddressOptions"));

					Assert.IsNotNull(settingsRow);
					settingsRow.Value = "13084";
					settings.Update(settingsRow);

					IAccountProvider sut = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;

					Assert.IsNotNull(sut);

					AddressOptions results = sut.GetAddressOptions(AddressOption.Delivery);

					Assert.IsNotNull(results);
					Assert.IsTrue(results.HasFlag(AddressOptions.AddressLine1Show));
					Assert.IsTrue(results.HasFlag(AddressOptions.AddressLine1Mandatory));
					Assert.IsTrue(results.HasFlag(AddressOptions.AddressLine2Show));
					Assert.IsTrue(results.HasFlag(AddressOptions.CityShow));
					Assert.IsTrue(results.HasFlag(AddressOptions.CityMandatory));
					Assert.IsTrue(results.HasFlag(AddressOptions.PostCodeShow));
					Assert.IsTrue(results.HasFlag(AddressOptions.PostCodeMandatory));
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}

