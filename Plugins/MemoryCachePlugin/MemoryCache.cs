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
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  MemoryCachePlugin
 *  
 *  File: MemoryCache.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  10/10/2018  Simon Carter        Move thread initialisation to constructor, better 
 *                                  validation of short memory cache at start
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using Shared.Classes;

using SharedPluginFeatures;

namespace MemoryCache.Plugin
{
    internal class MemoryCache : BaseCoreClass, IMemoryCache
    {
        #region Private Members

        private static CacheManager _cacheShort;

        private static CacheManager _cache;

        #endregion Private Members

        #region Constructors

        public MemoryCache(ISettingsProvider settingsProvider)
        {
            ThreadManager.Initialise();

            MemoryClassPluginSettings settings = settingsProvider.GetSettings<MemoryClassPluginSettings>("MemoryCachePluginConfiguration");

            // create the caches
            if (_cache == null)
                _cache = new CacheManager("Website Internal Cache", 
                    new TimeSpan(0, settings.DefaultCacheDuration, 0));

            if (_cacheShort == null)
                _cacheShort = new CacheManager("Website Internal Short Cache", 
                    new TimeSpan(0, settings.ShortCacheDuration, 0));
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
    }
}
