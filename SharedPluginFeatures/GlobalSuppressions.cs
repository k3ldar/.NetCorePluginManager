// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "System implementation", Scope = "member", Target = "~M:SharedPluginFeatures.SystemAdminSubMenu.op_GreaterThanOrEqual(SharedPluginFeatures.SystemAdminSubMenu,SharedPluginFeatures.SystemAdminSubMenu)~System.Boolean")]
[assembly: SuppressMessage("Style", "IDE0041:Use 'is null' check", Justification = "System implementation", Scope = "member", Target = "~M:SharedPluginFeatures.SystemAdminSubMenu.op_GreaterThan(SharedPluginFeatures.SystemAdminSubMenu,SharedPluginFeatures.SystemAdminSubMenu)~System.Boolean")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "Method is accessed else where as part of implementation", Scope = "member", Target = "~M:SharedPluginFeatures.SystemAdminMainMenu.Area~System.String")]
[assembly: SuppressMessage("Major Code Smell", "S3966:Objects should not be disposed more than once", Justification = "False positive as may not be disposed in current implementation", Scope = "member", Target = "~M:SharedPluginFeatures.CaptchaImage.GenerateImage")]
[assembly: SuppressMessage("Minor Code Smell", "S1694:An abstract class should have both abstract and concrete methods", Justification = "This allows descendants to override all in this implementation or use interface if required", Scope = "type", Target = "~T:SharedPluginFeatures.BaseClasses.CronJob")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "No issue here, need to remain backwards compatibility", Scope = "member", Target = "~M:SharedPluginFeatures.BaseModel.RouteText(System.String@)~System.String")]
[assembly: SuppressMessage("Minor Code Smell", "S2325:Methods and properties that don't access instance data should be static", Justification = "No issue here, need to remain backwards compatibility", Scope = "member", Target = "~M:SharedPluginFeatures.BaseController.IsUriLocalToHost(System.String)~System.Boolean")]
[assembly: SuppressMessage("Minor Code Smell", "S1694:An abstract class should have both abstract and concrete methods", Justification = "No issue as fine as abstract class", Scope = "type", Target = "~T:SharedPluginFeatures.CarouselImage")]
[assembly: SuppressMessage("Minor Code Smell", "S1694:An abstract class should have both abstract and concrete methods", Justification = "No issue as fine as abstract class", Scope = "type", Target = "~T:SharedPluginFeatures.MainMenuItem")]
