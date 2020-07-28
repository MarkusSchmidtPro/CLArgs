# Basics

In the command-line you can provide *Verbs* and *Options*. 

## Verbs and Commands

A *Verb* is a word that determines *what* your Console Application should do. 

A *Verb* is bound to a certain functionality: `YourApp.exe ConvertXml` ==> `MyXmlConverter()`. This functionality is implemented as a *Command*. 

```csharp
RegisterCommandFactory( "ConvertXml", () => new MyXmlConverter());
```

 or use *Annotations* (Zero-Coding [[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample01.SimpleAsThat/Program.cs)])

```csharp
[Command("ConvertXml")]
class MyXmlConverter : CommandBase<XmlConverterParameters>
{
	protected override void Execute(XmlConverterParameters ps) 
    {
		// ....
	}
}
```

> NOTE: Verbs are completely optional. If your Console Application has only one single functionality, well, then use *CLArgs* without Verbs:

 ```csharp
Arguments arguments = CommandLineParser.Parse(args);
var cmd = new MyXmlConverter();
cmd.Execute( arguments);
 ```


## Options

An *Option* is a name-value tag that is parsed from the command-line: 

`--filename=Input.xml --out='outDir\' --forceOverride`

Options are converted into *Parameters*:

```csharp
class XmlConverterParameters
{
    [OptionDescriptor("filename", "f", Required = true)]
    public string Filename { get; set; }

    [OptionDescriptor("out", "o", Required = false, Default = "out")]
    public string OutDir  { get; set; }

    [OptionDescriptor("forceOverride", "fo", 
                      Required = false, Default = true)]
    public bool ForceOverride { get; set; }
}
```

## Verbs and Options

Each *Verb* has its own set of *Options*, because each *Command* requires its own *Parameters*:

```
"ConvertXml" => XmlConverter( XmlConverterParameters ps);
"CreateSchema" => XsdSchemaGenerator( XsdSchemaGenerator ps);
```

## Automatic resolution

When you provide a Verb in the command-line, *CLArgs* automatically resolved the related Command. *CLArgs* then gets the *Command* Parameter type and converts the command-line arguments into an instance of this type. Finally it executes the command. And all this with a single line of code: `Commander.ExecuteCommand(args);`

# What's Next
* [Custom Conversion](convertValues.md)
* [Dynamic Defaults](dynamicDefaultValues.md)