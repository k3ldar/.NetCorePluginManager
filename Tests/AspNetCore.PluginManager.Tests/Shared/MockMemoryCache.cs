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
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockMemoryCache.cs
 *
 *  Purpose:  Mock IMemoryCache class
 *
 *  Date        Name                Reason
 *  15/05/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;

using Shared.Abstractions;
using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public sealed class MockMemoryCache : IMemoryCache
    {
        #region Private Members

        private readonly ICacheManager _cacheShort;

        private readonly ICacheManager _cache;

        private readonly ICacheManager _extendingCache;

        private readonly ICacheManager _permanentCache;

        #endregion Private Members

        #region Constructors

        public MockMemoryCache()
        {


            _cache = CreateCache(Constants.CacheNameDefault);

            _cacheShort = CreateCache(Constants.CacheNameShort);

            _extendingCache = CreateCache(Constants.CacheNameExtending);

            _permanentCache = CreateCache(Constants.CacheNamePermanent);
        }

        #endregion Constructors

        #region Private Methods

        private ICacheManager CreateCache(string name)
        {
            ICacheManagerFactory cacheManagerFactory = new CacheManagerFactory();

            ICacheManager Result = cacheManagerFactory.GetCacheIfExists(name);

            if (Result == null)
            {
                Result = cacheManagerFactory.CreateCache(name, new TimeSpan(0, 100, 0));
            }

            Result.Clear();

            return Result;
        }

        #endregion Private Methods

        #region Public Methods

        public ICacheManager GetCache()
        {
            return _cache;
        }

        public ICacheManager GetShortCache()
        {
            return _cacheShort;
        }

        public ICacheManager GetExtendingCache()
        {
            return _extendingCache;
        }

        public ICacheManager GetPermanentCache()
        {
            return _permanentCache;
        }

        public void ResetCaches()
        {

        }

        #endregion Public Methods
    }
}
