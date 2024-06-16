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
 *  Product:  Download Plugin
 *  
 *  File: DownloadController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  11/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using DownloadPlugin.Classes;
using DownloadPlugin.Models;

using Languages;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Downloads;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable CS1591, IDE0066

namespace DownloadPlugin.Controllers
{
	[Subdomain(DownloadController.Name)]
	public class DownloadController : BaseController
	{
		#region Private Members

		public const string Name = "Download";

		private static readonly CacheManager _downloadCache = new("Downloads", new TimeSpan(0, 60, 0));
		private readonly IWebHostEnvironment _hostingEnvironment;
		private readonly IDownloadProvider _downloadProvider;
		private readonly List<DownloadCategory> _categories;
		private readonly int _productsPerPage;

		#endregion Private Members

		#region Constructors

		public DownloadController(IDownloadProvider downloadProvider,
			IWebHostEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
			_downloadProvider = downloadProvider ?? throw new ArgumentNullException(nameof(downloadProvider));

			_categories = _downloadProvider.DownloadCategoriesGet();
			_productsPerPage = 12;
		}

		#endregion Constructors

		#region Public Action Methods

		public IActionResult Index()
		{
			if (_categories.Count == 0)
				Response.Redirect("/");

			return RedirectToAction(nameof(Category), new { id = _categories[0].Id, categoryName = _categories[0].Name });
		}

		[Route("/Download/{id}/Category/{categoryName}")]
		[Route("/Download/{id}/Category/{categoryName}/Page/{page}")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
		public IActionResult Category(int id, string categoryName, int? page)
		{
			if (_categories.Count == 0)
				return RedirectToAction(nameof(Index));

			DownloadCategory category = _categories.Find(c => c.Id == id);

			if (category == null)
				return RedirectToAction(nameof(Index));

			List<DownloadableItem> downloads = new();

			foreach (DownloadItem item in category.Downloads)
			{
				if (GetFileInformation(item.Filename, out string version, out string size))
				{
					if (!String.IsNullOrEmpty(version))
						version = $"v{version}";

					downloads.Add(new DownloadableItem(item.Id, item.Name, item.Description, version,
						item.Filename, size));
				}
			}

			List<CategoriesModel> categories = new();

			foreach (DownloadCategory item in _categories)
			{
				categories.Add(new CategoriesModel(item.Id, item.Name));
			}

			DownloadModel model = new(GetModelData(), category.Name, downloads, categories);
			model.Breadcrumbs.Add(new BreadcrumbItem(nameof(Languages.LanguageStrings.Download), "/Download/", false));
			model.Breadcrumbs.Add(new BreadcrumbItem(category.Name, $"/Download/{category.Id}/Category/{model.RouteText(category.Name)}", true));

			model.Pagination = BuildPagination(category.Downloads.Count, _productsPerPage,
				page ?? 1,
				$"/Download/{category.Id}/Category/{model.RouteText(category.Name)}/", "",
				LanguageStrings.Previous, LanguageStrings.Next);

			return View(model);
		}

		[Route("/Download/File/{id}/{name}")]
		public IActionResult File(int id, string name)
		{
			UserSession userSession = GetUserSession();

			if (userSession == null)
				_downloadProvider.ItemDownloaded(id);
			else
				_downloadProvider.ItemDownloaded(userSession.UserID, id);

			DownloadItem download = _downloadProvider.GetDownloadItem(id);

			if (download == null)
				return RedirectToAction(nameof(Index));

			string type = String.Empty;

			// set known types based on file extension  
			switch (Path.GetExtension(download.Filename).ToLower())
			{
				case ".htm":
				case ".html":
					type = "text/HTML";
					break;

				case ".txt":
					type = "text/plain";
					break;

				case ".doc":
				case ".rtf":
					type = "Application/msword";
					break;

				default:
					type = "application/octet-stream";
					break;
			}

			Response.Headers["content-disposition"] = "attachment; filename=" + name;

			Response.ContentType = type;
			string file = $"{_hostingEnvironment.ContentRootPath}{download.Filename}";
			byte[] fileBytes = System.IO.File.ReadAllBytes(file);
			return File(fileBytes, type, Path.GetFileName(file));
		}

		#endregion Public Action Methods

		#region Private Methods

		private bool GetFileInformation(in string fileName, out string version, out string size)
		{
			if (String.IsNullOrEmpty(fileName))
				throw new ArgumentNullException(nameof(fileName));

			version = String.Empty;
			size = String.Empty;

			string cacheName = $"FileInformation {fileName}";
			FileInformation fileInformation;
			CacheItem cacheItem = _downloadCache.Get(cacheName);

			if (cacheItem == null)
			{
				fileInformation = new FileInformation();

				string file = $"{_hostingEnvironment.ContentRootPath}{fileName}";

				if (!System.IO.File.Exists(file))
					return false;

				FileInfo fileInfo = new(file);
				fileInformation.Size = Shared.Utilities.FileSize(fileInfo.Length, 2);

				System.Diagnostics.FileVersionInfo versionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(file);

				fileInformation.Version = versionInfo == null || versionInfo.ProductVersion == null ?
					String.Empty : versionInfo.ProductVersion;

				cacheItem = new CacheItem(cacheName, fileInformation);
				_downloadCache.Add(cacheName, cacheItem);
			}

			fileInformation = (FileInformation)cacheItem.Value;
			version = fileInformation.Version ?? String.Empty;
			size = fileInformation.Size ?? String.Empty;

			return true;
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591, IDE0066
