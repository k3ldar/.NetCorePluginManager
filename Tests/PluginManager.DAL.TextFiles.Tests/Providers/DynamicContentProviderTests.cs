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
 *  File: DynamicContentProviderTests.cs
 *
 *  Purpose:  Dynamic content provider test for text based storage
 *
 *  Date        Name                Reason
 *  26/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using DynamicContent.Plugin.Templates;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.DynamicContent;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;
using PluginManager.Tests.Mocks;

using SharedPluginFeatures;
using SharedPluginFeatures.DynamicContent;
using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class DynamicContentProviderTests : BaseProviderTests
	{
		[TestInitialize]
		public void Setup()
		{
			ThreadManager.Initialise();
		}

		[TestMethod]
		public void CreateCustomPage_CreatesPage_ReturnsNewId()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					long result = sut.CreateCustomPage();
					Assert.AreEqual(0, result);
					Assert.AreEqual(1, pageTable.RecordCount);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Save_PageNotFound_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					DynamicContentPage dynamicContentpage = new(-10)
					{
						Name = "New Dynamic Content",
						RouteName = "home",
						ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
						ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
					};

					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "1", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });
					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "2", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });

					bool result = sut.Save(dynamicContentpage);

					Assert.IsFalse(result);
					Assert.AreEqual(0, pageTable.RecordCount);
					Assert.AreEqual(0, pageItemsTable.RecordCount);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Save_SavesChangesToTheDynamicPage_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					DynamicContentPage dynamicContentpage = new(sut.CreateCustomPage())
					{
						Name = "New Dynamic Content",
						RouteName = "home",
						ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
						ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
					};

					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "1", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });
					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "2", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });

					bool result = sut.Save(dynamicContentpage);

					Assert.IsTrue(result);
					Assert.AreEqual(1, pageTable.RecordCount);
					Assert.AreEqual(2, pageItemsTable.RecordCount);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RouteNameExists_RouteNotFound_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					DynamicContentPage dynamicContentpage = new(sut.CreateCustomPage())
					{
						Name = "New Dynamic Content",
						RouteName = "home",
						ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
						ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
					};

					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "1", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });
					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "2", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });

					bool result = sut.Save(dynamicContentpage);

					Assert.IsTrue(result);
					Assert.AreEqual(1, pageTable.RecordCount);
					Assert.AreEqual(2, pageItemsTable.RecordCount);

					result = sut.RouteNameExists(1, "page-1");
					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RouteNameExists_RouteFound_ReturnsTrue()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					DynamicContentPage dynamicContentpage = new(sut.CreateCustomPage())
					{
						Name = "New Dynamic Content",
						RouteName = "home",
						ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
						ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
					};

					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "1", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });
					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "2", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });

					bool result = sut.Save(dynamicContentpage);

					Assert.IsTrue(result);
					Assert.AreEqual(1, pageTable.RecordCount);
					Assert.AreEqual(2, pageItemsTable.RecordCount);

					result = sut.RouteNameExists(2, "home");
					Assert.IsTrue(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void PageNameExists_PageNotFound_ReturnsFalse()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					DynamicContentPage dynamicContentpage = new(sut.CreateCustomPage())
					{
						Name = "New Dynamic Content",
						RouteName = "home",
						ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
						ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
					};

					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "1", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });
					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "2", HeightType = DynamicContentHeightType.Automatic, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "blah" });

					bool result = sut.Save(dynamicContentpage);

					Assert.IsTrue(result);
					Assert.AreEqual(1, pageTable.RecordCount);
					Assert.AreEqual(2, pageItemsTable.RecordCount);

					result = sut.PageNameExists(1, "custom page");
					Assert.IsFalse(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void Templates_LoadsAllAvailableTemplates_ReturnsList()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					List<DynamicContentTemplate> result = sut.Templates();

					Assert.IsNotNull(result);
					Assert.AreEqual(3, result.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetCustomPage_PageNotFound_ReturnsNull()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					IDynamicContentPage result = sut.GetCustomPage(3);

					Assert.IsNull(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetCustomPage_PageFound_ReturnsValidInstance()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					_ = sut.CreateCustomPage();

					DynamicContentPage dynamicContentpage = new(sut.CreateCustomPage())
					{
						Name = "New Dynamic Content",
						RouteName = "home",
						ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
						ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
					};

					dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = "1", HeightType = DynamicContentHeightType.Pixels, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = "content 1" });
					dynamicContentpage.Content.Add(new YouTubeVideoTemplate() { UniqueId = "2", HeightType = DynamicContentHeightType.Percentage, Height = 80, Width = 100, WidthType = DynamicContentWidthType.Pixels, Data = "content 2" });

					bool isSaved = sut.Save(dynamicContentpage);
					Assert.IsTrue(isSaved);
					Assert.AreEqual(2, pageTable.RecordCount);
					Assert.AreEqual(2, pageItemsTable.RecordCount);

					IDynamicContentPage result = sut.GetCustomPage(1);

					Assert.IsNotNull(result);

					Assert.AreEqual(2, result.Content.Count);

					Assert.AreEqual("New Dynamic Content", result.Name);
					Assert.AreEqual("home", result.RouteName);

					Assert.AreEqual("1", result.Content[0].UniqueId);
					Assert.IsInstanceOfType(result.Content[0], typeof(HtmlTextTemplate));
					Assert.AreEqual(0, result.Content[0].Id);
					Assert.AreEqual(DynamicContentHeightType.Automatic, result.Content[0].HeightType);
					Assert.AreEqual(-1, result.Content[0].Height);
					Assert.AreEqual(DynamicContentWidthType.Percentage, result.Content[0].WidthType);
					Assert.AreEqual(100, result.Content[0].Width);
					Assert.AreEqual("content 1", result.Content[0].Data);

					Assert.AreEqual("2", result.Content[1].UniqueId);
					Assert.IsInstanceOfType(result.Content[1], typeof(YouTubeVideoTemplate));
					Assert.AreEqual(1, result.Content[1].Id);
					Assert.AreEqual(DynamicContentHeightType.Percentage, result.Content[1].HeightType);
					Assert.AreEqual(80, result.Content[1].Height);
					Assert.AreEqual(DynamicContentWidthType.Pixels, result.Content[1].WidthType);
					Assert.AreEqual(100, result.Content[1].Width);
					Assert.AreEqual("content 2|True", result.Content[1].Data);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetCustomPages_ReturnsListOfPages()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					for (int i = 0; i < 10; i++)
					{
						long id = sut.CreateCustomPage();
						DynamicContentPage dynamicContentpage = new(id)
						{
							Name = $"New Dynamic Content {id}",
							RouteName = $"home {id}",
							ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
							ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
						};

						dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = $"{id}1", HeightType = DynamicContentHeightType.Pixels, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = $"content {id} - 1" });
						dynamicContentpage.Content.Add(new YouTubeVideoTemplate() { UniqueId = $"{id}2", HeightType = DynamicContentHeightType.Percentage, Height = 80, Width = 100, WidthType = DynamicContentWidthType.Pixels, Data = $"content {id} - 2" });
						bool isSaved = sut.Save(dynamicContentpage);
						Assert.IsTrue(isSaved);
					}

					Assert.AreEqual(10, pageTable.RecordCount);
					Assert.AreEqual(20, pageItemsTable.RecordCount);

					List<IDynamicContentPage> result = sut.GetCustomPages();

					Assert.IsNotNull(result);

					Assert.AreEqual(10, result.Count);
					int j = -1;

					for (int i = 0; i < 10; i++)
					{
						Assert.AreEqual($"New Dynamic Content {i}", result[i].Name);
						Assert.AreEqual($"home {i}", result[i].RouteName);

						Assert.AreEqual($"{i}1", result[i].Content[0].UniqueId);
						Assert.IsInstanceOfType(result[i].Content[0], typeof(HtmlTextTemplate));
						Assert.AreEqual(++j, result[i].Content[0].Id);
						Assert.AreEqual(DynamicContentHeightType.Automatic, result[i].Content[0].HeightType);
						Assert.AreEqual(-1, result[i].Content[0].Height);
						Assert.AreEqual(DynamicContentWidthType.Percentage, result[i].Content[0].WidthType);
						Assert.AreEqual(100, result[i].Content[0].Width);
						Assert.AreEqual($"content {i} - 1", result[i].Content[0].Data);

						Assert.AreEqual($"{i}2", result[i].Content[1].UniqueId);
						Assert.IsInstanceOfType(result[i].Content[1], typeof(YouTubeVideoTemplate));
						Assert.AreEqual(++j, result[i].Content[1].Id);
						Assert.AreEqual(DynamicContentHeightType.Percentage, result[i].Content[1].HeightType);
						Assert.AreEqual(80, result[i].Content[1].Height);
						Assert.AreEqual(DynamicContentWidthType.Pixels, result[i].Content[1].WidthType);
						Assert.AreEqual(100, result[i].Content[1].Width);
						Assert.AreEqual($"content {i} - 2|True", result[i].Content[1].Data);
					}
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetCustomPageList_ReturnsListOfPages()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					DynamicContentProvider sut = (DynamicContentProvider)provider.GetService<IDynamicContentProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ContentPageDataRow> pageTable = provider.GetService<ISimpleDBOperations<ContentPageDataRow>>();
					Assert.IsNotNull(pageTable);
					Assert.AreEqual(0, pageTable.RecordCount);

					ISimpleDBOperations<ContentPageItemDataRow> pageItemsTable = provider.GetService<ISimpleDBOperations<ContentPageItemDataRow>>();
					Assert.IsNotNull(pageItemsTable);
					Assert.AreEqual(0, pageItemsTable.RecordCount);

					for (int i = 0; i < 10; i++)
					{
						long id = sut.CreateCustomPage();
						DynamicContentPage dynamicContentpage = new(id)
						{
							Name = $"New Dynamic Content {id}",
							RouteName = $"home {id}",
							ActiveFrom = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
							ActiveTo = new DateTime(2050, 12, 31, 23, 59, 59, DateTimeKind.Utc),
						};

						dynamicContentpage.Content.Add(new HtmlTextTemplate() { UniqueId = $"{id}1", HeightType = DynamicContentHeightType.Pixels, Width = 100, WidthType = DynamicContentWidthType.Percentage, Data = $"content {id} - 1" });
						dynamicContentpage.Content.Add(new YouTubeVideoTemplate() { UniqueId = $"{id}2", HeightType = DynamicContentHeightType.Percentage, Height = 80, Width = 100, WidthType = DynamicContentWidthType.Pixels, Data = $"content {id} - 2" });
						bool isSaved = sut.Save(dynamicContentpage);
						Assert.IsTrue(isSaved);
					}

					Assert.AreEqual(10, pageTable.RecordCount);
					Assert.AreEqual(20, pageItemsTable.RecordCount);

					List<LookupListItem> result = sut.GetCustomPageList();

					Assert.IsNotNull(result);

					Assert.AreEqual(10, result.Count);

					for (int i = 0; i < 10; i++)
					{
						Assert.AreEqual($"New Dynamic Content {i}", result[i].Description);
						Assert.AreEqual(i, result[i].Id);
					}
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}
