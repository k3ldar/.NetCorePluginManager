This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Product Plugin 
The Product plugin provides a convenient and easy way to display products within a website. Each product can have many properties that will be shown depending on whether they exist or not. The current list of supported properties are:

- Name.
- Multiple images.
- Description.
- Tag Line.
- Primary and sub categories.
- Features.
- Video Link
- Integration with the ShoppingCartplugin module.
- Name based product routes.
- Stock Availability.
- New Product.
- Featured Product.
- Best Selling Product.


## Integration with Middleware
The IProductProvider interface should be implemented by the host application, this contains all methods that are required to interact with the Product plugin module.

## Product Groups
Products are grouped into individual product groups for easy viweing within the site. Products can be categorized into one or more product groups, they must have a primary group.

## Customise Product Pages
As with all website plugin modules, the pages that are created in /Views/Product folder can be customised and personalised from their default settings to match the theme of the website where they will be shown.