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
 *  File: ProductSearchViewModel.cs
 *
 *  Purpose:  Tests for product search view model
 *
 *  Date        Name                Reason
 *  30/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ProductPlugin.Models;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ProductSearchViewModelTests : GenericBaseClass
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_DefaultConstructor_Success()
        {
            ProductSearchViewModel sut = new ProductSearchViewModel()
            {
                SearchText = "prod",
                ContainsVideo = true,
                SearchName = "name",
                VideoProductCount = 21
            };

            Assert.IsNotNull(sut.ProductGroups);
            Assert.AreEqual(0, sut.ProductGroups.Count);
            Assert.IsNotNull(sut.Prices);
            Assert.AreEqual(0, sut.Prices.Count);
            Assert.AreEqual("prod", sut.SearchText);
            Assert.AreEqual("name", sut.SearchName);
            Assert.IsTrue(sut.ContainsVideo);
            Assert.AreEqual(21, sut.VideoProductCount);
        }
    }
}