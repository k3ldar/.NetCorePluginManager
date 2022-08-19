# SimpleDB
## About

SimpleDB allows you to design your tables using standard C# classes, there are a number of methods for standard CRUD operations and support for the following features:

- Foreign Keys
- Unique indexes across multiple properties
- Before and after triggers for insert, update and select
- Sequences for unique indexes
- Select Caching Strategy for individual classes (tables)
    - None - Records are read from storage as required.
    - Memory - Records are kept in memory.
    - Sliding - Records are retained in memory for n ms and when no longer used, memory is released.
-Select Write Strategy for individual classes (tables)
    - Forced - Records are saved immediately to storage
    - Lazy - Records are saved periodically to storage or after specific time.
- Compression types for saving that is saved
    - None - Data is not compressed
    - Brotli - Data is compressed prior to saving

## How it works

The class represents a record (row) of data, which exposes properties for the data (columns), there is a TableAttribute that defines the policies for the table of data.  The class must descend from TableRowDefinition class in order to work.

Only properties which are public get/set are saved to storage, read only properties are not saved.  One important caveat is that the set method must call the Update() method which is defined in TableRowDefinition, this ensures that any changes are recognised when performing insert or update actions.

```csharp
    [Table("Settings", CompressionType.Brotli, CachingStrategy.None)]
    internal class SettingsDataRow : TableRowDefinition
    {
        string _name;
        string _value;

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name == value)
                    return;

                _name = value;
                Update();
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value == value)
                    return;

                _value = value;
                Update();
            }
        }
    }
```

## Registering Tables

SimpleDB has been designed with IoC in mind, tables can be registered and retrieved through DI engines.

```csharp
    services.AddSingleton(typeof(TableRowDefinition), typeof(SettingsDataRow));
```

## Using tables

```csharp
    internal sealed class SettingsProvider : IApplicationSettingsProvider
    {
        private readonly ISimpleDBOperations<SettingsDataRow> _settingsData;

        public SettingsProvider(ISimpleDBOperations<SettingsDataRow> settingsData)
        {
            _settingsData = settingsData ?? throw new ArgumentNullException(nameof(settingsData));
        }
    }
```

You can then call methods on _settingsData to perform normal CRUD operations.

## More Information
More information is available at https://www.pluginmanager.website/Docs/ or by visiting the GitHub Homepage https://github.com/k3ldar/.NetCorePluginManager

Please note the version number follows the parent project version
