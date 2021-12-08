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
 *  File: ProductCategoryProductModel.cs
 *
 *  Purpose:  Tests for product category product model
 *
 *  Date        Name                Reason
 *  30/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductPlugin.Models;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductCategoryProductModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            ProductCategoryProductModel sut = new ProductCategoryProductModel();

            Assert.IsNull(sut.GetRouteName());
            Assert.AreEqual(0, sut.Id);
            Assert.AreEqual(0, sut.ProductGroupId);
            Assert.IsNull(sut.Name);
            Assert.IsNull(sut.Images);
            Assert.IsFalse(sut.NewProduct);
            Assert.IsFalse(sut.BestSeller);
            Assert.IsNull(sut.Price);
            Assert.IsNull(sut.Url);
            Assert.IsNull(sut.Sku);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Name_Null_Throws_ArgumentNullException()
        {
            new ProductCategoryProductModel(123, null, "img", 5, true, true, 0, "sku");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Name_EmptyString_Throws_ArgumentNullException()
        {
            new ProductCategoryProductModel(123, "", "img", 5, true, true, 0, "sku");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Sku_Null_Throws_ArgumentNullException()
        {
            new ProductCategoryProductModel(123, "name", "img", 5, true, true, 0, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Sku_EmptyString_Throws_ArgumentNullException()
        {
            new ProductCategoryProductModel(123, "name", "img", 5, true, true, 0, "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_LowestPriceFree_Success()
        {
            ProductCategoryProductModel sut = new ProductCategoryProductModel(124, "name of prod", "img", 5, true, true, 0, "sku");

            Assert.AreEqual("name-of-prod", sut.GetRouteName());
            Assert.AreEqual(124, sut.Id);
            Assert.AreEqual(5, sut.ProductGroupId);
            Assert.AreEqual("name of prod", sut.Name);
            Assert.AreEqual("img", sut.Images);
            Assert.IsTrue(sut.NewProduct);
            Assert.IsTrue(sut.BestSeller);
            Assert.AreEqual("Free", sut.Price);
            Assert.AreEqual("/Product/124/name-of-prod/", sut.Url);
            Assert.AreEqual("sku", sut.Sku);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_LowestPriceNotFree_Success()
        {
            ProductCategoryProductModel sut = new ProductCategoryProductModel(123, "name of prod", "img", 5, true, true, 4.5m, "sku");

            Assert.AreEqual("name-of-prod", sut.GetRouteName());
            Assert.AreEqual(123, sut.Id);
            Assert.AreEqual(5, sut.ProductGroupId);
            Assert.AreEqual("name of prod", sut.Name);
            Assert.AreEqual("img", sut.Images);
            Assert.IsTrue(sut.NewProduct);
            Assert.IsTrue(sut.BestSeller);
            Assert.AreEqual("From £4.50", sut.Price);
            Assert.AreEqual("/Product/123/name-of-prod/", sut.Url);
            Assert.AreEqual("sku", sut.Sku);
        }
    }
}
