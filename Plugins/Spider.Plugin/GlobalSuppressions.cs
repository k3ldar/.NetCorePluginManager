// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0220:Add explicit cast", Justification = "the conversion is valid due to above line in retrieving array", Scope = "member", Target = "~M:Spider.Plugin.Classes.Robots.SortAndFilterDenyRoutesByAgent(Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorCollectionProvider,SharedPluginFeatures.IRouteDataService,System.Collections.Generic.List{System.Type})~System.Collections.Generic.Dictionary{System.String,System.Collections.Generic.List{SharedPluginFeatures.IRobotRouteData}}")]
[assembly: SuppressMessage("Performance", "CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "Reviewed and left as is", Scope = "member", Target = "~M:Spider.Plugin.SpiderMiddleware.Invoke(Microsoft.AspNetCore.Http.HttpContext)~System.Threading.Tasks.Task")]
