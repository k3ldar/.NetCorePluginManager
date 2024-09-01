// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:Middleware.Search.DefaultSearchThread.KeywordSearch(Middleware.ISearchKeywordProvider@,Middleware.Search.KeywordSearchOptions@)~System.Collections.Generic.List{Middleware.Search.SearchResponseItem}")]
[assembly: SuppressMessage("Major Code Smell", "S6561:Avoid using \"DateTime.Now\" for benchmarking or timing operations", Justification = "Required for this context", Scope = "member", Target = "~M:Middleware.Search.DefaultSearchThread.KeywordSearch(System.Collections.Generic.List{Middleware.ISearchKeywordProvider}@,Middleware.Search.KeywordSearchOptions@)~System.Collections.Generic.List{Middleware.Search.SearchResponseItem}")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Part of the design", Scope = "member", Target = "~M:Middleware.ShoppingCart.ShoppingCartItem.UpdateDiscountCode(System.String,SharedPluginFeatures.DiscountType,System.Decimal,System.Int32)")]
