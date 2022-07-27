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
 *  Copyright (c) 2012 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginMiddleware
 *  
 *  File: IDownloadProvider.cs
 *
 *  Purpose:  Download provider
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;

using Middleware.Downloads;

namespace Middleware
{
    /// <summary>
    /// Download provider.  Provides download information used by the DownloadPlugin module.
    /// 
    /// This item must be implemented by the host application and made available via DI.
    /// </summary>
    public interface IDownloadProvider
    {
        #region Downloads

        /// <summary>
        /// User download file Categories
        /// </summary>
        /// <param name="userId">Id of the user requesting a download, if they are logged in.</param>
        /// <returns>List&lt;DownloadCategory&gt;</returns>
        List<DownloadCategory> DownloadCategoriesGet(in long userId);

        /// <summary>
        /// Publicy downloadable file Categories
        /// </summary>
        /// <returns>List&lt;DownloadCategory&gt;</returns>
        List<DownloadCategory> DownloadCategoriesGet();

        /// <summary>
        /// Retrieve File
        /// </summary>
        /// <param name="fileId">Unique id of the file being downloaded.</param>
        /// <returns>DownloadItem</returns>
        DownloadItem GetDownloadItem(in long fileId);

        /// <summary>
        /// Retrieve File
        /// </summary>
        /// <param name="userId">Id of the user requesting a download, if they are logged in.</param>
        /// <param name="fileId">Unique id of the file being downloaded.</param>
        /// <returns>DownloadItem</returns>
        DownloadItem GetDownloadItem(in long userId, in long fileId);

        /// <summary>
        /// File download by a user
        /// </summary>
        /// <param name="userId">Id of the user requesting a download, if they are logged in.</param>
        /// <param name="fileId">Unique id of the file being downloaded.</param>
        void ItemDownloaded(in long userId, in long fileId);

        /// <summary>
        /// File download by anyone
        /// </summary>
        /// <param name="fileId">Unique id of the file being downloaded.</param>
        void ItemDownloaded(in long fileId);

        #endregion Downloads
    }
}
