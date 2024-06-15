This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# MVC Plugin Manager
Net Core Plugin Manager augments a modular design, it's primary purpose is to load modules from multiple assemblies and ensure they are registered within the MVC pipeline. This approach ensures that websites can easily expand as new features are bought online whilst allowing shared modules to be reused across multiple implementations. The primary benefit being that well tested code can be reused within multiple websites.

There are many types of plugin modules that can be created for use within a Net Core MVC application, they include:

- Sub website. Individual Controller, models and views for a specific module such as product, company services, shopping cart, login or user account.
- API controller. Add individual or multiple APIs to a website, this could include individual versions of a specific API.
- Middleware extensions. Easily include middleware classes that can control or affect the request pipeline.
- Services. Add services for IoC.
- Custom interfaces.