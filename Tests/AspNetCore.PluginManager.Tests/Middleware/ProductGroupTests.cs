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
 *  Copyright (c) 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: ProductGroupTests.cs
 *
 *  Purpose:  Tests for ProductGroup class
 *
 *  Date        Name                Reason
 *  10/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Products;

namespace AspNetCore.PluginManager.Tests.Middleware
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductGroupTests
    {
        private const string TestCategoryName = "Middleware";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Description_Null_Throws_ArgumentNullException()
        {
            ProductGroup sut = new ProductGroup(-1, null, true, 1, null, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_InvalidParam_Tagline_Null_ConvertsToEmptyString()
        {
            ProductGroup sut = new ProductGroup(-1, "description", true, 1, null, null);
            Assert.AreEqual("", sut.TagLine);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_InvalidParam_Url_Null_ConvertsToEmptyString()
        {
            ProductGroup sut = new ProductGroup(-1, "description", true, 1, null, null);
            Assert.AreEqual("", sut.TagLine);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            ProductGroup sut = new ProductGroup(-1, "description", true, 1, "tagline", "/url");
            Assert.AreEqual(-1, sut.Id);
            Assert.AreEqual("description", sut.Description);
            Assert.IsTrue(sut.ShowOnWebsite);
            Assert.AreEqual(1, sut.SortOrder);
            Assert.AreEqual("tagline", sut.TagLine);
            Assert.AreEqual("/url", sut.Url);
        }
    }
}
