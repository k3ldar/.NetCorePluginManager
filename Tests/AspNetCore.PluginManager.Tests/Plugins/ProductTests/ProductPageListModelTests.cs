/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ProductPageListModelTests.cs
 *
 *  Purpose:  Tests for Product page list model
 *
 *  Date        Name                Reason
 *  09/12/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Products;

using PluginManager.Abstractions;

using ProductPlugin;
using ProductPlugin.Models;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductPageListModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Products_Null_Throws_ArgumentNullException()
        {
            new ProductPageListModel(GenerateTestBaseModelData(), null, "pag", 1);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Pagination_Null_Throws_ArgumentNullException()
        {
            new ProductPageListModel(GenerateTestBaseModelData(), new List<Product>(), null, 1);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Pagination_EmptyString_Throws_ArgumentNullException()
        {
            new ProductPageListModel(GenerateTestBaseModelData(), new List<Product>(), "", 1);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_PageNumber_Zero_Throws_ArgumentOutOfRangeException()
        {
            new ProductPageListModel(GenerateTestBaseModelData(), new List<Product>(), "pag", 0);
        }


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductPageListModel sut = new ProductPageListModel(GenerateTestBaseModelData(), mockProductProvider.GetProducts(1, 3), "pag", 2);
            Assert.IsInstanceOfType(sut, typeof(BaseModel));

            Assert.IsNotNull(sut.Items);
            Assert.AreEqual(2, sut.PageNumber);
            Assert.AreEqual(3, sut.Items.Count);
            Assert.AreEqual(1, sut.Items[0].Id);
            Assert.AreEqual(2, sut.Items[1].Id);
            Assert.AreEqual(3, sut.Items[2].Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_RetrieveSecondPageOfProducts_Success()
        {
            MockProductProvider mockProductProvider = new MockProductProvider();
            ProductPageListModel sut = new ProductPageListModel(GenerateTestBaseModelData(), mockProductProvider.GetProducts(2, 3), "pag", 3);
            Assert.IsInstanceOfType(sut, typeof(BaseModel));

            Assert.IsNotNull(sut.Items);
            Assert.AreEqual(3, sut.PageNumber);
            Assert.AreEqual(3, sut.Items.Count);
            Assert.AreEqual(4, sut.Items[0].Id);
            Assert.AreEqual(5, sut.Items[1].Id);
            Assert.AreEqual(6, sut.Items[2].Id);
            Assert.AreEqual("pag", sut.Pagination);
        }
    }
}
