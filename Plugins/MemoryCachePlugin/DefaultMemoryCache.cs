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
 *  Product:  MemoryCachePlugin
 *  
 *  File: DefaultMemoryCache.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *  10/10/2018  Simon Carter        Move thread initialisation to constructor, better 
 *                                  validation of short memory cache at start
 *  02/06/2019  Simon Carter        Add extending and permanent cache managers.
 *  12/04/2020  Simon Carter        Renamed to DefaultMemoryCache
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

namespace MemoryCache.Plugin
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Used internally as part of IoC")]
    internal class DefaultMemoryCache : BaseCoreClass, IMemoryCache
    {
        #region Private Members

        private static CacheManager _cacheShort;

        private static CacheManager _cache;

        private static CacheManager _extendingCache;

        private static CacheManager _permanentCache;

        #endregion Private Members

        #region Constructors

        public DefaultMemoryCache(ISettingsProvider settingsProvider)
        {
			MemoryClassPluginSettings settings = settingsProvider.GetSettings<MemoryClassPluginSettings>("MemoryCachePluginConfiguration");

			// create the caches
			if (_cache == null)
				_cache = new CacheManager(Constants.CacheNameDefault,
					new TimeSpan(0, settings.DefaultCacheDuration, 0));

			if (_cacheShort == null)
				_cacheShort = new CacheManager(Constants.CacheNameShort,
					new TimeSpan(0, settings.ShortCacheDuration, 0));

			if (_extendingCache == null)
				_extendingCache = new CacheManager(Constants.CacheNameExtending,
					new TimeSpan(0, settings.DefaultCacheDuration, 0), true);

			if (_permanentCache == null)
				_permanentCache = new CacheManager(Constants.CacheNamePermanent,
					new TimeSpan(5000, 0, 0, 0), true);
		}

		/// <summary>
		/// Constructor used for internal unit testing only !!
		/// </summary>
		/// <param name="settingsProvider"></param>
		/// <param name="clearExisting"></param>
		/// <param name="clearDate"></param>
		internal DefaultMemoryCache(ISettingsProvider settingsProvider, bool clearExisting, DateTime clearDate)
		{
			if (clearExisting && DateTime.UtcNow.AddDays(10).Date.Equals(clearDate.Date))
			{
				if (_cache != null)
					CacheManager.RemoveCacheManager(_cache.Name);

				_cache = null;
				
				if (_cacheShort != null)
					CacheManager.RemoveCacheManager(_cacheShort.Name);
				_cacheShort = null;
				
				if (_extendingCache != null)
					CacheManager.RemoveCacheManager(_extendingCache.Name);
				_extendingCache = null;
				
				if (_permanentCache != null)
					CacheManager.RemoveCacheManager(_permanentCache.Name);

				_permanentCache = null;
			}

			MemoryClassPluginSettings settings = settingsProvider.GetSettings<MemoryClassPluginSettings>("MemoryCachePluginConfiguration");

            // create the caches
            if (_cache == null)
                _cache = new CacheManager(Constants.CacheNameDefault,
                    new TimeSpan(0, settings.DefaultCacheDuration, 0));

            if (_cacheShort == null)
                _cacheShort = new CacheManager(Constants.CacheNameShort,
                    new TimeSpan(0, settings.ShortCacheDuration, 0));

            if (_extendingCache == null)
                _extendingCache = new CacheManager(Constants.CacheNameExtending,
                    new TimeSpan(0, settings.DefaultCacheDuration, 0), true);

            if (_permanentCache == null)
                _permanentCache = new CacheManager(Constants.CacheNamePermanent,
                    new TimeSpan(5000, 0, 0, 0), true);
		}

        #endregion Constructors

        #region Public Methods

        public CacheManager GetCache()
        {
            return _cache;
        }

        public CacheManager GetShortCache()
        {
            return _cacheShort;
        }

        public CacheManager GetExtendingCache()
        {
            return _extendingCache;
        }

        public CacheManager GetPermanentCache()
        {
            return _permanentCache;
        }

        public void ResetCaches()
        {
            _cacheShort.Clear();
            _cache.Clear();
        }

        #endregion Public Methods
    }
}
