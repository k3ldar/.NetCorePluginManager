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
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  Tests for Product Manager Plugin Initialisation
 *
 *  Date        Name                Reason
 *  31/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using PluginManager.Abstractions;

using ProductPlugin;

using pm = PluginManager.Internal;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductPluginSettingsTests
    {
        private const string TestCategoryName = "Product Manager Tests";
        private const string DemoWebsiteImagePath = "..\\..\\..\\..\\..\\..\\.NetCorePluginManager\\Demo\\NetCorePluginDemoWebsite\\wwwroot\\images";


        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_ContainsDefaultPropertyValues_Success()
        {
            ProductPluginSettings sut = new ProductPluginSettings();
            Assert.IsNotNull(sut);

            Assert.AreEqual(0u, sut.ProductsPerPage);
            Assert.IsNull(sut.PriceGroups);
            Assert.IsFalse(sut.ShowProductCounts);
            Assert.IsFalse(sut.ResizeImages);
            Assert.IsNull(sut.ResizeWidths);
            Assert.IsNull(sut.ResizeBackfillColor);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_SetPropertyValues_Success()
        {
            ProductPluginSettings sut = new ProductPluginSettings()
            {
                PriceGroups = "0;10",
                ProductsPerPage = 6,
                ShowProductCounts = true,
                ResizeImages = true,
                ResizeWidths = "89;500",
                ResizeBackfillColor = "#ff2309"
            };
            Assert.IsNotNull(sut);

            Assert.AreEqual(6u, sut.ProductsPerPage);
            Assert.AreEqual("0;10", sut.PriceGroups);
            Assert.IsTrue(sut.ShowProductCounts);
            Assert.IsTrue(sut.ResizeImages);
            Assert.AreEqual("89;500", sut.ResizeWidths);
            Assert.AreEqual("#ff2309", sut.ResizeBackfillColor);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ViaSettingsProvider_ContainsDefaultValues_Success()
        {
            ISettingsProvider settingsProvider = new TestSettingsProvider("{}");
            ProductPluginSettings sut = settingsProvider.GetSettings<ProductPluginSettings>("Products");
            Assert.IsNotNull(sut);

            Assert.AreEqual(12u, sut.ProductsPerPage);
            Assert.AreEqual("0;5.00;10.00;20.00;35.00;50.00", sut.PriceGroups);
            Assert.IsTrue(sut.ShowProductCounts);
            Assert.IsTrue(sut.ResizeImages);
            Assert.AreEqual("178x128;148x114;200x145;89x64;288x268", sut.ResizeWidths);
            Assert.AreEqual("#FFFFFF", sut.ResizeBackfillColor);
        }
    }
}
