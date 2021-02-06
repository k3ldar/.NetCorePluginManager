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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Download Plugin
 *  
 *  File: DownloadSitemapProvider.cs
 *
 *  Purpose:  Provides sitemap functionality for downloads
 *
 *  Date        Name                Reason
 *  27/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Downloads;

using SharedPluginFeatures;

namespace DownloadPlugin.Classes
{
    /// <summary>
    /// Download sitemap provider, provides sitemap information for downloadable items
    /// </summary>
    public class DownloadSitemapProvider : ISitemapProvider
    {
        #region Private Members

        private readonly IDownloadProvider _downloadProvider;

        #endregion Private Members

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="downloadProvider">IDownloadProvider instance</param>
        public DownloadSitemapProvider(IDownloadProvider downloadProvider)
        {
            _downloadProvider = downloadProvider ?? throw new ArgumentNullException(nameof(downloadProvider));
        }

        #endregion Constructors

        /// <summary>
        /// Retrieve a list of all download items that will be included in the sitemap
        /// </summary>
        /// <returns>List&lt;ISitemapItem&gt;</returns>
        public List<SitemapItem> Items()
        {
            List<SitemapItem> Result = new List<SitemapItem>();

            foreach (DownloadCategory download in _downloadProvider.DownloadCategoriesGet())
            {
                Result.Add(new SitemapItem(
                    new Uri($"Download/{download.Id}/Category/{HtmlHelper.RouteFriendlyName(download.Name)}",
                        UriKind.RelativeOrAbsolute),
                    SitemapChangeFrequency.Monthly));
            }

            return Result;
        }
    }
}
