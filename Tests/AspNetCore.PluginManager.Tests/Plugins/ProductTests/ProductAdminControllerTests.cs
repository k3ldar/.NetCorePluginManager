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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using PluginManager.Abstractions;

using ProductPlugin.Controllers;
using ProductPlugin.Models;

using Shared.Classes;

using SharedPluginFeatures;

using static System.Net.Mime.MediaTypeNames;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductAdminControllerTests : BaseControllerTests
    {
        private const string TestCategoryName = "Product Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Controller_ValidateAttributes_Success()
        {
            Assert.IsTrue(ClassHasAttribute<DenySpiderAttribute>(typeof(ProductAdminController)));
            Assert.IsTrue(ClassHasAttribute<LoggedInAttribute>(typeof(ProductAdminController)));
            Assert.IsTrue(ClassHasAttribute<AuthorizeAttribute>(typeof(ProductAdminController)));

            Assert.IsTrue(ClassAuthorizeAttributeHasCorrectPolicy(typeof(ProductAdminController), "ManageProducts"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductProviderNull_Throws_ArgumentNullException()
        {
            new ProductAdminController(null, new MockSettingsProvider(), new MockStockProvider(), new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProviderNull_Throws_ArgumentNullException()
        {
            new ProductAdminController(new MockProductProvider(), null, new MockStockProvider(), new MockMemoryCache());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_MemoryCacheNull_Throws_ArgumentNullException()
        {
            new ProductAdminController(new MockProductProvider(), new MockSettingsProvider(), new MockStockProvider(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ProductAdminController sut = new ProductAdminController(new MockProductProvider(), new MockSettingsProvider(), new MockStockProvider(), new MockMemoryCache());
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

            IActionResult result = sut.EditProduct(10000, 20);

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

            IActionResult result = sut.EditProduct(1, 1);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreNotSame(editProductModel, viewResult.Model);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

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
            Assert.AreEqual("/Views/ProductAdmin/EditProduct.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

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

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewDeleteProduct_Validate_Attributes()
        {
            string MethodName = "ViewDeleteProduct";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsTrue(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewDeleteProduct_ProductNotFound_RedirectsToIndex()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.ViewDeleteProduct(-1);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.AreEqual(400, jsonResult.StatusCode);
            Assert.IsNull(jsonResult.SerializerSettings);

            JsonResponseModel value = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(value);
            Assert.IsFalse(value.Success);
            Assert.AreEqual("Invalid product", value.ResponseData);

            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewDeleteProduct_ProductFound_ReturnsPartialView()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.ViewDeleteProduct(1);

            PartialViewResult partialViewResult = result as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.IsNull(partialViewResult.ContentType);
            Assert.IsInstanceOfType(partialViewResult.Model, typeof(ProductDeleteModel));
            Assert.IsNull(partialViewResult.StatusCode);
            Assert.IsNull(partialViewResult.TempData);
            Assert.AreEqual("_ShowDeleteProduct", partialViewResult.ViewName);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProduct_Validate_Attributes()
        {
            string MethodName = "DeleteProduct";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProduct_ModelIsNull_RedirectsToIndex()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.DeleteProduct(null);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Invalid model", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProduct_ProductNotFound_ReturnsBadResponse()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.DeleteProduct(new ProductDeleteModel(0));

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Invalid product", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProduct_DeletionNotConfirmed_ReturnsModelWithModelStateError()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);
            ProductDeleteModel model = new ProductDeleteModel(1);

            IActionResult result = sut.DeleteProduct(model);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Please write CONFIRM in the text box above and click delete", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProduct_ProductProviderWillNotAllowDeletion_AddsModelError()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            mockProductProvider.ProductDeleteError = "Failed to Delete";
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);
            ProductDeleteModel model = new ProductDeleteModel(1);
            model.Confirmation = "CONFirm";

            IActionResult result = sut.DeleteProduct(model);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Failed to Delete", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProduct_ProductDeleted_ReturnsResponse200()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);
            ProductDeleteModel model = new ProductDeleteModel(1);
            model.Confirmation = "CONFirm";

            IActionResult result = sut.DeleteProduct(model);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("", jsonResponseModel.ResponseData);
            Assert.IsTrue(jsonResponseModel.Success);
            Assert.AreEqual(0, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GroupIndex_Validate_Attributes()
        {
            string MethodName = "GroupIndex";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GroupIndex_ReturnsCorrectModelAndView_Success()
        {
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider);

            IActionResult result = sut.GroupIndex();

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);

            Assert.IsNull(viewResult.ViewName);

            ProductGroupListModel model = viewResult.Model as ProductGroupListModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Groups.Count);
            Assert.AreEqual(3, model.Breadcrumbs.Count);
            Assert.AreEqual("/SystemAdmin/Index", model.Breadcrumbs[1].Route);
            Assert.AreEqual("System Admin", model.Breadcrumbs[1].Name);
            Assert.AreEqual("/ProductAdmin/GroupIndex", model.Breadcrumbs[2].Route);
            Assert.AreEqual("Product Groups", model.Breadcrumbs[2].Name);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void ViewDeleteProductGroup_ProductGroupNotFound_Returns400()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.ViewDeleteProduct(1);

            PartialViewResult partialViewResult = result as PartialViewResult;
            Assert.IsNotNull(partialViewResult);
            Assert.IsNull(partialViewResult.ContentType);
            Assert.IsInstanceOfType(partialViewResult.Model, typeof(ProductDeleteModel));
            Assert.IsNull(partialViewResult.StatusCode);
            Assert.IsNull(partialViewResult.TempData);
            Assert.AreEqual("_ShowDeleteProduct", partialViewResult.ViewName);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProductGroup_Validate_Attributes()
        {
            string MethodName = "DeleteProductGroup";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsTrue(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProductGroup_InvalidModel_Returns400()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.DeleteProductGroup(null);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Invalid model", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProductGroup_ProductGroupNotFound_ReturnsBadRequest()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.DeleteProductGroup(new ProductGroupDeleteModel(3000));

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Invalid product group", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProductGroup_DeletionNotConfirmed_ReturnsModelWithModelStateError()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);
            ProductGroupDeleteModel model = new ProductGroupDeleteModel(1);

            IActionResult result = sut.DeleteProductGroup(model);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Please write CONFIRM in the text box above and click delete", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProductGroup_ProductProviderWillNotAllowDeletion_AddsModelError()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            mockProductProvider.ProductDeleteError = "Failed to Delete";
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);
            ProductGroupDeleteModel model = new ProductGroupDeleteModel(1);
            model.Confirmation = "CONFirm";

            IActionResult result = sut.DeleteProductGroup(model);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(400, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("Failed to Delete", jsonResponseModel.ResponseData);
            Assert.IsFalse(jsonResponseModel.Success);
            Assert.AreEqual(1, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void DeleteProductGroup_ProductDeleted_ReturnsResponse200()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);
            ProductGroupDeleteModel model = new ProductGroupDeleteModel(1);
            model.Confirmation = "CONFirm";

            IActionResult result = sut.DeleteProductGroup(model);

            JsonResult jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);
            Assert.AreEqual("application/json", jsonResult.ContentType);
            Assert.IsNull(jsonResult.SerializerSettings);
            Assert.AreEqual(200, jsonResult.StatusCode);

            JsonResponseModel jsonResponseModel = jsonResult.Value as JsonResponseModel;
            Assert.IsNotNull(jsonResponseModel);
            Assert.AreEqual("", jsonResponseModel.ResponseData);
            Assert.IsTrue(jsonResponseModel.Success);
            Assert.AreEqual(0, mockMemoryCache.GetShortCache().Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditProductGroup_Validate_Attributes()
        {
            string MethodName = "EditProductGroup";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditProductGroup_InvalidProductGroupId_RedirectsToGroupIndex()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.EditProductGroup(999);

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("GroupIndex", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
            Assert.IsNull(redirectResult.ControllerName);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void EditProductGroup_ValidProductGroupId_ReturnsModelAndView()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductAdminController sut = CreateProductAdminController(mockProductProvider, null, mockMemoryCache);

            IActionResult result = sut.EditProductGroup(1);

            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNull(viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);

            EditProductGroupModel model = viewResult.Model as EditProductGroupModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(4, model.Breadcrumbs.Count);
            Assert.AreEqual(1, model.Id);
            Assert.AreEqual("Main Products", model.Description);
            Assert.AreEqual("Checkout our main products", model.TagLine);
            Assert.IsTrue(model.ShowOnWebsite);
            Assert.AreEqual(1, model.SortOrder);
            Assert.AreEqual("", model.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProductGroup_Validate_Attributes()
        {
            string MethodName = "SaveProductGroup";
            Assert.IsTrue(MethodHasAttribute<HttpPostAttribute>(typeof(ProductAdminController), MethodName));

            Assert.IsFalse(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<AjaxOnlyAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<LoggedInAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpDeleteAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<HttpPutAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsFalse(MethodHasAttribute<RouteAttribute>(typeof(ProductAdminController), MethodName));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProductGroup_InvalidModel_Null_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();

            IActionResult result = sut.SaveProductGroup(null);

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.IsNull(redirectResult.ControllerName);
            Assert.AreEqual("GroupIndex", redirectResult.ActionName);
            Assert.IsFalse(redirectResult.Permanent);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProductGroup_InvalidProductDescription_EmptyString_ReturnsViewWithModelStateError()
        {
            ProductAdminController sut = CreateProductAdminController();
            EditProductGroupModel editProductGroupModel = new EditProductGroupModel()
            {
                Description = ""
            };

            IActionResult result = sut.SaveProductGroup(editProductGroupModel);


            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProductGroup.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreNotSame(editProductGroupModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "Description", "Please provide a valid product group description."));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProductGroup_ProductProviderFailsToSave_ReturnsViewWithModelStateError()
        {
            MockProductProvider productProvider = new MockProductProvider();
            productProvider.ProductGroupSaveError = "Can not save";
            ProductAdminController sut = CreateProductAdminController(productProvider);

            EditProductGroupModel editProductGroupModel = new EditProductGroupModel()
            {
                Description = "asdfasdf"
            };

            IActionResult result = sut.SaveProductGroup(editProductGroupModel);


            ViewResult viewResult = result as ViewResult;
            Assert.IsNotNull(viewResult);
            Assert.IsNotNull(viewResult.ViewName);
            Assert.AreEqual("/Views/ProductAdmin/EditProductGroup.cshtml", viewResult.ViewName);
            Assert.IsNotNull(viewResult.Model);
            Assert.AreNotSame(editProductGroupModel, viewResult.Model);

            Assert.IsTrue(ViewResultContainsModelStateError(viewResult, "", "Can not save"));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SaveProductGroup_ProductGroupSaved_RedirectsToIndex()
        {
            MockMemoryCache mockMemoryCache = new MockMemoryCache();
            mockMemoryCache.GetShortCache().Add("test", new CacheItem("test", true));
            ProductAdminController sut = CreateProductAdminController();

            EditProductGroupModel editProductGroupModel = new EditProductGroupModel()
            {
                Description = "asdfasdf"
            };

            IActionResult result = sut.SaveProductGroup(editProductGroupModel);

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("GroupIndex", redirectResult.ActionName);
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
				new MockStockProvider(),
				memoryCache ?? new MockMemoryCache());

            Result.ControllerContext = CreateTestControllerContext(breadcrumbs ?? CreateBreadcrumbs());

            return Result;
        }

        private List<BreadcrumbItem> CreateBreadcrumbs()
        {
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>();
            breadcrumbs.Add(new BreadcrumbItem("Home", "/", false));
            return breadcrumbs;
        }
    }
}
