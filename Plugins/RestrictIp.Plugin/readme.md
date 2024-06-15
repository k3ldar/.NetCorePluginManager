This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Restrict Ip Plugin
The RestrictIp plugin modules allows individual routes, or a collection of roots to be restricted to a specific range of Ip Addresses. This can be useful for instance if you wnat to ensure that only connections from an internal Ip range are allowed into specific routes, for instance staff only areas.

It achieves this by adding an instance of the RestrictedIpRouteAttribute to a controller. The RestrictedIpRoute attribute takes a single parameter which is the name of the profile which controls what Ip addresses are allowed to enter the route. RestrictIpSettings class contains the configuration data and is read from appsettings.json.

The following code sample demonstrates how the attribute is added to a route with a profile name of RestrictedRouteRemote

```

[ApiController]
[RestrictedIpRoute("RestrictedRouteRemote")]
[Route("/api/Restricted/")]
public class RestrictedRouteController : ControllerBase
{
    [HttpGet]
    public string About()
    {
        return ("Test");
    }
}

```


The following sample shows the appsettings.json with the restricted route settings:

```

"RestrictedIpRoutes.Plugin": {
  "Disabled": false,
  "RouteRestrictions": {
    "RestrictedRouteAllowAll": "*",
    "RestrictedRouteLocal": "localhost",
    "RestrictedRouteRemote": "10.30.*;192.168.*",
    "SystemAdminRoute": "127.0.0.1;72.15.*"
  }
}

```