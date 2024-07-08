// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "ModelState will be invalid if it's null", Scope = "member", Target = "~M:Blog.Plugin.Controllers.BlogController.Edit(Blog.Plugin.Models.BlogPostViewModel)~Microsoft.AspNetCore.Mvc.IActionResult")]
[assembly: SuppressMessage("Usage", "ASP0018:Unused route parameter", Justification = "Left in as forms part of route name", Scope = "member", Target = "~M:Blog.Plugin.Controllers.BlogController.ViewBlog(System.Int32)~Microsoft.AspNetCore.Mvc.IActionResult")]
