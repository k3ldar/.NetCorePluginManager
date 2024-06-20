// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:PluginManager.DAL.TextFiles.Providers.BlogProvider.AddComment(Middleware.Blog.BlogItem@,Middleware.Blog.BlogComment@,System.Int64@,System.String@,System.String@)")]
[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>", Scope = "member", Target = "~M:PluginManager.DAL.TextFiles.Providers.DynamicContentProvider.Templates~System.Collections.Generic.List{SharedPluginFeatures.DynamicContent.DynamicContentTemplate}")]
[assembly: SuppressMessage("Major Bug", "S1244:Floating point numbers should not be tested for equality", Justification = "Fine as is", Scope = "member", Target = "~P:PluginManager.DAL.TextFiles.Tables.PageViewsDataRow.TotalTime")]
[assembly: SuppressMessage("Major Code Smell", "S1854:Unused assignments should be removed", Justification = "Without it there will be no depth", Scope = "member", Target = "~M:PluginManager.DAL.TextFiles.Providers.HelpdeskProvider.InternalGetKnowledgebaseGroup(System.Int64,System.Int32)~Middleware.Helpdesk.KnowledgeBaseGroup")]
