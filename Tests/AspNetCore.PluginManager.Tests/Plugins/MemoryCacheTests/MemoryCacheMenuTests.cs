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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: DynamicContentMenuTests.cs
 *
 *  Purpose:  Tests for dynamic content system admin menu
 *
 *  Date        Name                Reason
 *  29/06/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using MemoryCache.Plugin;
using MemoryCache.Plugin.Classes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Abstractions;
using Shared.Classes;

using static SharedPluginFeatures.Enums;

namespace AspNetCore.PluginManager.Tests.Plugins.MemoryCacheTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DynamicContentMenuTests
    {
        private const string TestCategoryName = "Memory Cache Plugin";
        private const string SystemAdminCategoryName = "System Admin";

        [TestInitialize]
        public void InitializeMenuTests()
        {
            for (int i = CacheManager.GetCount() - 1; i >= 0; i--)
            {
                string cacheName = CacheManager.GetCacheName(i);
                Debug.Print($"Removing Cache: {cacheName}");
                CacheManager.RemoveCacheManager(cacheName);
            }

            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();
            cacheManagerFactory.ClearAllCaches();

            cacheManagerFactory.RemoveCache("DefaultImageProvider");

            DefaultMemoryCache defaultMemoryCache = new DefaultMemoryCache(new MockSettingsProvider(), true, DateTime.UtcNow.AddDays(10));
            CacheManager.RemoveCacheManager("Login Cache");
            CacheManager.RemoveCacheManager("Test Cache");
            CacheManager.RemoveCacheManager("Available Cultures");
            CacheManager.RemoveCacheManager("Dynamic Content Cache Manager");
            CacheManager.RemoveCacheManager("Image Manager Cache");
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void Validate_MenuSettings_Success()
        {
            MemoryCacheMenu sut = new MemoryCacheMenu();

            Assert.IsNotNull(sut);

            Assert.AreEqual("", sut.Action());
            Assert.AreEqual("", sut.Area());
            Assert.AreEqual("", sut.Controller());
            Assert.IsTrue(sut.Data().StartsWith("Name|Age|Item Count"));
            Assert.AreEqual("", sut.Image());
            Assert.AreEqual(SystemAdminMenuType.Grid, sut.MenuType());
            Assert.AreEqual("Memory Cache", sut.Name());
            Assert.AreEqual("System", sut.ParentMenuName());
            Assert.AreEqual(0, sut.SortOrder());
        }

        [TestMethod]
        [TestCategory(TestCategoryName)]
        [TestCategory(SystemAdminCategoryName)]
        public void Validate_MenuData_Success()
        {
            CacheManager testCache = new CacheManager("Test Cache", new TimeSpan());
            testCache.Add("test item", new CacheItem("test item", null));

            MemoryCacheMenu sut = new MemoryCacheMenu();

            Assert.IsNotNull(sut);

            string data = sut.Data();
            Assert.IsTrue(data.StartsWith("Name|Age|Item Count\r")); 
            Assert.IsTrue(data.Contains("Test Cache|02:00:00|1"));

            testCache.Clear();
            CacheManager.RemoveCacheManager("Test Cache");
        }
    }
}
