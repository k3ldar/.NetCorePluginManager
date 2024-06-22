// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S1905:Redundant casts should not be used", Justification = "Left in situ to make it abundantly obvious as byte size matters", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.CreateTableHeaderRecords(System.String,SimpleDB.PageSize)")]
[assembly: SuppressMessage("Major Code Smell", "S3971:\"GC.SuppressFinalize\" should not be called", Justification = "Required at this point as part of functionality", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.#ctor(SimpleDB.ISimpleDBManager,SimpleDB.IForeignKeyManager)")]
[assembly: SuppressMessage("Major Code Smell", "S3358:Ternary operators should not be nested", Justification = "Easy to read/follow>", Scope = "member", Target = "~M:SimpleDB.TableAttribute.#ctor(System.String,System.String,SimpleDB.CompressionType,SimpleDB.CachingStrategy,SimpleDB.WriteStrategy)")]
[assembly: SuppressMessage("Minor Code Smell", "S3267:Loops should be simplified with \"LINQ\" expressions", Justification = "Linq not required here", Scope = "member", Target = "~M:SimpleDB.TableAttribute.#ctor(System.String,System.String,SimpleDB.CompressionType,SimpleDB.CachingStrategy,SimpleDB.WriteStrategy)")]
[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Not required here", Scope = "type", Target = "~T:SimpleDB.ForeignKeyException")]
[assembly: SuppressMessage("Minor Code Smell", "S1939:Inheritance list should not be redundant", Justification = "Needs to be explicit", Scope = "type", Target = "~T:SimpleDB.PageSize")]
[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Not required here", Scope = "type", Target = "~T:SimpleDB.UniqueIndexException")]
[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Not required here", Scope = "type", Target = "~T:SimpleDB.InvalidDataRowException")]
[assembly: SuppressMessage("Major Code Smell", "S3925:\"ISerializable\" should be implemented correctly", Justification = "Not required", Scope = "type", Target = "~T:SimpleDB.ObservableDictionary`2")]
[assembly: SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "This is by design to obtain just the table name without path data", Scope = "member", Target = "~P:SimpleDB.Internal.SimpleDBOperations`1.TableName")]
[assembly: SuppressMessage("Minor Code Smell", "S6602:\"Find\" method should be used instead of the \"FirstOrDefault\" extension", Justification = "OK in this context", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.GetForeignKeysForTable~System.Collections.Generic.Dictionary{System.String,SimpleDB.Internal.SimpleDBOperations`1.ForeignKeyRelation`0}")]
[assembly: SuppressMessage("Minor Code Smell", "S6602:\"Find\" method should be used instead of the \"FirstOrDefault\" extension", Justification = "OK in this context", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.BuildIndexListForTable~SimpleDB.Internal.BatchUpdateDictionary{System.String,SimpleDB.IIndexManager}")]
[assembly: SuppressMessage("Minor Code Smell", "S6602:\"Find\" method should be used instead of the \"FirstOrDefault\" extension", Justification = "OK in this context", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.GetTableAttributes~SimpleDB.TableAttribute")]
[assembly: SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "Implementations can return null at this point", Scope = "member", Target = "~M:SimpleDB.Internal.SimpleDBOperations`1.Initialize(PluginManager.Abstractions.IPluginClassesService)")]
