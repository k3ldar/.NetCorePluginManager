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
 *  File: ProductPriceInfoTestsTests.cs
 *
 *  Purpose:  Tests for ProductPriceInfoTests class
 *
 *  Date        Name                Reason
 *  30/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductPlugin.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductPriceInfoTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Text_Null_Throws_ArgumentNullException()
        {
            new ProductPriceInfo(null, 23, 34);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Text_EmptyString_Throws_ArgumentNullException()
        {
            new ProductPriceInfo(String.Empty, 23, 34);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_MinValue_LessThanZero_Throws_ArgumentOutOfRangeException()
        {
            new ProductPriceInfo("price info", -1, 34);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Construct_InvalidParam_MinValue_GreaterThanMaxValue_Throws_ArgumentOutOfRangeException()
        {
            new ProductPriceInfo("price info", 50, 34);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PriceMatch_Price_LessThanMinValue_Returns_False()
        {
            ProductPriceInfo sut = new ProductPriceInfo("price info", 20, 30);
            Assert.AreEqual("price info", sut.Text);
            Assert.AreEqual(20m, sut.MinValue);
            Assert.AreEqual(30m, sut.MaxValue);

            bool result = sut.PriceMatch(15);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PriceMatch_Price_MoreThanMaxValue_Returns_False()
        {
            ProductPriceInfo sut = new ProductPriceInfo("price info", 20, 30);
            Assert.AreEqual("price info", sut.Text);
            Assert.AreEqual(20m, sut.MinValue);
            Assert.AreEqual(30m, sut.MaxValue);

            bool result = sut.PriceMatch(31);
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PriceMatch_Price_EqualsMinValue_Returns_True()
        {
            ProductPriceInfo sut = new ProductPriceInfo("price info", 20, 30);
            Assert.AreEqual("price info", sut.Text);
            Assert.AreEqual(20m, sut.MinValue);
            Assert.AreEqual(30m, sut.MaxValue);

            bool result = sut.PriceMatch(20);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PriceMatch_Price_EqualsMaxValue_Returns_True()
        {
            ProductPriceInfo sut = new ProductPriceInfo("price info", 20, 30);
            Assert.AreEqual("price info", sut.Text);
            Assert.AreEqual(20m, sut.MinValue);
            Assert.AreEqual(30m, sut.MaxValue);

            bool result = sut.PriceMatch(30);
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void PriceMatch_Price_BetweenMinAndMaxValue_Returns_True()
        {
            ProductPriceInfo sut = new ProductPriceInfo("price info", 20, 30);
            Assert.AreEqual("price info", sut.Text);
            Assert.AreEqual(20m, sut.MinValue);
            Assert.AreEqual(30m, sut.MaxValue);

            bool result = sut.PriceMatch(25);
            Assert.IsTrue(result);
        }
    }
}
