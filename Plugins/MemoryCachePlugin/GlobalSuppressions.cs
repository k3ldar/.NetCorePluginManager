// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S3010:Static fields should not be updated in constructors", Justification = "Required as part of flyweight pattern where multiple instances share same underlying cache.  First one created creates the cache items with correct timeout values", Scope = "member", Target = "~M:MemoryCache.Plugin.DefaultMemoryCache.#ctor(PluginManager.Abstractions.ISettingsProvider)")]
