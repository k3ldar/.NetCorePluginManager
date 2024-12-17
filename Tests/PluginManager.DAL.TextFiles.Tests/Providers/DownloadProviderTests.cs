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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: DownloadProviderTests.cs
 *
 *  Purpose:  Download provider test for text based storage
 *
 *  Date        Name                Reason
 *  25/07/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Downloads;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;
using PluginManager.Tests.Mocks;
using Shared.Classes;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	[DoNotParallelize]
	public class DownloadProviderTests : BaseProviderTests
	{
		[TestInitialize]
		public void Setup()
		{
			ThreadManager.Initialise();
		}

		[TestMethod]
		public void DownloadCategoriesGet_AnyUser_ReturnsCategoryListForAllUsers()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
					Assert.IsNotNull(downloadCategoriesTable);

					downloadCategoriesTable.Insert(
					[
						new() { Name = "Cat 1" },
						new() { Name = "Cat 2" },
						new() { Name = "Cat 3" },
					]);

					ISimpleDBOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();
					Assert.IsNotNull(downloadItemsTable);

					downloadItemsTable.Insert(
					[
						new() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
						new() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
						new() { Name = "DL3", UserId = 0, CategoryId = 1, Description = "dl3", Filename = "dl3.txt" },
						new() { Name = "DL4", UserId = 0, CategoryId = 1, Description = "dl4", Filename = "dl4.txt" },
						new() { Name = "DL5", UserId = 0, CategoryId = 1, Description = "dl5", Filename = "dl5.txt" },
						new() { Name = "DL6", UserId = 0, CategoryId = 2, Description = "dl6", Filename = "dl6.txt" },
						new() { Name = "DL7", UserId = 1, CategoryId = 2, Description = "dl7", Filename = "dl7.txt" },
					]);

					List<DownloadCategory> result = sut.DownloadCategoriesGet(0);
					Assert.IsNotNull(result);
					Assert.AreEqual(3, result.Count);
					Assert.AreEqual("Cat 1", result[0].Name);
					Assert.AreEqual(1, result[0].Downloads.Count);

					Assert.AreEqual("Cat 2", result[1].Name);
					Assert.AreEqual(3, result[1].Downloads.Count);

					Assert.AreEqual("Cat 3", result[2].Name);
					Assert.AreEqual(1, result[2].Downloads.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void DownloadCategoriesGet_SpecificUser_ReturnsCategoryListForUser()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
					Assert.IsNotNull(downloadCategoriesTable);

					downloadCategoriesTable.Insert(
					[
						new() { Name = "Cat 1" },
						new() { Name = "Cat 2" },
						new() { Name = "Cat 3" },
					]);

					ISimpleDBOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();
					Assert.IsNotNull(downloadItemsTable);

					downloadItemsTable.Insert(
					[
						new() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
						new() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
						new() { Name = "DL3", UserId = 0, CategoryId = 1, Description = "dl3", Filename = "dl3.txt" },
						new() { Name = "DL4", UserId = 0, CategoryId = 1, Description = "dl4", Filename = "dl4.txt" },
						new() { Name = "DL5", UserId = 0, CategoryId = 1, Description = "dl5", Filename = "dl5.txt" },
						new() { Name = "DL6", UserId = 0, CategoryId = 2, Description = "dl6", Filename = "dl6.txt" },
						new() { Name = "DL7", UserId = 1, CategoryId = 2, Description = "dl7", Filename = "dl7.txt" },
					]);

					List<DownloadCategory> result = sut.DownloadCategoriesGet(1);
					Assert.IsNotNull(result);
					Assert.AreEqual(3, result.Count);
					Assert.AreEqual("Cat 1", result[0].Name);
					Assert.AreEqual(2, result[0].Downloads.Count);

					Assert.AreEqual("Cat 2", result[1].Name);
					Assert.AreEqual(3, result[1].Downloads.Count);

					Assert.AreEqual("Cat 3", result[2].Name);
					Assert.AreEqual(2, result[2].Downloads.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void DownloadCategoriesGet_ReturnsCategoryListForAnyUser()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
					Assert.IsNotNull(downloadCategoriesTable);

					downloadCategoriesTable.Insert(
					[
						new() { Name = "Cat 1" },
						new() { Name = "Cat 2" },
						new() { Name = "Cat 3" },
					]);

					ISimpleDBOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();
					Assert.IsNotNull(downloadItemsTable);

					downloadItemsTable.Insert(
					[
						new() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
						new() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
						new() { Name = "DL3", UserId = 0, CategoryId = 1, Description = "dl3", Filename = "dl3.txt" },
						new() { Name = "DL4", UserId = 0, CategoryId = 1, Description = "dl4", Filename = "dl4.txt" },
						new() { Name = "DL5", UserId = 0, CategoryId = 1, Description = "dl5", Filename = "dl5.txt" },
						new() { Name = "DL6", UserId = 0, CategoryId = 2, Description = "dl6", Filename = "dl6.txt" },
						new() { Name = "DL7", UserId = 1, CategoryId = 2, Description = "dl7", Filename = "dl7.txt" },
					]);

					List<DownloadCategory> result = sut.DownloadCategoriesGet();
					Assert.IsNotNull(result);
					Assert.AreEqual(3, result.Count);
					Assert.AreEqual("Cat 1", result[0].Name);
					Assert.AreEqual(1, result[0].Downloads.Count);

					Assert.AreEqual("Cat 2", result[1].Name);
					Assert.AreEqual(3, result[1].Downloads.Count);

					Assert.AreEqual("Cat 3", result[2].Name);
					Assert.AreEqual(1, result[2].Downloads.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetDownloadItem_ItemNotFound_ReturnsNull()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					DownloadItem result = sut.GetDownloadItem(38);
					Assert.IsNull(result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetDownloadItem_AllUsers_ReturnsDownLoadItem()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
					Assert.IsNotNull(downloadCategoriesTable);

					downloadCategoriesTable.Insert(
					[
						new() { Name = "Cat 1" },
					]);

					ISimpleDBOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();
					Assert.IsNotNull(downloadItemsTable);

					downloadItemsTable.Insert(
					[
						new() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
						new() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
					]);

					DownloadItem result = sut.GetDownloadItem(1);
					Assert.IsNotNull(result);
					Assert.AreEqual("dl1", result.Description);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetDownloadItem_UsersSpecificFile_ReturnsDownLoadItem()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
					Assert.IsNotNull(downloadCategoriesTable);

					downloadCategoriesTable.Insert(
					[
						new() { Name = "Cat 1" },
					]);

					ISimpleDBOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();
					Assert.IsNotNull(downloadItemsTable);

					downloadItemsTable.Insert(
					[
						new() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
						new() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
					]);

					DownloadItem result = sut.GetDownloadItem(1, 1);
					Assert.IsNotNull(result);
					Assert.AreEqual("dl1", result.Description);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ItemDownloaded_AllUsers_DownloadCountIncremented()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
					Assert.IsNotNull(downloadCategoriesTable);

					downloadCategoriesTable.Insert(
					[
						new() { Name = "Cat 1" },
					]);

					ISimpleDBOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();
					Assert.IsNotNull(downloadItemsTable);

					downloadItemsTable.Insert(
					[
						new() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
						new() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
					]);

					DownloadItem result = sut.GetDownloadItem(1);
					Assert.IsNotNull(result);
					Assert.AreEqual("dl1", result.Description);

					for (int i = 1; i < 100; i++)
					{
						sut.ItemDownloaded(result.Id);

						DownloadItemsDataRow item = downloadItemsTable.Select(result.Id);
						Assert.AreEqual(i, item.DownloadCount);
					}
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ItemDownloaded_SpecificUser_DownloadCountIncremented()
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

					DownloadProvider sut = (DownloadProvider)provider.GetService<IDownloadProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoriesTable = provider.GetService<ISimpleDBOperations<DownloadCategoryDataRow>>();
					Assert.IsNotNull(downloadCategoriesTable);

					downloadCategoriesTable.Insert(
					[
						new() { Name = "Cat 1" },
					]);

					ISimpleDBOperations<DownloadItemsDataRow> downloadItemsTable = provider.GetService<ISimpleDBOperations<DownloadItemsDataRow>>();
					Assert.IsNotNull(downloadItemsTable);

					downloadItemsTable.Insert(
					[
						new() { Name = "DL1", UserId = 1, CategoryId = 0, Description = "dl1", Filename = "dl1.txt" },
						new() { Name = "DL2", UserId = 0, CategoryId = 0, Description = "dl2", Filename = "dl2.txt" },
					]);

					DownloadItem result = sut.GetDownloadItem(1, 1);
					Assert.IsNotNull(result);
					Assert.AreEqual("dl1", result.Description);

					for (int i = 1; i < 100; i++)
					{
						sut.ItemDownloaded(1, result.Id);

						DownloadItemsDataRow item = downloadItemsTable.Select(result.Id);
						Assert.AreEqual(i, item.DownloadCount);
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
