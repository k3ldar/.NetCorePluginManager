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
 *  File: BaseModelDataTests.cs
 *
 *  Purpose:  Tests for BaseModelData
 *
 *  Date        Name                Reason
 *  01/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.SharedPluginFeatures
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BaseModelDataTests
    {
        private const string Category = "SharedPluginFeatures";

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_ValidInstance_NoBreadcrums_Success()
        {
            BaseModelData sut = new BaseModelData(null, null,
                null, null, null, null, false, false);

            Assert.IsNotNull(sut.Breadcrumbs);
            Assert.AreEqual(0, sut.Breadcrumbs.Count);
            Assert.IsNull(sut.CartSummary);
            Assert.AreEqual(String.Empty, sut.SeoAuthor);
            Assert.AreEqual(String.Empty, sut.SeoDescription);
            Assert.AreEqual(String.Empty, sut.SeoTitle);
            Assert.AreEqual(String.Empty, sut.SeoTags);
            Assert.IsFalse(sut.CanManageSeoData);
            Assert.IsFalse(sut.UserHasConsentCookie);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_ValidInstance_WithBreadcrums_Success()
        {
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>();
            breadcrumbs.Add(new BreadcrumbItem("test", "/", false));
            BaseModelData sut = new BaseModelData(breadcrumbs, null,
                null, null, null, null, false, false);

            Assert.IsNotNull(sut.Breadcrumbs);
            Assert.AreEqual(1, sut.Breadcrumbs.Count);
            Assert.AreEqual("test", sut.Breadcrumbs[0].Name);
            Assert.IsNull(sut.CartSummary);
            Assert.AreEqual(String.Empty, sut.SeoAuthor);
            Assert.AreEqual(String.Empty, sut.SeoDescription);
            Assert.AreEqual(String.Empty, sut.SeoTitle);
            Assert.AreEqual(String.Empty, sut.SeoTags);
            Assert.IsFalse(sut.CanManageSeoData);
            Assert.IsFalse(sut.UserHasConsentCookie);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void Construct_ValidInstance_WithValidSeoData_Success()
        {
            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>();
            breadcrumbs.Add(new BreadcrumbItem("test", "/", false));
            BaseModelData sut = new BaseModelData(breadcrumbs, null,
                "seo Title", "seo Author", "seo Description", "seo Tags", true, true);

            Assert.IsNotNull(sut.Breadcrumbs);
            Assert.AreEqual(1, sut.Breadcrumbs.Count);
            Assert.AreEqual("test", sut.Breadcrumbs[0].Name);
            Assert.IsNull(sut.CartSummary);
            Assert.AreEqual("seo Author", sut.SeoAuthor);
            Assert.AreEqual("seo Description", sut.SeoDescription);
            Assert.AreEqual("seo Title", sut.SeoTitle);
            Assert.AreEqual("seo Tags", sut.SeoTags);
            Assert.IsTrue(sut.CanManageSeoData);
            Assert.IsTrue(sut.UserHasConsentCookie);
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceBreadcrumbs_InvalidParamNull_Throws_ArgumentNullException()
        {
            BaseModelData sut = new BaseModelData(null, null,
                "seo Title", "seo Author", "seo Description", "seo Tags", true, true);

            Assert.IsNotNull(sut.Breadcrumbs);
            Assert.AreEqual(0, sut.Breadcrumbs.Count);

            sut.ReplaceBreadcrumbs(null);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void ReplaceBreadcrumbs_Success()
        {
            BaseModelData sut = new BaseModelData(null, null,
                "seo Title", "seo Author", "seo Description", "seo Tags", true, true);

            Assert.IsNotNull(sut.Breadcrumbs);
            Assert.AreEqual(0, sut.Breadcrumbs.Count);

            List<BreadcrumbItem> breadcrumbs = new List<BreadcrumbItem>();
            breadcrumbs.Add(new BreadcrumbItem("test", "/", false));
            sut.ReplaceBreadcrumbs(breadcrumbs);
            Assert.AreEqual(1, sut.Breadcrumbs.Count);
            Assert.AreEqual("test", sut.Breadcrumbs[0].Name);
        }

        [TestMethod]
        [TestCategory(Category)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceCartSummary_InvalidParamNull_Throws_ArgumentNullException()
        {
            BaseModelData sut = new BaseModelData(null, null,
                "seo Title", "seo Author", "seo Description", "seo Tags", true, true);

            Assert.IsNull(sut.CartSummary);

            sut.ReplaceCartSummary(null);
        }

        [TestMethod]
        [TestCategory(Category)]
        public void ReplaceCartSummary_Success()
        {
            BaseModelData sut = new BaseModelData(null, null,
                "seo Title", "seo Author", "seo Description", "seo Tags", true, true);

            Assert.IsNull(sut.CartSummary);

            sut.ReplaceCartSummary(new ShoppingCartSummary(-10, 2, 1.99m, 0, 1, .5m, Thread.CurrentThread.CurrentCulture, "USD"));

            Assert.AreEqual(-10, sut.CartSummary.Id);
            Assert.AreEqual(2, sut.CartSummary.TotalItems);
            Assert.AreEqual(1.99m, sut.CartSummary.SubTotal);
            Assert.AreEqual(0, sut.CartSummary.Discount);
            Assert.AreEqual(1, sut.CartSummary.Shipping);
            Assert.AreEqual(.5m, sut.CartSummary.TaxRate);
            Assert.AreEqual("USD", sut.CartSummary.CurrencyCode);
            Assert.AreEqual(Thread.CurrentThread.CurrentCulture.Name, sut.CartSummary.Culture.Name);

        }
    }
}
