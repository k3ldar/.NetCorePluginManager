// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S2925:\"Thread.Sleep\" should not be used in tests", Justification = "Is a part of the unit test", Scope = "member", Target = "~M:SimpleDB.Tests.SimpleDbManagerTests.Run_ClearsMemoryViaThread_AfterRecordInserted_Success")]
[assembly: SuppressMessage("Major Code Smell", "S3966:Objects should not be disposed more than once", Justification = "Is a part of the unit test", Scope = "member", Target = "~M:SimpleDB.Tests.SimpleDBOperationTests.Insert_Multiple_ObjectAlreadyDisposed_Throws_ObjectDisposedException")]
[assembly: SuppressMessage("Major Code Smell", "S3966:Objects should not be disposed more than once", Justification = "Is a part of the unit test", Scope = "member", Target = "~M:SimpleDB.Tests.SimpleDBOperationTests.Update_Multiple_ObjectAlreadydisposed_Throws_ObjectDisposedException")]
[assembly: SuppressMessage("Major Code Smell", "S3966:Objects should not be disposed more than once", Justification = "Is a part of the unit test", Scope = "member", Target = "~M:SimpleDB.Tests.SimpleDBOperationTests.NextSequence_ObjectAlreadyDisposed_Throws_ObjectDisposedException")]
[assembly: SuppressMessage("Major Code Smell", "S3966:Objects should not be disposed more than once", Justification = "Is a part of the unit test", Scope = "member", Target = "~M:SimpleDB.Tests.SimpleDBOperationTests.Select_ById_ObjectDisposed_Throws_ObjectDisposedException")]
[assembly: SuppressMessage("Major Code Smell", "S3966:Objects should not be disposed more than once", Justification = "Is a part of the unit test", Scope = "member", Target = "~M:SimpleDB.Tests.SimpleDBOperationTests.ResetSequence_ObjectAlreadyDisposed_Throws_ObjectDisposedException")]
