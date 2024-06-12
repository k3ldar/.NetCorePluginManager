// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S1905:Redundant casts should not be used", Justification = "Left in situ to make it abundantly obvious as byte size matters", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.CreateTableHeaderRecords(System.String,SimpleDB.PageSize)")]
[assembly: SuppressMessage("Major Code Smell", "S3971:\"GC.SuppressFinalize\" should not be called", Justification = "Required at this point as part of functionality", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.#ctor(SimpleDB.ISimpleDBManager,SimpleDB.IForeignKeyManager)")]
