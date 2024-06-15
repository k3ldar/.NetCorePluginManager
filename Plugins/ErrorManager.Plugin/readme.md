This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Eror Manager
The error manager plugin module is both a website and middleware plugin module. The middleware contains exception handling and should be loaded first to ensure that it can capture any exceptions generated. Host applications should implement the IErrorManager interface which is used by the middleware to obtain extra information for missing links or provide extra information on other exceptions

## System Admin Plugin
The error manager plugin enables three system admin menu items, one for general exceptions, one for missing links and a further Timings menu to view how much time was spent processing errors.

## Customise Error Pages
As with all website plugin modules, the pages that are created in /Views/Error folder can be customised and personalised from their default settings to match the theme of the website where they will be shown.