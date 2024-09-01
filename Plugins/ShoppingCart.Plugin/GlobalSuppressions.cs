// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Part of initial c# implementation", Scope = "member", Target = "~M:ShoppingCartPlugin.Startup.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Left as non static due to current usage and backwards compatibility", Scope = "member", Target = "~M:ShoppingCartPlugin.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder)")]
