This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Cache Control Plugin
Cache control plugin has been designed to add rule based cache settings for any route within a website. In its basic form it simple adds a Cache-Control header item to the response which depending on the configuration will tell the browser that the response should be cached or not, and if caching how long it should be cached for.

### Performance
Caching can dramatically improve the page load speed, after the initial request thus improving the overall user experience within the website. It can also improve server performance, if a response is cached then the browser would not need to contact the server to retrieve data that it has already received and saved locally.