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
 *  File: AddToCartModelTests.cs
 *
 *  Purpose:  Tests for add to cart model
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
    public class AddToCartModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            AddToCartModel sut = new AddToCartModel();
            Assert.AreEqual(1, sut.Quantity);
            Assert.AreEqual(0, sut.Id);
            Assert.AreEqual(0m, sut.RetailPrice);
            Assert.AreEqual(0m, sut.Discount);
            Assert.AreEqual((uint)0, sut.StockAvailability);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_RetailPriceBelowZero_Throws_ArgumentOutOfRangeException()
        {
            new AddToCartModel(1, 0, 0, 0);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_DiscountPercentBelowZero_Throws_ArgumentOutOfRangeException()
        {
            AddToCartModel sut = new AddToCartModel(1, 1.99m, -1, 0);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_DiscountPercentAboveZero_Throws_ArgumentOutOfRangeException()
        {
            AddToCartModel sut = new AddToCartModel(1, 1.99m, 101, 0);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstanceWithDiscount_Success()
        {
            AddToCartModel sut = new AddToCartModel(2, 4.99m, 5.5m, 23);
            Assert.AreEqual(1, sut.Quantity);
            Assert.AreEqual(2, sut.Id);
            Assert.AreEqual(4.99m, sut.RetailPrice);
            Assert.AreEqual(5.5m, sut.Discount);
            Assert.AreEqual((uint)23, sut.StockAvailability);
        }
    }
}
