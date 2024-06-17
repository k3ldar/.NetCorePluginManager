// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S2925:\"Thread.Sleep\" should not be used in tests", Justification = "Required in this context", Scope = "member", Target = "~M:PluginManager.DAL.TextFiles.Tests.Providers.UserSessionServiceTests.Closing_SessionIsSavedToDatabase_Success")]
