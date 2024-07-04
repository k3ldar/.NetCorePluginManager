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
 *  Product:  Demo Website
 *  
 *  File: MockBlogProvider.cs
 *
 *  Purpose:  Mock IBlogProvider for tesing purpose
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Middleware;
using Middleware.Blog;

namespace AspNetCore.PluginManager.DemoWebsite.Classes
{
	[ExcludeFromCodeCoverage(Justification = "Code coverage not required for mock classes")]
	public sealed class MockBlogProvider : IBlogProvider
	{
		#region Private Members

		private readonly List<BlogItem> _blogEntries;
		private int _blogCommentId = 3;

		#endregion Private Members

		#region Constructors

		public MockBlogProvider()
		{
			_blogEntries =
			[
				new(1, 123, "My First Blog Entry", "This is about my first blog entry", "Making blogs is easy", "Test User", true,
					DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-9),
					["Blogs", "First", "Test"],
					[
						new(1, null, DateTime.Now.AddDays(-8), 2, "A User", true, "This is the first comment"),
						new(2, null, DateTime.Now.AddDays(-7), 2, "Another User", true, "This is the second comment")
					]),
				new(2, 123, "Test", "Lorem Ipsum", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor " +
					"incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut " +
					"aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat " +
					"nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
					"Test User", true,
					DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-7), DateTime.Now.AddDays(-7),
					["Lorem", "Ipsum"],
					[
						new(3, null, DateTime.Now.AddDays(-7), 2, "A User", true, "honi soit qui mal y pense")
					])
		];
		}

		#endregion Constructors

		#region IBlogProvider Methods

		public List<BlogItem> GetRecentPosts(in int recentCount, in bool publishedOnly)
		{
			int count = recentCount;

			if (publishedOnly)
				return _blogEntries.Where(o => o.IsAvailable).OrderBy(o => o.PublishDateTime).Take(count).ToList();
			else
				return _blogEntries.OrderBy(o => o.PublishDateTime).Take(count).ToList();
		}

		public List<BlogItem> Search(in string tagName)
		{
			if (String.IsNullOrEmpty(tagName))
				throw new ArgumentNullException(nameof(tagName));

			string[] tags = tagName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			if (tags.Length == 0)
				throw new ArgumentNullException(nameof(tagName));

			return _blogEntries.Where(b => b.IsAvailable && b.Tags.Contains(tags[0], StringComparer.CurrentCultureIgnoreCase)).ToList();
		}

		public BlogItem GetBlog(in int id)
		{
			int blogId = id;

			return _blogEntries.Find(b => b.Id == blogId);
		}

		public List<BlogItem> GetMyBlogs(in long userId)
		{
			long user = userId;
			return _blogEntries.Where(b => b.UserId == user).OrderBy(o => o.Created).ToList();
		}

		public BlogItem SaveBlog(in BlogItem blogItem)
		{
			if (blogItem == null)
				throw new ArgumentNullException(nameof(blogItem));

			int newId;

			if (blogItem.Id == 0)
			{
				newId = _blogEntries.Count + 1;
				BlogItem newblog = new(newId, blogItem.UserId, blogItem.Title, blogItem.Excerpt,
					blogItem.BlogText, blogItem.Username, blogItem.Published, DateTime.Now,
					DateTime.Now, DateTime.Now, blogItem.Tags, blogItem.Comments);
				_blogEntries.Add(newblog);
				return newblog;
			}
			else
			{
				newId = blogItem.Id;
				_blogEntries.Remove(_blogEntries.Find(be => be.Id == newId));
				_blogEntries.Add(blogItem);
				return blogItem;
			}
		}

		public void AddComment(in BlogItem blogItem, in BlogComment parentComment, in long userId,
			in string userName, in string comment)
		{
			if (blogItem == null)
				throw new ArgumentNullException(nameof(blogItem));

			if (String.IsNullOrEmpty(comment))
				throw new ArgumentNullException(nameof(comment));

			BlogComment blogComment = new(++_blogCommentId, null,
				DateTime.Now, userId, userName, true, comment);
			blogItem.Comments.Add(blogComment);
		}

		#endregion IBlogProvider Methods
	}
}
