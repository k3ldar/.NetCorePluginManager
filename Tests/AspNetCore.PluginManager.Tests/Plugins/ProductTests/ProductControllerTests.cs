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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ProductControllerTests.cs
 *
 *  Purpose:  Tests for Product Controller 
 *
 *  Date        Name                Reason
 *  02/12/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;
using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Interfaces;

using Newtonsoft.Json;

using PluginManager.Abstractions;

using ProductPlugin.Classes;
using ProductPlugin.Controllers;
using ProductPlugin.Models;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductControllerTests : BaseControllerTests
    {
        private const string TestCategoryName = "Product Manager Tests";
        private readonly List<string> _loadedPlugins = new List<string>()
        {
            "ShoppingCartPlugin.dll"
        };

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductProvider_Null_Throws_ArgumentNullException()
        {
            new ProductController(null, new MockSettingsProvider(), new MockPluginHelperService(_loadedPlugins),
                new MockStockProvider(), new MockMemoryCache(), new MockImageProvider(), new MockShoppingCartProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProvider_Null_Throws_ArgumentNullException()
        {
            new ProductController(new MockProductProvider(), null, new MockPluginHelperService(_loadedPlugins),
                new MockStockProvider(), new MockMemoryCache(), new MockImageProvider(), new MockShoppingCartProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_PluginHelperService_Null_Throws_ArgumentNullException()
        {
            new ProductController(new MockProductProvider(), new MockSettingsProvider(), null,
                new MockStockProvider(), new MockMemoryCache(), new MockImageProvider(), new MockShoppingCartProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_StockProvider_Null_Throws_ArgumentNullException()
        {
            new ProductController(new MockProductProvider(), new MockSettingsProvider(), new MockPluginHelperService(_loadedPlugins),
                null, new MockMemoryCache(), new MockImageProvider(), new MockShoppingCartProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_MemoryCache_Null_Throws_ArgumentNullException()
        {
            new ProductController(new MockProductProvider(), new MockSettingsProvider(), new MockPluginHelperService(_loadedPlugins),
                new MockStockProvider(), null, new MockImageProvider(), new MockShoppingCartProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ImageProvider_Null_Throws_ArgumentNullException()
        {
            new ProductController(new MockProductProvider(), new MockSettingsProvider(), new MockPluginHelperService(_loadedPlugins),
                new MockStockProvider(), new MockMemoryCache(), null, new MockShoppingCartProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ShoppingCartProvider_Null_Throws_ArgumentNullException()
        {
            ProductController sut = new ProductController(new MockProductProvider(), new MockSettingsProvider(), new MockPluginHelperService(_loadedPlugins),
                new MockStockProvider(), new MockMemoryCache(), new MockImageProvider(), null);

            Assert.IsInstanceOfType(sut, typeof(BaseController));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ProductController sut = new ProductController(new MockProductProvider(), new MockSettingsProvider(), new MockPluginHelperService(_loadedPlugins),
                new MockStockProvider(), new MockMemoryCache(), new MockImageProvider(), new MockShoppingCartProvider());

            Assert.IsInstanceOfType(sut, typeof(BaseController));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ProductHome_ReturnsCorrectView_Success()
        {
            const string ExpectedPagination = "<ul class=\"pagination\"><li class=\"page-item disabled\"><a class=\"page-link\" href=\"javascript: void(0)\">&laquo; " + 
                "Previous</a></li><li class=\"page-item active\"><a class=\"page-link\" href=\"/Products/Main-Products/1/Page/1/\">1</a></li><li class=\"page-item " +
                "disabled\"><a class=\"page-link\" href=\"javascript: void(0)\">Next &raquo;</a></li></ul>";
            ProductController sut = CreateProductController();

            IActionResult response = sut.Index(null);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNull(result.ViewName);

            ProductGroupModel productModel = result.Model as ProductGroupModel;

            Assert.IsNotNull(productModel);
            Assert.AreEqual(2, productModel.Breadcrumbs.Count);
            Assert.AreEqual(3, productModel.Products.Count);
            Assert.AreEqual(3, productModel.ProductCategories.Count);
            Assert.AreEqual("Main Products", productModel.Description);
            Assert.AreEqual(ExpectedPagination, productModel.Pagination);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InvalidPage_Zero_Returns_PrimaryGroupPage_Success()
        {
            ProductController sut = CreateProductController();

            IActionResult response = sut.Index(null, -150, -10);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNull(result.ViewName);

            ProductGroupModel productModel = result.Model as ProductGroupModel;

            Assert.IsNotNull(productModel);
            Assert.AreEqual(2, productModel.Breadcrumbs.Count);
            Assert.AreEqual(3, productModel.Products.Count);
            Assert.AreEqual(3, productModel.ProductCategories.Count);
            Assert.AreEqual("Main Products", productModel.Description);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InvalidProduct_NotFound_Returns_PrimaryGroupPage_Success()
        {
            MockProductProvider mockProductProvider = new MockProductProvider();
            mockProductProvider.ReturnNullForProductGroupGet = true;

            ProductController sut = CreateProductController(mockProductProvider);

            IActionResult response = sut.Index(null, null, -10);

            RedirectToActionResult result = response as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_InvalidPage_Null_Returns_PrimaryGroupPage_Success()
        {
            ProductController sut = CreateProductController();

            IActionResult response = sut.Index(null, -150, null);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNull(result.ViewName);

            ProductGroupModel productModel = result.Model as ProductGroupModel;

            Assert.IsNotNull(productModel);
            Assert.AreEqual(2, productModel.Breadcrumbs.Count);
            Assert.AreEqual(3, productModel.Products.Count);
            Assert.AreEqual(3, productModel.ProductCategories.Count);
            Assert.AreEqual("Main Products", productModel.Description);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ValidPage_Two_Returns_PrimaryGroupPage_Success()
        {
            ProductController sut = CreateProductController();

            IActionResult response = sut.Index(null, -150,0);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNull(result.ViewName);

            ProductGroupModel productModel = result.Model as ProductGroupModel;

            Assert.IsNotNull(productModel);
            Assert.AreEqual(2, productModel.Breadcrumbs.Count);
            Assert.AreEqual(3, productModel.Products.Count);
            Assert.AreEqual(3, productModel.ProductCategories.Count);
            Assert.AreEqual("Main Products", productModel.Description);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_ValidPage_Returns_PrimaryGroupPage_Success()
        {
            ProductController sut = CreateProductController();

            IActionResult response = sut.Index(null, -150, 1);

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNull(result.ViewName);

            ProductGroupModel productModel = result.Model as ProductGroupModel;

            Assert.IsNotNull(productModel);
            Assert.AreEqual(2, productModel.Breadcrumbs.Count);
            Assert.AreEqual(3, productModel.Products.Count);
            Assert.AreEqual(3, productModel.ProductCategories.Count);
            Assert.AreEqual("Main Products", productModel.Description);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Product_ProductNotFoundRedirects_To_Action()
        {
            ProductController sut = CreateProductController();

            IActionResult response = sut.Product(-150, "Invalid Product");

            RedirectToActionResult result = response as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Product_ProductGroupNotFoundRedirects_To_Action()
        {
            MockProductProvider mockProductProvider = new MockProductProvider();
            mockProductProvider.ReturnNullForProductGroupGet = true;

            ProductController sut = CreateProductController(mockProductProvider);

            IActionResult response = sut.Product(3, "Product C");

            RedirectToActionResult result = response as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Product_ProductFound_WithShoppingCart_ReturnsProductView_Success()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            ProductController sut = CreateProductController();

            IActionResult response = sut.Product(3, "Product C");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNull(result.ViewName);

            ProductModel productModel = result.Model as ProductModel;

            Assert.IsNotNull(productModel);
            Assert.AreEqual(3, productModel.Breadcrumbs.Count);
            Assert.AreEqual("This is product c", productModel.Description);
            Assert.AreEqual(3, productModel.ProductCategories.Count);
            Assert.AreEqual("ProdC", productModel.Sku);
            Assert.IsTrue(productModel.AllowAddToBasket);
            Assert.IsTrue(productModel.BestSeller);
            Assert.AreEqual("1 year guarantee", productModel.Features);
            Assert.AreEqual("Product C", productModel.Name);
            Assert.IsFalse(productModel.NewProduct);
            Assert.AreEqual(1, productModel.ProductGroupId);
            Assert.AreEqual("£1.99", productModel.RetailPrice);
            Assert.AreEqual("E7Voso411Vs", productModel.VideoLink);

            Assert.IsNotNull(productModel.AddToCart);
            Assert.AreEqual(0, productModel.AddToCart.Discount);
            Assert.AreEqual(3, productModel.AddToCart.Id);
            Assert.AreEqual(1, productModel.AddToCart.Quantity);
            Assert.AreEqual(1.99m, productModel.AddToCart.RetailPrice);
            Assert.AreEqual(5u, productModel.AddToCart.StockAvailability);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Product_ProductFound_WithoutShoppingCart_WithImages_ReturnsProductView_Success()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");

            IPluginHelperService pluginHelperService = new MockPluginHelperService(new List<string>());
            MockImageProvider mockImageProvider = MockImageProvider.CreateDefaultMockImageProviderForProductC();
            ProductController sut = CreateProductController(null, null, null, pluginHelperService, null, null, mockImageProvider);

            IActionResult response = sut.Product(3, "Product C");

            ViewResult result = response as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsNull(result.ViewName);

            ProductModel productModel = result.Model as ProductModel;

            Assert.IsNotNull(productModel);
            Assert.AreEqual(3, productModel.Breadcrumbs.Count);
            Assert.AreEqual("This is product c", productModel.Description);
            Assert.AreEqual(3, productModel.ProductCategories.Count);
            Assert.AreEqual("ProdC", productModel.Sku);
            Assert.IsFalse(productModel.AllowAddToBasket);
            Assert.IsTrue(productModel.BestSeller);
            Assert.AreEqual("1 year guarantee", productModel.Features);
            Assert.AreEqual("Product C", productModel.Name);
            Assert.IsFalse(productModel.NewProduct);
            Assert.AreEqual(1, productModel.ProductGroupId);
            Assert.AreEqual("£1.99", productModel.RetailPrice);
            Assert.AreEqual("E7Voso411Vs", productModel.VideoLink);

            Assert.IsNotNull(productModel.AddToCart);
            Assert.AreEqual(0, productModel.AddToCart.Discount);
            Assert.AreEqual(3, productModel.AddToCart.Id);
            Assert.AreEqual(1, productModel.AddToCart.Quantity);
            Assert.AreEqual(1.99m, productModel.AddToCart.RetailPrice);
            Assert.AreEqual(5u, productModel.AddToCart.StockAvailability);

            Assert.AreEqual(1, productModel.Images.Length);
            Assert.AreEqual("myfile2", productModel.Images[0]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddToCart_Model_Null_ReturnsJsonResult_Success()
        {
            ProductController sut = CreateProductController();

            IActionResult response = sut.AddToCart(null);

            JsonResult result = response as JsonResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("application/json", result.ContentType);
            Assert.IsNotNull(result.Value);

            JsonResponseModel responseModel = result.Value as JsonResponseModel;
            Assert.AreEqual("Invalid Model", responseModel.ResponseData);
            Assert.IsFalse(responseModel.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddToCart_InvalidModel_NotInitialised_ReturnsJsonResult_Success()
        {
            ProductController sut = CreateProductController();

            IActionResult response = sut.AddToCart(new AddToCartModel());

            JsonResult result = response as JsonResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("application/json", result.ContentType);
            Assert.IsNotNull(result.Value);

            JsonResponseModel responseModel = result.Value as JsonResponseModel;
            Assert.AreEqual("Invalid Product", responseModel.ResponseData);
            Assert.IsFalse(responseModel.Success);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AddToCart_Model_Valid_RedirectsToProductPage_Success()
        {
            MockShoppingCartProvider mockShoppingCartProvider = new MockShoppingCartProvider();
            ProductController sut = CreateProductController(null, null, null, null, null, null, null, mockShoppingCartProvider);

            IActionResult response = sut.AddToCart(new AddToCartModel(8, 1.99m, 0, 5));

            RedirectToActionResult result = response as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Product", result.ActionName);
            Assert.IsFalse(result.Permanent);
            Assert.AreEqual(2, result.RouteValues.Keys.Count);
            Assert.IsTrue(result.RouteValues.Keys.Contains("id"));
            Assert.IsTrue(result.RouteValues.Keys.Contains("productName"));
            
            Assert.AreEqual(8, (int)result.RouteValues["id"]);
            Assert.AreEqual("Product-H", (string)result.RouteValues["productName"]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_InvalidSearchId_Null_ReturnsValidPartialViewAndModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();

                IActionResult response = sut.Search(null);

                PartialViewResult result = response as PartialViewResult;

                Assert.IsNotNull(result);
                Assert.AreEqual("/Views/Product/_ProductSearch.cshtml", result.ViewName);

                ProductSearchViewModel viewModel = result.Model as ProductSearchViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsFalse(viewModel.ContainsVideo);
                Assert.AreEqual(7, viewModel.Prices.Count);
                Assert.AreEqual(3, viewModel.ProductGroups.Count);
                Assert.IsFalse(String.IsNullOrEmpty(viewModel.SearchName));
                Assert.IsNotNull(viewModel.SearchName);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_InvalidSearchId_NotFound_ReturnsValidPartialViewAndModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();

                IActionResult response = sut.Search("P123456");

                PartialViewResult result = response as PartialViewResult;

                Assert.IsNotNull(result);
                Assert.AreEqual("/Views/Product/_ProductSearch.cshtml", result.ViewName);

                ProductSearchViewModel viewModel = result.Model as ProductSearchViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsFalse(viewModel.ContainsVideo);
                Assert.AreEqual(7, viewModel.Prices.Count);
                Assert.AreEqual(3, viewModel.ProductGroups.Count);
                Assert.IsFalse(String.IsNullOrEmpty(viewModel.SearchName));
                Assert.IsNotNull(viewModel.SearchName);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_InvalidSearchId_EmptyString_ReturnsValidPartialViewAndModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();

                IActionResult response = sut.Search("");

                PartialViewResult result = response as PartialViewResult;

                Assert.IsNotNull(result);
                Assert.AreEqual("/Views/Product/_ProductSearch.cshtml", result.ViewName);

                ProductSearchViewModel viewModel = result.Model as ProductSearchViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsFalse(viewModel.ContainsVideo);
                Assert.AreEqual(7, viewModel.Prices.Count);
                Assert.AreEqual(3, viewModel.ProductGroups.Count);
                Assert.IsFalse(String.IsNullOrEmpty(viewModel.SearchName));
                Assert.IsNotNull(viewModel.SearchName);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SearchOptions_InvalidSearchId_Null_ReturnsValidPartialViewAndModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();

                IActionResult response = sut.SearchOptions(null);

                PartialViewResult result = response as PartialViewResult;

                Assert.IsNotNull(result);
                Assert.AreEqual("/Views/Product/_ProductSearchOption.cshtml", result.ViewName);

                ProductSearchViewModel viewModel = result.Model as ProductSearchViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsFalse(viewModel.ContainsVideo);
                Assert.AreEqual(7, viewModel.Prices.Count);
                Assert.AreEqual(3, viewModel.ProductGroups.Count);
                Assert.IsFalse(String.IsNullOrEmpty(viewModel.SearchName));
                Assert.IsNotNull(viewModel.SearchName);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SearchOptions_InvalidSearchId_NotFound_ReturnsValidPartialViewAndModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();

                IActionResult response = sut.SearchOptions("P123456");

                PartialViewResult result = response as PartialViewResult;

                Assert.IsNotNull(result);
                Assert.AreEqual("/Views/Product/_ProductSearchOption.cshtml", result.ViewName);

                ProductSearchViewModel viewModel = result.Model as ProductSearchViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsFalse(viewModel.ContainsVideo);
                Assert.AreEqual(7, viewModel.Prices.Count);
                Assert.AreEqual(3, viewModel.ProductGroups.Count);
                Assert.IsFalse(String.IsNullOrEmpty(viewModel.SearchName));
                Assert.IsNotNull(viewModel.SearchName);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SearchOptions_InvalidSearchId_EmptyString_ReturnsValidPartialViewAndModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();

                IActionResult response = sut.SearchOptions("");

                PartialViewResult result = response as PartialViewResult;

                Assert.IsNotNull(result);
                Assert.AreEqual("/Views/Product/_ProductSearchOption.cshtml", result.ViewName);

                ProductSearchViewModel viewModel = result.Model as ProductSearchViewModel;
                Assert.IsNotNull(viewModel);
                Assert.IsFalse(viewModel.ContainsVideo);
                Assert.AreEqual(7, viewModel.Prices.Count);
                Assert.AreEqual(3, viewModel.ProductGroups.Count);
                Assert.IsFalse(String.IsNullOrEmpty(viewModel.SearchName));
                Assert.IsNotNull(viewModel.SearchName);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_InvalidModel_Null_RedirectsToAdvanceSearch_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();

                IActionResult response = sut.AdvancedSearch(null);

                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.AreEqual("/Search/Advanced/Products/", result.Url);
                Assert.IsFalse(result.Permanent);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_ValidModel_NullSearchText_RedirectsToSearchResult_WithValidModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();
                ProductSearchViewModel model = new ProductSearchViewModel();
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);
                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_ValidModel_WithProductGroups_RedirectsToSearchResult_WithValidModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.ProductGroups.Add(new CheckedViewItemModel("Main Products", true));
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);
                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_ValidModel_WithVideo_RedirectsToSearchResult_WithValidModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.ProductGroups.Add(new CheckedViewItemModel("Main Products", true));
                model.ContainsVideo = true;
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);
                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_ValidModel_WithPriceGroups_RedirectsToSearchResult_WithValidModel_Success()
        {
            ThreadManager.Initialise();
            try
            {
                ProductController sut = CreateProductController();
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.ProductGroups.Add(new CheckedViewItemModel("Main Products", true));
                model.ContainsVideo = true;
                model.Prices.Add(new CheckedViewItemModel("Under £5.00 (7)", true));
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);
                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AdvancedSearch_InvalidSettings_PriceGroups_ContainsValueLessThanPrevious_Throws_InvalidOperationException()
        {
            ThreadManager.Initialise();
            try
            {
                MockSettingsProvider mockSettingsProvider = new MockSettingsProvider("{\"Product\":{\"PriceGroups\":\"0;10.00;5.00;20.00;35.00;50.00\"}}");
                ProductController sut = CreateProductController(null, null, mockSettingsProvider);
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.ProductGroups.Add(new CheckedViewItemModel("Main Products", true));
                model.ContainsVideo = true;
                model.Prices.Add(new CheckedViewItemModel("Under £5.00 (7)", true));
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);
                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AdvancedSearch_InvalidSettings_PriceGroups_ContainsValueLessThanZero_Throws_InvalidOperationException()
        {
            ThreadManager.Initialise();
            try
            {
                MockSettingsProvider mockSettingsProvider = new MockSettingsProvider("{\"Product\":{\"PriceGroups\":\"0;-10.00;5.00;20.00;35.00;50.00\"}}");
                ProductController sut = CreateProductController(null, null, mockSettingsProvider);
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.ProductGroups.Add(new CheckedViewItemModel("Main Products", true));
                model.ContainsVideo = true;
                model.Prices.Add(new CheckedViewItemModel("Under £5.00 (7)", true));
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);
                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_ValidatePriceGroups_Success()
        {
            ThreadManager.Initialise();
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
                MockSettingsProvider mockSettingsProvider = new MockSettingsProvider("{\"Product\":{\"PriceGroups\":\"5.00;10.00;20.00;35.00;50.00\"}}");
                MockMemoryCache mockMemoryCache = new MockMemoryCache();
                ProductController sut = CreateProductController(null, null, mockSettingsProvider, null, null, mockMemoryCache);
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.ProductGroups.Add(new CheckedViewItemModel("Main Products", true));
                model.ContainsVideo = true;
                model.Prices.Add(new CheckedViewItemModel("Under £5.00 (7)", true));
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);

                WaitForThreadToFinish("Update price group product counts en-GB");

                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);

                CacheItem cacheItem = mockMemoryCache.GetCache().Get("Product Search Product Price Groups en-GB");

                Assert.IsNotNull(cacheItem);
                List<ProductPriceInfo> productPrices = (List<ProductPriceInfo>)cacheItem.Value;
                Assert.AreEqual(6, productPrices.Count);

                Assert.AreEqual(5.00m, productPrices[0].MaxValue);
                Assert.AreEqual(0m, productPrices[0].MinValue);
                Assert.AreEqual("£0.00 - £5.00 (7)", productPrices[0].Text);

                Assert.AreEqual(10.00m, productPrices[1].MaxValue);
                Assert.AreEqual(5.00m, productPrices[1].MinValue);
                Assert.AreEqual("£5.00 - £10.00 (0)", productPrices[1].Text);

                Assert.AreEqual(20.00m, productPrices[2].MaxValue);
                Assert.AreEqual(10.00m, productPrices[2].MinValue);
                Assert.AreEqual("£10.00 - £20.00 (1)", productPrices[2].Text);

                Assert.AreEqual(35.00m, productPrices[3].MaxValue);
                Assert.AreEqual(20.00m, productPrices[3].MinValue);
                Assert.AreEqual("£20.00 - £35.00 (1)", productPrices[3].Text);

                Assert.AreEqual(50.00m, productPrices[4].MaxValue);
                Assert.AreEqual(35.00m, productPrices[4].MinValue);
                Assert.AreEqual("£35.00 - £50.00 (0)", productPrices[4].Text);

                Assert.AreEqual(Decimal.MaxValue, productPrices[5].MaxValue);
                Assert.AreEqual(50.00m, productPrices[5].MinValue);
                Assert.AreEqual("Over £50.00 (0)", productPrices[5].Text);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_ValidModel_ValidatesCachedSearch_Success()
        {
            ThreadManager.Initialise();
            try
            {
                MockMemoryCache mockMemoryCache = new MockMemoryCache();
                ProductController sut = CreateProductController(null, null, null, null, null, mockMemoryCache);
                ProductSearchViewModel model = new ProductSearchViewModel();
                model.ProductGroups.Add(new CheckedViewItemModel("Main Products", true));
                model.ContainsVideo = true;
                model.Prices.Add(new CheckedViewItemModel("Under £5.00 (7)", true));
                string searchId = GetSearchId(model);

                IActionResult response = sut.AdvancedSearch(model);
                RedirectResult result = response as RedirectResult;

                Assert.IsNotNull(result);
                Assert.IsTrue(result.Url.StartsWith("/Search/Advanced/P"));
                Assert.IsFalse(result.Permanent);

                CacheItem cacheItem = mockMemoryCache.GetExtendingCache().Get($"Product Search View Model {model.SearchName}");
                Assert.IsNotNull(cacheItem);

                sut.Search(model.SearchName);
            }
            finally
            {
                ThreadManager.Finalise();
            }
        }

        private string GetSearchId(in ProductSearchViewModel model)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model));
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }

                return $"P{sb.ToString()}";
            }
        }

        private ProductController CreateProductController(
            IProductProvider productProvider = null,
            List<BreadcrumbItem> breadcrumbs = null,
            ISettingsProvider settingsProvider = null,
            IPluginHelperService pluginHelper = null, 
            IStockProvider stockProvider = null, 
            IMemoryCache memoryCache = null,
            IImageProvider imageProvider = null,
            IShoppingCartProvider shoppingCartProvider = null)
        {
            ProductController Result = new ProductController(
                productProvider ?? new MockProductProvider(), 
                settingsProvider ?? new MockSettingsProvider(), 
                pluginHelper ?? new MockPluginHelperService(_loadedPlugins),
                stockProvider ?? new MockStockProvider(), 
                memoryCache ?? new MockMemoryCache(), 
                imageProvider ?? new MockImageProvider(),
                shoppingCartProvider ?? new MockShoppingCartProvider());

            Result.ControllerContext = CreateTestControllerContext(breadcrumbs ?? GetDynamicBreadcrumbs());

            return Result;
        }

        private List<BreadcrumbItem> GetDynamicBreadcrumbs()
        {
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>();
            breadcrumbs.Add(new BreadcrumbItem("Home", "/", false));
            breadcrumbs.Add(new BreadcrumbItem("Custom Pages", "/DynamicContent/GetCustomPages", false));
            return breadcrumbs;
        }
    }
}
