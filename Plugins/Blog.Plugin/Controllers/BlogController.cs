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
using System.Threading.Tasks;
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
            BlogPostsViewModel model = GetBlogPostViewModel(_blogProvider.GetRecentPosts(10));

            return View(model);
        }

        #endregion Public Action Methods

        #region Private Methods

        private BlogPostsViewModel GetBlogPostViewModel(List<BlogEntry> entries)
        {
            List<BlogPostViewModel> blogPosts = new List<BlogPostViewModel>();
            UserSession user = GetUserSession();

            foreach (BlogEntry entry in entries)
            {
                BlogPostViewModel post = new BlogPostViewModel(entry.Id, entry.Title, entry.Excerpt, 
                    entry.BlogText, entry.Username, entry.Published, entry.PublishDateTime, 
                    user.UserID == entry.UserId,
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