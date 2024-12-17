/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
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
 *  Product:  Blog Plugin
 *  
 *  File: Comment.cs
 *
 *  Purpose:  Blog post comment view model
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

using SharedPluginFeatures;

namespace Blog.Plugin.Models
{
	/// <summary>
	/// Container view model which can hold one or more blog posts.
	/// </summary>
	public class BlogPostsViewModel : BaseModel
	{
		#region Constructors

		/// <summary>
		/// Default constructor for multiple blog entries.
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="blogEntries"></param>
		public BlogPostsViewModel(in BaseModelData baseModelData, in List<BlogPostViewModel> blogEntries)
			: base(baseModelData)
		{
			BlogEntries = blogEntries ?? throw new ArgumentNullException(nameof(blogEntries));
		}

		/// <summary>
		/// Default constructor for a single blog entry.
		/// </summary>
		/// <param name="baseModelData"></param>
		/// <param name="blogItem"></param>
		public BlogPostsViewModel(in BaseModelData baseModelData, in BlogPostViewModel blogItem)
			: base(baseModelData)
		{
			if (blogItem == null)
				throw new ArgumentNullException(nameof(blogItem));

			BlogEntries = [blogItem];
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// List of blog entries within the view model
		/// </summary>
		/// <value>List&lt;BlogPostViewModel&gt;</value>
		public List<BlogPostViewModel> BlogEntries { get; private set; }

		#endregion Properties
	}
}
