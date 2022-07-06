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
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Products;
using Middleware.ShoppingCart;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.DAL.TextFiles.Tables.Products;
using PluginManager.DAL.TextFiles.Tests.Mocks;
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
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
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
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
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

                    sut.ClearCache();
                    ShoppingCartDetail basket = sut.GetDetail(basketId);
                    Assert.IsNotNull(basket);
                    Assert.AreEqual("", basket.CouponCode);
                    Assert.AreEqual("GBP", basket.CurrencyCode);
                    Assert.AreEqual(0, basket.Discount);
                    Assert.AreEqual(1, basket.Items.Count);
                    Assert.AreEqual(0, basket.Shipping);
                    Assert.AreEqual(1.99m, basket.SubTotal);
                    Assert.AreEqual(0.34m, basket.Tax);
                    Assert.AreEqual(20, basket.TaxRate);
                    Assert.AreEqual(1.99m, basket.Total);
                    Assert.AreEqual(1, basket.TotalItems);
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
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
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

                    sut.ClearCache();

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

                    ShoppingCartDataRow savedBasket = shoppingCartData.Select().FirstOrDefault();
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
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
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

                    long basketId = sut.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);

                    sut.ClearCache();

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
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
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

                    sut.ClearCache();

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

                    ShoppingCartDataRow savedBasket = shoppingCartData.Select().FirstOrDefault();
                    Assert.AreEqual(1, savedBasket.Id);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }


        private IProductProvider GetTestProductProvider(ServiceProvider provider, bool addProducts = true)
        {
            IProductProvider Result = provider.GetService<IProductProvider>();
            Assert.IsNotNull(Result);

            if (addProducts)
            {
                if (!Result.ProductSave(-1, 1, "test product", "This is a description of my test product", "", "", true, false, 1.99m, "sku1", false, true, out string errorMessage))
                    throw new InvalidOperationException("product should have saved; Error: " + errorMessage);

                if (!Result.ProductSave(-1, 1, "test product", "This is a description of my test product", "", "", true, false, 1.99m, "sku2", false, true, out errorMessage))
                    throw new InvalidOperationException("product should have saved; Error: " + errorMessage);

                if (!Result.ProductSave(-1, 1, "test product", "This is a description of my test product", "", "", true, false, 1.99m, "sku3", false, true, out errorMessage))
                    throw new InvalidOperationException("product should have saved; Error: " + errorMessage);
            }

            return Result;
        }
    }
}
