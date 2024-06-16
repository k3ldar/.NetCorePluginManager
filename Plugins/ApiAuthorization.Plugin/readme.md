This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# API Authorization Plugin
There are many different ways in which you can secure an Api endpoint so that the resources are only used by those authorised to use them.

When writing open and accesible Api's that can be used across a public domain it is important to follow some basic principles, these being:

- Prioritize Security. Think about the type of security that you want to employ for endpoints at the start of a project, do not leave it as an afterthought, or believe that it's somebody elses issue.
- Use a strong authentication solution. Authenticating a user, using industry standard techniques can help protect data and ensure that only those requiring access to data are able to get access.
- Use authorization. Just because a user can authenticate, it does not mean the should have carte blanche once authentication is successfull. Using a principal of least privilege can help ensure that a user is not authorized to perform any action unless that action has been specifically granted.

## Implementing Api Authorization
Api authorization is accomplished by applying an attribute to the endpoint or controller for all endpoints and implementing two interfaces

ApiAuthorizationAttribute. An attribute that is put on each controller or individual action endpoint.
IApiAuthorizationService. The application must register an instance of IApiAuthorizationService within the service container. This interface contains one method and is used by the attribute to validate the request.
IUserApiQueryProvider. The application must register an instance of IUserApiQueryProvider within the service container.