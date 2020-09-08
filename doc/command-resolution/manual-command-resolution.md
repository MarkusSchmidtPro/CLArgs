---
description: AutoResolveCommands = false
---

# Manual Command Resolution

Manual Command resolution means going _low-level,_ not using the _Commander's_ benefits. 

`AutoCommandResolution` is based on _Parameters_ classes, `[Command()]` annotations and _Command_ classes that inherit from `CommandBase`. Auto-resolution requires you to use the CLArgs Framework. However, auto-resolution requires almost zero code.

Manual resolution requires a Commander instance first. There you can registers _verbs-to-functions._ When you then call `commander.Execute()`it will no longer resolve and look for a _Command_ implementation, it will just search the registered functions.

```csharp
private static void Main(string[] args)
{
    Commander commander = new Commander(new Settings
    {
        AutoResolveCommands = false
    });
    commander.RegisterFunction("word1", word);
    commander.RegisterFunction("word1.text2", text);
    commander.RegisterFunction("word1.text2.verb3", verb);
    
    CommandLineArguments commandLineArguments = CommandLineParser.Parse(args);
    commander.ExecuteCommand(commandLineArguments);
}

private static void word( CommandLineArguments commandLineArguments) 
    => Console.WriteLine("Function Word");
private static void text( CommandLineArguments commandLineArguments) 
    => Console.WriteLine("Function Text");
private static void verb( CommandLineArguments commandLineArguments) 
    => Console.WriteLine("Function Verb");
```

Please notice, there no _Options-to-Parameters_ conversion. Instead, the registered function receives a `CommandLineArguments` object as input. A `CommandLineArguments` contains all provided _Verbs, Options and Targets_ from command-line \(see [Sample2](https://github.com/msc4266/CLArgs/tree/master/samples/Sample02.Verbs)\).

### RegisterCommand

There is one last option how to resolve Commands: `Commander.RegisterCommand`. More details to be provided \(see also [Sample3](https://github.com/msc4266/CLArgs/tree/master/samples/Sample03.Options)\).

