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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  MemoryCachePlugin
 *  
 *  File: MemoryCache.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Shared.Classes;

using Microsoft.Extensions.Configuration;

using SharedPluginFeatures;

namespace MemoryCache.Plugin
{
    public class MemoryCache : IMemoryCache
    {
        #region Private Members

        private readonly CacheManager _cacheShort;

        private readonly CacheManager _cache;

        private readonly CacheManager _cacheSession;

        #endregion Private Members

        #region Constructors

        public MemoryCache()
        {
            MemoryClassPluginSettings settings = GetCacheSettings();

            // create the caches
            _cache = new CacheManager("Website Internal Cache", 
                new TimeSpan(0, settings.DefaultCacheDuration < 30 ? 120 : settings.DefaultCacheDuration, 0));

            _cacheShort = new CacheManager("Website Internal Short Cache", 
                new TimeSpan(0, settings.ShortCacheDuration == 0 ? 5 : settings.ShortCacheDuration, 0));

            _cacheSession = new CacheManager("Website Session Cache", 
                new TimeSpan(0, settings.SessionCacheDuration < 30 ? 30 :settings.SessionCacheDuration, 0), true, false);
        }

        #endregion Constructors

        #region Public Methods

        public CacheManager GetCache()
        {
            return (_cache);
        }

        public CacheManager GetShortCache()
        {
            return (_cacheShort);
        }

        public void ResetCaches()
        {
            _cacheShort.Clear();
            _cache.Clear();
        }

        #endregion Public Methods

        #region Private Methods

        private static MemoryClassPluginSettings GetCacheSettings()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            IConfigurationBuilder configBuilder = builder.SetBasePath(System.IO.Directory.GetCurrentDirectory());
            configBuilder.AddJsonFile("appsettings.json");
            IConfigurationRoot config = builder.Build();
            MemoryClassPluginSettings Result = new MemoryClassPluginSettings();
            config.GetSection("MemoryCachePluginConfiguration").Bind(Result);

            return (Result);
        }

        #endregion Private Methods
    }
}
