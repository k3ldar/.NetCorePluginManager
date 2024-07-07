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
using System.Net;

using Blog.Plugin.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Middleware;
using Middleware.Blog;

using PluginManager.Abstractions;

using Shared.Classes;

using SharedPluginFeatures;

using static SharedPluginFeatures.HtmlHelper;

#pragma warning disable CS1591

namespace Blog.Plugin.Controllers
{
	/// <summary>
	/// Controller for blogs.
	/// </summary>
	[Subdomain(BlogController.Name)]
	public class BlogController : BaseController
	{
		#region Private Members

		public const string Name = "Blog";

		private readonly IBlogProvider _blogProvider;
		private readonly BlogSettings _settings;

		#endregion Private Members

		#region Constructors

		public BlogController(IBlogProvider blogProvider, ISettingsProvider settingsProvider)
		{
			_blogProvider = blogProvider ?? throw new ArgumentNullException(nameof(blogProvider));

			if (settingsProvider == null)
				throw new ArgumentNullException(nameof(settingsProvider));

			_settings = settingsProvider.GetSettings<BlogSettings>(nameof(BlogSettings));
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
			BlogItem blogItem = _blogProvider.GetBlog(id);

			if (blogItem == null)
				return RedirectToAction(nameof(Index));

			BlogPostViewModel model = GetBlogPostViewModel(blogItem);
			model.Breadcrumbs.Add(new BreadcrumbItem(Languages.LanguageStrings.Blog, $"/{Name}/{nameof(Index)}", false));
			model.Breadcrumbs.Add(new BreadcrumbItem(blogItem.Title, model.Url, false));

			return View(model);
		}

		[HttpGet]
		[LoggedIn]
		[Breadcrumb(nameof(Languages.LanguageStrings.MyBlogs), Name, nameof(Index))]
		public IActionResult MyBlogs()
		{
			return View(GetMyBlogsViewModel(_blogProvider.GetMyBlogs(UserId())));
		}

