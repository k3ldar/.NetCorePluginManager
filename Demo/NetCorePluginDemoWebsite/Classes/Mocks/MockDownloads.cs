﻿/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Website
 *  
 *  File: MockDownloads.cs
 *
 *  Purpose:  Mock IDownloads for tesing purpose
 *
 *  Date        Name                Reason
 *  05/01/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Middleware;
using Middleware.Downloads;

#pragma warning disable S2696

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
	[ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
	public class MockDownloads : IDownloadProvider
	{
		#region Private Static Members

		private static List<DownloadCategory> _userDownloads;

		private static List<DownloadCategory> _publicDownloads;

		#endregion Private Static Members

		#region IDownloads

		public List<DownloadCategory> DownloadCategoriesGet(in long userId)
		{
			_userDownloads ??=
				[
					new(1, "Brochures",
					[
						new(1, "Summer 2018", "Summer 2018 Catalogue", null, "\\Files\\Catalogues\\summer2018.pdf"),
						new(2, "Autumn 2018", "Autumn 2018 Catalogue", null, "\\Files\\Catalogues\\autumn2018.pdf"),
						new(3, "Winter 2018", "Winter 2018 Catalogue", null, "\\Files\\Catalogues\\winter2018.pdf"),
						new(4, "Spring 2019", "Spring 2019 Catalogue", null, "\\Files\\Catalogues\\spring2019.pdf"),
					]),
					new(2, "Applications",
					[
						new(5, "FB Stored Proc Generator", "Firebird stored procedure generator",
							"2.2.0", "\\Files\\Install\\fbspgen_v2.2_setup.exe"),
						new(6, "FB Task Scheduler", "Firebird task scheduler",
							"1.2", "\\Files\\Install\\fbtaskscheduler_1.2_setup.exe"),
					])
				];

			return _userDownloads;
		}

		public List<DownloadCategory> DownloadCategoriesGet()
		{
			_publicDownloads ??=
				[
					new(3, "Brochures",
					[
						new(7, "Summer 2018", "Summer 2018 Catalogue", null, "\\Files\\Catalogues\\summer2018.pdf"),
						new(8, "Autumn 2018", "Autumn 2018 Catalogue", null, "\\Files\\Catalogues\\autumn2018.pdf"),
						new(9, "Winter 2018", "Winter 2018 Catalogue", null, "\\Files\\Catalogues\\winter2018.pdf"),
						new(10, "Spring 2019", "Spring 2019 Catalogue", null, "\\Files\\Catalogues\\spring2019.pdf"),
					]),
					new(4, "Applications",
					[
						new(11, "FB Stored Proc Generator", "Firebird stored procedure generator",
							"2.2.0", "\\Files\\Install\\fbspgen_v2.2_setup.exe"),
						new(12, "FB Task Scheduler", "Firebird task scheduler",
							"1.2", "\\Files\\Install\\fbtaskscheduler_1.2_setup.exe"),
					])
				];

			return _publicDownloads;
		}

		public DownloadItem GetDownloadItem(in long fileId)
		{
			foreach (DownloadCategory category in DownloadCategoriesGet())
			{
				foreach (DownloadItem downloadItem in category.Downloads)
				{
					if (downloadItem.Id == fileId)
						return downloadItem;
				}
			}

			return null;
		}

		public DownloadItem GetDownloadItem(in long userId, in long fileId)
		{
			foreach (DownloadCategory category in DownloadCategoriesGet(userId))
			{
				foreach (DownloadItem downloadItem in category.Downloads)
				{
					if (downloadItem.Id == fileId)
						return downloadItem;
				}
			}

			return null;
		}

		public void ItemDownloaded(in long userId, in long fileId)
		{
			// its a mock do nothing
		}

		public void ItemDownloaded(in long fileId)
		{
			// its a mock do nothing
		}

		#endregion IDownloads
	}
}

#pragma warning restore S2696