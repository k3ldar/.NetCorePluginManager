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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  PluginManager.DAL.TextFiles
 *  
 *  File: BlogProvider.cs
 *
 *  Purpose:  IBlogProvider for text based storage
 *
 *  Date        Name                Reason
 *  25/05/2022  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Middleware;
using Middleware.Blog;

using PluginManager.DAL.TextFiles.Tables;

using SharedPluginFeatures;

namespace PluginManager.DAL.TextFiles.Providers
{
    internal sealed class BlogProvider : IBlogProvider
    {
        #region Private Members

        private readonly ITextTableOperations<UserDataRow> _users;
        private readonly ITextTableOperations<BlogDataRow> _blogs;
        private readonly IMemoryCache _memoryCache;

        #endregion Private Members

        #region Constructors

        public BlogProvider(IMemoryCache memoryCache, ITextTableOperations<UserDataRow> users,
            ITextTableOperations<BlogDataRow> blogs)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _blogs = blogs ?? throw new ArgumentNullException(nameof(blogs));
        }

        #endregion Constructors

        #region IBlogProvider Methods

        public List<BlogItem> GetRecentPosts(in int recentCount, in bool publishedOnly)
        {
            List<BlogDataRow> blogs;
            
            if (publishedOnly)
                blogs = _blogs.Select().Where(b => b.Published).OrderByDescending(b => b.PublishDateTime).Take(recentCount).ToList();
            else
                blogs = _blogs.Select().OrderByDescending(b => b.PublishDateTime).Take(recentCount).ToList();

            

            return ConvertTableBlogToBlogItem(blogs);
        }

        public List<BlogItem> Search(in string tagName)
        {
            if (String.IsNullOrEmpty(tagName))
                throw new ArgumentNullException(nameof(tagName));

            string[] tags = tagName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tags.Length == 0)
                throw new ArgumentNullException(nameof(tagName));

            return ConvertTableBlogToBlogItem(_blogs.Select().Where(b => BlogHasTag(b, tags)).ToList());
        }

        public BlogItem GetBlog(in int id)
        {
            int blogId = id;

            return ConvertTableBlogToBlogItem(_blogs.Select(blogId));
        }

        public List<BlogItem> GetMyBlogs(in long userId)
        {
            long user = userId;
            return ConvertTableBlogToBlogItem(_blogs.Select().Where(b => b.UserId == user).OrderBy(o => o.Created).ToList());
        }

        public BlogItem SaveBlog(in BlogItem blogItem)
        {
            if (blogItem == null)
                throw new ArgumentNullException(nameof(blogItem));

            BlogDataRow tableBlog = ConvertBlogItemToTableBlog(blogItem);

            tableBlog.LastModified = DateTime.Now;
            _blogs.InsertOrUpdate(tableBlog);

            return ConvertTableBlogToBlogItem(_blogs.Select(tableBlog.Id));
        }

        public void AddComment(in BlogItem blogItem, in BlogComment parentComment, in long userId,
            in string userName, in string comment)
        {
            if (blogItem == null)
                throw new ArgumentNullException(nameof(blogItem));

            if (String.IsNullOrEmpty(comment))
                throw new ArgumentNullException(nameof(comment));

            TimeSpan span = DateTime.Now - new DateTime(2022, 1, 1);
            BlogComment blogComment = new BlogComment(Convert.ToInt32(span.TotalSeconds), parentComment?.Id, DateTime.Now, userId, userName, true, comment);

            if (parentComment == null)
                blogItem.Comments.Add(blogComment);
            else
                parentComment.Comments.Add(blogComment);

            BlogDataRow tableBlog = ConvertBlogItemToTableBlog(blogItem);
            tableBlog.LastModified = DateTime.Now;
            _blogs.Update(tableBlog);
        }

        #endregion IBlogProvider Methods

        #region Private Methods

        private bool BlogHasTag(BlogDataRow tableBlog, string[] tags)
        {
            foreach (string searchTag in tags)
            {
                foreach (string tag in tableBlog.Tags)
                {
                    if (tag.Equals(searchTag, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private List<BlogItem> ConvertTableBlogToBlogItem(List<BlogDataRow> tableBlogs)
        {
            List<BlogItem> Result = new List<BlogItem>();

            tableBlogs.ForEach(blog => Result.Add(new BlogItem(Convert.ToInt32(blog.Id), blog.UserId, blog.Title, blog.Excerpt, blog.BlogText, blog.Username, blog.Published,
                    blog.PublishDateTime, blog.Created, blog.LastModified, blog.Tags, blog.Comments)));

            return Result;
        }

        private BlogItem ConvertTableBlogToBlogItem(BlogDataRow blog)
        {
            if (blog == null)
                return null;

            return new BlogItem(Convert.ToInt32(blog.Id), blog.UserId, blog.Title, blog.Excerpt, blog.BlogText, blog.Username, blog.Published,
                    blog.PublishDateTime, blog.Created, blog.LastModified, blog.Tags, blog.Comments);
        }

        private BlogDataRow ConvertBlogItemToTableBlog(BlogItem blog)
        {
            if (blog == null)
                return null;

            return new BlogDataRow()
            {
                Id = Convert.ToInt32(blog.Id), 
                UserId = blog.UserId, 
                Title = blog.Title, 
                Excerpt = blog.Excerpt, 
                BlogText = blog.BlogText, 
                Username = blog.Username, 
                Published = blog.Published,
                PublishDateTime = blog.PublishDateTime, 
                Created = blog.Created,
                LastModified = blog.LastModified, 
                Tags = blog.Tags, 
                Comments = blog.Comments
            };
        }

        #endregion Private Methods
    }
}
