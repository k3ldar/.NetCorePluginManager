// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S6602:\"Find\" method should be used instead of the \"FirstOrDefault\" extension", Justification = "Not possible on arrays", Scope = "member", Target = "~M:BadEgg.Plugin.BadEggMiddleware.LoadRouteData(Microsoft.AspNetCore.Mvc.Infrastructure.IActionDescriptorCollectionProvider,SharedPluginFeatures.IRouteDataService,PluginManager.Abstractions.IPluginTypesService)")]
[assembly: SuppressMessage("Minor Code Smell", "S1643:Strings should not be concatenated using '+' in a loop", Justification = "Not always more efficient to use string builder", Scope = "member", Target = "~M:BadEgg.Plugin.WebDefender.IpConnectionInfo.GetResults~System.String")]
[assembly: SuppressMessage("Minor Code Smell", "S1643:Strings should not be concatenated using '+' in a loop", Justification = "Not always more efficient to use string builder", Scope = "member", Target = "~M:BadEgg.Plugin.WebDefender.ValidateConnections.ValidateRequest(Microsoft.AspNetCore.Http.HttpRequest@,System.Boolean@,System.Int32@)~SharedPluginFeatures.Enums.ValidateRequestResult")]
