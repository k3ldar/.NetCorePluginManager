// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S1215:\"GC.Collect\" should not be called", Justification = "Required at this point, should not be removed", Scope = "member", Target = "~M:SystemAdmin.Plugin.Classes.GCAnalysis.Run(System.Object)~System.Boolean")]
[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:SystemAdmin.Plugin.Classes.MenuItems.DatabaseTimings.Data~System.String")]
[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:SystemAdmin.Plugin.Classes.MenuItems.AllTimings.Data~System.String")]
