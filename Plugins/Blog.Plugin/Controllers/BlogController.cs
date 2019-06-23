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
 *  Product:  Blog Plugin
 *  
 *  File: BlogController.cs
 *
 *  Purpose:  Blog controller
 *
 *  Date        Name                Reason
 *  21/06/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using Blog.Plugin.Models;

using Middleware;
using Middleware.Blog;

using SharedPluginFeatures;

using Shared.Classes;

#pragma warning disable CS1591

namespace Blog.Plugin.Controllers
{
    /// <summary>
    /// Controller for blogs.
    /// </summary>
    public class BlogController : BaseController
    {
        #region Private Members

        public const string Name = "Blog";

        private readonly IBlogProvider _blogProvider;

        #endregion Private Members

        #region Constructors

        public BlogController(IBlogProvider blogProvider)
        {
            _blogProvider = blogProvider ?? throw new ArgumentNullException(nameof(blogProvider));
        }

        #endregion Constructors

        #region Public Action Methods

        [Breadcrumb(nameof(Languages.LanguageStrings.Blog))]
        public IActionResult Index()
        {
            BlogPostsViewModel model = GetBlogPostViewModel(_blogProvider.GetRecentPosts(10, true));

            return View(model);
        }

        [Route("Blog/{user}/{id}/{date}/{title}")]
        [Breadcrumb(nameof(Languages.LanguageStrings.View), Name, nameof(Index), HasParams = true)]
        public IActionResult ViewBlog(int id)
        {
            BlogEntry blogEntry = _blogProvider.GetBlogEntry(id);

            if (blogEntry == null)
                return RedirectToAction(nameof(Index));

            BlogPostViewModel model = GetBlogPostViewModel(blogEntry);

            return View(model);
        }

        [HttpGet]
        [LoggedIn]
        [Breadcrumb(nameof(Languages.LanguageStrings.Edit), Name, nameof(Index), HasParams = true)]
        public IActionResult Edit(int id)
        {
            BlogEntry blog = _blogProvider.GetBlogEntry(id);

            if (blog == null)
                return RedirectToAction(nameof(Index));

            BlogPostViewModel model = GetEditBlogPostViewModel(blog);

            return View(model);
        }

        [HttpPost]
        [LoggedIn]
        public IActionResult Edit (BlogPostViewModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            BlogEntry blogEntry = _blogProvider.GetBlogEntry(model.Id);

            if (blogEntry == null)
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.InvalidBlog);

            if (ModelState.IsValid)
            {
                blogEntry.UpdateBlog(model.Title, model.Excerpt, model.BlogText, 
                    model.Published, model.PublishDateTime,
                    model.Tags.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList());

                _blogProvider.SaveBlogEntry(blogEntry);

                return Redirect(model.Url);
            }

            return View(model);
        }

        [Route("Blog/TagSearch/{tagName}")]
        [Breadcrumb(nameof(Languages.LanguageStrings.Search), Name, nameof(Index), HasParams = true)]
        public IActionResult TagSearch(string tagName)
        {
            if (String.IsNullOrEmpty(tagName))
                return RedirectToAction(nameof(Index));

            List<BlogEntry> blogs = _blogProvider.Search(tagName);

            if (blogs.Count == 0)
                ModelState.AddModelError(String.Empty, Languages.LanguageStrings.NoBlogsFoundMatchingTags);

            BlogPostsViewModel model = GetBlogPostViewModel(blogs);
            
            return View(nameof(Index), model);

        }

        #endregion Public Action Methods

        #region Private Methods

        private BlogPostViewModel GetEditBlogPostViewModel(in BlogEntry blogEntry)
        {
            BlogPostViewModel Result;

            Result = new BlogPostViewModel(GetModelData(), blogEntry.Id, blogEntry.Title, 
                blogEntry.Excerpt, blogEntry.BlogText, blogEntry.Username, blogEntry.Published, 
                blogEntry.PublishDateTime, blogEntry.LastModified, blogEntry.Tags);

            return Result;
        }

        private BlogPostViewModel GetBlogPostViewModel(in BlogEntry blogEntry)
        {
            UserSession user = GetUserSession();
            BlogPostViewModel Result = new BlogPostViewModel(GetModelData(), blogEntry.Id,
                blogEntry.Title, blogEntry.Excerpt, blogEntry.BlogText, blogEntry.Username, 
                blogEntry.Published, blogEntry.PublishDateTime, blogEntry.LastModified, 
                blogEntry.UserId == user.UserID, blogEntry.Tags);

            foreach (BlogComment comment in blogEntry.Comments)
            {
                Result.Comments.Add(new BlogCommentViewModel(comment.Id, comment.DateTime, comment.Username, comment.Comment));
            }


            Result.SeoAuthor = blogEntry.Username;
            Result.SeoTitle = blogEntry.Title;
            Result.SeoTags = String.Join(' ', blogEntry.Tags);
            Result.SeoDescription = blogEntry.Excerpt;

            return Result;
        }

        private BlogPostsViewModel GetBlogPostViewModel(List<BlogEntry> entries)
        {
            List<BlogPostViewModel> blogPosts = new List<BlogPostViewModel>();
            UserSession user = GetUserSession();

            foreach (BlogEntry entry in entries)
            {
                BlogPostViewModel post = new BlogPostViewModel(entry.Id, entry.Title, entry.Excerpt, 
                    entry.BlogText, entry.Username, entry.Published, entry.PublishDateTime, 
                    entry.LastModified, user.UserID == entry.UserId,
                    entry.Tags, new List<BlogCommentViewModel>());

                foreach (BlogComment comment in entry.Comments)
                {
                    post.Comments.Add(new BlogCommentViewModel(comment.Id, comment.DateTime, comment.Username, comment.Comment));
                }

                blogPosts.Add(post);
            }

            return new BlogPostsViewModel(GetModelData(), blogPosts);
        }

        #endregion Private Methods
    }
}

#pragma warning restore CS1591