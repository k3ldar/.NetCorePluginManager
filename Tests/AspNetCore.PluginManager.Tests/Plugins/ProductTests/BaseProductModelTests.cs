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
 *  File: BaseProductModelTests.cs
 *
 *  Purpose:  Tests for base product model
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
    public class BaseProductModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_ValidInstance_Success()
        {
            BaseProductModel sut = new BaseProductModel();
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(BaseModel));
            Assert.IsNull(sut.ProductCategories);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_ModelDataConstructor_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            new BaseProductModel(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ModelDataConstructor_ValidInstance_Success()
        {
            BaseProductModel sut = new BaseProductModel(GenerateTestBaseModelData());
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(BaseModel));
            Assert.IsNull(sut.ProductCategories);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_ProductCategoriesConstructor_InvalidParam_ModelData_Null_Throws_ArgumentNullException()
        {
            new BaseProductModel(null, new List<ProductCategoryModel>());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_ProductCategoriesConstructor_InvalidParam_ProductCategoryModels_Null_Throws_ArgumentNullException()
        {
            new BaseProductModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ProductCategoriesConstructor_ValidInstance_Success()
        {
            BaseProductModel sut = new BaseProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>());
            Assert.IsNotNull(sut);
            Assert.IsInstanceOfType(sut, typeof(BaseModel));
            Assert.IsNotNull(sut.ProductCategories);
        }
    }
}
