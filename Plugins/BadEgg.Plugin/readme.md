This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Bad Egg Plugin
Nobody likes it when people don't play fair; the bad egg plugin is designed to complete several functions:

- Limit number of requests per minute.
- Determine the probability that a Sql injection attack is taking place.
- Determine the probability that a hack attempt is taking place.
- Allow for individual Ip addresses to be white/black listed.

- The important element to take into consideration is that Bad Egg does not make decisions on its own, in conjunction with IIpValidation which is implemented by the host application, reports are made through the interface which include:

- Ip Address.
- Query/Form data being submitted.
- Result of validation.
Given the information the host implementation can decide to ban the Ip address, if it wishes.

To prevent unnecessary delays in the request pipeline, Bad Egg middleware employs a background thread which is used to communicate with the IIpValidation implementation, this means there could be a slight delay in a ban being requested and actioned or a report being made for a request so as the host application can make decisions.
