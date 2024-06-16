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
 *  Product:  Demo Website Plugin
 *  
 *  File: DemoWebsiteSitemapProvider.cs
 *
 *  Purpose:  Provides sitemap functionality for Demo Website
 *
 *  Date        Name                Reason
 *  27/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using DemoWebsitePlugin.Controllers;

using SharedPluginFeatures;

namespace DemoWebsitePlugin.Classes
{
	/// <summary>
	/// Demo Website sitemap provider, provides sitemap information for the demo website items
	/// </summary>
	public class DemoRazorWebsiteSitemapProvider : ISitemapProvider
	{
		/// <summary>
		/// Retrieve a list of all demo website pages items that will be included in the sitemap
		/// </summary>
		/// <returns>List&lt;ISitemapItem&gt;</returns>
		public List<SitemapItem> Items()
		{
			List<SitemapItem> Result = new();

			Result.Add(new SitemapItem(
				new Uri($"{ServicesController.Name}/{nameof(ServicesController.Middleware)}", UriKind.RelativeOrAbsolute),
					SitemapChangeFrequency.Weekly));

			Result.Add(new SitemapItem(
				new Uri($"{ServicesController.Name}/{nameof(ServicesController.Api)}", UriKind.RelativeOrAbsolute),
					SitemapChangeFrequency.Weekly));

			Result.Add(new SitemapItem(
				new Uri($"{ServicesController.Name}/{nameof(ServicesController.DependencyInjection)}", UriKind.RelativeOrAbsolute),
					SitemapChangeFrequency.Weekly));

			Result.Add(new SitemapItem(
				new Uri($"{ServicesController.Name}/{nameof(ServicesController.Website)}", UriKind.RelativeOrAbsolute),
					SitemapChangeFrequency.Weekly));

			Result.Add(new SitemapItem(
				new Uri($"{ServicesController.Name}/{nameof(ServicesController.Custom)}", UriKind.RelativeOrAbsolute),
					SitemapChangeFrequency.Weekly));

			return Result;
		}
	}
}
