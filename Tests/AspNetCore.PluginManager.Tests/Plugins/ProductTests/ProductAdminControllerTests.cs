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
            new ProductAdminController(null, new MockSettingsProvider());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_SettingsProviderNull_Throws_ArgumentNullException()
        {
            new ProductAdminController(new MockProductProvider(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ProductAdminController sut = new ProductAdminController(new MockProductProvider(), new MockSettingsProvider());
            Assert.IsNotNull(sut);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Index_Validate_Attributes()
        {
            string MethodName = "Index";
            Assert.IsTrue(MethodHasAttribute<HttpGetAttribute>(typeof(ProductAdminController), MethodName));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ProductAdminController), MethodName, "/ProductAdmin/Page/{page}/"));
            Assert.IsTrue(MethodHasRouteAttribute(typeof(ProductAdminController), MethodName, "/ProductAdmin/{page?}/"));

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
                "class=\"page-item active\"><a class=\"page-link\" href=\"/ProductAdmin/Index/Page/1/\">1</a></li><li class=\"page-item disabled\"><a class=\"page-link\" href=\"javascript: " + 
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
            const string ExpectedPagination = "<ul class=\"pagination\"><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Index/Page/2/\">&laquo; Previous" + 
                "</a></li><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Index/Page/1/\">1</a></li><li class=\"page-item\"><a class=\"page-link\" href" + 
                "=\"/ProductAdmin/Index/Page/2/\">2</a></li><li class=\"page-item active\"><a class=\"page-link\" href=\"/ProductAdmin/Index/Page/3/\">3</a></li><li " + 
                "class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Index/Page/4/\">4</a></li><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Index/" + 
                "Page/5/\">5</a></li><li class=\"page-item\"><a class=\"page-link\" href=\"/ProductAdmin/Index/Page/4/\">Next &raquo;</a></li></ul>";
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


        private ProductAdminController CreateProductAdminController(
            IProductProvider productProvider = null,
            ISettingsProvider settingsProvider = null,
            List<BreadcrumbItem> breadcrumbs = null)
        {
            ProductAdminController Result = new ProductAdminController(
                productProvider ?? new MockProductProvider(),
                settingsProvider ?? new MockSettingsProvider());

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
