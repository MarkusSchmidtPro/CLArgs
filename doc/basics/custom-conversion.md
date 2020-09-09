---
description: How command-line values (always string) are converted into typed properties
---

# Custom Conversion

[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample05.ValueConversion/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample05.ValueConversion)

The command-line is always text. Therefore an _Option_ value is always of type _string_ - the string as it was provided in the command-line.

A _Command_ requires _Parameters_ - a typed object - and in between type / value conversion takes place. `CLarg` comes with out-of-the-box support for the following _Parameter_ property types:

* string
* bool
* int
* DateTime
* Enum

If you want to turn command-line arguments into other \(user-defined\) object types you have to provide your own converter. For example, the property type `FileInfo` requires your custom converter.

```csharp
void Main(string[] args)
{
    var settings = new Settings();
    settings.ValueConverters.Register(typeof(FileInfo),
        // Custom Conversion: string to FileInfo
        (optionValue, optionName, errors, targetType) 
            => new FileInfo(optionValue));

    Commander.ExecuteCommand( args, settings));
}

...
[OptionDescriptor("DestType", Required = true)]
public FileTypes FileTypes { get; set; }    // requires custom converter
```

