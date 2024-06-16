This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Localization Plugin
The Localization.Plugin module provides a simple mechanism for ensuring a site is localizable. It supports the UserSessionMiddleware.Plugin which contains the culture for the session and ensures that the executing thread uses the correct UI culture.

## Languages
As well as augmenting UserSessionMiddleware, this plugin also implements the IStringLocalizer and IStringLocalizerFactory classes which override the default Net Core localization and allows the Languages module to be used by default.