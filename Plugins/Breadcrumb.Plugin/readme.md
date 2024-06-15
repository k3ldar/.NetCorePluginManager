This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Breadcrumb Plugin
Breadcrumbs provide an additional form of navigational aid to users, they can help to inform the user of their current position within the site and provide an additional form of navigation.

The breadcrumb plugin makes it easy to create breadcrumbs within a web application, it works in two ways:

- Attributes are placed on controller methods where a breadcrumb is required.
- Progmatically create breadcrumb items and their heirerarchy whilst processing requests.

- However they are used, they follow a very simple form, a breadcrumb can function as both a parent and child to another breadcrumb item. If attribute breadcrumbs are used then the parent of the breadcrumb is also listed, this allows the middleware to build up the breadcrumb heirerarchy which is then inserted into the temp data for the context being processed.

Once the breadcrumb data has been provided to the controller, it can then be extracted and placed within the view model for display within the view.