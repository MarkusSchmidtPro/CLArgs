# Value Conversion

A explained in Level 1, an [Option value is always of type *string*](../Level1/options.md) - the string as it was parsed from the command-line.

`CLarg` comes with out-of-the-box support for the following property types:

* string
* int
* DateTime
* Enum

If you wanna turn command-line arguments (which are strings) into other object you have to provide your own converter:

```csharp
enum FileType { XML, JSON }

private class CommandParameters
{
    [OptionDescriptor("DestType", Required = true)]
    public FileType FileType { get; set; }

    [OptionDescriptor("SourceFile", Required = false)]
    public FileInfo SourceFile { get; set; }
}
```

The `enum` is supported out of the box: `--DestType=JSON`.

However, the property type `FileInfo` requires your custom converter. In your Command constructor add the Type you want to convert to the `ValueConverters`dictionary and provide a function that takes two parameters to convert the value:

```csharp
public Command()
{
	this.ValueConverters.Add( typeof(FileInfo),
		(propertyName, optionValue) => new FileInfo(optionValue));
}
```

```Command-Line: --DestType=XML --SourceFile=NewFile1.Txt
Command-Line: --DestType=XML --SourceFile=NewFile1.Txt

>>> Start Main()
FileType: XML
FilePath: ....\bin\Debug\netcoreapp3.1\NewFile1.Txt
<<< End Main()
```

> TIP: `ValueConverters` is a dictionary with a Type as the key. You can not only add new converters you can also override the out-of-the-box implementation:
>
> ```csharp
> this.ValueConverters[typeof(int)] 
>     = ( propertyName, optionValue) => int.Parse(optionValue);
> ```


See [source-code](../../samples/Level2/ValueConversion/Program.cs) / [sample project](../../samples/Level2/ValueConversion) / [all samples](../../samples).