This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Login Plugin
The Login.Plugin module has been designed to allow users to login from a single point, almost every interactive website contains a user area, requiring a user to login, this module has been designed to fill this gap.

At the core of the Login plugin module is the ILoginProvider interface, this interface provides the necessary methods which allow the plugin to operate and must be implemented by the host application.