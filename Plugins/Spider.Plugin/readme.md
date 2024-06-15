This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Spider Plugin
Spider plugin has been designed to manage robots that visit a site, it completes this by performing 3 tasks

- Create a list of routes that should not be visited by a bot, based on attributes placed on action methods.
- Monitor bots navigating through a website and return a 403 error if a bot enters a route that it should not visit.
- Serve /robots.txt which is built based on routes denied based on DenySpiderAttribute.
- 
This is achieved by adding DenySpiderAttribute to a controller class or action method, the following code sample demostrates 2 action methods that are denied to bots:

```

[DenySpider]
[Breadcrumb(nameof(Languages.LanguageStrings.Privacy))]
public IActionResult Privacy()
{
    return View(new BaseModel(GetBreadcrumbs(), GetCartSummary()));
}

```

```

[DenySpider("*")]
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
public IActionResult Error()
{
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}

```