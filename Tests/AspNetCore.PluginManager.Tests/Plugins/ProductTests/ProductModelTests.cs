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
 *  File: ProductModelTests.cs
 *
 *  Purpose:  Tests for product model
 *
 *  Date        Name                Reason
 *  29/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductPlugin.Models;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            ProductModel sut = new ProductModel();

            Assert.IsInstanceOfType(sut, typeof(BaseProductModel));
            Assert.AreEqual(0, sut.Id);
            Assert.AreEqual(0, sut.ProductGroupId);
            Assert.IsNull(sut.Name);
            Assert.IsNull(sut.Description);
            Assert.IsNull(sut.Features);
            Assert.IsNull(sut.VideoLink);
            Assert.AreEqual(null, sut.Images);
            Assert.IsFalse(sut.NewProduct);
            Assert.IsFalse(sut.BestSeller);
            Assert.IsNull(sut.RetailPrice);
            Assert.IsFalse(sut.AllowAddToBasket);
            Assert.IsNull(sut.AddToCart);
            Assert.AreEqual((uint)0, sut.StockAvailability);
            Assert.IsNull(sut.Sku);
            Assert.IsNull(sut.GetRouteName());
            Assert.AreEqual("", sut.GetVideoLink());
            Assert.IsNotNull(sut.FeatureList());
            Assert.AreEqual(0, sut.FeatureList().Length);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductCategories_Null_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidProductCategories_Success()
        {
            ProductModel sut = new ProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>());

            Assert.IsInstanceOfType(sut, typeof(BaseProductModel));
            Assert.AreEqual(0, sut.Id);
            Assert.AreEqual(0, sut.ProductGroupId);
            Assert.IsNull(sut.Name);
            Assert.IsNull(sut.Description);
            Assert.IsNull(sut.Features);
            Assert.IsNull(sut.VideoLink);
            Assert.AreEqual(null, sut.Images);
            Assert.IsFalse(sut.NewProduct);
            Assert.IsFalse(sut.BestSeller);
            Assert.IsNull(sut.RetailPrice);
            Assert.IsFalse(sut.AllowAddToBasket);
            Assert.IsNull(sut.AddToCart);
            Assert.AreEqual((uint)0, sut.StockAvailability);
            Assert.IsNull(sut.Sku);
            Assert.IsNull(sut.GetRouteName());
            Assert.AreEqual("", sut.GetVideoLink());
            Assert.IsNotNull(sut.FeatureList());
            Assert.AreEqual(0, sut.FeatureList().Length);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Name_Null_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), 1, null, new string[] { }, 1, false, false, 0, true, "sku");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Name_EmptyString_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), 1, "", new string[] { }, 1, false, false, 0, true, "sku");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Sku_Null_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), 1, "prod name", new string[] { }, 1, false, false, 0, true, null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Sku_EmptyString_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), 1, "prod name", new string[] { }, 1, false, false, 0, true, "");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_FreeProduct_Success()
        {
            ProductModel sut = new ProductModel(GenerateTestBaseModelData(), 23, "name", new string[] { }, 2, true, true, 0, true, "sku");

            Assert.IsInstanceOfType(sut, typeof(BaseProductModel));
            Assert.AreEqual(23, sut.Id);
            Assert.AreEqual(2, sut.ProductGroupId);
            Assert.AreEqual("name", sut.Name);
            Assert.AreEqual("", sut.Description);
            Assert.AreEqual("", sut.Features);
            Assert.AreEqual("", sut.VideoLink);
            Assert.IsNotNull(sut.Images);
            Assert.AreEqual(0, sut.Images.Length);
            Assert.IsTrue(sut.NewProduct);
            Assert.IsTrue(sut.BestSeller);
            Assert.AreEqual("Free", sut.RetailPrice);
            Assert.IsTrue(sut.AllowAddToBasket);
            Assert.IsNull(sut.AddToCart);
            Assert.AreEqual((uint)0, sut.StockAvailability);
            Assert.AreEqual("sku", sut.Sku);
            Assert.AreEqual("name", sut.GetRouteName());
            Assert.AreEqual("", sut.GetVideoLink());
            Assert.IsNotNull(sut.FeatureList());
            Assert.AreEqual(0, sut.FeatureList().Length);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_WithPriceCorrectlyFormatted_Success()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            ProductModel sut = new ProductModel(GenerateTestBaseModelData(), 23, "name", new string[] { }, 2, true, true, 3.56m, true, "sku");

            Assert.IsInstanceOfType(sut, typeof(BaseProductModel));
            Assert.AreEqual(23, sut.Id);
            Assert.AreEqual(2, sut.ProductGroupId);
            Assert.AreEqual("name", sut.Name);
            Assert.AreEqual("", sut.Description);
            Assert.AreEqual("", sut.Features);
            Assert.AreEqual("", sut.VideoLink);
            Assert.IsNotNull(sut.Images);
            Assert.AreEqual(0, sut.Images.Length);
            Assert.IsTrue(sut.NewProduct);
            Assert.IsTrue(sut.BestSeller);
            Assert.AreEqual("£3.56", sut.RetailPrice);
            Assert.IsTrue(sut.AllowAddToBasket);
            Assert.IsNull(sut.AddToCart);
            Assert.AreEqual((uint)0, sut.StockAvailability);
            Assert.AreEqual("sku", sut.Sku);
            Assert.AreEqual("name", sut.GetRouteName());
            Assert.AreEqual("", sut.GetVideoLink());
            Assert.IsNotNull(sut.FeatureList());
            Assert.AreEqual(0, sut.FeatureList().Length);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_PrimaryConstructor_InvalidParam_Name_Null_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), 1, 1, null, "prod desc",
                "features", "videolink", new string[] { }, 1.23m, "sku", true, true, true, 8);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_PrimaryConstructor_InvalidParam_Name_EmptyString_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), 1, 1, "", "prod desc",
                "features", "videolink", new string[] { }, 1.23m, "sku", true, true, true, 8);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_PrimaryConstructor_InvalidParam_Sku_Null_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), 1, 1, "prod name", "prod desc",
                "features", "videolink", new string[] { }, 1.23m, null, true, true, true, 8);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_PrimaryConstructor_InvalidParam_Sku_EmptyString_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), 1, 1, "prod name", "prod desc",
                "features", "videolink", new string[] { }, 1.23m, "", true, true, true, 8);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_PrimaryConstructor_InvalidParam_Description_Null_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), 1, 1, "prod name", null,
                "features", "videolink", new string[] { }, 1.23m, "sku", true, true, true, 8);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_PrimaryConstructor_InvalidParam_Description_EmptyString_Throws_ArgumentNullException()
        {
            new ProductModel(GenerateTestBaseModelData(), new List<ProductCategoryModel>(), 1, 1, "prod name", "",
                "features", "videolink", new string[] { }, 1.23m, "sku", true, true, true, 8);
        }




















        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_PrimaryConstructor_ValidInstance_WithVideoLinkAndFeatures_Success()
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-GB");
            const string ExpectedVideoLink = "<object class=\"productVideo\" ><param name=\"allowfullscreen\" value=\"true\" /> <param name=\"allowscriptaccess\" value=\"always\"" + 
                " /> <param name=\"movie\" value=\"https://www.facebook.com/v/abc123\" /> <embed src=\"https://www.facebook.com/v/abc123\" type=\"application/x-shockwave-flash\" " + 
                "allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"640\" height=\"390\"></embed></object>";
            List<ProductCategoryModel> categories = new List<ProductCategoryModel>()
            {
                new ProductCategoryModel(5, "cat desc")
            };
            string features = "feature 1\rfeature 2\r\nfeature 3";
            string[] images = { "img1", "img2" };

            ProductModel sut = new ProductModel(GenerateTestBaseModelData(), categories, 543, 5, "prod name", "prod desc", features, "https://www.facebook.com/video.php?v=abc123", 
                images, 12.9m, "prod sku", true, true, true, 123);

            Assert.IsInstanceOfType(sut, typeof(BaseProductModel));
            Assert.AreEqual(543, sut.Id);
            Assert.AreEqual(5, sut.ProductGroupId);
            Assert.AreEqual("prod name", sut.Name);
            Assert.AreEqual("prod desc", sut.Description);
            Assert.AreEqual("feature 1\rfeature 2\r\nfeature 3", sut.Features);
            Assert.AreEqual("https://www.facebook.com/video.php?v=abc123", sut.VideoLink);
            Assert.IsNotNull(sut.Images);
            Assert.AreEqual(2, sut.Images.Length);
            Assert.AreEqual("img1", sut.Images[0]);
            Assert.AreEqual("img2", sut.Images[1]);
            Assert.IsTrue(sut.NewProduct);
            Assert.IsTrue(sut.BestSeller);
            Assert.AreEqual("£12.90", sut.RetailPrice);
            Assert.IsTrue(sut.AllowAddToBasket);
            Assert.IsNotNull(sut.AddToCart);
            Assert.AreEqual(0, sut.AddToCart.Discount);
            Assert.AreEqual(543, sut.AddToCart.Id);
            Assert.AreEqual(1, sut.AddToCart.Quantity);
            Assert.AreEqual(12.9m, sut.AddToCart.RetailPrice);
            Assert.AreEqual((uint)123, sut.AddToCart.StockAvailability);
            Assert.AreEqual((uint)123, sut.StockAvailability);
            Assert.AreEqual("prod sku", sut.Sku);
            Assert.AreEqual("prod-name", sut.GetRouteName());
            Assert.AreEqual(ExpectedVideoLink, sut.GetVideoLink());
            Assert.IsNotNull(sut.FeatureList());
            Assert.AreEqual(3, sut.FeatureList().Length);
            Assert.AreEqual("feature 1", sut.FeatureList()[0]);
            Assert.AreEqual("feature 2", sut.FeatureList()[1]);
            Assert.AreEqual("feature 3", sut.FeatureList()[2]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_PrimaryConstructor_ValidInstance_WithVideoLinkFacebookUnsecureUri_Success()
        {
            const string ExpectedVideoLink = "<object class=\"productVideo\" ><param name=\"allowfullscreen\" value=\"true\" /> <param name=\"allowscriptaccess\" value=\"always\"" +
                " /> <param name=\"movie\" value=\"http://www.facebook.com/v/abc123\" /> <embed src=\"http://www.facebook.com/v/abc123\" type=\"application/x-shockwave-flash\" " +
                "allowscriptaccess=\"always\" allowfullscreen=\"true\" width=\"640\" height=\"390\"></embed></object>";
            List<ProductCategoryModel> categories = new List<ProductCategoryModel>()
            {
                new ProductCategoryModel(5, "cat desc")
            };
            string features = "feature 1\rfeature 2\r\nfeature 3";
            string[] images = { "img1", "img2" };

            ProductModel sut = new ProductModel(GenerateTestBaseModelData(), categories, 543, 5, "prod name", "prod desc", features, "http://www.facebook.com/video.php?v=abc123",
                images, 12.9m, "prod sku", true, true, true, 123);

            Assert.AreEqual("http://www.facebook.com/video.php?v=abc123", sut.VideoLink);
            Assert.AreEqual(ExpectedVideoLink, sut.GetVideoLink());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_PrimaryConstructor_ValidInstance_WithGoogleVideoLink_Success()
        {
            const string ExpectedVideoLink = "<iframe class=\"productVideo\" src=\"https://www.youtube.com/embed/abc123\" frameborder=\"0\"></iframe>";
            List<ProductCategoryModel> categories = new List<ProductCategoryModel>()
            {
                new ProductCategoryModel(5, "cat desc")
            };
            string features = "feature 1\rfeature 2\r\nfeature 3";
            string[] images = { "img1", "img2" };

            ProductModel sut = new ProductModel(GenerateTestBaseModelData(), categories, 543, 5, "prod name", "prod desc", features, "abc123",
                images, 12.9m, "prod sku", true, true, true, 123);

            Assert.AreEqual("abc123", sut.VideoLink);
            Assert.AreEqual(ExpectedVideoLink, sut.GetVideoLink());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_PrimaryConstructor_FreeProduct_Success()
        {
            List<ProductCategoryModel> categories = new List<ProductCategoryModel>()
            {
                new ProductCategoryModel(5, "cat desc")
            };
            string features = "feature 1\rfeature 2\r\nfeature 3";
            string[] images = { "img1", "img2" };

            ProductModel sut = new ProductModel(GenerateTestBaseModelData(), categories, 543, 5, "prod name", "prod desc", features, "abc123",
                images, 0, "prod sku", true, true, true, 123);

            Assert.AreEqual("Free", sut.RetailPrice);
        }
    }
}
