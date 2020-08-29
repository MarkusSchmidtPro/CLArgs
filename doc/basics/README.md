---
description: About Verbs and Options
---

# Basics

In the command-line you can provide _Verbs_ and _Options_.

### Verbs and Commands

A _Verb_ is a word that determines _what_ your Console Application should do.

A _Verb_ is bound to a certain functionality: `YourApp.exe ConvertXml` ==&gt; `MyXmlConverter()`. This functionality is implemented as a _Command_.

```csharp
RegisterCommandFactory( "ConvertXml", () => new MyXmlConverter());
```

or use _Annotations_ \(Zero-Coding \[[Sample-Code](https://github.com/msc4266/CLArgs/tree/master/samples/Sample01.SimpleAsThat/Program.cs)\]\)

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

> NOTE: Verbs are completely optional. If your Console Application has only one single functionality, well, then use _CLArgs_ without Verbs:

```csharp
Arguments arguments = CommandLineParser.Parse(args);
var cmd = new MyXmlConverter();
cmd.Execute( arguments);
```

### Options

An _Option_ is a name-value tag that is parsed from the command-line:

`--filename=Input.xml --out='outDir\' --forceOverride`

Options are converted into _Parameters_:

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

### Verbs and Options

Each _Verb_ has its own set of _Options_, because each _Command_ requires its own _Parameters_:

```text
"ConvertXml" => XmlConverter( XmlConverterParameters ps);
"CreateSchema" => XsdSchemaGenerator( XsdSchemaGenerator ps);
```

### Automatic resolution

When you provide a Verb in the command-line, _CLArgs_ automatically resolved the related Command. _CLArgs_ then gets the _Command_ Parameter type and converts the command-line arguments into an instance of this type. Finally it executes the command. And all this with a single line of code: `Commander.ExecuteCommand(args);`

## What's Next

* [Custom Conversion](convertValues.md)
* [Dynamic Defaults](dynamicDefaultValues.md)

