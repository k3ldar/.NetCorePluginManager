// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Bug", "S2674:The length returned from a stream read should be checked", Justification = "Reads entire length of stream", Scope = "member", Target = "~M:ApiAuthorization.Plugin.Classes.HmacApiAuthorizationService.ValidateApiRequest(Microsoft.AspNetCore.Http.HttpRequest,System.String,System.Int32@)~System.Boolean")]
[assembly: SuppressMessage("Reliability", "CA2022:Avoid inexact read with 'Stream.Read'", Justification = "Will only work with amount of data it gets back", Scope = "member", Target = "~M:ApiAuthorization.Plugin.Classes.HmacApiAuthorizationService.ValidateApiRequest(Microsoft.AspNetCore.Http.HttpRequest,System.String,System.Int32@)~System.Boolean")]
