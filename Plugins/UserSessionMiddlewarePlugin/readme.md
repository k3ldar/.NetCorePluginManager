This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# User Session Middleware
The user session middleware plugin module has been designed to manage a user session whilst navigating through a webiste. At its core the UserSession class provides all the details for the user including:

- GeoIp Data.
- Pages Visited.
- Sales Data.
- Bot identification.
- User Agent.
- Culture Information.
- Initial Referrer

## GeoIp Data

If the GeoIp.Plugin or the SieraDeltaGeoIp.Plugin modules are loaded, when the session is created GeoIp data for the session will be loaded.

## IUserSessionService
The IUserSessionService provides methods for saving session data into a database or other data store, this is particularly useful in post analysis of user sessions. Please view Web Analytics and User Session Blog for more information on how to manipulate the user session data collected from user sessions. This could provide a multitude of reports including:

- Visits by Hour.
- Visits by Day.
- Visits by Week.
- Visits by Month.
- Location - City/Month.
- Sales - City Month.
- Page View by Month
- Bounced Visits.
- Bot Visits.
- Conversions.
- Conversions by Mobile.
- Referral Data
- Direct
- Organic
- Bing
- Google
- Yahoo
- Facebook
- Twitter

There are litterally dozens of reports that can be generated using the Session Data that can be saved.