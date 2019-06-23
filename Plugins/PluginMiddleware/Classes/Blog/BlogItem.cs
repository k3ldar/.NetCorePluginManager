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
 *  Product:  Plugin Middleware
 *  
 *  File: BlogItem.cs
 *
 *  Purpose:  Middleware object for a blog entry
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;

namespace Middleware.Blog
{
    /// <summary>
    /// Blog item represents a blog post within a website.
    /// </summary>
    public sealed class BlogItem
    {
        #region Constructors

        /// <summary>
        /// Default constructor for new blogs.
        /// </summary>
        public BlogItem()
        {
            Tags = new List<string>();
            Comments = new List<BlogComment>();
        }

        /// <summary>
        /// Constructor for creating a BlogItem instance
        /// </summary>
        /// <param name="id">Unique id representing the blog entry.</param>
        /// <param name="userId">Unique id of user creating the blog entry.</param>
        /// <param name="title">Title of blog entry.</param>
        /// <param name="excerpt">Short description of blog entry.</param>
        /// <param name="blogtext">Blog text.</param>
        /// <param name="username">Name of user publishing the blog entry</param>
        /// <param name="published">Indicates whether the blog has been published or not.</param>
        /// <param name="publishDateTime">Date and time the blog entry is available.</param>
        /// <param name="created">Date and time the blog entry was created.</param>
        /// <param name="lastModified">Date and time the blog entry was last modified.</param>
        /// <param name="tags">Tags associated with the blog entry.</param>
        /// <param name="comments">List of comments for the blog entry.</param>
        public BlogItem(in int id, in long userId, in string title, in string excerpt, 
            in string blogtext, in string username, in bool published, in DateTime publishDateTime, 
            in DateTime created, in DateTime lastModified, in List<string> tags,
            in List<BlogComment> comments)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            if (String.IsNullOrEmpty(excerpt))
                throw new ArgumentNullException(nameof(excerpt));

            if (String.IsNullOrEmpty(blogtext))
                throw new ArgumentNullException(nameof(blogtext));

            if (String.IsNullOrEmpty(username))
                throw new ArgumentNullException(nameof(username));

            if (comments == null)
                throw new ArgumentNullException(nameof(comments));

            Id = id;
            UserId = userId;
            Title = title;
            Excerpt = excerpt;
            BlogText = blogtext;
            Username = username;
            Published = published;
            PublishDateTime = publishDateTime;
            Created = created;
            LastModified = lastModified;
            Tags = tags ?? throw new ArgumentNullException(nameof(tags));
            Comments = comments ?? throw new ArgumentNullException(nameof(comments));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Unique id representing the blog entry.
        /// </summary>
        /// <value>int</value>
        public int Id { get; private set; }

        /// <summary>
        /// Unique id of user creating the blog entry.
        /// </summary>
        public long UserId { get; private set; }

        /// <summary>
        /// Title of blog entry.
        /// </summary>
        /// <value>string</value>
        public string Title { get; private set; }

        /// <summary>
        /// Brief description describing the blog entry.
        /// </summary>
        /// <value>string</value>
        public string Excerpt { get; private set; }

        /// <summary>
        /// The main blog text.
        /// </summary>
        /// <value>string</value>
        public string BlogText { get; private set; }

        /// <summary>
        /// Name of user creating the blog entry.
        /// </summary>
        /// <string>string</string>
        /// <value>string</value>
        public string Username { get; private set; }

        /// <summary>
        /// Indicates whether the blog entry has been published or not.
        /// </summary>
        /// <value>bool</value>
        public bool Published { get; private set; }

        /// <summary>
        /// The date/time the blog entry will appear live on the website.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime PublishDateTime { get; private set; }

        /// <summary>
        /// Date and time the blog was created.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Date and time the blog entry was last modified.
        /// </summary>
        /// <value>DateTime</value>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Determines whether the blog entry is visible on the website or not.
        /// </summary>
        /// <value>bool</value>
        public bool IsAvailable
        {
            get
            {
                return Published && PublishDateTime <= DateTime.Now;
            }
        }

        /// <summary>
        /// Descriptive tags for the blog.
        /// </summary>
        /// <value>List&lt;string&gt;</value>
        public List<string> Tags { get; private set; }

        /// <summary>
        /// List of comments for the blog entry.
        /// </summary>
        /// <value>List&lt;BlogComment&gt;</value>
        public List<BlogComment> Comments { get; private set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Updates the public properties for a blog entry.
        /// </summary>
        /// <param name="title">Title of blog.</param>
        /// <param name="excerpt">Excerpt for blog.</param>
        /// <param name="blogText">Blog text.</param>
        /// <param name="published">Indicates whether the blog is published.</param>
        /// <param name="publishDateTime">Date and time the blog can be published.</param>
        /// <param name="tags">Blog tags.</param>
        public void UpdateBlog(in string title, in string excerpt, in string blogText, 
            in bool published, in DateTime publishDateTime, in List<string> tags)
        {
            if (String.IsNullOrEmpty(title))
                throw new ArgumentNullException(nameof(title));

            if (String.IsNullOrEmpty(excerpt))
                throw new ArgumentNullException(nameof(excerpt));

            if (String.IsNullOrEmpty(blogText))
                throw new ArgumentNullException(nameof(blogText));

            if (tags == null)
                throw new ArgumentNullException(nameof(tags));

            if (tags.Count == 0)
                throw new ArgumentException(nameof(tags));

            LastModified = DateTime.Now;
            Title = title;
            Excerpt = excerpt;
            BlogText = blogText;
            Published = published;
            PublishDateTime = publishDateTime;
            Tags = tags;
        }

        #endregion Public Methods
    }
}
