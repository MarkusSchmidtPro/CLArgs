---
description: Command-Line options
---

# Command Parameters

Command-line _Options_ are turned into _Parameters._ Each _Command_ has its own set of parameters. 

`app.exe --LocalDateTime='2020-08-01 08:10:00' --LocalTimeZone='Pacific Standard Time'`

```csharp
[Command("ConvertToUtc")]
class ConvertToUtcCommand : CommandBase<ConvertToUtcParameters>
{}
```

> A good naming convention is, to name the _Command_ class _&lt;Verb&gt;Command_ and the _Parameters_ class _&lt;Verb&gt;Parameters_ as shown in the example above. Anyway, this is not mandatory.

A _Verb_ defines the _Command_ that is executed and _Parameters_ take the _Options_ which were provided in the command-line. 

```csharp
class ConvertToUtcParameters
{
  [OptionDescriptor("LocalDateTime", required:true, 
    helpText:"A local date and time that should be converted into UTC.")]
  public DateTime LocalDateTime { get; set; }

  [OptionDescriptor("LocalTimeZone", required:true, 
    helpText:"Specify the LocalDateTime's time zone")]
  public string LocalTimeZone { get; set; }
}
```

The _Commander_ automatically creates a new instance of the _Parameters_ class that belongs to the _Command_ which will be executed and passed it to the _Command's_ Execute method.

```csharp
class ConvertToUtcCommand : CommandBase<ConvertToUtcParameters>
{
  protected override void Execute( ConvertToUtcParameters ps)
  { }
}
```

### Complex Parameter classes

Parameter classes can inherit form base-classes \(see `BaseParameters)` and they can contain complex properties which itself are Parameter classes \(see `OptionSet: Connection`\). 

```csharp
class DBConvertParameters : BaseParameters
{
    [OptionSet]
    public Connection DbConnection { get; set; }
    
    [OptionDescriptor("DatabaseTableName", "t", Required = false)]
    public string DatabaseTableName { get; set; }
}

class BaseParameters
{
    [OptionDescriptor("BaseSetting", "bs", Required = true)]
    public bool BaseSetting { get; set; }
}

class Connection
{
    [OptionDescriptor("u", Required = true)]
    public string UserName { get; set; }

    [OptionDescriptor("p", Required = true)]
    public string Password { get; set; }

    [OptionDescriptor("d", Required = false)]
    public string Domain { get; set; }
}
```

This makes the _Parameters_ extremely powerful and flexible, especially if you want to support multiple _Verbs/Commands_ which may share the same options. 

