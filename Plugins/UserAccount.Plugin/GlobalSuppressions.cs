// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Bug", "S2259:Null pointers should not be dereferenced", Justification = "ModelState will be invalid if it's null", Scope = "member", Target = "~M:UserAccount.Plugin.Controllers.AccountController.LicenceCreate(UserAccount.Plugin.Models.CreateLicenceViewModel)~Microsoft.AspNetCore.Mvc.IActionResult")]
[assembly: SuppressMessage("Major Code Smell", "S3928:Parameter names used into ArgumentException constructors should match an existing one ", Justification = "Probably not the best example but unusre what else to use", Scope = "member", Target = "~M:UserAccount.Plugin.Models.OrderViewModel.#ctor(SharedPluginFeatures.BaseModelData@,Middleware.Accounts.Orders.Order)")]
