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
 *  File: PluginInitialisationTests.cs
 *
 *  Purpose:  Tests for Image Manager Plugin Initialisation
 *
 *  Date        Name                Reason
 *  30/08/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using AspNetCore.PluginManager.Tests.Shared;

using MemoryCache.Plugin;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Shared.Classes;

namespace AspNetCore.PluginManager.Tests.Plugins.MemoryCacheTests
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class DefaultMemoryCacheTests
    {
        private const string TestsCategory = "MemoryCache General Tests";


        [TestMethod]
        [TestCategory(TestsCategory)]
        public void Construct_DefaultCachesAreCreated()
        {
            DefaultMemoryCache sut = new DefaultMemoryCache(new MockSettingsProvider(), true, DateTime.UtcNow.AddDays(10));

            Assert.IsNotNull(sut);

            Assert.IsNotNull(sut.GetCache());
            Assert.IsNotNull(sut.GetExtendingCache());
            Assert.IsNotNull(sut.GetPermanentCache());
            Assert.IsNotNull(sut.GetShortCache());
        }

        [TestMethod]
        [TestCategory(TestsCategory)]
        public void ResetCaches_EnsureCachedItemsAreCleared()
        {
            DefaultMemoryCache sut = new DefaultMemoryCache(new MockSettingsProvider(), true, DateTime.UtcNow.AddDays(10));

            Assert.IsNotNull(sut);

            Assert.IsNotNull(sut.GetCache());
            Assert.IsNotNull(sut.GetExtendingCache());
            Assert.IsNotNull(sut.GetPermanentCache());
            Assert.IsNotNull(sut.GetShortCache());

            sut.GetCache().Add("cache", new CacheItem("cache", true));
            sut.GetExtendingCache().Add("extending cache", new CacheItem("extending cache", true));
            sut.GetPermanentCache().Add("permanent cache", new CacheItem("permanent cache", true));
            sut.GetShortCache().Add("short cache", new CacheItem("short cache", true));

            Assert.IsNotNull(sut.GetCache().Get("cache"));
            Assert.IsNotNull(sut.GetExtendingCache().Get("extending cache"));
            Assert.IsNotNull(sut.GetPermanentCache().Get("permanent cache"));
            Assert.IsNotNull(sut.GetShortCache().Get("short cache"));

            sut.ResetCaches();

            Assert.IsNull(sut.GetCache().Get("cache"));
            Assert.IsNotNull(sut.GetExtendingCache().Get("extending cache"));
            Assert.IsNotNull(sut.GetPermanentCache().Get("permanent cache"));
            Assert.IsNull(sut.GetShortCache().Get("short cache"));
        }
    }
}
