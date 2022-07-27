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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: UserApiQueryProvider.cs
 *
 *  Purpose:  IUserApiQueryProvider for text based storage
 *
 *  Date        Name                Reason
 *  23/06/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;

using PluginManager.DAL.TextFiles.Tables;

using Shared.Classes;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class UserApiQueryProvider : IUserApiQueryProvider
    {
        private readonly ITextTableOperations<UserApiDataRow> _userApiDataRow;
        private readonly CacheManager _memoryCacheManager;

        public UserApiQueryProvider(ITextTableOperations<UserApiDataRow> userApiDataRow, IMemoryCache memoryCache)
        {
            _userApiDataRow = userApiDataRow ?? throw new ArgumentNullException(nameof(userApiDataRow));

            if (memoryCache == null)
                throw new ArgumentNullException(nameof(memoryCache));

            _memoryCacheManager = memoryCache.GetShortCache();
        }

        public bool ApiSecret(string merchantId, string apiKey, out string secret)
        {
            secret = String.Empty;

            if (String.IsNullOrEmpty(merchantId))
                return false;

            if (String.IsNullOrEmpty(apiKey))
                return false;

            string cacheName = $"api {merchantId} {apiKey}";

            CacheItem cacheItem = _memoryCacheManager.Get(cacheName);

            if (cacheItem == null)
            {
                UserApiDataRow row = _userApiDataRow.Select()
                    .Where(api => api.MerchantId.Equals(merchantId, StringComparison.InvariantCultureIgnoreCase) && 
                        api.ApiKey.Equals(apiKey, StringComparison.InvariantCulture))
                    .FirstOrDefault();

                if (row == null)
                    return false;

                cacheItem = new CacheItem(cacheName, row.Secret);
                _memoryCacheManager.Add(cacheName, cacheItem);
            }

            if (cacheItem == null)
                return false;

            secret = (string)cacheItem.Value;

            return true;
        }
    }
}
