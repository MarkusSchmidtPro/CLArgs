# How Commander uses Annotations

As you have read in [Command Resolution](../command-resolution/) the _Commander_ can automatically resolve Command implementation on certain places \(e.g. EntryAssembly or directory\). Auto resolution will scan for Command-Annotated classes and store them in a list of `CommandDesriptors`.

It is key to understand _Command_ resolution and execution is a two steps process:

1. Resolve all annotated classes
   1. store the results in the Commander's  `public List<CommandDescriptor> CommandDescriptors` property.
2. Find the verb-matching Command in the `CommandDescriptors` list 
   1. create the _Command_ instance
   2. run the Command's `Execute()`method 

Interesting to see, that you have the chance to register additional command-descriptors between step 1 and 2 \(see [Sample03.Options](https://github.com/msc4266/CLArgs/tree/master/samples/Sample03.Options/Program.cs)\).

A similar mechanism is used to resolve Options \(better `OptionDescriptors` and its Properties\) and you can add your own _Options_ without annotating properties `SetOption`. You can also modify or remove Options before the _Command_ is finally executed.

```csharp
Commander commander = new Commander(
  new Settings {IgnoreCase = true, AutoResolveCommands = true });
  
commander.RegisterCommand( 
  new CommandDescriptor( "TheOneAndOnlyCommand", () => new Command()));

// Let's change the argument from commands-line 
CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
commandLineArguments.SetOption("DatabaseTableName", "AnotherTable");

// Execute the Command with slightly different Options
commander.ExecuteCommand(commandLineArguments);
```

