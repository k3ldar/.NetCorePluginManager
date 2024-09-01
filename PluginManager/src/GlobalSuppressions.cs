// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Bug", "S2674:The length returned from a stream read should be checked", Justification = "Reads entire length in one go", Scope = "member", Target = "~M:PluginManager.BasePluginManager.ExtractResources(System.Reflection.Assembly@,PluginManager.PluginSetting@)")]
