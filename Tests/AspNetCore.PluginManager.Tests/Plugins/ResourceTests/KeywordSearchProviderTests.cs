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
 *  File: KeywordSearchProviderTests.cs
 *
 *  Purpose:  Tests for resourc plugin keyword search provider
 *
 *  Date        Name                Reason
 *  30/10/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AspNetCore.PluginManager.DemoWebsite.Classes.Mocks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware.Resources;
using Middleware.Search;

using Resources.Plugin.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.ResourceTests
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class KeywordSearchProviderTests
	{
		private const string TestCategoryName = "Resource Tests";

		[TestMethod]
		[TestCategory(TestCategoryName)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParam_Null_Throws_ArgumentNullException()
		{
			new KeywordSearchProvider(null);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void AdvancedSearch_ReturnsNull()
		{
			KeywordSearchProvider sut = new KeywordSearchProvider(new MockResourceProvider());
			Assert.IsNotNull(sut);
			Assert.IsNull(sut.AdvancedSearch());
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void SearchResponseTypes_QuickSearch_ReturnsQuickSearchOptions()
		{
			KeywordSearchProvider sut = new KeywordSearchProvider(new MockResourceProvider());
			List<string> searchTypes = sut.SearchResponseTypes(true);

			Assert.AreEqual(2, searchTypes.Count);
			Assert.AreEqual("Name", searchTypes[0]);
			Assert.AreEqual("Tags", searchTypes[1]);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void SearchResponseTypes_QuickSearch_ReturnsAdvancedSearchOptions()
		{
			KeywordSearchProvider sut = new KeywordSearchProvider(new MockResourceProvider());
			List<string> searchTypes = sut.SearchResponseTypes(false);

			Assert.AreEqual(3, searchTypes.Count);
			Assert.AreEqual("Name", searchTypes[0]);
			Assert.AreEqual("Tags", searchTypes[1]);
			Assert.AreEqual("Description", searchTypes[2]);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Search_InvalidParamNull_Throws_ArgumentNullException()
		{
			KeywordSearchProvider sut = new KeywordSearchProvider(new MockResourceProvider());
			sut.Search(null);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Search_ExactMatch_FindsAllResources_Success()
		{
			KeywordSearchProvider sut = new KeywordSearchProvider(new MockResourceProvider());
			KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "resource", true, true);

			List<SearchResponseItem> results = sut.Search(searchOptions);
			Assert.IsNotNull(results);
			Assert.AreEqual(99, results.Count);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Search_ExactMatch_MaximumNumberOfSearchItemsFound_Success()
		{
			KeywordSearchProvider sut = new KeywordSearchProvider(new MockResourceProvider());
			KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "resource", true, true)
			{
				MaximumSearchResults = 15
			};

			List<SearchResponseItem> results = sut.Search(searchOptions);
			Assert.IsNotNull(results);
			Assert.AreEqual(15, results.Count);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Search_ExactMatch_FindsByTag_Success()
		{
			MockResourceProvider mockResourceProvider = new MockResourceProvider();
			KeywordSearchProvider sut = new KeywordSearchProvider(mockResourceProvider);
			KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "testCase", true, true);
			
			ResourceItem resourceItem = mockResourceProvider.GetResourceItem(101);
			resourceItem.Tags.Add("testCase");
			mockResourceProvider.UpdateResourceItem(1, resourceItem);

			List<SearchResponseItem> results = sut.Search(searchOptions);
			Assert.IsNotNull(results);
			Assert.AreEqual(1, results.Count);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Search_PartialMatch_ReturnsMatchingItems_Success()
		{
			MockResourceProvider mockResourceProvider = new MockResourceProvider();
			KeywordSearchProvider sut = new KeywordSearchProvider(mockResourceProvider);
			KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "Case", false, false);

			ResourceItem resourceItem = mockResourceProvider.GetResourceItem(101);
			resourceItem.Tags.Add("testCase");
			mockResourceProvider.UpdateResourceItem(1, resourceItem);

			List<SearchResponseItem> results = sut.Search(searchOptions);
			Assert.IsNotNull(results);
			Assert.AreEqual(1, results.Count);
		}

		[TestMethod]
		[TestCategory(TestCategoryName)]
		public void Search_PartialMatch_AllResourcesWithTheNumberThreeOrTest_Success()
		{
			MockResourceProvider mockResourceProvider = new MockResourceProvider();
			KeywordSearchProvider sut = new KeywordSearchProvider(mockResourceProvider);
			KeywordSearchOptions searchOptions = new KeywordSearchOptions(false, "3 test", false, false);

			ResourceItem resourceItem = mockResourceProvider.GetResourceItem(101);
			resourceItem.Tags.Add("testCase");
			mockResourceProvider.UpdateResourceItem(1, resourceItem);

			List<SearchResponseItem> results = sut.Search(searchOptions);
			Assert.IsNotNull(results);
			Assert.AreEqual(30, results.Count);
		}
	}
}
