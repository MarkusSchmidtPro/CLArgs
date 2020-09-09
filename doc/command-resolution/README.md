---
description: How to control the Command resolution by using Settings
---

# Command Resolution

In general, the Commander's behavior can be controlled by providing a `Settings` object. If no `Settings` object is provided, of course, default settings are applied.

```csharp
Commander.ExecuteCommand( 
    args, 
    new Settings { AutoResolveCommands = true, ... } 
);
```

See also: [How _Settings_ control command-line parsing](../command-line-parser/)

### Default resolution - EntryAssembly

There are two settings relevant for _Command_  resolution:`bool AutoResolveCommands` and `ICommandResolver CommandResolver`

`AutoResolveCommands` is `true`, by default, so that _Commander_ automatically tries to resolve _Command_ implementations.

`CommandResolver` is a `new AssemblyCommandResolver(` `Assembly.GetEntryAssembly())`, by default, which will find all `[Command()]` annotated classes in the `EntryAssembly`, only.

This is good enough for simple application, with a single EXE Assembly.

### AutoResolve in user-defined Assemblies 

If you application is more complex you may want to implement your Commands in separate Assemblies. So, you need to tell the `AssemblyCommandResolver` in which assemblies it should look for Commands.

```csharp
// Define the filepattern for Assemblies 
// which may contain Command implementations
const string SEARCH_PATTERN = "CLArgs.Command.*.dll";

// Get those filenames 
string[] assemblyFileNames 
  = Directory.GetFiles( 
      AppDomain.CurrentDomain.BaseDirectory, 
      SEARCH_PATTERN,
      SearchOption.AllDirectories);
      
      
Commander.ExecuteCommand(args, new Settings
{
    AutoResolveCommands = true,
    // ask the AssemblyCommandResolver to search 
    // in all provided AssemblyFileNames
    CommandResolver     
        = new AssemblyCommandResolver( assemblyFileNames)
});

```

See [CommandRunner skeleton](https://github.com/msc4266/CLArgs/tree/master/CommandRunner) how Assembly resolution may look like.

### User-defined resolution

Finally, you can provide your own `ICommandResolver` implementation to provide a more sophisticated _Command_ resolution, than simply searching Assemblies in the file-system. 

What about a _DatabaseResolver_ that reads Assemblies from a database, to look there for _Command_ implementations.  Or a _HttpResolver_ ... whatever you want.

### What's next?

Read about manual Command Resolution

