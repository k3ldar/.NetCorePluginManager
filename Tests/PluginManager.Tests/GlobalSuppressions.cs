// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3885:\"Assembly.Load\" should be used", Justification = "Valid in this instance", Scope = "member", Target = "~M:PluginManager.Tests.PluginManagerTests.AddAssembly_PluginAlreadyLoaded_Returns_DynamicLoadResultAlreadyLoaded")]
