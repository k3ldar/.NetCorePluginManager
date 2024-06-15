This package is one of many packages that can be used with [Plugin Manager](https://www.nuget.org/packages/PluginManager) which can be used to extend any c#/.net based application (MVC, Winform, WPF, MAUI etc) by using a [Modular Approach](https://pluginmanager.website/docs/Document/A-Modular-Approach/).

# Documentation Plugin
Documenting your code is the most fun aspect of software development, said no developer ever! But lets face it, you could create the fastest, richest API in the world, but if nobody know how to use it then where will it go? Nowhere is the answer. So documentation is important, not just for maintainability but to ensure that our hard earned endevours are used how they should be.

Since the beginning, the C# compiler has included an important, yet underused feature in the form of XML Documentation, this is a very simple yet powerful feature which allows developers to create inline documentation by adding specific XML tags to their code. This is often overlooked by developers because lets face it, if it was hard to write it should be hard to understand right? Wrong!

This feature not only allows other developers to understand what something is meant to do, with the right compiler directives it is also exported to an XML document that can be parsed and displayed, so all consumers of the code can easily understand how it is meant to be used, what a propery or parameter is used for etc etc. Essentially the generated XML file can be used as the basis for online documentation. An example of this feature can be seen below:
```

namespace DocumentationPlugin.Classes
{
    /// <summary>
    /// Settings which affect how the Documentation Plugin is configured.
    /// </summary>
    public sealed class DocumentationSettings
    {
        /// <summary>
        /// Default path where documentation files are located.
        /// 
        /// Default value: %AppPath%\\Plugins
        /// </summary>
        /// <value>string</value>
        [SettingDefault("%AppPath%\\Plugins")]
        public string Path { get; set; }
    }
}

```
There are many tools in and around the internet which can read and use the generated XML files, Documentation.Plugin is one such tool. Quite simply the documentation plugin has several views which allow this level of documentation to be rendered in an easy to read format. This in itself is useful but it doesn't finish the task at hand, to do this we must be able to customise the standard data with extra information that is useful and aids other developers. To achieve this their is an interface that is responsible for parsing the data, cross referencing various assemblies, namespaces, classes and other types which make up the documentaion.

The IDocumentationService interface exposes methods that allow XML documents to be parsed and seperated into a heirerarchical state that is displayed within a website. As well as this, the interface allows for extra data or information to be loaded from files which augment the automatically generated documentation. All additional data is read from files which contain a specific naming convention. If a file exists the data replaces the automatically generated data, meaning developers can expand their documentation easily.