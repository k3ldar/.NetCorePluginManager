// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "static uri that is unlikely to change", Scope = "member", Target = "~F:Resources.Plugin.Controllers.ResourcesController.TikTokBaseUri")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "static uri that is unlikely to change>", Scope = "member", Target = "~F:Resources.Plugin.Controllers.ResourcesController.YouTubeImgBaseUri")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Part of original c# generated code", Scope = "member", Target = "~M:Resources.Plugin.Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Part of original c# generated code", Scope = "member", Target = "~M:Resources.Plugin.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder)")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Usage determines should not be static for backward compatibility", Scope = "member", Target = "~P:Resources.Plugin.Models.BaseResourceItemModel.AllResourceTypes")]
