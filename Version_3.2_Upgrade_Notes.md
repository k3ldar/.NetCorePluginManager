# Version 3.2 Upgrade

Sadly this version includes several changes, two of which are breaking changes.

## IPlugin relocated to new package

All plugins that implement IPlugin must reference PluginManager:

<PackageReference Include="PluginManager" Version="3.2.0" />

There are several other generic interfaces which have also relocated to the PluginManager module.

## Plugin Manager Abstractions

PluginManager specific interfaces are now within a different namespace and a new using clause is required:

using PluginManager.Abstractions;


## IPlugin Implements IPluginVersion

All implementations of IPlugin must implement the GetVersion() method:

        public ushort GetVersion()
        {
            return 1;
        }


## ILogger changes

AddToLog now has a new implementation which allows passing in the name of the module that is calling it


## LogLevel enums

AddToLog LogLevel enum is now located within PluginManager and has had plugin specific elements removed in favour of more generic members:

i.e. PluginManager.LogLevel.Error


## IPlugin Configure method

Configure(IApplicationBuilder app) method has been moved from IPlugin to IInitialiseEvents, this is a one time change which fits in with changing the underlying plugin manager to a generic architecture that can be used by more than just Net Core.



