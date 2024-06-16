This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Sitemap Plugin
The sitemap plugin is designed to manage sitemap files for a website. Sitemaps are not a requirement to a website but can improve overall search engine optimisation (SEO) by providing an easy way for robots to find and rank pages, index the pages which are more important to the site owner by providing ranks and find pages that are not directly linked, or have links that are too deeply rooted.

This plugin follows the standards and protocols for creating sitemaps, but leaves the details down to individual plugins to provide the data which is used to generate the sitemap.

To provide sitemap details a plugin must create a class that extends the ISitemapProvider interface, this interface provides a single method which is used by the sitemap plugin to generate a sitemap.

This plugin generates the sitemap.xml as a sitemap index file, this sitemap index will contain a link for each ISitemapProvider implementation, the only caveat to this is that any plugin which provides more than 25,000 items will be split into multiple index files. The maximum allowed limit is for the number of items is 50,000 items, however, there is also a limit of 50mb per xml file. By splitting the sitemap items into chunks of 25,000 items this should ensure the maximum item limit is not exceeded.