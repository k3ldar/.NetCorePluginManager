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
 *  File: ProductSitemapProviderTests.cs
 *
 *  Purpose:  Tests for ProductSitemapProvider class
 *
 *  Date        Name                Reason
 *  30/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductPlugin.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductSitemapProviderTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_ProductProvider_Null_Throws_ArgumentNullException()
        {
            new ProductSitemapProvider(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Items_ProcessesAllItems_ReturnsValidSitemap()
        {
            ProductSitemapProvider sut = new ProductSitemapProvider(new MockProductProvider());

            List<SitemapItem> result = sut.Items();

            Assert.IsNotNull(result);

            Assert.AreEqual(SitemapChangeFrequency.Daily, result[0].ChangeFrequency);
            Assert.AreEqual("Products/Main-Products/1/", result[0].Location.ToString());
            Assert.IsTrue(result[0].Priority.HasValue);
            Assert.AreEqual(1.0m, result[0].Priority.Value);
            Assert.AreEqual(SitemapChangeFrequency.Daily, result[1].ChangeFrequency);
            Assert.AreEqual("Products/Other-Products/2/", result[1].Location.ToString());
            Assert.IsTrue(result[1].Priority.HasValue);
            Assert.AreEqual(1.0m, result[1].Priority.Value);

            for (int i = 3; i < 12; i++)
            {
                Assert.AreEqual(SitemapChangeFrequency.Hourly, result[i].ChangeFrequency);
                Assert.IsTrue(result[i].Location.ToString().StartsWith($"Product/{i - 2}/Product-"));
                Assert.IsTrue(result[i].Priority.HasValue);
                Assert.AreEqual(0.5m, result[i].Priority.Value);
            }
        }
    }
}
