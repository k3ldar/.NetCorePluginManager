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
 *  Product:  PluginManager.DAL.TextFiles.Tests
 *  
 *  File: LoginProviderTests.cs
 *
 *  Purpose:  LoginProviderTests Tests for text based storage
 *
 *  Date        Name                Reason
 *  29/08/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Resources;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;
using PluginManager.Tests.Mocks;

using SimpleDB;
using SimpleDB.Internal;
using SimpleDB.Tests.Mocks;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class ResourceProviderTests : BaseProviderTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidInstance_TableUserNull_Throws_ArgumentNullException()
		{
			new ResourceProvider(null, null, null, null);
		}

		[TestMethod]
		[ExpectedException(typeof(UniqueIndexException))]
		public void InsertRecord_DuplicateName_Throws_UniqueIndexException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 5; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
						});
					}
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(UniqueIndexException))]
		public void InsertRecord_DuplicateRouteName_Throws_UniqueIndexException()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 5; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
						});
					}

					resources.Insert(new ResourceCategoryDataRow()
					{
						Name = $"Resource 1",
						Description = $"test resource 1",
						ForeColor = "black",
						BackColor = "white",
						RouteName = $"resource-1"
					});
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetAllResources_RetrievesValidListOfResources_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 5; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							RouteName = $"resource-{i}"
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					List<ResourceCategory> resourceList = sut.GetAllResources();

					Assert.IsNotNull(resourceList);
					Assert.AreEqual(5, resourceList.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetResourceFromRouteName_RetrievesResourceNotFound_ReturnsNull()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 5; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							RouteName = $"resource-{i}"
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceCategory resource = sut.GetResourceFromRouteName("invalid resource route name");

					Assert.IsNull(resource);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetResourceFromRouteName_RetrievesValidResource_ReturnsResource()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					ISimpleDBOperations<ResourceCategoryDataRow> resourceCategories = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resourceCategories);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceCategories);

					for (int i = 0; i < 5; i++)
					{
						ResourceCategoryDataRow newCategory = new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							RouteName = $"resource-{i}",
						};

						resourceCategories.Insert(newCategory);

						if (i == 3)
						{
							for (int j = 0; j < 10; j++)
							{
								resourceItems.Insert(new ResourceItemDataRow()
								{
									CategoryId = i,
									Approved = true,
									Description = $"Description of {i} {j}",
									Name = $"Name of {i} {j}",
									UserName = "admin",
								});
							}
						}
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceCategory resource = sut.GetResourceFromRouteName("resource-3");

					Assert.IsNotNull(resource);
					Assert.AreEqual("Resource 3", resource.Name);
					Assert.IsNotNull(resource.ResourceItems);
					Assert.AreEqual(10, resource.ResourceItems.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void IncrementResourceItemResponse_UserNotFound_ReturnsNull()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					ISimpleDBOperations<ResourceCategoryDataRow> resourceCategories = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resourceCategories);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceCategories);

					ISimpleDBOperations<ResourceItemUserResponseDataRow> resourceItemResponses = provider.GetRequiredService<ISimpleDBOperations<ResourceItemUserResponseDataRow>>();
					Assert.IsNotNull(resourceItemResponses);
					Assert.AreEqual(0, resourceItemResponses.RecordCount);

					for (int i = 0; i < 5; i++)
					{
						ResourceCategoryDataRow newCategory = new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							RouteName = $"resource-{i}",
						};

						resourceCategories.Insert(newCategory);

						if (i == 3)
						{
							for (int j = 0; j < 10; j++)
							{
								resourceItems.Insert(new ResourceItemDataRow()
								{
									CategoryId = i,
									Approved = true,
									Description = $"Description of {i} {j}",
									Name = $"Name of {i} {j}",
									UserName = "admin",
								});
							}
						}
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem resource = sut.IncrementResourceItemResponse(1, int.MaxValue, true);

					Assert.IsNull(resource);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void IncrementResourceItemResponse_UserNotPreviouslyVoted_RowCreated_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService<ISimpleDBOperations<UserDataRow>>();
					Assert.IsNotNull(userTable);

					userTable.Insert(new UserDataRow()
					{
						FirstName = "Joe",
						Surname = "Bloggs"
					});

					ISimpleDBOperations<ResourceCategoryDataRow> resourceCategories = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resourceCategories);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceCategories);

					ISimpleDBOperations<ResourceItemUserResponseDataRow> resourceItemResponses = provider.GetRequiredService<ISimpleDBOperations<ResourceItemUserResponseDataRow>>();
					Assert.IsNotNull(resourceItemResponses);
					Assert.AreEqual(0, resourceItemResponses.RecordCount);

					for (int i = 0; i < 5; i++)
					{
						ResourceCategoryDataRow newCategory = new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							RouteName = $"resource-{i}",
						};

						resourceCategories.Insert(newCategory);

						if (i == 3)
						{
							for (int j = 0; j < 10; j++)
							{
								resourceItems.Insert(new ResourceItemDataRow()
								{
									CategoryId = i,
									Approved = true,
									Description = $"Description of {i} {j}",
									Name = $"Name of {i} {j}",
									UserName = "admin",
								});
							}
						}
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem resource = sut.IncrementResourceItemResponse(1, 1, true);

					Assert.IsNotNull(resource);
					Assert.AreEqual(1, resource.Likes);
					Assert.AreEqual(0, resource.Dislikes);

					Assert.AreEqual(1, resourceItemResponses.RecordCount);

					ResourceItemUserResponseDataRow response = resourceItemResponses.Select(0);
					Assert.IsNotNull(response);
					Assert.IsTrue(response.Like);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void IncrementResourceItemResponse_UserChangesVote_RowUpdated_Success()
		{
			string directory = TestHelper.GetTestPath();
			try
			{
				Directory.CreateDirectory(directory);
				PluginInitialisation initialisation = new PluginInitialisation();
				ServiceCollection services = CreateDefaultServiceCollection(directory, out MockPluginClassesService mockPluginClassesService);

				using (ServiceProvider provider = services.BuildServiceProvider())
				{
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

					ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService<ISimpleDBOperations<UserDataRow>>();
					Assert.IsNotNull(userTable);

					userTable.Insert(new UserDataRow()
					{
						FirstName = "Joe",
						Surname = "Bloggs"
					});

					ISimpleDBOperations<ResourceCategoryDataRow> resourceCategories = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resourceCategories);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceCategories);

					ISimpleDBOperations<ResourceItemUserResponseDataRow> resourceItemResponses = provider.GetRequiredService<ISimpleDBOperations<ResourceItemUserResponseDataRow>>();
					Assert.IsNotNull(resourceItemResponses);
					Assert.AreEqual(0, resourceItemResponses.RecordCount);

					for (int i = 0; i < 5; i++)
					{
						ResourceCategoryDataRow newCategory = new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							RouteName = $"resource-{i}",
						};

						resourceCategories.Insert(newCategory);

						if (i == 3)
						{
							for (int j = 0; j < 10; j++)
							{
								resourceItems.Insert(new ResourceItemDataRow()
								{
									CategoryId = i,
									Approved = true,
									Description = $"Description of {i} {j}",
									Name = $"Name of {i} {j}",
									UserName = "admin",
								});
							}
						}
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem resource = sut.IncrementResourceItemResponse(1, 1, true);

					Assert.IsNotNull(resource);
					Assert.AreEqual(1, resource.Likes);
					Assert.AreEqual(0, resource.Dislikes);

					Assert.AreEqual(1, resourceItemResponses.RecordCount);

					ResourceItemUserResponseDataRow response = resourceItemResponses.Select(0);
					Assert.IsNotNull(response);
					Assert.IsTrue(response.Like);

					resource = sut.IncrementResourceItemResponse(1, 1, false);

					Assert.IsNotNull(resource);
					Assert.AreEqual(0, resource.Likes);
					Assert.AreEqual(1, resource.Dislikes);

					Assert.AreEqual(1, resourceItemResponses.RecordCount);

					response = resourceItemResponses.Select(0);
					Assert.IsNotNull(response);
					Assert.IsFalse(response.Like);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}
