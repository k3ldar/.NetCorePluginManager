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
 *  File: ShoppingCartProviderTests.cs
 *
 *  Purpose:  Shopping cart provider test for text based storage
 *
 *  Date        Name                Reason
 *  02/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Accounts;
using Middleware.Accounts.Orders;
using Middleware.ShoppingCart;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.DAL.TextFiles.Tables.Products;
using PluginManager.SimpleDB;

using Shared.Classes;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ShoppingCartProviderTests : BaseProviderTests
    {
        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),

                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IShoppingCartProvider sut = provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void AddToCart_ShoppingCartDetailCreated_ReturnsBasketId()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),

                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture,
                    "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IProductProvider productProvider = GetTestProductProvider(provider);

                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(1), 1);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartProvider.ClearCache();
                    ShoppingCartDetail basket = sut.GetDetail(basketId);
                    Assert.IsNotNull(basket);
                    Assert.AreEqual("", basket.CouponCode);
                    Assert.AreEqual("GBP", basket.CurrencyCode);
                    Assert.AreEqual(0, basket.Discount);
                    Assert.AreEqual(1, basket.Items.Count);
                    Assert.AreEqual(0, basket.Shipping);
                    Assert.AreEqual(2.99m, basket.SubTotal);
                    Assert.AreEqual(0.5m, basket.Tax);
                    Assert.AreEqual(20, basket.TaxRate);
                    Assert.AreEqual(2.99m, basket.Total);
                    Assert.AreEqual(1, basket.TotalItems);

                    ITextTableOperations<ShoppingCartItemDataRow> shoppingCartItemData = provider.GetRequiredService<ITextTableOperations<ShoppingCartItemDataRow>>();
                    Assert.IsNotNull(shoppingCartItemData);
                    Assert.AreEqual(1, shoppingCartItemData.RecordCount);
                    ShoppingCartItemDataRow shoppingCartItemDataRow = shoppingCartItemData.Select(0);
                    Assert.IsNotNull(shoppingCartItemDataRow);
                    Assert.AreEqual(1, shoppingCartItemDataRow.ProductId);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void AddToCart_HadPreviousBasketButDeleted_ShoppingCartDetailCreated_ReturnsBasketId()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),

                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture,
                    "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IProductProvider productProvider = GetTestProductProvider(provider);

                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartProvider.ClearCache();

                    ITextTableOperations<ShoppingCartItemDataRow> shoppingCartItemData = provider.GetRequiredService<ITextTableOperations<ShoppingCartItemDataRow>>();
                    shoppingCartItemData.Truncate();


                    ITextTableOperations<ShoppingCartDataRow> shoppingCartData = provider.GetRequiredService<ITextTableOperations<ShoppingCartDataRow>>();
                    shoppingCartData.Truncate();


                    ShoppingCartDetail basket = sut.GetDetail(basketId);
                    Assert.IsNotNull(basket);
                    Assert.AreEqual("", basket.CouponCode);
                    Assert.AreEqual("GBP", basket.CurrencyCode);
                    Assert.AreEqual(0, basket.Discount);
                    Assert.AreEqual(0, basket.Items.Count);
                    Assert.AreEqual(0, basket.Shipping);
                    Assert.AreEqual(0, basket.SubTotal);
                    Assert.AreEqual(0, basket.Tax);
                    Assert.AreEqual(20, basket.TaxRate);
                    Assert.AreEqual(0, basket.Total);
                    Assert.AreEqual(0, basket.TotalItems);

                    Assert.AreEqual(0, shoppingCartItemData.RecordCount);
                    Assert.AreEqual(1, shoppingCartData.RecordCount);

                    ShoppingCartDataRow savedBasket = shoppingCartData.Select().ToList().FirstOrDefault();
                    Assert.AreEqual(1, savedBasket.Id);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetSummary_ShoppingCartDetailCreated_ReturnsShoppingCartSummary()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),

                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture, "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IProductProvider productProvider = GetTestProductProvider(provider);

                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);
                    ShoppingCartProvider.ClearCache();

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartProvider.ClearCache();

                    ShoppingCartSummary basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);

                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0, basketSummary.Discount);
                    Assert.AreEqual(0, basketSummary.Shipping);
                    Assert.AreEqual(1.99m, basketSummary.SubTotal);
                    Assert.AreEqual(0.34m, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(1.99m, basketSummary.Total);
                    Assert.AreEqual(1, basketSummary.TotalItems);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetSummary_HadPreviousBasketButDeleted_ShoppingCartDetailCreated_ReturnsShoppingCartSummary()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),

                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture,
                    "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IProductProvider productProvider = GetTestProductProvider(provider);

                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);
                    ShoppingCartProvider.ClearCache();

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartProvider.ClearCache();

                    ITextTableOperations<ShoppingCartItemDataRow> shoppingCartItemData = provider.GetRequiredService<ITextTableOperations<ShoppingCartItemDataRow>>();
                    shoppingCartItemData.Truncate();


                    ITextTableOperations<ShoppingCartDataRow> shoppingCartData = provider.GetRequiredService<ITextTableOperations<ShoppingCartDataRow>>();
                    shoppingCartData.Truncate();


                    ShoppingCartSummary basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);
                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0, basketSummary.Discount);
                    Assert.AreEqual(0, basketSummary.Shipping);
                    Assert.AreEqual(0, basketSummary.SubTotal);
                    Assert.AreEqual(0, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(0, basketSummary.Total);
                    Assert.AreEqual(0, basketSummary.TotalItems);

                    Assert.AreEqual(0, shoppingCartItemData.RecordCount);
                    Assert.AreEqual(1, shoppingCartData.RecordCount);

                    ShoppingCartDataRow savedBasket = shoppingCartData.Select().ToList().FirstOrDefault();
                    Assert.AreEqual(1, savedBasket.Id);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ConvertToOrder_OrderCreatedSuccessfully_ReturnsTrue()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),
                    new AddressDataRowDefaults(),

                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture,
                    "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));
                    IProductProvider productProvider = GetTestProductProvider(provider);

                    IAccountProvider accountProvider = provider.GetService<IAccountProvider>();
                    Assert.IsNotNull(accountProvider);

                    bool accountCreated = accountProvider.CreateAccount("me@here.com", "test", "user", "pass", "", "",
                        "addr1", "", "", "city", "county", "zip", "DK", out long userId);
                    Assert.IsTrue(accountCreated);

                    bool shippingAddressCreated = accountProvider.AddDeliveryAddress(userId, new DeliveryAddress(-1, "", "Address Line 1", "", "", "My City", "", "Zip", "FR", 2.98m));
                    Assert.IsTrue(shippingAddressCreated);

                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);
                    ShoppingCartProvider.ClearCache();

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(1), 2);
                    Assert.AreEqual(1, basketId);

                    basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(2), 2);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartProvider.ClearCache();

                    ITextTableOperations<ShoppingCartItemDataRow> shoppingCartItemData = provider.GetRequiredService<ITextTableOperations<ShoppingCartItemDataRow>>();
                    ITextTableOperations<ShoppingCartDataRow> shoppingCartData = provider.GetRequiredService<ITextTableOperations<ShoppingCartDataRow>>();
                    ITextTableOperations<OrderItemDataRow> orderItemData = provider.GetRequiredService<ITextTableOperations<OrderItemDataRow>>();
                    ITextTableOperations<OrderDataRow> orderData = provider.GetRequiredService<ITextTableOperations<OrderDataRow>>();

                    Assert.AreEqual(0, orderItemData.RecordCount);
                    Assert.AreEqual(0, orderData.RecordCount);

                    ShoppingCartDetail cartDetail = sut.GetDetail(basketId);
                    cartDetail.SetDeliveryAddress(accountProvider.GetDeliveryAddress(userId, 1));

                    ShoppingCartSummary basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);
                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0, basketSummary.Discount);
                    Assert.AreEqual(2.98m, basketSummary.Shipping);
                    Assert.AreEqual(15.95m, basketSummary.SubTotal);
                    Assert.AreEqual(3.16m, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(18.93m, basketSummary.Total);
                    Assert.AreEqual(5, basketSummary.TotalItems);

                    Assert.AreEqual(3, shoppingCartItemData.RecordCount);
                    Assert.AreEqual(1, shoppingCartData.RecordCount);

                    ShoppingCartDataRow savedBasket = shoppingCartData.Select().ToList().FirstOrDefault();
                    Assert.AreEqual(1, savedBasket.Id);

                    bool orderCreated = sut.ConvertToOrder(basketSummary, userId, out Order order);
                    Assert.IsTrue(orderCreated);
                    Assert.IsNotNull(order);


                    // ensure basket is cleared
                    basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);
                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0, basketSummary.Discount);
                    Assert.AreEqual(0, basketSummary.Shipping);
                    Assert.AreEqual(0, basketSummary.SubTotal);
                    Assert.AreEqual(0, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(0, basketSummary.Total);
                    Assert.AreEqual(0, basketSummary.TotalItems);

                    Assert.AreEqual(0, shoppingCartItemData.RecordCount);
                    Assert.AreEqual(1, shoppingCartData.RecordCount);

                    // validate saved order details
                    Assert.AreEqual(3, orderItemData.RecordCount);
                    List<OrderItemDataRow> orderItems = orderItemData.Select().Where(oi => oi.OrderId.Equals(order.Id)).ToList();

                    Assert.AreEqual(1.99m, orderItems[0].Price);
                    Assert.AreEqual(1, orderItems[0].Quantity);
                    Assert.AreEqual("This is a description of my test product 1", orderItems[0].Description);

                    Assert.AreEqual(2.99m, orderItems[1].Price);
                    Assert.AreEqual(2, orderItems[1].Quantity);
                    Assert.AreEqual("This is a description of my test product 2", orderItems[1].Description);

                    Assert.AreEqual(3.99m, orderItems[2].Price);
                    Assert.AreEqual(2, orderItems[2].Quantity);
                    Assert.AreEqual("This is a description of my test product 3", orderItems[2].Description);

                    Assert.AreEqual(1, orderData.RecordCount);

                    OrderDataRow orderDataRow = orderData.Select().ToList().First();
                    Assert.AreEqual(2.98m, orderDataRow.Postage);
                    Assert.AreEqual("en-US", orderDataRow.Culture);
                    Assert.AreEqual(ProcessStatus.PaymentPending, (ProcessStatus)orderDataRow.ProcessStatus);
                    Assert.AreEqual(1, orderDataRow.DeliveryAddress);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [Ignore]
        [TestMethod]
        public void ConvertToOrder_OrderLicensesCreatedSuccessfully_ReturnsTrue()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),
                    new AddressDataRowDefaults(),

                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture,
                    "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    mockPluginClassesService.Items.Add(new UserDataRowDefaults(provider.GetService<ISettingsProvider>()));
                    IProductProvider productProvider = GetTestProductProvider(provider);

                    IAccountProvider accountProvider = provider.GetService<IAccountProvider>();
                    Assert.IsNotNull(accountProvider);

                    bool accountCreated = accountProvider.CreateAccount("me@here.com", "test", "user", "pass", "", "",
                        "addr1", "", "", "city", "county", "zip", "DK", out long userId);
                    Assert.IsTrue(accountCreated);

                    bool shippingAddressCreated = accountProvider.AddDeliveryAddress(userId, new DeliveryAddress(-1, "", "Address Line 1", "", "", "My City", "", "Zip", "FR", 2.98m));
                    Assert.IsTrue(shippingAddressCreated);

                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);
                    ShoppingCartProvider.ClearCache();

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(1), 2);
                    Assert.AreEqual(1, basketId);

                    basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(2), 2);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartProvider.ClearCache();

                    ITextTableOperations<ShoppingCartItemDataRow> shoppingCartItemData = provider.GetRequiredService<ITextTableOperations<ShoppingCartItemDataRow>>();
                    ITextTableOperations<ShoppingCartDataRow> shoppingCartData = provider.GetRequiredService<ITextTableOperations<ShoppingCartDataRow>>();
                    ITextTableOperations<OrderItemDataRow> orderItemData = provider.GetRequiredService<ITextTableOperations<OrderItemDataRow>>();
                    ITextTableOperations<OrderDataRow> orderData = provider.GetRequiredService<ITextTableOperations<OrderDataRow>>();
                    ITextTableOperations<LicenseDataRow> licenseData = provider.GetRequiredService<ITextTableOperations<LicenseDataRow>>();
                    ITextTableOperations<LicenseTypeDataRow> licenseTypeData = provider.GetRequiredService<ITextTableOperations<LicenseTypeDataRow>>();

                    Assert.AreEqual(0, orderItemData.RecordCount);
                    Assert.AreEqual(0, orderData.RecordCount);

                    ShoppingCartDetail cartDetail = sut.GetDetail(basketId);
                    cartDetail.SetDeliveryAddress(accountProvider.GetDeliveryAddress(userId, 1));

                    ShoppingCartSummary basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);
                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0, basketSummary.Discount);
                    Assert.AreEqual(2.98m, basketSummary.Shipping);
                    Assert.AreEqual(15.95m, basketSummary.SubTotal);
                    Assert.AreEqual(3.16m, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(18.93m, basketSummary.Total);
                    Assert.AreEqual(5, basketSummary.TotalItems);

                    Assert.AreEqual(3, shoppingCartItemData.RecordCount);
                    Assert.AreEqual(1, shoppingCartData.RecordCount);

                    ShoppingCartDataRow savedBasket = shoppingCartData.Select().ToList().FirstOrDefault();
                    Assert.AreEqual(1, savedBasket.Id);

                    bool orderCreated = sut.ConvertToOrder(basketSummary, userId, out Order order);
                    Assert.IsTrue(orderCreated);
                    Assert.IsNotNull(order);


                    // ensure basket is cleared
                    basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);
                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0, basketSummary.Discount);
                    Assert.AreEqual(0, basketSummary.Shipping);
                    Assert.AreEqual(0, basketSummary.SubTotal);
                    Assert.AreEqual(0, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(0, basketSummary.Total);
                    Assert.AreEqual(0, basketSummary.TotalItems);

                    Assert.AreEqual(0, shoppingCartItemData.RecordCount);
                    Assert.AreEqual(1, shoppingCartData.RecordCount);

                    // validate saved order details
                    Assert.AreEqual(3, orderItemData.RecordCount);
                    List<OrderItemDataRow> orderItems = orderItemData.Select().Where(oi => oi.OrderId.Equals(order.Id)).ToList();

                    Assert.AreEqual(1.99m, orderItems[0].Price);
                    Assert.AreEqual(1, orderItems[0].Quantity);
                    Assert.AreEqual("This is a description of my test product 1", orderItems[0].Description);

                    Assert.AreEqual(2.99m, orderItems[1].Price);
                    Assert.AreEqual(2, orderItems[1].Quantity);
                    Assert.AreEqual("This is a description of my test product 2", orderItems[1].Description);

                    Assert.AreEqual(3.99m, orderItems[2].Price);
                    Assert.AreEqual(2, orderItems[2].Quantity);
                    Assert.AreEqual("This is a description of my test product 3", orderItems[2].Description);

                    Assert.AreEqual(1, orderData.RecordCount);

                    OrderDataRow orderDataRow = orderData.Select().ToList().First();
                    Assert.AreEqual(2.98m, orderDataRow.Postage);
                    Assert.AreEqual("en-US", orderDataRow.Culture);
                    Assert.AreEqual(ProcessStatus.PaymentPending, (ProcessStatus)orderDataRow.ProcessStatus);
                    Assert.AreEqual(1, orderDataRow.DeliveryAddress);

                    Assert.AreEqual(2, licenseData.RecordCount);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void AddVoucher_VoucherDoesNotExist_ReturnsFalse()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),
                    new AddressDataRowDefaults(),
                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture, "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IProductProvider productProvider = GetTestProductProvider(provider);

                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);
                    ShoppingCartProvider.ClearCache();

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartSummary basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);

                    bool voucherFound = sut.ValidateVoucher(basketSummary, "not found", 23);
                    Assert.IsFalse(voucherFound);

                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0, basketSummary.Discount);
                    Assert.AreEqual(0, basketSummary.Shipping);
                    Assert.AreEqual(1.99m, basketSummary.SubTotal);
                    Assert.AreEqual(0.34m, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(1.99m, basketSummary.Total);
                    Assert.AreEqual(1, basketSummary.TotalItems);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void AddVoucher_VoucherExistsForAnyUser_ReturnsTrue()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();
                List<object> servicesList = new List<object>()
                {
                    new SettingsDataRowDefaults(),
                    new ProductGroupDataRowDefaults(),
                    new ProductGroupDataTriggers(),
                    new ProductDataTriggers(),
                    new ShoppingCartDataRowDefaults(),
                    new AddressDataRowDefaults(),
                    new VoucherDataRowTriggers(),
                };
                MockPluginClassesService mockPluginClassesService = new MockPluginClassesService(servicesList);

                services.AddSingleton<IPluginClassesService>(mockPluginClassesService);
                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture, "GBP");


                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IProductProvider productProvider = GetTestProductProvider(provider);
                    ShoppingCartProvider sut = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(sut);
                    ShoppingCartProvider.ClearCache();

                    ITextTableOperations<VoucherDataRow> voucherTable = provider.GetService<ITextTableOperations<VoucherDataRow>>();
                    Assert.IsNotNull(voucherTable);

                    voucherTable.Insert(new VoucherDataRow()
                    {
                        DiscountRate = 5,
                        DiscountType = (int)DiscountType.PercentageTotal,
                        ValidFromTicks = 0,
                        ValidToTicks = Int64.MaxValue,
                        Name = "Test Voucher"
                    });

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartSummary basketSummary = sut.GetSummary(basketId);
                    Assert.IsNotNull(basketSummary);

                    bool voucherFound = sut.ValidateVoucher(basketSummary, "Test Voucher", 23);
                    Assert.IsTrue(voucherFound);

                    Assert.AreEqual("GBP", basketSummary.CurrencyCode);
                    Assert.AreEqual(0.1m, basketSummary.Discount);
                    Assert.AreEqual(0, basketSummary.Shipping);
                    Assert.AreEqual(1.99m, basketSummary.SubTotal);
                    Assert.AreEqual(0.32m, basketSummary.Tax);
                    Assert.AreEqual(20, basketSummary.TaxRate);
                    Assert.AreEqual(1.89m, basketSummary.Total);
                    Assert.AreEqual(1, basketSummary.TotalItems);

                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
