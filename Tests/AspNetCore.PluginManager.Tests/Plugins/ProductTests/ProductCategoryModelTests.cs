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
 *  File: ProductCategoryModelTests.cs
 *
 *  Purpose:  Tests for product category model
 *
 *  Date        Name                Reason
 *  29/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductPlugin.Models;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductCategoryModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            ProductCategoryModel sut = new ProductCategoryModel();
            Assert.IsNull(sut.Description);
            Assert.AreEqual(0, sut.Id);
            Assert.IsNull(sut.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Description_Null_Throws_ArgumentNullException()
        {
            new ProductCategoryModel(21, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstanceWithDescription_Success()
        {
            ProductCategoryModel sut = new ProductCategoryModel(22, "desc");
            Assert.AreEqual("desc", sut.Description);
            Assert.AreEqual(22, sut.Id);
            Assert.IsNull(sut.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstanceWithUrlNull_Success()
        {
            ProductCategoryModel sut = new ProductCategoryModel(23, "desc", null);

            Assert.AreEqual("desc", sut.Description);
            Assert.AreEqual(23, sut.Id);
            Assert.AreEqual("/Products/desc/23/", sut.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstanceWithUrlEmptyString_Success()
        {
            ProductCategoryModel sut = new ProductCategoryModel(24, "desc", null);

            Assert.AreEqual("desc", sut.Description);
            Assert.AreEqual(24, sut.Id);
            Assert.AreEqual("/Products/desc/24/", sut.Url);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstanceWithValidUrl_Success()
        {
            ProductCategoryModel sut = new ProductCategoryModel(24, "desc", "/uri/something");

            Assert.AreEqual("desc", sut.Description);
            Assert.AreEqual(24, sut.Id);
            Assert.AreEqual("/uri/something", sut.Url);
        }
    }
}