		[HttpGet]
		[LoggedIn]
		[Breadcrumb(nameof(Languages.LanguageStrings.New), Name, nameof(Index))]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameBlogCreate)]
		public IActionResult New()
		{
			return View(nameof(Edit), GetEditBlogPostViewModel(new BlogItem()));
		}

		[HttpGet]
		[LoggedIn]
		[Breadcrumb(nameof(Languages.LanguageStrings.Edit), Name, nameof(Index), HasParams = true)]
		public IActionResult Edit(int id)
		{
			if (id == 0)
				return View(GetEditBlogPostViewModel(new BlogItem()));

			BlogItem blog = _blogProvider.GetBlog(id);

			if (blog == null)
				return RedirectToAction(nameof(Index));

			BlogPostViewModel model = GetEditBlogPostViewModel(blog);

			return View(model);
		}

		[HttpPost]
		[LoggedIn]
		public IActionResult Edit(BlogPostViewModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			if (model.Id == 0)
			{
				UserSession user = GetUserSession();
				BlogItem newBlog = _blogProvider.SaveBlog(new BlogItem(0, user.UserID, model.Title, model.Excerpt,
					model.BlogText, user.UserName, model.Published, DateTime.Now, DateTime.Now,
					DateTime.Now, [.. model.Tags.Split(' ', StringSplitOptions.RemoveEmptyEntries)],
					[]));

				return Redirect(GetBlogItemUrl(newBlog));
			}

			BlogItem blogItem = _blogProvider.GetBlog(model.Id);

			if (blogItem == null)
				ModelState.AddModelError(String.Empty, Languages.LanguageStrings.InvalidBlog);

			if (ModelState.IsValid)
			{
				blogItem.UpdateBlog(model.Title, model.Excerpt, model.BlogText,
					model.Published, model.PublishDateTime,
					[.. model.Tags.Split(' ', StringSplitOptions.RemoveEmptyEntries)]);

				_blogProvider.SaveBlog(blogItem);

				return Redirect(ValidateUserInput(model.Url, ValidationType.RedirectUriLocal));
			}

			return View(model);
		}

		[HttpGet]
		[Breadcrumb(nameof(Languages.LanguageStrings.Search), Name, nameof(Index), HasParams = true)]
		public IActionResult Search()
		{
			BlogSearchViewModel model = new(GetModelData());

			return View(model);
		}

		[HttpPost]
		public IActionResult Search(BlogSearchViewModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			List<BlogItem> blogs = _blogProvider.Search(model.TagName);

			if (blogs.Count == 0)
				ModelState.AddModelError(String.Empty, Languages.LanguageStrings.NoBlogsFoundMatchingTags);

			if (ModelState.IsValid)
			{
				BlogPostsViewModel searchModel = GetBlogPostViewModel(blogs);
				searchModel.Breadcrumbs.Add(new BreadcrumbItem(model.TagName, $"/{Name}/{nameof(Search)}/{model.TagName}/", false));
				return View(nameof(Index), searchModel);
			}

			return View(new BlogSearchViewModel(GetModelData()));
		}

		[HttpGet]
		[HttpPost]
		[Route("/Blog/Search/{tagName}/")]
		public IActionResult Search(string tagName)
		{
			if (String.IsNullOrEmpty(tagName))
				return View(new BlogSearchViewModel(GetModelData()));

			List<BlogItem> blogs = _blogProvider.Search(tagName);

			if (blogs.Count == 0)
				ModelState.AddModelError(String.Empty, Languages.LanguageStrings.NoBlogsFoundMatchingTags);

			if (ModelState.IsValid)
			{
				BlogPostsViewModel model = GetBlogPostViewModel(blogs);
				model.Breadcrumbs.RemoveAt(2);
				model.Breadcrumbs.Add(new BreadcrumbItem(Languages.LanguageStrings.Search, $"/{Name}/{nameof(Search)}/", false));
				model.Breadcrumbs.Add(new BreadcrumbItem(tagName, $"/{Name}/{nameof(Search)}/{tagName}/", false));

				return View(nameof(Index), model);
			}

			return View(new BlogSearchViewModel(GetModelData()));
		}

		[HttpPost]
		[Authorize(Policy = SharedPluginFeatures.Constants.PolicyNameBlogRespond)]
		public IActionResult Comment(CommentViewModel model)
		{
			if (model == null)
				throw new ArgumentNullException(nameof(model));

			if (!IsUserLoggedIn())
				ModelState.AddModelError(String.Empty, Languages.LanguageStrings.PleaseLoginToComment);

			BlogItem blogItem = _blogProvider.GetBlog(model.BlogId);

			if (blogItem == null)
				return RedirectToAction(nameof(Index));


			if (ModelState.IsValid)
			{
				UserSession user = GetUserSession();
				string comment = WebUtility.HtmlEncode(model.Comment).Replace("\r\n", "<br />");
				_blogProvider.AddComment(blogItem, null, user.UserID, user.UserName, comment);
				return Redirect(GetBlogItemUrl(blogItem));
			}

			BlogPostViewModel blogModel = GetBlogPostViewModel(blogItem);
			blogModel.Breadcrumbs.Add(new BreadcrumbItem(Languages.LanguageStrings.Blog, $"/{Name}/{nameof(Index)}", false));
			blogModel.Breadcrumbs.Add(new BreadcrumbItem(blogItem.Title, blogModel.Url, false));
			model.Comment = String.Empty;

			return View("ViewBlog", blogModel);
		}

		#endregion Public Action Methods

		#region Private Methods

		private static string GetBlogItemUrl(in BlogItem blogItem)
		{
			return $"/Blog/{RouteFriendlyName(blogItem.Username)}/{blogItem.Id}/" +
				$"{blogItem.LastModified.ToString("dd-MM-yyyy")}/{RouteFriendlyName(blogItem.Title)}";
		}

		private BlogPostViewModel GetEditBlogPostViewModel(in BlogItem blogItem)
		{
			BlogPostViewModel Result = new(GetModelData(), blogItem.Id, blogItem.Title,
				blogItem.Excerpt, blogItem.BlogText, blogItem.Username, blogItem.Published,
				blogItem.PublishDateTime, blogItem.LastModified, blogItem.Tags);

			return Result;
		}

		private BlogPostViewModel GetBlogPostViewModel(in BlogItem blogItem)
		{
			UserSession user = GetUserSession();
			BlogPostViewModel Result = new(GetModelData(), blogItem.Id,
				blogItem.Title, blogItem.Excerpt, blogItem.BlogText, blogItem.Username,
				blogItem.Published, blogItem.PublishDateTime, blogItem.LastModified,
				blogItem.UserId == user.UserID, blogItem.Tags, IsUserLoggedIn(),
				_settings.AllowComments);

			foreach (BlogComment comment in blogItem.Comments)
			{
				Result.Comments.Add(new BlogCommentViewModel(Convert.ToInt32(comment.Id), comment.DateTime, comment.Username, comment.Comment));
			}


			Result.SeoAuthor = blogItem.Username;
			Result.SeoTitle = blogItem.Title;
			Result.SeoTags = String.Join(' ', blogItem.Tags);
			Result.SeoDescription = blogItem.Excerpt;

			return Result;
		}

		private BlogPostsViewModel GetBlogPostViewModel(List<BlogItem> entries)
		{
			List<BlogPostViewModel> blogPosts = [];
			UserSession user = GetUserSession();

			foreach (BlogItem entry in entries)
			{
				BlogPostViewModel post = new(entry.Id, entry.Title, entry.Excerpt,
					entry.BlogText, entry.Username, entry.Published, entry.PublishDateTime,
					entry.LastModified, user.UserID == entry.UserId, entry.Tags,
					[], IsUserLoggedIn(),
					_settings.AllowComments);

				foreach (BlogComment comment in entry.Comments)
				{
					post.Comments.Add(new BlogCommentViewModel(comment.Id, comment.DateTime, comment.Username, comment.Comment));
				}

				blogPosts.Add(post);
			}

			return new BlogPostsViewModel(GetModelData(), blogPosts);
		}

		private MyBlogsViewModel GetMyBlogsViewModel(List<BlogItem> entries)
		{
			List<BlogPostViewModel> blogPosts = [];

			foreach (BlogItem entry in entries)
			{
				BlogPostViewModel post = new(entry.Id, entry.Title, entry.Excerpt,
					entry.BlogText, entry.Username, entry.Published, entry.PublishDateTime,
					entry.LastModified, true, entry.Tags,
					[], IsUserLoggedIn(),
					_settings.AllowComments);

				foreach (BlogComment comment in entry.Comments)
				{
					post.Comments.Add(new BlogCommentViewModel(comment.Id, comment.DateTime, comment.Username, comment.Comment));
				}

				blogPosts.Add(post);
			}

			return new MyBlogsViewModel(GetModelData(), blogPosts);
		}

		#endregion Private Methods
	}
}

#pragma warning restore CS1591