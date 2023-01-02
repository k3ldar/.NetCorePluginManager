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
 *  Product:  SharedPluginFeatures
 *  
 *  File: IMemoryCache.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  29/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Shared.Classes;


namespace SharedPluginFeatures
{
    /// <summary>
    /// MemoryCachePlugin implements an instance of IMemoryCache, and add the instance to the DI container.  
    /// Plugins and other modules can obtain an instance of IMemoryCache which can be used to store and 
    /// retrieve items easily from a designated cache.
    /// </summary>
    public interface IMemoryCache
    {
        #region Public Methods

        /// <summary>
        /// The short cache is inteded to hold items for upto 5 minutes, after which items will expire.
        /// 
        /// This can be useful for storing short term items like price information etc, that could
        /// change regularly.
        /// 
        /// There is no limit to the items that can be cached and cache retrieval is extremely quick.
        /// </summary>
        /// <returns>CacheManager instance</returns>
        CacheManager GetShortCache();

        /// <summary>
        /// The long cache is inteded to hold items for upto 2 hours, after which items will expire.
        /// 
        /// This can be useful for storing medium term items like product information etc, that doesn't
        /// change too regularly but will help speed up requests when retrieving.
        /// 
        /// There is no limit to the items that can be cached and cache retrieval is extremely quick.
        /// </summary>
        /// <returns>CacheManager instance</returns>
        CacheManager GetCache();

        /// <summary>
        /// The extending cache is intended to cache items for 2 hours, after which items will expire.
        /// 
        /// The difference between the extending cache and normal cache is that if an item is requested
        /// the time until expire will be extended by a further 2 hours.
        /// 
        /// There is no limit to the items that can be cached and cache retrieval is extremely quick.
        /// </summary>
        /// <returns>CacheManager instance</returns>
        CacheManager GetExtendingCache();

        /// <summary>
        /// The permanent cache holds items in memory indefinitely.
        /// 
        /// There is no limit to the items that can be cached and cache retrieval is extremely quick.
        /// </summary>
        /// <returns>CacheManager instance</returns>
        CacheManager GetPermanentCache();

        /// <summary>
        /// Resets all caches, clears all items.
        /// </summary>
        void ResetCaches();

        #endregion Public Methods
    }
}
