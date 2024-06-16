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
 *  Product:  Blog Plugin
 *  
 *  File: BlogSitemapProvider.cs
 *
 *  Purpose:  Provides sitemap functionality for blogs
 *
 *  Date        Name                Reason
 *  26/07/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using Middleware;
using Middleware.Blog;

using SharedPluginFeatures;

namespace Blog.Plugin.Classes
{
	/// <summary>
	/// Blog sitemap provider, provides sitemap information for blog items
	/// </summary>
	public class BlogSitemapProvider : ISitemapProvider
	{
		#region Private Members

		private readonly IBlogProvider _blogProvider;

		#endregion Private Members

		#region Constructors

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="blogProvider">IBlogProvider instance</param>
		public BlogSitemapProvider(IBlogProvider blogProvider)
		{
			_blogProvider = blogProvider ?? throw new ArgumentNullException(nameof(blogProvider));
		}

		#endregion Constructors

		/// <summary>
		/// Retrieve a list of all blog items that will be included in the sitemap
		/// </summary>
		/// <returns>List&lt;ISitemapItem&gt;</returns>
		public List<SitemapItem> Items()
		{
			List<SitemapItem> Result = new();

			List<BlogItem> blogs = _blogProvider.GetRecentPosts(1000, true);

			foreach (BlogItem blogItem in blogs)
			{
				Uri blogUrl = new($"Blog/{HtmlHelper.RouteFriendlyName(blogItem.Username)}/{blogItem.Id}/" +
					$"{blogItem.LastModified.ToString("dd-MM-yyyy")}/{HtmlHelper.RouteFriendlyName(blogItem.Title)}",
					UriKind.RelativeOrAbsolute);

				Result.Add(new SitemapItem(blogUrl, SitemapChangeFrequency.Daily, blogItem.PublishDateTime));
			}

			return Result;
		}
	}
}
