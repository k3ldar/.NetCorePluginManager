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
 *  File: LicenseProviderTests.cs
 *
 *  Purpose:  License Provider tests Tests for text based storage
 *
 *  Date        Name                Reason
 *  16/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Licences;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;

using SimpleDB;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
    [ExcludeFromCodeCoverage]
    public class LicenseProviderTests : BaseProviderTests
    {
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

                    LicenceProvider sut = provider.GetService(typeof(ILicenceProvider)) as LicenceProvider;

                    Assert.IsNotNull(sut);
                    Assert.IsInstanceOfType(sut, typeof(ILicenceProvider));
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicenceTypesGet_NoLicenseTypesAvailable_ReturnsEmptyList()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    ISimpleDBOperations<LicenseTypeDataRow> licenseTypeTable = (ISimpleDBOperations<LicenseTypeDataRow>)provider.GetService(typeof(ISimpleDBOperations<LicenseTypeDataRow>));
                    Assert.IsNotNull(licenseTypeTable);
                    Assert.AreEqual(0, licenseTypeTable.RecordCount);

                    List<LicenceType> result = sut.LicenceTypesGet();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(0, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicenceTypesGet_MultipleLicenseTypesAvailable_ReturnsList()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    ISimpleDBOperations<LicenseTypeDataRow> licenseTypeTable = (ISimpleDBOperations<LicenseTypeDataRow>)provider.GetService(typeof(ISimpleDBOperations<LicenseTypeDataRow>));
                    Assert.IsNotNull(licenseTypeTable);
                    Assert.AreEqual(0, licenseTypeTable.RecordCount);
                    licenseTypeTable.Insert(new List<LicenseTypeDataRow>()
                    {
                        new LicenseTypeDataRow() { Description = "License Type 1" },
                        new LicenseTypeDataRow() { Description = "License Type 2" },
                        new LicenseTypeDataRow() { Description = "License Type 3" }
                    });

                    List<LicenceType> result = sut.LicenceTypesGet();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(3, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicencesGet_UserDoesNotExist_ReturnsEmptyList()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);


                    List<Licence> result = sut.LicencesGet(23);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(0, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicencesGet_FindsUserLicences_ReturnsList()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
                    Assert.IsNotNull(accountProvider);

                    accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    DeliveryAddress deliveryAddress = new DeliveryAddress(-1, "", "Street 1", "", "", "city", "county", "postcode", "GB", 5.99m);
                    accountProvider.AddDeliveryAddress(userId, deliveryAddress);

                    ISimpleDBOperations<OrderDataRow> orderData = provider.GetService<ISimpleDBOperations<OrderDataRow>>();
                    Assert.IsNotNull(orderData);

                    ISimpleDBOperations<OrderItemDataRow> orderItemsData = provider.GetService<ISimpleDBOperations<OrderItemDataRow>>();
                    Assert.IsNotNull(orderItemsData);

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

                    ISimpleDBOperations<LicenseTypeDataRow> licenseType = provider.GetService<ISimpleDBOperations<LicenseTypeDataRow>>();
                    Assert.IsNotNull(licenseType);

                    licenseType.Insert(new List<LicenseTypeDataRow>()
                    {
                        new LicenseTypeDataRow() { Description = "license type 1"},
                        new LicenseTypeDataRow() { Description = "license type 2"},
                    });

                    ISimpleDBOperations<LicenseDataRow> licenseTable = (ISimpleDBOperations<LicenseDataRow>)provider.GetService(typeof(ISimpleDBOperations<LicenseDataRow>));
                    Assert.IsNotNull(licenseTable);

                    accountProvider.OrderPaid(accountProvider.OrdersGet(userId)[0], PaymentStatus.PaidCash, "Paid by cash");

                    licenseTable.Insert(new List<LicenseDataRow>()
                    {
                        new LicenseDataRow() { UserId = userId, LicenseType = 1, StartDateTicks = DateTime.Now.AddDays(-1).Ticks,
                            DomainName = "", ExpireDateTicks = DateTime.Now.AddDays(1).Ticks, InvoiceId = 0, IsValid = true, EncryptedLicense = "blah" },
                        new LicenseDataRow() { UserId = userId, LicenseType = 0, StartDateTicks = DateTime.Now.AddDays(-1).Ticks,
                            DomainName = "", ExpireDateTicks = DateTime.Now.AddDays(1).Ticks, InvoiceId = 0, IsValid = true, EncryptedLicense = "blah" },
                    });


                    List<Licence> result = sut.LicencesGet(userId);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(2, result.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicencesUpdateDomain_LicenseIsNull_ReturnsFalse()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    Assert.IsFalse(sut.LicenceUpdateDomain(1, null, "domain"));
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicencesUpdateDomain_DomainIsNull_ReturnsFalse()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    Assert.IsFalse(sut.LicenceUpdateDomain(1, new Licence(1, 1, new LicenceType(1, "license type"), DateTime.Now, DateTime.Now, true, true, 0, 1, "domain", "afd"), null));
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicencesUpdateDomain_UserNotFound_ReturnsFalse()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    Assert.IsFalse(sut.LicenceUpdateDomain(5, new Licence(1, 1, new LicenceType(1, "license type"), DateTime.Now, DateTime.Now, true, true, 0, 1, "domain", "afd"), "domain"));
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicenceUpdateDomain_DomainUpdated_IncrementCountUpdated_ReturnsTrue()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
                    Assert.IsNotNull(accountProvider);

                    accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    DeliveryAddress deliveryAddress = new DeliveryAddress(-1, "", "Street 1", "", "", "city", "county", "postcode", "GB", 5.99m);
                    accountProvider.AddDeliveryAddress(userId, deliveryAddress);

                    ISimpleDBOperations<OrderDataRow> orderData = provider.GetService<ISimpleDBOperations<OrderDataRow>>();
                    Assert.IsNotNull(orderData);

                    ISimpleDBOperations<OrderItemDataRow> orderItemsData = provider.GetService<ISimpleDBOperations<OrderItemDataRow>>();
                    Assert.IsNotNull(orderItemsData);

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

                    accountProvider.OrderPaid(accountProvider.OrdersGet(userId)[0], PaymentStatus.PaidCash, "Paid by cash");

                    ISimpleDBOperations<LicenseTypeDataRow> licenseType = provider.GetService<ISimpleDBOperations<LicenseTypeDataRow>>();
                    Assert.IsNotNull(licenseType);

                    licenseType.Insert(new List<LicenseTypeDataRow>()
                    {
                        new LicenseTypeDataRow() { Description = "license type 1"},
                        new LicenseTypeDataRow() { Description = "license type 2"},
                    });

                    ISimpleDBOperations<LicenseDataRow> licenseTable = (ISimpleDBOperations<LicenseDataRow>)provider.GetService(typeof(ISimpleDBOperations<LicenseDataRow>));
                    Assert.IsNotNull(licenseTable);

                    licenseTable.Insert(new List<LicenseDataRow>()
                    {
                        new LicenseDataRow() { UserId = userId, LicenseType = 1, StartDateTicks = DateTime.Now.AddDays(-1).Ticks,
                            DomainName = "", ExpireDateTicks = DateTime.Now.AddDays(1).Ticks, InvoiceId = 0, IsValid = true, EncryptedLicense = "blah" },
                        new LicenseDataRow() { UserId = userId, LicenseType = 0, StartDateTicks = DateTime.Now.AddDays(-1).Ticks,
                            DomainName = "", ExpireDateTicks = DateTime.Now.AddDays(1).Ticks, InvoiceId = 0, IsValid = true, EncryptedLicense = "blah" },
                    });


                    List<Licence> licenses = sut.LicencesGet(userId);
                    Assert.IsNotNull(licenses);
                    Assert.AreEqual(2, licenses.Count);

                    Licence license = licenses[0];
                    Assert.AreEqual(0, license.UpdateCount);

                    bool result = sut.LicenceUpdateDomain(userId, license,"my domain");
                    Assert.IsTrue(result);

                    LicenseDataRow licenseDataRow = licenseTable.Select(license.Id);
                    Assert.IsNotNull(licenseDataRow);
                    Assert.AreEqual(1, licenseDataRow.UpdateCount);
                    Assert.AreEqual("my domain", licenseDataRow.DomainName);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void LicenceTrialCreate_LicenseTypeNull_Throws_ArgumentNullException()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    sut.LicenceTrialCreate(5, null);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicenceTrialCreate_UserNotFound_ReturnsFailed()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    LicenceCreate Result = sut.LicenceTrialCreate(5, new LicenceType(1, "test"));
                    Assert.AreEqual(LicenceCreate.Failed, Result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicenceTrialCreate_TrialLicenseAlreadyExists_ReturnsExisting()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
                    Assert.IsNotNull(accountProvider);

                    accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    ISimpleDBOperations<LicenseTypeDataRow> licenseType = provider.GetService<ISimpleDBOperations<LicenseTypeDataRow>>();
                    Assert.IsNotNull(licenseType);

                    licenseType.Insert(new List<LicenseTypeDataRow>()
                    {
                        new LicenseTypeDataRow() { Description = "license type 1"},
                        new LicenseTypeDataRow() { Description = "license type 2"},
                    });

                    ISimpleDBOperations<LicenseDataRow> licenseTable = (ISimpleDBOperations<LicenseDataRow>)provider.GetService(typeof(ISimpleDBOperations<LicenseDataRow>));
                    Assert.IsNotNull(licenseTable);

                    licenseTable.Insert(new List<LicenseDataRow>()
                    {
                        new LicenseDataRow() { UserId = userId, LicenseType = 1, StartDateTicks = DateTime.Now.AddDays(-1).Ticks,
                            DomainName = "", ExpireDateTicks = DateTime.Now.AddDays(1).Ticks, InvoiceId = 0, IsValid = false, IsTrial = true, EncryptedLicense = "blah" },
                        new LicenseDataRow() { UserId = userId, LicenseType = 0, StartDateTicks = DateTime.Now.AddDays(-1).Ticks,
                            DomainName = "", ExpireDateTicks = DateTime.Now.AddDays(1).Ticks, InvoiceId = 0, IsValid = true, IsTrial = true, EncryptedLicense = "blah" },
                    });

                    LicenceCreate Result = sut.LicenceTrialCreate(userId, sut.LicenceTypesGet()[0]);
                    Assert.AreEqual(LicenceCreate.Existing, Result);

                    Result = sut.LicenceTrialCreate(userId, sut.LicenceTypesGet()[1]);
                    Assert.AreEqual(LicenceCreate.Existing, Result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void LicenceTrialCreate_UserValidAndDoesNotHaveTrialLicense_ReturnsSuccess()
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

                    ILicenceProvider sut = provider.GetService<ILicenceProvider>();
                    Assert.IsNotNull(sut);

                    IAccountProvider accountProvider = provider.GetService(typeof(IAccountProvider)) as IAccountProvider;
                    Assert.IsNotNull(accountProvider);

                    accountProvider.CreateAccount("me@here.com", "Joe", "Bloggs", "password", "", "", "", "", "", "", "", "", "US", out long userId);

                    ISimpleDBOperations<LicenseTypeDataRow> licenseType = provider.GetService<ISimpleDBOperations<LicenseTypeDataRow>>();
                    Assert.IsNotNull(licenseType);

                    licenseType.Insert(new List<LicenseTypeDataRow>()
                    {
                        new LicenseTypeDataRow() { Description = "license type 1"},
                        new LicenseTypeDataRow() { Description = "license type 2"},
                    });

                    ISimpleDBOperations<LicenseDataRow> licenseTable = (ISimpleDBOperations<LicenseDataRow>)provider.GetService(typeof(ISimpleDBOperations<LicenseDataRow>));
                    Assert.IsNotNull(licenseTable);
                    Assert.AreEqual(0, licenseTable.RecordCount);

                    LicenceCreate Result = sut.LicenceTrialCreate(userId, sut.LicenceTypesGet()[0]);
                    Assert.AreEqual(LicenceCreate.Success, Result);
                    Assert.AreEqual(1, licenseTable.RecordCount);

                    Result = sut.LicenceTrialCreate(userId, sut.LicenceTypesGet()[1]);
                    Assert.AreEqual(LicenceCreate.Success, Result);
                    Assert.AreEqual(2, licenseTable.RecordCount);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
