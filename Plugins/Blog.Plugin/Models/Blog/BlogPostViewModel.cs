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
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using SharedPluginFeatures;

namespace Blog.Plugin.Models
{
	/// <summary>
	/// Blog entry represents a blog post within a website.
	/// </summary>
	public sealed class BlogPostViewModel : BaseModel
	{
		#region Constructors

		/// <summary>
		/// Constructor to be used when posting edit form.
		/// </summary>
		public BlogPostViewModel()
		{

		}

		/// <summary>
		/// Constructor for viewing multiple blog entries
		/// </summary>
		/// <param name="id">Unique id representing the blog entry.</param>
		/// <param name="title">Title of blog entry.</param>
		/// <param name="excerpt">Short description of blog entry.</param>
		/// <param name="blogtext">Blog text.</param>
		/// <param name="username">Name of user publishing the blog entry</param>
		/// <param name="published">Indicates whether the blog has been published or not.</param>
		/// <param name="publishDateTime">Date and time the blog entry is available.</param>
		/// <param name="lastModified">Date and time last modified.</param>
		/// <param name="canEdit">Determines whether the current user can edit the post or not.</param>
		/// <param name="tags">List of tags assigned to the post.</param>
		/// <param name="comments">List of comments for the blog entry.</param>
		/// <param name="isLoggedIn">Indicates whether the user is logged in or not.</param>
		/// <param name="allowComments">Determines whether comments are allowed or not.</param>
		public BlogPostViewModel(in int id, in string title, in string excerpt, in string blogtext,
			in string username, in bool published, in DateTime publishDateTime, in DateTime lastModified,
			in bool canEdit, in List<string> tags, in List<BlogCommentViewModel> comments,
			in bool isLoggedIn, in bool allowComments)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException(nameof(title));

			if (String.IsNullOrEmpty(excerpt))
				throw new ArgumentNullException(nameof(excerpt));

			if (String.IsNullOrEmpty(blogtext))
				throw new ArgumentNullException(nameof(blogtext));

			if (String.IsNullOrEmpty(username))
				throw new ArgumentNullException(nameof(username));

			if (tags == null)
				throw new ArgumentNullException(nameof(tags));

			Id = id;
			Title = title;
			Excerpt = excerpt;
			BlogText = blogtext;
			Username = username;
			Published = published;
			PublishDateTime = publishDateTime;
			LastModified = lastModified;
			CanEdit = canEdit;
			Tags = String.Join(' ', tags);
			Comments = comments ?? throw new ArgumentNullException(nameof(comments));
			IsLoggedIn = isLoggedIn;
			AllowComments = allowComments;
		}

		/// <summary>
		/// Constructor for viewing a single blog entry.
		/// </summary>
		/// <param name="baseModelData">Base model data.</param>
		/// <param name="id">Unique id representing the blog entry.</param>
		/// <param name="title">Title of blog entry.</param>
		/// <param name="excerpt">Short description of blog entry.</param>
		/// <param name="blogtext">Blog text.</param>
		/// <param name="username">Name of user publishing the blog entry</param>
		/// <param name="published">Indicates whether the blog has been published or not.</param>
		/// <param name="publishDateTime">Date and time the blog entry is available.</param>
		/// <param name="lastModified">Date and time last modified.</param>
		/// <param name="canEdit">Determines whether the current user can edit the post or not.</param>
		/// <param name="tags">List of tags assigned to the post.</param>
		/// <param name="isLoggedIn">Indicates whether the user is logged in or not.</param>
		/// <param name="allowComments">Determines whether comments are allowed or not.</param>
		public BlogPostViewModel(in IBaseModelData baseModelData, in int id, in string title, in string excerpt,
			in string blogtext, in string username, in bool published, in DateTime publishDateTime,
			in DateTime lastModified, in bool canEdit, in List<string> tags, in bool isLoggedIn,
			in bool allowComments)
			: base(baseModelData)
		{
			if (String.IsNullOrEmpty(title))
				throw new ArgumentNullException(nameof(title));

			if (String.IsNullOrEmpty(excerpt))
				throw new ArgumentNullException(nameof(excerpt));

			if (String.IsNullOrEmpty(blogtext))
				throw new ArgumentNullException(nameof(blogtext));

			if (String.IsNullOrEmpty(username))
				throw new ArgumentNullException(nameof(username));

			if (tags == null)
				throw new ArgumentNullException(nameof(tags));

			Id = id;
			Title = title;
			Excerpt = excerpt;
			BlogText = blogtext;
			Username = username;
			Published = published;
			PublishDateTime = publishDateTime;
			LastModified = lastModified;
			CanEdit = canEdit;
			Tags = String.Join(' ', tags);
			Comments = [];
			IsLoggedIn = isLoggedIn;
			AllowComments = allowComments;
		}

