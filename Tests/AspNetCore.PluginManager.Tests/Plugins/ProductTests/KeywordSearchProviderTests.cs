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
 *  File: KeywordSearchProviderTests.cs
 *
 *  Purpose:  Tests for product plugin keyword search provider
 *
 *  Date        Name                Reason
 *  26/11/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Search;

using ProductPlugin.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.ProductTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class KeywordSearchProviderTests
    {
        private const string TestCategoryName = "Product Manager Tests";

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidParam_Null_Throws_ArgumentNullException()
        {
            new KeywordSearchProvider(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Construct_ValidInstance_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());

            Assert.IsInstanceOfType(sut, typeof(ISearchKeywordProvider));
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Validate_Public_Constants_Success()
        {
            Assert.AreEqual("ContainsVideo", KeywordSearchProvider.ContainsVideo);
            Assert.AreEqual("ProductGroup", KeywordSearchProvider.ProductGroup);
            Assert.AreEqual("Price", KeywordSearchProvider.Price);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SearchResponseTypes_QuickSearch_ReturnsSearchTypes_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            List<string> searchTypes = sut.SearchResponseTypes(true);

            Assert.AreEqual(1, searchTypes.Count);
            Assert.AreEqual("ProductName", searchTypes[0]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void AdvancedSearch_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            Dictionary<string, AdvancedSearchOptions> advancedSearchResult = sut.AdvancedSearch();

            Assert.AreEqual(1, advancedSearchResult.Count);
            Assert.IsTrue(advancedSearchResult.ContainsKey("Products"));

            AdvancedSearchOptions prodSearchOption = advancedSearchResult["Products"];
            Assert.AreEqual("AdvancedSearch", prodSearchOption.ActionName);
            Assert.AreEqual("Product", prodSearchOption.ControllerName);
            Assert.AreEqual("/Product/Search/", prodSearchOption.SearchName);
            Assert.AreEqual("/Product/SearchOptions/", prodSearchOption.SearchOption);
            Assert.AreEqual("/css/products.css", prodSearchOption.StyleSheet);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void SearchResponseTypes_NormalSearch_ReturnsSearchTypes_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            List<string> searchTypes = sut.SearchResponseTypes(false);

            Assert.AreEqual(3, searchTypes.Count);
            Assert.AreEqual("ProductName", searchTypes[0]);
            Assert.AreEqual("ProductDescription", searchTypes[1]);
            Assert.AreEqual("ProductSku", searchTypes[2]);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Search_InvalidParam_Null_Throws_ArgumentNullException()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            sut.Search(null);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_ExactMatchQuickSearch_FindsSingleProduct_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product b", true, true);

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_ExactMatchNormalSearch_FindsSingleProduct_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product c", false, true);

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_QuickSearch_FindsSingleProduct_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product a", true, false);

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(1, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_FindbySku_LimitToThreeResults_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "SKU", false, true);
            searchOptions.MaximumSearchResults = 3;

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_FindbyProductDescription_LimitToFiveResults_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "description", false, true);
            searchOptions.MaximumSearchResults = 5;

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(2, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_FindbyProductName_LimitToFourResults_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product", true, true);
            searchOptions.MaximumSearchResults = 4;

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(4, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_FindbyProductName_LimitedToTotalZeroResults_NoProductsAdded_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product", true, true);
            searchOptions.MaximumSearchResults = 0;

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_Partial_FindsAllProducts_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product \r sku desc", false, false);
            searchOptions.MaximumSearchResults = 500;

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(14, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_Partial_FindsAllProductsWithAPrice_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product \r sku desc", false, false);
            searchOptions.MaximumSearchResults = 500;
            searchOptions.Properties.Add("ProductGroup", "Main Products");
            searchOptions.Properties.Add("InvalidGroupName", "Invalid Product Group Name");
            searchOptions.Properties.Add("Price", new List<ProductPriceInfo>()
            {
                new ProductPriceInfo("0 to 4.99", 0, 4.99m)
            });

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(11, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_Partial_FindsAllProductsWithGroup_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product \r sku desc", false, false);
            searchOptions.MaximumSearchResults = 500;
            searchOptions.Properties.Add("ProductGroup", "Main Products");
            searchOptions.Properties.Add("InvalidGroupName", "Invalid Product Group Name");
            searchOptions.Properties.Add("ContainsVideo", "");

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(4, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_Partial_FindsAllProductsWithVideo_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "", false, false);
            searchOptions.MaximumSearchResults = 500;
            searchOptions.Properties.Add("ContainsVideo", "");

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_Partial_FindsAllProducts_UptoSpecifiedCount_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "product \r sku desc", false, false);
            searchOptions.MaximumSearchResults = 4;

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(4, results.Count);
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        public void Search_ProductGroup_Success()
        {
            KeywordSearchProvider sut = new KeywordSearchProvider(new MockProductProvider());
            KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "main product", false, false);
            searchOptions.MaximumSearchResults = 500;
            searchOptions.Properties.Add("Main Products", "ProductGroup");

            List<SearchResponseItem> results = sut.Search(searchOptions);

            Assert.AreEqual(3, results.Count);
        }
    }
}
