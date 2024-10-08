﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "<Pending>", Scope = "member", Target = "~M:DemoApiPlugin.Controllers.DemoApiController.TestApi(System.String,System.String,System.String)~Microsoft.AspNetCore.Mvc.IActionResult")]
[assembly: SuppressMessage("Major Code Smell", "S6962:You should pool HTTP connections with HttpClientFactory", Justification = "Used as part of a demo app in this context", Scope = "member", Target = "~M:DemoApiPlugin.Controllers.DemoApiController.TestApi(System.String,System.String,System.String)~Microsoft.AspNetCore.Mvc.IActionResult")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Part of original c# generated code", Scope = "member", Target = "~M:DemoApiPlugin.Startup.Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder)")]
