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
 *  File: BlogProviderTests.cs
 *
 *  Purpose:  Blog test for text based storage
 *
 *  Date        Name                Reason
 *  07/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Claims;
using System.Linq;

using AspNetCore.PluginManager.Tests.Shared;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Middleware;
using Middleware.Blog;

using PluginManager.Abstractions;
using PluginManager.DAL.TextFiles.Internal;
using PluginManager.DAL.TextFiles.Providers;
using PluginManager.DAL.TextFiles.Tables;

using SharedPluginFeatures;
using PluginManager.DAL.TextFiles.Tests.Mocks;
using Middleware.Accounts;
using Middleware.Accounts.Orders;

namespace PluginManager.DAL.TextFiles.Tests.Providers
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class BlogProviderTests : BaseProviderTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_ParamMemoryCacheNull_Throws_ArgumentNullException()
        {
            new BlogProvider(null, new MockTextTableOperations<TableUser>(), new MockTextTableOperations<TableBlog>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_ParamTableUserNull_Throws_ArgumentNullException()
        {
            new BlogProvider(new MockMemoryCache(), null, new MockTextTableOperations<TableBlog>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct_InvalidInstance_ParamTableBlogNull_Throws_ArgumentNullException()
        {
            new BlogProvider(new MockMemoryCache(), new MockTextTableOperations<TableUser>(), null);
        }

        [TestMethod]
        public void Construct_ValidInstance_Success()
        {
            BlogProvider sut = new BlogProvider(new MockMemoryCache(), new MockTextTableOperations<TableUser>(), new MockTextTableOperations<TableBlog>());

            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void GetRecentPosts_PublishedOnlyTrue_ReturnsMostRecentPosts_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user"});

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate,
                            LastModified = firstDate,
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                        });

                        firstDate.AddDays(1);
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    List<BlogItem> blogs = sut.GetRecentPosts(6, true);

                    Assert.AreEqual(5, blogs.Count);
                    Assert.AreEqual("Blog title 0", blogs[0].Title);
                    Assert.IsTrue(blogs[0].Published);
                    Assert.AreEqual("Blog title 2", blogs[1].Title);
                    Assert.IsTrue(blogs[1].Published);
                    Assert.AreEqual("Blog title 4", blogs[2].Title);
                    Assert.IsTrue(blogs[2].Published);
                    Assert.AreEqual("Blog title 6", blogs[3].Title);
                    Assert.IsTrue(blogs[3].Published);
                    Assert.AreEqual("Blog title 8", blogs[4].Title);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetRecentPosts_PublishedOnlyFalse_ReturnsMostRecentPosts_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate,
                            LastModified = firstDate,
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                        });

                        firstDate.AddDays(1);
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    List<BlogItem> blogs = sut.GetRecentPosts(6, false);

                    Assert.AreEqual(6, blogs.Count);
                    Assert.AreEqual("Blog title 0", blogs[0].Title);
                    Assert.IsTrue(blogs[0].Published);
                    Assert.AreEqual("Blog title 1", blogs[1].Title);
                    Assert.IsFalse(blogs[1].Published);
                    Assert.AreEqual("Blog title 2", blogs[2].Title);
                    Assert.IsTrue(blogs[2].Published);
                    Assert.AreEqual("Blog title 3", blogs[3].Title);
                    Assert.IsFalse(blogs[3].Published);
                    Assert.AreEqual("Blog title 4", blogs[4].Title);
                    Assert.IsTrue(blogs[4].Published);
                    Assert.AreEqual("Blog title 5", blogs[5].Title);
                    Assert.IsFalse(blogs[5].Published);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Search_FindsAllBlogsWithTag_test_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate,
                            LastModified = firstDate,
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                            {
                                "test",
                                $"{i % 2 == 0}"
                            }
                        });

                        firstDate.AddDays(1);
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    List<BlogItem> blogs = sut.Search("test");

                    Assert.AreEqual(10, blogs.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Search_FindsAllBlogsWithTag_true_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate,
                            LastModified = firstDate,
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                            {
                                "test",
                                $"{i % 2 == 0}"
                            }
                        });

                        firstDate.AddDays(1);
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    List<BlogItem> blogs = sut.Search("true");

                    Assert.AreEqual(5, blogs.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void Search_FindsAllBlogsWithTag_TrueOrFalse_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate,
                            LastModified = firstDate,
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                            {
                                "test",
                                $"{i % 2 == 0}"
                            }
                        });

                        firstDate.AddDays(1);
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    List<BlogItem> blogs = sut.Search("true fAlSe");

                    Assert.AreEqual(10, blogs.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetBlog_IdNotFound_ReturnsNull()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate,
                            LastModified = firstDate,
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                            {
                                "test",
                                $"{i % 2 == 0}"
                            }
                        });

                        firstDate.AddDays(1);
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    BlogItem blog = sut.GetBlog(-1);

                    Assert.IsNull(blog);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetBlog_IdFound_ReturnsBlogItem()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate,
                            LastModified = firstDate,
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                            {
                                "test",
                                $"{i % 2 == 0}"
                            }
                        });

                        firstDate.AddDays(1);
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    BlogItem blog = sut.GetBlog(3);

                    Assert.IsNotNull(blog);

                    Assert.AreEqual("Blog title 3", blog.Title);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void GetMyBlogs_ReturnsListInCorrectOrder_ReturnsBlogItem()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });
                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    DateTime firstDate = new DateTime(2022, 06, 07, 18, 11, 12);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = firstDate.AddDays(i),
                            LastModified = firstDate.AddDays(i),
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = i % 2 == 0 ? 1 : 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                            {
                                "test",
                                $"{i % 2 == 0}"
                            }
                        });
                    }

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    List<BlogItem> blogs = sut.GetMyBlogs(1);

                    Assert.IsNotNull(blogs);

                    Assert.AreEqual(5, blogs.Count);
                    Assert.IsTrue(blogs[1].Created.Ticks > blogs[0].Created.Ticks);
                    Assert.IsTrue(blogs[2].Created.Ticks > blogs[1].Created.Ticks);
                    Assert.IsTrue(blogs[3].Created.Ticks > blogs[2].Created.Ticks);
                    Assert.IsTrue(blogs[4].Created.Ticks > blogs[3].Created.Ticks);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SaveBlog_NewBlog_SequenceApplied_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });
                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    List<string> tags = new List<string>()
                    {
                        "new",
                        "blog"
                    };

                    BlogItem newBlog = new BlogItem(-1, 1, "My Blog", "My blog...", "Just a blog", "Blog writer", false, DateTime.Now, DateTime.Now, DateTime.Now.AddDays(-10), tags, new List<BlogComment>());
                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    BlogItem savedBlog = sut.SaveBlog(newBlog);

                    Assert.IsNotNull(savedBlog);

                    Assert.AreEqual(0, savedBlog.Id);
                    Assert.AreEqual(1, savedBlog.UserId);
                    Assert.AreEqual("My Blog", savedBlog.Title);
                    Assert.AreEqual("My blog...", savedBlog.Excerpt);
                    Assert.AreEqual("Just a blog", savedBlog.BlogText);
                    Assert.AreEqual("Blog writer", savedBlog.Username);
                    Assert.IsFalse(savedBlog.Published);
                    Assert.IsTrue(savedBlog.LastModified.Ticks > DateTime.Now.AddSeconds(-1).Ticks);
                    Assert.IsTrue(savedBlog.Created.Ticks > DateTime.Now.AddSeconds(-1).Ticks);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void SaveBlog_ExistingBlog_LastUpdatedSetCorrectly_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });
                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    for (int i = 0; i < 10; i++)
                    {
                        blogTable.Insert(new TableBlog()
                        {
                            Title = $"Blog title {i}",
                            BlogText = $"Blog {i}",
                            Created = DateTime.Now.AddDays(-1),
                            LastModified = DateTime.Now.AddDays(-1),
                            Published = i % 2 == 0,
                            Excerpt = $"Test blog {i}",
                            Username = "Test User",
                            UserId = 0,
                            Comments = new List<BlogComment>(),
                            Tags = new List<string>()
                            {
                                "test",
                                $"{i % 2 == 0}"
                            }
                        });
                    }

                    List<string> tags = new List<string>()
                    {
                        "new",
                        "blog"
                    };

                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);
                    BlogItem existingBlog = sut.GetBlog(5);

                    Assert.IsNotNull(existingBlog);

                    existingBlog.UpdateBlog("existing", existingBlog.Excerpt, existingBlog.BlogText, existingBlog.Published, existingBlog.PublishDateTime, existingBlog.Tags);

                    sut.SaveBlog(existingBlog);

                    Assert.AreEqual(5, existingBlog.Id);
                    Assert.AreEqual(0, existingBlog.UserId);
                    Assert.AreEqual("existing", existingBlog.Title);
                    Assert.AreEqual("Test blog 5", existingBlog.Excerpt);
                    Assert.AreEqual("Blog 5", existingBlog.BlogText);
                    Assert.AreEqual("Test User", existingBlog.Username);
                    Assert.IsFalse(existingBlog.Published);
                    Assert.IsTrue(existingBlog.LastModified.Ticks > DateTime.Now.AddSeconds(-1).Ticks);
                    Assert.IsTrue(existingBlog.Created.Ticks > DateTime.Now.AddDays(-1).AddSeconds(-1).Ticks);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void AddComment_NewCommentWithoutParent_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });
                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    List<string> tags = new List<string>()
                    {
                        "new",
                        "blog"
                    };

                    BlogItem newBlog = new BlogItem(-1, 1, "My Blog", "My blog...", "Just a blog", "Blog writer", false, DateTime.Now, DateTime.Now, DateTime.Now.AddDays(-10), tags, new List<BlogComment>());
                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    BlogItem savedBlog = sut.SaveBlog(newBlog);

                    Assert.IsNotNull(savedBlog);

                    Assert.AreEqual(0, savedBlog.Id);
                    Assert.AreEqual(1, savedBlog.UserId);
                    Assert.AreEqual("My Blog", savedBlog.Title);
                    Assert.AreEqual("My blog...", savedBlog.Excerpt);
                    Assert.AreEqual("Just a blog", savedBlog.BlogText);
                    Assert.AreEqual("Blog writer", savedBlog.Username);
                    Assert.IsFalse(savedBlog.Published);
                    Assert.IsTrue(savedBlog.LastModified.Ticks > DateTime.Now.AddSeconds(-1).Ticks);
                    Assert.IsTrue(savedBlog.Created.Ticks > DateTime.Now.AddSeconds(-1).Ticks);

                    sut.AddComment(savedBlog, null, 0, "first user", "just a comment");

                    savedBlog = sut.GetBlog(savedBlog.Id);

                    Assert.IsNotNull(savedBlog);
                    Assert.AreEqual(1, savedBlog.Comments.Count);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void AddComment_NewSubCommentWithoutParent_Success()
        {
            string directory = Path.Combine(Path.GetTempPath(), DateTime.Now.Ticks.ToString());
            try
            {
                Directory.CreateDirectory(directory);
                PluginInitialisation initialisation = new PluginInitialisation();
                ServiceCollection services = new ServiceCollection();

                services.AddSingleton<IMemoryCache, MockMemoryCache>();
                services.AddSingleton<IPluginClassesService, MockPluginClassesService>();
                services.AddSingleton<IForeignKeyManager, ForeignKeyManager>();
                services.AddSingleton<ISettingsProvider>(new MockSettingsProvider(TestPathSettings.Replace("$$", directory.Replace("\\", "\\\\"))));

                initialisation.BeforeConfigureServices(services);

                using (ServiceProvider provider = services.BuildServiceProvider())
                {
                    ITextTableOperations<TableUser> userTable = provider.GetRequiredService<ITextTableOperations<TableUser>>();
                    Assert.IsNotNull(userTable);

                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });
                    userTable.Insert(new TableUser() { FirstName = "test", Surname = "user" });

                    ITextTableOperations<TableBlog> blogTable = provider.GetRequiredService<ITextTableOperations<TableBlog>>();
                    Assert.IsNotNull(blogTable);

                    List<string> tags = new List<string>()
                    {
                        "new",
                        "blog"
                    };

                    BlogItem newBlog = new BlogItem(-1, 1, "My Blog", "My blog...", "Just a blog", "Blog writer", false, DateTime.Now, DateTime.Now, DateTime.Now.AddDays(-10), tags, new List<BlogComment>());
                    IBlogProvider sut = provider.GetRequiredService<IBlogProvider>();
                    Assert.IsNotNull(sut);

                    BlogItem savedBlog = sut.SaveBlog(newBlog);

                    Assert.IsNotNull(savedBlog);

                    Assert.AreEqual(0, savedBlog.Id);
                    Assert.AreEqual(1, savedBlog.UserId);
                    Assert.AreEqual("My Blog", savedBlog.Title);
                    Assert.AreEqual("My blog...", savedBlog.Excerpt);
                    Assert.AreEqual("Just a blog", savedBlog.BlogText);
                    Assert.AreEqual("Blog writer", savedBlog.Username);
                    Assert.IsFalse(savedBlog.Published);
                    Assert.IsTrue(savedBlog.LastModified.Ticks > DateTime.Now.AddSeconds(-1).Ticks);
                    Assert.IsTrue(savedBlog.Created.Ticks > DateTime.Now.AddSeconds(-1).Ticks);

                    sut.AddComment(savedBlog, null, 0, "first user", "just a comment");

                    savedBlog = sut.GetBlog(savedBlog.Id);

                    Assert.IsNotNull(savedBlog);
                    Assert.AreEqual(1, savedBlog.Comments.Count);

                    Assert.AreEqual("first user", savedBlog.Comments[0].Username);
                    Assert.AreEqual(0, savedBlog.Comments[0].UserId);
                    Assert.AreEqual("just a comment", savedBlog.Comments[0].Comment);
                    sut.AddComment(savedBlog, savedBlog.Comments[0], 0, "me", "another");

                    savedBlog = sut.GetBlog(savedBlog.Id);

                    Assert.IsNotNull(savedBlog);
                    Assert.AreEqual(1, savedBlog.Comments.Count);

                    Assert.AreEqual("first user", savedBlog.Comments[0].Username);
                    Assert.AreEqual(0, savedBlog.Comments[0].UserId);
                    Assert.AreEqual("just a comment", savedBlog.Comments[0].Comment);
                    Assert.AreEqual(1, savedBlog.Comments[0].Comments.Count);
                    Assert.AreEqual("me", savedBlog.Comments[0].Comments[0].Username);
                    Assert.AreEqual(0, savedBlog.Comments[0].Comments[0].UserId);
                    Assert.AreEqual("another", savedBlog.Comments[0].Comments[0].Comment);
                }
            }
            finally
            {
                Directory.Delete(directory, true);
            }
        }
    }
}
