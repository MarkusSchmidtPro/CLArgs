# Custom Conversion

[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample05.ValueConversion/Program.cs) / [Sample-Project](https://github.com/msc4266/CLArgs/tree/master/samples/Sample05.ValueConversion)

The command-line is always text. Therefore, an *Option* value is always of type *string* - the string as it was provided in the command-line.

A *Command* requires *Parameters* - a typed object - and in between type / value conversion takes place. `CLarg` comes with out-of-the-box support for the following *Parameter* property types:

* string
* bool
* int
* DateTime
* Enum

If you wanna turn command-line arguments into other object types you have to provide your own converter. For example, the property type `FileInfo` requires your custom converter.

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