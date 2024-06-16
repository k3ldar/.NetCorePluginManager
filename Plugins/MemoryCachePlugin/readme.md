This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Memory Cache Plugin
Even with the advent of faster solid state drives and quicker processors, there is no substitute to reading data from memory rather than from a database or other data store. Especially if the database is on a different server connected via the network, all of which add a slight delay in each step to obtaining and serving data to the end user.

The Memory Cache plugin is designed to provide access to several different types of cache which can be used to cache data and improve the speed at which requests are processed by keeping regularly used items or static data in memory.

There are advantages and disadvantages to caching data and a decision needs to be made on whether the data should be cached, the length of time it can be cached, how to ensure cached data does not go stale and managing the automatic expiration of cached data when it expires.

CacheManager is a class that is specifically designed to manage cached data in multiple, user defined caches, it employs a seperate background thread that is used to manage a CacheItem lifetime, as each CacheItem expires it is automatically removed from the CacheManager.

## Short Cache
The first cache is the short cache, this has by default a 5 minute lifetime, although this can be changed to between 1 and 30 minutes. This cache should be used to store data that could go stale quite quickly, for instance pricing data or availability.

## Default Cache
The default cache has a default lifetime of 2 hours but can also be configured to cache for between 30 and 480 minutes. This cache is designed to store data which is subject to less frequent changes such as Product data etc.

## Extending Cache
The extending cache has the same settings as the default cache with one exception, every time an item is retrieved the expiration date is reset. This is useful if you want to keep regularly used data in memory.

## Permanent Cache
The permanent cache is used to permanently store data in memory, the items in their will never expire. This can be useful to store data that requires fast lookups but never changes.