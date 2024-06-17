// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockHelpdeskProvider.GetFeedback(System.Boolean@)~System.Collections.Generic.List{Middleware.Helpdesk.Feedback}")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "static uri that is unlikely to change", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockClaimsProvider.GetUserClaims(System.Int64@)~System.Collections.Generic.List{System.Security.Claims.ClaimsIdentity}")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "static uri that is unlikely to change", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.PluginInitialisation.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