		/// <summary>
		/// Constructor for creating or editing a blog entry
		/// </summary>
		/// <param name="baseModelData">Basic view model data</param>
		/// <param name="id">Unique id representing the blog entry.</param>
		/// <param name="title">Title of blog entry.</param>
		/// <param name="excerpt">Short description of blog entry.</param>
		/// <param name="blogtext">Blog text.</param>
		/// <param name="username">Name of user publishing the blog entry</param>
		/// <param name="published">Indicates whether the blog has been published or not.</param>
		/// <param name="publishDateTime">Date and time the blog entry is available.</param>
		/// <param name="lastModified">Date and time last modified.</param>
		/// <param name="tags">List of tags assigned to the post.</param>
		public BlogPostViewModel(in IBaseModelData baseModelData, in int id, in string title, in string excerpt,
			in string blogtext, in string username, in bool published, in DateTime publishDateTime,
			in DateTime lastModified, in List<string> tags)
			: base(baseModelData)
		{
			if (tags == null)
				throw new ArgumentNullException(nameof(tags));

			Id = id;
			Title = title ?? String.Empty;
			Excerpt = excerpt ?? String.Empty;
			BlogText = blogtext ?? String.Empty;
			Username = username ?? String.Empty;
			Published = published;
			PublishDateTime = publishDateTime;
			LastModified = lastModified;
			Tags = String.Join(' ', tags);
		}

		#endregion Constructors

		#region Properties

		/// <summary>
		/// Unique id representing the blog entry.
		/// </summary>
		/// <value>int</value>
		[JsonRequired]
		public int Id { get; set; }

		/// <summary>
		/// Title of blog entry.
		/// </summary>
		/// <value>string</value>
		[Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterBlogTitle))]
		public string Title { get; set; }

		/// <summary>
		/// Brief description describing the blog entry.
		/// </summary>
		/// <value>string</value>
		[Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterExcerpt))]
		public string Excerpt { get; set; }

		/// <summary>
		/// The main blog text.
		/// </summary>
		/// <value>string</value>
		[Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterBlogText))]
		public string BlogText { get; set; }

		/// <summary>
		/// Name of user creating the blog entry.
		/// </summary>
		/// <string>string</string>
		/// <value>string</value>
		public string Username { get; set; }

		/// <summary>
		/// Indicates whether the blog entry has been published or not.
		/// </summary>
		/// <value>bool</value>
		public bool Published { get; set; }

		/// <summary>
		/// The date/time the blog entry will appear live on the website.
		/// </summary>
		/// <value>DateTime</value>
		public DateTime PublishDateTime { get; set; }

		/// <summary>
		/// Date and time the blog was last modified.
		/// </summary>
		public DateTime LastModified { get; private set; }

		/// <summary>
		/// Tags associated with the blog post.
		/// </summary>
		/// <value>string</value>
		[Required(ErrorMessage = nameof(Languages.LanguageStrings.PleaseEnterBlogTags))]
		public string Tags { get; set; }

		/// <summary>
		/// List of comments for the blog entry.
		/// </summary>
		/// <value>List&lt;Comment&gt;</value>
		public List<BlogCommentViewModel> Comments { get; private set; }

		/// <summary>
		/// Determines whether the current user can edit the post
		/// </summary>
		/// <value>bool</value>
		public bool CanEdit { get; private set; }

		/// <summary>
		/// Url for the blog.
		/// </summary>
		/// <value>string</value>
		public string Url
		{
			get
			{
				return $"/Blog/{RouteFriendlyName(Username)}/{Id}/{LastModified:dd-MM-yyyy}/{RouteFriendlyName(Title)}";
			}
		}

		/// <summary>
		/// Indicates whether the user is logged in or not
		/// </summary>
		/// <value>bool</value>
		public bool IsLoggedIn { get; set; }

		/// <summary>
		/// Determines whether users are allowed to add comments or not.
		/// </summary>
		/// <value>bool</value>
		public bool AllowComments { get; private set; }

		#endregion Properties
	}
}
