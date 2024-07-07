// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S1215:\"GC.Collect\" should not be called", Justification = "Required at this point, should not be removed", Scope = "member", Target = "~M:SystemAdmin.Plugin.Classes.GCAnalysis.Run(System.Object)~System.Boolean")]
[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:SystemAdmin.Plugin.Classes.MenuItems.DatabaseTimings.Data~System.String")]
[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:SystemAdmin.Plugin.Classes.MenuItems.AllTimings.Data~System.String")]
[assembly: SuppressMessage("Minor Code Smell", "S6602:\"Find\" method should be used instead of the \"FirstOrDefault\" extension", Justification = "Not possible on arrays", Scope = "member", Target = "~M:SystemAdmin.Plugin.Controllers.SystemAdminController.ValidateIncomingProperties(SystemAdmin.Plugin.Models.SettingsViewModel,SystemAdmin.Plugin.Classes.MenuItems.SettingsMenuItem,System.Collections.Generic.List{System.String})")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Left as non static due to current usage and backwards compatibility", Scope = "member", Target = "~M:SystemAdmin.Plugin.Models.AvailableIconViewModel.GetMenuLink(SharedPluginFeatures.SystemAdminSubMenu@)~System.String")]
[assembly: SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Left as non static due to current usage and backwards compatibility", Scope = "member", Target = "~M:SystemAdmin.Plugin.Models.AvailableIconViewModel.ProcessImage(System.String@)~System.String")]
