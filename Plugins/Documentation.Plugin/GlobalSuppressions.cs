// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<Pending>")]
[assembly: SuppressMessage("Major Code Smell", "S2589:Boolean expressions should not be gratuitous", Justification = "seems ok in switch statement", Scope = "member", Target = "~M:DocumentationPlugin.Classes.DocumentPostProcess.SplitAndFindReplaceableTags(Shared.Docs.Document@,Shared.Docs.BaseDocument@,System.String@,System.Boolean@,System.Boolean@)~System.String")]
[assembly: SuppressMessage("Minor Code Smell", "S3220:Method calls should not resolve ambiguously to overloads with \"params\"", Justification = "Reviewed and ok", Scope = "member", Target = "~M:DocumentationPlugin.Classes.DefaultDocumentationService.SaveFileList(System.Collections.Generic.List{System.String}@)")]
