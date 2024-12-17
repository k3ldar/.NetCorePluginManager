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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ProductTests.cs
 *
 *  Purpose:  Tests for Products class
 *
 *  Date        Name                Reason
 *  11/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Middleware.Products;

namespace AspNetCore.PluginManager.Tests.Middleware
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductTests
    {
        private const string TestCategoryName = "Middleware";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Name_Null_Throws_ArgumentNullException()
        {
            new Product(-1, 0, null, "description", "features", "videolink", new string[] { }, 1, "sku", false, false, true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Name_EmptyString_Throws_ArgumentNullException()
        {
            new Product(-1, 0, "", "description", "features", "videolink", new string[] { }, 1, "sku", false, false, true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Description_Null_Throws_ArgumentNullException()
        {
            new Product(-1, 0, "name", null, "features", "videolink", new string[] { }, 1, "sku", false, false, true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Description_EmptyString_Throws_ArgumentNullException()
        {
            new Product(-1, 0, "name", "", "features", "videolink", new string[] { }, 1, "sku", false, false, true);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance()
        {
            Product sut = new Product(-1, 21, "name", "description", "features", "videolink", new string[] { "img" }, 1.11m, "sku", true, true, true, true, true);

            Assert.AreEqual(-1, sut.Id);
            Assert.AreEqual(21, sut.ProductGroupId);
            Assert.AreEqual("name", sut.Name); 
            Assert.AreEqual("description", sut.Description); 
            Assert.AreEqual("features", sut.Features); 
            Assert.AreEqual("videolink", sut.VideoLink); 
            Assert.AreEqual(0u, sut.StockAvailability); 
            Assert.AreEqual(1, sut.Images.Length);
            Assert.AreEqual("img", sut.Images[0]);
            Assert.AreEqual(1.11m, sut.RetailPrice);
            Assert.AreEqual("sku", sut.Sku);
            Assert.IsTrue(sut.IsDownload);
            Assert.IsTrue(sut.AllowBackorder);
            Assert.IsTrue(sut.NewProduct);
            Assert.IsTrue(sut.BestSeller);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SetCurrentStockLevel_Success()
        {
            Product sut = new Product(-1, 21, "name", "description", "features", "videolink", new string[] { "img" }, 1.11m, "sku", true, true, true);

            Assert.AreEqual(0u, sut.StockAvailability);

            sut.SetCurrentStockLevel(34);
            Assert.AreEqual(34u, sut.StockAvailability);
        }
    }
}
