This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Helpdesk Plugin
The Heldesk plugin is designed to add helpdesk facilities to any website in the form of:

- Frequently asked questions
- Support Tickets
- Feedback


To facilitate heldesk functionality the host application must implement IHelpdeskProvider interface, this exposes methods that provide data that enables the helpdesk to operate.

## Frequently Asked Questions
The purpose of the frequently asked questions (FAQ) is to provide information on questions or concerns that arise on a frequent basis. It is a useful mechanism for organizing information and data consisting of questions and their answers that users may have over time.

The FAQ section is organised into a series of folders and sub folders, each sub folder can contain further sub folders or FAQ items. This affords maximum flexibility when organising FAQ items into logical groups. There is also a mechanism built in to indicate how many times an FAQ item has been viewed, which further allows the items to be sorted by popularity.

## Support Tickets
Support tickets allow site owners to respond to specific user questions regarding their products or services. Users can choose a priority for their questions and the IHelpdeskProvider interface provides a method for specifying what departments are available for a ticket to be submitted to. Users can view existing tickets and respond.

When implementing the IHelpdeskProvider interface, consideration should be given to:

Providing an email receipt for the support question.
Provide an email notification of a response to the user.
Automatic integration with a specific email account for creating and responding to tickets.
## Feedback
The feedback section provides users with an area that allows feedback to be submitted and optionally displayed within the website for other site users to see. To help verify a user their is also Captcha verification.