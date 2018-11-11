# .NetCorePluginManager
.Net Core Plugin Manager, extend web applications using plugin technology

https://sicarterblog.wordpress.com/2018/10/02/asp-net-core-plugin-manager/

## Current Version
Version 1.0.12

# ASPNetCore.PluginManager
Extend website with plugin technology.

PM> Install-Package AspNetCore.PluginManager -Version 1.0.6

https://www.nuget.org/packages/AspNetCore.PluginManager/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/Plugin-Manager-Setup-and-Configuration for setup and configuration.

# Memory Cache Plugin
Provides memory cache capability for any application or middleware.

PM> Install-Package MemoryCache.Plugin -Version 1.0.0

https://www.nuget.org/packages/MemoryCache.Plugin/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/Memory-Cache-Plugin for setup and configuration.

# User Session Middleware
Manage user sessions within Controllers.

PM> Install-Package UserSessionMiddleware.Plugin -Version 1.0.3

https://www.nuget.org/packages/UserSessionMiddleware.Plugin/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/User-Session-Manager-Middleware for setup and configuration.

# CacheControl
Manage cache-control headers for user defined routes.  Add browser caching for static files like .js, .css and image files etc.

PM> Install-Package CacheControl.Plugin -Version 1.0.0

See https://github.com/k3ldar/.NetCorePluginManager/wiki/CacheControl-Plugin-Settings for setup and configuration.

# Deny Spider
Automatically generate robots.txt file from attributes applied to conntroller classes and methods.  If used in conjunction with UserSession Manager it will provide a forbidden response if a bot/spider attempts to go to a denied path.

PM > Install-Package Spider.Plugin -Version 1.0.2

https://www.nuget.org/packages/Spider.Plugin/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/Spider-Plugin-Middleware for setup and configuration.

# Restrict Ip
Automatically restricts a route to specific Ip Addresses.

https://github.com/k3ldar/.NetCorePluginManager/wiki/Restrict-Ip-Address-Plugin

# Geo Ip
Integrates GeoIp services for internal use, or use with User Session Middleware

https://github.com/k3ldar/.NetCorePluginManager/wiki/Geo-Ip-Plugin

# System Admin
Displays Application specific system data

# Bad Egg
Nobody likes it when people don't play fair, the bad egg plugin is designed to complete 2 functions.

the first is to restrict the maximum connections per minute for an Ip Address.
The second is to look at query strings and form input values, and determine whether the connection is attempting to use Sql injection techniques.  This provides a "probability" and it is down to the host application to white/black list Ip addresses.

