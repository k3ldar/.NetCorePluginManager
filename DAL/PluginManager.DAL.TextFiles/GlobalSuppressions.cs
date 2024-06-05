// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:PluginManager.DAL.TextFiles.Providers.BlogProvider.AddComment(Middleware.Blog.BlogItem@,Middleware.Blog.BlogComment@,System.Int64@,System.String@,System.String@)")]
