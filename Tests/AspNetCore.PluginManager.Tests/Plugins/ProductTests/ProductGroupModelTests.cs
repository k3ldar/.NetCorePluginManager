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
 *  File: ProductGroupModelTests.cs
 *
 *  Purpose:  Tests for ProductGroupModel
 *
 *  Date        Name                Reason
 *  15/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductPlugin.Models;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductGroupModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_SimpleConstructor_Success()
        {
            ProductGroupModel sut = new ProductGroupModel();

            Assert.IsNull(sut.Description);
            Assert.IsNull(sut.TagLine);
            Assert.IsNull(sut.Pagination);
            Assert.IsNull(sut.Products);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            ProductGroupModel sut = new ProductGroupModel(null, new List<ProductCategoryModel>(), "Group Description", "My Tagline");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductGroups_Null_Throws_ArgumentNullException()
        {
            new ProductGroupModel(GenerateTestBaseModelData(), null, "Group Description", "My Tagline");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Description_Null_Throws_ArgumentNullException()
        {
            new ProductGroupModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), null, "My Tagline");

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Description_EmptyString_Throws_ArgumentNullException()
        {
            new ProductGroupModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), "", "My Tagline");

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Tagline_Null_Throws_ArgumentNullException()
        {
            new ProductGroupModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), "Group Description", null);

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Tagline_EmptyString_Throws_ArgumentNullException()
        {
            new ProductGroupModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), "Group Description", "");

        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ProductGroupModel sut = new ProductGroupModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), "Group Description", "Tag Line");

            Assert.AreEqual("Group Description", sut.Description);
            Assert.AreEqual("Tag Line", sut.TagLine);
            Assert.IsNull(sut.Pagination);
            Assert.IsNotNull(sut.Products);
            Assert.AreEqual(0, sut.Products.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void GetRouteDescription_Success()
        {
            ProductGroupModel sut = new ProductGroupModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), "Group Description", "Tag Line");

            Assert.AreEqual("Group-Description", sut.GetRouteDescription());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Pagination_Set_Success()
        {
            ProductGroupModel sut = new ProductGroupModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), "Group Description", "Tag Line");
            sut.Pagination = "Pag";

            Assert.AreEqual("Pag", sut.Pagination);
        }
    }
}
