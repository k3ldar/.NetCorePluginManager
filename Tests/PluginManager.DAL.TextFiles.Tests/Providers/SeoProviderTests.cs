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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: CountriesProviderTests.cs
 *
 *  Purpose:  Countries test for text based storage
 *
 *  Date        Name                Reason
 *  07/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;
using SimpleDB.Tests.Mocks;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class SeoProviderTests : BaseProviderTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidParam_SeoDataNull_Throws_ArgumentNullException()
		{
			new SeoProvider(null);
		}

		[TestMethod]
		public void Construct_ValidInstance_Success()
		{
			SeoProvider sut = new(new MockTextTableOperations<SeoDataRow>());

			Assert.IsNotNull(sut);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddKeyword_RouteNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					Assert.IsNotNull(sut);
					sut.AddKeyword(null, "keyword");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddKeyword_KeywordNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					Assert.IsNotNull(sut);
					sut.AddKeyword("/route", null);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddKeyword_KeywordAddedToNewRoute_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					Assert.IsNotNull(sut);
					bool result = sut.AddKeyword("/route", "key");

					Assert.IsTrue(result);

					IReadOnlyList<SeoDataRow> seoData = seoTable.Select();
					Assert.IsNotNull(seoData);
					Assert.AreEqual(1, seoData.Count);
					Assert.AreEqual("/route", seoData[0].Route);
					Assert.AreEqual(1, seoData[0].Keywords.Count);
					Assert.AreEqual("key", seoData[0].Keywords[0]);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddKeyword_DuplicateKeywordAddedToRoute_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					Assert.IsNotNull(sut);

					bool result = sut.AddKeyword("/route", "key");
					Assert.IsTrue(result);

					result = sut.AddKeyword("/route", "key");
					Assert.IsFalse(result);

					IReadOnlyList<SeoDataRow> seoData = seoTable.Select();
					Assert.IsNotNull(seoData);
					Assert.AreEqual(1, seoData.Count);
					Assert.AreEqual("/route", seoData[0].Route);
					Assert.AreEqual(1, seoData[0].Keywords.Count);
					Assert.AreEqual("key", seoData[0].Keywords[0]);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddKeywords_RouteNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					Assert.IsNotNull(sut);
					sut.AddKeywords(null, new List<string>());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddKeywords_KeywordsNull_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					Assert.IsNotNull(sut);
					bool result = sut.AddKeywords("/", null);
					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddKeywords_AddMultipleWithDuplicates_OnlyUniqueItemsAdded()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
					};

					newRecord.Keywords.Add("four");
					newRecord.Keywords.Add("five");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					Assert.IsNotNull(sut);

					List<string> keywords = new()
					{
						"one",
						"two",
						"three",
						"four",
					};

					bool result = sut.AddKeywords("/route", keywords);
					Assert.IsTrue(result);

					IReadOnlyList<SeoDataRow> seoData = seoTable.Select();
					Assert.IsNotNull(seoData);
					Assert.AreEqual(1, seoData.Count);
					Assert.AreEqual("/route", seoData[0].Route);
					Assert.AreEqual(5, seoData[0].Keywords.Count);
					Assert.AreEqual("four", seoData[0].Keywords[0]);
					Assert.AreEqual("five", seoData[0].Keywords[1]);
					Assert.AreEqual("one", seoData[0].Keywords[2]);
					Assert.AreEqual("two", seoData[0].Keywords[3]);
					Assert.AreEqual("three", seoData[0].Keywords[4]);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetSeoDataForRoute_NoDataFound_Returns_False()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.GetSeoDataForRoute("/route", out string title, out string metaDescription,
						out string author, out List<string> keywords);
					Assert.IsFalse(result);

					Assert.AreEqual("", title);
					Assert.AreEqual("", metaDescription);
					Assert.AreEqual("", author);
					Assert.IsNotNull(keywords);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetSeoDataForRoute_DataFound_Returns_True()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
						Title = "the title",
						Description = "the description",
						Author = "the author",
					};

					newRecord.Keywords.Add("one");
					newRecord.Keywords.Add("two");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.GetSeoDataForRoute("/route", out string title, out string metaDescription,
						out string author, out List<string> keywords);
					Assert.IsTrue(result);

					Assert.IsNotNull(title);
					Assert.AreEqual("the title", title);

					Assert.IsNotNull(metaDescription);
					Assert.AreEqual("the description", metaDescription);

					Assert.IsNotNull(author);
					Assert.AreEqual("the author", author);

					Assert.IsNotNull(keywords);
					Assert.AreEqual(2, keywords.Count);
					Assert.AreEqual("one", keywords[0]);
					Assert.AreEqual("two", keywords[1]);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveKeyword_RouteIsNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					sut.RemoveKeyword(null, "keyword");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RemoveKeyword_KeywordIsNull_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.RemoveKeyword("/route", null);

					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RemoveKeyword_RouteNotFound_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.RemoveKeyword("/route", "keyword");
					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RemoveKeyword_KeywordNotFound_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
						Title = "the title",
						Description = "the description",
						Author = "the author",
					};

					newRecord.Keywords.Add("one");
					newRecord.Keywords.Add("two");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.RemoveKeyword("/route", "three");
					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RemoveKeyword_KeywordFoundAndRemoved_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
						Title = "the title",
						Description = "the description",
						Author = "the author",
					};

					newRecord.Keywords.Add("one");
					newRecord.Keywords.Add("two");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.RemoveKeyword("/route", "one");
					Assert.IsTrue(result);

					SeoDataRow record = seoTable.Select(0);
					Assert.IsNotNull(record);

					Assert.IsNotNull(record.Keywords);
					Assert.AreEqual(1, record.Keywords.Count);
					Assert.AreEqual("two", record.Keywords[0]);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveKeywords_RouteIsNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					sut.RemoveKeywords(null, new List<string>());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void RemoveKeywords_KeywordsIsNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					sut.RemoveKeywords("/route", null);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RemoveKeywords_RouteNotFound_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.RemoveKeywords("/route", new List<string>());
					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RemoveKeywords_KeywordsFoundAndRemoved_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
						Title = "the title",
						Description = "the description",
						Author = "the author",
					};

					newRecord.Keywords.Add("one");
					newRecord.Keywords.Add("two");
					newRecord.Keywords.Add("three");
					newRecord.Keywords.Add("four");
					newRecord.Keywords.Add("five");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.RemoveKeywords("/route", new List<string>() { "one", "four" });
					Assert.IsTrue(result);

					SeoDataRow record = seoTable.Select(0);
					Assert.IsNotNull(record);

					Assert.IsNotNull(record.Keywords);
					Assert.AreEqual(3, record.Keywords.Count);
					Assert.AreEqual("two", record.Keywords[0]);
					Assert.AreEqual("three", record.Keywords[1]);
					Assert.AreEqual("five", record.Keywords[2]);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateDescription_RouteIsNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					sut.UpdateDescription(null, "description");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateDescription_DescriptionIsNull_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.UpdateDescription("/route", null);

					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateDescription_RouteNotFound_RouteCreated_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.UpdateDescription("/route", "description");
					Assert.IsTrue(result);

					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = seoTable.Select(0);
					Assert.IsNotNull(newRecord);
					Assert.AreEqual("description", newRecord.Description);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateDescription_DataFoundAndUpdated_Returns_True()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
						Title = "the title",
						Description = "the description",
						Author = "the author",
					};

					newRecord.Keywords.Add("one");
					newRecord.Keywords.Add("two");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.UpdateDescription("/route", "new description");
					Assert.IsTrue(result);


					SeoDataRow updatedRecord = seoTable.Select(0);
					Assert.AreEqual("the title", updatedRecord.Title);
					Assert.AreEqual("new description", updatedRecord.Description);
					Assert.AreEqual("the author", updatedRecord.Author);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateTitle_RouteIsNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					sut.UpdateTitle(null, "title");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateTitle_TitleIsNull_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.UpdateTitle("/route", null);

					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateTitle_RouteNotFound_CreateRoute_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.UpdateTitle("/route", "title");
					Assert.IsTrue(result);

					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = seoTable.Select(0);
					Assert.IsNotNull(newRecord);
					Assert.AreEqual("title", newRecord.Title);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateTitle_DataFoundAndUpdated_Returns_True()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
						Title = "the title",
						Description = "the description",
						Author = "the author",
					};

					newRecord.Keywords.Add("one");
					newRecord.Keywords.Add("two");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.UpdateTitle("/route", "new title");
					Assert.IsTrue(result);


					SeoDataRow updatedRecord = seoTable.Select(0);
					Assert.AreEqual("new title", updatedRecord.Title);
					Assert.AreEqual("the description", updatedRecord.Description);
					Assert.AreEqual("the author", updatedRecord.Author);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateAuthor_RouteIsNull_Throws_ArgumentNullException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					sut.UpdateAuthor(null, "Author");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateAuthor_AuthorIsNull_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.UpdateAuthor("/route", null);

					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateAuthor_RouteNotFound_RouteCreated_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();

					bool result = sut.UpdateAuthor("/route", "author");
					Assert.IsTrue(result);

					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = seoTable.Select(0);
					Assert.IsNotNull(newRecord);
					Assert.AreEqual("author", newRecord.Author);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateAuthor_DataFoundAndUpdated_Returns_True()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<SeoDataRow> seoTable = provider.GetRequiredService<ISimpleDBOperations<SeoDataRow>>();
					Assert.IsNotNull(seoTable);

					SeoDataRow newRecord = new()
					{
						Route = "/route",
						Title = "the title",
						Description = "the description",
						Author = "the author",
					};

					newRecord.Keywords.Add("one");
					newRecord.Keywords.Add("two");

					seoTable.Insert(newRecord);

					ISeoProvider sut = provider.GetRequiredService<ISeoProvider>();


					bool result = sut.UpdateAuthor("/route", "new author");
					Assert.IsTrue(result);


					SeoDataRow updatedRecord = seoTable.Select(0);
					Assert.AreEqual("the title", updatedRecord.Title);
					Assert.AreEqual("the description", updatedRecord.Description);
					Assert.AreEqual("new author", updatedRecord.Author);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}
