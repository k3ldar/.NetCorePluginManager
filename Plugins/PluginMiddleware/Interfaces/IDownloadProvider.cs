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
 *  Copyright (c) 2012 - 2018 Simon Carter.  All Rights Reserved.
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
using System;
using System.Collections.Generic;

using Middleware.Downloads;

namespace Middleware
{
    public interface IDownloadProvider
    {

        #region Downloads

        /// <summary>
        /// User download file Categories
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<DownloadCategory> DownloadCategoriesGet(in Int64 userId);

        /// <summary>
        /// Publicy downloadable file Categories
        /// </summary>
        /// <returns></returns>
        List<DownloadCategory> DownloadCategoriesGet();

        /// <summary>
        /// Retrieve File
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        DownloadItem GetDownloadItem(in int fileId);

        /// <summary>
        /// Retrieve File
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        DownloadItem GetDownloadItem(in Int64 userId, in int fileId);

        /// <summary>
        /// File download by a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileId"></param>
        void ItemDownloaded(in Int64 userId, in int fileId);

        /// <summary>
        /// File download by anyone
        /// </summary>
        /// <param name="fileId"></param>
        void ItemDownloaded(in int fileId);

        #endregion Downloads
    }
}
