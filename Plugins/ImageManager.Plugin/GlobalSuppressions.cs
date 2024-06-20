// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "Required for unique id in app lifetime", Scope = "member", Target = "~M:ImageManager.Plugin.Controllers.ImageManagerController.GetCacheId~System.String")]
