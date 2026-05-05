# SimpleDB
## About

SimpleDB allows you to design your tables using standard C# classes, there are a number of methods for standard CRUD operations and support for the following features:

- Foreign Keys
- Unique indexes across multiple properties
- Before and after triggers for insert, update and delete
- Sequences for unique indexes
- Select Caching Strategy for individual classes (tables)
    - None - Records are read from storage as required.
    - Memory - Records are kept in memory.
    - Sliding - Records are retained in memory for n ms and when no longer used, memory is released.
- Select Write Strategy for individual classes (tables)
    - Forced - Records are saved immediately to storage
    - Lazy - Records are saved periodically to storage or after specific time.
- Compression types for saving data to storage
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

## SQL Server Backend

The `PluginManager.SimpleDB.SqlServer` package provides a drop-in SQL Server backend for SimpleDB.  All existing table row definitions and consumers that depend on `ISimpleDBOperations<T>` require **zero changes** — only the DI registration differs.

### Installation

Add a reference to the `PluginManager.SimpleDB.SqlServer` project or NuGet package alongside `PluginManager.SimpleDB`.

### Registering with SQL Server

Replace the `AddSimpleDB()` call with `AddSimpleDBSqlServer()`, passing a standard ADO.NET connection string:

```csharp
    // File backend (original)
    // services.AddSimpleDB(options => options.Path = "C:\\Data\\");

    // SQL Server backend
    services.AddSimpleDBSqlServer("Server=myserver;Database=mydb;Integrated Security=true;");

    // Table row registrations are unchanged
    services.AddSingleton(typeof(TableRowDefinition), typeof(SettingsDataRow));
```

The helper registers all required services (`ISimpleDBManager`, `IForeignKeyManager`, `DatabaseTimings`, and `ISimpleDBOperations<>`) automatically.

### Automatic Schema Management

When the application starts, `SqlServerDBOperations<T>` inspects each registered table row type using reflection and:

- **Creates** the SQL table if it does not exist, including a `[Id]` `BIGINT IDENTITY` primary key column.
- **Migrates** the table by adding any new columns that appear in the C# class but are missing from the database.  Existing columns and data are never removed or modified.
- **Creates** the `[dbo].[__sequences]` table used to manage unique index sequences.

No migration tool or manual DDL is required.

### .NET Property to SQL Column Type Mapping

| C# Type | SQL Server Type |
|---|---|
| `long` | `BIGINT` |
| `int` | `INT` |
| `bool` | `BIT` |
| `decimal` | `DECIMAL(18,4)` |
| `string` | `NVARCHAR(MAX)` or `NVARCHAR(N)` when `[MaxLength(N)]` is applied |
| `DateTime` | `DATETIME2` |
| `Guid` | `UNIQUEIDENTIFIER` |
| `enum` | `INT` |

Read-only properties (no public setter, or setter does not call `Update()`) are ignored, consistent with the file backend behaviour.

### Sequences

The `[UniqueIndex]` attribute is fully supported.  Sequences are stored in the `[dbo].[__sequences]` table and incremented atomically using `UPDATE … OUTPUT` so no duplicate keys are issued under concurrent load.

### Foreign Keys

`[ForeignKey]` constraints are enforced at the application layer by `ForeignKeyManager`, identical to the file backend.  No SQL `FOREIGN KEY` constraints are created in the database.

### Supported Features

| Feature | SQL Server backend |
|---|---|
| Foreign Keys | ✅ (application-enforced) |
| Unique Indexes | ✅ |
| Before / After triggers | ✅ |
| Sequences | ✅ |
| Caching Strategy | ✅ (`[Table]` attribute honoured) |
| Write Strategy | N/A – writes are immediate |
| Compression | N/A – handled by SQL Server |

## More Information
More information is available at https://www.pluginmanager.website/Docs/ or by visiting the GitHub Homepage https://github.com/k3ldar/.NetCorePluginManager
