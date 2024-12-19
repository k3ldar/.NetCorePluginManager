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
 *  File: EditProductModelTests.cs
 *
 *  Purpose:  Edit Product Model Tests
 *
 *  Date        Name                Reason
 *  18/12/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;

using ProductPlugin.Models;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class EditProductModelTests : BaseControllerTests
    {
        private const string TestCategoryName = "Product Admin";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductGroups_Null_Throws_ArgumentNullException()
        {
            EditProductModel sut = new EditProductModel(GenerateTestBaseModelData(), null, -1, -1, "test", "desc", "features", "", false, true, 1.99m, "123", false, false, true, 1);
            Assert.IsInstanceOfType(sut, typeof(BaseModel));
            Assert.AreEqual(0, sut.Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultInstance_Success()
        {
            EditProductModel sut = new EditProductModel();
            Assert.IsInstanceOfType(sut, typeof(BaseModel));
            Assert.AreEqual(0, sut.Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_CreateProductInstance_Success()
        {
            EditProductModel sut = new EditProductModel(GenerateTestBaseModelData());
            Assert.AreEqual(-1, sut.Id);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_CreateProductEditInstance_Success()
        {
            List<LookupListItem> productGroups = new List<LookupListItem>()
            {
                new LookupListItem(1, "test"),
                new LookupListItem(5, "test 5")
            };

            EditProductModel sut = new EditProductModel(GenerateTestBaseModelData(), productGroups, 10, 5, "test prod", "my description", "features", "XYZ123", true, true, 1.99m, "TST01", true, true, true, 5);
            Assert.AreEqual(10, sut.Id);
            Assert.AreEqual(5, sut.ProductGroupId);
            Assert.AreEqual("test prod", sut.Name);
            Assert.AreEqual("my description", sut.Description);
            Assert.AreEqual("features", sut.Features);
            Assert.AreEqual("XYZ123", sut.VideoLink);
            Assert.IsTrue(sut.NewProduct);
            Assert.IsTrue(sut.BestSeller);
            Assert.AreEqual(1.99m, sut.RetailPrice);
            Assert.AreEqual("TST01", sut.Sku);
            Assert.IsTrue(sut.IsDownload);
            Assert.IsTrue(sut.AllowBackorder);
			Assert.IsTrue(sut.IsVisible);
            Assert.IsNotNull(sut.ProductGroups);
            Assert.AreEqual(2, sut.ProductGroups.Count);
            Assert.AreEqual(5, sut.PageNumber);
        }
    }
}
