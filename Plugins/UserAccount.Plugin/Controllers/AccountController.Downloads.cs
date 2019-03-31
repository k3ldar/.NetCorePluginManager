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
 *  Product:  UserAccount.Plugin
 *  
 *  File: AccountController.Downloads.cs
 *
 *  Purpose:  Manages user downloads
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using UserAccount.Plugin.Models;

using SharedPluginFeatures;

using Middleware.Downloads;

namespace UserAccount.Plugin.Controllers
{
    public partial class AccountController
    {
        #region Public Controller Methods

        [Breadcrumb(nameof(Languages.LanguageStrings.Downloads), nameof(AccountController), nameof(Index))]
        public IActionResult Downloads(int id)
        {
            List<DownloadCategory> categories = _downloadProvider.DownloadCategoriesGet(UserId());
            DownloadCategory activeCategory = categories.Where(d => d.Id == id).FirstOrDefault();

            if (activeCategory == null)
            {
                activeCategory = categories[0];
            }

            List<ViewDownloadViewItem> downloads = new List<ViewDownloadViewItem>();

            foreach (DownloadItem item in activeCategory.Downloads)
            {
                downloads.Add(new ViewDownloadViewItem(GetBreadcrumbs(), GetCartSummary(),
                    item.Id, item.Name, item.Description,
                    item.Version, item.Filename, item.Icon, item.Size));
            }

            DownloadViewModel model = new DownloadViewModel(GetBreadcrumbs(), GetCartSummary(),
                categories, activeCategory.Name, downloads);

            model.Breadcrumbs = GetBreadcrumbs();
            model.CartSummary = GetCartSummary();

            return View(model);
        }

        [Breadcrumb(nameof(Languages.LanguageStrings.Download), nameof(AccountController), nameof(Downloads))]
        public IActionResult DownloadView(int id)
        {
            DownloadItem downloadItem = null;

            foreach (DownloadCategory category in _downloadProvider.DownloadCategoriesGet(UserId()))
            {
                foreach (DownloadItem item in category.Downloads)
                {
                    if (item.Id == id)
                    {
                        downloadItem = item;
                        break;
                    }
                }

                if (downloadItem != null)
                    break;
            }

            if (downloadItem == null)
                return RedirectToAction(nameof(Index));

            ViewDownloadViewItem model = new ViewDownloadViewItem(GetBreadcrumbs(), GetCartSummary(),
                downloadItem.Id, downloadItem.Name, downloadItem.Description, downloadItem.Version, 
                downloadItem.Filename, downloadItem.Icon, downloadItem.Size);

            return View(model);
        }

        [HttpPost]
        public FileStreamResult DownloadFile(ViewDownloadViewItem model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            string path = model.Filename;
            string name = Path.GetFileName(path);
            string ext = Path.GetExtension(path) ?? String.Empty;

            Response.Headers.Add("content-disposition", $"attachment; filename={name}");
            return File(new FileStream(path, FileMode.Open), GetContentType(ext));
        }

        #endregion Public Controller Methods

        #region Private Methods

        private string GetContentType(in string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".htm":
                case ".html":
                    return "text/HTML";

                case ".txt":
                    return "text/plain";

                case ".doc":
                case ".rtf":
                    return "Application/msword";

                case ".js":
                    return "text/javascript";

                case ".css":
                    return "text/css";

                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";

                case ".png":
                    return "image/png";

                case ".mpeg":
                    return "audio/mpeg";

                case ".mp4":
                    return "video/mp4";

                case ".json":
                    return "application/json";

                case ".pdf":
                    return "application/pdf";
            }

            return "application/octet-stream";
        }

        #endregion Private Methods
    }
}
