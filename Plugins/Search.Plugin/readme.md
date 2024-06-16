This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Search Plugin
The search plugin introduces two areas that can be used for searching, they are:

- Quick Search.
- Advanced Search

## ISearchProvider Interface
The ISearchProvider interface is used to facilitate searching within an ASP Core website, there should only be one instance of this interface registered as a singleton through DI, if a custom implementation is not registered then a default search provider will be registered.

The search provider is responsible for both performing a search and retrieving advanced search options for use within the UI to aid user searching.

## ISearchKeywordProvider Interface
At the heart of both quick and advanced searching lies the ISearchKeywordProvider interface which is implemented by each plugin that wishes to expose search facilities to the search plugin.

This interface contians a method for searching which uses the KeywordSearchOptions class to pass search data and returns a list SearchResponseItem's for the search results. It also allows implementations to return advanced search configuration data that can optionally be used to provide custom search facilities within the search plugin home page.