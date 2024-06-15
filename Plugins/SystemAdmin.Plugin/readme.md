This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# System Admin Plugin
The SystemAdmin plugin module is a website based plugin which allows system administrators to view the state of the website as a snapshot, as well as historical data on error and user sessions.

At it's core the SystemAdmin will obtain all instances of SystemAdminMainMenu classes, these are created by individual plugin modules, or the primary website itself and provide admin functionality. It also looks for all instances of SystemAdminSubMenu class, these classes are also implemented by individual plugin modules and provide a rich set of data that can be used to check on the health of the website as a whole.

SystemAdmin plugin only displays data exposed by other plugin modules, or the host application.