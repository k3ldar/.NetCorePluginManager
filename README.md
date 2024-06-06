# .NetCorePluginManager
.Net Core Plugin Manager, extend web applications, services and normal applications using plugin technology

Copyright (c) 2018 - 2023 Simon Carter (s1cart3r@gmail.com).

https://sicarterblog.wordpress.com/2018/10/02/asp-net-core-plugin-manager/

## Devops
https://dev.azure.com/NetCorePluginManager/NetCorePluginManager
https://ci.appveyor.com/project/k3ldar/netcorepluginmanager

master: ![Build Status Master Branch](https://ci.appveyor.com/api/projects/status/o2ievlf7vrs2rn0x/branch/master?svg=true)

Latest Branch: ![Build Status Latest Build](https://ci.appveyor.com/api/projects/status/o2ievlf7vrs2rn0x?svg=true)

Master: [![CodeQL](https://github.com/k3ldar/.NetCorePluginManager/actions/workflows/codeql-analysis.yml/badge.svg?branch=master)](https://github.com/k3ldar/.NetCorePluginManager/actions/workflows/codeql-analysis.yml)

[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=vulnerabilities)](https://sonarcloud.io/summary/overall?id=k3ldar_.NetCorePluginManager) [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=bugs)](https://sonarcloud.io/summary/overalloverall?id=k3ldar_.NetCorePluginManager) [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=code_smells)](https://sonarcloud.io/summary/overall?id=k3ldar_.NetCorePluginManager) [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=sqale_index)](https://sonarcloud.io/summary/overall?id=k3ldar_.NetCorePluginManager)

[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=reliability_rating)](https://sonarcloud.io/summary/overall?id=k3ldar_.NetCorePluginManager) [![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=sqale_rating)](https://sonarcloud.io/summary/overall?id=k3ldar_.NetCorePluginManager) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=security_rating)](https://sonarcloud.io/summary/overall?id=k3ldar_.NetCorePluginManager) 

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=k3ldar_.NetCorePluginManager) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=k3ldar_.NetCorePluginManager&metric=coverage)](https://sonarcloud.io/summary/new_code?id=k3ldar_.NetCorePluginManager)

# Live Demo
http://PluginManager.website

http://PluginManager.website/SystemAdmin/

http://PluginManager.website/Account/

http://PluginManager.website/Blog/

#Documentation

http://PluginManager.website/Docs/

if asked to login this requires the following details:

Username: admin
Password: password

It only uses mock data providers and will reset periodically

## Current Version
Version 5.6.0


# ASPNetCore.PluginManager
Extend any website with plugin technology.

PM> Install-Package AspNetCore.PluginManager -Version 5.6.0

https://www.nuget.org/packages/AspNetCore.PluginManager/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/Plugin-Manager-Setup-and-Configuration for setup and configuration.

# Build Prerequisites
In order to build the latest version both Visual Studio 2022 and net 6.0 or net8.0.

# Current Plugins
There are a number of pre-built plugins which can be used in most application types, they include:

## Memory Cache Plugin
Provides memory cache capability for any application or middleware.

https://www.nuget.org/packages/MemoryCache.Plugin/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/Memory-Cache-Plugin for setup and configuration.

## User Session Middleware
Manage user sessions within Controllers.

https://www.nuget.org/packages/UserSessionMiddleware.Plugin/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/User-Session-Manager-Middleware for setup and configuration.

## CacheControl
Manage cache-control headers for user defined routes.  Add browser caching for static files like .js, .css and image files etc.

See https://github.com/k3ldar/.NetCorePluginManager/wiki/CacheControl-Plugin-Settings for setup and configuration.

## Deny Spider
Automatically generate robots.txt file from attributes applied to conntroller classes and methods.  If used in conjunction with UserSession Manager it will provide a forbidden response if a bot/spider attempts to go to a denied path.

https://www.nuget.org/packages/Spider.Plugin/

See https://github.com/k3ldar/.NetCorePluginManager/wiki/Spider-Plugin-Middleware for setup and configuration.

## Restrict Ip
Automatically restricts a route to specific Ip Addresses.

https://github.com/k3ldar/.NetCorePluginManager/wiki/Restrict-Ip-Address-Plugin

## Geo Ip
Integrates GeoIp services for internal use, or use with User Session Middleware

https://github.com/k3ldar/.NetCorePluginManager/wiki/Geo-Ip-Plugin

## System Admin
Displays Application specific system data

## Bad Egg
Nobody likes it when people don't play fair, the bad egg plugin is designed to complete 2 functions.

the first is to restrict the maximum connections per minute for an Ip Address.
The second is to look at query strings and form input values, and determine whether the connection is attempting to use Sql injection techniques.  This provides a "probability" and it is down to the host application to white/black list Ip addresses.

https://github.com/k3ldar/.NetCorePluginManager/wiki/Bad-Egg-Plugin

## Error Manager
Manages exceptions and 404 errors, allows implementation to provide a replacement for missing pages.  All other errors are notified but will not be notified again until after a timeout period, which prevents spamming on errors.

https://github.com/k3ldar/.NetCorePluginManager/wiki/Error-Manager-Plugin

