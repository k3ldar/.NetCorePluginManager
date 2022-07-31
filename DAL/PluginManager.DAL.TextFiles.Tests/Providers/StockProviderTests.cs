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
 *  File: StockProviderTests.cs
 *
 *  Purpose:  Stock provider test for text based storage
 *
 *  Date        Name                Reason
 *  02/07/2022  Simon Carter        Initially Created
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
using Middleware.Products;
using Middleware.ShoppingCart;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.DAL.TextFiles.Tables.Products;
using SimpleDB;

using Shared.Classes;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class StockProviderTests : BaseProviderTests
    {
        [TestMethod]
        public void GetStockAvailability_SingleShoppingCartItem_ReturnsCorrectStockCount()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture, "GBP");

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<StockDataRow> stockTable = provider.GetRequiredService<ISimpleDBOperations<StockDataRow>>();
                    Assert.IsNotNull(stockTable);

                    IProductProvider productProvider = GetTestProductProvider(provider);

                    stockTable.Insert(new List<StockDataRow>()
                    {
                        new StockDataRow() { ProductId = 1, StockAvailability = 12 }
                    });

                    ShoppingCartProvider shoppingCartProvider = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(shoppingCartProvider);
                    ShoppingCartProvider.ClearCache();

                    long basketId = shoppingCartProvider.AddToCart(userSession, shoppingCart, productProvider.GetProduct(1), 1);
                    Assert.AreEqual(1, basketId);

                    ShoppingCartDetail basketDetail = shoppingCartProvider.GetDetail(basketId);
                    Assert.IsNotNull(basketDetail);

                    IStockProvider sut = provider.GetService<IStockProvider>();
                    Assert.IsNotNull(sut);

                    Assert.AreEqual(0u, basketDetail.Items[0].StockAvailability);

                    sut.GetStockAvailability(basketDetail.Items[0]);

                    Assert.AreEqual(12u, basketDetail.Items[0].StockAvailability);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetStockAvailability_MultipleShoppingCartItem_ReturnsCorrectStockCount()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture, "GBP");

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<StockDataRow> stockTable = provider.GetRequiredService<ISimpleDBOperations<StockDataRow>>();
                    Assert.IsNotNull(stockTable);

                    IProductProvider productProvider = GetTestProductProvider(provider);

                    stockTable.Insert(new List<StockDataRow>()
                    {
                        new StockDataRow() { ProductId = 0, StockAvailability = 16 },
                        new StockDataRow() { ProductId = 1, StockAvailability = 28 },
                        new StockDataRow() { ProductId = 2, StockAvailability = 3 }
                    });

                    ShoppingCartProvider shoppingCartProvider = (ShoppingCartProvider)provider.GetService<IShoppingCartProvider>();
                    Assert.IsNotNull(shoppingCartProvider);
                    ShoppingCartProvider.ClearCache();

                    long basketId = shoppingCartProvider.AddToCart(userSession, shoppingCart, productProvider.GetProduct(0), 1);
                    Assert.AreEqual(1, basketId);
                    shoppingCartProvider.AddToCart(userSession, shoppingCart, productProvider.GetProduct(1), 1);
                    shoppingCartProvider.AddToCart(userSession, shoppingCart, productProvider.GetProduct(2), 1);

                    ShoppingCartDetail basketDetail = shoppingCartProvider.GetDetail(basketId);
                    Assert.IsNotNull(basketDetail);

                    IStockProvider sut = provider.GetService<IStockProvider>();
                    Assert.IsNotNull(sut);

                    Assert.AreEqual(0u, basketDetail.Items[0].StockAvailability);
                    Assert.AreEqual(0u, basketDetail.Items[1].StockAvailability);
                    Assert.AreEqual(0u, basketDetail.Items[2].StockAvailability);

                    sut.GetStockAvailability(basketDetail.Items);

                    Assert.AreEqual(16u, basketDetail.Items[0].StockAvailability);
                    Assert.AreEqual(28u, basketDetail.Items[1].StockAvailability);
                    Assert.AreEqual(3u, basketDetail.Items[2].StockAvailability);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetStockAvailability_SingleProduct_ReturnsCorrectStockCount()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                UserSession userSession = new UserSession();

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<StockDataRow> stockTable = provider.GetRequiredService<ISimpleDBOperations<StockDataRow>>();
                    Assert.IsNotNull(stockTable);

                    IProductProvider productProvider = GetTestProductProvider(provider);

                    stockTable.Insert(new List<StockDataRow>()
                    {
                        new StockDataRow() { ProductId = 1, StockAvailability = 17 }
                    });

                    IStockProvider sut = provider.GetService<IStockProvider>();
                    Assert.IsNotNull(sut);

                    Product product = productProvider.GetProduct(1);
                    Assert.AreEqual(0u, product.StockAvailability);

                    sut.GetStockAvailability(product);

                    Assert.AreEqual(17u, product.StockAvailability);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetStockAvailability_MultipleProducts_ReturnsCorrectStockCount()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                UserSession userSession = new UserSession();
                ShoppingCartSummary shoppingCart = new ShoppingCartSummary(0, 0, 0, 0, 0, 20,
                    System.Threading.Thread.CurrentThread.CurrentUICulture, "GBP");

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<StockDataRow> stockTable = provider.GetRequiredService<ISimpleDBOperations<StockDataRow>>();
                    Assert.IsNotNull(stockTable);

                    IProductProvider productProvider = GetTestProductProvider(provider);

                    stockTable.Insert(new List<StockDataRow>()
                    {
                        new StockDataRow() { ProductId = 0, StockAvailability = 19 },
                        new StockDataRow() { ProductId = 1, StockAvailability = 2678 },
                        new StockDataRow() { ProductId = 2, StockAvailability = 514 }
                    });

                    List<Product> products = productProvider.GetProducts(1, 20);
                    Assert.IsNotNull(products);

                    IStockProvider sut = provider.GetService<IStockProvider>();
                    Assert.IsNotNull(sut);

                    Assert.AreEqual(0u, products[0].StockAvailability);
                    Assert.AreEqual(0u, products[1].StockAvailability);
                    Assert.AreEqual(0u, products[2].StockAvailability);

                    sut.GetStockAvailability(products);

                    Assert.AreEqual(19u, products[0].StockAvailability);
                    Assert.AreEqual(2678u, products[1].StockAvailability);
                    Assert.AreEqual(514u, products[2].StockAvailability);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
