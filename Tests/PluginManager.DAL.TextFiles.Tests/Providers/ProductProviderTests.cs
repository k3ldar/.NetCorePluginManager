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
 *  File: ProductProviderTests.cs
 *
 *  Purpose:  product provider test for text based storage
 *
 *  Date        Name                Reason
 *  07/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Products;

using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using SimpleDB;
using SimpleDB.Tests.Mocks;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductProviderTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_ParamProductDataNull_Throws_ArgumentNullException()
        {
            new ProductProvider(null, new MockTextTableOperations<ProductGroupDataRow>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_ParamProductGroupDataNull_Throws_ArgumentNullException()
        {
            new ProductProvider(new MockTextTableOperations<ProductDataRow>(), null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
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

					ISimpleDBOperations<ProductGroupDataRow> prodGroups = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(prodGroups);

                    IReadOnlyList<ProductGroupDataRow> allRows = prodGroups.Select();

                    Assert.IsNotNull(allRows);
                    // contains default group
                    Assert.AreEqual(1, allRows.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupDelete_OnlyOneGroupRemains_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupDelete(0, out string err);
                    Assert.IsFalse(result);
                    Assert.AreEqual("There must be at least 1 product group", err);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupDelete_GroupNotFound_ReturnsFalse()
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

					ISimpleDBOperations<ProductGroupDataRow> prodGroups = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(prodGroups);

                    prodGroups.Insert(new ProductGroupDataRow() { Description = "test group" });

                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupDelete(-100, out string err);
                    Assert.IsFalse(result);
                    Assert.AreEqual("The product group could not be found", err);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupDelete_GroupContainsProducts_ReturnsFalse()
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

					ISimpleDBOperations<ProductGroupDataRow> prodGroups = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(prodGroups);

                    prodGroups.Insert(new ProductGroupDataRow() { Description = "test group" });


                    ISimpleDBOperations<ProductDataRow> prodData = provider.GetRequiredService<ISimpleDBOperations<ProductDataRow>>();
                    Assert.IsNotNull(prodData);

                    prodData.Insert(new ProductDataRow() { Description = "test product description which should be at least 20 chars", Name = "product name", ProductGroupId = 1 });

                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupDelete(1, out string err);
                    Assert.IsFalse(result);
                    Assert.AreEqual("The product group contains products", err);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupDelete_GroupDeleted_ReturnsTrue()
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

					ISimpleDBOperations<ProductGroupDataRow> prodGroups = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(prodGroups);

                    prodGroups.Insert(new ProductGroupDataRow() { Description = "test group" });

                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupDelete(1, out string err);
                    Assert.IsTrue(result);
                    Assert.AreEqual("", err);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupGet_GroupNotFound_ReturnsNull()
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

					ISimpleDBOperations<ProductGroupDataRow> prodGroups = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(prodGroups);

                    prodGroups.Insert(new ProductGroupDataRow() { Description = "test group" });

                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    ProductGroup result = sut.ProductGroupGet(-1);
                    Assert.IsNull(result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupGet_GroupFound_ReturnsProductGroup()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    ProductGroup result = sut.ProductGroupGet(1);
                    Assert.IsNotNull(result);
                    Assert.AreEqual("Default Product Group", result.Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupsGet_ReturnsDefaultProductGroup()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    List<ProductGroup> result = sut.ProductGroupsGet();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(1, result.Count);
                    Assert.AreEqual("Default Product Group", result[0].Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupsGet_ReturnsAllProductGroups_PreSorted()
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

					ISimpleDBOperations<ProductGroupDataRow> prodGroups = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(prodGroups);

                    prodGroups.Insert(new ProductGroupDataRow() { Description = "test 1", SortOrder = 5 });
                    prodGroups.Insert(new ProductGroupDataRow() { Description = "test 2", SortOrder = 1 });
                    prodGroups.Insert(new ProductGroupDataRow() { Description = "test 3", SortOrder = 3 });


                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    List<ProductGroup> result = sut.ProductGroupsGet();
                    Assert.IsNotNull(result);
                    Assert.AreEqual(4, result.Count);
                    Assert.AreEqual("Default Product Group", result[0].Description);
                    Assert.AreEqual("test 2", result[1].Description);
                    Assert.AreEqual("test 3", result[2].Description);
                    Assert.AreEqual("test 1", result[3].Description);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupSave_ExistingRecord_DescriptionTooShort_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupSave(1, "tst", true, 5, null, null, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Minimum length for description is 5 characters; Table: ProductGroupDataRow; Property Description", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupSave_NewRecord_DescriptionTooShort_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupSave(-1, "tst", true, 5, null, null, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Minimum length for description is 5 characters; Table: ProductGroupDataRow; Property Description", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupSave_NewRecord_DescriptionNull_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupSave(-3, null, true, 5, null, null, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Can not be null or empty; Table: ProductGroupDataRow; Property Description", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupSave_NewRecord_DescriptionTooLong_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupSave(-1, "This is my exceedingly long description for a product group and it is clearly too long to be of any use", true, 5, null, null, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Maximum length for description is 50 characters; Table: ProductGroupDataRow; Property Description", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductGroupSave_NewRecord_CreatesProductGroup_ReturnsTrue()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductGroupSave(-1, "Fancy Products", true, 5, null, null, out string errorMessage);
                    Assert.IsTrue(result);
                    Assert.AreEqual("", errorMessage);

                    ISimpleDBOperations<ProductGroupDataRow> prodGroups = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(prodGroups);
                    Assert.AreEqual(2, prodGroups.RecordCount);

                    ProductGroupDataRow newGroup = prodGroups.Select().FirstOrDefault(pg => pg.Description.Equals("Fancy Products"));
                    Assert.IsNotNull(newGroup);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetProduct_ProductDoesNotExist_ReturnsNull()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    Product result = sut.GetProduct(1);
                    Assert.IsNull(result);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetProduct_ProductExists_ReturnsProduct()
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

                    ISimpleDBOperations<ProductDataRow> productsData = provider.GetRequiredService<ISimpleDBOperations<ProductDataRow>>();
                    Assert.IsNotNull(productsData);

					ISimpleDBOperations<StockDataRow> stocksData = provider.GetRequiredService<ISimpleDBOperations<StockDataRow>>();
					Assert.IsNotNull(stocksData);
					Assert.AreEqual(0, stocksData.RecordCount);

                    productsData.Insert(new ProductDataRow()
                    {
                        AllowBackorder = true,
                        BestSeller = false,
                        Description = "The product description goes here",
                        Features = "",
                        IsDownload = false,
                        Name = "My Product",
						Sku = "Prod1"
                    });

					Assert.AreEqual(1, stocksData.RecordCount);

                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    Product result = sut.GetProduct(0);
                    Assert.IsNotNull(result);
                    Assert.AreEqual("My Product", result.Name);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetProducts_FiveProductsFromPageTwo_ReturnsProducts()
        {
            string directory = TestHelper.GetTestPath();
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new();
                ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ISimpleDBOperations<ProductGroupDataRow> productsGroupData = provider.GetRequiredService<ISimpleDBOperations<ProductGroupDataRow>>();
                    Assert.IsNotNull(productsGroupData);

                    productsGroupData.Insert(new ProductGroupDataRow()
                    {
                        Description = "New Product Group"
                    });

                    ISimpleDBOperations<ProductDataRow> productsData = provider.GetRequiredService<ISimpleDBOperations<ProductDataRow>>();
                    Assert.IsNotNull(productsData);

                    for (int i = 0; i < 30; i++)
                    {
                        productsData.Insert(new ProductDataRow()
                        {
                            AllowBackorder = true,
                            BestSeller = false,
                            Description = $"The product {i} description goes here",
                            Features = "",
                            IsDownload = false,
                            Name = $"My Product {i}",
                            Sku = $"SKU{i}",
                            ProductGroupId = i % 2 == 0 ? 0 : 1,
                        });
                    }

                    IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    List<Product> result = sut.GetProducts(new ProductGroup(1, "adf", true, 1, "", ""), 2, 5);
                    Assert.IsNotNull(result);
                    Assert.AreEqual(5, result.Count);
                    Assert.AreEqual("My Product 19", result[0].Name);
                    Assert.AreEqual("My Product 21", result[1].Name);
                    Assert.AreEqual("My Product 23", result[2].Name);
                    Assert.AreEqual("My Product 25", result[3].Name);
                    Assert.AreEqual("My Product 27", result[4].Name);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductSave_NewRecord_InvalidName_TooShort_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductSave(-1, 0, "adf", "my product description", "", "",
                        true, true, 1.99m, "sku", false, true, true, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Minimum length for Name is 5 characters; Table: ProductDataRow; Property Name", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductSave_ExistingRecord_InvalidName_TooShort_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductSave(-1, 0, "Test Product", "my product description", "", "",
                        true, true, 1.99m, "sku", false, true, true, out string errorMessage);
                    Assert.IsTrue(result);

                    result = sut.ProductSave(0, 0, "tst", "My product description", "", "",
                        true, true, 1.99m, "sku", false, true, true, out errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Minimum length for Name is 5 characters; Table: ProductDataRow; Property Name", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductSave_NewRecord_InvalidName_TooLong_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductSave(-1, 0, "The maximum length of a product name is capped at one hundred charactgers which this piece of text will exceed", "my product description", "", "",
                        true, true, 1.99m, "sku", false, true, true, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Maximum length for Name is 100 characters; Table: ProductDataRow; Property Name", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductSave_NewRecord_InvalidName_Null_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductSave(-1, 0, null, "my product description", "", "",
                        true, true, 1.99m, "sku", false, true, true, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Can not be null or empty; Table: ProductDataRow; Property Name", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductSave_NewRecord_InvalidDescription_TooShort_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductSave(-1, 0, "My product", "desc", "", "",
                        true, true, 1.99m, "sku", false, true, true, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Minimum length for Description is 20 characters; Table: ProductDataRow; Property Description", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void ProductSave_NewRecord_InvalidDescription_Null_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
                    Assert.IsNotNull(sut);

                    bool result = sut.ProductSave(-1, 0, "My Product", null, "", "",
                        true, true, 1.99m, "sku", false, true, true, out string errorMessage);
                    Assert.IsFalse(result);
                    Assert.AreEqual("Can not be null or empty; Table: ProductDataRow; Property Description", errorMessage);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

		[TestMethod]
		public void ProductSave_NewRecord_InvalidSku_Null_ReturnsFalse()
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

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
					Assert.IsNotNull(sut);

					bool result = sut.ProductSave(-1, 0, "My Product", "This is the product description", "", "",
						true, true, 1.99m, null, false, true, true, out string errorMessage);
					Assert.IsFalse(result);
					Assert.AreEqual("Minimum length for Sku is 3 characters; Table: ProductDataRow; Property Sku", errorMessage);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ProductSave_IsVisibleSetToFalse_SavesCorrectly()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					mockPluginClassesService.Provider = provider;
					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
					Assert.IsNotNull(sut);

					bool result = sut.ProductSave(-1, 0, "My Product", "This is the product description", "", "",
						true, true, 1.99m, "sku", false, true, false, out string errorMessage);
					Assert.IsTrue(result);
					Assert.AreEqual("", errorMessage);

					Product product = sut.GetProduct(0);
					Assert.IsNotNull(product);
					Assert.IsFalse(product.IsVisible);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ProductCreated_StockRecordCreated_StockRecordDeletedWhenProductDeleted()
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

					ISimpleDBOperations<ProductDataRow> productsData = provider.GetRequiredService<ISimpleDBOperations<ProductDataRow>>();
					Assert.IsNotNull(productsData);

					ISimpleDBOperations<StockDataRow> stocksData = provider.GetRequiredService<ISimpleDBOperations<StockDataRow>>();
					Assert.IsNotNull(stocksData);
					Assert.AreEqual(0, stocksData.RecordCount);

					ProductDataRow newProduct = new()
					{
						AllowBackorder = true,
						BestSeller = false,
						Description = "The product description goes here",
						Features = "",
						IsDownload = false,
						Name = "My Product",
						Sku = "Prod1"
					};

					productsData.Insert(newProduct);

					Assert.AreEqual(1, stocksData.RecordCount);

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
					Assert.IsNotNull(sut);

					Product result = sut.GetProduct(0);
					Assert.IsNotNull(result);
					Assert.AreEqual("My Product", result.Name);

					productsData.Delete(newProduct);
					Assert.AreEqual(0, productsData.RecordCount);

					Assert.AreEqual(0, stocksData.RecordCount);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ProductCreated_StockRecordCreated_StockRecordNotDeletedWhenProductDeletedAsStockAvailable()
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

					ISimpleDBOperations<ProductDataRow> productsData = provider.GetRequiredService<ISimpleDBOperations<ProductDataRow>>();
					Assert.IsNotNull(productsData);

					ISimpleDBOperations<StockDataRow> stocksData = provider.GetRequiredService<ISimpleDBOperations<StockDataRow>>();
					Assert.IsNotNull(stocksData);
					Assert.AreEqual(0, stocksData.RecordCount);

					IStockProvider stockProvider = provider.GetRequiredService<IStockProvider>();
					Assert.IsNotNull(stockProvider);

					ProductDataRow newProduct = new()
					{
						AllowBackorder = true,
						BestSeller = false,
						Description = "The product description goes here",
						Features = "",
						IsDownload = false,
						Name = "My Product",
						Sku = "Prod1"
					};

					productsData.Insert(newProduct);

					Assert.AreEqual(1, stocksData.RecordCount);

					IProductProvider sut = provider.GetRequiredService<IProductProvider>();
					Assert.IsNotNull(sut);

					Product result = sut.GetProduct(0);
					Assert.IsNotNull(result);
					Assert.AreEqual("My Product", result.Name);

					stockProvider.AddStockToProduct(result, 2, out string error);
					bool exceptionRaised = false;
					try
					{
						productsData.Delete(newProduct);
					}
					catch (InvalidDataRowException ex)
					{
						Assert.IsTrue(ex.Message.Contains("Unable to delete a product when stock is available."));
						exceptionRaised = true;
					}

					Assert.IsTrue(exceptionRaised);
					Assert.AreEqual(1, productsData.RecordCount);
					Assert.AreEqual(1, stocksData.RecordCount);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}
