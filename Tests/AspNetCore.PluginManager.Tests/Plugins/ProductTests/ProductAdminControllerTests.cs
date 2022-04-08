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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ProductAdminControllerTests.cs
 *
 *  Purpose:  Tests for Product Admin Controller
 *
 *  Date        Name                Reason
 *  14/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Controllers;
using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.Abstractions;

using ProductPlugin.Controllers;
using ProductPlugin.Models;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductAdminControllerTests : BaseControllerTests
    {
        private const string TestCategoryName = "Product Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductProviderNull_Throws_ArgumentNullException()
        {
            new ProductAdminController(null, new MockSettingsProvider(), new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProviderNull_Throws_ArgumentNullException()
        {
            new ProductAdminController(new MockProductProvider(), null, new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_MemoryCacheNull_Throws_ArgumentNullException()
        {
            new ProductAdminController(new MockProductProvider(), new MockSettingsProvider(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ProductAdminController sut = new ProductAdminController(new MockProductProvider(), new MockSettingsProvider(), new MockMemoryCache());
            Assert.IsNotNull(sut);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_Validate_Attributes()
        {
            string MethodName = "Index";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ProductAdminController), MethodName, "/ProductAdmin/Page/{page}/"));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ProductAdminController), MethodName, "/ProductAdmin/Index/"));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_PageValue_Null_ReturnsPage1_Success()
        {
            const string ExpectedPagination = "<ul class=\"pagination\"><li class=\"page-item disabled\"><a class=\"page-link\" href=\"javascript: void(0)\">&laquo; Previous</a></li><li " +
                "class=\"page-item active\"><a class=\"page-link\" href=\"/ProductAdmin/Page/1/\">1</a></li><li class=\"page-item disabled\"><a class=\"page-link\" href=\"javascript: " +
                "void(0)\">Next &raquo;</a></li></ul>";
            ProductAdminController sut = CreateProductAdminController();

            IActionResult result = sut.Index(null);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            Assert.IsNull(viewResult.ViewName);

            ProductPageListModel model = viewResult.Model as ProductPageListModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(9, model.Items.Count);
            Assert.AreEqual(ExpectedPagination, model.Pagination);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_PageValue_3_TwoItemsPerPage_ReturnsPage3_Success()
        {
            const string ExpectedPagination = "<ul class=\"pagination\"><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Page/2/\">&laquo; Previous" +
                "</a></li><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Page/1/\">1</a></li><li class=\"page-item\"><a class=\"page-link\" href" +
                "=\"/ProductAdmin/Page/2/\">2</a></li><li class=\"page-item active\"><a class=\"page-link\" href=\"/ProductAdmin/Page/3/\">3</a></li><li " +
                "class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Page/4/\">4</a></li><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/" +
                "Page/5/\">5</a></li><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Page/4/\">Next &raquo;</a></li></ul>";
            MockSettingsProvider mockSettingsProvider = new MockSettingsProvider("{\"Product\":{\"ProductsPerPage\":2}}");
            ProductAdminController sut = CreateProductAdminController(null, mockSettingsProvider);

            IActionResult result = sut.Index(3);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            Assert.IsNull(viewResult.ViewName);

            ProductPageListModel model = viewResult.Model as ProductPageListModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Items.Count);
            Assert.AreEqual(ExpectedPagination, model.Pagination);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditProduct_Validate_Attributes()
        {
            string MethodName = "EditProduct";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditProduct_ProductNotFound_RedirectsToIndex()
        {
            MockSettingsProvider mockSettingsProvider = new MockSettingsProvider("{\"Product\":{\"ProductsPerPage\":2}}");
            ProductAdminController sut = CreateProductAdminController(null, mockSettingsProvider);

            IActionResult result = sut.EditProduct(10000);

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.IsNull(redirectResult.ControllerName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditProduct_ProductFound_ReturnsValidViewAndModel()
        {
            MockSettingsProvider mockSettingsProvider = new MockSettingsProvider("{\"Product\":{\"ProductsPerPage\":2}}");
            ProductAdminController sut = CreateProductAdminController(null, mockSettingsProvider);

            IActionResult result = sut.EditProduct(1);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            EditProductModel editProductModel = viewResult.Model as EditProductModel;
            Assert.IsNotNull(editProductModel);
            Assert.IsFalse(editProductModel.AllowBackorder);
            Assert.IsFalse(editProductModel.BestSeller);
            Assert.AreEqual("This is product a", editProductModel.Description);
            Assert.AreEqual(1, editProductModel.Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void NewProduct_Validate_Attributes()
        {
            string MethodName = "NewProduct";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void NewProduct_ReturnsValidViewAndModel()
        {
            MockSettingsProvider mockSettingsProvider = new MockSettingsProvider("{\"Product\":{\"ProductsPerPage\":2}}");
            ProductAdminController sut = CreateProductAdminController(null, mockSettingsProvider);

            IActionResult result = sut.NewProduct();

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            EditProductModel editProductModel = viewResult.Model as EditProductModel;
            Assert.IsNotNull(editProductModel);
            Assert.IsFalse(editProductModel.AllowBackorder);
            Assert.IsFalse(editProductModel.BestSeller);
            Assert.IsNull(editProductModel.Description);
            Assert.AreEqual(-1, editProductModel.Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_Validate_Attributes()
        {
            string MethodName = "SaveProduct";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_ProductModelInvalid_Null_RedirectsToIndex()
        {
            ProductAdminController sut = CreateProductAdminController();

            IActionResult result = sut.SaveProduct(null);

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.IsNull(redirectResult.ControllerName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidProductName_Null_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();

            IActionResult result = sut.SaveProduct(new EditProductModel());

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Name", "Please specify a valid product item name."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidProductName_EmptyString_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = ""
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Name", "Please specify a valid product item name."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidProductDescription_Null_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = null
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Description", "Please provide a valid product description."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidProductDescription_EmptyString_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = ""
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Description", "Please provide a valid product description."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidRetailPrice_BelowZero_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = "My Desc",
                RetailPrice = -0.01m
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "RetailPrice", "Product price must be at least zero."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidSku_Null_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = "My Desc",
                RetailPrice = 0.01m
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Sku", "Please provide a valid SKU for the product."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidSku_EmptyString_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = "My Desc",
                RetailPrice = 0.01m,
                Sku = ""
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Sku", "Please provide a valid SKU for the product."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_InvalidProductGroup_NotFound_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = "My Desc",
                RetailPrice = 0.01m,
                Sku = "TST01",
                ProductGroupId = -10
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "ProductGroupId", "The product must have a primary group!"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_ProductProviderFailedToSave_ReturnsViewWithModelStateError()
        {
            MockProductProvider mockProductProvider = new MockProductProvider();
            mockProductProvider.ProductSaveError = "Failed to save";
            ProductAdminController sut = CreateProductAdminController(mockProductProvider);
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = "My Desc",
                RetailPrice = 0.01m,
                Sku = "TST01",
                ProductGroupId = 1
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.aspx", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreSame(editProductModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Failed to save"));
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProduct_ProductSaved_RedirectsToIndex()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);
            EditProductModel editProductModel = new EditProductModel()
            {
                Name = "New Product",
                Description = "My Desc",
                RetailPrice = 0.01m,
                Sku = "TST01",
                ProductGroupId = 1
            };

            IActionResult result = sut.SaveProduct(editProductModel);

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual(0, mockMemoryCache.GetShortCache().Count);
        }

        private ProductAdminController CreateProductAdminController(
            IProductProvider productProvider = null,
            ISettingsProvider settingsProvider = null,
            IMemoryCache memoryCache = null,
            List<BreadcrumbItem> breadcrumbs = null)
        {
            ProductAdminController Result = new ProductAdminController(
                productProvider ?? new MockProductProvider(),
                settingsProvider ?? new MockSettingsProvider(),
                memoryCache ?? new MockMemoryCache());

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
