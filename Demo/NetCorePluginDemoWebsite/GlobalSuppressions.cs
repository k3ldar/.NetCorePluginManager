﻿// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockHelpdeskProvider.GetFeedback(System.Boolean@)~System.Collections.Generic.List{Middleware.Helpdesk.Feedback}")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "static uri that is unlikely to change", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockClaimsProvider.GetUserClaims(System.Int64@)~System.Collections.Generic.List{System.Security.Claims.ClaimsIdentity}")]
[assembly: SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "static uri that is unlikely to change", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.PluginInitialisation.ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection)")]
[assembly: SuppressMessage("Major Code Smell", "S3010:Static fields should not be updated in constructors", Justification = "Mock class so ok", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockHelpdeskProvider.#ctor")]
[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "Mock class", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockShoppingCartPluginProvider.AddToCart(Shared.Classes.UserSession@,SharedPluginFeatures.ShoppingCartSummary@,Middleware.Products.Product@,System.Int32@)~System.Int64")]
[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "Mock class", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.Mocks.MockDynamicContentProvider.Templates~System.Collections.Generic.List{SharedPluginFeatures.DynamicContent.DynamicContentTemplate}")]
[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "By design", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockShoppingCartPluginProvider.cartCacheManager_ItemNotFound(System.Object,Shared.CacheItemNotFoundArgs)")]
[assembly: SuppressMessage("Major Code Smell", "S3010:Static fields should not be updated in constructors", Justification = "Mock class", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockShoppingCartPluginProvider.#ctor(Middleware.IProductProvider,Middleware.Accounts.IAccountProvider)")]
[assembly: SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "Mock class", Scope = "member", Target = "~M:AspNetCore.PluginManager.DemoWebsite.Classes.MockShoppingCartPluginProvider.Dispose")]
