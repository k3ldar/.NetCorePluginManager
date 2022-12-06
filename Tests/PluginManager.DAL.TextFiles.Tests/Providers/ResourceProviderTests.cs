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
using PluginManager.DAL.TextFiles.Tables.Resources;
using PluginManager.Tests.Mocks;

using Shared.Classes;

using SimpleDB;
using SimpleDB.Internal;
using SimpleDB.Tests.Mocks;

#pragma warning disable CA1826

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
	[TestClass]
	[ExcludeFromCodeCoverage]
	public class ResourceProviderTests : BaseProviderTests
	{
		[TestInitialize]
		public void Setup() 
		{
			ThreadManager.Initialise();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void Construct_InvalidInstance_TableUserNull_Throws_ArgumentNullException()
		{
			new ResourceProvider(null, null, null, null, null);
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
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

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
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

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
					MockApplicationBuilder mockApplicationBuilder = new MockApplicationBuilder(provider);
					initialisation.AfterConfigure(mockApplicationBuilder);

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
							RouteName = $"resource-{i}",
							IsVisible = true,
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
		public void GetResourceCategory_RetrievesResourceNotFound_ReturnsNull()
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

					ResourceCategory resource = sut.GetResourceCategory(-10);

					Assert.IsNull(resource);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetResourceCategory_RetrievesValidResource_ReturnsResource()
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

					ResourceCategory resource = sut.GetResourceCategory(3);

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


					resource = sut.IncrementResourceItemResponse(1, 1, true);

					Assert.IsNotNull(resource);
					Assert.AreEqual(1, resource.Likes);
					Assert.AreEqual(0, resource.Dislikes);

					Assert.AreEqual(1, resourceItemResponses.RecordCount);

					response = resourceItemResponses.Select(0);
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
		public void IncrementResourceItemResponse_ItemNotFound_ReturnsNull()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem resource = sut.IncrementResourceItemResponse(100, 1, true);

					Assert.IsNull(resource);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceCategory_ResourceNameNull_Throws_ArgumentNullException()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceCategory(0, 0, null, "description");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceCategory_ResourceDescriptionNull_Throws_ArgumentNullException()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceCategory(0, 0, "New category", null);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceCategory_InvalidUserId_NotDefaultValue_UserDoesNotExist_Throws_ArgumentNullException()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceCategory(25, 0, "New category", "Description");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(UniqueIndexException))]
		public void AddResourceCategory_NameAlreadyExist_Throws_UniqueIndexException()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Description = "description",
						Name = "test",						
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceCategory(0, 0, "test", "Description");
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddResourceCategory_CategoryCreated_Success()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceCategory result = sut.AddResourceCategory(1, 0, "test cat", "The Description");

					Assert.IsNotNull(result);
					Assert.AreEqual("test cat", result.Name);
					Assert.AreEqual("The Description", result.Description);
					Assert.AreEqual("test-cat", result.RouteName);
					Assert.AreEqual(0, result.ResourceItems.Count);

					Assert.AreEqual(1, resourceCategories.RecordCount);

					ResourceCategoryDataRow resourceCategoryDataRow = resourceCategories.Select(result.Id);
					Assert.IsNotNull(resourceCategoryDataRow);
					Assert.AreEqual(1, resourceCategoryDataRow.UserId);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddResourceCategory_ExternalUser_Success()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceCategory result = sut.AddResourceCategory(-9383747575, 0, "test cat", "The Description");

					Assert.IsNotNull(result);
					Assert.AreEqual("test cat", result.Name);
					Assert.AreEqual("The Description", result.Description);
					Assert.AreEqual("test-cat", result.RouteName);
					Assert.AreEqual(0, result.ResourceItems.Count);

					Assert.AreEqual(1, resourceCategories.RecordCount);

					ResourceCategoryDataRow resourceCategoryDataRow = resourceCategories.Select(result.Id);
					Assert.IsNotNull(resourceCategoryDataRow);
					Assert.AreEqual(0, resourceCategoryDataRow.UserId);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateResourceCategory_NullCategory_Throws_ArgumentNullException()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.UpdateResourceCategory(0, null);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateResourceCategory_CategoryNotFound_Throws_ArgumentOutOfRangeException()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.UpdateResourceCategory(0, new ResourceCategory(23, 0, "name", "desc", null, null, null, "name", true));
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateResourceCategory_InvalidUserId_NotDefaultValue_UserDoesNotExist_Throws_ArgumentNullException()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "description",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.UpdateResourceCategory(21, new ResourceCategory(0, 0, "name", "desc", null, null, null, "name", true));
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}


		[TestMethod]
		public void UpdateResourceCategory_CategoryUpdated_Success()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "description",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceCategory updateCategory = new ResourceCategory(0, 0, "new name", "desc", "black", "white", "image", "custom", true);
					ResourceCategory result = sut.UpdateResourceCategory(1, updateCategory);

					Assert.IsNotNull(result);
					Assert.AreNotSame(updateCategory, result);
					Assert.AreEqual("new name", result.Name);
					Assert.AreEqual("desc", result.Description);
					Assert.AreEqual("name", result.RouteName);
					Assert.AreEqual("white", result.BackColor);
					Assert.AreEqual("black", result.ForeColor);
					Assert.AreEqual("image", result.Image);

					ResourceCategoryDataRow resourceCategoryDataRow = resourceCategories.Select(result.Id);
					Assert.IsNotNull(resourceCategoryDataRow);
					Assert.AreEqual(1, resourceCategoryDataRow.UserId);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetAllResources_SubCategoriesNotReturned_Success()
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

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : 2,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					List<ResourceCategory> categories = sut.GetAllResources();

					Assert.AreEqual(5, categories.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void GetAllResources_SubCategoriesReturned_Success()
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

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 :  i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					List<ResourceCategory> categories = sut.GetAllResources();

					Assert.AreEqual(5, categories.Count);

					categories = sut.GetAllResources(2);
					Assert.IsNotNull(categories);
					Assert.AreEqual(3, categories.Count);

					categories = sut.GetAllResources(3);
					Assert.IsNotNull(categories);
					Assert.AreEqual(2, categories.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceItem_UserNameNull_Throw_ArgumentNullException()
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

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceItem(1, ResourceType.Image, 1, null, "Resource name", "Description", "Resource Value", false, new());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceItem_NameNull_Throw_ArgumentNullException()
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

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceItem(1, ResourceType.Image, 1, "user name", null, "Description", "Resource Value", false, new());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceItem_DescriptionEmptyString_Throw_ArgumentNullException()
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

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceItem(1, ResourceType.Image, 1, "user name", "Resource Name", "", "Resource Value", false, new());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceItem_ValueEmptyString_Throw_ArgumentNullException()
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

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceItem(1, ResourceType.Image, 1, "user name", "Resource Name", "Description", "", false, new());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddResourceItem_TagsNull_Throw_ArgumentNullException()
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

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceItem(1, ResourceType.Image, 1, "bob", "Resource name", "Description", "Resource Value", false, null);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(UniqueIndexException))]
		public void AddResourceItem_DuplicateCategoryAndName_Throws_UniqueIndexException()
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

					ISimpleDBOperations<ResourceItemDataRow> resourceItemsTable = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItemsTable);
					Assert.AreEqual(0, resourceItemsTable.RecordCount);

					ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService<ISimpleDBOperations<UserDataRow>>();
					Assert.IsNotNull(userTable);

					userTable.Insert(new UserDataRow()
					{
						FirstName = "Joe",
						Surname = "Bloggs"
					});

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					resourceItemsTable.Insert(new ResourceItemDataRow()
					{
						CategoryId = 1,
						UserId = 1,
						UserName = "user name",
						Name = "Resource Name",
						Description = "Description",
						Value = "some value"
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceItem(1, ResourceType.Image, 1, "user name", "Resource Name", "Description", "a value", false, new());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ForeignKeyException))]
		public void AddResourceItem_CategoryNotFound_Throws_ForeignKeyException()
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

					ISimpleDBOperations<ResourceItemDataRow> resourceItemsTable = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItemsTable);
					Assert.AreEqual(0, resourceItemsTable.RecordCount);

					ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService<ISimpleDBOperations<UserDataRow>>();
					Assert.IsNotNull(userTable);

					userTable.Insert(new UserDataRow()
					{
						FirstName = "Joe",
						Surname = "Bloggs"
					});

					resourceItemsTable.Insert(new ResourceItemDataRow()
					{
						CategoryId = 1,
						UserId = 1,
						UserName = "user name",
						Name = "Resource Name",
						Description = "Description",
						Value = "some value"
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.AddResourceItem(1, ResourceType.Image, 1, "user name", "Resource Name", "Description", "a value", false, new());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddResourceItem_ValidResourceItem_ReturnsResourceItemInstance()
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

					ISimpleDBOperations<ResourceItemDataRow> resourceItemsTable = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItemsTable);
					Assert.AreEqual(0, resourceItemsTable.RecordCount);

					ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService<ISimpleDBOperations<UserDataRow>>();
					Assert.IsNotNull(userTable);

					userTable.Insert(new UserDataRow()
					{
						FirstName = "Joe",
						Surname = "Bloggs"
					});

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem Result = sut.AddResourceItem(1, ResourceType.Image, 1, "user name", "Resource Name", "Description", "some value", false, new());
					Assert.IsNotNull(Result);
					Assert.AreEqual(1, resourceItemsTable.RecordCount);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddResourceItem_UserIsExternalUser_AddsRecord()
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

					ISimpleDBOperations<ResourceItemDataRow> resourceItemsTable = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItemsTable);
					Assert.AreEqual(0, resourceItemsTable.RecordCount);

					ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService<ISimpleDBOperations<UserDataRow>>();
					Assert.IsNotNull(userTable);

					userTable.Insert(new UserDataRow()
					{
						FirstName = "Joe",
						Surname = "Bloggs"
					});

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem Result = sut.AddResourceItem(1, ResourceType.Image, -83736363, "user name", "Resource Name", "Description", "some value", false, new());
					Assert.IsNotNull(Result);
					Assert.AreEqual(1, resourceItemsTable.RecordCount);

					ResourceItemDataRow newResourceItem = resourceItemsTable.Select().FirstOrDefault();
					Assert.IsNotNull(newResourceItem);
					Assert.AreEqual(0, newResourceItem.UserId);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void AddResourceItem_WithMultipleTags_ReturnsResourceItemInstance()
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

					ISimpleDBOperations<ResourceItemDataRow> resourceItemsTable = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItemsTable);
					Assert.AreEqual(0, resourceItemsTable.RecordCount);

					ISimpleDBOperations<UserDataRow> userTable = provider.GetRequiredService<ISimpleDBOperations<UserDataRow>>();
					Assert.IsNotNull(userTable);

					userTable.Insert(new UserDataRow()
					{
						FirstName = "Joe",
						Surname = "Bloggs"
					});

					ISimpleDBOperations<ResourceCategoryDataRow> resources = provider.GetRequiredService<ISimpleDBOperations<ResourceCategoryDataRow>>();
					Assert.IsNotNull(resources);

					for (int i = 0; i < 10; i++)
					{
						resources.Insert(new ResourceCategoryDataRow()
						{
							Name = $"Resource {i}",
							Description = $"test resource {i}",
							ForeColor = "black",
							BackColor = "white",
							ParentCategoryId = i < 5 ? 0 : i < 8 ? 2 : 3,
							IsVisible = true,
						});
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					List<string> tags = new()
					{
						"tag                         1",
						"ta          g2",
						"tag\t=__(*&^%$£\"!3",
						"t=ag 4"
					};

					ResourceItem Result = sut.AddResourceItem(1, ResourceType.Image, 1, "user name", "Resource Name", "Description", "some value", false, tags);
					Assert.IsNotNull(Result);
					Assert.AreEqual(1, resourceItemsTable.RecordCount);

					ResourceItemDataRow newResource = resourceItemsTable.Select(0);
					Assert.IsNotNull(newResource);
					Assert.AreEqual(4, newResource.Tags.Count);
					Assert.AreEqual("tag1", newResource.Tags[0]);
					Assert.AreEqual("tag2", newResource.Tags[1]);
					Assert.AreEqual("tag__3", newResource.Tags[2]);
					Assert.AreEqual("tag4", newResource.Tags[3]);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RetrieveAllResourceItems_Returns_ListWithAllResourceItems()
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

					ISimpleDBOperations<ResourceItemDataRow> resourceItemsTable = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
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
								resourceItemsTable.Insert(new ResourceItemDataRow()
								{
									CategoryId = i,
									Approved = i % 3 == 0,
									Description = $"Description of {i} {j}",
									Name = $"Name of {i} {j}",
									UserName = "admin",
								});
							}
						}
					}

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					List<ResourceItem> Result = sut.RetrieveAllResourceItems();
					Assert.IsNotNull(Result);
					Assert.AreEqual(10, resourceItemsTable.RecordCount);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateResourceItem_NullResourceItem_Throws_ArgumentNullException()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.UpdateResourceItem(0, null);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void UpdateResourceItem_CategoryNotFound_Throws_ArgumentOutOfRangeException()
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

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.UpdateResourceItem(0, new ResourceItem(23, 0, ResourceType.Uri, 1, "user", "name", "desc", "value", 1, 1, 1, true, new()));
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void UpdateResourceItem_InvalidUserId_NotDefaultValue_UserDoesNotExist_Throws_ArgumentNullException()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItems);

					resourceItems.Insert(new ResourceItemDataRow()
					{
						UserName = "user",
						Name = "name",
						Description = "description",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.UpdateResourceItem(21, new ResourceItem(0, 1, ResourceType.Uri, 1, "user", "name", "desc", "value", 1, 1, 1, true, new()));
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void UpdateResourceItem_InvalidCategoryId_CategoryDoesNotExist_Throws_ArgumentException()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItems);

					resourceItems.Insert(new ResourceItemDataRow()
					{
						UserName = "user",
						Name = "name",
						Description = "description",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					sut.UpdateResourceItem(1, new ResourceItem(0, 38, ResourceType.Uri, 1, "user", "name", "desc", "value", 1, 1, 1, true, new()));
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void UpdateResourceItem_ResourceUpdated_Success()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItems);

					resourceItems.Insert(new ResourceItemDataRow()
					{
						UserId = 1,
						UserName = "user",
						Name = "name",
						Description = "description",
						Likes = 5,
						Dislikes = 5,
						ViewCount = 20,
						Approved = false,
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem updateItem = new ResourceItem(0, 0, ResourceType.Uri, 1, "user name", "new name", "my description", "the value", 100, 100, 100, true, new());
					ResourceItem result = sut.UpdateResourceItem(2, updateItem);

					Assert.IsNotNull(result);
					Assert.AreNotSame(updateItem, result);
					Assert.AreEqual("new name", result.Name);
					Assert.AreEqual("my description", result.Description);
					Assert.AreEqual("user name", result.UserName);
					Assert.AreEqual("the value", result.Value);
					Assert.AreEqual(5, result.Likes);
					Assert.AreEqual(5, result.Dislikes);
					Assert.AreEqual(20, result.ViewCount);
					Assert.IsTrue(result.Approved);

				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ToggleResourceBookmark_UserNotFound_ReturnsUnknown()
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


					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem resourceItem = new ResourceItem(0, 0, ResourceType.Uri, 1, "user name", "new name", "my description", "the value", 100, 100, 100, true, new());
					BookmarkActionResult result = sut.ToggleResourceBookmark(2, resourceItem);

					Assert.AreEqual(BookmarkActionResult.Unknown, result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ToggleResourceBookmark_ResourceItemNull_ThrowsArgumentNullException()
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


					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					BookmarkActionResult result = sut.ToggleResourceBookmark(2, null);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ToggleResourceBookmark_ResourceItemNotFound_ReturnsUnknown()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ResourceItem resourceItem = new ResourceItem(-100, 0, ResourceType.Uri, 1, "user name", "new name", "my description", "the value", 100, 100, 100, true, new());
					BookmarkActionResult result = sut.ToggleResourceBookmark(1, resourceItem);

					Assert.AreEqual(BookmarkActionResult.Unknown, result);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ToggleResourceBookmark_ResourceItemFound_NoPreviousBookmark_ReturnsAdded()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItems);

					resourceItems.Insert(new ResourceItemDataRow()
					{
						UserId = 1,
						UserName = "user",
						Name = "name",
						Description = "description",
						Likes = 5,
						Dislikes = 5,
						ViewCount = 20,
						Approved = false,
					});

					ISimpleDBOperations<ResourceBookmarkDataRow> resourceBookmarks = provider.GetRequiredService<ISimpleDBOperations<ResourceBookmarkDataRow>>();
					Assert.IsNotNull(resourceBookmarks);
					Assert.AreEqual(0, resourceBookmarks.RecordCount);


					ResourceItem resourceItem = new ResourceItem(0, 0, ResourceType.Uri, 1, "user name", "new name", "my description", "the value", 100, 100, 100, true, new());
					BookmarkActionResult result = sut.ToggleResourceBookmark(1, resourceItem);

					Assert.AreEqual(BookmarkActionResult.Added, result);
					Assert.AreEqual(1, resourceBookmarks.RecordCount);
					Assert.IsNotNull(resourceBookmarks.Select(rb => rb.UserId.Equals(1) && rb.ResourceId.Equals(0)).FirstOrDefault());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ToggleResourceBookmark_ResourceItemFound_PreviouslyBookmarked_ReturnsRemoved()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItems);

					resourceItems.Insert(new ResourceItemDataRow()
					{
						UserId = 1,
						UserName = "user",
						Name = "name",
						Description = "description",
						Likes = 5,
						Dislikes = 5,
						ViewCount = 20,
						Approved = false,
					});



					ISimpleDBOperations<ResourceBookmarkDataRow> resourceBookmarks = provider.GetRequiredService<ISimpleDBOperations<ResourceBookmarkDataRow>>();
					Assert.IsNotNull(resourceBookmarks);
					Assert.AreEqual(0, resourceBookmarks.RecordCount);

					resourceBookmarks.Insert(new ResourceBookmarkDataRow()
					{
						ResourceId = 0,
						UserId = 1
					});

					Assert.AreEqual(1, resourceBookmarks.RecordCount);


					ResourceItem resourceItem = new ResourceItem(0, 0, ResourceType.Uri, 1, "user name", "new name", "my description", "the value", 100, 100, 100, true, new());
					BookmarkActionResult result = sut.ToggleResourceBookmark(1, resourceItem);

					Assert.AreEqual(BookmarkActionResult.Removed, result);
					Assert.AreEqual(0, resourceBookmarks.RecordCount);
					Assert.IsNull(resourceBookmarks.Select(rb => rb.UserId.Equals(1) && rb.ResourceId.Equals(0)).FirstOrDefault());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void ToggleResourceBookmark_UserExceedsBookMarkQuota_ReturnsQuotaExceeded()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItems);

					for (int i = 0; i < 50; i++)
					{
						resourceItems.Insert(new ResourceItemDataRow()
						{
							UserId = 1,
							UserName = "user",
							Name = $"name {i}",
							Description = "description",
							Likes = 5,
							Dislikes = 5,
							ViewCount = 20,
							Approved = false,
						});
					}


					ISimpleDBOperations<ResourceBookmarkDataRow> resourceBookmarks = provider.GetRequiredService<ISimpleDBOperations<ResourceBookmarkDataRow>>();
					Assert.IsNotNull(resourceBookmarks);
					Assert.AreEqual(0, resourceBookmarks.RecordCount);

					for (int i = 0; i < 30; i++)
					{
						resourceBookmarks.Insert(new ResourceBookmarkDataRow()
						{
							ResourceId = i,
							UserId = 1
						});
					}

					Assert.AreEqual(30, resourceBookmarks.RecordCount);


					ResourceItem resourceItem = new ResourceItem(35, 0, ResourceType.Uri, 1, "user name", "new name", "my description", "the value", 100, 100, 100, true, new());
					BookmarkActionResult result = sut.ToggleResourceBookmark(1, resourceItem);

					Assert.AreEqual(BookmarkActionResult.QuotaExceeded, result);
					Assert.AreEqual(30, resourceBookmarks.RecordCount);
					Assert.IsNull(resourceBookmarks.Select(rb => rb.UserId.Equals(1) && rb.ResourceId.Equals(35)).FirstOrDefault());
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RetrieveUserBookmarks_UserDoesNotExist_ReturnsEmptyList()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					List<ResourceItem> result = sut.RetrieveUserBookmarks(101);

					Assert.IsNotNull(result);
					Assert.AreEqual(0, result.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RetrieveUserBookmarks_UserHasNoBookmarks_ReturnsEmptyList()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					List<ResourceItem> result = sut.RetrieveUserBookmarks(1);

					Assert.IsNotNull(result);
					Assert.AreEqual(0, result.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}

		[TestMethod]
		public void RetrieveUserBookmarks_UserHasBookmarks_ReturnsBookmarkList()
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

					resourceCategories.Insert(new ResourceCategoryDataRow()
					{
						Name = "name",
						Description = "desc",
					});

					IResourceProvider sut = provider.GetRequiredService<IResourceProvider>();
					Assert.IsNotNull(sut);

					ISimpleDBOperations<ResourceItemDataRow> resourceItems = provider.GetRequiredService<ISimpleDBOperations<ResourceItemDataRow>>();
					Assert.IsNotNull(resourceItems);

					for (int i = 0; i < 50; i++)
					{
						resourceItems.Insert(new ResourceItemDataRow()
						{
							UserId = 1,
							UserName = "user",
							Name = $"name {i}",
							Description = "description",
							Likes = 5,
							Dislikes = 5,
							ViewCount = 20,
							Approved = false,
						});
					}


					ISimpleDBOperations<ResourceBookmarkDataRow> resourceBookmarks = provider.GetRequiredService<ISimpleDBOperations<ResourceBookmarkDataRow>>();
					Assert.IsNotNull(resourceBookmarks);
					Assert.AreEqual(0, resourceBookmarks.RecordCount);

					for (int i = 0; i < 27; i++)
					{
						resourceBookmarks.Insert(new ResourceBookmarkDataRow()
						{
							ResourceId = i,
							UserId = 1
						});
					}

					Assert.AreEqual(27, resourceBookmarks.RecordCount);

					List<ResourceItem> result = sut.RetrieveUserBookmarks(1);

					Assert.IsNotNull(result);
					Assert.AreEqual(27, result.Count);
				}
			}
			finally
			{
				Directory.Delete(directory, true);
			}
		}
	}
}

#pragma warning restore CA1826