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
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: DownloadsProvider.cs
 *
 *  Purpose:  IDownloadProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using Middleware;
using Middleware.Downloads;

using PluginManager.DAL.TextFiles.Tables;
using SimpleDB;

using Shared.Classes;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal class DownloadProvider : IDownloadProvider
    {
        #region Private Members

        private readonly IMemoryCache _memoryCache;
        private readonly ISimpleDBOperations<DownloadItemsDataRow> _downloadItemData;
        private readonly ISimpleDBOperations<DownloadCategoryDataRow> _downloadCategoryData;

        #endregion Private Members

        #region Constructors

        public DownloadProvider(IMemoryCache memoryCache,
            ISimpleDBOperations<DownloadItemsDataRow> downloadItemData,
            ISimpleDBOperations<DownloadCategoryDataRow> downloadCategoryData)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _downloadItemData = downloadItemData ?? throw new ArgumentNullException(nameof(downloadItemData));
            _downloadCategoryData = downloadCategoryData ?? throw new ArgumentNullException(nameof(downloadCategoryData));
        }

        #endregion Constructors

        #region IDownloads

        public List<DownloadCategory> DownloadCategoriesGet(in long userId)
        {
            string cacheName = $"{nameof(DownloadCategoriesGet)} {userId}";

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheName);

            if (cacheItem == null)
            {
                List<DownloadCategory> result = new();

                long user = userId;

                IEnumerable<DownloadCategoryDataRow> categories = _downloadCategoryData.Select().Where(dc => dc.UserId.Equals(user) || dc.UserId.Equals(0));

                foreach (DownloadCategoryDataRow category in categories)
                {
                    List<DownloadItem> downloads = new();

                    IEnumerable<DownloadItemsDataRow> items = _downloadItemData.Select().Where(di => di.CategoryId.Equals(category.Id) && (di.UserId.Equals(0) || di.UserId.Equals(user)));

                    foreach (DownloadItemsDataRow download in items)
                    {
                        downloads.Add(new DownloadItem(download.Id, download.Name, download.Description, download.Version, download.Filename, download.Icon, download.Size));
                    }

                    result.Add(new DownloadCategory(category.Id, category.Name, downloads));
                }

                cacheItem = new CacheItem(cacheName, result);
                _memoryCache.GetExtendingCache().Add(cacheName, cacheItem);
            }

            return (List<DownloadCategory>)cacheItem.Value;
        }

        public List<DownloadCategory> DownloadCategoriesGet()
        {
            string cacheName = $"{nameof(DownloadCategoriesGet)} All Users";

            CacheItem cacheItem = _memoryCache.GetExtendingCache().Get(cacheName);

            if (cacheItem == null)
            {
                List<DownloadCategory> result = new();

                IEnumerable<DownloadCategoryDataRow> categories = _downloadCategoryData.Select().Where(dc => dc.UserId.Equals(0));

                foreach (DownloadCategoryDataRow category in categories)
                {
                    List<DownloadItem> downloads = new();

                    IEnumerable<DownloadItemsDataRow> items = _downloadItemData.Select().Where(di => di.CategoryId.Equals(category.Id) && di.UserId.Equals(0));

                    foreach (DownloadItemsDataRow download in items)
                    {
                        downloads.Add(new DownloadItem(download.Id, download.Name, download.Description, download.Version, download.Filename, download.Icon, download.Size));
                    }

                    result.Add(new DownloadCategory(category.Id, category.Name, downloads));
                }

                cacheItem = new CacheItem(cacheName, result);
                _memoryCache.GetExtendingCache().Add(cacheName, cacheItem);
            }

            return (List<DownloadCategory>)cacheItem.Value;
        }

        public DownloadItem GetDownloadItem(in long fileId)
        {
            DownloadItemsDataRow download = _downloadItemData.Select(fileId);

            if (download == null)
                return null;

            return new DownloadItem(download.Id, download.Name, download.Description, download.Version, download.Filename, download.Icon, download.Size);
        }

        public DownloadItem GetDownloadItem(in long userId, in long fileId)
        {
            DownloadItemsDataRow download = _downloadItemData.Select(fileId);

            if (download == null || (download.UserId > 0 && !download.UserId.Equals(userId)))
                return null;

            return new DownloadItem(download.Id, download.Name, download.Description, download.Version, download.Filename, download.Icon, download.Size);
        }

        public void ItemDownloaded(in long userId, in long fileId)
        {
            DownloadItemsDataRow download = _downloadItemData.Select(fileId);

            if (download == null || (download.UserId > 0 && !download.UserId.Equals(userId)))
                return;

            download.DownloadCount++;
            _downloadItemData.Update(download);
        }

        public void ItemDownloaded(in long fileId)
        {
            DownloadItemsDataRow download = _downloadItemData.Select(fileId);

            if (download == null)
                return;

            download.DownloadCount++;
            _downloadItemData.Update(download);
        }

        #endregion IDownloads
    }
}
