---
description: The main class that does all
---

# The Commander

The `Commander` is the main object within `ClArgs`. As you have seen [in the introduction](../) with a single line of code `Commander.ExecuteCommand( args)` _Commander_ will do: 

* parse the command-line
* resolve existing _Commands_
* transform command-line arguments \(strings\) into typed _Options_
* create a typed _Parameters_ object
* run the appropriate _Command_ \(based on the provided _Verb\(s\)\)_ 
* do the error-handling
* display help messages if requested

The [simplest way](https://github.com/msc4266/CLArgs/blob/master/samples/Sample01.SimpleAsThat/Program.cs) to use the `Commander` is by using its static `ExecuteCommand()` method. Alternatively, you may create your own instance  `new Commander(..`.`)`.

```csharp
public static void ExecuteCommand(string[] args, Settings settings = null) =>
    new Commander(settings).ExecuteCommand(CommandLineParser.Parse(args));
```

### Annotations

The `Commander` normally relies on Annotations, like`[Command]`, `[OptionDescriptor]` or `[OtionSet]`.

```csharp
[Command("HelloWorld", "This is the help text of my HelloWorld command.")]
internal class HelloWorldCommand : CommandBase<HelloWorldParameters>
{ }

class HelloWorldParameters
{ 
  [OptionDescriptor("country", "c", Required = true, 
    HelpText = "The country you're sending greetings to.")]
  public string Country { get; set; }

```

#### Command Annotation

Classes that implement a _Command_  must have this annotation. 

`CommandAttribute(string verb, string helpText = null)`

**Verb** - is mandatory and it is used to find the right Command. 

**HelpText** - is optional and it will be displayed whenever _help_ is requested



