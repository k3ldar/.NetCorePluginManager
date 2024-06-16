This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Subdomain Plugin
A subdomain allows you to seperate portions of a website or API into a dedicated heirerarchy, for instance you could have specific subdomains for:

- Searching
- Login
- Blogs
- Helpdesk
- Specific API Versions

The default subdomain for any website is typically www (world wide web) and most websites will easily function with a single subdomain, using MVC and a domain name of mywebsite.com, the default uri would be

www.mywebsite.com/

The above uri would invoke the home controller and the same page as a uri of www.mywebsite.com/home (assuming a standard setup). Given the above examples we would have controllers for specific areas e.g.

www.mywebsite.com/Login
www.mywebsite.com/Blogs
www.mywebsite.com/Helpdesk
www.mywebsite.com/api/v2/

To reconfigure to use subdomains for the above you would instead have something similar to:

login.mywebsite.com/
blogs.mywebsite.com/
helpdesk.mywebsite.com/
apiv2.mywebsite.com/

This allows us to split the behaviour of a website into individual subdomains with specific areas of responsibility.